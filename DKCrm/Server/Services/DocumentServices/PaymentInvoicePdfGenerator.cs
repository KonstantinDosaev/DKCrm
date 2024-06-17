using DKCrm.Server.Interfaces.DocumentInterfaces;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models.OrderModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using DKCrm.Shared.Constants;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;
using PageSize = iTextSharp.text.PageSize;
using Paragraph = iTextSharp.text.Paragraph;

namespace DKCrm.Server.Services.DocumentServices
{
    public class PaymentInvoicePdfGenerator
    {
        private ExportedOrder? Order { get; set; } = null!;
        private readonly IExportedOrderService _orderService;
        private readonly IPriceToStringConverter _priceToString;
        private readonly IInfoSetFromDocumentToOrderService _infoSetFromDocumentToOrderService;
        private Font _font = null!;
        private Font _fontBold = null!;
        private Font _fontBigBold = null!;
        private float _stampPosition;
        private float _totalWidth;
        private int _indexInstanceDocument;
        private string _mainPathToFiles  = null!;

        public PaymentInvoicePdfGenerator(IExportedOrderService orderService, IPriceToStringConverter priceToString, IInfoSetFromDocumentToOrderService infoSetFromDocumentToOrderService)
        {
            _orderService = orderService;
            _priceToString = priceToString;
            _infoSetFromDocumentToOrderService = infoSetFromDocumentToOrderService;
        }

        public async Task<bool> CreatePaymentAsync(Guid orderId)
        {
            Order = await _orderService.GetDetailAsync(orderId);
            _mainPathToFiles = Directory.GetCurrentDirectory();
            // Order = await _orderService.GetDetailAsync(new Guid("f41f77b1-0b1d-4025-b972-d985d742d772"));
            if (Order == null) return false;
            var docs = await _infoSetFromDocumentToOrderService
                .GetAllInfoSetsDocumentsToOrderAsync(Order.Id);
            var infoSetFromDocumentToOrders = docs as InfoSetFromDocumentToOrder[] ?? docs.ToArray();
            _indexInstanceDocument = infoSetFromDocumentToOrders.Any() ? infoSetFromDocumentToOrders.Where(w => w.DocumentType == (int)DocumentTypes.PaymentInvoice)
                .GroupBy(g => g.FileType)
                .Select(s => s.Count()).Max() + 1 : 1;
            var memoryStream = new MemoryStream();
            const float margeLeft = 1.5f;
            const float margeRight = 1.5f;
            const float margeTop = 1.0f;
            const float margeBottom = 1.0f;

            var pdf = new Document(
                                    PageSize.A4,
                                    margeLeft.ToDpi(),
                                    margeRight.ToDpi(),
                                    margeTop.ToDpi(),
                                    margeBottom.ToDpi()
                                   );

            pdf.AddTitle("Счет на оплату");
            pdf.AddAuthor(Order.OurCompany?.Name);
            pdf.AddCreationDate();
            var writer = PdfWriter.GetInstance(pdf, memoryStream);
            pdf.Open();

            await FillPdf(pdf, writer);
            pdf.Close();

            return await SaveToFile(memoryStream.ToArray());
        }

        private async Task<bool> SaveToFile(byte[] pdfBytes)
        { 
            if (Order == null) return false;
            var folderName = Path.Combine("StaticFiles", "Documents", "PaymentInvoices");
            var pathToSave = Path.Combine(_mainPathToFiles, folderName);
            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);
            
            var fileOutPdfName = $"Счет на оплату № {Order.Number}-{_indexInstanceDocument}.pdf";
            var fullOutPdfPath = Path.Combine(pathToSave, fileOutPdfName);

            await File.WriteAllBytesAsync(fullOutPdfPath, pdfBytes);
            if (!File.Exists(fullOutPdfPath))
                throw new FileNotFoundException();
            
            var document = new InfoSetFromDocumentToOrder()
            {
                Name = fileOutPdfName,
                FileType = (int)FileTypes.Pdf,
                DateTimeCreated = DateTime.Now,
                OrderId = Order.Id,
                DocumentType = (int)DocumentTypes.PaymentInvoice,
                PathToFile = fullOutPdfPath,
                StampPosition = _stampPosition
            };
            var resultDb = await _infoSetFromDocumentToOrderService.AddInfoSetToOrderAsync(document);
            if (resultDb == 0)
             File.Delete(fullOutPdfPath); 
            return resultDb != 0;
        }

        private async Task FillPdf(Document pdf, PdfWriter writer)
        {
            var ttf = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", "Fonts", "Arial.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            _font = new Font(baseFont, 10, Font.NORMAL);
            _fontBold = new Font(baseFont, 10, Font.BOLD);
            _fontBigBold = new Font(baseFont, 14, Font.BOLD); 
            _totalWidth = pdf.PageSize.Width - 3f.ToDpi();
            
            var bankTable = CreateBankTable();
            pdf.Add(bankTable);

            var title = new Paragraph($"Счет на оплату № {Order!.Number}-{_indexInstanceDocument}   от {DateTime.Now.ToLongDateString()}", _fontBigBold)
            {
                SpacingAfter = 18f,
                SpacingBefore = 10f
            };
            pdf.Add(title);

            var contractPartiesTable = CreateContractPartiesTable();
            contractPartiesTable.SpacingAfter = 10f;
            pdf.Add(contractPartiesTable);

            var productTable = await CreateProductTable();
            productTable.SpacingAfter = 20f;
            pdf.Add(productTable);

            var tablePaintings = CreatePaintingsTable();
           pdf.Add(tablePaintings);

            _stampPosition = writer.GetVerticalPosition(false);
        }

