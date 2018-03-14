using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for MunicipalityController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
public class MunicipalityController
{
    public static void ValidateInsertCode(string code) {
        Broker.DataAccess.Municipality municipality = new Broker.DataAccess.Municipality();
        municipality.Code = code;
        municipality.TestBeforeInsert();
    }

    public static void ValidateUpdateCode(int codeID, string code) {
        Broker.DataAccess.Municipality municipality = new Broker.DataAccess.Municipality();
        municipality.Code = code;
        municipality.ID = codeID;
        municipality.TestBeforeUpdate();
    }
}
}