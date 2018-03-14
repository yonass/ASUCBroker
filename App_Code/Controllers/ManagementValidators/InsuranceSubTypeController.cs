using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for InsuranceSubTypeController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
public class InsuranceSubTypeController
{
    public static void ValidateInsertCode(string code) {
        InsuranceSubType insuranceSubType = new InsuranceSubType();
        insuranceSubType.Code = code;
        insuranceSubType.TestBeforeInsert();
    }

    public static void ValidateUpdateCode(int codeID, string code) {
        InsuranceSubType insuranceSubType = new InsuranceSubType();
        insuranceSubType.Code = code;
        insuranceSubType.ID = codeID;
        insuranceSubType.TestBeforeUpdate();
    }
}
}