        private PdfPTable CreateBankTable()
        {
            var table = new PdfPTable(2)
            {
                TotalWidth = _totalWidth
            };

            var widths = new float[] { _totalWidth / 2, _totalWidth / 2 };
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
            var rightWidths = new float[] { 50, _totalWidth / 2 - 50 };
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
        private PdfPTable CreateContractPartiesTable()
        {
            var ourCompany = Order.OurCompany;
            var buyerCompany = Order.CompanyBuyer;
            var table = new PdfPTable(2)
            {
                TotalWidth = _totalWidth
            };
            if (buyerCompany != null && ourCompany != null)
            {
                var widths = new float[] { 80, _totalWidth - 80 };
                table.SetWidths(widths);
                table.LockedWidth = true;

                var rowOne = new PdfPTable(1);
                var rowOneTop = new PdfPCell(new Phrase("Поставщик", _font))
                {
                    Border = Rectangle.NO_BORDER,
                    BorderWidthTop = 1.5f,
                    PaddingTop = 10
                };
                rowOne.AddCell(rowOneTop);
                var rowOneBottom = new PdfPCell(new Phrase("(Исполнитель):", _font))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 10
                };
                rowOne.AddCell(rowOneBottom);
                var rowOneCellOne = new PdfPCell(rowOne) { Border = Rectangle.NO_BORDER };
                table.AddCell(rowOneCellOne);
                var rowOneCellTwo = new PdfPCell(new Phrase($"{ourCompany.Name},ИНН {ourCompany.FnsRequest?.INN},КПП ????, post???,{ourCompany.FnsRequest?.LegalAddress}  ", _fontBold))
                { Border = Rectangle.NO_BORDER, BorderWidthTop = 1.5f, PaddingTop = 10 };
                table.AddCell(rowOneCellTwo);

                var rowTwo = new PdfPTable(1);
                var rowTwoTop = new PdfPCell(new Phrase("Покупатель", _font)) { Border = Rectangle.NO_BORDER };
                rowTwo.AddCell(rowTwoTop);
                var rowTwoBottom = new PdfPCell(new Phrase("(Заказчик):", _font))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 10
                };
                rowTwo.AddCell(rowTwoBottom);
                var rowTwoCellOne = new PdfPCell(rowTwo) { Border = Rectangle.NO_BORDER };
                table.AddCell(rowTwoCellOne);
                var rowTwoCellTwo = new PdfPCell(new Phrase($"{buyerCompany.Name},ИНН {buyerCompany.FnsRequest?.INN},КПП ????, post???,{buyerCompany.FnsRequest?.LegalAddress}  ", _fontBold))
                {
                    Border = Rectangle.NO_BORDER,
                };
                table.AddCell(rowTwoCellTwo);

                var rowThreeCellOne = new PdfPCell(new Phrase("Основание:", _font)) { Border = Rectangle.NO_BORDER };
                var rowThreeCellTwo = new PdfPCell(new PdfPCell(new Phrase($"Договор №???????????? от 00.00.0000", _fontBold))) { Border = Rectangle.NO_BORDER };
                table.AddCell(rowThreeCellOne);
                table.AddCell(rowThreeCellTwo);
            }
            return table;
        }
        private async Task<PdfPTable> CreateProductTable()
        {
            var ourCompany = Order!.OurCompany;
            var buyerCompany = Order.CompanyBuyer;
            var productList = Order.ExportedProducts;
            decimal fullPrice = 0;
            var table = new PdfPTable(6)
            {
                TotalWidth = _totalWidth
            };
            if (productList != null)
            {
                var widthMainCell = _totalWidth / 2;
                var widths = new float[] { 30, widthMainCell, 60, 30, 80, 90 };
                table.SetWidths(widths);
                table.LockedWidth = true;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                var round = 2;


                table.AddCell(new PdfPCell(new Phrase("№", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Товары(работы, услуги)", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Кол-вo", _fontBold))
                {
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
                var fullPriceToPaymentCellRight = new PdfPCell(new Phrase((decimal.Round(fullPrice, round)).ToString(CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture), _fontBold))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(fullPriceToPaymentCellRight);

                table.AddCell(new PdfPCell(new Phrase($"Всего наименований {number - 1}, на сумму {(decimal.Round(fullPrice, round)).ToString(CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture)} руб.", _font))
                {
                    Colspan = 6,
                    Border = Rectangle.NO_BORDER,
                });
                var priceWord =
                    await _priceToString.ConvertNumber(decimal.Round(fullPrice, round), Order.TransactionCurrency ?? "RUB");
                table.AddCell(new PdfPCell(new Phrase($"Прописью {priceWord}.", _fontBold))
                {
                    Colspan = 6,
                    Border = Rectangle.NO_BORDER,
                });
                table.AddCell(new PdfPCell(new Phrase($"ИГК ?????{number}.", _font))
                {
                    Colspan = 6,
                    Border = Rectangle.NO_BORDER,
                    BorderWidthBottom = 1.5f,
                    PaddingTop = 10,
                    PaddingBottom = 10
                });

            }
            return table;
        }
        private PdfPTable CreatePaintingsTable()
        {
            var tablePaintings = new PdfPTable(4)
            {
                TotalWidth = _totalWidth
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
            return tablePaintings;
        }
    }
}
