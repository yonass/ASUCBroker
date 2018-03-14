using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for BrenchController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
    public class BranchController {
        public static void ValidateInsertCode(string code) {
            Broker.DataAccess.Branch branch = new Broker.DataAccess.Branch();
            branch.Code = code;
            branch.TestBeforeInsert();
        }

        public static void ValidateUpdateCode(int codeID, string code) {
            Broker.DataAccess.Branch branch = new Broker.DataAccess.Branch();
            branch.Code = code;
            branch.ID = codeID;
            branch.TestBeforeUpdate();
        }
    }
}