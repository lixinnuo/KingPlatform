using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Basic.Code
{
    public class iTextPDF
    {
        /// <summary>
        /// 导出华为订单的PDF
        /// </summary>
        public static string  CreateHWPOPDF()
        {
            Document document = new Document(PageSize.A3);
            string pdfURL = "";
            try
            {
                string pdfName = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Download/HWPO/pdf/")))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Download/HWPO/pdf/"));
                }
                string pdfPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Download/HWPO/pdf/{0}.pdf", pdfName));
                PdfWriter writer = PdfWriter.getInstance(document, new FileStream(pdfPath, FileMode.Create));

                pdfURL = System.Web.HttpContext.Current.Request.Headers["host"] + (string.Format("~/Download/HWPO/pdf/{0}.pdf", pdfName));

                HeaderFooter header = new HeaderFooter(new Phrase("page: 1"), false);
                header.Border = Rectangle.NO_BORDER;
                document.Header = header;

                document.Open();

                BaseFont ChineseSun = BaseFont.createFont(@"c:\Windows\fonts\simsun.TTC,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font font = new Font(ChineseSun, 18, Font.BOLD);
                Font font1 = new Font(ChineseSun, 12, Font.BOLD);
                Font font2 = new Font(ChineseSun, 12, Font.NORMAL);
                Font EnglishFont = FontFactory.getFont(FontFactory.TIMES_NEW_ROMAN, 12, Font.NORMAL);
                Font EnglishFont1 = FontFactory.getFont(FontFactory.TIMES_NEW_ROMAN, 14, Font.BOLD);

                Image title = Image.getInstance(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/hw_title.png"));
                document.Add(title);

                Paragraph p1 = new Paragraph(new Chunk("PO Detail Screen for PO:XXXXXX", EnglishFont1));
                document.Add(p1);

                PdfPTable ptable = new PdfPTable(4);
                ptable.DefaultCell.Border = 0;
                ptable.DefaultCell.PaddingBottom = 10;
                float[] headerwidths = { 15, 25, 25, 35 };
                ptable.setWidths(headerwidths);
                ptable.WidthPercentage = 100;
                ptable.addCell(new Phrase("PO/Rel. NO:", EnglishFont));
                ptable.addCell(new Phrase("HW20195422-106", EnglishFont));
                ptable.addCell(new Phrase("Supplier Code:", EnglishFont));
                ptable.addCell(new Phrase("021821", EnglishFont));

                ptable.addCell(new Phrase("PO Type:", EnglishFont));
                ptable.addCell(new Phrase("STANDARD", EnglishFont));
                ptable.addCell(new Phrase("Supplier:", EnglishFont));
                ptable.addCell(new Phrase("深圳金信诺高新技术股份有限公司", font2));

                ptable.addCell(new Phrase("PO/Rel. Ver.:", EnglishFont));
                ptable.addCell(new Phrase("0", EnglishFont));
                ptable.addCell(new Phrase("Total Amount(Exclude Tax):", EnglishFont));
                ptable.addCell(new Phrase("1056858.37", EnglishFont));

                ptable.addCell(new Phrase("Data Created:", EnglishFont));
                ptable.addCell(new Phrase("2014-01-06", EnglishFont));
                ptable.addCell(new Phrase("Tax Rate:", EnglishFont));
                ptable.addCell(new Phrase("0.17", EnglishFont));

                ptable.addCell(new Phrase("Last Modified:", EnglishFont));
                ptable.addCell(new Phrase("2014-01-06", EnglishFont));
                ptable.addCell(new Phrase("Tax:", EnglishFont));
                ptable.addCell(new Phrase("179665.92", EnglishFont));

                ptable.addCell(new Phrase("Currency:", EnglishFont));
                ptable.addCell(new Phrase("CNY", EnglishFont));
                ptable.addCell(new Phrase("Total Amount(Include Tax):", EnglishFont));
                ptable.addCell(new Phrase("1236524.29", EnglishFont));

                ptable.addCell(new Phrase("Payment Terms:", EnglishFont));
                Phrase ph1 = new Phrase("货到", font2);
                ph1.Add(new Phrase("90", EnglishFont));
                ph1.Add(new Phrase("天", font2));
                ptable.addCell(ph1);
                ptable.addCell(new Phrase("Phone:", EnglishFont));
                ptable.addCell(new Phrase("(0755)26581829", EnglishFont));

                ptable.addCell(new Phrase("Term/Mode:", EnglishFont));
                ptable.addCell(new Phrase("DDP_SZ/Final delivery", EnglishFont));
                ptable.addCell(new Phrase("Fax:", EnglishFont));
                ptable.addCell(new Phrase("(0755)26581802", EnglishFont));

                ptable.addCell(new Phrase("Buyer:", EnglishFont));
                Phrase ph2 = new Phrase("于汇海 ", font2);
                ph2.Add(new Phrase("00123023_2, Yu Huihai", EnglishFont));
                ptable.addCell(ph2);
                ptable.addCell(new Phrase("Bill To Address:", EnglishFont));
                ptable.addCell(new Phrase("华为技术有限公司应付业务部(生产采购核算部）", font2));

                document.Add(ptable);

                PdfPTable ptable1 = new PdfPTable(8);
                ptable1.DefaultCell.Border = Rectangle.LEFT | Rectangle.RIGHT | Rectangle.TOP | Rectangle.BOTTOM;
                ptable1.DefaultCell.BorderWidth = 1;
                ptable1.DefaultCell.PaddingBottom = 5;
                float[] headerwidths1 = { 5, 10, 40, 10, 5, 10, 10, 10 };
                ptable1.setWidths(headerwidths1);
                ptable1.WidthPercentage = 100;
                ptable1.addCell(new Phrase("SN", EnglishFont));
                ptable1.addCell(new Phrase("HW P/N", EnglishFont));
                ptable1.addCell(new Phrase("Description", EnglishFont));
                ptable1.addCell(new Phrase("Qty", EnglishFont));
                ptable1.addCell(new Phrase("Unit", EnglishFont));
                ptable1.addCell(new Phrase("Price", EnglishFont));
                ptable1.addCell(new Phrase("Del.Date", EnglishFont));
                ptable1.addCell(new Phrase("Del Place", EnglishFont));

                ptable1.addCell(new Phrase("1", EnglishFont));
                ptable1.addCell(new Phrase("04130104", EnglishFont));
                ptable1.addCell(new Phrase("射频电缆-6m-(N50直公-ⅩⅢ)-(COAX50-8.7/3.55黑)-(N50直公 - ⅩⅢ) - 1 / 2英寸超柔跳线////", font2));
                ptable1.addCell(new Phrase("4784", EnglishFont));
                ptable1.addCell(new Phrase("PCS", EnglishFont));
                ptable1.addCell(new Phrase("71.1785", EnglishFont));
                ptable1.addCell(new Phrase("08-FEB-2014", EnglishFont));
                ptable1.addCell(new Phrase("H80_松山湖B1 - 4号楼", font2));

                document.Add(ptable1);

                Image notes = Image.getInstance(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/hw_notes.png"));
                document.Add(notes);

            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message);
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message);
            }
            document.Close();
            return pdfURL;
        }
        
    }
}
