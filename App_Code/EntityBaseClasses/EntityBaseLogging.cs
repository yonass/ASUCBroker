using System;
using System.Web;
using System.Web.Caching;
using System.Configuration;
using System.Web.Configuration;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Threading;
using System.Globalization;
using ControlsLibriry.Utility;
using Broker.Controllers.ReportControllers;
using Microsoft.Office.Interop.Excel;
using MyClass.WriteToExcel;
using MyClass.WriteToWord;

/// <summary>
/// Provides the base class for all entities. Adds Select, Insert, Update, Get and
/// Delete methods to entities including support for data source paging, sorting, and caching
/// </summary>
/// 
namespace Broker.LoggingDataAccess 
{

    public abstract class EntityBaseLogging 
    {


    }


    public abstract class EntityBaseLogging<T> : EntityBaseLogging where T : EntityBaseLogging<T>, new() 
    {

        private const string FONTPATH = @"C:\Windows\Fonts\Arial.ttf";


        public static void ClearCache()
        {
            //string key = GetPrimaryCacheKey();
            IDictionaryEnumerator idenum = HttpContext.Current.Cache.GetEnumerator();
            while (idenum.MoveNext())
            {
                string key = idenum.Key.ToString();
                HttpContext.Current.Cache.Remove(key);
            }
        }

        #region Insert Methods


        public virtual void Insert() {
            Table.InsertOnSubmit((T)this);
            Context.SubmitChanges();
            ClearCache();

            if (this == null)
                throw new Exception(String.Format("Could not insert {0}", typeof(T)));
        }

        public static T Insert(T entityToInsert) {
            entityToInsert.Insert();
            return entityToInsert;
        }



        #endregion


        public static Table<T> Table {
            get {
                return Context.GetTable<T>();
            }
        }

        protected static DataContext Context {
            get {
                DataContext context = (DataContext)HttpContext.Current.Items["DataContext"];
                if (context == null) {
                    ConnectionStringSettings conSettings = WebConfigurationManager.ConnectionStrings["ASUCBrokerConnectionString"];
                    if (conSettings == null)
                        throw new Exception("Missing connection string named 'ASUCBrokerConnectionString' in web configuration.");
                    context = new DataContext(conSettings.ConnectionString);
                    HttpContext.Current.Items["DataContext"] = context;
                    context.Log = new TraceWriter();
                    //context.Log = new Logger();
                }
                return context;
            }
        }

    }
}