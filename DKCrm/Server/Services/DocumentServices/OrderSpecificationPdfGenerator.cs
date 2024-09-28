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

namespace DKCrm.Server.Services.DocumentServices;
public class OrderSpecificationPdfGenerator
{
        private ExportedOrder? Order { get; set; }
        private CreateOrderSpecificationRequest? CreateOrderSpecificationRequest { get; set; }
        private readonly IExportedOrderService _orderService;
        private readonly IInfoSetFromDocumentToOrderService _infoSetFromDocumentToOrderService;
        private readonly IConfiguration _configuration;
    private Font _font = null!;
        private Font _fontBold = null!;
        private Font _fontBigBold = null!;
        private float _stampPosition;
        private float _totalWidth;
        private int _indexInstanceDocument;
        private string _mainPathToFiles  = null!;

        public OrderSpecificationPdfGenerator(IExportedOrderService orderService, 
            IInfoSetFromDocumentToOrderService infoSetFromDocumentToOrderService, IConfiguration configuration)
        {
            _orderService = orderService;
            _infoSetFromDocumentToOrderService = infoSetFromDocumentToOrderService;
            _configuration = configuration;
        }

        public async Task<bool> CreateSpecificationAsync(CreateOrderSpecificationRequest createOrderSpecificationRequest)
        {
            CreateOrderSpecificationRequest = createOrderSpecificationRequest;
            if (CreateOrderSpecificationRequest == null) return false;
           
            Order = await _orderService.GetDetailAsync(createOrderSpecificationRequest.OrderId);
            _mainPathToFiles = _configuration[$"{DirectoryType.PrivateFolder}"] ?? throw new InvalidOperationException();

        var docs = await _infoSetFromDocumentToOrderService
                .GetAllInfoSetsDocumentsToOrderAsync(Order.Id);
            var infoSetFromDocumentToOrders = docs as InfoSetFromDocumentToOrder[] ?? docs.ToArray();
            var infoSetFromSpecification = infoSetFromDocumentToOrders
                .Where(w => w.DocumentType == (int)DocumentTypes.OrderSpecification).ToArray();
            _indexInstanceDocument = infoSetFromSpecification.Any() ? infoSetFromSpecification
                .Where(w => w.DateTimeCreated.Date == DateTime.Now.Date)
                .GroupBy(g => g.FileType)
                .Select(s => s.Count()).Max(s=>s) + 1 : 1;
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

            pdf.AddTitle("Спецификация");
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
           
            var pathToSave = Path.Combine(_mainPathToFiles + PathsToDirectories.Documents, Order.Number!);
        if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);
        var date = DateTime.Now.Date.ToShortDateString();
        var fileOutPdfName = $"СПЕЦИФИКАЦИЯ № {Order.Number}-{date}-{_indexInstanceDocument}.pdf";
            var fullOutPdfPath = Path.Combine(pathToSave, fileOutPdfName);

            await File.WriteAllBytesAsync(fullOutPdfPath, pdfBytes);
            if (!File.Exists(fullOutPdfPath))
                throw new FileNotFoundException();
            
            var document = new InfoSetFromDocumentToOrder()
            {
                Name = fileOutPdfName,
                FileType = (int)FileTypes.Documents,
                DateTimeCreated = DateTime.Now,
                OrderId = Order.Id,
                DocumentType = (int)DocumentTypes.OrderSpecification,
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
            var ttf = Path.Combine(_mainPathToFiles + PathsToDirectories.Fonts, "times.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            _font = new Font(baseFont, 10, Font.NORMAL);
            _fontBold = new Font(baseFont, 10, Font.BOLD);
            _fontBigBold = new Font(baseFont, 14, Font.BOLD); 
            _totalWidth = pdf.PageSize.Width - 3f.ToDpi();
            var spaceAfterBase = 10f;
            var title = new Paragraph($"СПЕЦИФИКАЦИЯ № {Order!.Number}-{_indexInstanceDocument}   " +
                                      $"от {DateTime.Now.ToLongDateString()}", _fontBigBold)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = spaceAfterBase,
                SpacingBefore = 10f
            };
            pdf.Add(title);
            var subTitle = new Paragraph($"к контракту № ???????? от ?????????", _fontBold)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = spaceAfterBase,
            };
            pdf.Add(subTitle);

