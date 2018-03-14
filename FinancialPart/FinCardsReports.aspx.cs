using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.ReportControllers;
using Broker.DataAccess;

public partial class FinancialPart_FinCardsReports : AuthenticationPage {
    protected void Page_Load(object sender, EventArgs e) {
        if (!Page.IsPostBack) {
            BindTree();
        }
    }
    protected void btnReport_Click(object sender, EventArgs e) {
        DateTime fromDate = Convert.ToDateTime(tbFromDate.Text);
        DateTime toDate = Convert.ToDateTime(tbToDate.Text);
        List<int> lstBranches = new List<int>();
        foreach (ListItem li in cblBranches.Items) {
            if (li.Selected) {
                if (li.Value != "All") {
                    lstBranches.Add(Convert.ToInt32(li.Value));
                }
            }
        }
        if (lstBranches.Count == 0) {
            RegisterStartupScript("myAlert", "<script>alert('Немате избрано филијала!')</script>");
            return;
        }
        List<int> lstInsuranceCompanies = new List<int>();
        foreach (ListItem li in cblInsuranceCompanies.Items) {
            if (li.Selected) {
                if (li.Value != "All") {
                    lstInsuranceCompanies.Add(Convert.ToInt32(li.Value));
                }
            }
        }
        if (lstInsuranceCompanies.Count == 0) {
            RegisterStartupScript("myAlert", "<script>alert('Немате избрано осигурителна компанија!')</script>");
            return;
        }
        List<int> lstInsuranceSubTypes = new List<int>();
        //foreach (ListItem li in cblInsuranceSubTypes.Items) {
        //    if (li.Selected) {
        //        if (li.Value != "All") {
        //            lstInsuranceSubTypes.Add(Convert.ToInt32(li.Value));
        //        }
        //    }
        //}
        foreach (TreeNode tn in tvInsuranceTypes.Nodes) {
            foreach (TreeNode tsn in tn.ChildNodes) {
                if (tsn.Checked) {
                    lstInsuranceSubTypes.Add(Convert.ToInt32(tsn.Value));
                }
            }
        }
        if (lstInsuranceSubTypes.Count == 0) {
            RegisterStartupScript("myAlert", "<script>alert('Немате избрано подкласа на осигурување!')</script>");
            return;
        }
        List<int> lstUsers = new List<int>();
        foreach (ListItem li in cblUsers.Items) {
            if (li.Selected) {
                if (li.Value != "All") {
                    lstUsers.Add(Convert.ToInt32(li.Value));
                }
            }
        }
        if (lstUsers.Count == 0) {
            RegisterStartupScript("myAlert", "<script>alert('Немате избрано корисник!')</script>");
            return;
        }
        List<int> lstMarketingAgents = new List<int>();
        foreach (ListItem li in cblUsers.Items) {
            if (li.Selected) {
                if (li.Value != "All" && li.Value != "NoMarketingAgents" && li.Value != "AllMarketingAgents") {
                    lstMarketingAgents.Add(Convert.ToInt32(li.Value));
                }
            }
        }
        if (rblFinCardType.SelectedValue == "FinCardByApplicationDate") {
            PrintFinCardController.PrintFinCardByApplicationDateReport(fromDate, toDate, lstBranches,
            lstInsuranceCompanies, lstInsuranceSubTypes, lstUsers, lstMarketingAgents);
        } else if (rblFinCardType.SelectedValue == "FinCardByPaidDates") {
            PrintFinCardController.PrintFinCardByPaidDates(fromDate, toDate, lstBranches,
            lstInsuranceCompanies, lstInsuranceSubTypes, lstUsers, lstMarketingAgents);
        }

    }
    protected void cblBranches_SelectedIndexChanged(object sender, EventArgs e) {
        string[] control = Request.Form.Get("__EVENTTARGET").Split('$');
        int idx = control.Length - 1;
        string sel = cblBranches.Items[Int32.Parse(control[idx])].Value;
        if (sel == "All") {
            if (cblBranches.Items[0].Selected) {
                foreach (ListItem li in cblBranches.Items) {
                    li.Selected = true;
                }
            } else {
                foreach (ListItem li in cblBranches.Items) {
                    li.Selected = false;
                }
            }
        }
    }
    protected void cblInsuranceCompanies_SelectedIndexChanged(object sender, EventArgs e) {
        string[] control = Request.Form.Get("__EVENTTARGET").Split('$');
        int idx = control.Length - 1;
        string sel = cblInsuranceCompanies.Items[Int32.Parse(control[idx])].Value;
        if (sel == "All") {
            if (cblInsuranceCompanies.Items[0].Selected) {
                foreach (ListItem li in cblInsuranceCompanies.Items) {
                    li.Selected = true;
                }
            } else {
                foreach (ListItem li in cblInsuranceCompanies.Items) {
                    li.Selected = false;
                }
            }
        }
    }
    //protected void cblInsuranceSubTypes_SelectedIndexChanged(object sender, EventArgs e) {
    //    string[] control = Request.Form.Get("__EVENTTARGET").Split('$');
    //    int idx = control.Length - 1;
    //    string sel = cblInsuranceSubTypes.Items[Int32.Parse(control[idx])].Value;
    //    if (sel == "All") {
    //        if (cblInsuranceSubTypes.Items[0].Selected) {
    //            foreach (ListItem li in cblInsuranceSubTypes.Items) {
    //                li.Selected = true;
    //            }
    //        } else {
    //            foreach (ListItem li in cblInsuranceSubTypes.Items) {
    //                li.Selected = false;
    //            }
    //        }
    //    }
    //}
    protected void cblUsers_SelectedIndexChanged(object sender, EventArgs e) {
        string[] control = Request.Form.Get("__EVENTTARGET").Split('$');
        int idx = control.Length - 1;
        string sel = cblUsers.Items[Int32.Parse(control[idx])].Value;
        if (sel == "All") {
            if (cblUsers.Items[0].Selected) {
                foreach (ListItem li in cblUsers.Items) {
                    li.Selected = true;
                }
            } else {
                foreach (ListItem li in cblUsers.Items) {
                    li.Selected = false;
                }
            }
        }
    }
    protected void cblMarketingAgents_SelectedIndexChanged(object sender, EventArgs e) {

    }
   
    void BindTree() {
        List<InsuranceType> lstInsuranceTypes = InsuranceType.Table.Where(c => c.IsForLive == false).OrderBy(c => c.Code).ToList();
        foreach (InsuranceType it in lstInsuranceTypes) {
            TreeNode tnit = new TreeNode(it.ShortName, it.ID.ToString());
            List<InsuranceSubType> lstInsuranceSubTypes = InsuranceSubType.GetByInsuranceType(it.ID);
            foreach (InsuranceSubType ist in lstInsuranceSubTypes) {
                TreeNode tnist = new TreeNode(ist.ShortDescription, ist.ID.ToString());
                tnit.ChildNodes.Add(tnist);
            }
            tvInsuranceTypes.Nodes.Add(tnit);
        }
    }
    protected void cbAllInsuranceSubTypes_CheckedChanged(object sender, EventArgs e) {
        foreach (TreeNode tn in tvInsuranceTypes.Nodes) {
            tn.Checked = cbAllInsuranceSubTypes.Checked;
            foreach (TreeNode tsn in tn.ChildNodes) {
                tsn.Checked = cbAllInsuranceSubTypes.Checked;
            }
        }
    }
}
