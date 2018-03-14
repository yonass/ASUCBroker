using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;

public partial class BROKERAdmin_TestBrokerage : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

    }
    protected void Button1_Click(object sender, EventArgs e) {
        decimal number = Brokerage.GetBrokerageForBrokerage(22);

       }
    }

