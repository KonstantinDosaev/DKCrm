using System.Globalization;
using DKCrm.Server.Interfaces;
using System.Reflection;
using TemplateEngine.Docx;
using System.Reflection.PortableExecutable;
using System.Xml;
using OpenXmlPowerTools;
using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Wordprocessing;

using System.Xml.Linq;
//using DocXToPdfConverter;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocXToPdfConverter;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DKCrm.Client.Pages;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Server.Services.OrderServices;
using DKCrm.Shared.Models.OrderModels;
using iTextSharp.text.rtf.document;
using Table = iTextSharp.text.Table;


namespace DKCrm.Server.Services
{
    public class DocumentService : IDocumentService
    {
        public DocumentService(IExportedOrderService orderService)
        {
            _orderService = orderService;
        }

        public ExportedOrder Order { get; set; } = null!;
        private readonly IExportedOrderService _orderService;

        public void CreatePaymentInvoiceAsync(Guid orderId)
        {
            //for (int i = 0; i < doc.Sections.Count; i++)
           // {
                //Document newWord = new Document();
                //newWord.Sections.Add(doc.Sections[i].Clone());
                //newWord.SaveToFile($@"{fullOutSplitItemPath}{i}.docx");
           // }
        }
        public async Task<string> CreateAsync()
        {
            Order = await _orderService.GetDetailAsync(new Guid("f41f77b1-0b1d-4025-b972-d985d742d772"));

            var folderName = Path.Combine("StaticFiles", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileTempName = "sample.docx";
            var fileOutName = "sampleOut.docx";

            var fullTempPath = Path.Combine(pathToSave, fileTempName);
            var fullOutPath = Path.Combine(pathToSave, fileOutName);

            //File.Delete(fullOutPath);
            //File.Copy(fullTempPath, fullOutPath);

            //var content = new Content(
            //    new FieldContent("BuyerBankName", "VYB"),
            //    new FieldContent("BuyerInn", "222222244444"),
            //    new FieldContent("BuyerKpp", "3524635758346"),
            //    new TableContent("Table")
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"), 
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "BoshY 2452346244"),
            //            new FieldContent("Quantity", "122"),
            //            new FieldContent("PriceOne", "2"),
            //            new FieldContent("PriceOneSum", "244"))
            //        .AddRow(
            //            new FieldContent("NameProduct", "YboshY 452346244"),
            //            new FieldContent("Quantity", "10"),
            //            new FieldContent("PriceOne", "10"),
            //            new FieldContent("PriceOneSum", "100")),

            //    new FieldContent("PriceSum", "4676"),
            //    new FieldContent("NdsAll", "345"),
            //    new FieldContent("PriceOver", "555"),
            //    new FieldContent("QuantityPosition", "393")
            //    );

            //using (var outputDocument = new TemplateProcessor(fullOutPath)
            //           .SetRemoveContentControls(true))
            //{
            //    outputDocument.FillContent(content);
            //    outputDocument.SaveChanges();
            //    outputDocument.Dispose();
            //}

                var fileOutPdfName = "sampleOutLong.pdf";
                var fullOutPdfPath = Path.Combine(pathToSave, fileOutPdfName); 
                var oo = "sts.pdf";
                var stam = Path.Combine(pathToSave, oo);

            //ConvertToPdfLongDoc(fullOutPath, fullOutPdfPath);

            // ConvertToPdfLo(fullOutPath, fullOutPdfPath);




            var arr = ReportPDF();
            File.Delete(fullOutPdfPath);
            File.WriteAllBytes(fullOutPdfPath, arr.Item1);


           // AddStamp(fullOutPdfPath, stam, arr.Item2);

            //Pp(fullOutPath);
            return fileOutName;
        }

