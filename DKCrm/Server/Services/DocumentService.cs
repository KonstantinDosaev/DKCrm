using DKCrm.Server.Interfaces;
//using DocumentFormat.OpenXml.Wordprocessing;

//using DocXToPdfConverter;
using DocXToPdfConverter;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.DocumentInterfaces;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Server.Services.DocumentServices;
using DKCrm.Shared.Models.OrderModels;


namespace DKCrm.Server.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDBContext _context;
        private readonly IDocumentToOrderService _documentToOrderService;
        private readonly PaymentInvoicePdfGenerator _generator;
        public DocumentService(IExportedOrderService orderService, IPriceToStringConverter priceToString, ApplicationDBContext context, PaymentInvoicePdfGenerator generator, IDocumentToOrderService documentToOrderService)
        {
            _orderService = orderService;
            _priceToString = priceToString;
            _context = context;
            this._generator = generator;
            _documentToOrderService = documentToOrderService;
        }

        public ExportedOrder Order { get; set; } = null!;
        private readonly IExportedOrderService _orderService;
        private readonly IPriceToStringConverter _priceToString;
     
        public async Task<bool> CreatePaymentInvoicePdfAsync(Guid orderId)
        {
           return await _generator.CreatePaymentAsync(orderId);
            //for (int i = 0; i < doc.Sections.Count; i++)
            // {
            //Document newWord = new Document();
            //newWord.Sections.Add(doc.Sections[i].Clone());
            //newWord.SaveToFile($@"{fullOutSplitItemPath}{i}.docx");
            // }
        }

        
        public void CreateDocumentAsync(Guid orderId)
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




            //var arr = await ReportPdf();
            //File.Delete(fullOutPdfPath);
            //await File.WriteAllBytesAsync(fullOutPdfPath, arr.Item1);


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

