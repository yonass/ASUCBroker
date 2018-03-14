using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for InsuranceCompanyController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
public class InsuranceCompanyController
{
    public static void ValidateInsertCode(string code) {
        InsuranceCompany insurancecompany = new InsuranceCompany();
        insurancecompany.Code = code;
        insurancecompany.TestBeforeInsert();
    }

    public static void ValidateUpdateCode(int codeID, string code) {
        InsuranceCompany insurancecompany = new InsuranceCompany();
        insurancecompany.Code = code;
        insurancecompany.ID = codeID;
        insurancecompany.TestBeforeUpdate();
    }
}
}