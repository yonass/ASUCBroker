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
//using ASUC.Utility;
using System.Collections;
using System.Data;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Threading;
using System.Globalization;
//using ASUC.Controllers.ReportControllers;
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
namespace Broker.DataAccess {

    public abstract class EntityBase {

        private ValidationErrorCollection _ValidationErrors;


        public ValidationErrorCollection ValidationErrors {
            get {
                if (_ValidationErrors == null)
                    _ValidationErrors = new ValidationErrorCollection();
                return _ValidationErrors;
            }
            set {
                if (_ValidationErrors == null)
                    _ValidationErrors = new ValidationErrorCollection();
                _ValidationErrors = value;
            }
        }



        public bool IsValid {
            get {
                return (ValidationErrors.Count == 0);
            }
        }

        public void PrintErrors() {
            foreach (String param in ValidationErrors.Keys) {
                ValidationError error = ValidationErrors[param];
                HttpContext.Current.Response.Write(param + " : " + error.ErrorMessage + "<br>");
            }
        }


        protected void PerformValidation() {
            Validate();

            if (!IsValid)
                throw new ValidationException(this.ToString() + " failed validation", this);

        }

        public void PerformCustomValidation() {
            if (!IsValid)
                throw new ValidationException(this.ToString() + " failed validation", this);

        }


        //protected abstract void Validate();
        public virtual void Validate() {

        }
    }


    public abstract class EntityBase<T> : EntityBase where T : EntityBase<T>, new() {



        private const string FONTPATH = @"C:\Windows\Fonts\Arial.ttf";

        #region Select Methods

        /// <summary>
        /// Returns unsorted list of entity
        /// </summary>
        public static IQueryable<T> Select() {
            return Table;
        }

        /// <summary>
        /// Returns cached unsorted list of entity
        /// </summary>
        public static IEnumerable<T> SelectCached() {
            string key = GetCacheKey("SelectCached");
            List<T> results = (List<T>)HttpContext.Current.Cache[key];
            if (results == null) {
                results = Select().ToList();
                AddCache(key, results);
                HttpContext.Current.Trace.Warn("Retrieving " + typeof(T) + " records from database");
            }
            return results;
        }


        /// <summary>
        /// Returns sorted list of entity
        /// </summary>
        /// <param name="orderBy">Name of property to order by</param>
        public static IQueryable<T> Select(string orderBy) {
            if (String.IsNullOrEmpty(orderBy))
                return Select();

            return SelectDynamicSort(orderBy);
        }


        /// <summary>
        /// Returns unsorted list of paged entities
        /// </summary>
        /// <param name="startRowIndex">The starting database row</param>
        /// <param name="maximumRows">The number of rows to return</param>
        public static IQueryable<T> Select(int startRowIndex, int maximumRows) {
            if (startRowIndex == -1)
                return null;

            return Select().Skip(startRowIndex).Take(maximumRows);
        }

        /// <summary>
        /// Returns sorted list of paged entities
        /// </summary>
        /// <param name="startRowIndex">The starting database row</param>
        /// <param name="maximumRows">The number of rows to return</param>
        /// <param name="orderBy">The property to order by</param>
        public static IQueryable<T> Select(int startRowIndex, int maximumRows, string orderBy) {
            if (startRowIndex == -1)
                return null;

            if (String.IsNullOrEmpty(orderBy))
                return Select(startRowIndex, maximumRows);

            return SelectDynamicSort(orderBy).Skip(startRowIndex).Take(maximumRows);
        }



        #endregion

        #region Select Foreign Key

        public static IQueryable<T> SelectByFK(string foreignKeyName, int id, string orderBy, int startRowIndex, int maximumRows) {
            if (startRowIndex == -1)
                return null;

            return SelectDynamicByFK(foreignKeyName, id, orderBy).Skip(startRowIndex).Take(maximumRows);

        }
        public static IQueryable<T> SelectDynamicByFK(string foreignKeyName, int id, string orderBy) {
            ParameterExpression e = Expression.Parameter(typeof(T), "e");
            PropertyInfo propInfo = typeof(T).GetProperty(foreignKeyName);
            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(id, typeof(int));
            BinaryExpression b = Expression.Equal(m, c);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(b, e);
            return Select(orderBy).Where(lambda);
        }

        #endregion

        #region Sort Methods

        /// <summary>
        /// Takes an expression like AuthorName DESC and breaks it
        /// into AuthorName and DESC
        /// </summary>
        /// <param name="orderBy">The full order by expression</param>
        /// <param name="orderByColumn">The name of the order by column</param>
        /// <param name="orderByDirection">The direction of the sort (asc or desc)</param>
        private static void ParseOrderBy(string orderBy, ref string orderByColumn, ref string orderByDirection) {
            string[] orderByParts = orderBy.Split(' ');
            orderByColumn = orderByParts[0];
            if (orderByParts.Length > 1)
                orderByDirection = orderByParts[1].ToLower();
        }

