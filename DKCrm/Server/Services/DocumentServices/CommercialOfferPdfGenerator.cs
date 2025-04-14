using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Globalization;
using System.Security.Claims;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Shared.Models.CompanyModels;
using OpenXmlPowerTools;
using iTextSharp.text.html.simpleparser;

namespace DKCrm.Server.Services.DocumentServices
{
    public class CommercialOfferPdfGenerator
    {
        private ExportedOrder? Order { get; set; }
        private CreateCommercialOfferPdfRequest? CreateCommercialOfferPdfRequest { get; set; }
        private readonly IExportedOrderService _orderService;
        private readonly ICompanyService _companyService;
        private readonly IInfoSetFromDocumentToOrderService _infoSetFromDocumentToOrderService;
        private readonly IConfiguration _configuration;
        private Font _font = null!;
        private Font _fontBig= null!;
        private Font _fontBold = null!;
        private Font _fontBigBold = null!;
        private float _totalWidth;
        private int _indexInstanceDocument;
        private string _mainPathToFiles = null!;
        private ClaimsPrincipal _user;
        private Company _ourCompany;
        public CommercialOfferPdfGenerator(IExportedOrderService orderService,
            IInfoSetFromDocumentToOrderService infoSetFromDocumentToOrderService, IConfiguration configuration, ICompanyService companyService)
        {
            _orderService = orderService;
            _infoSetFromDocumentToOrderService = infoSetFromDocumentToOrderService;
            _configuration = configuration;
            _companyService = companyService;
        }

        public async Task<byte[]> CreateOfferAsync(CreateCommercialOfferPdfRequest createRequest, ClaimsPrincipal user)
        {
            _user = user;
            CreateCommercialOfferPdfRequest = createRequest;
            if (CreateCommercialOfferPdfRequest == null) return new byte[] { };

            Order = await _orderService.GetDetailAsync(CreateCommercialOfferPdfRequest.OrderId, user);
            _mainPathToFiles = _configuration[$"{DirectoryType.PrivateFolder}"] ?? throw new InvalidOperationException();
             _ourCompany = await _companyService.GetAsync((Guid)Order.OurCompanyId!, _user);
            var docs = await _infoSetFromDocumentToOrderService
                    .GetAllInfoSetsDocumentsToOrderAsync(Order.Id);
            var infoSetFromDocumentToOrders = docs as InfoSetToDocument[] ?? docs.ToArray();
            var infoSetFromSpecification = infoSetFromDocumentToOrders
                .Where(w => w.DocumentType == (int)DocumentTypes.OrderSpecification).ToArray();
            _indexInstanceDocument = infoSetFromSpecification.Any() ? infoSetFromSpecification
                .Where(w => w.DateTimeCreated.Date == DateTime.Now.Date)
                .GroupBy(g => g.FileType)
                .Select(s => s.Count()).Max(s => s) + 1 : 1;
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

            pdf.AddTitle("Коммерческое предложение");
            pdf.AddAuthor(Order.OurCompany?.Name);
            pdf.AddCreationDate();
            var writer = PdfWriter.GetInstance(pdf, memoryStream);
            pdf.Open();
            FillPdf(pdf, writer);
            pdf.Close();

            return memoryStream.ToArray();
        }

        private void FillPdf(Document pdf, PdfWriter writer)
        {
            var ttf = Path.Combine(_mainPathToFiles, PathsToDirectories.FileContainer, PathsToDirectories.Fonts, "times.ttf");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            _font = new Font(baseFont, 10, Font.NORMAL);
            _fontBig = new Font(baseFont, 14, Font.NORMAL);
            _fontBold = new Font(baseFont, 10, Font.BOLD);
            _fontBigBold = new Font(baseFont, 14, Font.BOLD);
            _totalWidth = pdf.PageSize.Width - 3f.ToDpi();
            var spaceAfterBase = 10f;

            var companyTable = CreateCompanyTable();
            companyTable.SpacingAfter = spaceAfterBase;
            pdf.Add(companyTable);

            var title = new Paragraph($"Коммерческое предложение № {Order!.Number}-{_indexInstanceDocument}   " +
                                      $"от {DateTime.Now.ToLongDateString()}", _fontBigBold)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = spaceAfterBase,
                SpacingBefore = 10f
            };
            pdf.Add(title);
            
            var productTable = CreateProductTable();
            productTable.SpacingAfter = spaceAfterBase;
            pdf.Add(productTable);
        }

