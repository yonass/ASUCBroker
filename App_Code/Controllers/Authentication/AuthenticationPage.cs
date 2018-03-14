using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Broker.Authentication;
using Broker.DataAccess;
using Broker.Controllers.Tree;
using System.Collections.Generic;
using NBOServerControls;


public partial class AuthenticationPage : System.Web.UI.Page {

    /// <summary>
    /// Event (nastan) koj se povikuva pri PreLoad na sekoja web-strana
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Page_PreLoad(object sender, EventArgs e) {
        // TODO: Da se zema od Session
        
        HashSet<int> usersSet = (HashSet<int>)HttpContext.Current.Application["usersSet"];
        HttpContext.Current.Application.UnLock();
        Broker.DataAccess.User user = this.PageUser;
        Broker.DataAccess.Branch branch = this.PageBranch;
        if (IsPostBack) {
            
            if (user == null) {                
                Response.Redirect(AuthenticationController.ERROR_PAGE + "?errMessage=" + AuthenticationController.SESSION_ERROR_MESSAGE);
                return;
            } 
        }
        
        if (user == null) {
            Response.Redirect(AuthenticationController.ERROR_PAGE + "?errMessage=" + AuthenticationController.SESSION_ERROR_MESSAGE);
            return;
        } 
        
        int userId = user.ID;
        string IpAddress = HttpContext.Current.Request.UserHostAddress;
        string relativeUrl = Request.ServerVariables["PATH_INFO"].Substring(1);
        if (!AuthenticationController.IsUserPrivileged(userId, relativeUrl)) {
            Response.Redirect(AuthenticationController.ERROR_PAGE + "?errMessage=" + AuthenticationController.ERROR_MESSAGE);
            //Response.Write("Error for "+userId + " "+ relativeUrl+"..redirecting...");
        }

        NBOTreeDataSource treeDataSource = Page.Master.FindControl("NBOTreeDataSource1") as NBOTreeDataSource;
        treeDataSource.UserID = userId;
        string sessionId = System.Web.HttpContext.Current.Session.SessionID;

        if (relativeUrl != "ASUCBroker/Broker/LoggingReport.aspx" && relativeUrl != "ASUCBroker/Broker/LoggingReportQuery.aspx" 
            && relativeUrl != "ASUCBroker/Broker/LoggingReportSelectQuery.aspx" && relativeUrl != "ASUCBroker/Broker/LoggingReportAction.aspx"
            && relativeUrl != "ASUCBroker/Broker/LoggingReportPagesAndActions.aspx")
        {
            using (Broker.LoggingDataAccess.LoggingDataContext dc = new Broker.LoggingDataAccess.LoggingDataContext())
            {

                if (!IsPostBack)
                {
                    
                    
                    Broker.LoggingDataAccess.DBAccessedPage acPage = new Broker.LoggingDataAccess.DBAccessedPage();
                    acPage.IpAddress = IpAddress;
                    acPage.UserID = userId;
                    acPage.TimeOfAccess = DateTime.Now;
                    acPage.SessionID = sessionId;
                    acPage.Title = this.Page.Title;
                    acPage.RelativeUrl = relativeUrl;

                    dc.DBAccessedPages.InsertOnSubmit(acPage);
                    dc.SubmitChanges();

                    Broker.LoggingDataAccess.DBAccessedPageAction newAction = new Broker.LoggingDataAccess.DBAccessedPageAction();

                    newAction.DBAccessedPage = acPage;
                    //newAction.ControlID = myControl.ID;
                    newAction.TimeAccessed = DateTime.Now;
                    newAction.Action = "Отворил Преглед";
                    //newAction.ControlNamingContainer = namingControler;
                    //newAction.ControlUniqueID = myControl.UniqueID;
                    newAction.RelativeUrl = relativeUrl;

                    dc.DBAccessedPageActions.InsertOnSubmit(newAction);
                    dc.SubmitChanges();
                }
                if (IsPostBack)
                {
                    System.Web.UI.Control myControl = GetPostBackControl(this);

                    

                    string tempTitle = myControl.Page.Title;

                    string tempID = string.Empty;

                    if (myControl != null)
                    {
                        string namingControler = string.Empty;
                        if (myControl.NamingContainer != null)
                        {
                            namingControler = myControl.NamingContainer.ID;
                        }
                        if (myControl.ID != null)
                        {

                            string actionText = "";
                            try
                            {
                                Button btn = (Button)myControl;
                                actionText = btn.Text;
                            }
                            catch { }

                            Broker.LoggingDataAccess.DBAccessedPageAction newAction= new Broker.LoggingDataAccess.DBAccessedPageAction();
                            Broker.LoggingDataAccess.DBAccessedPage acPage = dc.DBAccessedPages.Where(c => c.UserID == userId && c.SessionID == sessionId).OrderByDescending(c => c.ID).First();
                            newAction.DBAccessedPage = acPage;
                            newAction.ControlID = myControl.ID;
                            newAction.TimeAccessed = DateTime.Now;
                            if (actionText == string.Empty)
                            {
                                switch (myControl.ID)
                                {
                                    case "btnNew":
                                        {
                                            actionText = "Отворил форма за внес";
                                            break;
                                        }
                                    case "btnDelete":
                                        {
                                            actionText = "Отворил форма за бришење";
                                            break;
                                        }
                                    case "btnEdit":
                                        {
                                            actionText = "Посетил";

                                            

                                            //DetailsView tempControl = FindControl("ctl00$ContentPlaceHolder1$DetailsView1") as DetailsView;

                                            

                                            //if (tempControl != null)
                                            //{
                                            //    if (tempControl.SelectedValue != null)
                                            //    {
                                            //        tempID = tempControl.SelectedValue.ToString();
                                            //    }
                                            //}

                                            GXGridView tempGV = FindControl("ctl00$ContentPlaceHolder1$GXGridView1") as GXGridView;

                                            if (tempGV.SelectedValue != null)
                                            {
                                                tempID = tempGV.SelectedValue.ToString();
                                            }

                                            //if (Session["GXGridView1SelectedValue"] != null)
                                            //{
                                            //    tempID = Session["GXGridView1SelectedValue"].ToString();
                                            //}

                                            break;
                                        }
                                    case "btnReport":
                                        {
                                            actionText = "Отворил форма за извештај";
                                            break;
                                        }
                                    case "btnRepresentationDeal":
                                        {
                                            actionText = "Отворил форма за Договор - клиенти";
                                            break;
                                        }
                                    default:
                                        {
                                            actionText = "Друго";
                                            break;
                                        }
                                }
                            }
                            else
                            {
                                switch (actionText)
                                {
                                    case "Внеси":
                                        {
                                            actionText = "Внел/Креирал";
                                            break;
                                        }
                                    case "Измени":
                                        {
                                            actionText = "Изменил";
                                            break;
                                        }
                                    case "Избриши":
                                        {
                                            actionText = "Избришал";
                                            break;
                                        }
                                    case "Печати":
                                        {
                                            actionText = "Испечатил";
                                            break;
                                        }
                                    case "Нов запис":
                                        {
                                            if (myControl.ID == "btnNewAccreditation")
                                            {
                                                actionText = "Отворил форма за внес на нов договор";
                                            }
                                            else
                                            {
                                                actionText = "Запишал";
                                            }
                                            break;
                                        }
                                    case "Откажи":
                                        {
                                            actionText = "Откажал";
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }

                            newAction.Action = actionText;
                            newAction.ControlNamingContainer = namingControler;
                            newAction.ControlUniqueID = myControl.UniqueID;
                            newAction.RelativeUrl = relativeUrl;
                            newAction.OpenedFormID = tempID;

                            dc.DBAccessedPageActions.InsertOnSubmit(newAction);
                            dc.SubmitChanges();

                        }
                    }
                }
            }
        }
    }


    public static System.Web.UI.Control GetPostBackControl(Page page)
    {
        System.Web.UI.Control control = null;

        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                System.Web.UI.Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }

    /// <summary>
    /// Interfejs za User krenat na sesija
    /// </summary>
    public Broker.DataAccess.User PageUser
    {
        get {
            return (Broker.DataAccess.User)HttpContext.Current.Session["User"];
        }
        set {
            HttpContext.Current.Session["User"] = value;
        }
    }

    /// <summary>
    /// Interfejs za Branch kranata na sesija
    /// </summary>
    public Broker.DataAccess.Branch PageBranch
    {
        get {
            return (Broker.DataAccess.Branch)HttpContext.Current.Session["Branch"];
        }
        set {
            HttpContext.Current.Session["Branch"] = value;
        }
    }

    /// <summary>
    /// Interfejs za WebPage spored relativnoto URL na web-stranata
    /// </summary>
    public WebPage WebPage {
        get {
            string relativeUrl = Request.ServerVariables["PATH_INFO"].Substring(1);
            return WebPage.GetWebPageByUrl(relativeUrl);
        }
    }


}
