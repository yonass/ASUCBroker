using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Controllers;

/// <summary>
/// Summary description for AccountFacture
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class AccountFacture : EntityBase<AccountFacture> {

        public static AccountFacture GetByFactureNumber(string factureNumber) {
            return Table.Where(c => c.FactureNumber == factureNumber).SingleOrDefault();
        }

        public static void DiscardFacture(int id) {

            AccountFacture f = Get(id);
            f.Discard = true;
            AccountFacture.Table.Context.SubmitChanges();
            IEnumerable<AccountFactureItemPolicyItem> afipiList = AccountFactureItemPolicyItem.GetByAccountFacture(id);
            foreach (AccountFactureItemPolicyItem afipi in afipiList) {

                PolicyItem pi = PolicyItem.Get(afipi.PolicyItemID);
                pi.IsAccountFactured = false;
                PolicyItem.Table.Context.SubmitChanges();
            }
        }

        public static AccountFacture Generate(string factureNumber, DateTime startDate, DateTime endDate, InsuranceCompany ic, User u, DateTime dateOfPayment, List<PolicyItem> policiesToInclude, Branch branch) {
            decimal brokerageValue = 0;
            decimal totalSum = 0;
            decimal currentBrokerageValue = 0;
            Dictionary<int, decimal[]> dictionary = new Dictionary<int, decimal[]>();
            Dictionary<int, List<int>> dictionaryPolList = new Dictionary<int, List<int>>();
            foreach (PolicyItem policyItem in policiesToInclude) {
                if (policyItem.Policy.PacketID != null) {
                    if (policyItem.Policy.Client.IsLaw) {
                        currentBrokerageValue = policyItem.PremiumValue * policyItem.PacketsInsuranceSubType.BrokeragePecentageForLaws / 100;

                    } else {
                        currentBrokerageValue = policyItem.PremiumValue * policyItem.PacketsInsuranceSubType.BrokeragePecentageForPrivates / 100;
                    }
                } else {
                    if (policyItem.Policy.Client.IsLaw) {
                        currentBrokerageValue = policyItem.PremiumValue * policyItem.Brokerage.PercentageForLaws / 100;
                    } else {
                        currentBrokerageValue = policyItem.PremiumValue * policyItem.Brokerage.PercentageForPrivates / 100;
                    }
                }
                brokerageValue += currentBrokerageValue;
                if (dictionary.ContainsKey(policyItem.InsuranceSubTypeID)) {
                    dictionary[policyItem.InsuranceSubTypeID][0] += currentBrokerageValue;
                    dictionary[policyItem.InsuranceSubTypeID][1] += policyItem.PremiumValue;
                    //dictionary[policyItem.InsuranceSubTypeID][2] += 1;
                    dictionaryPolList[policyItem.InsuranceSubTypeID].Add(policyItem.ID);
                } else {
                    decimal[] newValues = { currentBrokerageValue, policyItem.PremiumValue/*, 1*/};
                    dictionary.Add(policyItem.InsuranceSubTypeID, newValues);
                    List<int> cL = new List<int>();
                    cL.Add(policyItem.ID);
                    dictionaryPolList.Add(policyItem.InsuranceSubTypeID, cL);
                }
                totalSum += policyItem.PremiumValue;
            }
            
            
            //da se zemat vo predvid i zaokruzuvanjata            
            decimal calcBrokValue = 0;
            foreach (KeyValuePair<int, decimal[]> kvp in dictionary) {
                calcBrokValue += ScaleController.Scale5(kvp.Value[0]);
            }


            if (policiesToInclude.Count > 0) {
                AccountFacture f = new AccountFacture();
                //f.BrokerageValue = ScaleController.Scale5(brokerageValue);
                f.BrokerageValue = calcBrokValue;
                f.TotalCost = totalSum;
                int clientID = 1;
                Client client = Client.GetByEmbg(ic.EMBG);
                if (client != null) {
                    clientID = client.ID;
                } else {
                    Client newClient = new Client();
                    newClient.Address = ic.Address;
                    newClient.EMail = string.Empty;
                    newClient.EMBG = ic.EMBG;
                    newClient.Fax = string.Empty;
                    newClient.IsInsuranceCompany = true;
                    newClient.Mobile = string.Empty;
                    newClient.Name = ic.Name;
                    newClient.Phone = string.Empty;
                    newClient.PlaceID = Place.Table.First().ID;
                    newClient.Insert();
                    clientID = newClient.ID;
                }
                f.ClientID = clientID;
                f.DateOfCreation = DateTime.Today;
                f.DocumentSubTypeID = DocumentSubType.GetByCode(DocumentSubType.FAKTURA_PROVIZIJA).ID;
                f.FactureNumber = factureNumber; //CodeGenerator.GenerateFactureNumber();
                f.FromDate = startDate;
                f.ToDate = endDate;
                f.BranchID = branch.ID;
                f.StatusID = Statuse.GetByCode(Statuse.NEPLATENA_IZLEZNA_FAKTURA_ZA_PROVIZIJA).ID;
                f.DateOfPayment = dateOfPayment;
                f.Discard = false;
                f.InsuranceCompanyID = ic.ID;
                f.UserID = u.ID;
                f.IsAccountBooked = false;
                f.Insert();

                int ordinalNumber = 1;
                foreach (KeyValuePair<int, decimal[]> kvPair in dictionary) {
                    AccountFactureItem fi = new AccountFactureItem();
                    fi.FactureID = f.ID;
                    fi.BrokerageValue = ScaleController.Scale5(kvPair.Value[0]);
                    fi.PremiumValue = kvPair.Value[1];
                    fi.Number = ordinalNumber;
                    fi.InsuranceSubTypeID = kvPair.Key;
                    fi.Count = dictionaryPolList[kvPair.Key].Count;
                    fi.Description = InsuranceSubType.Get(kvPair.Key).ShortDescription;
                    ordinalNumber++;
                    fi.Insert();
                    AccountFactureItemPolicyItem.InsertForFacture(fi.ID, dictionaryPolList[kvPair.Key]);
                }
                PolicyItem.SetIsAccountFactured(policiesToInclude);
                return f;
            }
            return null;
        }
    }
}
