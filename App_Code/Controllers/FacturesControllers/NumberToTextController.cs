using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for NumberToTextController
/// </summary>
/// 
namespace Broker.Controllers {
    public partial class NumberToTextController {
        public static string Konvertiranje(decimal Br) {
            decimal Iznos = Br;
            //Iznos = Zaokruzuvanje(Iznos);
            //Iznos = Iznos - Decimal.Truncate(Iznos);
            string St = Br.ToString();
            string[] BrSplit = St.Split('.', ',');
            St = BrSplit[0];
            string Zborovi = string.Empty;
            int Kolku = St.Length / 3;
            int Ostatok = St.Length - (Kolku * 3);
            if (Ostatok == 0) {
            } else {
                Kolku = Kolku + 1;
            }
            if (St.Length < Kolku * 3) {
                for (int i = 0; i <= (Kolku * 3) - St.Length; i++) {
                    St = "0" + St;
                }
            }
            for (int i = 1; i <= Kolku; i++) {
                string St1 = string.Empty;
                string BrojZbor = string.Empty;
                if (i == 1) {
                    St1 = St.Substring(St.Length - 3, 3);
                } else {
                    St1 = St.Substring(St.Length - ((i - 1) * 3) - 3, 3);
                }
                if (i == 1) {
                    BrojZbor = SoZborovi(St1, false);

                    if (St1.Substring(St1.Length - 2) == "1" & !(St1.Substring(St1.Length - 3) == "11")) {
                        Zborovi = BrojZbor + "денар ";
                    } else {
                        Zborovi = BrojZbor + "денари ";
                    }
                } else if (i == 2) {
                    BrojZbor = SoZborovi(St1, true);
                    if (St1.Substring(St1.Length - 2) == "1" & !(St1.Substring(St1.Length - 3) == "11")) {
                        Zborovi = BrojZbor + "илјада " + Zborovi;
                    } else {
                        Zborovi = BrojZbor + "илјади " + Zborovi;
                    }
                } else if (i == 3) {
                    BrojZbor = SoZborovi(St1, false);
                    if (St1.Substring(St1.Length - 2) == "1" & !(St1.Substring(St1.Length - 3) == "11")) {
                        Zborovi = BrojZbor + "милион " + Zborovi;
                    } else {
                        Zborovi = BrojZbor + "милиони " + Zborovi;
                    }
                } else if (i == 4) {
                    BrojZbor = SoZborovi(St1, true);
                    if (St1.Substring(St1.Length - 2) == "1" & !(St1.Substring(St1.Length - 3) == "11")) {
                        Zborovi = BrojZbor + "милјарда " + Zborovi;
                    } else {
                        Zborovi = BrojZbor + "милјарди " + Zborovi;
                    }
                }
            }
            if (Iznos == (decimal)0.5) {
                Zborovi = Zborovi + " и педесет дени";
            }
            return Zborovi;
        }





        public static decimal Zaokruzuvanje(decimal Piznos) {
            decimal functionReturnValue = 0;
            string St = string.Empty;
            //int MyPos = 0;
            string Pole = string.Empty;
            int pole = 0;
            St = Piznos.ToString();

            //if (MyPos == 0)
            //{
            //    functionReturnValue = Convert.ToDecimal(St);
            //    return functionReturnValue;
            //}
            string[] StSplit = St.Split('.', ',');
            if (StSplit.Length == 1) {
                return functionReturnValue;
            } else {
                if (StSplit[1].Length > 2) {
                    StSplit[1] = StSplit[1].Substring(0, 2);
                }
                pole = Convert.ToInt32(StSplit[1]);
                Pole = pole.ToString();
                if (Pole.Length == 1) {
                    Pole = Pole + "0";
                }
                pole = Convert.ToInt32(Pole);
                if (pole < 26) {
                    Pole = "00";
                    St = StSplit[0] + "," + Pole;
                    functionReturnValue = Convert.ToDecimal(St);
                } else if (pole < 76) {
                    Pole = "50";
                    St = StSplit[0] + "," + Pole;
                    functionReturnValue = Convert.ToDecimal(St);
                } else {
                    Pole = "00";
                    St = StSplit[0] + "," + Pole;
                    functionReturnValue = Convert.ToDecimal(St) + 1;
                }
                return functionReturnValue;
            }
        }



