using System.Security.Claims;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DKCrm.Server.Data;
using DKCrm.Server.Interfaces;
using DKCrm.Server.Interfaces.DocumentInterfaces;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Requests.FileService;
using iTextSharp.text.pdf.events;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using DocumentFormat.OpenXml.Drawing.Charts;
using OpenXmlPowerTools;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Spreadsheet;


namespace DKCrm.Server.Services.DocumentServices
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDBContext _context;
        private readonly IInfoSetFromDocumentToOrderService _infoSetFromDocumentToOrderService;

        private readonly PaymentInvoicePdfGenerator _generatorPayment;
        private readonly OrderSpecificationPdfGenerator _generatorSpecification;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly string _mainPathToPrivateDirectory;
        public DocumentService(ApplicationDBContext context, PaymentInvoicePdfGenerator generatorPayment, IInfoSetFromDocumentToOrderService infoSetFromDocumentToOrderService, OrderSpecificationPdfGenerator generatorSpecification, IFileService fileService, IConfiguration configuration)
        {
            _context = context;
            _generatorPayment = generatorPayment;
            _infoSetFromDocumentToOrderService = infoSetFromDocumentToOrderService;
            _generatorSpecification = generatorSpecification;
            _fileService = fileService;
            _configuration = configuration;
            _mainPathToPrivateDirectory =  Path.Combine(PathsToDirectories.FileContainer, PathsToDirectories.OrdersDocuments);
        }
        public async Task<byte[]> GetDocumentBytArrayAsync(Guid infoSetId)
        {
            var infoSet = await _infoSetFromDocumentToOrderService.GetOneAsync(infoSetId);
            var byt = await _fileService.GetFileInBytArrayAsync(new GetFileRequest()
            {
                IsFullPath = false, DirectoryType = DirectoryType.PrivateFolder, Path = infoSet.PathToFile
            });
            return byt;
        }
        public async Task<int> RemoveDocumentAsync(Guid infoSetId)
        {
            var infoSet = await _infoSetFromDocumentToOrderService.GetOneAsync(infoSetId);
            if (File.Exists(infoSet.PathToFile))
                File.Delete(infoSet.PathToFile);
            _context.Entry(infoSet).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> CreatePaymentInvoicePdfAsync(CreatePaymentInvoiceRequest request, ClaimsPrincipal user)
        {
            var byteArr = await _generatorPayment.CreatePaymentAsync(request, user);
            var pathToDirectory = Path.Combine(_mainPathToPrivateDirectory, request.OrderId.ToString());
            if (!Directory.Exists(pathToDirectory))
            {
                Directory.CreateDirectory(pathToDirectory);
            }
            var countDoc = await GetCountDocumentByType(DocumentTypes.PaymentInvoice, request.OrderId);
            var fileToDbName = $"Счет на оплату от {DateTime.Now.Date}-{countDoc}.pdf";
            var saveResult = await _fileService.SaveFileAsync(new SaveFileRequest()
            {
                ContentType = FileTypes.Documents,
                DirectoryType = DirectoryType.PrivateFolder,
                Content = byteArr,
                PathToDirectory = pathToDirectory,
                FileName = fileToDbName,
                IsFullPath = false,
            });
            if (string.IsNullOrEmpty(saveResult.FileName))
                return false;

            var pathToDb = Path.Combine(pathToDirectory, saveResult.FileName);
            var document = new InfoSetFromDocumentToOrder()
            {
                Name = fileToDbName,
                FileType = (int)FileTypes.Documents,
                DateTimeCreated = DateTime.Now,
                OrderId = request.OrderId,
                DocumentType = (int)DocumentTypes.PaymentInvoice,
                PathToFile = pathToDb,
            };
            var resultDb = await _infoSetFromDocumentToOrderService.AddInfoSetToOrderAsync(document);
            if (resultDb == 0)
            {
                _fileService.RemoveFile(new RemoveFileRequest()
                {
                    FileType = FileTypes.Documents,
                    DirectoryType = DirectoryType.PrivateFolder,
                    FileName = saveResult.FileName,
                    IsFullPath = false,
                    Path = pathToDb
                });
            }
            return resultDb == 1;
        }

        public async Task<bool> CreateOrderSpecificationPdfAsync(CreateOrderSpecificationRequest request, ClaimsPrincipal user)
        {
            var byteArr = await _generatorSpecification.CreateSpecificationAsync(request, user);
            var pathToDirectory = Path.Combine(_mainPathToPrivateDirectory, request.OrderId.ToString());
            if(!Directory.Exists(pathToDirectory))
            {
                Directory.CreateDirectory(pathToDirectory);
            }
            var countDoc = await GetCountDocumentByType(DocumentTypes.OrderSpecification, request.OrderId);
            var fileToDbName = $"Спецификация от {DateTime.Now.Date}-{countDoc}.pdf";
            var saveResult = await _fileService.SaveFileAsync(new SaveFileRequest()
            {
                ContentType = FileTypes.Documents,
                DirectoryType = DirectoryType.PrivateFolder, Content = byteArr, 
                PathToDirectory = pathToDirectory,
                FileName = fileToDbName, 
                IsFullPath = false,
            });
            if (string.IsNullOrEmpty(saveResult.FileName))
                return false;
            
            var pathToDb = Path.Combine(pathToDirectory, saveResult.FileName);
            var document = new InfoSetFromDocumentToOrder()
            {
                Name = fileToDbName,
                FileType = (int)FileTypes.Documents,
                DateTimeCreated = DateTime.Now,
                OrderId = request.OrderId,
                DocumentType = (int)DocumentTypes.OrderSpecification,
                PathToFile = pathToDb,
            };
            var resultDb = await _infoSetFromDocumentToOrderService.AddInfoSetToOrderAsync(document);
            if (resultDb == 0)
            { 
                _fileService.RemoveFile(new RemoveFileRequest()
                {
                    FileType = FileTypes.Documents,
                    DirectoryType = DirectoryType.PrivateFolder,
                    FileName = saveResult.FileName, IsFullPath = false,
                    Path = pathToDb
                });
            }
            return resultDb == 1;
        }
        
        private async Task<bool> SaveToFile(byte[] pdfBytes, string pathToDirectory,string fullOutPath)
        {
            if (!Directory.Exists(pathToDirectory))
                Directory.CreateDirectory(pathToDirectory);
            if (File.Exists(fullOutPath))
                File.Delete(fullOutPath);
            
            await File.WriteAllBytesAsync(fullOutPath, pdfBytes);
            return File.Exists(fullOutPath);
        }
        public async Task<byte[]> AddStampToPdfAsync(AddStampToPdfRequest request)
        {
            try
            {
                var infoSet = await _infoSetFromDocumentToOrderService.GetOneAsync(request.InfoSetId);

                byte[] byt = new byte[] { };
                var folderName = Path.Combine("StaticFiles", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var firstSt = request.StampPositionList.FirstOrDefault();
                var image = firstSt.StampImage;
                var imageGuid = firstSt.StampImageId;
                //var image = await 
                // var document = new Document();
                var mainPath = _configuration[$"{DirectoryType.PrivateFolder}"];
                var bytDocument = await GetDocumentBytArrayAsync(request.InfoSetId);
                var pdfReader = new PdfReader(bytDocument);
                var tempFilePath = Path.Combine(mainPath, "tempPdf.pdf");
                //var fs = new MemoryStream(byt);
                var fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                var stamp = new PdfStamper(pdfReader, fs);
                var img = iTextSharp.text.Image.GetInstance(image);
                img.RotationDegrees = 0;
                var stampWidth = 100f;
                var stampHeight = 100f;
                img.ScaleToFit(stampWidth, stampHeight);
                var pageNumbersToStampLists = request.StampPositionList.Select(s => s.PageNumber).ToArray();
                var maxPage = pageNumbersToStampLists.Max();
                for (var page = 1; page <= maxPage; page++)
                {
                    var pageSize = pdfReader.GetPageSize(page);
                    var x = pageSize.Width;
                    var y = pageSize.Height;
                    if (pageNumbersToStampLists.Contains(page))
                    {
                        var currentStampPositionInPage = request.StampPositionList
                            .Where(f => f.PageNumber == page)!;
                        foreach (var stampPos in currentStampPositionInPage)
                        {
                            if (imageGuid != stampPos.StampImageId)
                            {
                                imageGuid = stampPos.StampImageId;
                                img = Image.GetInstance(image);
                                img.RotationDegrees = 0;
                                img.ScaleToFit(stampWidth, stampHeight);
                            }
                            var procXSt = (float)stampPos.PercentOfLeftEdge;
                            var procYSt = (float)stampPos.PercentOfBottomEdge;
                            var positionX = ((x / 100) * procXSt);
                            var positionY = ((y / 100) * procYSt);
                           
                            img.SetAbsolutePosition(positionX, positionY);
                            
                            var waterMarkImage = stamp.GetOverContent(page);
                           
                            waterMarkImage.AddImage(img);
                        }
                       
                    }

                }
                
                stamp.FormFlattening = true;
                stamp.Close();
                // axAcroPDF1.LoadFile(fs.Name);
                fs.Close();
                pdfReader.Close();
                // document.Dispose();
                byt = await File.ReadAllBytesAsync(tempFilePath);
               
                return byt;
            }

            catch
            {
                throw;
            }
        }

        public async Task<int> GetCountDocumentByType(DocumentTypes documentType, Guid orderId)
        {
             var indexInstanceDocument = 0;
            var docs = await _infoSetFromDocumentToOrderService
                .GetAllInfoSetsDocumentsToOrderAsync(orderId);
            var infoSetFromDocumentToOrders = docs as InfoSetFromDocumentToOrder[] ?? docs.ToArray();
            var infoSetFromPayment = infoSetFromDocumentToOrders
                .Where(w => w.DocumentType == (int)documentType).ToArray();
            if (infoSetFromPayment.Any())
            {
                indexInstanceDocument = infoSetFromPayment.GroupBy(g => g.FileType)
                    .Select(s => s.Count()).Max(s => s) + 1;
            }
            return indexInstanceDocument;
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