        private static bool IsNullableType(Type theType) {
            return (theType.IsGenericType && theType.
              GetGenericTypeDefinition().Equals
              (typeof(Nullable<>)));
        }
        /// <summary>
        /// Used to perform typed and dynamic order by.
        /// </summary>
        /// <param name="orderBy">The full order by expression, e.g. AuthorName DESC</param>
        private static IQueryable<T> SelectDynamicSort(string orderBy) {
            // Parse order by            
            string orderByColumn = String.Empty;
            string orderByDirection = "asc";
            ParseOrderBy(orderBy, ref orderByColumn, ref orderByDirection);

            // Get sort results

            PropertyInfo prop = typeof(T).GetProperty(orderByColumn);
            Type myType;
            bool nullable = false;
            if (IsNullableType(prop.PropertyType)) {
                NullableConverter nc = new NullableConverter(prop.PropertyType);
                myType = nc.UnderlyingType;
                nullable = true;
            } else {
                myType = prop.PropertyType;
            }

            if (!nullable) {
                switch (Type.GetTypeCode(myType)) {
                    case TypeCode.Boolean:
                        return GetDynamicSortResults<bool>(orderByColumn, orderByDirection);
                    case TypeCode.Byte:
                        return GetDynamicSortResults<byte>(orderByColumn, orderByDirection);
                    case TypeCode.Char:
                        return GetDynamicSortResults<char>(orderByColumn, orderByDirection);
                    case TypeCode.DateTime:
                        return GetDynamicSortResults<DateTime>(orderByColumn, orderByDirection);
                    case TypeCode.Decimal:
                        return GetDynamicSortResults<Decimal>(orderByColumn, orderByDirection);
                    case TypeCode.Double:
                        return GetDynamicSortResults<double>(orderByColumn, orderByDirection);
                    case TypeCode.Int16:
                        return GetDynamicSortResults<Int16>(orderByColumn, orderByDirection);
                    case TypeCode.Int32:
                        return GetDynamicSortResults<Int32>(orderByColumn, orderByDirection);
                    case TypeCode.Int64:
                        return GetDynamicSortResults<Int64>(orderByColumn, orderByDirection);
                    case TypeCode.Single:
                        return GetDynamicSortResults<Single>(orderByColumn, orderByDirection);
                    case TypeCode.String:
                        return GetDynamicSortResults<String>(orderByColumn, orderByDirection);
                    default:
                        throw new Exception("Cannot sort column " + orderByColumn + " because of its type");
                }
            } else {
                switch (Type.GetTypeCode(myType)) {
                    case TypeCode.Boolean:
                        return GetDynamicSortResults<System.Nullable<bool>>(orderByColumn, orderByDirection);
                    case TypeCode.Byte:
                        return GetDynamicSortResults<System.Nullable<byte>>(orderByColumn, orderByDirection);
                    case TypeCode.Char:
                        return GetDynamicSortResults<System.Nullable<char>>(orderByColumn, orderByDirection);
                    case TypeCode.DateTime:
                        return GetDynamicSortResults<System.Nullable<DateTime>>(orderByColumn, orderByDirection);
                    case TypeCode.Decimal:
                        return GetDynamicSortResults<System.Nullable<Decimal>>(orderByColumn, orderByDirection);
                    case TypeCode.Double:
                        return GetDynamicSortResults<System.Nullable<double>>(orderByColumn, orderByDirection);
                    case TypeCode.Int16:
                        return GetDynamicSortResults<System.Nullable<Int16>>(orderByColumn, orderByDirection);
                    case TypeCode.Int32:
                        return GetDynamicSortResults<System.Nullable<Int32>>(orderByColumn, orderByDirection);
                    case TypeCode.Int64:
                        return GetDynamicSortResults<System.Nullable<Int64>>(orderByColumn, orderByDirection);
                    case TypeCode.Single:
                        return GetDynamicSortResults<System.Nullable<Single>>(orderByColumn, orderByDirection);
                    case TypeCode.String:
                        return GetDynamicSortResults<String>(orderByColumn, orderByDirection);
                    default:
                        throw new Exception("Cannot sort column " + orderByColumn + " because of its type");
                }
            }
        }


        /// <summary>
        /// Dynamically builds a Linq expression that sorts by a particular column
        /// </summary>
        /// <typeparam name="S">The return type of the Lambda</typeparam>
        /// <param name="orderByColumn">The name of the order by column</param>
        /// <param name="orderByDirection">The direction of the order by sort</param>
        private static IQueryable<T> GetDynamicSortResults<S>(string orderByColumn, string orderByDirection) {
            ParameterExpression sortParameterExpression = Expression.Parameter(typeof(T), "e");
            PropertyInfo propInfo = typeof(T).GetProperty(orderByColumn);

            MemberExpression sortPropertyExpression = Expression.MakeMemberAccess(sortParameterExpression, propInfo);

            Expression<Func<T, S>> sortExpression = Expression.Lambda<Func<T, S>>(sortPropertyExpression, sortParameterExpression);

            if (orderByDirection == "asc")
                return Table.OrderBy(sortExpression);
            else
                return Table.OrderByDescending(sortExpression);
        }


        #endregion



        #region Select Count Methods


        /// <summary>
        /// Returns count of the records in a database
        /// </summary>
        public static int SelectCount() {
            return Table.Count();
        }

        /// <summary>
        /// Returns count of the records in a database 
        /// </summary>
        /// <param name="orderBy">The order by </param>
        public static int SelectCount(string orderBy) {
            return SelectCount();
        }

        public static int SelectFilterCount(FilterArgument fArgument) {
            string orderBy = "";
            return SelectDynamicFilter(fArgument, orderBy).Count();
        }

        public static int SelectSearchCount(SearchArgument sArgument) {
            string orderBy = "";
            return SelectDynamicSearch(sArgument, orderBy).Count();
        }

        public static int SelectByFKCount(string foreignKeyName, int id) {
            string orderBy = "";
            return SelectDynamicByFK(foreignKeyName, id, orderBy).Count();
        }

        public static int SelectFilterByFKCount(string foreignKeyName, int id, FilterArgument fArgument) {
            string orderBy = "";
            return SelectDynamicFilterByFK(foreignKeyName, id, fArgument, orderBy).Count();
        }

