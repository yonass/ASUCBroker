using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Controllers;
/// <summary>
/// Summary description for Facture
/// </summary>
/// 
namespace Broker.DataAccess {
    public partial class Facture : EntityBase<Facture> {

        public static bool ExistFactureNumberInFactures(string factureNumber) {
            return (Table.Where(f => f.FactureNumber == factureNumber).SingleOrDefault() != null);
        }

        public static bool ExistOtherFactureNumber(int factureID, string factureNumber) {
            return (Table.Where(f => f.FactureNumber == factureNumber && f.ID != factureID).SingleOrDefault() != null);
        }


        public override void Validate() {

        }

        public void TestBeforeInsert() {
            if (Facture.ExistFactureNumberInFactures(this.FactureNumber)) {
                ValidationErrors.Add("FACTURENUMBER_INSERT_EXISTS", this.FactureNumber + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public void TestBeforeUpdate() {
            if (Facture.ExistOtherFactureNumber(this.ID, this.FactureNumber)) {
                ValidationErrors.Add("FACTURENUMBER_UPDATE_EXISTS", this.FactureNumber + " е веќе зафатено!");
            }
            this.PerformCustomValidation();
        }

        public static Facture GetByNumber(string number) {
            return Table.Where(f => f.FactureNumber == number).SingleOrDefault();
        }

        public static Facture Generate(DateTime startDate, DateTime endDate, InsuranceCompany ic, User u, DateTime dateOfPayment, List<PolicyItem> policiesToRemove) {
            List<Payment> paymentList = Payment.GetForFactureByDateAndInsuranceCompany(endDate, ic.ID);
            decimal brokerageValue = 0;
            decimal totalSum = 0;
            decimal currentBrokerageValue = 0;
            Dictionary<int, decimal[]> dictionary = new Dictionary<int, decimal[]>();
            List<PolicyItem> policyItemList = new List<PolicyItem>();

            foreach (Payment p in paymentList) {
                //Koeficient so cel da se presmetuva brokerazata na polisiranata premija
                decimal k = 1;
                if (p.Rate.PolicyItem.PremiumValue > 0) {
                    k = p.Rate.PolicyItem.RealPremiumValue / p.Rate.PolicyItem.PremiumValue;
                }
                PolicyItem policyItem = p.Rate.PolicyItem;
                if (!policiesToRemove.Contains(policyItem)) {
                    if (!policyItemList.Contains(policyItem)) {
                        policyItemList.Add(policyItem);
                    }
                    if (policyItem.Policy.PacketID != null) {
                        if (policyItem.Policy.Client.IsLaw) {
                            currentBrokerageValue = k * p.Value * policyItem.PacketsInsuranceSubType.BrokeragePecentageForLaws / 100;

                        } else {
                            currentBrokerageValue = k * p.Value * policyItem.PacketsInsuranceSubType.BrokeragePecentageForPrivates / 100;
                        }
                    } else {
                        if (policyItem.Policy.Client.IsLaw) {
                            currentBrokerageValue = k * p.Value * policyItem.Brokerage.PercentageForLaws / 100;
                        } else {
                            currentBrokerageValue = k * p.Value * policyItem.Brokerage.PercentageForPrivates / 100;
                        }
                    }
                    brokerageValue += currentBrokerageValue;
                    if (dictionary.ContainsKey(policyItem.InsuranceSubTypeID)) {
                        dictionary[policyItem.InsuranceSubTypeID][0] += currentBrokerageValue;
                        dictionary[policyItem.InsuranceSubTypeID][1] += p.Value;
                        // dictionary[policyItem.InsuranceSubTypeID][2] += 1;
                    } else {
                        decimal[] newValues = { currentBrokerageValue, p.Value/*, 1 */};
                        dictionary.Add(policyItem.InsuranceSubTypeID, newValues);
                    }
                    totalSum += p.Value;
                }
            }
            

            //da se zemat vo predvid i zaokruzuvanjata            
            decimal calcBrokValue = 0;
            foreach (KeyValuePair<int, decimal[]> kvp in dictionary) {
                calcBrokValue += ScaleController.Scale5(kvp.Value[0]);
            }


            if (policyItemList.Count > 0) {
                Facture f = new Facture();
                //f.BrokerageValue = ScaleController.Scale5(brokerageValue);
                f.BrokerageValue = calcBrokValue;
                f.TotalCost = totalSum;
                int clientID = 1;
                Client client = Client.GetByEmbg(ic.EMBG);
                if (client != null)
                {
                    clientID = client.ID;
                } else
                {
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
                //f.FactureNumber = CodeGenerator.GenerateFactureNumber();
                f.FactureNumber = CodeGenerator.GenerateFactureNumberForBrokerageFactures();
                f.FromDate = startDate;
                f.ToDate = endDate;
                f.StatusID = Statuse.GetByCode(Statuse.NEPLATENA_IZLEZNA_FAKTURA_ZA_PROVIZIJA).ID;
                f.DateOfPayment = dateOfPayment;
                f.Discard = false;
                f.InsuranceCompanyID = ic.ID;
                f.UserID = u.ID;
                f.Insert();

                //List<FactureItem> listFactureItems = new List<FactureItem>();
                int ordinalNumber = 1;
                foreach (KeyValuePair<int, decimal[]> kvPair in dictionary) {
                    FactureItem fi = new FactureItem();
                    fi.Facture = f;
                    fi.BrokerageValue = ScaleController.Scale5(kvPair.Value[0]);
                    fi.PremiumValue = kvPair.Value[1];
                    fi.Number = ordinalNumber;
                    fi.InsuranceSubTypeID = kvPair.Key;
                    List<PolicyItem> subList = policyItemList.Where(c => c.InsuranceSubTypeID == kvPair.Key).ToList();
                    fi.Count = subList.Count;
                    fi.Description = InsuranceSubType.Get(kvPair.Key).ShortDescription;
                    ordinalNumber++;
                    f.FactureItems.Add(fi);
                    fi.Insert();
                    PolicyItemFactureItem.InsertForFacture(fi, subList);
                }

                Payment.SetFactured(paymentList, f, policiesToRemove);
                return f;
            }
            return null;
        }

        public static Facture GenerateForLife(DateTime startDate, DateTime endDate, InsuranceCompany ic, User u, DateTime dateOfPayment, List<LifePolicy> policiesToRemove) {
            List<LifePolicyBrokerage> paymentList = LifePolicyBrokerage.Table.Where(c => c.LifePolicy.InsuranceCompanyID == ic.ID && c.LifePolicy.Discard == false
                && c.FromDate.Date <= endDate.Date).ToList();
            decimal brokerageValue = 0;
            decimal totalSum = 0;
            Dictionary<int, decimal[]> dictionary = new Dictionary<int, decimal[]>();
            List<LifePolicy> policyItemList = new List<LifePolicy>();

            foreach (LifePolicyBrokerage p in paymentList) {
                if (!policiesToRemove.Contains(p.LifePolicy)) {
                    if (!policyItemList.Contains(p.LifePolicy)) {
                        policyItemList.Add(p.LifePolicy);
                    }
                    brokerageValue += p.Value;
                    if (dictionary.ContainsKey(p.LifePolicy.InsuranceSubTypeID)) {
                        dictionary[p.LifePolicy.InsuranceSubTypeID][0] += brokerageValue;
                        dictionary[p.LifePolicy.InsuranceSubTypeID][1] += p.LifePolicy.TotalPremumValue;
                    } else {
                        decimal[] newValues = { brokerageValue, p.LifePolicy.TotalPremumValue};
                        dictionary.Add(p.LifePolicy.InsuranceSubTypeID, newValues);
                    }
                    totalSum += p.LifePolicy.TotalPremumValue;
                }
            }


            //da se zemat vo predvid i zaokruzuvanjata            
            decimal calcBrokValue = 0;
            foreach (KeyValuePair<int, decimal[]> kvp in dictionary) {
                calcBrokValue += ScaleController.Scale5(kvp.Value[0]);
            }


            if (policyItemList.Count > 0) {
                Facture f = new Facture();
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
                f.DocumentSubTypeID = DocumentSubType.GetByCode(DocumentSubType.IZLEZNA_FAKTURA_ZA_PROVZIJA_ZA_ZIVOTNO_OSUGURUVANjE).ID;
                f.FactureNumber = CodeGenerator.GenerateFactureNumber();
                f.FromDate = startDate;
                f.ToDate = endDate;
                f.StatusID = Statuse.GetByCode(Statuse.NEPLATENA_IZLEZNA_FAKTURA_ZA_PROVIZIJA_ZA_ZIVOTNO).ID;
                f.DateOfPayment = dateOfPayment;
                f.Discard = false;
                f.InsuranceCompanyID = ic.ID;
                f.UserID = u.ID;
                f.Insert();

                //List<FactureItem> listFactureItems = new List<FactureItem>();
                int ordinalNumber = 1;
                foreach (KeyValuePair<int, decimal[]> kvPair in dictionary) {
                    FactureItem fi = new FactureItem();
                    fi.Facture = f;
                    fi.BrokerageValue = ScaleController.Scale5(kvPair.Value[0]);
                    fi.PremiumValue = kvPair.Value[1];
                    fi.Number = ordinalNumber;
                    fi.InsuranceSubTypeID = kvPair.Key;
                    List<LifePolicy> subList = policyItemList.Where(c => c.InsuranceSubTypeID == kvPair.Key).ToList();
                    fi.Count = subList.Count;
                    fi.Description = InsuranceSubType.Get(kvPair.Key).ShortDescription;
                    ordinalNumber++;
                    f.FactureItems.Add(fi);
                    fi.Insert();
                    LifePolicyFactureItem.InsertForFacture(fi, subList);
                }

                LifePolicyBrokerage.SetFactured(paymentList, f, policiesToRemove);
                return f;
            }
            return null;
        }


        public static List<FactureItem> GenerateFactureItemsForSinglePolicy(List<PolicyItem> lPI, Facture f) {
            List<FactureItem> lFI = new List<FactureItem>();
            int i = 1;
            foreach (PolicyItem pi in lPI) {
                FactureItem fi = new FactureItem();
                fi.PremiumValue = pi.PremiumValue;
                fi.FactureID = f.ID;
                fi.Count = 1;
                fi.Description = "Полиса бр." + pi.PolicyNumber;
                fi.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                fi.Number = i;
                lFI.Add(fi);
                i++;
            }
            return lFI;
        }

        public static Facture GenerateForSinglePolicy(Policy p)
        {

            Facture f = new Facture();
            List<PolicyItem> pi = PolicyItem.GetByPolicy(p.ID);
            if (pi.Count == 1)
            {
                if (pi[0].RateDealInsuranceSubTypeID != null)
                {
                    f.ClientID = pi[0].RateDealInsuranceSubType.RateDeal.ClientID;
                }
                else
                {
                    f.ClientID = p.ClientID;
                }
            }
            else
            {
                f.ClientID = p.ClientID;
            }
            //f.DateOfCreation = p.ApplicationDate;
	    f.DateOfCreation = DateTime.Today;
            f.DocumentSubTypeID = DocumentSubType.GetByCode(DocumentSubType.FAKTURA_KLIENT).ID;
            f.FactureNumber = CodeGenerator.GenerateFactureNumber();
            f.FromDate = DateTime.Today;
            f.ToDate = DateTime.Today;
            f.TotalCost = p.TotalSum;
            f.UserID = p.UserID;
            List<Payment> lstPayments = Payment.GetByPolicyItemID(pi[0].ID);
            if (lstPayments.Count > 0)
            {
                f.StatusID = Statuse.GetByCode(Statuse.DELUMNO_PLATENA_IZLEZNA_FAKTURA_KON_KLIENT).ID;
            }
            else
            {
                f.StatusID = Statuse.GetByCode(Statuse.NEPLATENA_IZLEZNA_FAKTURA_KON_KLIENT).ID;
            }
            f.DateOfPayment = DateTime.Today.Date.AddDays(int.Parse(Parameter.GetByCode(Parameter.DENOVI_ZA_NAPLATA_NA_FAKTURA).Value));
            List<PolicyItem> lpi = PolicyItem.GetByPolicy(p.ID);
            List<Rate> lstRates = Rate.GetByPolicyItemIDExtend(lpi.First().ID);
            if (lstRates.Count > 1)
            {
                f.DateOfPayment = lstRates.Last().Date.AddDays(1);
            }
            f.InsuranceCompanyID = p.InsuranceCompanyID;
            return f;
        }

        public static void InsertFactureAndFactureItems(Facture f, List<FactureItem> lFI) {
            Facture.Insert(f);
            foreach (FactureItem fi in lFI) {
                FactureItem.Insert(fi);
            }
        }

        public static void InsertFactureItems(List<FactureItem> lFI) {
            foreach (FactureItem fi in lFI) {
                FactureItem.Insert(fi);
            }
        }

        public static void DiscardFacture(int id) {
            Facture f = Get(id);
            f.Discard = true;
            Facture.Table.Context.SubmitChanges();
            IEnumerable<FacturePayment> fpIenum = FacturePayment.GetByFacture(id);
            foreach (FacturePayment fp in fpIenum) {
                Payment p = Payment.Get(fp.PaymentID);
                p.IsFactured = false;
                Payment.Table.Context.SubmitChanges();
            }
            List<PolicyItemFactureItem> lstPIFI = PolicyItemFactureItem.GetByFactureID(id);
            foreach (PolicyItemFactureItem pifi in lstPIFI) {
                PolicyItem pi = PolicyItem.Get(pifi.PolicyItemID);
                pi.IsFactureCreated = false;
                PolicyItem.Table.Context.SubmitChanges();
            }

        }

        public static int CreateGroupFacture(int userID, int clientID, DateTime startDate, DateTime endDate) {
            
            List<PolicyItem> items = PolicyItem.GetForGroupFactures(startDate,endDate,clientID);
            Facture f = new Facture();
            Parameter par = Parameter.GetByCode(Parameter.DENOVI_ZA_NAPLATA_NA_FAKTURA);
            f.BrokerageValue = 0;
            f.ClientID = clientID;
            f.DateOfCreation = DateTime.Today;
            f.DateOfPayment = f.DateOfCreation.AddDays(Convert.ToDouble(par.Value));
            f.Discard = false;
            f.DocumentSubTypeID = DocumentSubType.GetByCode(DocumentSubType.GRUPNA_FAKTURA).ID;
            f.FactureNumber = CodeGenerator.GenerateFactureNumber();
            f.FromDate = startDate;
            f.InsuranceCompanyID = items[0].Policy.InsuranceCompanyID;
            f.StatusID = Statuse.GetByCode(Statuse.NEPLATENA_GRUPNA_FAKTURA_KON_KLIENT).ID;
            f.ToDate = endDate;
            f.TotalCost = 0;
            f.UserID = userID;
            f.Insert();
            
            decimal brokerageValue =0;
            decimal totalValue = 0;
            int orderNumber = 0;

            foreach (PolicyItem pi in items) {
                FactureItem fi = new FactureItem();
                fi.BrokerageValue = pi.BrokerageValue;
                brokerageValue += fi.BrokerageValue;
                fi.Count = 1;
                fi.Description = "Полиса бр. " + pi.PolicyNumber;
                fi.FactureID = f.ID;
                fi.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                fi.Number = ++orderNumber;
                fi.PremiumValue = pi.PremiumValue;
                totalValue += fi.PremiumValue;
                fi.Insert();
                PolicyItemFactureItem pifi = new PolicyItemFactureItem();
                pifi.PolicyItemID = pi.ID;
                pifi.FactureItemID = fi.ID;
                pifi.Insert();
                pi.UpdateFacturedStatus(true);
            }
            f.UpdateValues(totalValue, brokerageValue);
            
            return f.ID;
        }

        public static int CreateGroupFacture(int userID, int clientID, DateTime startDate, DateTime endDate, List<PolicyItem> items) {
            Facture f = new Facture();
            Parameter par = Parameter.GetByCode(Parameter.DENOVI_ZA_NAPLATA_NA_FAKTURA);
            f.BrokerageValue = 0;
            f.ClientID = clientID;
            f.DateOfCreation = DateTime.Today;
            f.DateOfPayment = f.DateOfCreation.AddDays(Convert.ToDouble(par.Value));
            f.Discard = false;
            f.DocumentSubTypeID = DocumentSubType.GetByCode(DocumentSubType.GRUPNA_FAKTURA).ID;
            f.FactureNumber = CodeGenerator.GenerateFactureNumber();
            f.FromDate = startDate;
            f.InsuranceCompanyID = null;
            f.StatusID = Statuse.GetByCode(Statuse.NEPLATENA_IZLEZNA_FAKTURA_KON_KLIENT).ID;
            f.ToDate = endDate;
            f.TotalCost = 0;
            f.UserID = userID;
            f.Insert();

            decimal brokerageValue = 0;
            decimal totalValue = 0;
            int orderNumber = 0;

            foreach (PolicyItem pi in items) {
                FactureItem fi = new FactureItem();
                fi.BrokerageValue = pi.BrokerageValue;
                brokerageValue += fi.BrokerageValue;
                fi.Count = 1;
                fi.Description = "Полиса бр. " + pi.PolicyNumber;
                fi.FactureID = f.ID;
                fi.InsuranceSubTypeID = pi.InsuranceSubTypeID;
                fi.Number = ++orderNumber;
                fi.PremiumValue = pi.PremiumValue;
                totalValue += fi.PremiumValue;
                fi.Insert();
                PolicyItemFactureItem pifi = new PolicyItemFactureItem();
                pifi.PolicyItemID = pi.ID;
                pifi.FactureItemID = fi.ID;
                pifi.Insert();
                pi.UpdateFacturedStatus(true);
            }
            f.UpdateValues(totalValue, brokerageValue);

            return f.ID;
        }

        void UpdateValues(decimal totalValue, decimal brokerageValue) {
            this.BrokerageValue = brokerageValue;
            this.TotalCost = totalValue;
            Table.Context.SubmitChanges();
        }

        public static decimal GetPaidValuesForFacture(Facture f) {
            List<PolicyItemFactureItem> lst = PolicyItemFactureItem.GetByFactureID(f.ID);
            decimal totalPaidValue = 0;
            foreach (PolicyItemFactureItem pifi in lst) {
                decimal paidValue = Payment.GetPaidValueForPolicyItem(pifi.PolicyItemID);
                totalPaidValue += paidValue;
            }
            return totalPaidValue;
        }

        public static decimal GetPaidValuesForFactureExtend(Facture f) {
            List<PolicyItemFactureItem> lst = PolicyItemFactureItem.GetByFactureID(f.ID);
            decimal totalPaidValue = 0;
            foreach (PolicyItemFactureItem pifi in lst) {
                decimal paidValue = Payment.GetPaidValueForPolicyItemExtend(pifi.PolicyItemID);
                totalPaidValue += paidValue;
            }
            return totalPaidValue;
        }

        public static void UpdatePaidStatusForFacture(int policyItemID) {
            Facture f = PolicyItemFactureItem.GetByPolicyItemID(policyItemID);
            if (f != null) {
                decimal paidValuePerFacure = GetPaidValuesForFacture(f);
                Facture facture = Facture.Get(f.ID);
                if (paidValuePerFacure == f.TotalCost) {
                    if (facture.DocumentSubType.Code == DocumentSubType.GRUPNA_FAKTURA) {
                        facture.StatusID = Statuse.GetByCode(Statuse.PLATENA_GRUPNA_FAKTURA_KON_KLIENT).ID;
                    } else if (facture.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT) {
                        facture.StatusID = Statuse.GetByCode(Statuse.PLATENA_IZLEZNA_FAKTURA_KON_KLIENT).ID;
                    }
                } else if (paidValuePerFacure == 0) {
                    if (facture.DocumentSubType.Code == DocumentSubType.GRUPNA_FAKTURA) {
                        facture.StatusID = Statuse.GetByCode(Statuse.NEPLATENA_GRUPNA_FAKTURA_KON_KLIENT).ID;
                    } else if (facture.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT) {
                        facture.StatusID = Statuse.GetByCode(Statuse.NEPLATENA_IZLEZNA_FAKTURA_KON_KLIENT).ID;
                    }
                } else {
                    if (facture.DocumentSubType.Code == DocumentSubType.GRUPNA_FAKTURA) {
                        facture.StatusID = Statuse.GetByCode(Statuse.DELUMNO_PLATENA_GRUPNA_FAKTURA_KON_KLIENT).ID;
                    } else if (facture.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT) {
                        facture.StatusID = Statuse.GetByCode(Statuse.DELUMNO_PLATENA_IZLEZNA_FAKTURA_KON_KLIENT).ID;
                    }
                }
                Facture.Table.Context.SubmitChanges();
            }
        }

        public static void UpdatePaidStatusForFactureExtend(int policyItemID) {
            Facture f = PolicyItemFactureItem.GetByPolicyItemID(policyItemID);
            if (f != null) {
                decimal paidValuePerFacure = GetPaidValuesForFactureExtend(f);
                Facture facture = Facture.Get(f.ID);
                if (paidValuePerFacure == f.TotalCost) {
                    if (facture.DocumentSubType.Code == DocumentSubType.GRUPNA_FAKTURA) {
                        facture.StatusID = Statuse.GetByCode(Statuse.PLATENA_GRUPNA_FAKTURA_KON_KLIENT).ID;
                    } else if (facture.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT) {
                        facture.StatusID = Statuse.GetByCode(Statuse.PLATENA_IZLEZNA_FAKTURA_KON_KLIENT).ID;
                    }
                } else if (paidValuePerFacure == 0) {
                    if (facture.DocumentSubType.Code == DocumentSubType.GRUPNA_FAKTURA) {
                        facture.StatusID = Statuse.GetByCode(Statuse.NEPLATENA_GRUPNA_FAKTURA_KON_KLIENT).ID;
                    } else if (facture.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT) {
                        facture.StatusID = Statuse.GetByCode(Statuse.NEPLATENA_IZLEZNA_FAKTURA_KON_KLIENT).ID;
                    }
                } else {
                    if (facture.DocumentSubType.Code == DocumentSubType.GRUPNA_FAKTURA) {
                        facture.StatusID = Statuse.GetByCode(Statuse.DELUMNO_PLATENA_GRUPNA_FAKTURA_KON_KLIENT).ID;
                    } else if (facture.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT) {
                        facture.StatusID = Statuse.GetByCode(Statuse.DELUMNO_PLATENA_IZLEZNA_FAKTURA_KON_KLIENT).ID;
                    }
                }
                Facture.Table.Context.SubmitChanges();
            }
        }


    }
}
