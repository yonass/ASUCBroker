using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.Validators;

/// <summary>
/// Summary description for Client
/// </summary>
/// 
namespace Broker.DataAccess
{
    public partial class Client : EntityBase<Client>
    {

        //public static bool GetForValidate (string embg){
        //    List<Client> cList = Table.Where(c => c.EMBG == embg).ToList();

        //}
        public static string DISCARD_EMBG="0000000000000";


        public static Client GetByEmbg(string embg)
        {
            return Table.Where(c => c.EMBG == embg).SingleOrDefault();

        }
        public static Client GetByEmbg()
        {
            return null;
        }

        public static Client GetClientByOrderID(int orderID)
        {
            try
            {
                Order o = Order.Get(orderID);
                return o.Client;
            } catch
            {
                return new Client();
            }
        }

        public static Client GetOwnerByOrderID(int orderID)
        {
            try
            {
                Order o = Order.Get(orderID);
                return o.Client1;
            } catch
            {
                return new Client();
            }
        }

        public static Client GetClientByOfferID(int offerID)
        {
            try
            {
                Offer o = Offer.Get(offerID);
                return o.Client;
            } catch
            {
                return new Client();
            }
        }

        public static Client GetOwnerByOfferID(int offerID)
        {
            try
            {
                Offer o = Offer.Get(offerID);
                return o.Client1;
            } catch
            {
                return new Client();
            }
        }


        bool? _exists = null;
        public bool Exists
        {
            get
            {
                if (_exists != null)
                {
                    return (bool)_exists;
                }
                _exists = false;
                if (ID > 0)
                {
                    try
                    {
                        Get(ID);
                        _exists = true;
                    } catch (Exception)
                    {
                    }
                }
                return (bool)_exists;
            }

        }

        public void ValidateClient()
        {
            if (this.Name == null || this.Name == string.Empty)
            {
                ValidationErrors.Add("NAME", "Задолжително е внесување на име!");
            }
            if (this.Address == null || this.Address == string.Empty)
            {
                ValidationErrors.Add("ADDRESS", "Задолжително е внесување на адреса!");
            }
            if (this.EMBG == null) {
                ValidationErrors.Add("EMBG", "Невалиден ЕМБГ!");
                return;
            }
            if (this.IsLaw)
            {
                if (!ClientValidator.IsValidLawEmbg(this.EMBG))
                {
                    ValidationErrors.Add("EMBG", "Невалиден ЕМБГ!");

                }
            } else
            {
                if (!this.IsForeigner) {
                    if (!ClientValidator.isValidPersonalEmbg(this.EMBG)) {
                        ValidationErrors.Add("EMBG", "Невалиден ЕМБГ!");
                    }
                }
            }
            Client c = null;
            try
            {
                c = Client.GetByEmbg(this.EMBG);
            } catch
            {
                ValidationErrors.Add("EMBG", "Контактирајте со SIMT");
            }
            if (!Exists)
            {
                if (c != null)
                {
                    ValidationErrors.Add("EMBG", "Клиентот веќе постои");
                }
            } else
            {
                if (c != null && c.ID != ID)
                {
                    ValidationErrors.Add("EMBG", "EMBG постои");
                }
            }
        }
        public void ValidateOwner()
        {
            if (this.Name == null || this.Name == string.Empty)
            {
                ValidationErrors.Add("OwnerNAME", "Задолжително е внесување на име!");
            }
            if (this.Address == null || this.Address == string.Empty)
            {
                ValidationErrors.Add("OwnerADDRESS", "Задолжително е внесување на адреса!");
            }
            if (this.EMBG == null) {
                ValidationErrors.Add("OwnerEMBG", "Невалиден ЕМБГ!");
                return;
            }
            if (this.IsLaw)
            {
                if (!ClientValidator.IsValidLawEmbg(this.EMBG))
                {
                    ValidationErrors.Add("OwnerEMBG", "Невалиден ЕМБГ!");

                }
            } else
            {
                if (!this.IsForeigner) {
                    if (!ClientValidator.isValidPersonalEmbg(this.EMBG)) {
                        ValidationErrors.Add("OwnerEMBG", "Невалиден ЕМБГ!");
                    }
                }
            }
            Client c = null;
            try
            {
                c = Client.GetByEmbg(this.EMBG);
            } catch
            {
                ValidationErrors.Add("OwnerEMBG", "Контактирајте со SIMT");
            }
            if (!Exists)
            {
                if (c != null)
                {
                    ValidationErrors.Add("OwnerEMBG", "Клиентот веќе постои");
                }
            } else
            {
                if (c != null && c.ID != ID)
                {
                    ValidationErrors.Add("OwnerEMBG", "EMBG постои");
                }
            }
        }

        public static List<Client> GetInsuranceCompanies()
        {
            return Table.Where(c => c.IsInsuranceCompany == true).ToList();
        }

        public static List<Client> GetLawClients() {
            return Table.Where(c => c.IsLaw == true).OrderBy(c => c.Name).ToList();
        }
      
        public static List<Client> GetForGroupFactures(DateTime startDate, DateTime endDate) {
            string query = @"select c.name,c.id from clients c
                            INNER JOIN policies p ON c.id = p.clientID
                            INNER JOIN policyitems pi ON pi.policyID = p.ID
                            WHERE p.discard = 0 and pi.IsFactureCreated = 0 and pi.IsPaidInBrokerHouse=1 and
                            p.ApplicationDate >='" + startDate.Year + -+startDate.Month + -+startDate.Day +
                            @"' and p.applicationdate<='" + endDate.Year + -+endDate.Month + -+endDate.Day +
                            @"'group by c.name,c.id";
            DataClassesDataContext dcdc = new DataClassesDataContext();
            List<Client> clients = dcdc.ExecuteQuery<Client>(query).ToList();
            return clients;
        }


       
    }
}