        static void ConvertToPdfLongDoc(string inputFilePath, string outputFilePath)
        {
            var folderName = Path.Combine("StaticFiles", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileTempName = "htt.html";
            var fullHtmlTempPath = Path.Combine(pathToSave, fileTempName);
            byte[] byteArray = File.ReadAllBytes(inputFilePath);
            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    memoryStream.Write(byteArray, 0, byteArray.Length);
            //    using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, true))
            //    {
            //        HtmlConverterSettings settings = new HtmlConverterSettings()
            //        {
            //            PageTitle = "My Page Title"
            //        };
            //        XElement html = HtmlConverter.ConvertToHtml(doc, settings);
            //       File.WriteAllText(fullHtmlTempPath , html.ToStringNewLineOnAttributes());


            //        var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

            //        htmlToPdf.GeneratePdfFromFile( fullHtmlTempPath, null, outputFilePath);

            //    }
            //}
        }
        
        public static void ConvertToPdfLo(string inputFilePath, string outputFilePath) //, string outputDirectory
        {
            string locationOfLibreOfficeSoffice = "C:\\LibreOfficePortable\\App\\libreoffice\\program\\soffice.exe";
            var test = new ReportGenerator(locationOfLibreOfficeSoffice);

            //Convert from HTML to HTML


            //Convert from DOCX to PDF
            test.Convert(inputFilePath, outputFilePath);
        }

        private (byte[],float) ReportPDF()
        {
            var memoryStream = new MemoryStream();

            // Marge in centimeter, then I convert with .ToDpi()
            float margeLeft = 1.5f;
            float margeRight = 1.5f;
            float margeTop = 1.0f;
            float margeBottom = 1.0f;

            Document pdf = new Document(
                                    PageSize.A4,
                                    margeLeft.ToDpi(),
                                    margeRight.ToDpi(),
                                    margeTop.ToDpi(),
                                    margeBottom.ToDpi()
                                   );

            pdf.AddTitle("Blazor-PDF");
            pdf.AddAuthor("Christophe Peugnet");
            pdf.AddCreationDate();
            pdf.AddKeywords("blazor");
            pdf.AddSubject("Create a pdf file with iText");

            PdfWriter writer = PdfWriter.GetInstance(pdf, memoryStream);

            //HEADER and FOOTER
            var fontStyle = FontFactory.GetFont("Arial", 16, BaseColor.White);
            
         
            pdf.Open();

            var stampPos = CreatePayment(pdf, writer);
            //if (_pagenumber == 1)
            //    Page1.PageText(pdf);
            //else if (_pagenumber == 2)
            //    Page2.PageBookmark(pdf);
            //else if (_pagenumber == 3)
            //    Page3.PageImage(pdf, writer);
            //else if (_pagenumber == 4)
            //    Page4.PageTable(pdf, writer);
            //else if (_pagenumber == 5)
            //    Page5.PageFonts(pdf, writer);
            //else if (_pagenumber == 6)
            //    Page6.PageList(pdf);
            //else if (_pagenumber == 7)
            //    page7.PageShapes(pdf, writer);

            pdf.Close();

            return (memoryStream.ToArray(),stampPos);
        }


        private Font _font = null!; 
        private Font _fontBold = null!;
        private Font _fontBigBold = null!;
        public  float CreatePayment(Document pdf, PdfWriter writer)
        {
            var ttf = Path.Combine(Directory.GetCurrentDirectory(),"StaticFiles", "Fonts", "Arial.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
             _font = new Font(baseFont, 10, Font.NORMAL);
             _fontBold = new Font(baseFont, 10, Font.BOLD);
             _fontBigBold = new Font(baseFont, 14, Font.BOLD);
            var totalWidth = pdf.PageSize.Width - 3f.ToDpi();
            

            var bankTable = CreateBankTable(totalWidth);
            pdf.Add(bankTable);

            var title = new Paragraph($"Счет на оплату № 0000       от {DateTime.Now.ToLongDateString()}", _fontBigBold) {
                    SpacingAfter = 18f,
                    SpacingBefore = 10f
                };
            pdf.Add(title);

            var contractPartiesTable = CreateContractPartiesTable(totalWidth);
            contractPartiesTable.SpacingAfter = 10f;
            pdf.Add(contractPartiesTable);
            
            var productTable = CreateProductTable(totalWidth);
            productTable.SpacingAfter = 20f;
            pdf.Add(productTable);

            var tablePaintings = new PdfPTable(4)
            {
                TotalWidth = totalWidth
            };
            var widths = new float[] { 90, 200, 90, 200 };
            tablePaintings.SetWidths(widths);
            tablePaintings.LockedWidth = true;
            tablePaintings.AddCell(new PdfPCell(new Phrase($"Руководитель", _fontBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            var directorArr = Order.OurCompany?.Director!.Split(' ');
            tablePaintings.AddCell(new PdfPCell(new Phrase($"{directorArr?[0]} {directorArr?[1][0]}. {directorArr?[2][0]}.", _font))
            {
                Border = Rectangle.NO_BORDER,
                BorderWidthBottom = 1f,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            tablePaintings.AddCell(new PdfPCell(new Phrase($"Бухгалтер", _fontBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            tablePaintings.AddCell(new PdfPCell(new Phrase($"{directorArr?[0]} {directorArr?[1][0]}. {directorArr?[2][0]}.", _font))
            {
                Border = Rectangle.NO_BORDER,
                BorderWidthBottom = 1f,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });

            pdf.Add(tablePaintings);
            var stampMarcker = writer.GetVerticalPosition(false);
            //img.Alignment = Image.IMGRAW | Image.ALIGN_RIGHT;
            //img.SetAbsolutePosition(
            //      (PageSize.A4.Width - img.ScaledWidth) / 2,
            //      pdf.GetVerticalPosition(true));

            return stampMarcker;
        }

        private PdfPTable CreateBankTable(float totalWidth)
        {
            var table = new PdfPTable(2)
            {
                TotalWidth = totalWidth
            };

            var widths = new float[] { totalWidth / 2, totalWidth / 2 };
            table.SetWidths(widths);
            table.LockedWidth = true;

            var leftTop = new PdfPTable(1);
            var leftTopTop = new PdfPCell(new Phrase(" .Название банка получателя Название банка получателя" +
                                                     "Название банка получателя Название банка получателя", _font))
            {
                BorderWidthBottom = 0
            };
            leftTop.AddCell(leftTopTop);
            var leftTopBottom = new PdfPCell(new Phrase("Банк получателя", _font))
            {
                BorderWidthTop = 0,
                PaddingTop = 10
            };
            leftTop.AddCell(leftTopBottom);
            var usingLefTop = new PdfPCell(leftTop)
            {
                Padding = 0f
            };
            table.AddCell(usingLefTop);

            var rightTop = new PdfPTable(2);
            var rightWidths = new float[] { 50, totalWidth / 2 - 50 };
            rightTop.SetWidths(rightWidths);
            rightTop.AddCell(new Phrase("БИК", _font));
            rightTop.AddCell(new Phrase(".бик", _font));
            rightTop.AddCell(new Phrase("Сч. №", _font));
            rightTop.AddCell(new Phrase(".сч", _font));
            var usingRightTop = new PdfPCell(rightTop);
            table.AddCell(usingRightTop);

            var leftBottom = new PdfPTable(2);
            leftBottom.AddCell(new Phrase("ИНН 354635635", _font));
            leftBottom.AddCell(new Phrase("КПП  356363633", _font));
            var middleLeftBottom = new PdfPCell(new Phrase(".название комп.", _font))
            {
                Colspan = 2,
                BorderWidthBottom = 0
            };
            leftBottom.AddCell(middleLeftBottom);
            var bottomLeftBottom = new PdfPCell(new Phrase("Получатель", _font))
            {
                BorderWidthTop = 0,
                PaddingTop = 10,
                Colspan = 2
            };
            leftBottom.AddCell(bottomLeftBottom);
            var usingLeftBottom = new PdfPCell(leftBottom);
            table.AddCell(usingLeftBottom);

            var rightBottom = new PdfPTable(2);
            rightBottom.SetWidths(rightWidths);
            rightBottom.AddCell(new Phrase("Сч. №", _font));
            rightBottom.AddCell(new Phrase(".счч", _font));
            var usingRightBottom = new PdfPCell(rightBottom);
            table.AddCell(usingRightBottom);
            return table;
        }

        private PdfPTable CreateContractPartiesTable(float totalWidth)
        {
            var ourCompany = Order.OurCompany;
            var buyerCompany = Order.CompanyBuyer;
            var table = new PdfPTable(2)
            {
                TotalWidth = totalWidth
            };
            if (buyerCompany != null && ourCompany != null)
            {
                var widths = new float[] { 80, totalWidth - 80 };
                table.SetWidths(widths);
                table.LockedWidth = true;

                var rowOne = new PdfPTable(1);
                var rowOneTop = new PdfPCell(new Phrase("Поставщик", _font)) {
                    Border = Rectangle.NO_BORDER, BorderWidthTop = 1.5f, PaddingTop = 10
                };
                rowOne.AddCell(rowOneTop);
                var rowOneBottom = new PdfPCell(new Phrase("(Исполнитель):", _font)) {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 10
                };
                rowOne.AddCell(rowOneBottom);
                var RowOneCellOne = new PdfPCell(rowOne) {Border = Rectangle.NO_BORDER };
                table.AddCell(RowOneCellOne);
                var rowOneCellTwo = new PdfPCell(new Phrase($"{ourCompany.Name},ИНН {ourCompany.FnsRequest?.INN},КПП ????, post???,{ourCompany.FnsRequest?.LegalAddress}  ", _fontBold)) 
                    {Border = Rectangle.NO_BORDER, BorderWidthTop = 1.5f, PaddingTop = 10 };
                table.AddCell(rowOneCellTwo);

                var rowTwo = new PdfPTable(1);
                var RowTwoTop = new PdfPCell(new Phrase("Покупатель", _font)) {Border = Rectangle.NO_BORDER };
                rowTwo.AddCell(RowTwoTop);
                var rowTwoBottom = new PdfPCell(new Phrase("(Заказчик):", _font)) {Border = Rectangle.NO_BORDER,
                    PaddingBottom = 10
                };
                rowTwo.AddCell(rowTwoBottom);
                var RowTwoCellOne = new PdfPCell(rowTwo) {Border = Rectangle.NO_BORDER };
                table.AddCell(RowTwoCellOne);
                var rowTwoCellTwo = new PdfPCell(new Phrase($"{buyerCompany.Name},ИНН {buyerCompany.FnsRequest?.INN},КПП ????, post???,{buyerCompany.FnsRequest?.LegalAddress}  ", _fontBold)) {
                    Border = Rectangle.NO_BORDER,
                };
                table.AddCell(rowTwoCellTwo);

                var rowThreeCellOne = new PdfPCell(new Phrase("Основание:", _font)){Border = Rectangle.NO_BORDER };
                var rowThreeCellTwo = new PdfPCell(new PdfPCell(new Phrase($"Договор №???????????? от 00.00.0000", _fontBold))){Border = Rectangle.NO_BORDER};
                table.AddCell(rowThreeCellOne);
                table.AddCell(rowThreeCellTwo);
            }
            return table;
        }
        private PdfPTable CreateProductTable(float totalWidth)
        {
            var ourCompany = Order.OurCompany;
            var buyerCompany = Order.CompanyBuyer;
            var productList = Order.ExportedProducts;
            decimal fullPrice = 0;
            var table = new PdfPTable(6)
            {
                TotalWidth = totalWidth
            };
            if (productList != null)
            {
                var widthMainCell = totalWidth / 2;
                var widths = new float[] { 30,widthMainCell, 60, 30, 80, 90 };
                table.SetWidths(widths);
                table.LockedWidth = true;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                var round = 2;

              
                table.AddCell(new PdfPCell(new Phrase("№", _fontBold)) {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Товары(работы, услуги)", _fontBold)) {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Кол-вo", _fontBold)) {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Ед.", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Цена", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Сумма", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });

                var number = 1;
                foreach (var exportedProduct in productList)
                {
                    var priceOnePositionSum = exportedProduct.Quantity * exportedProduct.PriceInTransactionCurrency ?? 0;
                    var priceOnePosition = exportedProduct.PriceInTransactionCurrency ?? 0;
                    fullPrice += priceOnePositionSum;


                    var numCell = new PdfPCell(new Phrase(number.ToString(), _font))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(numCell);
                    table.AddCell(new Phrase(exportedProduct.Product?.Name, _font));
                    var quantityCell = new PdfPCell(new Phrase(exportedProduct.Quantity.ToString(), _font))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    table.AddCell(quantityCell);
                    table.AddCell(new Phrase("шт.", _font));
                    var priceOneCell = new PdfPCell(new Phrase((decimal.Round(priceOnePosition, round)).ToString(CultureInfo.CurrentCulture), _font))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    table.AddCell(priceOneCell);
                    var priceSumCell = new PdfPCell(new Phrase((decimal.Round(priceOnePositionSum, round)).ToString(CultureInfo.CurrentCulture), _font))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    table.AddCell(priceSumCell);
                    number++;
                }
                var fullPriceCellLeft = new PdfPCell(new Phrase("Итого:", _fontBold))
                {
                    Colspan = 5,
                    Border = Rectangle.NO_BORDER,
                    PaddingTop = 10,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(fullPriceCellLeft);
                var fullPriceCellRight = new PdfPCell(new Phrase((decimal.Round(fullPrice, round)).ToString(CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture), _fontBold))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingTop = 10,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(fullPriceCellRight);
                var ndsCellLeft = new PdfPCell(new Phrase("В том числе НДС:", _fontBold))
                {
                    Colspan = 5,
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(ndsCellLeft);
                var ndsCellRight = new PdfPCell(new Phrase((decimal.Round(fullPrice / 100 * (decimal)Order.Nds!, round)).ToString(CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture), _fontBold))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(ndsCellRight);
                var fullPriceToPaymentCellLeft = new PdfPCell(new Phrase("Всего к оплате:", _fontBold))
                {
                    Colspan = 5,
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(fullPriceToPaymentCellLeft);
                var fullPriceToPaymentCellRight = new PdfPCell(new Phrase((decimal.Round(fullPrice, round)).ToString(CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture), _fontBold)) {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(fullPriceToPaymentCellRight);

                table.AddCell(new PdfPCell(new Phrase($"Всего наименований {number}, на сумму {(decimal.Round(fullPrice, round)).ToString(CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture)} руб.", _font)) {
                    Colspan = 6,
                    Border = Rectangle.NO_BORDER,
                });
                table.AddCell(new PdfPCell(new Phrase($"Прописью {number}.", _fontBold))
                {
                    Colspan = 6,
                    Border = Rectangle.NO_BORDER,
                });
                table.AddCell(new PdfPCell(new Phrase($"ИГК ?????{number}.", _font))
                {
                    Colspan = 6,
                    Border = Rectangle.NO_BORDER,
                    BorderWidthBottom = 1.5f,
                    PaddingTop = 10, PaddingBottom = 10
                });

            }
            return table;
        }
        private void AddStamp(string inputFilePath, string outputFilePath, float position)
        {
            try
            {

                var folderName = Path.Combine("StaticFiles", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var image = Path.Combine(pathToSave, "im.png");
                Document document = new Document();

                PdfReader pdfReader = new PdfReader(inputFilePath);

                FileStream fs = new FileStream(outputFilePath, FileMode.Create);
                PdfStamper stamp = new PdfStamper(pdfReader, fs);




                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(image);
                img.RotationDegrees = 0;
                img.ScaleToFit(100f, 100f);

                //center
                img.SetAbsolutePosition(100, position - 50);

                PdfContentByte waterMarkImage;
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    if (page== pdfReader.NumberOfPages)
                    {
                        waterMarkImage = stamp.GetOverContent(page);
                        waterMarkImage.AddImage(img);
                    }
                    
                }

                // Flat pdf
                stamp.FormFlattening = true;
                stamp.Close();
                // axAcroPDF1.LoadFile(fs.Name);
                fs.Close();
                pdfReader.Close();
                document.Dispose();

            }

            catch (Exception ex)
            {

            }
           
        }
    }
}

