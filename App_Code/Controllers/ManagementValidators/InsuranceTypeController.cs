using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for InsuranceTypeController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
public class InsuranceTypeController
{
    public static void ValidateInsertCode(string code) {
        InsuranceType insuranceType = new InsuranceType();
        insuranceType.Code = code;
        insuranceType.TestBeforeInsert();
    }

    public static void ValidateUpdateCode(int codeID, string code) {
        InsuranceType insuranceType = new InsuranceType();
        insuranceType.Code = code;
        insuranceType.ID = codeID;
        insuranceType.TestBeforeUpdate();
    }
}
}