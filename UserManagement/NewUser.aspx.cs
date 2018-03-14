using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Broker.Controllers.EmployeeManagement;

public partial class UserManagement_NewUser : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack) {
            MultiView1.SetActiveView(View1);
        }
    }

    /// <summary>
    /// metod za vnesuvanje na osnovni podatoci za korisnikot.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNext_Click1(object sender, EventArgs e) {
        string username = UsernameTextBox.Text;
        string EMBG = EMBGTextBox.Text;
        //prverka dali e validene UserName na korisnikot.
        try {
            EmployeeController.ValidateUserInfo(username);
        } catch (Exception ex) {
            ValidationUtility.ShowValidationErrors(this, ex);
            return;
        }
        //proverka dali e validen EMBG.
        try {
            EmployeeController.ValidateUserEMBG(EMBG);
        } catch (Exception ex) {
            ValidationUtility.ShowValidationErrors(this, ex);
            return;
        }
        //prverka za selektirana uloga na korisnikot na sistemot.
        try {
            string selectedRole = RolesList.SelectedItem.Text;
            MultiView1.SetActiveView(View2);
        } catch (Exception ex) {
            lblError.Visible = true;
            lblError.Text = "Изберете улога на новиот корисник на системот!";
        }
    }
    /// <summary>
    /// metod za vnesuvanje na nov korisnik vo sistemot.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnInsert_Click(object sender, EventArgs e) {

        ////setiranje na osnovnite podatoci za korisnikot.
        string roleName = RolesList.SelectedItem.Text;
        string username = UsernameTextBox.Text.Trim();
        string fullname = FullNameTextBox.Text.Trim();
        string EMBG = EMBGTextBox.Text.Trim();
        string password = passwordTextBox.Text.Trim();
        string Address = AddressTextBox.Text.Trim();
        string Email = EmailTextBox.Text.Trim();
        string phone = PhoneNmberTextBox.Text.Trim();

        string role = RolesList.SelectedItem.Text;
        int roleId = Convert.ToInt32(RolesList.SelectedValue);
        int branchID = Convert.ToInt32(BranchesList.SelectedValue);

        string cert = null;
        ////setiranje na ulogata na korisnikot na sisitemot.
        if (roleName == RolesInfo.Broker) {
            try {
                cert = EmployeeController.InsertBroker(roleId, username, fullname, branchID, password, Address, EMBG, phone, Email);
            } catch (Exception ex) {
                SetVisibleComponents();
                ValidationUtility.ShowValidationErrors(this, ex);
                return;
            }
        }

        if (roleName == RolesInfo.BROKERAdmin) {
            try {
                cert = EmployeeController.InsertBROKERAdmin(roleId, username, fullname, branchID, password, Address, EMBG, phone, Email);
            } catch (Exception ex) {
                SetVisibleComponents();
                ValidationUtility.ShowValidationErrors(this, ex);
                return;
            }
        }


        if (roleName == RolesInfo.SEAdmin) {
            try {
                cert = EmployeeController.InsertSEAdmin(roleId, username, fullname, branchID, password, Address, EMBG, phone, Email);
            } catch (Exception ex) {
                SetVisibleComponents();
                ValidationUtility.ShowValidationErrors(this, ex);
                return;
            }
        }
        
        


        //EmployeeController.SendCertificate(cert);
        lblOK.Visible = true;
        lblOK.Text = "Успешно е внесен новиот корисник во системот";

    }



    private void SetVisibleComponents() {
        // TODO Za broker visibleCompnents
        //   BrokerCompanyList.Visible = true;
       
        MultiView1.SetActiveView(View1);
    }

    public List<int> selectedChildren() {
        List<int> list = new List<int>();
        //foreach (ListItem item in BranchSEAdministrators.Items) {
        //    if (item.Selected) {
        //        list.Add(Convert.ToInt32(item.Value));
        //    }
        //}
        return list;
    }
    /// <summary>
    /// metod za generiranje na password.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Generate_click(object sender, EventArgs e) {
        Random lRandom = new Random();
        int number = lRandom.Next(7, 10);
        int ascii = Convert.ToInt32('1');
        //int ascii1 = Convert.ToInt32('a');
        //int ascii2 = Convert.ToInt32('1');
        string s = string.Empty;
        for (int i = 0; i < number; i++) {
            int k = lRandom.Next(52);
            char c = Convert.ToChar(ascii + k);
            s += c.ToString();
        }
        passwordTextBox.Text = s;
        passwordTextBox.ReadOnly = true;
    }
}

