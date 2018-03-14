using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TemporaryPolicy
/// </summary>
/// 
namespace Broker.DataAccess
{
    public partial class TemporaryPolicy:EntityBase<TemporaryPolicy>
    {

        public static TemporaryPolicy GetByPolicyNumber(string policyNumber)
        {
            return Table.Where(p => p.PolicyNumber == policyNumber).SingleOrDefault();
        }

    }
}
