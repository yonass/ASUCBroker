using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DistributionDocumentType
/// </summary>
namespace Broker.DataAccess{
public partial class DistributionDocumentType:EntityBase<DistributionDocumentType>{

    public static string BLANKO = "02";

    public static DistributionDocumentType GetByCode(string code) {
        return Table.Where(c => c.Code==code).SingleOrDefault();
    }




    }
}
