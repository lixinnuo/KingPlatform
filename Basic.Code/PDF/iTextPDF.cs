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
        public static void  CreateHWPOPDF(string mes)
        {
            PODatails data = new PODatails();
            data = mes.ToObject<PODatails>();
            Document document = new Document(PageSize.A3);
            string pdfName = "", pdfPath = "";
            try
            {
                pdfName =   DateTime.Now.ToString("yyyyMMddHHmmss");
                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Download/HWPO/pdf/")))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Download/HWPO/pdf/"));
                }
                pdfPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Download/HWPO/pdf/{0}.pdf", pdfName));
                PdfWriter writer = PdfWriter.getInstance(document, new FileStream(pdfPath, FileMode.Create));

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

                Paragraph p1 = new Paragraph(new Chunk("PO Detail Screen for PO:" + data.poNumber, EnglishFont1));
                document.Add(p1);

                PdfPTable ptable = new PdfPTable(4);
                ptable.DefaultCell.Border = 0;
                ptable.DefaultCell.PaddingBottom = 10;
                float[] headerwidths = { 15, 25, 25, 35 };
                ptable.setWidths(headerwidths);
                ptable.WidthPercentage = 100;
                ptable.addCell(new Phrase("PO/Rel. NO:", EnglishFont));
                ptable.addCell(new Phrase(data.poNumber, EnglishFont));
                ptable.addCell(new Phrase("Supplier Code:", EnglishFont));
                ptable.addCell(new Phrase(data.vendorCode, EnglishFont));

                ptable.addCell(new Phrase("PO Type:", EnglishFont));
                ptable.addCell(new Phrase(data.shipmentType, EnglishFont));
                ptable.addCell(new Phrase("Supplier:", EnglishFont));
                ptable.addCell(new Phrase(data.vendorName, font2));

                ptable.addCell(new Phrase("PO/Rel. Ver.:", EnglishFont));
                ptable.addCell(new Phrase("", EnglishFont));
                ptable.addCell(new Phrase("Total Amount(Exclude Tax):", EnglishFont));
                ptable.addCell(new Phrase((data.quantity * data.priceOverride).ToString(), EnglishFont));

                ptable.addCell(new Phrase("Data Created:", EnglishFont));
                ptable.addCell(new Phrase(data.publishDate.Substring(10), EnglishFont));
                ptable.addCell(new Phrase("Tax Rate:", EnglishFont));
                ptable.addCell(new Phrase(data.taxRate.ToString(), EnglishFont));

                ptable.addCell(new Phrase("Last Modified:", EnglishFont));
                ptable.addCell(new Phrase(data.publishDate.Substring(10), EnglishFont));
                ptable.addCell(new Phrase("Tax:", EnglishFont));
                ptable.addCell(new Phrase((data.quantity * data.priceOverride * data.taxRate).ToString(), EnglishFont));

                ptable.addCell(new Phrase("Currency:", EnglishFont));
                ptable.addCell(new Phrase(data.currencyCode, EnglishFont));
                ptable.addCell(new Phrase("Total Amount(Include Tax):", EnglishFont));
                ptable.addCell(new Phrase((data.quantity * data.priceOverride * (1 + data.taxRate)).ToString(), EnglishFont));

                ptable.addCell(new Phrase("Payment Terms:", EnglishFont));
                Phrase ph1 = new Phrase("货到", font2);
                ph1.Add(new Phrase("90", EnglishFont));
                ph1.Add(new Phrase("天", font2));
                ptable.addCell(ph1);
                ptable.addCell(new Phrase("Phone:", EnglishFont));
                ptable.addCell(new Phrase(data.sendVendorTelNum, EnglishFont));

                ptable.addCell(new Phrase("Term/Mode:", EnglishFont));
                ptable.addCell(new Phrase(data.businessMode, EnglishFont));
                ptable.addCell(new Phrase("Fax:", EnglishFont));
                ptable.addCell(new Phrase(data.sendVendorFax, EnglishFont));

                ptable.addCell(new Phrase("Buyer:", EnglishFont));
                ptable.addCell(new Phrase(data.agentName, font2));
                ptable.addCell(new Phrase("Bill To Address:", EnglishFont));
                string[] sbillToLocation = data.billToLocation.Split(',');
                ptable.addCell(new Phrase(sbillToLocation[0], font2));

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
                ptable1.addCell(new Phrase(data.itemCode, EnglishFont));
                ptable1.addCell(new Phrase(data.itemDescription + "////", font2));
                ptable1.addCell(new Phrase(data.quantity.ToString(), EnglishFont));
                ptable1.addCell(new Phrase(data.unitOfMeasure, EnglishFont));
                ptable1.addCell(new Phrase(data.priceOverride.ToString(), EnglishFont));
                ptable1.addCell(new Phrase("", EnglishFont));
                ptable1.addCell(new Phrase("", font2));

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
            if (FileDownHelper.FileExists(pdfPath))
            {
                FileDownHelper.DownLoadold(pdfPath, pdfName + ".pdf");
            }
        }
    }
}