        public static string SoZborovi(string BrojT, bool IndEd) {
            bool IndZa3 = false;
            string BrojZ = null;
            if (BrojT.Substring(0, 1) == "1") {
                BrojZ = "сто ";
            } else if (BrojT.Substring(0, 1) == "2") {
                BrojZ = "двестотини ";
            } else if (BrojT.Substring(0, 1) == "3") {
                BrojZ = "тристотини ";
            } else if (BrojT.Substring(0, 1) == "4") {
                BrojZ = "четиристотини ";
            } else if (BrojT.Substring(0, 1) == "5") {
                BrojZ = "петстотини ";
            } else if (BrojT.Substring(0, 1) == "6") {
                BrojZ = "шестстотини ";
            } else if (BrojT.Substring(0, 1) == "7") {
                BrojZ = "седумстотини ";
            } else if (BrojT.Substring(0, 1) == "8") {
                BrojZ = "осумстотини ";
            } else if (BrojT.Substring(0, 1) == "9") {
                BrojZ = "деветстотини ";
            }
            IndZa3 = true;
            if (BrojT.Substring(1, 1) == "1") {
                IndZa3 = false;
                if (BrojT.Substring(2, 1) == "0") {
                    BrojZ = BrojZ + "десет ";
                } else if (BrojT.Substring(2, 1) == "1") {
                    BrojZ = BrojZ + "единаесет ";
                } else if (BrojT.Substring(2, 1) == "2") {
                    BrojZ = BrojZ + "дванаесет ";
                } else if (BrojT.Substring(2, 1) == "3") {
                    BrojZ = BrojZ + "тринаесет ";
                } else if (BrojT.Substring(2, 1) == "4") {
                    BrojZ = BrojZ + "четиринаесет ";
                } else if (BrojT.Substring(2, 1) == "5") {
                    BrojZ = BrojZ + "петнаесет ";
                } else if (BrojT.Substring(2, 1) == "6") {
                    BrojZ = BrojZ + "шеснаесет ";
                } else if (BrojT.Substring(2, 1) == "7") {
                    BrojZ = BrojZ + "седумнаесет ";
                } else if (BrojT.Substring(2, 1) == "8") {
                    BrojZ = BrojZ + "осумнаесет ";
                } else if (BrojT.Substring(2, 1) == "9") {
                    BrojZ = BrojZ + "деветнаесет ";
                }
            } else if (BrojT.Substring(1, 1) == "2") {
                BrojZ = BrojZ + "дваесет ";
            } else if (BrojT.Substring(1, 1) == "3") {
                BrojZ = BrojZ + "триесет ";
            } else if (BrojT.Substring(1, 1) == "4") {
                BrojZ = BrojZ + "четириесет ";
            } else if (BrojT.Substring(1, 1) == "5") {
                BrojZ = BrojZ + "педесет ";
            } else if (BrojT.Substring(1, 1) == "6") {
                BrojZ = BrojZ + "шеесет ";
            } else if (BrojT.Substring(1, 1) == "7") {
                BrojZ = BrojZ + "седумдесет ";
            } else if (BrojT.Substring(1, 1) == "8") {
                BrojZ = BrojZ + "осумдесет ";
            } else if (BrojT.Substring(1, 1) == "9") {
                BrojZ = BrojZ + "деведесет ";
            }
            if (IndZa3) {
                if (BrojT.Substring(1, 1) != "0" & BrojT.Substring(2, 1) != "0") BrojZ = BrojZ + "и ";
                if (BrojT.Substring(2, 1) == "1") {
                    if (IndEd) {
                        BrojZ = BrojZ + "една ";
                    } else {
                        BrojZ = BrojZ + "еден ";
                    }
                } else if (BrojT.Substring(2, 1) == "2") {
                    if (IndEd) {
                        BrojZ = BrojZ + "две ";
                    } else {
                        BrojZ = BrojZ + "два ";
                    }
                } else if (BrojT.Substring(2, 1) == "3") {
                    BrojZ = BrojZ + "три ";
                } else if (BrojT.Substring(2, 1) == "4") {
                    BrojZ = BrojZ + "четири ";
                } else if (BrojT.Substring(2, 1) == "5") {
                    BrojZ = BrojZ + "пет ";
                } else if (BrojT.Substring(2, 1) == "6") {
                    BrojZ = BrojZ + "шест ";
                } else if (BrojT.Substring(2, 1) == "7") {
                    BrojZ = BrojZ + "седум ";
                } else if (BrojT.Substring(2, 1) == "8") {
                    BrojZ = BrojZ + "осум ";
                } else if (BrojT.Substring(2, 1) == "9") {
                    BrojZ = BrojZ + "девет ";
                }
            }
            return BrojZ;
        }
    }
}
