using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using ControlsLibriry.Utility;
using System.Web.Caching;
using Broker.Controllers.ReportControllers;
using MyClass.WriteToExcel;
using MyClass.WriteToWord;
using System.Globalization;


/// <summary>
/// Summary description for RollBackDistributionInfo
/// </summary>
namespace Broker.Controllers.DistributionControllers {
    [Serializable]
    public class RollBackDistributionInfo {
        public RollBackDistributionInfo() {
            //
            // TODO: Add constructor logic here
            //
        }
        public int ID { get; set; }
        public string PolicyNumber { get; set; }
        public bool IsForRollBack { get; set; }
        public int InsuranceSubTypeID { get; set; }
        public decimal PremiumValue { get; set; }
        public decimal BrokerageValue { get; set; }
    }
    [Serializable]
    public class RollBackDistributionCompanyInfo {
        public RollBackDistributionCompanyInfo() {
            //
            // TODO: Add constructor logic here
            //
        }

        public RollBackDistributionCompanyInfo(ViewRollBackDistributionCompany rbdc) {

            this.Date = rbdc.Date;
            this.EndDate = rbdc.EndDate;
            this.ID = rbdc.ID;
            this.InsuranceCompanyName = rbdc.InsuranceCompanyName;
            this.IsForRollBack = false;
            this.Name = rbdc.Name;
            this.Number = rbdc.Number;
            this.StartDate = rbdc.StartDate;
            this.InsuranceCompanyID = rbdc.InsuranceCompanyID;
        }

        public static DateTime getDate(List<RollBackDistributionCompanyInfo> infoList, bool start) {
            DateTime returnDate = new DateTime();
            if (start) {
                returnDate = new DateTime(9999, 12, 1);
            } 
            
            foreach (RollBackDistributionCompanyInfo info in infoList) {
                if (start && returnDate > info.StartDate) {
                    returnDate = info.StartDate;
                }
                if (!start && returnDate< info.EndDate) {
                    returnDate = info.EndDate;
                }
            }
            return returnDate;
        }
        public static IQueryable<RollBackDistributionCompanyInfo> Select(int startRowIndex, int maximumRows, string orderBy) {
            if (startRowIndex == -1)
                return null;

            if (String.IsNullOrEmpty(orderBy))
                return Select(startRowIndex, maximumRows);

            return SelectDynamicSort(orderBy).Skip(startRowIndex).Take(maximumRows);
        }

        public static IQueryable<RollBackDistributionCompanyInfo> Select(int startRowIndex, int maximumRows) {
            if (startRowIndex == -1)
                return null;

            return Select().Skip(startRowIndex).Take(maximumRows);
        }

        /// <summary>
        /// Returns sorted list of entity
        /// </summary>
        /// <param name="orderBy">Name of property to order by</param>
        public static IQueryable<RollBackDistributionCompanyInfo> Select(string orderBy) {
            if (String.IsNullOrEmpty(orderBy))
                return Select();

            return SelectDynamicSort(orderBy);
        }


        public static IQueryable<RollBackDistributionCompanyInfo> Select() {
            IQueryable<ViewRollBackDistributionCompany> all = ViewRollBackDistributionCompany.Select();
            List<RollBackDistributionCompanyInfo> list = new List<RollBackDistributionCompanyInfo>();
            foreach (ViewRollBackDistributionCompany view in all) {
                // RollBackDistributionCompanyInfo info = new RollBackDistributionCompanyInfo(view);
                list.Add(new RollBackDistributionCompanyInfo(view));
            }
            return list.AsQueryable();
        }

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

        public static int SelectCountCached() {
            return Select().Count();
        }

