using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Broker.DataAccess;

/// <summary>
/// Summary description for FactureSpecificationsController
/// </summary>
public class FactureSpecificationsController {

    public string InsuranceSubType { get; set; }
    public int InusuranceSubTypeID { get; set; }
    public decimal Difference { get; set; }
    public bool IsEqual { get; set; }



    public FactureSpecificationsController() {
        


    }

    public static List<FactureSpecificationsController> GetFromDictionary(Dictionary<int, decimal> resultDictionary) {
        List<FactureSpecificationsController> fscList = new List<FactureSpecificationsController>();
        foreach (KeyValuePair<int, decimal> pair in resultDictionary) {
            FactureSpecificationsController fsc = new FactureSpecificationsController();
            fsc.InusuranceSubTypeID = pair.Key;
            fsc.InsuranceSubType = string.Empty;//InsuranceSubType.Get(pair.Key).Description;
            fsc.Difference = pair.Value;
            fsc.IsEqual = (pair.Value == 0);
            fscList.Add(fsc);
        }
        return fscList;
    }


}
