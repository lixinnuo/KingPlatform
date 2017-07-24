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
        public static string CreateHWPOPDF(string mes, string filePath)
        {
            POListParamBackOutput data = new POListParamBackOutput();
            data = mes.ToObject<POListParamBackOutput>();
            Document document = new Document(PageSize.A4, 20, 20, 20, 20);
            string pdfName = "", pdfPath = "";
            try
            {
                pdfName = data.result[0].poNumber + "--" + DateTime.Now.ToString("yyyyMMddHHmmss");
                if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(filePath)))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(filePath));
                }
                pdfPath = System.Web.HttpContext.Current.Server.MapPath(string.Format(filePath + "{0}.pdf", pdfName));

                PdfWriter writer = PdfWriter.getInstance(document, new FileStream(pdfPath, FileMode.Create));
                //HeaderFooter header = new HeaderFooter(new Phrase("page: 1"), false);
                //header.Border = Rectangle.NO_BORDER;
                //document.Header = header;

                document.Open();

                BaseFont ChineseSun = BaseFont.createFont(@"c:\Windows\fonts\simsun.TTC,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                Font font = new Font(ChineseSun, 18, Font.BOLD);
                Font font1 = new Font(ChineseSun, 12, Font.BOLD);
                Font font2 = new Font(ChineseSun, 12, Font.NORMAL);
                Font font3 = new Font(ChineseSun, 10, Font.NORMAL);
                Font font4 = new Font(ChineseSun, 8, Font.NORMAL);
                Font EnglishFont = FontFactory.getFont(FontFactory.TIMES_NEW_ROMAN, 18, Font.BOLD);
                Font EnglishFont1 = FontFactory.getFont(FontFactory.TIMES_NEW_ROMAN, 12, Font.BOLD);
                Font EnglishFont2 = FontFactory.getFont(FontFactory.TIMES_NEW_ROMAN, 12, Font.NORMAL);
                Font EnglishFont3 = FontFactory.getFont(FontFactory.TIMES_NEW_ROMAN, 10, Font.NORMAL);

                //标题
                Paragraph title = new Paragraph(new Chunk("采购订单", font));
                title.Add(new Phrase("Purchase Order", EnglishFont));
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                Paragraph logo = new Paragraph();
                Image image = Image.getInstance(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/huawei-logo.png"));
                //image.setAbsolutePosition(40, 40);
                image.Alignment = Image.LEFT | Image.UNDERLYING;
                logo.Add(image);
                document.Add(logo);

                Paragraph logoContent4 = new Paragraph();
                logoContent4.Add(new Phrase(" ", font1));
                document.Add(logoContent4);

                Paragraph logoContent1 = new Paragraph();
                logoContent1.Add(new Phrase("          华为技术有限公司", font1));
                document.Add(logoContent1);

                Paragraph logoContent2 = new Paragraph();
                logoContent2.Add(new Phrase("                  Huawei Technologies Co., Ltd.", EnglishFont1));
                document.Add(logoContent2);

                Paragraph logoContent3 = new Paragraph();
                logoContent3.Add(new Phrase(" ", font1));
                document.Add(logoContent3);

                Paragraph p1 = new Paragraph(new Chunk("PO Detail Screen for PO:  " + data.result[0].poNumber, EnglishFont1));
                document.Add(p1);

                PdfPTable ptable = new PdfPTable(4);
                ptable.DefaultCell.Border = 0;
                ptable.DefaultCell.PaddingBottom = 5;
                float[] headerwidths = { 20, 30, 25, 25 };
                ptable.setWidths(headerwidths);
                ptable.WidthPercentage = 100;
                ptable.addCell(new Phrase("PO/Rel. NO:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].poNumber, EnglishFont2));
                ptable.addCell(new Phrase("Supplier Code:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].vendorCode, EnglishFont2));

                ptable.addCell(new Phrase("PO Type:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].shipmentType, EnglishFont2));
                ptable.addCell(new Phrase("Supplier:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].vendorName, font4));

                ptable.addCell(new Phrase("PO/Rel. Ver.:", EnglishFont1));
                ptable.addCell(new Phrase("0", EnglishFont2));
                ptable.addCell(new Phrase("Contact:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].sendConnecter, font4));

                ptable.addCell(new Phrase("Data Created:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].publishDate.Substring(0, 10), EnglishFont2));
                ptable.addCell(new Phrase("Total Amount(Exclude Tax):", EnglishFont3));
                int totalAmount = 0;
                for (int i = 0; i < data.result.Count; i++) {
                    totalAmount += (int)(data.result[i].quantity) - (int)data.result[i].quantityCancelled;
                }
                ptable.addCell(new Phrase(Math.Round((double)(totalAmount * data.result[0].priceOverride), 2).ToString(), EnglishFont2));

                ptable.addCell(new Phrase("Last Modified:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].lastUpdateDate.Substring(0, 10), EnglishFont2));
                ptable.addCell(new Phrase("Tax Rate:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].taxRate.ToString(), EnglishFont2));

                ptable.addCell(new Phrase("Currency:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].currencyCode, EnglishFont2));
                ptable.addCell(new Phrase("Tax:", EnglishFont1));
                ptable.addCell(new Phrase(Math.Round((double)(totalAmount * data.result[0].priceOverride * data.result[0].taxRate)).ToString(), EnglishFont2));

                ptable.addCell(new Phrase("Payment Terms:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].paymentTerms, font3));
                ptable.addCell(new Phrase("Total Amount(Include Tax):", EnglishFont3));
                ptable.addCell(new Phrase(Math.Round((double)(totalAmount * data.result[0].priceOverride * (1 + data.result[0].taxRate))).ToString(), EnglishFont2));

                ptable.addCell(new Phrase("Term/Mode:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].businessMode, font3));
                ptable.addCell(new Phrase("Phone:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].sendVendorTelNum, EnglishFont2));

                ptable.addCell(new Phrase("Buyer:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].agentName, font3));
                ptable.addCell(new Phrase("Fax:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].sendVendorFax, EnglishFont2));
                
                ptable.addCell(new Phrase("Bill To Address:", EnglishFont1));
                PdfPCell cell;
                cell = new PdfPCell(new Phrase(data.result[0].billToLocation, font3));
                cell.Colspan = 3;
                cell.Border = 0;
                ptable.addCell(cell);

                ptable.addCell(new Phrase("Ship To Address:", EnglishFont1));
                cell = new PdfPCell(new Phrase(data.result[0].sendVendorAddr, font3));
                cell.Colspan = 3;
                cell.Border = 0;
                ptable.addCell(cell);

                ptable.addCell(new Phrase("Note To Supplier:", EnglishFont1));
                cell = new PdfPCell(new Phrase("", font3));
                cell.Colspan = 3;
                cell.Border = 0;
                ptable.addCell(cell);
                
                cell = new PdfPCell(new Phrase("", font3));
                cell.Colspan = 4;
                cell.Border = 0;
                ptable.addCell(cell);

                document.Add(ptable);

                PdfPTable ptable1 = new PdfPTable(8);
                ptable1.DefaultCell.Border = Rectangle.LEFT | Rectangle.RIGHT | Rectangle.TOP | Rectangle.BOTTOM;
                ptable1.DefaultCell.BorderWidth = 1;
                ptable1.DefaultCell.PaddingBottom = 5;
                float[] headerwidths1 = { 5, 15, 25, 5, 5, 10, 15, 20 };
                ptable1.setWidths(headerwidths1);
                ptable1.WidthPercentage = 100;
                ptable1.addCell(new Phrase("SN", EnglishFont2));
                ptable1.addCell(new Phrase("HW P/N", EnglishFont2));
                ptable1.addCell(new Phrase("Description", EnglishFont2));
                ptable1.addCell(new Phrase("Qty", EnglishFont2));
                ptable1.addCell(new Phrase("Unit", EnglishFont2));
                ptable1.addCell(new Phrase("Price", EnglishFont2));
                ptable1.addCell(new Phrase("Del.Date", EnglishFont2));
                ptable1.addCell(new Phrase("Del Place", EnglishFont2));

                for (int i = 0; i < data.result.Count; i++)
                {
                    ptable1.addCell(new Phrase(data.result[i].poLineNum, EnglishFont3)); 
                    ptable1.addCell(new Phrase(data.result[i].itemCode, EnglishFont3));
                    ptable1.addCell(new Phrase(data.result[i].itemDescription, font3));
                    ptable1.addCell(new Phrase(data.result[i].quantity.ToString(), EnglishFont3));
                    ptable1.addCell(new Phrase(data.result[i].unitOfMeasure, EnglishFont3));
                    ptable1.addCell(new Phrase(data.result[i].priceOverride.ToString(), EnglishFont3));
                    ptable1.addCell(new Phrase(data.result[i].needByDate.Substring(0, 10), EnglishFont3));
                    ptable1.addCell(new Phrase("", font2));
                }  
                document.Add(ptable1);

                Image notes = Image.getInstance(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/huawei_notes.png"));
                notes.scalePercent(65);
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
            return pdfName + ".pdf";
        }
    }
}
