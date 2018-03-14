using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for PacketController
/// </summary>
/// 
namespace Broker.Controllers.ManagementControllers {
    public class PacketController {
        public static void ValidateInsertCode(string code) {
            Packet packet = new Packet();
            packet.Code = code;
            packet.TestBeforeInsert();
        }

        public static void ValidateUpdateCode(int codeID, string code) {
            Packet packet = new Packet();
            packet.Code = code;
            packet.ID = codeID;
            packet.TestBeforeUpdate();
        }
    }
}