        public static int SelectSearchByFKCount(string foreignKeyName, int id, SearchArgument sArgument) {
            string orderBy = "";
            return SelectDynamicSearchByFK(foreignKeyName, id, sArgument, orderBy).Count();
        }
        /// <summary>
        /// Returns count of the records in a database, caching the
        /// result.
        /// </summary>
        public static int SelectCountCached() {
            string key = GetCacheKey("Count");
            int? count = (int?)HttpContext.Current.Cache[key];
            if (count == null) {
                count = SelectCount();
                AddCache(key, count);
            }
            return count.Value;
        }
        public static int SelectByFKCountCached(string foreignKeyName, int id) {
            string key = GetCacheKey("Count") + foreignKeyName + id.ToString();
            int? count = (int?)HttpContext.Current.Cache[key];
            if (count == null) {
                count = SelectByFKCount(foreignKeyName, id);
                AddCache(key, count);
            }
            return count.Value;
        }
        public static int SelectFilterCountCached(FilterArgument fArgument) {
            string key = GetCacheKey("Count") + fArgument.PropertyField + fArgument.Comparator + fArgument.FValue.ToString();
            int? count = (int?)HttpContext.Current.Cache[key];
            if (count == null) {
                count = SelectFilterCount(fArgument);
                AddCache(key, count);
            }
            return count.Value;
        }


        public static int SelectSearchCountCached(SearchArgument sArgument) {
            int n = sArgument.SearchArguments.Count;
            string key = GetCacheKey("Count");
            for (int i = 0; i < n; i++) {
                key += sArgument.SearchArguments[i].PropertyField
                    + sArgument.SearchArguments[i].Comparator + sArgument.SearchArguments[i].FValue.ToString();
            }
            int? count = (int?)HttpContext.Current.Cache[key];
            if (count == null) {
                count = SelectSearchCount(sArgument);
                AddCache(key, count);
            }
            return count.Value;
        }

        public static int SelectFKCountCached(string foreignKeyName, int id) {
            string key = GetCacheKey("Count") + foreignKeyName + id.ToString();
            int? count = (int?)HttpContext.Current.Cache[key];
            if (count == null) {
                count = SelectByFKCount(foreignKeyName, id);
                AddCache(key, count);
            }
            return count.Value;
        }

        public static int SelectFilterByFKCountCached(string foreignKeyName, int id, FilterArgument fArgument) {
            string key = GetCacheKey("Count") + fArgument.PropertyField + fArgument.Comparator + fArgument.FValue.ToString() + foreignKeyName + id.ToString();
            int? count = (int?)HttpContext.Current.Cache[key];
            if (count == null) {
                count = SelectFilterByFKCount(foreignKeyName, id, fArgument);
                AddCache(key, count);
            }
            return count.Value;
        }

        public static int SelectSearchByFKCountCached(string foreignKeyName, int id, SearchArgument sArgument) {
            int n = sArgument.SearchArguments.Count;
            string key = GetCacheKey("Count");
            for (int i = 0; i < n; i++) {
                key += sArgument.SearchArguments[i].PropertyField
                    + sArgument.SearchArguments[i].Comparator + sArgument.SearchArguments[i].FValue.ToString();
            }
            key += foreignKeyName + id.ToString();
            int? count = (int?)HttpContext.Current.Cache[key];
            if (count == null) {
                count = SelectSearchByFKCount(foreignKeyName, id, sArgument);
                AddCache(key, count);
            }
            return count.Value;
        }
        /// <summary>
        /// Returns count of records in database
        /// </summary>
        /// <param name="orderBy">The orderby parameter is ignored, but 
        /// required by ObjectDataSource control</param>
        public static int SelectCountCached(string orderBy) {
            return SelectCountCached();
        }


        #endregion


        #region Get Methods


        public static T Get(int? id) {
            if (id == null)
                return new T();

            return Table.Single(GetDynamicGet(id.Value));
        }

        public static IEnumerable<T> GetExtend(int FirstId, int SecondId) {
            IEnumerable<T> firstIEnum = Table.Where(GetDynamicGetGreaterThanEqaul(FirstId)).ToList();
            IEnumerable<T> secondIenum = Table.Where(GetDynamicGetLessThanEqaul(SecondId));
            return Enumerable.Intersect(firstIEnum, secondIenum);    
        }


        private static Expression<Func<T, bool>> GetDynamicGet(int id) {
            ParameterExpression e = Expression.Parameter(typeof(T), "e");
            string identityColumnName = GetIdentityColumnName();
            PropertyInfo propInfo = typeof(T).GetProperty(identityColumnName);
            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(id, typeof(int));
            BinaryExpression b = Expression.Equal(m, c);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(b, e);
            return lambda;
        }

        private static Expression<Func<T, bool>> GetDynamicGetGreaterThanEqaul(int FirstId) {
            ParameterExpression e = Expression.Parameter(typeof(T), "e");
            string identityColumnName = GetIdentityColumnName();
            PropertyInfo propInfo = typeof(T).GetProperty(identityColumnName);
            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(FirstId, typeof(int));
            BinaryExpression b = Expression.GreaterThanOrEqual(m, c);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(b, e);
            return lambda;
        }

        private static Expression<Func<T, bool>> GetDynamicGetLessThanEqaul(int SecondId) {
            ParameterExpression e = Expression.Parameter(typeof(T), "e");
            string identityColumnName = GetIdentityColumnName();
            PropertyInfo propInfo = typeof(T).GetProperty(identityColumnName);
            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(SecondId, typeof(int));
            BinaryExpression b = Expression.LessThanOrEqual(m, c);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(b, e);
            return lambda;
        }

        private static string GetIdentityColumnName()
        {
            return Context.Mapping.GetMetaType(typeof(T)).DBGeneratedIdentityMember.Name;
        }



        #endregion

        #region Save Methods

        //public virtual void Save(T newEntity) {
        //    T oldEntity = (T)this;
        //    string identityColumnName = GetIdentityColumnName();
        //    if ((int)oldEntity.GetType().GetProperty(identityColumnName).GetValue(oldEntity, null) == 0)
        //        newEntity.Insert();
        //    else
        //        oldEntity.Update(newEntity);
        //}


        //public static T Save(T oldEntity, T newEntity) {
        //    oldEntity.Save(newEntity);
        //    return newEntity;
        //}

        #endregion


        #region Insert Methods


