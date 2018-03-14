using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ScaleController
/// </summary>
namespace Broker.Controllers {
    public class ScaleController {
        public ScaleController() {
            //
            // TODO: Add constructor logic here
            //
        }

        public static decimal Scale5(decimal value) {
            decimal floor = Decimal.Floor(value);
            decimal part = value - floor;
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

