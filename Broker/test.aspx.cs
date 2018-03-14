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

public class TableDetail
{

    public string Name { get; set; }

    public bool Select { get; set; }

    public bool Insert { get; set; }

    public bool Update { get; set; }

    public bool Delete { get; set; }

    public int Index { get; set; }

    public TableDetail()
    {

    }
    
}



public partial class Broker_test : AuthenticationPage
{
    enum Activities
    {
        INSERT = 1,
        DELETE = 2,
        SELECT = 3,
        UPDATE = 4
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(IsPostBack))
        {

            var model = new AttributeMappingSource().GetModel(typeof(DataClassesDataContext));

            List<TableDetail> tableDetails = new List<TableDetail>();

            List<MetaTable> temp = model.GetTables().ToList();

            for(int i =0; i < temp.Count; i++)
            {
                string tableName = temp[i].TableName;

                string prefix = tableName.Substring(0, 3);

                string tableWithoutPrefix = tableName.Substring(4);

                string tableNameInDB = "[" + prefix + "].[" + tableWithoutPrefix + "]";


                using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
                {
                    TableDetail newDetail = new TableDetail();

                    newDetail.Name = tableWithoutPrefix;

                    newDetail.Index = i;
                    newDetail.Insert = true;
                    newDetail.Delete = true;
                    newDetail.Select = true;
                    newDetail.Update = true;

                    DBLoggingIgnoredTable tempIgnoredTable = dc.DBLoggingIgnoredTables.Where(c => c.TableName == tableWithoutPrefix).FirstOrDefault();

                    if (tempIgnoredTable != null)
                    {

                        int ignoredTableID = tempIgnoredTable.ID;

                        foreach (DBLoggingIgnoredTableActivity item in dc.DBLoggingIgnoredTableActivities.Where(c => c.DbLoggingIgnoredTableID == ignoredTableID).ToList())
                        {
                            switch (item.ActivityTypeID)
                            {
                                case 1:
                                    {
                                        newDetail.Insert = false;
                                        break;
                                    }
                                case 2:
                                    {
                                        newDetail.Delete = false;
                                        break;
                                    }
                                case 3:
                                    {
                                        newDetail.Select = false;
                                        break;
                                    }
                                case 4:
                                    {
                                        newDetail.Update = false;
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                    }

                    tableDetails.Add(newDetail);
                }

            }


            gvTables.DataSource = tableDetails;
            gvTables.DataBind();
        }
    }



    protected void cbInsert_changed(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in gvTables.Rows)
        {
            CheckBox chk = (CheckBox)gvrow.FindControl("cbInsert");

            if (chk != null & chk.Checked)
            {

                string tableName = gvTables.Rows[gvrow.RowIndex].Cells[0].Text;
                using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
                {


                    DBLoggingIgnoredTable ignoredTable = dc.DBLoggingIgnoredTables.Where(c => c.TableName == tableName).FirstOrDefault();

                    if (ignoredTable != null)
                    {
                        foreach (DBLoggingIgnoredTableActivity ignoredActivity in ignoredTable.DBLoggingIgnoredTableActivities)
                        {
                            if (ignoredActivity.ActivityTypeID == (int)Activities.INSERT)
                            {
                                dc.DBLoggingIgnoredTableActivities.DeleteOnSubmit(ignoredActivity);
                                
                            }
                        }
                        dc.SubmitChanges();

                        if (ignoredTable.DBLoggingIgnoredTableActivities.Count == 0)
                        {
                            dc.DBLoggingIgnoredTables.DeleteOnSubmit(ignoredTable);
                            dc.SubmitChanges();
                        }
                    }
                }
            }

            if (chk != null & chk.Checked == false)
            {

                //string tableName = gvTables.DataKeys[gvrow.RowIndex].Values[0].ToString();
                string tableName = gvTables.Rows[gvrow.RowIndex].Cells[0].Text;
                using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
                {


                    DBLoggingIgnoredTable ignoredTable = dc.DBLoggingIgnoredTables.Where(c => c.TableName == tableName).FirstOrDefault();

                    if (ignoredTable != null)
                    {
                        bool exists = false;

                        foreach (DBLoggingIgnoredTableActivity ignoredActivity in ignoredTable.DBLoggingIgnoredTableActivities)
                        {
                            if (ignoredActivity.ActivityTypeID == (int)Activities.INSERT)
                            {
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            DBLoggingIgnoredTableActivity newIgnoredActivity = new DBLoggingIgnoredTableActivity();
                            newIgnoredActivity.ActivityTypeID = (int)Activities.INSERT;
                            newIgnoredActivity.DBLoggingIgnoredTable = ignoredTable;
                            dc.DBLoggingIgnoredTableActivities.InsertOnSubmit(newIgnoredActivity);
                            dc.SubmitChanges();
                        }

                    }
                    else
                    {
                        ignoredTable = new DBLoggingIgnoredTable();
                        ignoredTable.TableName = tableName;
                        dc.DBLoggingIgnoredTables.InsertOnSubmit(ignoredTable);

                        DBLoggingIgnoredTableActivity newIgnoredActivity = new DBLoggingIgnoredTableActivity();
                        newIgnoredActivity.ActivityTypeID = (int)Activities.INSERT;
                        newIgnoredActivity.DBLoggingIgnoredTable = ignoredTable;
                        dc.DBLoggingIgnoredTableActivities.InsertOnSubmit(newIgnoredActivity);
                        dc.SubmitChanges();
                    }
                }
            }

        }

    }

    protected void cbDelete_changed(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in gvTables.Rows)
        {
            CheckBox chk = (CheckBox)gvrow.FindControl("cbDelete");

            if (chk != null & chk.Checked)
            {

                string tableName = gvTables.Rows[gvrow.RowIndex].Cells[0].Text;
                using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
                {


                    DBLoggingIgnoredTable ignoredTable = dc.DBLoggingIgnoredTables.Where(c => c.TableName == tableName).FirstOrDefault();

                    if (ignoredTable != null)
                    {
                        foreach (DBLoggingIgnoredTableActivity ignoredActivity in ignoredTable.DBLoggingIgnoredTableActivities)
                        {
                            if (ignoredActivity.ActivityTypeID == (int)Activities.DELETE)
                            {
                                dc.DBLoggingIgnoredTableActivities.DeleteOnSubmit(ignoredActivity);

                            }
                        }
                        dc.SubmitChanges();

                        if (ignoredTable.DBLoggingIgnoredTableActivities.Count == 0)
                        {
                            dc.DBLoggingIgnoredTables.DeleteOnSubmit(ignoredTable);
                            dc.SubmitChanges();
                        }
                    }
                }
            }

            if (chk != null & chk.Checked == false)
            {

                //string tableName = gvTables.DataKeys[gvrow.RowIndex].Values[0].ToString();
                string tableName = gvTables.Rows[gvrow.RowIndex].Cells[0].Text;
                using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
                {


                    DBLoggingIgnoredTable ignoredTable = dc.DBLoggingIgnoredTables.Where(c => c.TableName == tableName).FirstOrDefault();

                    if (ignoredTable != null)
                    {
                        bool exists = false;

                        foreach (DBLoggingIgnoredTableActivity ignoredActivity in ignoredTable.DBLoggingIgnoredTableActivities)
                        {
                            if (ignoredActivity.ActivityTypeID == (int)Activities.DELETE)
                            {
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            DBLoggingIgnoredTableActivity newIgnoredActivity = new DBLoggingIgnoredTableActivity();
                            newIgnoredActivity.ActivityTypeID = (int)Activities.DELETE;
                            newIgnoredActivity.DBLoggingIgnoredTable = ignoredTable;
                            dc.DBLoggingIgnoredTableActivities.InsertOnSubmit(newIgnoredActivity);
                            dc.SubmitChanges();
                        }

                    }
                    else
                    {
                        ignoredTable = new DBLoggingIgnoredTable();
                        ignoredTable.TableName = tableName;
                        dc.DBLoggingIgnoredTables.InsertOnSubmit(ignoredTable);

                        DBLoggingIgnoredTableActivity newIgnoredActivity = new DBLoggingIgnoredTableActivity();
                        newIgnoredActivity.ActivityTypeID = (int)Activities.DELETE;
                        newIgnoredActivity.DBLoggingIgnoredTable = ignoredTable;
                        dc.DBLoggingIgnoredTableActivities.InsertOnSubmit(newIgnoredActivity);
                        dc.SubmitChanges();
                    }
                }
            }

        }

    }

    protected void cbSelect_changed(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in gvTables.Rows)
        {
            CheckBox chk = (CheckBox)gvrow.FindControl("cbSelect");

            if (chk != null & chk.Checked)
            {

                string tableName = gvTables.Rows[gvrow.RowIndex].Cells[0].Text;
                using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
                {


                    DBLoggingIgnoredTable ignoredTable = dc.DBLoggingIgnoredTables.Where(c => c.TableName == tableName).FirstOrDefault();

                    if (ignoredTable != null)
                    {
                        foreach (DBLoggingIgnoredTableActivity ignoredActivity in ignoredTable.DBLoggingIgnoredTableActivities)
                        {
                            if (ignoredActivity.ActivityTypeID == (int) Activities.SELECT)
                            {
                                dc.DBLoggingIgnoredTableActivities.DeleteOnSubmit(ignoredActivity);

                            }
                        }
                        dc.SubmitChanges();

                        if (ignoredTable.DBLoggingIgnoredTableActivities.Count == 0)
                        {
                            dc.DBLoggingIgnoredTables.DeleteOnSubmit(ignoredTable);
                            dc.SubmitChanges();
                        }
                    }
                }
            }

            if (chk != null & chk.Checked == false)
            {

                //string tableName = gvTables.DataKeys[gvrow.RowIndex].Values[0].ToString();
                string tableName = gvTables.Rows[gvrow.RowIndex].Cells[0].Text;
                using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
                {


                    DBLoggingIgnoredTable ignoredTable = dc.DBLoggingIgnoredTables.Where(c => c.TableName == tableName).FirstOrDefault();

                    if (ignoredTable != null)
                    {
                        bool exists = false;

                        foreach (DBLoggingIgnoredTableActivity ignoredActivity in ignoredTable.DBLoggingIgnoredTableActivities)
                        {
                            if (ignoredActivity.ActivityTypeID == (int)Activities.SELECT)
                            {
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            DBLoggingIgnoredTableActivity newIgnoredActivity = new DBLoggingIgnoredTableActivity();
                            newIgnoredActivity.ActivityTypeID = (int)Activities.SELECT;
                            newIgnoredActivity.DBLoggingIgnoredTable = ignoredTable;
                            dc.DBLoggingIgnoredTableActivities.InsertOnSubmit(newIgnoredActivity);
                            dc.SubmitChanges();
                        }

                    }
                    else
                    {
                        ignoredTable = new DBLoggingIgnoredTable();
                        ignoredTable.TableName = tableName;
                        dc.DBLoggingIgnoredTables.InsertOnSubmit(ignoredTable);

                        DBLoggingIgnoredTableActivity newIgnoredActivity = new DBLoggingIgnoredTableActivity();
                        newIgnoredActivity.ActivityTypeID = (int)Activities.SELECT;
                        newIgnoredActivity.DBLoggingIgnoredTable = ignoredTable;
                        dc.DBLoggingIgnoredTableActivities.InsertOnSubmit(newIgnoredActivity);
                        dc.SubmitChanges();
                    }
                }
            }

        }

    }

    protected void cbUpdate_changed(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in gvTables.Rows)
        {
            CheckBox chk = (CheckBox)gvrow.FindControl("cbUpdate");

            if (chk != null & chk.Checked)
            {

                string tableName = gvTables.Rows[gvrow.RowIndex].Cells[0].Text;
                using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
                {


                    DBLoggingIgnoredTable ignoredTable = dc.DBLoggingIgnoredTables.Where(c => c.TableName == tableName).FirstOrDefault();

                    if (ignoredTable != null)
                    {
                        foreach (DBLoggingIgnoredTableActivity ignoredActivity in ignoredTable.DBLoggingIgnoredTableActivities)
                        {
                            if (ignoredActivity.ActivityTypeID == (int) Activities.UPDATE)
                            {
                                dc.DBLoggingIgnoredTableActivities.DeleteOnSubmit(ignoredActivity);

                            }
                        }
                        dc.SubmitChanges();

                        if (ignoredTable.DBLoggingIgnoredTableActivities.Count == 0)
                        {
                            dc.DBLoggingIgnoredTables.DeleteOnSubmit(ignoredTable);
                            dc.SubmitChanges();
                        }
                    }
                }
            }

            if (chk != null & chk.Checked == false)
            {

                //string tableName = gvTables.DataKeys[gvrow.RowIndex].Values[0].ToString();
                string tableName = gvTables.Rows[gvrow.RowIndex].Cells[0].Text;
                using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
                {


                    DBLoggingIgnoredTable ignoredTable = dc.DBLoggingIgnoredTables.Where(c => c.TableName == tableName).FirstOrDefault();

                    if (ignoredTable != null)
                    {
                        bool exists = false;

                        foreach (DBLoggingIgnoredTableActivity ignoredActivity in ignoredTable.DBLoggingIgnoredTableActivities)
                        {
                            if (ignoredActivity.ActivityTypeID == (int)Activities.UPDATE)
                            {
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            DBLoggingIgnoredTableActivity newIgnoredActivity = new DBLoggingIgnoredTableActivity();
                            newIgnoredActivity.ActivityTypeID = (int)Activities.UPDATE;
                            newIgnoredActivity.DBLoggingIgnoredTable = ignoredTable;
                            dc.DBLoggingIgnoredTableActivities.InsertOnSubmit(newIgnoredActivity);
                            dc.SubmitChanges();
                        }

                    }
                    else
                    {
                        ignoredTable = new DBLoggingIgnoredTable();
                        ignoredTable.TableName = tableName;
                        dc.DBLoggingIgnoredTables.InsertOnSubmit(ignoredTable);

                        DBLoggingIgnoredTableActivity newIgnoredActivity = new DBLoggingIgnoredTableActivity();
                        newIgnoredActivity.ActivityTypeID = (int)Activities.UPDATE;
                        newIgnoredActivity.DBLoggingIgnoredTable = ignoredTable;
                        dc.DBLoggingIgnoredTableActivities.InsertOnSubmit(newIgnoredActivity);
                        dc.SubmitChanges();
                    }
                }
            }

        }

    }

    protected void btnSelectAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in gvTables.Rows)
        {
            CheckBox cbInsert = (CheckBox)gvrow.FindControl("cbInsert");
            CheckBox cbDelete = (CheckBox)gvrow.FindControl("cbDelete");
            CheckBox cbSelect = (CheckBox)gvrow.FindControl("cbSelect");
            CheckBox cbUpdate = (CheckBox)gvrow.FindControl("cbUpdate");

            if (cbInsert.Checked == false)
            {
                cbInsert.Checked = true;
            }
            if (cbDelete.Checked == false)
            {
                cbDelete.Checked = true;
            }
            if (cbSelect.Checked == false)
            {
                cbSelect.Checked = true;
            }
            if (cbUpdate.Checked == false)
            {
                cbUpdate.Checked = true;
            }

        }

        cbInsert_changed(gvTables, null);
        cbDelete_changed(gvTables, null);
        cbSelect_changed(gvTables, null);
        cbUpdate_changed(gvTables, null);

    }
    protected void btnDeSelectAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in gvTables.Rows)
        {
            CheckBox cbInsert = (CheckBox)gvrow.FindControl("cbInsert");
            CheckBox cbDelete = (CheckBox)gvrow.FindControl("cbDelete");
            CheckBox cbSelect = (CheckBox)gvrow.FindControl("cbSelect");
            CheckBox cbUpdate = (CheckBox)gvrow.FindControl("cbUpdate");

            if (cbInsert.Checked)
            {
                cbInsert.Checked = false;
            }
            if (cbDelete.Checked)
            {
                cbDelete.Checked = false;
            }
            if (cbSelect.Checked)
            {
                cbSelect.Checked = false;
            }
            if (cbUpdate.Checked)
            {
                cbUpdate.Checked = false;
            }
        }

        cbInsert_changed(gvTables, null);
        cbDelete_changed(gvTables, null);
        cbSelect_changed(gvTables, null);
        cbUpdate_changed(gvTables, null);
    }
}