        private PdfPTable CreateCompanyTable()
        {
            var bank = CreateCommercialOfferPdfRequest!.OurSelectedBank;
           

                var table = new PdfPTable(2)
            {
                TotalWidth = _totalWidth,
                DefaultCell = { Border = Rectangle.NO_BORDER }
                };
                if (_ourCompany == null)
                    return table;
            var widths = new[] { 120, _totalWidth - 120 };
            table.SetWidths(widths);
            table.LockedWidth = true;
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            var img = Image.GetInstance(CreateCommercialOfferPdfRequest.Avatar);
            img.RotationDegrees = 0;
            img.ScaleToFit(120, 120);
           // img.SetAbsolutePosition(positionX, positionY);

            var left = new PdfPTable(1){ DefaultCell = { Border = Rectangle.NO_BORDER } };
            var leftOne= new PdfPCell(new Phrase($"", _font))
            {
                Border = Rectangle.NO_BORDER,
                BorderWidthBottom = 0
            };
            leftOne.AddElement(img);
            left.AddCell(leftOne);
            var usingLeft = new PdfPCell(left)
            {
                Border = Rectangle.NO_BORDER,
                Padding = 0f
            };
            table.AddCell(usingLeft);

            var right = new PdfPTable(1){ DefaultCell = { Border = Rectangle.NO_BORDER } };
           
            var rightOne = new PdfPCell(new Phrase($"{_ourCompany.Name.ToUpper()}", _fontBigBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER, 
                PaddingBottom = 10,
                PaddingTop = 10
            };
            right.AddCell(rightOne);

            var rightTwo = new PdfPCell(new Phrase("ПОСТАВЩИК ИМПОРТНЫХ ЭЛЕКТРОННЫХ КОМПОНЕНТОВ", _fontBig))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                PaddingBottom = 10
            };
            right.AddCell(rightTwo);
            var rightThree = new PdfPCell(new Phrase($"ИНН {_ourCompany.FnsRequest!.INN}", _font))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
            };
            right.AddCell(rightThree);
            var rightFour = new PdfPCell(new Phrase($"Юридический адрес: {_ourCompany.FnsRequest.LegalAddress}", _font))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
            };
            right.AddCell(rightFour);
            var rightFive = new PdfPCell(new Phrase($@"Наименование банка: {bank.Name}. Расчетный счет: {bank.BeneficiaryAccount}. Кор. счет: {bank.KorBeneficiaryAccount}. БИК: {bank.BankAccount}.", _font))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
            };
            right.AddCell(rightFive);
            var rightSix = new PdfPCell(new Phrase($@"Контакты: тел. {_ourCompany.Phone} email: {_ourCompany.Email}", _font))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
            };
            right.AddCell(rightSix);


            var usingRight = new PdfPCell(right)
            {
                Padding = 0f,
                Border = Rectangle.NO_BORDER
            };
            table.AddCell(usingRight);
            return table;
        }

        private PdfPTable CreateProductTable()
        {
            var productList = Order!.ExportedProducts;
            decimal fullPrice = 0;
            var table = new PdfPTable(6)
            {
                TotalWidth = _totalWidth
            };
            if (productList != null)
            {
                var widthMainCell = _totalWidth / 2;
                var widths = new[] { 30, 160, 90, 90, 100, 100 };
                table.SetWidths(widths);
                table.LockedWidth = true;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                var round = 2;


                table.AddCell(new PdfPCell(new Phrase("№", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Пратномер", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });

                table.AddCell(new PdfPCell(new Phrase("Кол-во запрошено", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Кол-вo предложение", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                var curr = Order.TransactionCurrency;

                    table.AddCell(new PdfPCell(new Phrase($"Цена за ед. с НДС, ({curr}.)", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                table.AddCell(new PdfPCell(new Phrase("Срок поставки, (дней)", _fontBold))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });
                var number = 1;
                foreach (var exportedProduct in productList)
                {
                    var reserved = GetAllReserveQuantity(exportedProduct);
               

                    var numCell = new PdfPCell(new Phrase(number.ToString(), _font))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(numCell);
                    table.AddCell(new Phrase(exportedProduct.Product?.PartNumber?.ToString() ?? "", _font));
                 
                    var quantityRequestedCell = new PdfPCell(new Phrase(exportedProduct.Quantity.ToString(), _font))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    table.AddCell(quantityRequestedCell);
                    var quantityCell = new PdfPCell(new Phrase(reserved.ToString(), _font))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    table.AddCell(quantityCell);
                    var price = exportedProduct.PriceInTransactionCurrency ?? 0;
                    var priceNds = Math.Round(price + (price / 100 * (decimal)Order.Nds) ,4);
                    table.AddCell(new Phrase($"{ priceNds}", _font));
                   
                    var daysCell = new PdfPCell(new Phrase($"{exportedProduct.MinDaysForDeliveryPlaned} - {exportedProduct.MaxDaysForDeliveryPlaned} дн.", _font))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    table.AddCell(daysCell);
                    number++;
                }
            }
            return table;
        }
        
        private int GetAllReserveQuantity(ExportedProduct exportedProduct)
        {
            var quantityInReserveAll = 0;
            if (exportedProduct!.SoldFromStorage != null)
                quantityInReserveAll += exportedProduct.SoldFromStorage.Select(s => s.Quantity).Sum();

            if (exportedProduct.PurchaseAtExports != null)
                quantityInReserveAll += exportedProduct.PurchaseAtExports.Select(s => s.Quantity).Sum();

            return quantityInReserveAll;
        }
    }
}
