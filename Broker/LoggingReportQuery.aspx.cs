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

//public class TableDetail
//{

//    public string Name { get; set; }

//    public bool Select { get; set; }

//    public bool Insert { get; set; }

//    public bool Update { get; set; }

//    public bool Delete { get; set; }

//    public int Index { get; set; }

//    public TableDetail()
//    {

//    }
    
//}



public partial class Broker_LoggingReportQuery : AuthenticationPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
        {
            List<ViewDBActivitiesBase> tempList = dc.ViewDBActivitiesBases.ToList();

            gvDBActivitiesBase.DataSource = tempList;
            gvDBActivitiesBase.DataBind();
        }
    }


    protected void gvDBActivitiesBase_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int id = Convert.ToInt32(gvDBActivitiesBase.DataKeys[e.Row.RowIndex].Value.ToString());
            GridView gvDBActivities = (GridView)e.Row.FindControl("gvDBActivities");

            using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
            {


                ViewDBActivitiesBase tempbase = dc.ViewDBActivitiesBases.Where(c => c.ID == id).FirstOrDefault();
                List<ViewDBActivity> templist = dc.ViewDBActivities.Where(c => c.UserID == tempbase.UserID && c.SessionID == tempbase.SessionID && c.ActivityTypeID == 3).ToList();

                gvDBActivities.DataSource = templist;
                gvDBActivities.DataBind();
            }
            
        }
    }

    protected void gvDBActivities_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int id = (e.Row.DataItem as ViewDBActivity).ViewDBActivitiesID;
            GridView gvDBActivitiesUpdateFields = (GridView)e.Row.FindControl("gvDBActivitiesUpdateFields");

            string tempid = gvDBActivitiesUpdateFields.ID.ToString();

            using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
            {


                ViewDBActivity tempbase = dc.ViewDBActivities.Where(c => c.ViewDBActivitiesID == id).FirstOrDefault();
                List<ViewDBActivityParameter> templist = dc.ViewDBActivityParameters.Where(c => c.ID == tempbase.ViewDBActivitiesID).ToList();

                gvDBActivitiesUpdateFields.DataSource = templist;
                gvDBActivitiesUpdateFields.DataBind();
            }

        }
    }

}
