using System;
using System.IO;
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
using Broker.DataAccess;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using Broker.LoggingDataAccess;



public partial class Broker_LoggingReportPagesAndActions : AuthenticationPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
        {
            List<ViewDBAccessedPage> tempList = dc.ViewDBAccessedPages.Where(c => c.Title != string.Empty).ToList();

            gvDBActivitiesBase.DataSource = tempList;
            gvDBActivitiesBase.DataBind();
        }
    }

    protected void gvDBActivitiesBase_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //GridView tempgv = e.Row.FindControl("ctl00_ContentPlaceHolder1_gvDBActivities") as GridView;
        //using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
        //{
        //    ViewDBActivitiesBase tempbase = dc.ViewDBActivitiesBases.Where(c => c.UserID == Convert.ToInt32(e.Row.ID)).FirstOrDefault();
        //    List<ViewDBActivity> templist = dc.ViewDBActivities.Where(c => c.UserID == tempbase.UserID && c.SessionID == tempbase.SessionID).ToList();

        //    tempgv.DataSource = templist;
        //    tempgv.DataBind();
        //}
    }

    protected void gvDBActivitiesBase_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int id = Convert.ToInt32(gvDBActivitiesBase.DataKeys[e.Row.RowIndex].Value.ToString());
            GridView gvDBActivities = (GridView)e.Row.FindControl("gvDBActivities");

            using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
            {


                ViewDBAccessedPage tempbase = dc.ViewDBAccessedPages.Where(c => c.ID == id).FirstOrDefault();
                List<ViewDBAccessedPageAction> templist = dc.ViewDBAccessedPageActions.Where(c => c.DbAccesedPagesID == tempbase.ID).ToList();

                gvDBActivities.DataSource = templist;
                gvDBActivities.DataBind();
            }
            
        }
    }

    protected void gvDBActivities_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int id = (e.Row.DataItem as ViewDBAccessedPageAction).ID;
            GridView gvDBActivitiesUpdateFields = (GridView)e.Row.FindControl("gvDBActivitiesUpdateFields");

            string tempid = gvDBActivitiesUpdateFields.ID.ToString();

            using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
            {

                ViewDBAccessedPageAction tempbase = dc.ViewDBAccessedPageActions.Where(c => c.ID == id).FirstOrDefault();
                List<ViewDBUpdatedTable> templist = dc.ViewDBUpdatedTables.Where(c => c.DBAccessedPageActionID == tempbase.ID).ToList();

                gvDBActivitiesUpdateFields.DataSource = templist;
                gvDBActivitiesUpdateFields.DataBind();
            }

        }
    }

}
