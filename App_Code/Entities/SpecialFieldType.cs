using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SpecialFieldType
/// </summary>
namespace  Broker.DataAccess{
public partial class SpecialFieldType:EntityBase<SpecialFieldType>{

    public static string REPORT = "02";
    public static string FISCAL = "03";
    public static string SEARCH = "04";

    public static SpecialFieldType GetByCode(string code) {
        return Table.Where(c => c.Code == code).SingleOrDefault();
    }

    }
}
