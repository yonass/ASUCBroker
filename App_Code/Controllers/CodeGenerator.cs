using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Broker.DataAccess;
using System.Collections.Generic;

/// <summary>
/// Summary description for CodeGenerator
/// </summary>
namespace Broker.Controllers
{
    public class CodeGenerator
    {

        public static string InsuranceTypeCodeGenerator()
        {
            List<InsuranceType> itList = InsuranceType.Select().OrderBy(c => c.Code).ToList();
            string code = itList.Last().Code;
            int length = code.Length;
            int codeNumber = int.Parse(code);
            int newCodeNumber = codeNumber + 1;
            string newCode = newCodeNumber.ToString();
            if (newCode.Length < length)
            {
                newCode = "0" + newCode;
            }
            return newCode;
        }

        public static string InsuranceSubTypeCodeGenerator(int InsuranceTypeID)
        {
            List<InsuranceSubType> insuranceSubTypeList = InsuranceSubType.GetByInsuranceType(InsuranceTypeID).OrderBy(c => c.Code).ToList();
            string lastCode = string.Empty;

            if (insuranceSubTypeList.Last().Code.Substring(2) == InsuranceSubType.LAST_CODE)
            {
                lastCode = insuranceSubTypeList[insuranceSubTypeList.Count - 2].Code.Substring(2);
            } else
            {
                lastCode = insuranceSubTypeList.Last().Code.Substring(2);
            }
            int lastCodeNumber = int.Parse(lastCode);
            InsuranceType it = InsuranceType.Get(InsuranceTypeID);
            int length = lastCode.Length;

            int newCodeNumber = lastCodeNumber + 1;
            string newCode = newCodeNumber.ToString();
            while (newCode.Length < length)
            {
                newCode = "0" + newCode;
            }

            newCode = it.Code + newCode;

            return newCode;
        }
        public static string BranchCodeGenerator()
        {
            string code;
            List<string> myListString = Broker.DataAccess.Branch.Table.Select(o => o.Code).ToList();
            List<int> myListInt = new List<int>();
            foreach (string s in myListString)
            {
                myListInt.Add(int.Parse(s));
            }
            myListInt.Sort();
            int codeNumber = myListInt[myListInt.Count - 1];
            code = (codeNumber + 1).ToString();
            return code;
        }

        public static string OrderCodeGenerator()
        {
            string code;
            List<string> myListString = Broker.DataAccess.Order.Table.Select(o => o.OrderNumber).ToList();
            List<int> myListInt = new List<int>();
            foreach (string s in myListString)
            {
                myListInt.Add(int.Parse(s));
            }
            if (myListInt.Count == 0)
            {
                return "1";
            } else
            {
                myListInt.Sort();
                int codeNumber = myListInt[myListInt.Count - 1];
                code = (codeNumber + 1).ToString();
                return code;
            }
        }


        public static string OfferCodeGenerator()
        {
            string code;
            List<string> myListString = Offer.Table.Select(o => o.OfferNumber).ToList();
            List<int> myListInt = new List<int>();
            foreach (string s in myListString)
            {
                myListInt.Add(int.Parse(s));
            }
            if (myListInt.Count == 0)
            {
                return "1";
            } else
            {
                myListInt.Sort();
                int codeNumber = myListInt[myListInt.Count - 1];
                code = (codeNumber + 1).ToString();
                return code;
            }
        }

        public static string DocumentSubTypeCodeGenerator(int DocumentTypeID)
        {
            List<DocumentSubType> dstList = DocumentSubType.Select().Where(d => d.DocumentTypeID == DocumentTypeID).OrderBy(d => d.Code).ToList();
            Broker.DataAccess.DocumentType dt = Broker.DataAccess.DocumentType.Get(DocumentTypeID);

            if (dstList.Count == 0)
            {
                return dt.Code + "001";
            } else
            {
                string code = dstList.Last().Code.Substring(dt.Code.Length);
                int length = code.Length;
                string newCode = (int.Parse(code) + 1).ToString();
                while (newCode.Length < length)
                {
                    newCode = "0" + newCode;
                }
                return dt.Code + newCode;
            }
        }

        public static string DocumentTypeCodeGenerator()
        {
            List<Broker.DataAccess.DocumentType> dtList = Broker.DataAccess.DocumentType.Select().OrderBy(d => d.Code).ToList();
            if (dtList.Count == 0)
            {
                return "001";
            } else
            {
                string code = dtList.Last().Code;
                int length = code.Length;
                string newCode = (int.Parse(code) + 1).ToString();
                while (newCode.Length < length)
                {
                    newCode = "0" + newCode;
                }
                return newCode;
            }
        }

        public static string PaymentTypeCodeGenerator()
        {
            string code;
            List<string> myListString = Broker.DataAccess.PaymentType.Table.Select(c => c.Code).ToList();
            List<int> myListInt = new List<int>();
            foreach (string s in myListString)
            {
                myListInt.Add(int.Parse(s));
            }
            if (myListInt.Count == 0)
            {
                return "1";
            } else
            {
                myListInt.Sort();
                int codeNumber = myListInt[myListInt.Count - 1];
                code = (codeNumber + 1).ToString();
                return code;
            }
        }

        public static string GenerateFactureNumber()
        {
            string nextNumber = string.Empty;
            //List<Broker.DataAccess.Facture> myList = Broker.DataAccess.Facture.Table.Where(c => c.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT).ToList();
            List<Broker.DataAccess.Facture> myList = Broker.DataAccess.Facture.Table.Where(c=>c.DocumentSubType.Code == DocumentSubType.GRUPNA_FAKTURA || c.DocumentSubType.Code == DocumentSubType.FAKTURA_KLIENT).ToList();
            myList = myList.OrderBy(sf => sf.ID).ToList();
            if (myList.Count > 0)
            {
                string[] splitString = myList.Last().FactureNumber.Split('/');
                if (splitString[1].EndsWith("-Б")) {
                    splitString[1] = splitString[1].Substring(0, 4);
                }
                if (splitString[1] == DateTime.Now.Year.ToString())
                {
                    int previousNumber = Convert.ToInt32(splitString[0]);
                    ++previousNumber;
                    nextNumber = previousNumber.ToString() + "/" + splitString[1];

                } else
                {
                    nextNumber = "1" + "/" + DateTime.Now.Year;
                }
            } else
            {
                nextNumber = "1" + "/" + DateTime.Now.Year;
            }

            return nextNumber;

        }

        public static string GenerateFactureNumberForBrokerageFactures() {
            string nextNumber = string.Empty;
            //List<Broker.DataAccess.Facture> myList = Broker.DataAccess.Facture.Table.Where(c => c.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA).ToList();
            List<Broker.DataAccess.Facture> myList = Broker.DataAccess.Facture.Table.Where(c=>c.DocumentSubType.Code == DocumentSubType.FAKTURA_PROVIZIJA).ToList();
            myList = myList.OrderBy(sf => sf.ID).ToList();
            if (myList.Count > 0) {
                string[] splitString = myList.Last().FactureNumber.Split('/');
                if (splitString[1].EndsWith("-Б")) {
                    splitString[1] = splitString[1].Substring(0, 4);
                }
                if (splitString[1] == DateTime.Now.Year.ToString()) {
                    int previousNumber = Convert.ToInt32(splitString[0]);
                    ++previousNumber;
                    nextNumber = previousNumber.ToString() + "/" + splitString[1];

                } else {
                    nextNumber = "1" + "/" + DateTime.Now.Year;
                }
            } else {
                nextNumber = "1" + "/" + DateTime.Now.Year;
            }

            return nextNumber + "-Б";

        }

    }
}
