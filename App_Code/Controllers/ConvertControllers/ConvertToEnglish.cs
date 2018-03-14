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

    public class ConvertToEnglish {

        /// <summary>
        /// Konvertiranje na string napisan so makedonska, hrvatska ili
        /// angliska podrska vo angliska podrska
        /// </summary>
        /// <param name="ime">string ime</param>
        /// <returns>string</returns>
        /// Se koristi vo kontrolerite za pecatenje na zelena karta
        /// za polinjata Ime i prezime i Adresa i vo laserskoto i vo matricnoto
        /// pecatenje na ZK - ReportControllers\PrintGreenCardMatrix.cs i ReportControllers\PrintGreenCardLaser.cs
        public static string ConvertToENGLISH(string ime) {
            int flag = 0;
            string newIme = string.Empty;
            for (int i = 0; i < ime.Length; i++) {

                switch (ime[i]) {
                    ///od latinica na latinica(vklucuvajki gi i hrvatskite bukvi)
                    case 'A':
                        newIme = string.Concat(newIme, 'A'.ToString());
                        break;
                    case 'a':
                        newIme = string.Concat(newIme, 'a'.ToString());
                        break;
                    case 'B':
                        newIme = string.Concat(newIme, 'B'.ToString());
                        break;
                    case 'b':
                        newIme = string.Concat(newIme, 'b'.ToString());
                        break;
                    case 'V':
                        newIme = string.Concat(newIme, 'V'.ToString());
                        break;
                    case 'v':
                        newIme = string.Concat(newIme, 'v'.ToString());
                        break;
                    case 'G':
                        newIme = string.Concat(newIme, 'G'.ToString());
                        break;
                    case 'g':
                        newIme = string.Concat(newIme, 'g'.ToString());
                        break;
                    case 'D':
                        newIme = string.Concat(newIme, 'D'.ToString());
                        break;
                    case 'd':
                        newIme = string.Concat(newIme, 'd'.ToString());
                        break;
                    case 'Đ':
                        newIme = string.Concat(newIme, "Đ".ToString());
                        break;
                    case 'đ':
                        newIme = string.Concat(newIme, "đ".ToString());
                        break;
                    case 'E':
                        newIme = string.Concat(newIme, 'E'.ToString());
                        break;
                    case 'e':
                        newIme = string.Concat(newIme, 'e'.ToString());
                        break;
                    case 'Ž':
                        newIme = string.Concat(newIme, "Ž".ToString());
                        break;
                    case 'ž':
                        newIme = string.Concat(newIme, "ž".ToString());
                        break;
                    case 'Z':
                        newIme = string.Concat(newIme, 'Z'.ToString());
                        break;
                    case 'z':
                        newIme = string.Concat(newIme, 'z'.ToString());
                        break;
                    case 'Y':
                        newIme = string.Concat(newIme, 'Y'.ToString());
                        break;
                    case 'y':
                        newIme = string.Concat(newIme, 'y'.ToString());
                        break;
                    case 'I':
                        newIme = string.Concat(newIme, 'I'.ToString());
                        break;
                    case 'i':
                        newIme = string.Concat(newIme, 'i'.ToString());
                        break;
                    case 'J':
                        newIme = string.Concat(newIme, 'J'.ToString());
                        break;
                    case 'j':
                        newIme = string.Concat(newIme, 'j'.ToString());
                        break;
                    case 'K':
                        newIme = string.Concat(newIme, 'K'.ToString());
                        break;
                    case 'k':
                        newIme = string.Concat(newIme, 'k'.ToString());
                        break;
                    case 'L':
                        newIme = string.Concat(newIme, 'L'.ToString());
                        break;
                    case 'l':
                        newIme = string.Concat(newIme, 'l'.ToString());
                        break;
                    case 'Q':
                        newIme = string.Concat(newIme, 'Q'.ToString());
                        break;
                    case 'q':
                        newIme = string.Concat(newIme, 'q'.ToString());
                        break;
                    case 'M':
                        newIme = string.Concat(newIme, 'M'.ToString());
                        break;
                    case 'm':
                        newIme = string.Concat(newIme, 'm'.ToString());
                        break;
                    case 'N':
                        newIme = string.Concat(newIme, 'N'.ToString());
                        break;
                    case 'n':
                        newIme = string.Concat(newIme, 'n'.ToString());
                        break;
                    case 'W':
                        newIme = string.Concat(newIme, 'W'.ToString());
                        break;
                    case 'w':
                        newIme = string.Concat(newIme, 'w'.ToString());
                        break;
                    case 'O':
                        newIme = string.Concat(newIme, 'O'.ToString());
                        break;
                    case 'o':
                        newIme = string.Concat(newIme, 'o'.ToString());
                        break;
                    case 'P':
                        newIme = string.Concat(newIme, 'P'.ToString());
                        break;
                    case 'p':
                        newIme = string.Concat(newIme, 'p'.ToString());
                        break;
                    case 'R':
                        newIme = string.Concat(newIme, 'R'.ToString());
                        break;
                    case 'r':
                        newIme = string.Concat(newIme, 'r'.ToString());
                        break;
                    case 'S':
                        newIme = string.Concat(newIme, 'S'.ToString());
                        break;
                    case 's':
                        newIme = string.Concat(newIme, 's'.ToString());
                        break;
                    case 'T':
                        newIme = string.Concat(newIme, 'T'.ToString());
                        break;
                    case 't':
                        newIme = string.Concat(newIme, 't'.ToString());
                        break;
                    case 'Ć':
                        newIme = string.Concat(newIme, "Ć".ToString());
                        break;
                    case 'ć':
                        newIme = string.Concat(newIme, "ć".ToString());
                        break;
                    case 'U':
                        newIme = string.Concat(newIme, 'U'.ToString());
                        break;
                    case 'u':
                        newIme = string.Concat(newIme, 'u'.ToString());
                        break;
                    case 'F':
                        newIme = string.Concat(newIme, 'F'.ToString());
                        break;
                    case 'f':
                        newIme = string.Concat(newIme, 'f'.ToString());
                        break;
                    case 'H':
                        newIme = string.Concat(newIme, 'H'.ToString());
                        break;
                    case 'h':
                        newIme = string.Concat(newIme, 'h'.ToString());
                        break;
		    case 'X':
                        newIme = string.Concat(newIme, 'X'.ToString());
                        break;
                    case 'x':
                        newIme = string.Concat(newIme, 'x'.ToString());
                        break;
                    case 'C':
                        newIme = string.Concat(newIme, 'C'.ToString());
                        break;
                    case 'c':
                        newIme = string.Concat(newIme, 'c'.ToString());
                        break;
                    case 'Č':
                        newIme = string.Concat(newIme, "Č".ToString());
                        break;
                    case 'č':
                        newIme = string.Concat(newIme, "č".ToString());
                        break;
                    case 'Š':
                        newIme = string.Concat(newIme, "Š".ToString());
                        break;
                    case 'š':
                        newIme = string.Concat(newIme, "š".ToString());
                        break;
                    case '[':
                        newIme = string.Concat(newIme, '['.ToString());
                        break;
                    case '{':
                        newIme = string.Concat(newIme, '{'.ToString());
                        break;
                    case ']':
                        newIme = string.Concat(newIme, ']'.ToString());
                        break;
                    case '}':
                        newIme = string.Concat(newIme, '}'.ToString());
                        break;
                    case '-':
                        newIme = string.Concat(newIme, '-'.ToString());
                        break;
                    case ' ':
                        newIme = string.Concat(newIme, ' '.ToString());
                        flag = 1;
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
                    case '\t':
                        newIme = string.Concat(newIme, '\t'.ToString());
                        break;
                    case '\n':
                        newIme = string.Concat(newIme, '\n'.ToString());
                        break;

                    ///od kirilica na latinica
                    case 'А':
                        newIme = string.Concat(newIme, 'A'.ToString());
                        break;
                    case 'а':
                        newIme = string.Concat(newIme, 'a'.ToString());
                        break;
                    case 'Б':
                        newIme = string.Concat(newIme, 'B'.ToString());
                        break;
                    case 'б':
                        newIme = string.Concat(newIme, 'b'.ToString());
                        break;
                    case 'В':
                        newIme = string.Concat(newIme, 'V'.ToString());
                        break;
                    case 'в':
                        newIme = string.Concat(newIme, 'v'.ToString());
                        break;
                    case 'Г':
                        newIme = string.Concat(newIme, 'G'.ToString());
                        break;
                    case 'г':
                        newIme = string.Concat(newIme, 'g'.ToString());
                        break;
                    case 'Д':
                        newIme = string.Concat(newIme, 'D'.ToString());
                        break;
                    case 'д':
                        newIme = string.Concat(newIme, 'd'.ToString());
                        break;
                    case 'Ѓ':
                        newIme = string.Concat(newIme, "Đ".ToString());
                        break;
                    case 'ѓ':
                        newIme = string.Concat(newIme, "đ".ToString());
                        break;
                    case 'Е':
                        newIme = string.Concat(newIme, 'E'.ToString());
                        break;
                    case 'е':
                        newIme = string.Concat(newIme, 'e'.ToString());
                        break;
                    case 'Ж':
                        newIme = string.Concat(newIme, "Ž".ToString());
                        break;
                    case 'ж':
                        newIme = string.Concat(newIme, "ž".ToString());
                        break;
                    case 'З':
                        newIme = string.Concat(newIme, 'Z'.ToString());
                        break;
                    case 'з':
                        newIme = string.Concat(newIme, 'z'.ToString());
                        break;
                    case 'Ѕ':
                        newIme = string.Concat(newIme, "Dz".ToString());
                        break;
                    case 'ѕ':
                        newIme = string.Concat(newIme, "dz".ToString());
                        break;
                    case 'И':
                        newIme = string.Concat(newIme, 'I'.ToString());
                        break;
                    case 'и':
                        newIme = string.Concat(newIme, 'i'.ToString());
                        break;
                    case 'Ј':
                        newIme = string.Concat(newIme, 'J'.ToString());
                        break;
                    case 'ј':
                        newIme = string.Concat(newIme, 'j'.ToString());
                        break;
                    case 'К':
                        newIme = string.Concat(newIme, 'K'.ToString());
                        break;
                    case 'к':
                        newIme = string.Concat(newIme, 'k'.ToString());
                        break;
                    case 'Л':
                        newIme = string.Concat(newIme, 'L'.ToString());
                        break;
                    case 'л':
                        newIme = string.Concat(newIme, 'l'.ToString());
                        break;
                    case 'Љ':
                        newIme = string.Concat(newIme, "Lj".ToString());
                        break;
                    case 'љ':
                        newIme = string.Concat(newIme, "lj".ToString());
                        break;
                    case 'М':
                        newIme = string.Concat(newIme, 'M'.ToString());
                        break;
                    case 'м':
                        newIme = string.Concat(newIme, 'm'.ToString());
                        break;
                    case 'Н':
                        newIme = string.Concat(newIme, 'N'.ToString());
                        break;
                    case 'н':
                        newIme = string.Concat(newIme, 'n'.ToString());
                        break;
                    case 'Њ':
                        newIme = string.Concat(newIme, "Nj".ToString());
                        break;
                    case 'њ':
                        newIme = string.Concat(newIme, "nj".ToString());
                        break;
                    case 'О':
                        newIme = string.Concat(newIme, 'O'.ToString());
                        break;
                    case 'о':
                        newIme = string.Concat(newIme, 'o'.ToString());
                        break;
                    case 'П':
                        newIme = string.Concat(newIme, 'P'.ToString());
                        break;
                    case 'п':
                        newIme = string.Concat(newIme, 'p'.ToString());
                        break;
                    case 'Р':
                        newIme = string.Concat(newIme, 'R'.ToString());
                        break;
                    case 'р':
                        newIme = string.Concat(newIme, 'r'.ToString());
                        break;
                    case 'С':
                        newIme = string.Concat(newIme, 'S'.ToString());
                        break;
                    case 'с':
                        newIme = string.Concat(newIme, 's'.ToString());
                        break;
                    case 'Т':
                        newIme = string.Concat(newIme, 'T'.ToString());
                        break;
                    case 'т':
                        newIme = string.Concat(newIme, 't'.ToString());
                        break;
                    case 'Ќ':
                        newIme = string.Concat(newIme, "Ć".ToString());
                        break;
                    case 'ќ':
                        newIme = string.Concat(newIme, "ć".ToString());
                        break;
                    case 'У':
                        newIme = string.Concat(newIme, 'U'.ToString());
                        break;
                    case 'у':
                        newIme = string.Concat(newIme, 'u'.ToString());
                        break;
                    case 'Ф':
                        newIme = string.Concat(newIme, 'F'.ToString());
                        break;
                    case 'ф':
                        newIme = string.Concat(newIme, 'f'.ToString());
                        break;
                    case 'Х':
                        newIme = string.Concat(newIme, 'H'.ToString());
                        break;
                    case 'х':
                        newIme = string.Concat(newIme, 'h'.ToString());
                        break;
                    case 'Ц':
                        newIme = string.Concat(newIme, 'C'.ToString());
                        break;
                    case 'ц':
                        newIme = string.Concat(newIme, 'c'.ToString());
                        break;
                    case 'Ч':
                        newIme = string.Concat(newIme, "Č".ToString());
                        break;
                    case 'ч':
                        newIme = string.Concat(newIme, "č".ToString());
                        break;
                    case 'Џ':
                        newIme = string.Concat(newIme, "Đ".ToString());
                        break;
                    case 'џ':
                        newIme = string.Concat(newIme, "đ".ToString());
                        break;
                    case 'Ш':
                        newIme = string.Concat(newIme, "Š".ToString());
                        break;
                    case 'ш':
                        newIme = string.Concat(newIme, "š".ToString());
                        break;
                    case '„':
                        newIme = string.Concat(newIme, '"'.ToString());
                        break;
                    case '“':
                        newIme = string.Concat(newIme, '"'.ToString());
                        break;


                    delault: break;
                }
                //    char c = newIme[0];
                //  c = char.ToUpper(c);
                //string krajnoIme = c.ToString();
                //newIme.ToUpper()
                //if (i == 0)
               //     newIme = newIme.ToUpper();

               // if (flag == 2) {
                //    string c = newIme[i].ToString();
                //    c = c.ToUpper();
                //    newIme = newIme.Remove(newIme.Length - 1);
                //    newIme = string.Concat(newIme, c);
                //    flag = 0;
                //} else if (flag == 1)
                //    flag++;

            }
            return newIme;
        }
    }
}