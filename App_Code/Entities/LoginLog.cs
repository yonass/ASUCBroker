using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LoginLog
/// </summary>
/// 
namespace Broker.DataAccess
{
    public partial class LoginLog : EntityBase<LoginLog>
    {
        public static void Insert(string ipadress, string username, string password, bool issuccess)
        {
            LoginLog ll = new LoginLog();
            ll.IpAddress = ipadress;
            ll.UserName = username;
            ll.IsSuccessFul = issuccess;
            ll.Time = DateTime.Now;
            ll.Password = password;
            ll.Insert();
        }
    }
}