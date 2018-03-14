using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Broker.DataAccess
{
    
    partial class DataClassesDataContext
    {
        private bool IsInIgnoreList(string value)
        {

            using (Broker.LoggingDataAccess.LoggingDataContext dc = new Broker.LoggingDataAccess.LoggingDataContext())
            {


                foreach (Broker.LoggingDataAccess.DBLoggingIgnoredTable ignoredTable in dc.DBLoggingIgnoredTables)
                {
                    
                    string temp2 = value.Split('.').Last();

                    if (ignoredTable.TableName.Contains(temp2))
                    {

                        return true;
                    }
                }
            }

            return false;
        }

        public override void SubmitChanges(ConflictMode failureMode)
        {
            
            var delta = GetChangeSet();

            object services = this.GetType().BaseType.GetField("services", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField).GetValue(this);

            object tracker = services.GetType().GetField("tracker", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField).GetValue(services);

            System.Collections.IDictionary trackerItems = (System.Collections.IDictionary)tracker.GetType().GetField("items", BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.GetField).GetValue(tracker);

            foreach (System.Collections.DictionaryEntry entry in trackerItems)
            {

                EntityBase original = (EntityBase)entry.Value.GetType().GetField("original",
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.GetField).GetValue(entry.Value);


                EntityBase modified = (EntityBase)entry.Key;
                
                object asdasd = modified.GetType();

                if (!(IsInIgnoreList(modified.GetType().ToString())))
                {

                    var columnNames = Mapping.MappingSource
                    .GetModel(typeof(Broker.DataAccess.DataClassesDataContext))
                    .GetMetaType(modified.GetType())
                    .PersistentDataMembers;

                    var tempasas = Mapping.MappingSource
                    .GetModel(typeof(Broker.DataAccess.DataClassesDataContext))
                    .GetMetaType(modified.GetType()).Table.TableName.Last();

                    using (Broker.LoggingDataAccess.LoggingDataContext dc = new Broker.LoggingDataAccess.LoggingDataContext())
                    {

                        string sessionID = HttpContext.Current.Session.SessionID;
                        int userID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

                        Broker.LoggingDataAccess.DBUpdatedTable updatedTable = new LoggingDataAccess.DBUpdatedTable();
                        
                        updatedTable.TableName = modified.GetType().ToString();
                        updatedTable.UserID = userID;
                        updatedTable.SessionID = sessionID;
                        updatedTable.TimeOfUpdate = DateTime.Now;

                        Broker.LoggingDataAccess.DBAccessedPage tempPage = dc.DBAccessedPages.Where(c => c.UserID == userID && c.SessionID == sessionID).OrderByDescending(x => x.ID).First();
                        updatedTable.DBAccessedPageAction = dc.DBAccessedPageActions.Where(c => c.DbAccesedPagesID == tempPage.ID).OrderByDescending(x => x.ID).First();

                        dc.DBUpdatedTables.InsertOnSubmit(updatedTable);
                        dc.SubmitChanges();

                        for (int i = 0; i < columnNames.Count; i++)
                        {

                            if (original == null)
                            {
                                //PropertyInfo prOld = original.GetType().GetProperty(columnNames[i].Name);

                                PropertyInfo prNew = modified.GetType().GetProperty(columnNames[i].Name);

                                Type t = prNew.PropertyType;

                                if ((t.IsValueType || t == typeof(string)) && (prNew.CanRead) && (prNew.CanWrite))
                                {

                                    string value = string.Empty;
                                    try
                                    {
                                        value = prNew.GetValue(modified, null).ToString();
                                    }
                                    catch
                                    {

                                    }

                                    string oldValue = string.Empty;
                                    string tableName = updatedTable.TableName.Split('.').Last() + "s";
                                    var results = dc.ExecuteQuery<int>(
                                   @"SELECT ID FROM " + tableName + " ORDER BY ID DESC");
                                    int lastID = results.First();
                                    Broker.LoggingDataAccess.DBUpdatedTableField updatedField = new LoggingDataAccess.DBUpdatedTableField();
                                    updatedField.DBUpdatedTable = updatedTable;
                                    updatedField.FieldName = columnNames[i].Name;
                                    if (updatedField.FieldName == "ID" && value == "0")
                                    {
                                        updatedField.Value = (lastID + 1).ToString();
                                    }
                                    else
                                    {
                                        updatedField.Value = value;
                                    }
                                    updatedField.OldValue = oldValue;


                                    dc.SubmitChanges();
                                }
                            }
                            else
                            {
                                PropertyInfo prOld = original.GetType().GetProperty(columnNames[i].Name);
                                PropertyInfo prNew = modified.GetType().GetProperty(columnNames[i].Name);

                                Type t = prNew.PropertyType;

                                if ((t.IsValueType || t == typeof(string)) && (prNew.CanRead) && (prNew.CanWrite))
                                {

                                    string value = string.Empty;
                                    string oldValue = string.Empty;

                                    try
                                    {
                                        value = prNew.GetValue(modified, null).ToString();
                                    }
                                    catch { }

                                    try
                                    {
                                        oldValue = prOld.GetValue(original, null).ToString();
                                    }
                                    catch { }

                                    Broker.LoggingDataAccess.DBUpdatedTableField updatedField = new LoggingDataAccess.DBUpdatedTableField();
                                    updatedField.DBUpdatedTable = updatedTable;
                                    updatedField.FieldName = columnNames[i].Name;
                                    updatedField.Value = value;
                                    updatedField.OldValue = oldValue;


                                    dc.SubmitChanges();
                                }
                            }
                        }


                    }

                }

            }

            base.SubmitChanges(failureMode);
        }
        
    }
}
