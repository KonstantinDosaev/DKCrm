using DKCrm.Server.Interfaces;
using System.Reflection;
using TemplateEngine.Docx;
using System.Reflection.PortableExecutable;
using OpenXmlPowerTools;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using Spire.Doc.Pages;
using Spire.Doc;
using Spire.Doc.Documents;


using System.Xml.Linq;
using SaveOptions = System.Xml.Linq.SaveOptions;
using System.Collections;
using System.Drawing.Imaging;
using System.Globalization;

namespace DKCrm.Server.Services
{
    public class DocumentService : IDocumentService
    {
        public void CreatePaymentInvoiceAsync(Guid orderId)
        {
            //for (int i = 0; i < doc.Sections.Count; i++)
           // {
                //Document newWord = new Document();
                //newWord.Sections.Add(doc.Sections[i].Clone());
                //newWord.SaveToFile($@"{fullOutSplitItemPath}{i}.docx");
           // }
        }
        public string CreateAsync()
        {
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

                var fileOutPdfName = "sampleOutLong.html";
                var fullOutPdfPath = Path.Combine(pathToSave, fileOutPdfName); 
                var oo = "sampleWordFile.docx";
                var ooo = Path.Combine(pathToSave, oo);

           //ConvertToPdfLongDoc(fullOutPath, fullOutPdfPath);

           ConvertToHtml(fullOutPath, fullOutPdfPath);
            //Pp(fullOutPath);
            return fileOutName;
        }

        //private void Pp(string pat)
        //{
        //    var folderName = Path.Combine("StaticFiles", "Images");
        //    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        //    var fileOutPdfName = "sampleOutp.pdf";
        //    var fullOutPdfPath = Path.Combine(pathToSave, fileOutPdfName);

        //    var document = PdfReader.Open(pat, PdfDocumentOpenMode.Import);

        //    // Convert Word to PDF using PdfSharp
        //    document.Save(fullOutPdfPath);


        //}
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

          
                using (WordprocessingDocument doc = WordprocessingDocument.Open(inputFilePath, true))
                {
                    HtmlConverterSettings settings = new HtmlConverterSettings()
                    {
                        PageTitle = "My Page Title",

                    };
                    XElement html = HtmlConverter.ConvertToHtml(doc, settings);
                    File.WriteAllText(fullHtmlTempPath, html.ToStringNewLineOnAttributes());


                    var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

                    htmlToPdf.GeneratePdfFromFile(fullHtmlTempPath, null, outputFilePath);

                }
            




        }

        static void ConvertWordToPdf(string inputFilePath, string outputFilePath)
        {
            //using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(inputFilePath, false))
            //{
            //    // Extract text from Word document
            //    string text = wordDoc.MainDocumentPart.Document.Body.InnerText;

            //    // Create a PDF document
            //    using (PdfDocument pdfDocument = new PdfDocument())
            //    {
            //        PdfPage pdfPage = pdfDocument.AddPage();
            //        XGraphics gfx = XGraphics.FromPdfPage(pdfPage);

            //        // Set font and other formatting options as needed
            //        XFont font = new XFont("Arial", 9, XFontStyle.Regular);

            //        // Draw the text on the PDF page
            //       gfx.DrawString(text, font, XBrushes.Black, new XRect(10, 10, pdfPage.Width - 20, pdfPage.Height - 20));

            //        // Save the PDF document
            //        pdfDocument.Save(outputFilePath);
            //    }
            //}

        

        }

        public static void ConvertToHtml(string inputFilePath, string outputFilePath) //, string outputDirectory
        {
           
                dynamic wdApp = Activator.CreateInstance(Type.GetTypeFromProgID("Word.Application"));
                dynamic wdDoc = wdApp.Documents.Open(inputFilePath, AddToRecentFiles: false);
                float appVersion = float.Parse(wdApp.Version.ToString(), CultureInfo.InvariantCulture);
                string fullPath = outputFilePath;
                if (appVersion < 14)
                {
                    wdDoc.SaveAs(fullPath, 10, AddToRecentFiles: false);
                }
                else
                {
                    wdDoc.SaveAs2(fullPath, 10, AddToRecentFiles: false);
                }
                wdDoc.Close();
                wdApp.Quit();
            
        }
    }
}

