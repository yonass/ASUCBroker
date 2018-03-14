using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Slip
/// </summary>
namespace Broker.DataAccess{
public partial class Slip:EntityBase<Slip>
    {

    public static List<Slip> GetForPeriodAndBank(DateTime startDate, DateTime endDate, int bankID) {
        return Table.Where(c => c.BankID == bankID && c.Date <= endDate && c.Date >= startDate).ToList();
    }

    }
}
