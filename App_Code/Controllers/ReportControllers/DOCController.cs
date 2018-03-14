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
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;
//using Microsoft.Office.Core;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;
using System.Web.Caching;
using System.Web.Configuration;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;
using System.ComponentModel;
using Broker.Utility;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Threading;
using Broker.Controllers.ReportControllers;
using ControlsLibriry.Utility;


namespace MyClass.WriteToWord {

    public class DOCController {
        public DOCController() {
        }
    }

    /// <summary>
    /// Klasa za kreiranje na izvestaj vo Word dokument
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WordFileWriter<T> {

        public static string FILE_PATH {
            get {
                string physicalPath = AttachmentController.ApplicationPath();
                string Path = physicalPath + @"\TEMP";
                return Path;
            }
        }

        private Microsoft.Office.Interop.Word.Application _wordApplication = null;
        private Microsoft.Office.Interop.Word.Document _wordDocument = null;
        private object _value = Missing.Value;
        private Microsoft.Office.Interop.Word.Tables _wordTables = null;
        private Microsoft.Office.Interop.Word.Table _wordTable = null;
        private Microsoft.Office.Interop.Word.Range _wordRange = null;
        private Microsoft.Office.Interop.Word.Font _wordFont = null;
        private Microsoft.Office.Interop.Word.Cell _wordCell = null;
        private bool orientation;

        /// <summary>
        /// Interfejs od logicki tip za orientacijata na stranite
        /// vo dokumentot - dali e ispraveno-vertikalna ili legnata-horizontalna
        /// </summary>
        public bool Orientation {
            get {
                return orientation;
            }
            set {
                orientation = value;
            }
        }

        object[] headerNames;
        object[,] fillValues;

        /// <summary>
        /// Interfejs preku koj se definiraat zaglaviata na kolonite
        /// </summary>
        public object[] Headers {
            get {
                return headerNames;
            }
            set {
                headerNames = value;
            }
        }

        /// <summary>
        /// Interfejs za dvodimenzionalno pole od objekti (dokolku sakame ednovremeno da vnesime 
        /// cela matrica vo tabelata) vo Word dokumentot
        /// </summary>
        public object[,] FillValues {
            get {
                return fillValues;
            }
            set {
                fillValues = value;
            }
        }

        /// <summary>
        /// Metod koj vrsi aktiviranje - otvoranje na nov Word fajl vo koj se
        /// otvora nov dokument vo koj ke se zapisuvaat podatocite od izvestajot
        /// </summary>
        public void ActivateWord() {
            object missing = System.Reflection.Missing.Value;
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            _wordApplication = new Microsoft.Office.Interop.Word.Application();
            //_wordApplication.Visible = true;
            //_wordDocument = new Microsoft.Office.Interop.Word.Document();
            _wordDocument = _wordApplication.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            if (Orientation == false) {
                _wordDocument.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientLandscape;
            } else {
                _wordDocument.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;
            }

            _wordDocument.Activate();
        }

        
        /// <summary>
        /// Pecatenje na proizvolen tekst vo dokumentot - kako paragraf
        /// </summary>
        /// <param name="text"></param>
        public void InsertText(string text) {
            _wordApplication.Selection.TypeText(text);
            _wordApplication.Selection.TypeParagraph();
        }

        /// <summary>
        /// Pecatenje na proizvolen tekst vo dokumentot
        /// </summary>
        /// <param name="text"></param>
        public void InsertingText(string text) {
            object start = 0;
            object end = 0;
            Microsoft.Office.Interop.Word.Range rng = _wordDocument.Range(ref start, ref end);
            rng.Text = text;
        }


        /// <summary>
        /// Metod koj kreira tabela vo dokumentot vo koja se zapisuvaat 
        /// najprvo zaglavijata na kolonite, a potoa i
        /// polinjata so vrednostite za izvestajot
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        public void CreateTable(int rowNum, int colNum) {
            object start = 0;
            object end = 0;

            object styleName = "Table Grid 3";

            _wordRange = _wordApplication.ActiveDocument.Range(ref start, ref end);
            _wordApplication.ActiveDocument.Tables.Add(_wordRange, rowNum, colNum, ref _value, ref _value);
            _wordTable = _wordDocument.Tables[1];
            _wordTable.Range.Font.Size = 8;
            _wordTable.set_Style(ref styleName);

            for (int k = 0; k < colNum; k++) {
                _wordCell = _wordTable.Cell(1, k + 1);
                _wordCell.Range.Text = (string)Headers[k];
            }


            for (int i = 0; i < rowNum; i++) {
                for (int j = 0; j < colNum; j++) {
                    _wordCell = _wordTable.Cell(i + 2, j + 1);
                    _wordCell.Range.Text = Convert.ToString(FillValues[i, j]);

                }
            }

        }


        /// <summary>
        /// Metod koj vrsi vnesuvanje na Footer vo Word dokumentot 
        /// kade se pecati brojot na strana
        /// </summary>
        public void InsertFooter() {
            foreach (Microsoft.Office.Interop.Word.Section wordSection in _wordDocument.Sections) {

                wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkRed;

                wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.Font.Size = 20;
                object align = WdPageNumberAlignment.wdAlignPageNumberRight;

                wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.Add(ref align, ref _value);
               

            }
        }

        /// <summary>
        /// Metod so koj se vnesuva Header so proizvolen tekst vo dokumentot
        /// </summary>
        /// <param name="headerText"></param>
        public void InsertHeader(string headerText) {
            foreach (Microsoft.Office.Interop.Word.Section section in _wordDocument.Sections) {
                section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Text = headerText;
            }
        }


        /// <summary>
        /// Metod koj vrsi zacuvuvanje na dokumentot pod opredeleno ime -
        /// momentalno vo direktoriumot C:\TEMP\NBOFILES
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveDOC(string fileName) {
            object fileNameToSave = FILE_PATH + "\\" + fileName;
            _wordDocument.SaveAs(ref fileNameToSave, ref _value, ref _value,
                ref _value, ref _value, ref _value, ref _value,
                ref _value, ref _value, ref  _value, ref  _value, ref  _value, ref _value, ref _value, ref  _value, ref _value);


            object doNotSaveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;
            _wordDocument.Close(ref doNotSaveChanges, ref _value, ref _value);
            _wordApplication.Quit(ref doNotSaveChanges, ref _value, ref _value);
        }

        /// <summary>
        /// Metod so koj dokumentot koj e iskreiran se pusta na stream za da 
        /// mozi da se otvori ili zacuva na clientska strana i se vrsi brisenje 
        /// na dokumetot od serverskiot direktorium C:\TEMP\NBOFILES
        /// </summary>
        /// <param name="fileName"></param>
        public void FinishDOC(string fileName) {
            FileStream fs = File.OpenRead(FILE_PATH + "\\" + fileName);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);
            fs.Close();
            File.Delete(FILE_PATH + "\\" + fileName);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/vnd.ms-word";
            HttpContext.Current.Response.Charset = string.Empty;
            HttpContext.Current.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            HttpContext.Current.Response.AddHeader("Content-Disposition:", "attachment; filename=" + fileName);
            HttpContext.Current.Response.OutputStream.Write(data, 0, data.Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.OutputStream.Close();
            HttpContext.Current.Response.End();

        }



    }


}



