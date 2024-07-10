using DKCrm.Server.Interfaces.DocumentInterfaces;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models.OrderModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.CompanyModels;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;
using PageSize = iTextSharp.text.PageSize;
using Paragraph = iTextSharp.text.Paragraph;

namespace DKCrm.Server.Services.DocumentServices;
public class OrderSpecificationPdfGenerator
{
    private ExportedOrder? Order { get; set; }
    private Company? OurCompany { get; set; }
    private Company? BuyerCompany { get; set; }
        private readonly IExportedOrderService _orderService;
        private readonly ICompanyService _companyService;
        private readonly IPriceToStringConverter _priceToString;
        private readonly IInfoSetFromDocumentToOrderService _infoSetFromDocumentToOrderService;
        private Font _font = null!;
        private Font _fontBold = null!;
        private Font _fontBigBold = null!;
        private float _stampPosition;
        private float _totalWidth;
        private int _indexInstanceDocument;
        private string _mainPathToFiles  = null!;

        public OrderSpecificationPdfGenerator(IExportedOrderService orderService, IPriceToStringConverter priceToString, 
            IInfoSetFromDocumentToOrderService infoSetFromDocumentToOrderService, ICompanyService companyService)
        {
            _orderService = orderService;
            _priceToString = priceToString;
            _infoSetFromDocumentToOrderService = infoSetFromDocumentToOrderService;
            _companyService = companyService;
        }

        public async Task<bool> CreateSpecificationAsync(CreateOrderSpecificationRequest createOrderSpecificationRequest)
        {
            Order = await _orderService.GetDetailAsync(createOrderSpecificationRequest.OrderId);
            _mainPathToFiles = Directory.GetCurrentDirectory();
            // Order = await _orderService.GetDetailAsync(new Guid("f41f77b1-0b1d-4025-b972-d985d742d772"));
            if (Order?.OurCompanyId == null || Order.CompanyBuyerId == null) return false;
            OurCompany = await _companyService.GetAsync((Guid)Order.OurCompanyId);
            BuyerCompany = await _companyService.GetAsync((Guid)Order.CompanyBuyerId);
            if (OurCompany == null || BuyerCompany == null) return false;
            
            var docs = await _infoSetFromDocumentToOrderService
                .GetAllInfoSetsDocumentsToOrderAsync(Order.Id);
            var infoSetFromDocumentToOrders = docs as InfoSetFromDocumentToOrder[] ?? docs.ToArray();
            _indexInstanceDocument = infoSetFromDocumentToOrders.Any() ? infoSetFromDocumentToOrders
                .Where(w => w.DocumentType == (int)DocumentTypes.PaymentInvoice)
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
            FillPdf(pdf, writer);
            pdf.Close();

            return await SaveToFileAsync(memoryStream.ToArray());
        }

        private async Task<bool> SaveToFileAsync(byte[] pdfBytes)
        { 
            if (Order == null) return false;
            var folderName = Path.Combine("StaticFiles", "Documents", "PaymentInvoices");
            var pathToSave = Path.Combine(_mainPathToFiles, folderName);
            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);
            
            var fileOutPdfName = $"СПЕЦИФИКАЦИЯ № {Order.Number}-{_indexInstanceDocument}.pdf";
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

        private void FillPdf(Document pdf, PdfWriter writer)
        {
            var ttf = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", "Fonts", "Arial.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            _font = new Font(baseFont, 10, Font.NORMAL);
            _fontBold = new Font(baseFont, 10, Font.BOLD);
            _fontBigBold = new Font(baseFont, 14, Font.BOLD); 
            _totalWidth = pdf.PageSize.Width - 3f.ToDpi();
            
            var title = new Paragraph($"СПЕЦИФИКАЦИЯ № {Order!.Number}-{_indexInstanceDocument}   " +
                                      $"от {DateTime.Now.ToLongDateString()}", _fontBigBold)
            {
                SpacingAfter = 18f,
                SpacingBefore = 10f
            };
            pdf.Add(title);
            var subTitle = new Paragraph($"к контракту № ????????   " +
                                      $"от ?????????", _fontBold)
            {
                SpacingAfter = 18f,
                SpacingBefore = 10f
            };
            pdf.Add(subTitle);
            var naming = new Paragraph
            {
                new Phrase($"{OurCompany?.FnsRequest?.Name}, в лице Генерального директора {OurCompany?.FnsRequest?.Director}, " +
                           $"действующим на основании Устава, именуемое в дальнейшем ", _font),
                new Phrase("Поставщик", _fontBold),
                new Phrase($" и {BuyerCompany?.FnsRequest?.Name}, в лице {BuyerCompany?.FnsRequest?.Director}, действующего на основании Устава, именуемое в дальнейшем ", _font),
                new Phrase("Покупатель", _fontBigBold),
                new Phrase($", при совместном упоминании именуемые ", _font),
                new Phrase("Стороны", _fontBigBold),
                new Phrase($", согласовали поставку следующего товара: ", _font)
            };
            pdf.Add(naming);
            /*var contractPartiesTable = CreateContractPartiesTable();
            contractPartiesTable.SpacingAfter = 10f;
            pdf.Add(contractPartiesTable);

            var tablePaintings = CreatePaintingsTable();
           pdf.Add(tablePaintings);*/
            var productTable = CreateProductTable();
            productTable.SpacingAfter = 20f;
            pdf.Add(productTable);
            var conditionsTable = CreateConditionsTable();
            conditionsTable.SpacingAfter = 20f;
            pdf.Add(conditionsTable);
            _stampPosition = writer.GetVerticalPosition(false);
        }

