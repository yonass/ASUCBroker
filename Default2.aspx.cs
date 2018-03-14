using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.DataAccess;

public partial class Default2 : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

    }
    protected void Button1_Click(object sender, EventArgs e) {
        //insert new facture
        int insuranceCompanyID = 14;
        //get all policies in period
        DateTime fromDate = new DateTime(2012, 1, 1);
        DateTime toDate = new DateTime(2012, 12, 31);
        List<LifePolicy> listPolicies = LifePolicyBrokerage.Table.Where(c => c.FromDate.Date >= fromDate.Date && c.FromDate.Date <= toDate.Date && c.IsFactured == false).Select(c => c.LifePolicy).ToList();
        //calculate brokerage value for current year
        

        Facture f = new Facture();
        
    }
}
