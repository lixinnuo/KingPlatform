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
                Font font5 = new Font(ChineseSun, 6, Font.NORMAL);
                Font EnglishFont = FontFactory.getFont(FontFactory.TIMES_NEW_ROMAN, 18, Font.BOLD);
                Font EnglishFont1 = FontFactory.getFont(FontFactory.TIMES_NEW_ROMAN, 10, Font.BOLD);
                Font EnglishFont2 = FontFactory.getFont(FontFactory.TIMES_NEW_ROMAN, 10, Font.NORMAL);
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

                Paragraph logoContent3 = new Paragraph();
                logoContent3.Add(new Phrase(" ", font5));
                document.Add(logoContent3);

                Paragraph logoContent1 = new Paragraph();
                logoContent1.Add(new Phrase("          华为技术有限公司", font1));
                document.Add(logoContent1);

                Paragraph logoContent2 = new Paragraph();
                logoContent2.Add(new Phrase("                      Huawei Technologies Co., Ltd.", EnglishFont1));
                document.Add(logoContent2);

                document.Add(logoContent3);

                Paragraph p1 = new Paragraph(new Phrase("PO Detail Screen for PO:  " + data.result[0].poNumber, EnglishFont1));
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
                double amount1 = (double)(totalAmount * data.result[0].priceOverride);
                ptable.addCell(new Phrase(String.Format("{0:F}", amount1), EnglishFont2));               //默认为保留两位

                ptable.addCell(new Phrase("Last Modified:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].lastUpdateDate.Substring(0, 10), EnglishFont2));
                ptable.addCell(new Phrase("Tax Rate:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].taxRate.ToString(), EnglishFont2));

                ptable.addCell(new Phrase("Currency:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].currencyCode, EnglishFont2));
                ptable.addCell(new Phrase("Tax:", EnglishFont1));
                double tax = (double)(totalAmount * data.result[0].priceOverride * data.result[0].taxRate);
                ptable.addCell(new Phrase(String.Format("{0:F}", tax), EnglishFont2));

                ptable.addCell(new Phrase("Payment Terms:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].paymentTerms, font3));
                ptable.addCell(new Phrase("Total Amount(Include Tax):", EnglishFont3));
                double amount2 = (double)(totalAmount * data.result[0].priceOverride * (1 + data.result[0].taxRate));
                ptable.addCell(new Phrase(String.Format("{0:F}", amount2), EnglishFont2));

                ptable.addCell(new Phrase("Term/Mode:", EnglishFont1));
                ptable.addCell(new Phrase(data.result[0].fobLookupCode + "/" + data.result[0].carrierName, font3));
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

                document.Add(logoContent3); //空一行

                PdfPTable ptable1 = new PdfPTable(8);
                ptable1.DefaultCell.Border = Rectangle.LEFT | Rectangle.RIGHT | Rectangle.TOP | Rectangle.BOTTOM;
                ptable1.DefaultCell.BorderWidth = 1;
                ptable1.DefaultCell.PaddingBottom = 5;
                float[] headerwidths1 = { 5, 15, 25, 7, 7, 6, 12, 23 };
                ptable1.setWidths(headerwidths1);
                ptable1.WidthPercentage = 100;
                ptable1.addCell(new Phrase("SN", EnglishFont2));
                ptable1.addCell(new Phrase("HW P/N", EnglishFont2));
                ptable1.addCell(new Phrase("Description", EnglishFont2));
                ptable1.addCell(new Phrase("Qty", EnglishFont2));
                ptable1.addCell(new Phrase("Unit", EnglishFont2));
                ptable1.addCell(new Phrase("Price", EnglishFont2));
                ptable1.addCell(new Phrase("Del.Date", EnglishFont2));
                ptable1.addCell(new Phrase("Del.Place", EnglishFont2));

                for (int i = 0; i < data.result.Count; i++)
                {
                    ptable1.addCell(new Phrase(data.result[i].poLineNum, EnglishFont3)); 
                    ptable1.addCell(new Phrase(data.result[i].itemCode, EnglishFont3));
                    ptable1.addCell(new Phrase(data.result[i].itemDescription, font3));
                    ptable1.addCell(new Phrase(data.result[i].quantity.ToString(), EnglishFont3));
                    ptable1.addCell(new Phrase(data.result[i].unitOfMeasure, EnglishFont3));
                    ptable1.addCell(new Phrase(data.result[i].priceOverride.ToString(), EnglishFont3));
                    ptable1.addCell(new Phrase(data.result[i].needByDate.Substring(0, 10), EnglishFont3));
                    ptable1.addCell(new Phrase("H80_DCS_南岗工业区2栋", font3));
                }  
                document.Add(ptable1);

                /*Paragraph notes1 = new Paragraph();
                string content1 = "Notes/备注:";
                Phrase notesPhrase1 = new Phrase(content1, font5);
                notes1.Add(notesPhrase1);
                document.Add(notes1);

                Paragraph notes2 = new Paragraph();
                string content2 = "1.This Purchase Order (“PO”) is governed by all applicable agreements executed between the supplier named under this PO and Huawei, whether by physical signature or online through the Huawei Supplier Internet Portal at http://scs.huawei.com/eSupplier/ (“Huawei Supplier Portal”). Such agreements include but are not limited to: (i) the latest versions of the following agreements published by Huawei at the Huawei Supplier Portal: Master Purchase Agreement for Products, Master Purchase Agreement for Services,Master Purchase Agreement for Products an Services, Basic Agreements for Purchases, and Basic Agreements for Huawei Supplier Qualification; (ii)any valid master purchase agreement or similar agreements purchase physically signed by the supplier under which this PO is issued; (iii)any project agreements, statement of works (“SOW”) applicable to this PO, if any; (iv)any special agreements, including but not limited to Non - disclosure Agreement and Honesty and Integrity Commitment, etc.; and(v) all documents attached to and / or referred to by the aforesaid agreements.";
                Phrase notesPhrase2 = new Phrase(content2, font5);
                notes2.Add(notesPhrase2);
                document.Add(notes2);

                Paragraph notes3 = new Paragraph();
                string content3 = "本订单受双方签署的所有适用的协议的约束，无论签署是以书面形式还是通过华为供应商门户网站http://scs.huawei.com/eSupplier/网上进行。该等协议包括但不限于：(i)华为在前述供应商门户网站上发布的以下协议的最新版本：《产品采购主协议》/《服务采购主协议》/《产品及服务采购主协议》、《采购基础协议》，及《华为供应商认证基础协议》；(ii)双方书面签署的仍然有效的《采购主协议》或相同性质的其他采购协议，而该订单是在其下发出；(iii)所有适用于本订单的项目协议、《工作说明书》（如有）；(iv)专项协议，包括但不限于《保密协议》、《诚信廉洁承诺书》等；以及(v) 前述各协议的附件及 / 或其指引适用的文件。";
                Phrase notesPhrase3 = new Phrase(content3, font5);
                notes3.Add(notesPhrase3);
                document.Add(notes3);

                Paragraph notes4 = new Paragraph();
                string content4 = "2.Within five (5) days after receipting this PO or a given period as stipulated in an applicable agreement (if any, then the period in the applicable agreement shall take priority),Supplier shall either confirm its acceptance of the PO or inquiry about this PO to the Huawei contact point/ person as specified in the applicable agreement.If Supplier is failure to do so within the given period, it shall be deemed that Supplier has accept the PO.";
                Phrase notesPhrase4 = new Phrase(content4, font5);
                notes4.Add(notesPhrase4);
                document.Add(notes4);

                Paragraph notes5 = new Paragraph();
                string content5 = "供应商应在收到本订单后五（5）天内或在相关协议中另行约定的期限内（如有，则以协议中另行约定的为准）接受订单，或向协议中指定的接口点/人进行澄清。若供应商既未在前述期限内接受订单又未向指定人员做出澄清，则视作供应商已接受订单。";
                Phrase notesPhrase5 = new Phrase(content5, font5);
                notes5.Add(notesPhrase5);
                document.Add(notes5);

                Paragraph notes6 = new Paragraph();
                string content6 = "The PO number and the applicable line numbers in the PO shall appear on each invoice and bill of lading relating to the PO.";
                Phrase notesPhrase6 = new Phrase(content6, font5);
                notes6.Add(notesPhrase6);
                document.Add(notes6);

                Paragraph notes7 = new Paragraph();
                string content7 = "发票及提单上应注明相关订单号及行号。";
                Phrase notesPhrase7 = new Phrase(content7, font5);
                notes7.Add(notesPhrase7);
                document.Add(notes7);

                Paragraph notes8 = new Paragraph();
                string content8 = "4.Any change made to an existing PO shall be subject to written confirmation between Huawei and Supplier; and the PO issued by Huawei after such confirmation shall be the final binding version of the PO.";
                Phrase notesPhrase8 = new Phrase(content8, font5);
                notes8.Add(notesPhrase8);
                document.Add(notes8);

                Paragraph notes9 = new Paragraph();
                string content9 = "对于已生效订单的任何变更应当由华为与供应商进行书面确认，书面确认后华为重新发出的PO将是最终有效的版本。";
                Phrase notesPhrase9 = new Phrase(content9, font5);
                notes8.Add(notesPhrase9);
                document.Add(notes9);*/

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
