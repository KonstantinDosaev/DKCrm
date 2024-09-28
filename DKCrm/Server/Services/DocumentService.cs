using DKCrm.Server.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.DocumentInterfaces;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Server.Services.DocumentServices;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using iTextSharp.text.pdf.events;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;


namespace DKCrm.Server.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDBContext _context;
        private readonly IInfoSetFromDocumentToOrderService _infoSetFromDocumentToOrderService;

        private readonly PaymentInvoicePdfGenerator _generatorPayment;
        private readonly OrderSpecificationPdfGenerator _generatorSpecification;
        public DocumentService(ApplicationDBContext context, PaymentInvoicePdfGenerator generatorPayment, IInfoSetFromDocumentToOrderService infoSetFromDocumentToOrderService, OrderSpecificationPdfGenerator generatorSpecification)
        {
            _context = context;
            _generatorPayment = generatorPayment;
            _infoSetFromDocumentToOrderService = infoSetFromDocumentToOrderService;
            _generatorSpecification = generatorSpecification;
        }
        public async Task<byte[]> GetDocumentBytArrayAsync(Guid infoSetId)
        {
            var infoSet = await _infoSetFromDocumentToOrderService.GetOneAsync(infoSetId);
            var byt = await File.ReadAllBytesAsync(infoSet.PathToFile);
            return byt;
        }
       public async Task<int> RemoveDocumentAsync(Guid infoSetId)
        {
            var infoSet = await _infoSetFromDocumentToOrderService.GetOneAsync(infoSetId);
            File.Delete(infoSet.PathToFile);
            if (!File.Exists(infoSet.PathToFile))
                _context.Entry(infoSet).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }
        
        public async Task<bool> CreatePaymentInvoicePdfAsync(Guid orderId)
        {
            return await _generatorPayment.CreatePaymentAsync(orderId);
        }

        public async Task<bool> CreateOrderSpecificationPdfAsync(CreateOrderSpecificationRequest createOrderSpecificationRequest)
        {
            return await _generatorSpecification.CreateSpecificationAsync(createOrderSpecificationRequest);
        }

        public async Task<byte[]> AddStampToPdfAsync(AddStampToPdfRequest request)
        {
            try
            {
                var infoSet = await _infoSetFromDocumentToOrderService.GetOneAsync(request.InfoSetId);
                
                byte[] byt = new byte[] { };
                var folderName = Path.Combine("StaticFiles", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var image = request.StampPositionList.FirstOrDefault()?.StampImageName;
                // var document = new Document();
                var pdfReader = new PdfReader(infoSet.PathToFile);
                var fs = new MemoryStream(byt);
                var stamp = new PdfStamper(pdfReader, fs);
                var img = iTextSharp.text.Image.GetInstance(image);
                img.RotationDegrees = 0;
                img.ScaleToFit(100f, 100f); ;
                var pageNumbersToStampLists = request.StampPositionList.Select(s=>s.PageNumber).ToArray();
                var maxPage = pageNumbersToStampLists.Max();
                for (var page = 1; page <= maxPage; page++)
                {
                    if (pageNumbersToStampLists.Contains(page))
                    {
                        var currentStampPosition = request.StampPositionList
                            .FirstOrDefault(f => f.PageNumber == page)!;
                     
                        if (image != currentStampPosition.StampImageName)
                        {
                            img = iTextSharp.text.Image.GetInstance(image);
                            img.RotationDegrees = 0;
                            img.ScaleToFit(100f, 100f);
                        }
                        
                        var positionX = (float)currentStampPosition.PercentOfLeftEdge;
                        var positionY = (float)currentStampPosition.PercentOfBottomEdge;
                        img.SetAbsolutePosition(positionX, positionY);
                        var waterMarkImage = stamp.GetOverContent(page);
                        var x = waterMarkImage.Xtlm;
                        var y = waterMarkImage.Ytlm;
                        waterMarkImage.AddImage(img);
                    }

                }
                stamp.FormFlattening = true;
                stamp.Close();
                // axAcroPDF1.LoadFile(fs.Name);
                fs.Close();
                pdfReader.Close();
                // document.Dispose();

                return byt;
            }

            catch
            {
                throw;
            }
        }

        //private void AddStampToPdf(string inputFilePath, string outputFilePath, float positionY, DocumentTypes documentType)
        //{
        //    try
        //    {
        //        float positionX = documentType switch
        //        {
        //            DocumentTypes.PaymentInvoice => 100,
        //            DocumentTypes.OrderSpecification => 200,
        //            _ => 0
        //        };
        //        var folderName = Path.Combine("StaticFiles", "Images");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        var image = Path.Combine(pathToSave, "im.png");
        //        var document = new Document();
        //        var pdfReader = new PdfReader(inputFilePath);
        //        var fs = new FileStream(outputFilePath, FileMode.Create);
        //        var stamp = new PdfStamper(pdfReader, fs);

        //        var img = iTextSharp.text.Image.GetInstance(image);
        //        img.RotationDegrees = 0;
        //        img.ScaleToFit(100f, 100f);

        //        img.SetAbsolutePosition(positionX, positionY - 50);

        //        for (int page = 1; page <= pdfReader.NumberOfPages; page++)
        //        {
        //            if (page== pdfReader.NumberOfPages)
        //            {
        //                var waterMarkImage = stamp.GetOverContent(page);
        //                waterMarkImage.AddImage(img);
        //            }

        //        }
        //        stamp.FormFlattening = true;
        //        stamp.Close();
        //        // axAcroPDF1.LoadFile(fs.Name);
        //        fs.Close();
        //        pdfReader.Close();
        //        document.Dispose();

        //    }

        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}

