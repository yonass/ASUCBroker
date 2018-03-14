using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for PlaceController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
public class PlaceController
{
    public static void ValidateInsertCode(string code) {
        Broker.DataAccess.Place place = new Broker.DataAccess.Place();
        place.Code = code;
        place.TestBeforeInsert();
    }

    public static void ValidateUpdateCode(int codeID, string code) {
        Broker.DataAccess.Place place = new Broker.DataAccess.Place();
        place.Code = code;
        place.ID = codeID;
        place.TestBeforeUpdate();
    }
}
}