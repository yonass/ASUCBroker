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
using System.IO;
using System.Collections.Generic;
using Broker.DataAccess;


namespace ASUC.Controllers.ConvertController {
    public class ConvertToMacedonian {

        /// <summary>
        /// Konvertiranje na string napisan so makedonska, hrvatska ili
        /// angliska podrska vo makedonska podrska
        /// </summary>
        /// <param name="ime">string ime</param>
        /// <returns>string</returns>
        /// Se koristi vo kontrolerite za pecatenje na AO polisa
        /// za polinjata Ime i prezime i Adresa na dogovoruvac i osigurenik
        /// i Ime i prezime na agentot sto ja izrabotil polisata
        /// i Filijalata/brokerskoto drustvo kade e izrabotena
        /// i vo laserskoto i vo matricnoto pecatenje - ReportControllers\PrintPolicy.cs
        public static string ConvertToMACEDONIAN(string ime) {
            int flag = 0;
            string newIme = string.Empty;
            for (int i = 0; i < ime.Length; i++) {

                switch (ime[i]) {
                    ///od latinica na kirilica
                    case 'A':
                        newIme = string.Concat(newIme, 'А'.ToString());
                        break;
                    case 'a':
                        newIme = string.Concat(newIme, 'а'.ToString());
                        break;
                    case 'B':
                        newIme = string.Concat(newIme, 'Б'.ToString());
                        break;
                    case 'b':
                        newIme = string.Concat(newIme, 'б'.ToString());
                        break;
                    case 'V':
                        newIme = string.Concat(newIme, 'В'.ToString());
                        break;
                    case 'v':
                        newIme = string.Concat(newIme, 'в'.ToString());
                        break;
                    case 'G':
                        newIme = string.Concat(newIme, 'Г'.ToString());
                        break;
                    case 'g':
                        newIme = string.Concat(newIme, 'г'.ToString());
                        break;
                    case 'D':
                        if (ime.Length-1 > i) {
                            if (ime[i + 1] == 'Ž') { newIme = string.Concat(newIme, 'Џ'.ToString()); i++; } 
                            else if (ime[i + 1] == 'J') { newIme = string.Concat(newIme, 'Џ'.ToString()); i++; }
                            else newIme = string.Concat(newIme, 'Д'.ToString());
                        } else
                            newIme = string.Concat(newIme, 'Д'.ToString());
                        break;
                    case 'd':
                        if (ime.Length-1 > i) {
                            if (ime[i + 1] == 'ž') { newIme = string.Concat(newIme, 'џ'.ToString()); i++; }
                            else if (ime[i + 1] == 'j') { newIme = string.Concat(newIme, 'џ'.ToString()); i++; } 
                            else newIme = string.Concat(newIme, 'д'.ToString());
                        } else
                            newIme = string.Concat(newIme, 'д'.ToString());
                        break;
                    case 'Đ':
                        newIme = string.Concat(newIme, 'Ѓ'.ToString());
                        break;
                    case 'đ':
                        newIme = string.Concat(newIme, 'ѓ'.ToString());
                        break;
                    case 'E':
                        newIme = string.Concat(newIme, 'Е'.ToString());
                        break;
                    case 'e':
                        newIme = string.Concat(newIme, 'е'.ToString());
                        break;
                    case 'Ž':
                        newIme = string.Concat(newIme, 'Ж'.ToString());
                        break;
                    case 'ž':
                        newIme = string.Concat(newIme, 'ж'.ToString());
                        break;
                    case 'Z':
                        newIme = string.Concat(newIme, 'З'.ToString());
                        break;
                    case 'z':
                        newIme = string.Concat(newIme, 'з'.ToString());
                        break;
                    case 'Y':
                        newIme = string.Concat(newIme, 'Ѕ'.ToString());
                        break;
                    case 'y':
                        newIme = string.Concat(newIme, 'ѕ'.ToString());
                        break;
                    case 'I':
                        newIme = string.Concat(newIme, 'И'.ToString());
                        break;
                    case 'i':
                        newIme = string.Concat(newIme, 'и'.ToString());
                        break;
                    case 'J':
                        newIme = string.Concat(newIme, 'Ј'.ToString());
                        break;
                    case 'j':
                        newIme = string.Concat(newIme, 'ј'.ToString());
                        break;
                    case 'K':
                        newIme = string.Concat(newIme, 'К'.ToString());
                        break;
                    case 'k':
                        newIme = string.Concat(newIme, 'к'.ToString());
                        break;
                    case 'L':
                        if (ime.Length-1 > i) {
                            if (ime[i + 1] == 'J') { newIme = string.Concat(newIme, 'Љ'.ToString()); i++; } else {
                                newIme = string.Concat(newIme, 'Л'.ToString());
                            }
                        } else
                            newIme = string.Concat(newIme, 'Л'.ToString());
                        break;
                    case 'l':
                        if (ime.Length-1 > i) {
                            if (ime[i + 1] == 'j') { newIme = string.Concat(newIme, 'љ'.ToString()); i++; } else {
                                newIme = string.Concat(newIme, 'л'.ToString());
                            }
                        } else
                            newIme = string.Concat(newIme, 'л'.ToString());
                        break;
                    case 'Q':
                        newIme = string.Concat(newIme, 'Љ'.ToString());
                        break;
                    case 'q':
                        newIme = string.Concat(newIme, 'љ'.ToString());
                        break;
                    case 'M':
                        newIme = string.Concat(newIme, 'М'.ToString());
                        break;
                    case 'm':
                        newIme = string.Concat(newIme, 'м'.ToString());
                        break;
                    case 'N':
                        if (ime.Length-1 > i) {
                            if (ime[i + 1] == 'J') { newIme = string.Concat(newIme, 'Њ'.ToString()); i++; } else {
                                newIme = string.Concat(newIme, 'Н'.ToString());
                            }
                        } else
                            newIme = string.Concat(newIme, 'Н'.ToString());
                        break;
                    case 'n':
                        if (ime.Length-1 > i) {
                            if (ime[i + 1] == 'ј') { newIme = string.Concat(newIme, 'њ'.ToString()); i++; } else {
                                newIme = string.Concat(newIme, 'н'.ToString());
                            }
                        } else
                            newIme = string.Concat(newIme, 'н'.ToString());
                        break;
                    case 'W':
                        newIme = string.Concat(newIme, 'Њ'.ToString());
                        break;
                    case 'w':
                        newIme = string.Concat(newIme, 'њ'.ToString());
                        break;
                    case 'O':
                        newIme = string.Concat(newIme, 'О'.ToString());
                        break;
                    case 'o':
                        newIme = string.Concat(newIme, 'о'.ToString());
                        break;
                    case 'P':
                        newIme = string.Concat(newIme, 'П'.ToString());
                        break;
                    case 'p':
                        newIme = string.Concat(newIme, 'п'.ToString());
                        break;
                    case 'R':
                        newIme = string.Concat(newIme, 'Р'.ToString());
                        break;
                    case 'r':
                        newIme = string.Concat(newIme, 'р'.ToString());
                        break;
                    case 'S':
                        newIme = string.Concat(newIme, 'С'.ToString());
                        break;
                    case 's':
                        newIme = string.Concat(newIme, 'с'.ToString());
                        break;
                    case 'T':
                        newIme = string.Concat(newIme, 'Т'.ToString());
                        break;
                    case 't':
                        newIme = string.Concat(newIme, 'т'.ToString());
                        break;
                    case 'Ć':
                        newIme = string.Concat(newIme, 'Ќ'.ToString());
                        break;
                    case 'ć':
                        newIme = string.Concat(newIme, 'ќ'.ToString());
                        break;
                    case 'U':
                        newIme = string.Concat(newIme, 'У'.ToString());
                        break;
                    case 'u':
                        newIme = string.Concat(newIme, 'у'.ToString());
                        break;
                    case 'F':
                        newIme = string.Concat(newIme, 'Ф'.ToString());
                        break;
                    case 'f':
                        newIme = string.Concat(newIme, 'ф'.ToString());
                        break;
                    case 'H':
                        newIme = string.Concat(newIme, 'Х'.ToString());
                        break;
                    case 'h':
                        newIme = string.Concat(newIme, 'х'.ToString());
                        break;
                    case 'C':
                        newIme = string.Concat(newIme, 'Ц'.ToString());
                        break;
                    case 'c':
                        newIme = string.Concat(newIme, 'ц'.ToString());
                        break;
                    case 'Č':
                        newIme = string.Concat(newIme, 'Ч'.ToString());
                        break;
                    case 'č':
                        newIme = string.Concat(newIme, 'ч'.ToString());
                        break;
                    case 'X':
                        newIme = string.Concat(newIme, 'Џ'.ToString());
                        break;
                    case 'x':
                        newIme = string.Concat(newIme, 'џ'.ToString());
                        break;
                    case 'Š':
                        newIme = string.Concat(newIme, 'Ш'.ToString());
                        break;
                    case 'š':
                        newIme = string.Concat(newIme, 'ш'.ToString());
                        break;
                    case '[':
                        newIme = string.Concat(newIme, 'ш'.ToString());
                        break;
                    case '{':
                        newIme = string.Concat(newIme, 'Ш'.ToString());
                        break;
                    case ']':
                        newIme = string.Concat(newIme, 'ѓ'.ToString());
                        break;
                    case '}':
                        newIme = string.Concat(newIme, 'Ѓ'.ToString());
                        break;
                    case '-':
                        newIme = string.Concat(newIme, '-'.ToString());
                        break;
                    case ' ':
                        newIme = string.Concat(newIme, ' '.ToString());
                        flag = 1;
                        break;
                    case '\t':
                        newIme = string.Concat(newIme, '\t'.ToString());
                        break;
                    case '\n':
                        newIme = string.Concat(newIme, '\n'.ToString());
                        break;

                    ///broevi
                    case '1':
                        newIme = string.Concat(newIme, '1'.ToString());
                        break;
                    case '2':
                        newIme = string.Concat(newIme, '2'.ToString());
                        break;
                    case '3':
                        newIme = string.Concat(newIme, '3'.ToString());
                        break;
                    case '4':
                        newIme = string.Concat(newIme, '4'.ToString());
                        break;
                    case '5':
                        newIme = string.Concat(newIme, '5'.ToString());
                        break;
                    case '6':
                        newIme = string.Concat(newIme, '6'.ToString());
                        break;
                    case '7':
                        newIme = string.Concat(newIme, '7'.ToString());
                        break;
                    case '8':
                        newIme = string.Concat(newIme, '8'.ToString());
                        break;
                    case '9':
                        newIme = string.Concat(newIme, '9'.ToString());
                        break;
                    case '0':
                        newIme = string.Concat(newIme, '0'.ToString());
                        break;
                    ///specijalni znaci
                    case '`':
                        newIme = string.Concat(newIme, '`'.ToString());
                        break;
                    case '~':
                        newIme = string.Concat(newIme, '~'.ToString());
                        break;
                    case '!':
                        newIme = string.Concat(newIme, '!'.ToString());
                        break;
                    case '@':
                        newIme = string.Concat(newIme, '@'.ToString());
                        break;
                    case '#':
                        newIme = string.Concat(newIme, '#'.ToString());
                        break;
                    case '$':
                        newIme = string.Concat(newIme, '$'.ToString());
                        break;
                    case '%':
                        newIme = string.Concat(newIme, '%'.ToString());
                        break;
                    case '^':
                        newIme = string.Concat(newIme, '^'.ToString());
                        break;
                    case '&':
                        newIme = string.Concat(newIme, '&'.ToString());
                        break;
                    case '*':
                        newIme = string.Concat(newIme, '*'.ToString());
                        break;
                    case '(':
                        newIme = string.Concat(newIme, '('.ToString());
                        break;
                    case ')':
                        newIme = string.Concat(newIme, ')'.ToString());
                        break;
                    case '_':
                        newIme = string.Concat(newIme, '_'.ToString());
                        break;
                    case '=':
                        newIme = string.Concat(newIme, '='.ToString());
                        break;
                    case '+':
                        newIme = string.Concat(newIme, '+'.ToString());
                        break;
                    case '.':
                        newIme = string.Concat(newIme, '.'.ToString());
                        break;
                    case ',':
                        newIme = string.Concat(newIme, ','.ToString());
                        break;
                    case '/':
                        newIme = string.Concat(newIme, '/'.ToString());
                        break;
                    case '<':
                        newIme = string.Concat(newIme, '<'.ToString());
                        break;
                    case '>':
                        newIme = string.Concat(newIme, '>'.ToString());
                        break;
                    case '?':
                        newIme = string.Concat(newIme, '?'.ToString());
                        break;
                    case '|':
                        newIme = string.Concat(newIme, '|'.ToString());
                        break;
                    case '\\':
                        newIme = string.Concat(newIme, '\\'.ToString());
                        break;
                    case ';':
                        newIme = string.Concat(newIme, ';'.ToString());
                        break;
                    case ':':
                        newIme = string.Concat(newIme, ':'.ToString());
                        break;
                    case '"':
                        newIme = string.Concat(newIme, '"'.ToString());
                        break;
                    case '\'':
                        newIme = string.Concat(newIme, '\''.ToString());
                        break;

                    ///od kirilica na kirilica
                    case 'А':
                        newIme = string.Concat(newIme, 'А'.ToString());
                        break;
                    case 'а':
                        newIme = string.Concat(newIme, 'а'.ToString());
                        break;
                    case 'Б':
                        newIme = string.Concat(newIme, 'Б'.ToString());
                        break;
                    case 'б':
                        newIme = string.Concat(newIme, 'б'.ToString());
                        break;
                    case 'В':
                        newIme = string.Concat(newIme, 'В'.ToString());
                        break;
                    case 'в':
                        newIme = string.Concat(newIme, 'в'.ToString());
                        break;
                    case 'Г':
                        newIme = string.Concat(newIme, 'Г'.ToString());
                        break;
                    case 'г':
                        newIme = string.Concat(newIme, 'г'.ToString());
                        break;
                    case 'Д':
                        newIme = string.Concat(newIme, 'Д'.ToString());
                        break;
                    case 'д':
                        newIme = string.Concat(newIme, 'д'.ToString());
                        break;
                    case 'Ѓ':
                        newIme = string.Concat(newIme, 'Ѓ'.ToString());
                        break;
                    case 'ѓ':
                        newIme = string.Concat(newIme, 'ѓ'.ToString());
                        break;
                    case 'Е':
                        newIme = string.Concat(newIme, 'Е'.ToString());
                        break;
                    case 'е':
                        newIme = string.Concat(newIme, 'е'.ToString());
                        break;
                    case 'Ж':
                        newIme = string.Concat(newIme, 'Ж'.ToString());
                        break;
                    case 'ж':
                        newIme = string.Concat(newIme, 'ж'.ToString());
                        break;
                    case 'З':
                        newIme = string.Concat(newIme, 'З'.ToString());
                        break;
                    case 'з':
                        newIme = string.Concat(newIme, 'з'.ToString());
                        break;
                    case 'Ѕ':
                        newIme = string.Concat(newIme, 'Ѕ'.ToString());
                        break;
                    case 'ѕ':
                        newIme = string.Concat(newIme, 'ѕ'.ToString());
                        break;
                    case 'И':
                        newIme = string.Concat(newIme, 'И'.ToString());
                        break;
                    case 'и':
                        newIme = string.Concat(newIme, 'и'.ToString());
                        break;
                    case 'Ј':
                        newIme = string.Concat(newIme, 'Ј'.ToString());
                        break;
                    case 'ј':
                        newIme = string.Concat(newIme, 'ј'.ToString());
                        break;
                    case 'К':
                        newIme = string.Concat(newIme, 'К'.ToString());
                        break;
                    case 'к':
                        newIme = string.Concat(newIme, 'к'.ToString());
                        break;
                    case 'Л':
                        newIme = string.Concat(newIme, 'Л'.ToString());
                        break;
                    case 'л':
                        newIme = string.Concat(newIme, 'л'.ToString());
                        break;
                    case 'Љ':
                        newIme = string.Concat(newIme, 'Љ'.ToString());
                        break;
                    case 'љ':
                        newIme = string.Concat(newIme, 'љ'.ToString());
                        break;
                    case 'М':
                        newIme = string.Concat(newIme, 'М'.ToString());
                        break;
                    case 'м':
                        newIme = string.Concat(newIme, 'м'.ToString());
                        break;
                    case 'Н':
                        newIme = string.Concat(newIme, 'Н'.ToString());
                        break;
                    case 'н':
                        newIme = string.Concat(newIme, 'н'.ToString());
                        break;
                    case 'Њ':
                        newIme = string.Concat(newIme, 'Њ'.ToString());
                        break;
                    case 'њ':
                        newIme = string.Concat(newIme, 'њ'.ToString());
                        break;
                    case 'О':
                        newIme = string.Concat(newIme, 'О'.ToString());
                        break;
                    case 'о':
                        newIme = string.Concat(newIme, 'о'.ToString());
                        break;
                    case 'П':
                        newIme = string.Concat(newIme, 'П'.ToString());
                        break;
                    case 'п':
                        newIme = string.Concat(newIme, 'п'.ToString());
                        break;
                    case 'Р':
                        newIme = string.Concat(newIme, 'Р'.ToString());
                        break;
                    case 'р':
                        newIme = string.Concat(newIme, 'р'.ToString());
                        break;
                    case 'С':
                        newIme = string.Concat(newIme, 'С'.ToString());
                        break;
                    case 'с':
                        newIme = string.Concat(newIme, 'с'.ToString());
                        break;
                    case 'Т':
                        newIme = string.Concat(newIme, 'Т'.ToString());
                        break;
                    case 'т':
                        newIme = string.Concat(newIme, 'т'.ToString());
                        break;
                    case 'Ќ':
                        newIme = string.Concat(newIme, 'Ќ'.ToString());
                        break;
                    case 'ќ':
                        newIme = string.Concat(newIme, 'ќ'.ToString());
                        break;
                    case 'У':
                        newIme = string.Concat(newIme, 'У'.ToString());
                        break;
                    case 'у':
                        newIme = string.Concat(newIme, 'у'.ToString());
                        break;
                    case 'Ф':
                        newIme = string.Concat(newIme, 'Ф'.ToString());
                        break;
                    case 'ф':
                        newIme = string.Concat(newIme, 'ф'.ToString());
                        break;
                    case 'Х':
                        newIme = string.Concat(newIme, 'Х'.ToString());
                        break;
                    case 'х':
                        newIme = string.Concat(newIme, 'х'.ToString());
                        break;
                    case 'Ц':
                        newIme = string.Concat(newIme, 'Ц'.ToString());
                        break;
                    case 'ц':
                        newIme = string.Concat(newIme, 'ц'.ToString());
                        break;
                    case 'Ч':
                        newIme = string.Concat(newIme, 'Ч'.ToString());
                        break;
                    case 'ч':
                        newIme = string.Concat(newIme, 'ч'.ToString());
                        break;
                    case 'Џ':
                        newIme = string.Concat(newIme, 'Џ'.ToString());
                        break;
                    case 'џ':
                        newIme = string.Concat(newIme, 'џ'.ToString());
                        break;
                    case 'Ш':
                        newIme = string.Concat(newIme, 'Ш'.ToString());
                        break;
                    case 'ш':
                        newIme = string.Concat(newIme, 'ш'.ToString());
                        break;
                    case '„':
                        newIme = string.Concat(newIme, '„'.ToString());
                        break;
                    case '“':
                        newIme = string.Concat(newIme, '“'.ToString());
                        break;


                    delault: break;
                }
                //    char c = newIme[0];
                //  c = char.ToUpper(c);
                //string krajnoIme = c.ToString();
                //newIme.ToUpper()
                //if (i == 0)
                //    newIme = newIme.ToUpper();

                //if (flag == 2) {
                //    string c = newIme[i].ToString();
                 //   c = c.ToUpper();
                 //   newIme = newIme.Remove(newIme.Length - 1);
                 //   newIme = string.Concat(newIme, c);
                 //   flag = 0;
               // } else if (flag == 1)
                 //   flag++;

            }
            return newIme;
        }
    }
}