            var naming = new Paragraph();
            foreach (var stringToFont in CreateOrderSpecificationRequest!.NamingCondition)
            {
                var font = stringToFont.FontType switch
                {
                    FontTypes.Normal10 => _font,
                    FontTypes.Bold10 => _fontBold,
                    FontTypes.Bold14 => _fontBigBold,
                    FontTypes.Normal14 => _font,
                    _ => _font
                };
                naming.Add(new Phrase(stringToFont.Value, font));
            }
            naming.SpacingAfter = spaceAfterBase;
            pdf.Add(naming);
            var productTable = CreateProductTable();
            productTable.SpacingAfter = spaceAfterBase;
            pdf.Add(productTable);
            var conditionsTable = CreateConditionsTable();
            conditionsTable.SpacingAfter = spaceAfterBase;
            pdf.Add(conditionsTable);
            var paintingsTable = CreatePaintingsTable(writer);
            pdf.Add(paintingsTable);
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
                var widths = new [] { 30, 180, 40, 60, 80, 90 ,90};
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
                    table.AddCell(new Phrase($"{CreateOrderSpecificationRequest!.DaysOfDelivery}", _font));
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
                table.AddCell(profitCellLeft);
                var profitCellRight = new PdfPCell(new Phrase( "?????", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };
                table.AddCell(profitCellRight);

            }
            return table;
        }
        private PdfPTable CreateConditionsTable()
        {
            var table = new PdfPTable(2)
            {
                TotalWidth = _totalWidth
            };
            var widths = new [] { 30, _totalWidth - 30};
            table.SetWidths(widths);
            table.LockedWidth = true;
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            var index = 1;
            foreach (var stringToFont in CreateOrderSpecificationRequest!.Conditions)
            {
                var font = stringToFont.FontType switch
                {
                    FontTypes.Normal10 => _font,
                    FontTypes.Bold10 => _fontBold,
                    FontTypes.Bold14 => _fontBigBold,
                    FontTypes.Normal14 => _font,
                    _ => _font
                };
                var numCell = new PdfPCell(new Phrase($"{index}.", font))
                {
                    Border = Rectangle.NO_BORDER,
                };
                table.AddCell(numCell);
                var conditionCell = new PdfPCell(new Phrase(stringToFont.Value, font))
                {
                    Border = Rectangle.NO_BORDER,
                };
                table.AddCell(conditionCell);
                index++;
            }
            return table;
        }
        private PdfPTable CreatePaintingsTable(PdfWriter writer)
        {
            var tablePaintings = new PdfPTable(2)
            {
                TotalWidth = _totalWidth
            };
            var widths = new [] { _totalWidth/2 , _totalWidth/2 };
            tablePaintings.SetWidths(widths);
            tablePaintings.LockedWidth = true;
            tablePaintings.AddCell(new PdfPCell(new Phrase($"ПОСТАВЩИК", _fontBold))
            {
                Border = Rectangle.NO_BORDER,
            });
            tablePaintings.AddCell(new PdfPCell(new Phrase($"ПОКУПАТЕЛЬ", _fontBold))
            {
                Border = Rectangle.NO_BORDER,
                 HorizontalAlignment = Element.ALIGN_RIGHT
            });
            var sellerCertifyingPerson = CreateOrderSpecificationRequest!.SellerCertifyingPersonId;
            var buyerCertifyingPerson = CreateOrderSpecificationRequest!.BuyerCertifyingPersonId;
            tablePaintings.AddCell(new PdfPCell(new Phrase(sellerCertifyingPerson.Position, _font))
            {
                Border = Rectangle.NO_BORDER,
            });
            tablePaintings.AddCell(new PdfPCell(new Phrase(buyerCertifyingPerson.Position, _font))
            {
                Border = Rectangle.NO_BORDER,
                 HorizontalAlignment = Element.ALIGN_RIGHT
            });
            _stampPosition = writer.GetVerticalPosition(false);
            tablePaintings.AddCell(new PdfPCell(new Phrase($"_______________ {sellerCertifyingPerson.LastName} " +
                                                           $"{sellerCertifyingPerson.FirstName?[0]}. " +
                                                           $"{sellerCertifyingPerson.MiddleName?[0]}.", _font))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT
            });


        tablePaintings.AddCell(new PdfPCell(new Phrase($"_______________ {buyerCertifyingPerson.LastName} " +
                                                       $"{buyerCertifyingPerson.FirstName?[0]}. " +
                                                       $"{buyerCertifyingPerson.MiddleName?[0]}.", _font))
        {
            Border = Rectangle.NO_BORDER,
            HorizontalAlignment = Element.ALIGN_RIGHT
        });
        return tablePaintings;
        }
}