using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using System.Globalization;
using Broker.DataAccess;
namespace Broker.Controllers.ReportControllers {
    /// <summary>
    /// Klasa za kreiranje na PDF fajlovi
    /// </summary>
    public class PDFCreators {

        public enum FontsEnum {
            Arial,
            ArialB,
            ArialBI,
            ArialI,
            TimesNewRoman,
            TimesNewRomanI,
            TimesNewRomanB,
            TimesNewRomanBI
        }


        //Fontovi koio se koristat vo izvestaite vo PDF
        private const string FONTPATH = @"C:\Windows\Fonts\Arial.ttf";
        private const string FONTPATHbold = @"C:\Windows\Fonts\ArialBD.ttf";
        private const string FONTPATHTNRbold = @"C:\Windows\Fonts\TimesBD.ttf";
        private const string FONTPATHTNR = @"C:\Windows\Fonts\Times.ttf";
        private Document doc;
        private PdfWriter writer;
        private Font font;
        private Font fontBold;
        private Font croatianFont;
        private Font croatianFontBold;
        private BaseFont bf;
        private BaseFont bfBold;
        private BaseFont croatianBaseFont;
        private BaseFont croatianBaseFontBold;
        private float fontSize;
        Rectangle pageSize;
        Table table;
        int tableColumns;
        string[] headerNames;
        string branchName;
        MemoryStream memStream;
        PdfContentByte pcb;
        private bool orientation;

        public PDFCreators(bool orientation, float marginLeft, float marginRight, float marginTop, float marginBottom) {
            this.orientation = orientation;
            pageSize = PageSize.A4;
            if (!orientation)
                pageSize = pageSize.Rotate();
            fontSize = 9;
            bf = BaseFont.CreateFont(FONTPATH, "Cp1251", true);
            font = new Font(bf, fontSize);
            bfBold = BaseFont.CreateFont(FONTPATHbold, "Cp1251", true);
            fontBold = new Font(bfBold, 9);
            doc = new Document(pageSize, marginLeft, marginRight, marginTop, marginBottom);
            memStream = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, memStream);
        }

