using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RateController
/// </summary>
/// 
namespace Broker.Controllers.RateController {
    public class RateController {
        public static decimal Scale5(decimal num) {
            decimal floor = Decimal.Floor(num);
            decimal part = num - floor;
            if (part < (decimal)0.5) {
                return floor;
            }
            if (part >= (decimal)0.5) {
                return floor + 1;
            }
            return floor;
        }
    }
}