        private PdfPTable CreateProductTable()
        {
            var productList = Order!.ExportedProducts;
            decimal fullPrice = 0;
            var table = new PdfPTable(7)
            {
                TotalWidth = _totalWidth
            };
            if (productList != null)
            {
                var widthMainCell = _totalWidth / 2;
                var widths = new [] { 30, 180, 30, 60, 90, 90 ,90};
                table.SetWidths(widths);
                table.LockedWidth = true;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                var round = 2;


                table.AddCell(new PdfPCell(new Phrase("№", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Наименование товара(работ, услуг)", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                
                table.AddCell(new PdfPCell(new Phrase("Ед. изм.", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Кол-вo, (ед.)", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Срок поставки, (дней)", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Цена без НДС, (руб./ ед.)", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Сумма, (руб.)", _fontBold))
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
                    table.AddCell(new Phrase("шт.", _font));
                    var quantityCell = new PdfPCell(new Phrase(exportedProduct.Quantity.ToString(), _font))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    table.AddCell(quantityCell);
                    table.AddCell(new Phrase("????", _font));
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
                var fullPriceCellLeft = new PdfPCell(new Phrase("Итого без НДС,(руб.):", _fontBold))
                {
                    Colspan = 6,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(fullPriceCellLeft);
                var fullPriceCellRight = new PdfPCell(new Phrase((decimal.Round(fullPrice, round)).ToString(CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture), _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(fullPriceCellRight);
                var ndsCellLeft = new PdfPCell(new Phrase($"НДС {Order.Nds}%, (руб.):", _fontBold))
                {
                    Colspan = 6,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(ndsCellLeft);
                var nds = fullPrice / 100 * (decimal)Order.Nds!;
                var ndsCellRight = new PdfPCell(new Phrase((decimal.Round(nds, round)).ToString(CultureInfo.CurrentCulture), _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(ndsCellRight);
                var fullPriceToPaymentCellLeft = new PdfPCell(new Phrase("Всего с НДС,(руб.):", _fontBold))
                {
                    Colspan = 6,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(fullPriceToPaymentCellLeft);
                var fullPriceToPaymentCellRight = new PdfPCell(new Phrase((decimal.Round(fullPrice +nds, round)).ToString(CultureInfo.CurrentCulture), _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(fullPriceToPaymentCellRight);

                var profitCellLeft = new PdfPCell(new Phrase("В т.ч. прибыль по Спецификации,(руб.):", _fontBold))
                {
                    Colspan = 6,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(fullPriceToPaymentCellLeft);
                var profitCellRight = new PdfPCell(new Phrase( "?????", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(fullPriceToPaymentCellRight);

            }
            return table;
        }
        private PdfPTable CreateConditionsTable()
        {
            var table = new PdfPTable(7)
            {
                TotalWidth = _totalWidth
            };
            var widths = new [] { 30, _totalWidth - 30};
            table.SetWidths(widths);
            table.LockedWidth = true;
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            string[] conditions =
            {
                "Базис поставки - франко-склад Покупателя.",
                "Срок поставки с момента подписания настоящей Спецификации Сторонами.",
                "Порядок расчетов: Покупатель производит 100% оплату стоимости Товара надлежащего качества на расчетный" +
                " счет Поставщика в течении ______ банковских дней с момента приемки Товара и подписания обеими сторонами" +
                " товарной накладной (универсального передаточного документа).",
                "Настоящяя Спецификация вступает в силу с момента её подписания Сторонами."
            };
            for (var i = 0; i < conditions.Length; i++)
            {
                var numCell = new PdfPCell(new Phrase($"{i}.", _font))
                {
                    Border = Rectangle.NO_BORDER,
                    //HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(numCell);
                var conditionCell = new PdfPCell(new Phrase(conditions[i], _font))
                {
                    Border = Rectangle.NO_BORDER,
                    //HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(conditionCell);
            }
            return table;
        }
        private PdfPTable CreatePaintingsTable()
        {
            var tablePaintings = new PdfPTable(4)
            {
                TotalWidth = _totalWidth
            };
            var widths = new [] { _totalWidth/2 , _totalWidth/2 };
            tablePaintings.SetWidths(widths);
            tablePaintings.LockedWidth = true;
            tablePaintings.AddCell(new PdfPCell(new Phrase($"ПОСТАВЩИК", _fontBigBold))
            {
                Border = Rectangle.NO_BORDER,
               // HorizontalAlignment = Element.ALIGN_CENTER
            });
            tablePaintings.AddCell(new PdfPCell(new Phrase($"ПОКУПАТЕЛЬ", _fontBigBold))
            {
                Border = Rectangle.NO_BORDER,
                // HorizontalAlignment = Element.ALIGN_CENTER
            });
            tablePaintings.AddCell(new PdfPCell(new Phrase("", _font))
            {
                Border = Rectangle.NO_BORDER,
                // HorizontalAlignment = Element.ALIGN_CENTER
            });
            var directorOurArr = Order!.OurCompany?.Director!.Split(' ');
            tablePaintings.AddCell(new PdfPCell(new Phrase($"{directorOurArr?[0]} {directorOurArr?[1][0]}. {directorOurArr?[2][0]}. _______________", _font))
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

            tablePaintings.AddCell(new PdfPCell(new Phrase($"{directorOurArr?[0]} {directorOurArr?[1][0]}. {directorOurArr?[2][0]}. _______________", _font))
            {
                Border = Rectangle.NO_BORDER,
                BorderWidthBottom = 1f,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            return tablePaintings;
        }
}