        public PDFCreators(bool orientation, float marginLeft, float marginRight, float marginTop, float marginBottom, bool isForBill) {
            this.orientation = orientation;
            if (!isForBill) {
                pageSize = PageSize.A4;
            } else {
                pageSize = PageSize.A4;
            }
            if (!orientation)
                pageSize = pageSize.Rotate();
            fontSize = 9;
            bf = BaseFont.CreateFont(FONTPATH, "Cp1251", true);
            font = new Font(bf, fontSize);
            bfBold = BaseFont.CreateFont(FONTPATHbold, "Cp1251", true);
            fontBold = new Font(bfBold, 9);
            doc = new Document(pageSize, marginLeft, marginRight, marginTop, marginBottom);
            memStream = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, memStream);
        }

        public void GetContentByte() {
            pcb = writer.DirectContent;
        }

        public PDFCreators(float marginLeft, float marginRight, float marginTop, float marginBottom) {
            pageSize = PageSize.A4;
            //pageSize = pageSize.Rotate();
            fontSize = 9;
            bf = BaseFont.CreateFont(FONTPATH, "Cp1251", true);
            font = new Font(bf, fontSize);
            bfBold = BaseFont.CreateFont(FONTPATHbold, "Cp1251", true);
            fontBold = new Font(bfBold, fontSize);
            croatianBaseFont = BaseFont.CreateFont(FONTPATHTNR, "Cp1250", true);
            croatianFont = new Font(croatianBaseFont, fontSize);
            croatianBaseFontBold = BaseFont.CreateFont(FONTPATHTNRbold, "Cp1250", true);
            croatianFontBold = new Font(croatianBaseFontBold, fontSize);
            doc = new Document(pageSize, marginLeft, marginRight, marginTop, marginBottom);
            memStream = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, memStream);
            OpenPDF();
            pcb = writer.DirectContent;
        }

        public PDFCreators(float marginLeft, float marginRight, float marginTop, float marginBottom, string page_Size) {
            if (page_Size == "A3") {
                pageSize = PageSize.A3;
            } else {
                pageSize = PageSize.A4;
            }
            //pageSize = pageSize.Rotate();
            fontSize = 9;
            bf = BaseFont.CreateFont(FONTPATH, "Cp1251", true);
            font = new Font(bf, fontSize);
            bfBold = BaseFont.CreateFont(FONTPATHbold, "Cp1251", true);
            fontBold = new Font(bfBold, fontSize);
            croatianBaseFont = BaseFont.CreateFont(FONTPATHTNR, "Cp1250", true);
            croatianFont = new Font(croatianBaseFont, fontSize);
            croatianBaseFontBold = BaseFont.CreateFont(FONTPATHTNRbold, "Cp1250", true);
            croatianFontBold = new Font(croatianBaseFontBold, fontSize);
            doc = new Document(pageSize, marginLeft, marginRight, marginTop, marginBottom);
            memStream = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, memStream);
            OpenPDF();
            pcb = writer.DirectContent;
        }

        public void OpenPDF() {
            doc.Open();
        }
        public void FinishPDF() {
            doc.Close();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.Charset = string.Empty;
            HttpContext.Current.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            HttpContext.Current.Response.AddHeader("Content-Disposition:", "attachment; filename=Report.pdf");
            HttpContext.Current.Response.OutputStream.Write(memStream.GetBuffer(), 0, memStream.GetBuffer().Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.OutputStream.Close();
            HttpContext.Current.Response.End();
        }

        public void FinishPDF_FileName(string nameoffile) {
            doc.Close();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.Charset = string.Empty;
            HttpContext.Current.Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            HttpContext.Current.Response.AddHeader("Content-Disposition:", "attachment; filename=" + nameoffile + ".pdf");
            HttpContext.Current.Response.OutputStream.Write(memStream.GetBuffer(), 0, memStream.GetBuffer().Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.OutputStream.Close();
            HttpContext.Current.Response.End();
        }

        public void SetDocumentHeaderFooter(string headerText) {
            HeaderFooter header = new HeaderFooter(new Phrase(headerText, font), false);
            HeaderFooter footer = new HeaderFooter(new Phrase("Страница ", font), true);
            header.BorderWidthTop = 0;
            header.BorderWidthBottom = 0.5f;
            footer.BorderWidthBottom = 0;
            footer.BorderWidthTop = 0.5f;
            footer.SetAlignment("RIGHT");
            doc.Header = header;
            doc.Footer = footer;
        }

        public void SetDocumentHeaderFooter() {
            HeaderFooter footer = new HeaderFooter(new Phrase(Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.ADRESA).Value + " " +
                Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.MESTO).Value + "\n" +
                "Тел.  : " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.TELEFON).Value + "\n" +
                "Факс  : " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.FAKS).Value + "\n" +
                "e-mail: " + Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.EMAIL).Value + "\n" +
                Broker.DataAccess.Parameter.GetByCode(Broker.DataAccess.Parameter.WEB_STRANA).Value, font), false);
            footer.BorderWidthBottom = 0;
            footer.BorderWidthTop = 0.5f;
            footer.SetAlignment("LEFT");
            doc.Footer = footer;
        }

        public void SetTitle(string title) {
            Paragraph p = new Paragraph(title, new Font(bf, 14, Font.BOLD));
            p.Alignment = 1;
            doc.Add(p);
        }

        public void SetTitleJustified(string title) {
            Paragraph p = new Paragraph(title, new Font(bf, 10, Font.NORMAL));
            p.Alignment = Element.ALIGN_JUSTIFIED;
            doc.Add(p);
        }

        public void SetTitleLeftBold16(string title)
        {
            Paragraph p = new Paragraph(title, new Font(bf, 16, Font.BOLD));
            p.Alignment = 0;
            doc.Add(p);
        }

        public void SetTitleLeftBold14(string title)
        {
            Paragraph p = new Paragraph(title, new Font(bf, 14, Font.BOLD));
            p.Alignment = 0;
            doc.Add(p);
        }

        public void SetTitleRightBold14(string title)
        {
            Paragraph p = new Paragraph(title, new Font(bf, 14, Font.BOLD));
            p.Alignment = 2;
            doc.Add(p);
        }

        public void SetTitleLeft10(string title)
        {
            Paragraph p = new Paragraph(title, new Font(bf, 10, Font.NORMAL));
            p.Alignment = 0;
            doc.Add(p);
        }

        public void SetTitleLeft8(string title)
        {
            Paragraph p = new Paragraph(title, new Font(bf, 8, Font.NORMAL));
            p.Alignment = 0;
            doc.Add(p);
        }

        public void SetTitleLeft10Bold(string title)
        {
            Paragraph p = new Paragraph(title, new Font(bf, 10, Font.BOLD));
            p.Alignment = 0;
            doc.Add(p);
        }

        public void SetTitleLeftItalic10(string title)
        {
            Paragraph p = new Paragraph(title, new Font(bf, 10, Font.ITALIC));
            p.Alignment = 0;
            doc.Add(p);
        }

        public void SetTitleCenterForFactureNumber(string title)
        {
            Paragraph p = new Paragraph(title, new Font(bf, 18, Font.BOLD));
            p.Alignment = 1;
            doc.Add(p);
        }

        public void SetTitleCenter8(string title) {
            Paragraph p = new Paragraph(title, new Font(bf, 8, Font.NORMAL));
            p.Alignment = 1;
            doc.Add(p);
        }

        public void SetTitleLeftWithFontSize10(string title) {
            Paragraph p = new Paragraph(title, new Font(bf, 10, Font.BOLD));
            p.Alignment = 0;
            doc.Add(p);
        }
        //public void SetTitleLeftWithFontSize12(string title) {
        //    Paragraph p = new Paragraph(title, new Font(bf, 10, Font.BOLD));
        //    p.Alignment = 0;
        //    doc.Add(p);
        //}

        public void SetTitleLeft(string title) {
            Paragraph p = new Paragraph(title, new Font(bf, 12, Font.BOLD));
            p.Alignment = 0;
            doc.Add(p);
        }


        public void SetTitleRightWithFontSize10(string title) {
            Paragraph p = new Paragraph(title, new Font(bf, 10, Font.BOLD));
            p.Alignment = 2;
            doc.Add(p);
        }

        public void SetTitleRightWithFontSize9(string title) {
            Paragraph p = new Paragraph(title, new Font(bf, 9, Font.BOLD));
            p.Alignment = 2;
            doc.Add(p);
        }

        public void SetTitleRight(string title) {
            Paragraph p = new Paragraph(title, new Font(bf, 12, Font.BOLD));
            p.Alignment = 2;
            doc.Add(p);
        }

        public void SetTitleLeftSmall(string title) {
            Paragraph p = new Paragraph(title, new Font(bf, 8, Font.NORMAL));
            p.Alignment = 0;
            doc.Add(p);
        }


        public void CreateTable(int columns, bool compDateSignature, string[] headerNames, string branchName) {
            tableColumns = columns;
            this.headerNames = headerNames;
            this.branchName = branchName;
            table = new Table(tableColumns);
            table.TableFitsPage = true;
            table.BorderWidth = 1;
            table.BorderWidthLeft = 1;
            table.BorderWidthBottom = 1;
            table.BorderWidthRight = 1;
            table.BorderWidthTop = 1;
            table.Padding = 0;
            table.Spacing = 0;
            table.Cellpadding = 2;
            table.Cellspacing = 0;
            table.Width = 100;
            table.DefaultCellBorderWidth = 0;
            Cell c;
            if (compDateSignature) {
                Table pomtable = new Table(2, 2);
                pomtable.TableFitsPage = true;
                pomtable.BorderWidth = 0;
                pomtable.Spacing = 0;
                pomtable.Padding = 0;
                pomtable.Cellpadding = 2;
                pomtable.Cellspacing = 0;
                pomtable.Width = 100;
                pomtable.Widths = new float[] { 30, 70 };
                c = new Cell(new Chunk("Датум: ", new Font(bf, 9)));
                c.BorderWidth = 0;
                c.HorizontalAlignment = Element.ALIGN_RIGHT;
                pomtable.AddCell(c);
                c = new Cell(new Chunk(DateTime.Now.ToShortDateString(), new Font(bf, 9)));
                c.BorderWidth = 0;
                pomtable.AddCell(c);
                c = new Cell(new Chunk("Брокерско друштво: ", new Font(bf, 9)));
                c.BorderWidth = 0;
                c.HorizontalAlignment = Element.ALIGN_RIGHT;
                pomtable.AddCell(c);
                c = new Cell(new Chunk(branchName, new Font(bf, 9, Font.BOLD)));
                c.BorderWidth = 0;
                pomtable.AddCell(c);
                doc.Add(pomtable);
            }

            for (int i = 0; i < columns; i++) {
                c = new Cell(new Chunk(headerNames[i], new Font(bf, 9, Font.BOLD)));
                c.Header = true;
                c.UseAscender = true;
                c.HorizontalAlignment = Element.ALIGN_CENTER;
                c.BorderWidthLeft = 0.5f;
                c.BorderWidthRight = 0.5f;
                c.BorderWidthBottom = 0.5f;
                c.BorderWidthTop = 0.5f;
                table.AddCell(c);
            }
            table.EndHeaders();
        }

        public void CreateTable(int columns, bool compDateSignature, string[] headerNames, string branchName, float[] widthPercentagePerColumn) {
            tableColumns = columns;
            this.headerNames = headerNames;
            this.branchName = branchName;
            table = new Table(tableColumns);
            table.TableFitsPage = true;
            table.BorderWidth = 1;
            table.BorderWidthLeft = 1;
            table.BorderWidthBottom = 1;
            table.BorderWidthRight = 1;
            table.BorderWidthTop = 1;
            table.Padding = 0;
            table.Spacing = 0;
            table.Cellpadding = 2;
            table.Widths = widthPercentagePerColumn;
            table.Cellspacing = 0;
            table.Width = 100;
            table.DefaultCellBorderWidth = 0;
            Cell c;

            for (int i = 0; i < columns; i++) {
                c = new Cell(new Chunk(headerNames[i], new Font(bf, 10, Font.BOLD)));
                c.Header = true;
                c.UseAscender = true;
                c.HorizontalAlignment = Element.ALIGN_CENTER;
                c.BorderWidthLeft = 0.5f;
                c.BorderWidthRight = 0.5f;
                c.BorderWidthBottom = 0.5f;
                c.BorderWidthTop = 0.5f;
                table.AddCell(c);
            }
            table.EndHeaders();
        }


        public void CreateTableWithBorder(int columns, bool compDateSignature, string[] headerNames, string branchName, float[] widthPercentagePerColumn) {
            tableColumns = columns;
            this.headerNames = headerNames;
            this.branchName = branchName;
            table = new Table(tableColumns);

            table.TableFitsPage = true;
            //table.BorderWidth = 0;
            //table.BorderWidthLeft = 0;
            table.BorderWidth = 0.5f;
            table.Padding = 0;
            table.Spacing = 0;
            table.Cellpadding = 2;
            table.Widths = widthPercentagePerColumn;
            table.Cellspacing = 0;
            table.Width = 100;
            table.DefaultCellBorderWidth = 0;
            Cell c;

            for (int i = 0; i < columns; i++) {
                c = new Cell(new Chunk(headerNames[i], new Font(bf, 10, Font.BOLD)));
                c.Header = true;
                c.HorizontalAlignment = Element.ALIGN_CENTER;
                c.UseAscender = true;
                c.BorderWidth = 0.5f;
                table.AddCell(c);
            }
            table.EndHeaders();
        }

        public void CreateTableForSkadencar(int columns, bool compDateSignature, string[] headerNames, string branchName, float[] widthPercentagePerColumn) {
            tableColumns = columns;
            this.headerNames = headerNames;
            this.branchName = branchName;
            table = new Table(tableColumns);

            table.TableFitsPage = true;
            //table.BorderWidth = 0;
            //table.BorderWidthLeft = 0;
            table.BorderWidth = 0.5f;
            table.Padding = 0;
            table.Spacing = 0;
            table.Cellpadding = 2;
            table.Widths = widthPercentagePerColumn;
            table.Cellspacing = 0;
            table.Width = 100;
            table.DefaultCellBorderWidth = 0;
            Cell c;

            for (int i = 0; i < columns; i++) {
                c = new Cell(new Chunk(headerNames[i], new Font(bf, 9, Font.NORMAL)));
                c.Header = true;
                c.HorizontalAlignment = Element.ALIGN_LEFT;
                c.UseAscender = true;
                c.BorderWidth = 0.5f;
                table.AddCell(c);
            }
            table.EndHeaders();
        }

        public void CreateTableWithFontSize(int columns, string[] headerNames, float[] widthPercentagePerColumn, float headerFontSize) {
            tableColumns = columns;
            //this.headerNames = headerNames;
            //this.branchName = branchName;
            table = new Table(tableColumns);

            table.TableFitsPage = true;
            table.BorderWidth = 1;
            table.BorderWidthLeft = 1;
            table.BorderWidthBottom = 1;
            table.BorderWidthRight = 1;
            table.BorderWidthTop = 1;
            table.Padding = 0;
            table.Spacing = 0;
            table.Cellpadding = 2;
            table.Widths = widthPercentagePerColumn;
            table.Cellspacing = 0;
            table.Width = 100;
            table.DefaultCellBorderWidth = 0;
            Cell c;

            for (int i = 0; i < columns; i++) {
                c = new Cell(new Chunk(headerNames[i], new Font(bf, headerFontSize, Font.NORMAL)));
                c.Header = true;
                c.UseAscender = true;
                c.HorizontalAlignment = Element.ALIGN_CENTER;
                c.BorderWidthLeft = 0.5f;
                c.BorderWidthRight = 0.5f;
                c.BorderWidthBottom = 1;
                c.BorderWidthTop = 0.5f;
                table.AddCell(c);
            }
            table.EndHeaders();
        }


        public void CreateTable_Facture(int columns, string[] headerNames, float[] widthPercentagePerColumn) {
            tableColumns = columns;
            //this.headerNames = headerNames;
            //this.branchName = branchName;
            table = new Table(tableColumns);

            table.TableFitsPage = true;
            table.BorderWidth = 1;
            table.BorderWidthLeft = 1;
            table.BorderWidthBottom = 1;
            table.BorderWidthRight = 1;
            table.BorderWidthTop = 1;
            table.Padding = 0;
            table.Spacing = 0;
            table.Cellpadding = 2;
            table.Widths = widthPercentagePerColumn;
            table.Cellspacing = 0;
            table.Width = 100;
            table.DefaultCellBorderWidth = 0;
            Cell c;

            for (int i = 0; i < columns; i++) {
                c = new Cell(new Chunk(headerNames[i], new Font(bf, 10, Font.BOLD)));
                c.Header = true;
                c.UseAscender = true;
                c.HorizontalAlignment = Element.ALIGN_CENTER;
                c.BorderWidthLeft = 0.5f;
                c.BorderWidthRight = 0.5f;
                c.BorderWidthBottom = 1;
                c.BorderWidthTop = 0.5f;
                table.AddCell(c);
            }
            table.EndHeaders();
        }

        public void CreateTableForBill(int columns, float[] widthPercentagePerColumn, float widthPercentage) {
            tableColumns = columns;
            table = new Table(tableColumns);
            table.Alignment = 0;
            table.TableFitsPage = true;
            table.BorderWidth = 0;
            table.BorderWidthLeft = 0;
            table.Padding = 0;
            table.Spacing = 0;
            table.Cellpadding = 0;
            table.Widths = widthPercentagePerColumn;
            table.Cellspacing = 0;
            table.Width = widthPercentage;
            table.DefaultCellBorderWidth = 0;
            Cell c;
        }

        public void AddDataRowForBill(object[] values, int n) {
            Cell c;

            for (int i = 0; i < n; i++) {
                if (values[i] != null)
                    c = new Cell(new Chunk(values[i].ToString(), font));
                else
                    c = new Cell();
                c.VerticalAlignment = Element.ALIGN_LEFT;
                c.BorderWidth = 0;
                c.UseAscender = true;
                if (i < n - 1)
                    c.BorderWidthRight = 0;
                table.AddCell(c);
            }

        }

        public void AddDataRowForBillWithRigthAlligmentForValues(object[] values, int n) {
            Cell c;

            for (int i = 0; i < n; i++) {
                if (values[i] != null)
                    c = new Cell(new Chunk(values[i].ToString(), font));
                else
                    c = new Cell();
                c.BorderWidth = 0.5f;
                c.BorderWidthBottom = 0.5f;
                c.BorderWidthLeft = 0.5f;
                c.BorderWidthTop = 0.5f;
                c.BorderWidthRight = 0.5f;
                c.UseAscender = true;
                if (i < n - 1) {
                    c.BorderWidth = 0.5f;
                    c.BorderWidthBottom = 0.5f;
                    c.BorderWidthLeft = 0.5f;
                    c.BorderWidthTop = 0.5f;
                    c.BorderWidthRight = 0.5f;
                }
                if (i == n - 1) {
                    c.VerticalAlignment = Element.ALIGN_RIGHT;
                    c.HorizontalAlignment = Element.ALIGN_RIGHT;
                } else {
                    c.VerticalAlignment = Element.ALIGN_LEFT;
                    c.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                table.AddCell(c);
            }

        }


        /// <summary>
        /// Metod koj vraka vrednost vitina ako tipot na kodot za opredelena 
        /// promenliva e 16 biten integer, 32 biten integer, 64 biten integer,
        /// ili decimalen broj
        /// </summary>
        /// <param name="code"></param>
        /// <returns>bool</returns>
        private bool IsNumber(TypeCode code) {
            return (code == TypeCode.Int16 ||
                    code == TypeCode.Int32 ||
                    code == TypeCode.Int64 ||
                    code == TypeCode.Decimal ||
                    code == TypeCode.Double);
        }

        public bool AddDataRow(object[] values, int n, TypeCode[] codes) {
            Cell c;

            for (int i = 0; i < n; i++) {
                if (values[i] != null) {
                    c = new Cell(new Chunk(values[i].ToString(), font));
                } else {
                    c = new Cell();
                }
                if (IsNumber(codes[i])) {
                    c.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                c.VerticalAlignment = Element.ALIGN_MIDDLE;
                c.UseAscender = true;
                c.BorderWidth = 0.5f;
                table.AddCell(c);
            }
            if (!writer.FitsPage(table)) {
                table.DeleteLastRow();
                table.Offset = 0;
                doc.Add(table);
                doc.NewPage();
                CreateTable(tableColumns, false, headerNames, branchName);
                return true;
            }
            return false;
        }

        public bool AddDataRowForFactures(object[] values, int n, TypeCode[] codes)
        {
            Cell c;

            for (int i = 0; i < n; i++)
            {
                if (values[i] != null)
                {
                    c = new Cell(new Chunk(values[i].ToString(), font));
                } else
                {
                    c = new Cell();
                }
                if (IsNumber(codes[i]))
                {
                    c.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                c.VerticalAlignment = Element.ALIGN_MIDDLE;
                c.UseAscender = true;
                c.BorderWidth = 0.5f;
                table.AddCell(c);
            }
            //if (!writer.FitsPage(table))
            //{
            //    table.DeleteLastRow();
            //    table.Offset = 0;
            //    doc.Add(table);
            //    doc.NewPage();
            //    CreateTable(tableColumns, false, headerNames, branchName);
            //    return true;
            //}
            return false;
        }


        public bool AddDataRowForBrokerageSlips(object[] values, int n, TypeCode[] codes) {
            Cell c;

            for (int i = 0; i < n; i++) {
                if (values[i] != null) {
                    c = new Cell(new Chunk(values[i].ToString(), font));
                } else {
                    c = new Cell();
                }
                if (IsNumber(codes[i])) {
                    c.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                c.VerticalAlignment = Element.ALIGN_MIDDLE;
                //c.UseAscender = true;
                c.BorderWidth = 0.5f;
                table.AddCell(c);
            }
            return false;
        }


        public bool AddDataRowForFacturesExtend(object[] values, int n, TypeCode[] codes, string[] headers, float[] widthPercentage)
        {
            Cell c;

            for (int i = 0; i < n; i++)
            {
                if (values[i] != null)
                {
                    c = new Cell(new Chunk(values[i].ToString(), font));
                } else
                {
                    c = new Cell();
                }
                if (IsNumber(codes[i]))
                {
                    c.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                c.VerticalAlignment = Element.ALIGN_MIDDLE;
                c.UseAscender = true;
                c.BorderWidth = 0.5f;
                table.AddCell(c);
            }
            //if (!writer.FitsPage(table))
            //{
                //table.DeleteLastRow();
                //table.Offset = 0;
                //doc.Add(table);
                //doc.NewPage();
                //CreateTable_Facture(headers.Length, headers, widthPercentage);
                //return true;
            //}
            return false;
        }


        public bool AddDataRow(object[] values, int n) {
            Cell c;

            for (int i = 0; i < n; i++) {
                if (values[i] != null) {
                    c = new Cell(new Chunk(values[i].ToString(), font));
                } else {
                    c = new Cell();
                }
                c.VerticalAlignment = Element.ALIGN_MIDDLE;
                c.UseAscender = true;
                c.BorderWidth = 0.5f;
                table.AddCell(c);
            }
            if (!writer.FitsPage(table)) {
                table.DeleteLastRow();
                table.Offset = 0;
                doc.Add(table);
                doc.NewPage();
                CreateTable(tableColumns, false, headerNames, branchName);
                return true;
            }
            return false;
        }

        public void AddDataRow1(object[] values, int n, TypeCode[] codes){
            Cell c;
            for (int i = 0; i < n; i++) {
                if (values[i] != null) {
                    c = new Cell(new Chunk(values[i].ToString(), font));
                } else {
                    c = new Cell();
                }
                if (IsNumber(codes[i])) {
                    c.HorizontalAlignment = Element.ALIGN_RIGHT;
                } 
                if(codes[i] == TypeCode.DateTime){
                    c.HorizontalAlignment = Element.ALIGN_CENTER;
                }
                c.VerticalAlignment = Element.ALIGN_MIDDLE;
                c.UseAscender = true;
                c.BorderWidth = 0.5f;
                
                table.AddCell(c);
            }

        }
        // Adds row in table with borders aroun cells
        public void AddDataRowWithBorder(object[] values, int n, TypeCode[] codes) {
            Cell c;
            for (int i = 0; i < n; i++) {
                if (values[i] != null) {
                    c = new Cell(new Chunk(values[i].ToString(), font));
                } else {
                    c = new Cell();
                }
                if (IsNumber(codes[i])) {
                    c.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                if (codes[i] == TypeCode.DateTime) {
                    c.HorizontalAlignment = Element.ALIGN_CENTER;
                }
                c.VerticalAlignment = Element.ALIGN_MIDDLE;
                c.UseAscender = true;
                c.BorderWidth = 0.5f;
                //if (i < n - 1)
                //    c.BorderWidthRight = 0.5f;
                table.AddCell(c);
            }

        }


        public void AddDataRow1(object[] values, int n) {
            Cell c;

            for (int i = 0; i < n; i++) {
                if (values[i] != null)
                    c = new Cell(new Chunk(values[i].ToString(), font));
                else
                    c = new Cell();
                c.VerticalAlignment = Element.ALIGN_MIDDLE;
                c.UseAscender = true;
                c.BorderWidth = 0.5f;
                table.AddCell(c);
            }

        }


        public void AddTable() {
            doc.Add(table);
        }

        public void AddAbsoluteText(string text, int x, int y, int alignement, float _fontSize) {
            pcb.BeginText();
            this.fontSize = _fontSize;
            pcb.SetFontAndSize(bf, fontSize);
            pcb.ShowTextAligned(alignement, text, x, y, 0);
            pcb.EndText();
        }

        public void AddAbsoluteCroatianText(string text, int x, int y, int alignement, float _fontSize) {
            pcb.BeginText();
            this.fontSize = _fontSize;
            BaseFont base_font = BaseFont.CreateFont(FONTPATH, "Cp1250", true);
            pcb.SetFontAndSize(base_font, fontSize);
            pcb.ShowTextAligned(alignement, text, x, y, 0);
            pcb.EndText();
        }

        public void AddAbsoluteBoldText(string text, int x, int y, int alignement, float _fontSize) {
            pcb.BeginText();
            this.fontSize = _fontSize;
            pcb.SetFontAndSize(bfBold, fontSize);
            pcb.ShowTextAligned(alignement, text, x, y, 0);
            pcb.EndText();
        }

        public void AddASUCLogo(int x_position, int y_position) {
            Image image = Image.GetInstance(@"C:\Temp\logo_ASUC.gif");
            image.SetAbsolutePosition(x_position, y_position);
            doc.Add(image);
        }

        public void AddJDBLogo(int x_position, int y_position)
        {
            string physicalPath = AttachmentController.ApplicationPath();
            string imageDirectoryPath = physicalPath + @"\_assets\img\imgInApp\";
            BrokerHouseInformation bhi = BrokerHouseInformation.GetInformation();
            string imagePath = imageDirectoryPath + bhi.BrokerHouseLogoRelativeUrl;
            Image image = Image.GetInstance(imagePath);
            image.SetAbsolutePosition(x_position, y_position);
            doc.Add(image);
        }

        public void AddJDBLogoForFactures(int x_position, int y_position)
        {
            string physicalPath = AttachmentController.ApplicationPath();
            string imageDirectoryPath = physicalPath + @"\_assets\img\imgInApp\";
            string fileName = @"euromak_td.png";
            //BrokerHouseInformation bhi = BrokerHouseInformation.GetInformation();
            string imagePath = imageDirectoryPath + fileName;
            Image image = Image.GetInstance(System.Drawing.Image.FromFile(imagePath), System.Drawing.Imaging.ImageFormat.Png);
            image.ScaleToFit(220f, 138f);
            image.SetAbsolutePosition(x_position, y_position);
            doc.Add(image);
        }

        public void AddPage() {
            doc.NewPage();
        }



        public void AddCroatianAbsoluteBoldText(string text, int x, int y, int alignement, float _fontSize) {
            pcb.BeginText();
            this.fontSize = _fontSize;
            pcb.SetFontAndSize(croatianBaseFontBold, fontSize);
            pcb.ShowTextAligned(alignement, text, x, y, 0);
            pcb.EndText();
        }

        public void AddAbsoluteTextForReport(string text, int x, int y, int alignement, float _fontSize, FontsEnum _font) {
            pcb.BeginText();
            this.fontSize = _fontSize;
            pcb.SetFontAndSize(bf, fontSize);
            string FONT_PATH = "";
            if (_font == FontsEnum.Arial) {
                FONT_PATH = @"C:\Windows\Fonts\Arial.ttf";
            } else if (_font == FontsEnum.ArialB) {
                FONT_PATH = @"C:\Windows\Fonts\ArialBD.ttf";
            } else if (_font == FontsEnum.ArialBI) {
                FONT_PATH = @"C:\Windows\Fonts\ArialBI.ttf";
            } else if (_font == FontsEnum.ArialI) {
                FONT_PATH = @"C:\Windows\Fonts\ArialI.ttf";
            } else if (_font == FontsEnum.TimesNewRoman) {
                FONT_PATH = @"C:\Windows\Fonts\Times.ttf";
            } else if (_font == FontsEnum.TimesNewRomanB) {
                FONT_PATH = @"C:\Windows\Fonts\TimesBD.ttf";
            } else if (_font == FontsEnum.TimesNewRomanBI) {
                FONT_PATH = @"C:\Windows\Fonts\TimesBI.ttf";
            } else if (_font == FontsEnum.TimesNewRomanI) {
                FONT_PATH = @"C:\Windows\Fonts\TimesI.ttf";
            }
            BaseFont base_font = BaseFont.CreateFont(FONT_PATH, "Cp1251", true);
            pcb.SetFontAndSize(base_font, _fontSize);
            pcb.ShowTextAligned(alignement, text, x, y, 0);
            pcb.EndText();
        }

        public void AddCroatianAbsoluteText(string text, int x, int y, int alignement, float _fontSize) {
            pcb.BeginText();
            this.fontSize = _fontSize;
            pcb.SetFontAndSize(croatianBaseFont, fontSize);
            pcb.ShowTextAligned(alignement, text, x, y, 0);
            pcb.EndText();
        }


        public void NewPage() {
            doc.NewPage();
        }


        public void AddLine(float x1, float y1, float x2, float y2) {
            PdfContentByte cb = writer.DirectContent;
            cb.MoveTo(x1, y1);
            cb.LineTo(x2, y2);
            // stroke the lines
            cb.Stroke();


        }
    }

}