using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Parameter
/// </summary>
/// 
namespace Broker.DataAccess
{
    public partial class Parameter : EntityBase<Parameter>
    {
        public static string IME = "11";
        public static string ADRESA = "12";
        public static string MESTO = "13";
        public static string TELEFON = "14";
        public static string FAKS = "15";
        public static string TREZORSKA_SMETKA = "16";
        public static string BUDZETSKI_KORISNIK = "17";
        public static string PRIHODNA_SIFRA = "18";
        public static string EDB = "19";
        public static string DEPONENT = "20";
        public static string WEB_STRANA = "21";
        public static string EMAIL = "22";
        public static string ZIRO_SMETKA = "23";
        public static string MATICEN_BROJ = "24";
        public static string DENOVI_ZA_NAPLATA_NA_FAKTURA = "25";
        public static string SE_KORISTAT_PREFIKSI = "30";
        public static string DOLZINA_NA_PREFIKS = "31";
        public static string SE_PREBARUVA_OD_TEHNICKI_PREGLED = "35";
        public static string SE_PREBARUVA_OD_NBO = "40";
        public static string ZIRO_SMETKA_ZA_OSIG_KOMPANII = "26";
        public static string ZIRO_SMETKA_ZA_KLIENTI = "27";
        public static string ZADOLZITELEN_PROCENT_ZA_PRVA_RATA = "28";
        public static string SE_PECATI_SPECIFIKACIJA_NA_F_RA_ZA_KLIENT = "45";
        public static string VREDNOST_NA_EVRO = "41";
        
        public static Parameter GetByCode(string code)
        {
            return Table.Where(c => c.Code== code).SingleOrDefault();
        }


    }
}