        public virtual void Insert() {
            this.PerformValidation();
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


        #region Update Methods

        public virtual void Update(T newEntity)
        {
            T oldEntity = (T)this;
            newEntity.PerformValidation();

            
           


            Table.Attach(newEntity, oldEntity);

            Context.SubmitChanges();
            ClearCache();

            if (oldEntity == null)
                throw new Exception(String.Format("Could not update {0}", typeof(T)));

            oldEntity.ValidationErrors = newEntity.ValidationErrors;
            newEntity = oldEntity;

        }

        public virtual void Update2()
        {
            
            T oldEntity = (T)this;

            Context.SubmitChanges();
            ClearCache();
        }

        public static T ShallowClone<T>(T srcObject) where T : class, new()
        {
            // Get the object type
            Type objectType = typeof(T);

            // Get the public properties of the object
            PropertyInfo[] propInfo = srcObject.GetType()
               .GetProperties(
                  System.Reflection.BindingFlags.Instance |
                  System.Reflection.BindingFlags.Public
               );

            // Create a new  object
            T newObject = new T();

            // Loop through all the properties and copy the information 
            // from the source object to the new instance
            foreach (PropertyInfo p in propInfo)
            {
                Type t = p.PropertyType;
                if ((t.IsValueType || t == typeof(string)) && (p.CanRead) && (p.CanWrite))
                {
                    p.SetValue(newObject, p.GetValue(srcObject, null), null);
                }
            }

            // Return the cloned object.
            return newObject;
        }



        public static T Update(T oldEntity, T newEntity)
        {
            oldEntity.Update(newEntity);
            return oldEntity;
        }




        #endregion



        #region Delete Methods


        public virtual void Delete() {
            T entityToDelete = (T)this;
            Table.Attach(entityToDelete);
            Table.DeleteOnSubmit(entityToDelete);
            Context.SubmitChanges();
            ClearCache();
        }


        public static T Delete(T entityToDelete) {
            entityToDelete.Delete();
            return entityToDelete;
        }


        #endregion



        #region Cache Methods


        protected static void AddCache(string key, object value) {
            EnsurePrimaryCacheKey();
            string primaryKey = GetPrimaryCacheKey();
            HttpContext context = HttpContext.Current;
            CacheDependency keyDepend = new CacheDependency(null, new string[] { primaryKey });
            context.Cache.Insert(key, value, keyDepend);

        }


        private static void EnsurePrimaryCacheKey() {
            string primaryKey = GetPrimaryCacheKey();
            HttpContext context = HttpContext.Current;
            if (context.Cache[primaryKey] == null)
                context.Cache.Insert(primaryKey, DateTime.Now.ToString());
        }



        private static string GetPrimaryCacheKey() {
            return String.Format("{0}_Cache", typeof(T));
        }

        public static string GetCacheKey(string key) {
            return String.Format("{0}_{1}", GetPrimaryCacheKey(), key);
        }


        public static void ClearCache() {
            //string key = GetPrimaryCacheKey();
            IDictionaryEnumerator idenum = HttpContext.Current.Cache.GetEnumerator();
            while (idenum.MoveNext()) {
                string key = idenum.Key.ToString();
                HttpContext.Current.Cache.Remove(key);
            }
        }



        #endregion

        #region Filter Methods

        /// <summary>
        /// Filter
        /// </summary>
        /// 
        public static IQueryable<T> SelectFilter(FilterArgument fArgument, string orderBy, int startRowIndex, int maximumRows) {
            if (startRowIndex == -1)
                return null;

            return SelectDynamicFilter(fArgument, orderBy).Skip(startRowIndex).Take(maximumRows);
        }
        public static IQueryable<T> SelectFilterByFK(string foreignKeyName, int id, FilterArgument fArgument, string orderBy, int startRowIndex, int maximumRows) {
            if (startRowIndex == -1)
                return null;

            return SelectDynamicFilterByFK(foreignKeyName, id, fArgument, orderBy).Skip(startRowIndex).Take(maximumRows);
        }
        public static IQueryable<T> SelectDynamicFilterByFK(string foreignKeyName, int id, FilterArgument fArgument, string orderBy) {
            ParameterExpression e = Expression.Parameter(typeof(T), "e");
            PropertyInfo propInfo = typeof(T).GetProperty(foreignKeyName);
            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(id, typeof(int));
            BinaryExpression b = Expression.Equal(m, c);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(b, e);

            return SelectDynamicFilter(fArgument, orderBy).Where(lambda);
        }

        private static IQueryable<T> SelectDynamicFilter(FilterArgument fArgument, string orderBy) {
            string propertyName = fArgument.PropertyField;
            string comparator = fArgument.Comparator;
            object value = fArgument.FValue;
            PropertyInfo prop = typeof(T).GetProperty(propertyName);
            Type myType;
            bool nullable = false;
            if (IsNullableType(prop.PropertyType)) {
                NullableConverter nc = new NullableConverter(prop.PropertyType);
                myType = nc.UnderlyingType;
                nullable = true;
            } else {
                myType = prop.PropertyType;
            }

            if (!nullable) {
                switch (Type.GetTypeCode(myType)) {
                    case TypeCode.Boolean:
                        return GetDynamicFilterResults<bool>(propertyName, comparator, Convert.ToBoolean(value), orderBy, myType);
                    case TypeCode.Byte:
                        return GetDynamicFilterResults<byte>(propertyName, comparator, Convert.ToByte(value), orderBy, myType);
                    case TypeCode.Char:
                        return GetDynamicFilterResults<char>(propertyName, comparator, Convert.ToChar(value), orderBy, myType);
                    case TypeCode.DateTime:
                        return GetDynamicFilterResults<DateTime>(propertyName, comparator, Convert.ToDateTime(value), orderBy, myType);
                    case TypeCode.Decimal:
                        return GetDynamicFilterResults<Decimal>(propertyName, comparator, Convert.ToDecimal(value), orderBy, myType);
                    case TypeCode.Double:
                        return GetDynamicFilterResults<double>(propertyName, comparator, Convert.ToDouble(value), orderBy, myType);
                    case TypeCode.Int16:
                        return GetDynamicFilterResults<Int16>(propertyName, comparator, Convert.ToInt16(value), orderBy, myType);
                    case TypeCode.Int32:
                        return GetDynamicFilterResults<Int32>(propertyName, comparator, Convert.ToInt32(value), orderBy, myType);
                    case TypeCode.Int64:
                        return GetDynamicFilterResults<Int64>(propertyName, comparator, Convert.ToInt64(value), orderBy, myType);
                    case TypeCode.Single:
                        return GetDynamicFilterResults<Single>(propertyName, comparator, Convert.ToSingle(value), orderBy, myType);
                    case TypeCode.String:
                        return GetDynamicFilterResults<String>(propertyName, comparator, Convert.ToString(value), orderBy, myType);
                    default:
                        throw new Exception("Cannot filter by column " + propertyName + " because of its type");
                }
            } else {
                switch (Type.GetTypeCode(myType)) {
                    case TypeCode.Boolean:
                        return GetDynamicFilterResults<System.Nullable<bool>>(propertyName, comparator, Convert.ToBoolean(value), orderBy, myType);
                    case TypeCode.Byte:
                        return GetDynamicFilterResults<System.Nullable<byte>>(propertyName, comparator, Convert.ToByte(value), orderBy, myType);
                    case TypeCode.Char:
                        return GetDynamicFilterResults<System.Nullable<char>>(propertyName, comparator, Convert.ToChar(value), orderBy, myType);
                    case TypeCode.DateTime:
                        return GetDynamicFilterResults<System.Nullable<DateTime>>(propertyName, comparator, Convert.ToDateTime(value), orderBy, myType);
                    case TypeCode.Decimal:
                        return GetDynamicFilterResults<System.Nullable<Decimal>>(propertyName, comparator, Convert.ToDecimal(value), orderBy, myType);
                    case TypeCode.Double:
                        return GetDynamicFilterResults<System.Nullable<double>>(propertyName, comparator, Convert.ToDouble(value), orderBy, myType);
                    case TypeCode.Int16:
                        return GetDynamicFilterResults<System.Nullable<Int16>>(propertyName, comparator, Convert.ToInt16(value), orderBy, myType);
                    case TypeCode.Int32:
                        return GetDynamicFilterResults<System.Nullable<Int32>>(propertyName, comparator, Convert.ToInt32(value), orderBy, myType);
                    case TypeCode.Int64:
                        return GetDynamicFilterResults<System.Nullable<Int64>>(propertyName, comparator, Convert.ToInt64(value), orderBy, myType);
                    case TypeCode.Single:
                        return GetDynamicFilterResults<System.Nullable<Single>>(propertyName, comparator, Convert.ToSingle(value), orderBy, myType);
                    case TypeCode.String:
                        return GetDynamicFilterResults<String>(propertyName, comparator, Convert.ToString(value), orderBy, myType);
                    default:
                        throw new Exception("Cannot filter by column " + propertyName + " because of its type");
                }
            }


        }

        private static IQueryable<T> GetDynamicFilterResults<T1>(string propertyName, string comparator, object value, string orderBy, Type type) {
            ParameterExpression e = Expression.Parameter(typeof(T), "e");
            PropertyInfo propInfo = typeof(T).GetProperty(propertyName);

            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(value, typeof(T1));

            BinaryExpression b = null;
            MethodCallExpression mce = null;
            Expression<Func<T, bool>> lambda = null;
            if (Type.GetTypeCode(type) != TypeCode.String && Type.GetTypeCode(type) != TypeCode.Boolean) {
                switch (comparator) {
                    case "Equal":
                        b = Expression.Equal(m, c);
                        break;
                    case "NotEqual":
                        b = Expression.NotEqual(m, c);
                        break;
                    case "LessThan":
                        b = Expression.LessThan(m, c);
                        break;
                    case "LessThanOrEqual":
                        b = Expression.LessThanOrEqual(m, c);
                        break;
                    case "GreaterThan":
                        b = Expression.GreaterThan(m, c);
                        break;
                    case "GreaterThanOrEqual":
                        b = Expression.GreaterThanOrEqual(m, c);
                        break;
                }
                lambda = Expression.Lambda<Func<T, bool>>(b, e);
            }
            if (Type.GetTypeCode(type) == TypeCode.String) {
                Type[] types = { typeof(string) };
                ConstantExpression[] express = { c };
                switch (comparator) {
                    case "Equals":
                        mce = Expression.Call(m, typeof(string).GetMethod("Equals", types), express);
                        break;
                    case "StartsWith":
                        mce = Expression.Call(m, typeof(string).GetMethod("StartsWith", types), express);
                        break;
                    case "EndsWith":
                        mce = Expression.Call(m, typeof(string).GetMethod("EndsWith", types), express);
                        break;
                    case "Contains":
                        mce = Expression.Call(m, typeof(string).GetMethod("Contains", types), express);
                        break;
                }
                lambda = Expression.Lambda<Func<T, bool>>(mce, e);
            }
            if (Type.GetTypeCode(type) == TypeCode.Boolean) {
                b = Expression.Equal(m, c);
                lambda = Expression.Lambda<Func<T, bool>>(b, e);
            }
            return Select(orderBy).AsQueryable().Where(lambda);
        }
        #endregion


        #region Search Methods

        /// <summary>
        /// Search
        /// </summary>
        /// new
        public static IQueryable<T> SelectSearch(SearchArgument sArgument, string orderBy, int startRowIndex, int maximumRows) {
            if (startRowIndex == -1)
                return null;

            return SelectDynamicSearch(sArgument, orderBy).Skip(startRowIndex).Take(maximumRows);
        }
        public static IQueryable<T> SelectSearchByFK(string foreignKeyName, int id, SearchArgument sArgument, string orderBy, int startRowIndex, int maximumRows) {
            if (startRowIndex == -1)
                return null;

            return SelectDynamicSearchByFK(foreignKeyName, id, sArgument, orderBy).Skip(startRowIndex).Take(maximumRows);
        }
        public static IQueryable<T> SelectSearch(SearchArgument sArgument, string orderBy) {
            return SelectDynamicSearch(sArgument, orderBy);
        }
        public static IQueryable<T> SelectDynamicSearchByFK(string foreignKeyName, int id, SearchArgument sArgument, string orderBy) {
            ParameterExpression e = Expression.Parameter(typeof(T), "e");
            PropertyInfo propInfo = typeof(T).GetProperty(foreignKeyName);
            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(id, typeof(int));
            BinaryExpression b = Expression.Equal(m, c);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(b, e);
            return SelectSearch(sArgument, orderBy).Where(lambda);
        }
        private static IQueryable<T> SelectDynamicSearch(SearchArgument sArgument, string orderBy) {
            int n = sArgument.SearchArguments.Count;
            ParameterExpression e = Expression.Parameter(typeof(T), "e");
            Expression<Func<T, bool>> lambda = null;
            Expression be = null;
            int i;
            for (i = 0; i < n; i++) {
                string pField = sArgument.SearchArguments[i].PropertyField;
                string comparator = sArgument.SearchArguments[i].Comparator;
                object fvalue = sArgument.SearchArguments[i].FValue;
                PropertyInfo prop = typeof(T).GetProperty(pField);
                Type myType;
                bool nullable = false;
                if (IsNullableType(prop.PropertyType)) {
                    NullableConverter nc = new NullableConverter(prop.PropertyType);
                    myType = nc.UnderlyingType;
                    nullable = true;
                } else {
                    myType = prop.PropertyType;
                }
                Expression next = null;
                if (!nullable) {
                    switch (Type.GetTypeCode(myType)) {
                        case TypeCode.Boolean:
                            next = GetDynamicSearchExpression<bool>(pField, comparator, Convert.ToBoolean(fvalue), e, myType);
                            break;
                        case TypeCode.Byte:
                            next = GetDynamicSearchExpression<byte>(pField, comparator, Convert.ToByte(fvalue), e, myType);
                            break;
                        case TypeCode.Char:
                            next = GetDynamicSearchExpression<char>(pField, comparator, Convert.ToChar(fvalue), e, myType);
                            break;
                        case TypeCode.DateTime:
                            next = GetDynamicSearchExpression<DateTime>(pField, comparator, Convert.ToDateTime(fvalue), e, myType);
                            break;
                        case TypeCode.Decimal:
                            next = GetDynamicSearchExpression<Decimal>(pField, comparator, Convert.ToDecimal(fvalue), e, myType);
                            break;
                        case TypeCode.Double:
                            next = GetDynamicSearchExpression<double>(pField, comparator, Convert.ToDouble(fvalue), e, myType);
                            break;
                        case TypeCode.Int16:
                            next = GetDynamicSearchExpression<Int16>(pField, comparator, Convert.ToInt16(fvalue), e, myType);
                            break;
                        case TypeCode.Int32:
                            next = GetDynamicSearchExpression<Int32>(pField, comparator, Convert.ToInt32(fvalue), e, myType);
                            break;
                        case TypeCode.Int64:
                            next = GetDynamicSearchExpression<Int64>(pField, comparator, Convert.ToInt64(fvalue), e, myType);
                            break;
                        case TypeCode.Single:
                            next = GetDynamicSearchExpression<Single>(pField, comparator, Convert.ToSingle(fvalue), e, myType);
                            break;
                        case TypeCode.String:
                            next = GetDynamicSearchExpression<String>(pField, comparator, Convert.ToString(fvalue), e, myType);
                            break;
                        default:
                            throw new Exception("Cannot search by column " + pField + " because of its type");
                    }
                } else {
                    switch (Type.GetTypeCode(myType)) {
                        case TypeCode.Boolean:
                            next = GetDynamicSearchExpression<System.Nullable<bool>>(pField, comparator, Convert.ToBoolean(fvalue), e, myType);
                            break;
                        case TypeCode.Byte:
                            next = GetDynamicSearchExpression<System.Nullable<byte>>(pField, comparator, Convert.ToByte(fvalue), e, myType);
                            break;
                        case TypeCode.Char:
                            next = GetDynamicSearchExpression<System.Nullable<char>>(pField, comparator, Convert.ToChar(fvalue), e, myType);
                            break;
                        case TypeCode.DateTime:
                            next = GetDynamicSearchExpression<System.Nullable<DateTime>>(pField, comparator, Convert.ToDateTime(fvalue), e, myType);
                            break;
                        case TypeCode.Decimal:
                            next = GetDynamicSearchExpression<System.Nullable<Decimal>>(pField, comparator, Convert.ToDecimal(fvalue), e, myType);
                            break;
                        case TypeCode.Double:
                            next = GetDynamicSearchExpression<System.Nullable<double>>(pField, comparator, Convert.ToDouble(fvalue), e, myType);
                            break;
                        case TypeCode.Int16:
                            next = GetDynamicSearchExpression<System.Nullable<Int16>>(pField, comparator, Convert.ToInt16(fvalue), e, myType);
                            break;
                        case TypeCode.Int32:
                            next = GetDynamicSearchExpression<System.Nullable<Int32>>(pField, comparator, Convert.ToInt32(fvalue), e, myType);
                            break;
                        case TypeCode.Int64:
                            next = GetDynamicSearchExpression<System.Nullable<Int64>>(pField, comparator, Convert.ToInt64(fvalue), e, myType);
                            break;
                        case TypeCode.Single:
                            next = GetDynamicSearchExpression<System.Nullable<Single>>(pField, comparator, Convert.ToSingle(fvalue), e, myType);
                            break;
                        case TypeCode.String:
                            next = GetDynamicSearchExpression<String>(pField, comparator, Convert.ToString(fvalue), e, myType);
                            break;
                        default:
                            throw new Exception("Cannot filter by column " + pField + " because of its type");
                    }
                }
                if (i == 0)
                    be = next;
                else
                    be = Expression.And(be, next);

            }
            lambda = Expression.Lambda<Func<T, bool>>(be, e);
            return Select(orderBy).AsQueryable().Where(lambda);
        }

        private static Expression GetDynamicSearchExpression<T1>(string propertyName, string comparator, object value, ParameterExpression e, Type type) {
            PropertyInfo propInfo = typeof(T).GetProperty(propertyName);
            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(value, typeof(T1));

            BinaryExpression b = null;
            MethodCallExpression mce = null;

            if (Type.GetTypeCode(type) != TypeCode.String && Type.GetTypeCode(type) != TypeCode.Boolean) {
                switch (comparator) {
                    case "Equal":
                        b = Expression.Equal(m, c);
                        break;
                    case "NotEqual":
                        b = Expression.NotEqual(m, c);
                        break;
                    case "LessThan":
                        b = Expression.LessThan(m, c);
                        break;
                    case "LessThanOrEqual":
                        b = Expression.LessThanOrEqual(m, c);
                        break;
                    case "GreaterThan":
                        b = Expression.GreaterThan(m, c);
                        break;
                    case "GreaterThanOrEqual":
                        b = Expression.GreaterThanOrEqual(m, c);
                        break;
                }
            }
            if (Type.GetTypeCode(type) == TypeCode.String) {
                Type[] types = { typeof(string) };
                ConstantExpression[] express = { c };
                switch (comparator) {
                    case "Equals":
                        mce = Expression.Call(m, typeof(string).GetMethod("Equals", types), express);
                        break;
                    case "StartsWith":
                        mce = Expression.Call(m, typeof(string).GetMethod("StartsWith", types), express);
                        break;
                    case "EndsWith":
                        mce = Expression.Call(m, typeof(string).GetMethod("EndsWith", types), express);
                        break;
                    case "Contains":
                        mce = Expression.Call(m, typeof(string).GetMethod("Contains", types), express);
                        break;
                }
            }
            if (Type.GetTypeCode(type) == TypeCode.Boolean) {
                b = Expression.Equal(m, c);
            }

            if (b == null)
                return mce;
            else
                return b;
        }
        #endregion

        #region ReportMethods

        /// <summary>
        /// Filter
        /// </summary>
        /// 
        public static IQueryable<T> SelectReport(ReportArgument rArgument, SearchArgument sArgument, string orderBy, string companyName, string branchName, string foreignKeyName, int id, string UserName) {
            SelectDynamicReport(rArgument, sArgument, orderBy, companyName, branchName, foreignKeyName, id, UserName);
            

            return null;
        }


        // Izmeni vo delot za dobivanje na count bidejki loso rabotese so cash-irani podatoci
        private static void SelectDynamicReport(ReportArgument rArgument, SearchArgument sArgument, string orderBy, string companyName, string branchName, string foreignKeyName, int id, string UserName) {
            // IEnumerable<T> data;
            List<T> data;
            int count;
            if (!String.IsNullOrEmpty(foreignKeyName)) {
                if (sArgument != null) {
                    data = SelectDynamicSearchByFK(foreignKeyName, id, sArgument, orderBy).ToList<T>();
                    count = SelectSearchByFKCountCached(foreignKeyName, id, sArgument);
                } else {
                    data = SelectDynamicByFK(foreignKeyName, id, orderBy).ToList<T>();
                    count = SelectByFKCountCached(foreignKeyName, id);
                }
            } else {
                if (sArgument != null) {
                    data = SelectSearch(sArgument, orderBy).ToList<T>();
                    count = data.Count;
                    //count = SelectSearchCountCached(sArgument);
                } else {
                    data = Select(orderBy).ToList<T>();
                    count = data.Count;
                    //count = SelectCountCached();
                }
            }
            int n = count;
            int m = rArgument.ReportArguments.Count;
            TypeCode[] codes = new TypeCode[m];
            PropertyInfo[] infos = new PropertyInfo[m];
            bool first = true;
            if (rArgument.Format == "PDF") {

                PDFCreators creator = new PDFCreators(rArgument.Orientation, rArgument.MarginLeft, rArgument.MarginRight, rArgument.MarginTop, rArgument.MarginBottom);

                creator.SetDocumentHeaderFooter(companyName);
                creator.OpenPDF();
                creator.SetTitle(rArgument.ReportName);
                string[] headers = new string[m];
                for (int i = 0; i < m; i++) {
                    headers[i] = rArgument.ReportArguments[i].HeaderText;
                }
                creator.CreateTable(m, true, headers, branchName);
                                
                for (int i = 0; i < n; i++) {
                    object[] values = new object[m];
                    for (int j = 0; j < m; j++) {

                        if (first) {
                            infos[j] = typeof(T).GetProperty(rArgument.ReportArguments[j].PropertyField);
                            if (IsNullableType(infos[j].PropertyType)) {
                                NullableConverter nc = new NullableConverter(infos[j].PropertyType);                               
                                codes[j] = Type.GetTypeCode(nc.UnderlyingType);
                            } else {
                                codes[j] = Type.GetTypeCode(infos[j].PropertyType);
                            }
                        }
                        values[j] = infos[j].GetValue(data[i], null);
                        if (codes[j] == TypeCode.DateTime) {
                            DateTime date = (DateTime)values[j];
                            values[j] = date.ToShortDateString();
                        }

                        if (codes[j] == TypeCode.Decimal) {
                            decimal dec = (decimal)values[j];
                            values[j] = String.Format("{0:#,0.00}", dec);
                        }

                        if (codes[j] == TypeCode.Boolean) {
                            bool temp = (bool)values[j];
                            if (temp == true) {
                                values[j] = "Да";
                            } else {
                                values[j] = "Не";
                            }
                        }
                    }
                    first = false;
                    if (creator.AddDataRow(values, m, codes))
                        i--;
                }
                creator.AddTable();
                creator.FinishPDF();
            }

            //Creating Excel document
            else if (rArgument.Format == "XLS") {
                ExcelFileWriter<T> myExcel = new ExcelFileWriter<T>();
                string[] headers = new string[m];
                for (int i = 0; i < m; i++) {
                    headers[i] = rArgument.ReportArguments[i].HeaderText;
                }
                myExcel.Headers = headers;

                int temp_num = ('A') + m - 1;
                char lastCol = Convert.ToChar(temp_num);
                string finalColumn = lastCol + "1";

                myExcel.ColumnCount = m - 1;
                myExcel.RowCount = n;

                myExcel.ActivateExcel();
                myExcel.FillHeaderColumn(headers, "A1", finalColumn);

                for (int i = 0; i < n; i++) {
                    object[] values = new object[m];

                    for (int j = 0; j < m; j++) {
                        if (first) {
                            infos[j] = typeof(T).GetProperty(rArgument.ReportArguments[j].PropertyField);
                            if (IsNullableType(infos[j].PropertyType)) {
                                NullableConverter nc = new NullableConverter(infos[j].PropertyType);
                                codes[j] = Type.GetTypeCode(nc.UnderlyingType);
                            } else {
                                codes[j] = Type.GetTypeCode(infos[j].PropertyType);
                            }
                        }
                        values[j] = infos[j].GetValue(data[i], null);

                        if (codes[j] == TypeCode.DateTime) {
                            DateTime date = (DateTime)values[j];
                            values[j] = date.ToShortDateString();
                        }

                        if (codes[j] == TypeCode.Boolean) {
                            bool temp = (bool)values[j];
                            if (temp == true) {
                                values[j] = "Да";
                            } else {
                                values[j] = "Не";
                            }
                        }
                    }
                    first = false;
                    string fColumn = "A" + (i + 2).ToString();
                    string lColumn = lastCol + (i + 2).ToString();
                    myExcel.FillRowData_Mine(values, fColumn, lColumn);

                }
                string fileName = UserName + "_" + rArgument.ReportName + ".xls";
                myExcel.SaveExcel(fileName);
                myExcel.FinishExcel(fileName);
                
            }

            //Create Word document
            else if (rArgument.Format == "DOC") {
                WordFileWriter<T> myWord = new WordFileWriter<T>();
                myWord.Orientation = rArgument.Orientation;
                myWord.ActivateWord();

                //myWord.InsertingText(rArgument.ReportName);
                //myWord.InsertingText("Датум на извештај: " + DateTime.Now.ToShortDateString());

                string[] headers = new string[m];
                for (int i = 0; i < m; i++) {
                    headers[i] = rArgument.ReportArguments[i].HeaderText;
                }
                myWord.Headers = headers;
                object[,] tempFillValues = new object[n, m];

                CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
                CultureInfo oldUICI = System.Threading.Thread.CurrentThread.CurrentUICulture;

                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("mk-MK");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("mk-MK");


                for (int i = 0; i < n; i++) {
                    object[] values = new object[m];
                    for (int j = 0; j < m; j++) {
                        if (first) {
                            infos[j] = typeof(T).GetProperty(rArgument.ReportArguments[j].PropertyField);
                            if (IsNullableType(infos[j].PropertyType)) {
                                NullableConverter nc = new NullableConverter(infos[j].PropertyType);
                                codes[j] = Type.GetTypeCode(nc.UnderlyingType);
                            } else {
                                codes[j] = Type.GetTypeCode(infos[j].PropertyType);
                            }
                        }
                        values[j] = infos[j].GetValue(data[i], null);

                        if (codes[j] == TypeCode.DateTime) {
                            DateTime date = (DateTime)values[j];
                            values[j] = date.ToShortDateString();
                        }

                        if (codes[j] == TypeCode.Boolean) {
                            bool temp = (bool)values[j];
                            if (temp == true) {
                                values[j] = "Да";
                            } else {
                                values[j] = "Не";
                            }
                        }
                        tempFillValues[i, j] = values[j];

                    }
                    first = false;
                }

                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
                System.Threading.Thread.CurrentThread.CurrentUICulture = oldUICI;

                myWord.FillValues = tempFillValues;
                myWord.CreateTable(n, m);

                myWord.InsertFooter();
                myWord.InsertHeader(rArgument.BranchName);

                string fileName = UserName + "_" + rArgument.ReportName + ".doc";
                myWord.SaveDOC(fileName);
                myWord.FinishDOC(fileName);
            }

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
                    //context = new DataContext(conSettings.ConnectionString);
                    context = new DataClassesDataContext(conSettings.ConnectionString);
                    HttpContext.Current.Items["DataContext"] = context;
                    //context.Log = new TraceWriter();
                    context.Log = new Logger();
                    
                }
                return context;
            }
        }

        

        public static IEnumerable<PropertyInfo> GetProperties<T>() {
            return typeof(T).GetProperties();
        }

        public static IEnumerable<PropertyTypeInfo> GetPropertyInfo() {
            PropertyInfo[] pinfos = typeof(T).GetProperties().ToArray<PropertyInfo>();
            int n = pinfos.Count() - 2;
            List<PropertyTypeInfo> res = new List<PropertyTypeInfo>(n);
            for (int i = 0; i < n; i++) {
                if (Type.GetTypeCode(pinfos[i].PropertyType) != TypeCode.String) {
                    res.Add(new PropertyTypeInfo("true" + i.ToString(), pinfos[i].Name));
                } else {
                    res.Add(new PropertyTypeInfo("false" + i.ToString(), pinfos[i].Name));
                }
            }
            return res;
        }

        private static object GetValueForProperty<T>(string propertyName, object element) {
            return typeof(T).GetProperty(propertyName).GetValue(element, null);
        }

    }
}

