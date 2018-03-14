using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.Linq;
using Broker.LoggingDataAccess;

namespace Broker.DataAccess
{
    /// <summary>
    /// Summary description for Logger
    /// </summary>
    public class Logger : StreamWriter
    {
        public string Buffer { get; private set; } public int QueryCounter { get; private set; }

        public Logger()
            : base(new MemoryStream())
        { }

        public override void Write(string value)
        {
            //string tempid = HttpContext.Current.Session.SessionID;

            //int userid =Convert.ToInt32(HttpContext.Current.Session["UserID"]);

            //Buffer += value + "<br/><br/>";
            //if (!value.StartsWith("--")) QueryCounter++;
        }

        public override void WriteLine(string value)
        {
            

            //string temp = System.Text.RegularExpressions.Regex.Match(value, @"\[([^)]*)\]").Groups[1].Value;
            if (IsInIgnoreList(value))
            {
                return;
            }

            if (IgnoreFlagChecked(value))
            {
                return;
            }


            string sessionID = HttpContext.Current.Session.SessionID;
            int userID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

            using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
            {
                if (userID > 0)
                {
                    int activityTypeID = 0;
                    int dBUpdatedTableID = 0;

                    int start = 0;
                    var match2 = string.Empty;

                    if (value.StartsWith("SELECT"))
                    {
                        if (value.StartsWith("SELECT COUNT"))
                        {
                            return;
                        }
                        activityTypeID = 3;
                        start = value.IndexOf("FROM") + 4;
                        match2 = value.Substring(start, value.IndexOf("AS") - start);
                    }
                    else
                    {
                        if (value.StartsWith("INSERT"))
                        {
                            activityTypeID = 1;
                            dBUpdatedTableID = dc.DBUpdatedTables.Where(c => c.UserID == userID && c.SessionID == sessionID).OrderByDescending(x => x.TimeOfUpdate).FirstOrDefault().ID;
                            start = value.IndexOf("INTO") + 4;
                            match2 = value.Substring(start, value.IndexOf("(") - start);
                        }
                        else
                        {
                            if (value.StartsWith("DELETE"))
                            {
                                activityTypeID = 2;
                                dBUpdatedTableID = dc.DBUpdatedTables.Where(c => c.UserID == userID && c.SessionID == sessionID).OrderByDescending(x => x.TimeOfUpdate).FirstOrDefault().ID;
                                start = value.IndexOf("FROM") + 4;
                                match2 = value.Substring(start, value.IndexOf("AS") - start);
                            }
                            else
                            {
                                if (value.StartsWith("UPDATE"))
                                {
                                    activityTypeID = 4;
                                    
                                    dBUpdatedTableID = dc.DBUpdatedTables.Where(c => c.UserID == userID && c.SessionID == sessionID).OrderByDescending(x => x.TimeOfUpdate).FirstOrDefault().ID;
                                    start = value.IndexOf("FROM") + 4;
                                    match2 = dc.DBUpdatedTables.Where(c => c.UserID == userID && c.SessionID == sessionID).OrderByDescending(x => x.TimeOfUpdate).FirstOrDefault().TableName;
                                }
                                else
                                {
                                    if (value.StartsWith("-- @p"))
                                    {
                                        activityTypeID = 5;
                                        //start = string.Empty;
                                        //match2 = string.Empty;
                                    }
                                }
                            }
                        }
                    }


                    if (activityTypeID > 0)
                    {
                        if (activityTypeID < 5)
                        {
                            DBActivity activity = new DBActivity();

                            activity.ActivityTypeID = activityTypeID;
                            activity.Query = value;
                            activity.UserID = userID;
                            activity.SessionID = sessionID;
                            activity.Date = DateTime.Now;
                            activity.TableName = match2.ToString();
                            if (dBUpdatedTableID > 0)
                            {
                                activity.DBUpdatedTableID = dBUpdatedTableID;
                            }

                            try
                            {

                                dc.DBActivities.InsertOnSubmit(activity);
                                dc.SubmitChanges();

                            }
                            catch { }
                        }
                        else
                        {
                            if (activityTypeID == 5)
                            {
                                DBActivityParameter activityParameter = new DBActivityParameter();
                                DBActivity tempActivity = dc.DBActivities.Where(c => c.UserID == userID && c.SessionID == sessionID).OrderByDescending(c => c.Date).FirstOrDefault();
                                //DBActivity tempActivity = DBActivity.Table.Where(c => c.UserID == userID && c.SessionID == sessionID).OrderByDescending(c => c.Date).FirstOrDefault();
                                if (tempActivity != null)
                                {
                                    int activityID = tempActivity.ID;

                                    string substring = value.Substring(3);
                                    string parameterName = substring.Split(':').First();
                                    int startIndex = value.IndexOf("[") + 1;
                                    string parameterValue = value.Substring(startIndex, value.IndexOf("]") - startIndex);

                                    int startIndex2 = value.IndexOf("Input") + 6;
                                    string parameterType = value.Substring(startIndex2, (value.IndexOf("(") - 1) - startIndex2);

                                    string tempQuery = tempActivity.Query;
                                    if (parameterType.Contains("nvarchar"))
                                    {
                                        tempQuery = tempQuery.Replace(parameterName, "'" + parameterValue + "'");
                                    }
                                    else
                                    {
                                        tempQuery = tempQuery.Replace(parameterName, parameterValue);
                                    }

                                    tempActivity.Query = tempQuery;
                                    dc.SubmitChanges();

                                    activityParameter.DBActivityID = activityID;
                                    activityParameter.Value = value;

                                    dc.DBActivityParameters.InsertOnSubmit(activityParameter);
                                    dc.SubmitChanges();
                                }


                            }
                        }
                    }
                }

            }
        }

        private bool IsInIgnoreList(string value)
        {
            using (var dc = new LoggingDataContext())
            {
                List<DBLoggingIgnoredTable> ignoreList = dc.DBLoggingIgnoredTables.ToList();


                foreach (DBLoggingIgnoredTable ignoredTable in ignoreList)
                {
                    if (value.Contains(ignoredTable.TableName))
                    {
                        DBLoggingControl tempControl = dc.DBLoggingControls.FirstOrDefault();
                        tempControl.IgnoreNext = true;
                        dc.SubmitChanges();

                        return true;
                    }
                }
            }
            return false;
        }

        private bool IgnoreFlagChecked(string value)
        {
            using (Broker.LoggingDataAccess.LoggingDataContext dc = new LoggingDataContext())
            {
                DBLoggingControl tempControl = dc.DBLoggingControls.FirstOrDefault();
                //bool IsIgnored = dc.DBLoggingControls.FirstOrDefault().IgnoreNext;

                if (tempControl.IgnoreNext)
                {
                    if (value.StartsWith("-- Context"))
                    {
                        tempControl.IgnoreNext = false;
                        dc.SubmitChanges();
                    }

                    return true;
                }

                return false;
            }

        }
    }
}