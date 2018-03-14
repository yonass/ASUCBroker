using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Accreditation
/// </summary>
namespace Broker.DataAccess
{
    public partial class Accreditation : EntityBase<Accreditation>
    {
        public static List<Accreditation> GetByClient(int clientID)
        {
            return Table.Where(a => a.ClientID == clientID).ToList();
        }
    }
}

