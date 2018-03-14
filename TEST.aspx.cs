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

public class ColumnName
{
    //public int Number { get; set; }
    public string Name { get; set; }
    public string NameMKD { get; set; }
    public string Description { get; set; }

    public ColumnName()
    {

    }

    
}


public partial class TEST : System.Web.UI.Page
{
    //public int TotalRecords { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var model = new AttributeMappingSource().GetModel(typeof(DataClassesDataContext));
            using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
            {

                string ColName = "";
                List<Broker.LoggingDataAccess.DBTranslatedColumnName> columnList = new List<Broker.LoggingDataAccess.DBTranslatedColumnName>();

                foreach (var mt in model.GetTables())
                {

                    foreach (var dm in mt.RowType.PersistentDataMembers)
                    {
                        if (dm.DbType != null)
                        {


                            ColName = dm.MappedName;


                            //bool contains = false;
                            //foreach (Broker.LoggingDataAccess.DBTranslatedColumnName item in columnList)
                            //{
                            //    if (item.Name == ColName)
                            //    {
                            //        contains = true;
                            //    }

                            //}

                            //if (!contains)
                            //{
                            if (dc.DBTranslatedColumnNames.Where(c => c.Name == ColName).FirstOrDefault() == null)
                            {

                                Broker.LoggingDataAccess.DBTranslatedColumnName newColumn = new Broker.LoggingDataAccess.DBTranslatedColumnName();
                                newColumn.Name = ColName;
                                newColumn.NameMKD = string.Empty;


                                dc.DBTranslatedColumnNames.InsertOnSubmit(newColumn);
                                dc.SubmitChanges();
                            }

                        }
                    }
                }


                //TotalRecords = dc.DBTranslatedColumnNames.Count();
                gvTables.DataSource = dc.DBTranslatedColumnNames;
                gvTables.DataBind();
                
            }
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
        {
            dc.ExecuteCommand("DELETE FROM DBTranslatedColumnNames");
            dc.SubmitChanges();

            foreach (GridViewRow gvr in this.gvTables.Rows)
            {
                string name = gvr.Cells[1].Text;
                string nameMKD = (gvr.FindControl("tbNameMKD") as TextBox).Text;
                string description = (gvr.FindControl("tbDescription") as TextBox).Text;


                Broker.LoggingDataAccess.DBTranslatedColumnName newColName = new DBTranslatedColumnName();
                newColName.Name = name;
                newColName.NameMKD = nameMKD;
                newColName.Description = description;
                dc.DBTranslatedColumnNames.InsertOnSubmit(newColName);
                dc.SubmitChanges();

            }
            gvTables.DataSource = dc.DBTranslatedColumnNames;
            gvTables.DataBind();
        }

        
    }
}