        /// <summary>
        /// Used to perform typed and dynamic order by.
        /// </summary>
        /// <param name="orderBy">The full order by expression, e.g. AuthorName DESC</param>
        private static IQueryable<RollBackDistributionCompanyInfo> SelectDynamicSort(string orderBy) {
            // Parse order by            
            string orderByColumn = String.Empty;
            string orderByDirection = "asc";
            ParseOrderBy(orderBy, ref orderByColumn, ref orderByDirection);

            // Get sort results

            PropertyInfo prop = typeof(RollBackDistributionCompanyInfo).GetProperty(orderByColumn);
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
        private static IQueryable<RollBackDistributionCompanyInfo> GetDynamicSortResults<S>(string orderByColumn, string orderByDirection) {
            ParameterExpression sortParameterExpression = Expression.Parameter(typeof(RollBackDistributionCompanyInfo), "e");
            PropertyInfo propInfo = typeof(RollBackDistributionCompanyInfo).GetProperty(orderByColumn);

            MemberExpression sortPropertyExpression = Expression.MakeMemberAccess(sortParameterExpression, propInfo);

            Expression<Func<RollBackDistributionCompanyInfo, S>> sortExpression = Expression.Lambda<Func<RollBackDistributionCompanyInfo, S>>(sortPropertyExpression, sortParameterExpression);

            if (orderByDirection == "asc")
                return Select().OrderBy(sortExpression);
            else
                return Select().OrderByDescending(sortExpression);
        }

        public static IQueryable<RollBackDistributionCompanyInfo> SelectSearch(SearchArgument sArgument, string orderBy, int startRowIndex, int maximumRows) {
            if (startRowIndex == -1)
                return null;

            return SelectDynamicSearch(sArgument, orderBy).Skip(startRowIndex).Take(maximumRows);
        }
        public static IQueryable<RollBackDistributionCompanyInfo> SelectSearchByFK(string foreignKeyName, int id, SearchArgument sArgument, string orderBy, int startRowIndex, int maximumRows) {
            if (startRowIndex == -1)
                return null;

            return SelectDynamicSearchByFK(foreignKeyName, id, sArgument, orderBy).Skip(startRowIndex).Take(maximumRows);
        }
        public static IQueryable<RollBackDistributionCompanyInfo> SelectSearch(SearchArgument sArgument, string orderBy) {
            return SelectDynamicSearch(sArgument, orderBy);
        }
        public static IQueryable<RollBackDistributionCompanyInfo> SelectDynamicSearchByFK(string foreignKeyName, int id, SearchArgument sArgument, string orderBy) {
            ParameterExpression e = Expression.Parameter(typeof(RollBackDistributionCompanyInfo), "e");
            PropertyInfo propInfo = typeof(RollBackDistributionCompanyInfo).GetProperty(foreignKeyName);
            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(id, typeof(int));
            BinaryExpression b = Expression.Equal(m, c);
            Expression<Func<RollBackDistributionCompanyInfo, bool>> lambda = Expression.Lambda<Func<RollBackDistributionCompanyInfo, bool>>(b, e);
            return SelectSearch(sArgument, orderBy).Where(lambda);
        }
        private static IQueryable<RollBackDistributionCompanyInfo> SelectDynamicSearch(SearchArgument sArgument, string orderBy) {
            int n = sArgument.SearchArguments.Count;
            ParameterExpression e = Expression.Parameter(typeof(RollBackDistributionCompanyInfo), "e");
            Expression<Func<RollBackDistributionCompanyInfo, bool>> lambda = null;
            Expression be = null;
            int i;
            for (i = 0; i < n; i++) {
                string pField = sArgument.SearchArguments[i].PropertyField;
                string comparator = sArgument.SearchArguments[i].Comparator;
                object fvalue = sArgument.SearchArguments[i].FValue;
                PropertyInfo prop = typeof(RollBackDistributionCompanyInfo).GetProperty(pField);
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
            lambda = Expression.Lambda<Func<RollBackDistributionCompanyInfo, bool>>(be, e);
            return Select(orderBy).AsQueryable().Where(lambda);
        }

        private static Expression GetDynamicSearchExpression<T1>(string propertyName, string comparator, object value, ParameterExpression e, Type type) {
            PropertyInfo propInfo = typeof(RollBackDistributionCompanyInfo).GetProperty(propertyName);
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

        private static string GetPrimaryCacheKey() {
            return String.Format("{0}_Cache", typeof(RollBackDistributionCompanyInfo));
        }

        public static string GetCacheKey(string key) {
            return String.Format("{0}_{1}", GetPrimaryCacheKey(), key);
        }

        public static int SelectSearchCount(SearchArgument sArgument) {
            string orderBy = "";
            return SelectDynamicSearch(sArgument, orderBy).Count();
        }
        private static void EnsurePrimaryCacheKey() {
            string primaryKey = GetPrimaryCacheKey();
            HttpContext context = HttpContext.Current;
            if (context.Cache[primaryKey] == null)
                context.Cache.Insert(primaryKey, DateTime.Now.ToString());
        }
        protected static void AddCache(string key, object value) {
            EnsurePrimaryCacheKey();
            string primaryKey = GetPrimaryCacheKey();
            HttpContext context = HttpContext.Current;
            CacheDependency keyDepend = new CacheDependency(null, new string[] { primaryKey });
            context.Cache.Insert(key, value, keyDepend);

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

        #region Filter Methods

        /// <summary>
        /// Filter
        /// </summary>
        /// 
        public static IQueryable<RollBackDistributionCompanyInfo> SelectFilter(FilterArgument fArgument, string orderBy, int startRowIndex, int maximumRows) {
            if (startRowIndex == -1)
                return null;

            return SelectDynamicFilter(fArgument, orderBy).Skip(startRowIndex).Take(maximumRows);
        }
        public static IQueryable<RollBackDistributionCompanyInfo> SelectFilterByFK(string foreignKeyName, int id, FilterArgument fArgument, string orderBy, int startRowIndex, int maximumRows) {
            if (startRowIndex == -1)
                return null;

            return SelectDynamicFilterByFK(foreignKeyName, id, fArgument, orderBy).Skip(startRowIndex).Take(maximumRows);
        }
      
        private static IQueryable<RollBackDistributionCompanyInfo> GetDynamicFilterResults<T1>(string propertyName, string comparator, object value, string orderBy, Type type) {
            ParameterExpression e = Expression.Parameter(typeof(RollBackDistributionCompanyInfo), "e");
            PropertyInfo propInfo = typeof(RollBackDistributionCompanyInfo).GetProperty(propertyName);

            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(value, typeof(T1));

            BinaryExpression b = null;
            MethodCallExpression mce = null;
            Expression<Func<RollBackDistributionCompanyInfo, bool>> lambda = null;
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
                lambda = Expression.Lambda<Func<RollBackDistributionCompanyInfo, bool>>(b, e);
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
                lambda = Expression.Lambda<Func<RollBackDistributionCompanyInfo, bool>>(mce, e);
            }
            if (Type.GetTypeCode(type) == TypeCode.Boolean) {
                b = Expression.Equal(m, c);
                lambda = Expression.Lambda<Func<RollBackDistributionCompanyInfo, bool>>(b, e);
            }
            return Select(orderBy).AsQueryable().Where(lambda);
        }
        #endregion

        #region Filter Methods

        /// <summary>
        /// Filter
        /// </summary>
        /// 
        
        public static IQueryable<RollBackDistributionCompanyInfo> SelectDynamicFilterByFK(string foreignKeyName, int id, FilterArgument fArgument, string orderBy) {
            ParameterExpression e = Expression.Parameter(typeof(RollBackDistributionCompanyInfo), "e");
            PropertyInfo propInfo = typeof(RollBackDistributionCompanyInfo).GetProperty(foreignKeyName);
            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(id, typeof(int));
            BinaryExpression b = Expression.Equal(m, c);
            Expression<Func<RollBackDistributionCompanyInfo, bool>> lambda = Expression.Lambda<Func<RollBackDistributionCompanyInfo, bool>>(b, e);

            return SelectDynamicFilter(fArgument, orderBy).Where(lambda);
        }

        private static IQueryable<RollBackDistributionCompanyInfo> SelectDynamicFilter(FilterArgument fArgument, string orderBy) {
            string propertyName = fArgument.PropertyField;
            string comparator = fArgument.Comparator;
            object value = fArgument.FValue;
            PropertyInfo prop = typeof(RollBackDistributionCompanyInfo).GetProperty(propertyName);
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

       
        #endregion

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

        public static int SelectByFKCount(string foreignKeyName, int id) {
            string orderBy = "";
            return SelectDynamicByFK(foreignKeyName, id, orderBy).Count();
        }

        public static int SelectFilterCount(FilterArgument fArgument) {
            string orderBy = "";
            return SelectDynamicFilter(fArgument, orderBy).Count();
        }
        public static IQueryable<RollBackDistributionCompanyInfo> SelectDynamicByFK(string foreignKeyName, int id, string orderBy) {
            ParameterExpression e = Expression.Parameter(typeof(RollBackDistributionCompanyInfo), "e");
            PropertyInfo propInfo = typeof(RollBackDistributionCompanyInfo).GetProperty(foreignKeyName);
            MemberExpression m = Expression.MakeMemberAccess(e, propInfo);
            ConstantExpression c = Expression.Constant(id, typeof(int));
            BinaryExpression b = Expression.Equal(m, c);
            Expression<Func<RollBackDistributionCompanyInfo, bool>> lambda = Expression.Lambda<Func<RollBackDistributionCompanyInfo, bool>>(b, e);
            return Select(orderBy).Where(lambda);
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

        public static IQueryable<RollBackDistributionCompanyInfo> SelectReport(ReportArgument rArgument, SearchArgument sArgument, string orderBy, string companyName, string branchName, string foreignKeyName, int id, string UserName) {
            SelectDynamicReport(rArgument, sArgument, orderBy, companyName, branchName, foreignKeyName, id, UserName);
            return null;
        }

        public static int SelectSearchByFKCount(string foreignKeyName, int id, SearchArgument sArgument) {
            string orderBy = "";
            return SelectDynamicSearchByFK(foreignKeyName, id, sArgument, orderBy).Count();
        }


        private static void SelectDynamicReport(ReportArgument rArgument, SearchArgument sArgument, string orderBy, string companyName, string branchName, string foreignKeyName, int id, string UserName) {
            // IEnumerable<T> data;
            List<RollBackDistributionCompanyInfo> data;
            int count;
            if (!String.IsNullOrEmpty(foreignKeyName)) {
                if (sArgument != null) {
                    data = SelectDynamicSearchByFK(foreignKeyName, id, sArgument, orderBy).ToList<RollBackDistributionCompanyInfo>();
                    count = SelectSearchByFKCountCached(foreignKeyName, id, sArgument);
                } else {
                    data = SelectDynamicByFK(foreignKeyName, id, orderBy).ToList<RollBackDistributionCompanyInfo>();
                    count = SelectByFKCountCached(foreignKeyName, id);
                }
            } else {
                if (sArgument != null) {
                    data = SelectSearch(sArgument, orderBy).ToList<RollBackDistributionCompanyInfo>();
                    count = data.Count;
                    //count = SelectSearchCountCached(sArgument);
                } else {
                    data = Select(orderBy).ToList<RollBackDistributionCompanyInfo>();
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
                            infos[j] = typeof(RollBackDistributionCompanyInfo).GetProperty(rArgument.ReportArguments[j].PropertyField);
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
                ExcelFileWriter<RollBackDistributionCompanyInfo> myExcel = new ExcelFileWriter<RollBackDistributionCompanyInfo>();
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
                            infos[j] = typeof(RollBackDistributionCompanyInfo).GetProperty(rArgument.ReportArguments[j].PropertyField);
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
                WordFileWriter<RollBackDistributionCompanyInfo> myWord = new WordFileWriter<RollBackDistributionCompanyInfo>();
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
                            infos[j] = typeof(RollBackDistributionCompanyInfo).GetProperty(rArgument.ReportArguments[j].PropertyField);
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

        public int ID { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public bool IsForRollBack { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string InsuranceCompanyName { get; set; }
        public string Name { get; set; }
        public int InsuranceCompanyID { get; set; }

    }

}
