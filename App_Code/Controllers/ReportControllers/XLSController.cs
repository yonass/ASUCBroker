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
using Microsoft.Office.Interop.Excel;
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
using System.IO;
using System.Threading;
using Broker.Controllers.ReportControllers;
using ControlsLibriry.Utility;



namespace MyClass.WriteToExcel
{

    public class XLSController {
        /// <summary>
        /// Default-en konstruktor
        /// </summary>
        public XLSController() {
        }
    }

    /// <summary>
    /// Klasa za rabota (ispis vo) Excel datoteka
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExcelFileWriter<T> {
        public static string FILE_PATH {
            get {
                string physicalPath = AttachmentController.ApplicationPath();
                string Path = physicalPath + @"\TEMP";
                return Path;
            }
        }

        private int columnCount;
        private int rowCount;
        object[] headerNames;
        private int myRowCnt;
        object[,] myExcelData;

        private Microsoft.Office.Interop.Excel.Application _excelApplication = null;
        private Microsoft.Office.Interop.Excel.Workbooks _workBooks = null;
        private Microsoft.Office.Interop.Excel._Workbook _workBook = null;
        private object _value = Missing.Value;
        private Microsoft.Office.Interop.Excel.Sheets _excelSheets = null;
        private Microsoft.Office.Interop.Excel._Worksheet _excelSheet = null;
        private Microsoft.Office.Interop.Excel.Range _excelRange = null;
        private Microsoft.Office.Interop.Excel.Font _excelFont = null;

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
        /// Metod so koj se vrsi polnenje na cel red vo tabelata so pole od objekti 
        /// i soodvetni vrednosti vo rangot od pocetna do krajna kolona
        /// </summary>
        /// <param name="values"></param>
        /// <param name="startColumn"></param>
        /// <param name="endColumn"></param>
        public void FillRowData_Mine(object[] values, string startColumn, string endColumn) {
            _excelRange = _excelSheet.get_Range(startColumn, endColumn);
            _excelRange.set_Value(_value, values);
            this.AddRow(startColumn, endColumn);
            //_excelRange.EntireColumn.AutoFit();
        }

        /// <summary>
        /// Interfejs za dvodimenzionalno pole od objekti (dokolku sakame ednovremeno da vnesime 
        /// cela matrica vo tabelata)
        /// </summary>
        public object[,] ExcelData {
            get {
                return myExcelData;
            }
            set {
                myExcelData = value;
            }
        }

        /// <summary>
        /// Interfejs za brojot na koloni vo tabelata vo dokumentot vo Excel
        /// </summary>
        public int ColumnCount {
            get {
                return columnCount;
            }
            set {
                columnCount = value;
            }

        }

        /// <summary>
        /// Interfejs za brojot na redovi vo tabelata vo dokumentot vo Excel
        /// </summary>
        public int RowCount {
            get {
                return rowCount;
            }
            set {
                rowCount = value;
            }
        }

        /// <summary>
        /// Metod koj vraka vrednost tocno i se povikuva pri popolnuvanjata na
        /// zaglaviata na kolonite - fontot da bide zdebelen (bold)
        /// </summary>
        protected virtual bool BoldHeaders {
            get {
                return true;
            }
        }

        /// <summary>
        /// Metod koj vrsi aktiviranje - otvoranje na nov Excel dokument vo koj se
        /// otvora nov list (sheet) vo koj ke se zapisuvaat podatocite od izvestajot
        /// </summary>
        public void ActivateExcel() {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            _excelApplication = new Microsoft.Office.Interop.Excel.Application();
            //_excelApplication.Visible = false;
            //_excelApplication.UserControl = true;
            _workBooks = (Microsoft.Office.Interop.Excel.Workbooks)_excelApplication.Workbooks;
            _workBook = (Microsoft.Office.Interop.Excel._Workbook)(_workBooks.Add(_value));
            _excelSheets = (Microsoft.Office.Interop.Excel.Sheets)_workBook.Worksheets;
            _excelSheet = (Microsoft.Office.Interop.Excel._Worksheet)(_excelSheets.get_Item(1));
        }

        /// <summary>
        /// Metod koj vrsi popolnuvanje na zaglavijata na kolonite na izvestajot
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="startColumn"></param>
        /// <param name="endColumn"></param>
        public void FillHeaderColumn(object[] headers, string startColumn, string endColumn) {
            _excelRange = _excelSheet.get_Range(startColumn, endColumn);
            _excelRange.set_Value(_value, headers);
            if (BoldHeaders == true) {
                this.BoldRow(startColumn, endColumn);
            }
            _excelRange.EntireColumn.AutoFit();
        }

        /// <summary>
        /// Popolnuvanje na polinjata vo tabelata vo dokumentot
        /// (ovoj metod momentalno ne se povikuva)
        /// </summary>
        public void FillExcelWithData() {
            _excelRange = _excelSheet.get_Range("A2", _value);
            _excelRange = _excelRange.get_Resize(RowCount + 1, ColumnCount);
            _excelRange.set_Value(Missing.Value, ExcelData);
            _excelRange.EntireColumn.AutoFit();
        }

        /// <summary>
        /// Metod koj vrsi zacuvuvanje na dokumentot pod opredeleno ime -
        /// momentalno vo direktoriumot C:\TEMP\NBOFILES
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveExcel(string fileName) {
            object fileNameToSave = FILE_PATH + "\\" + fileName;
            _workBook.SaveAs(fileNameToSave, _value, _value,
            _value, _value, _value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
            _value, _value, _value, _value, null);
            _workBook.Close(false, _value, _value);
            _excelApplication.Quit();
        }

        /// <summary>
        /// Metod koj vrsi dodavanje na red vo tabelata vo dokumentot so
        /// font koj e zdebelen (bold)
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        public void BoldRow(string row1, string row2) {
            _excelRange = _excelSheet.get_Range(row1, row2);
            _excelFont = _excelRange.Font;
            _excelFont.Bold = true;
        }

        /// <summary>
        /// Metod koj vrsi dodavanje na red vo tabelata vo dokumentot so
        /// obicen font 
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        public void AddRow(string row1, string row2) {
            _excelRange = _excelSheet.get_Range(row1, row2);
            _excelFont = _excelRange.Font;
            _excelFont.Bold = false;
        }

        /// <summary>
        /// Metod so koj dokumentot koj e iskreiran se pusta na stream za da 
        /// mozi da se otvori ili zacuva na clientska strana i se vrsi brisenje 
        /// na dokumetot od serverskiot direktorium C:\TEMP\NBOFILES
        /// </summary>
        /// <param name="fileName"></param>
        public void FinishExcel(string fileName) {
            FileStream fs = File.OpenRead(FILE_PATH + "\\" + fileName);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);
            fs.Close();
            File.Delete(FILE_PATH + "\\" + fileName);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/vnd.xls";
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
