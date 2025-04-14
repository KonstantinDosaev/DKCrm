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
using Microsoft.EntityFrameworkCore;


namespace DKCrm.Server.Services.DocumentServices
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDBContext _context;
        private readonly IInfoSetFromDocumentToOrderService _infoSetFromDocumentToOrderService;

        private readonly PaymentInvoicePdfGenerator _generatorPayment;
        private readonly OrderSpecificationPdfGenerator _generatorSpecification;
        private readonly CommercialOfferPdfGenerator _generatorOffer;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly string _mainPathToPrivateDirectory;
        public DocumentService(ApplicationDBContext context, PaymentInvoicePdfGenerator generatorPayment, IInfoSetFromDocumentToOrderService infoSetFromDocumentToOrderService, OrderSpecificationPdfGenerator generatorSpecification, IFileService fileService, IConfiguration configuration, CommercialOfferPdfGenerator generatorOffer)
        {
            _context = context;
            _generatorPayment = generatorPayment;
            _infoSetFromDocumentToOrderService = infoSetFromDocumentToOrderService;
            _generatorSpecification = generatorSpecification;
            _fileService = fileService;
            _configuration = configuration;
            _generatorOffer = generatorOffer;
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
            var document = new InfoSetToDocument()
            {
                Name = fileToDbName,
                FileType = (int)FileTypes.Documents,
                DateTimeCreated = DateTime.Now,
                OwnerId = request.OrderId,
                OwnerType = (int)DocumentOwner.ExportedOrder,
                Extension = (int)DocumentExtension.Pdf,
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
            var document = new InfoSetToDocument()
            {
                Name = fileToDbName,
                FileType = (int)FileTypes.Documents,
                DateTimeCreated = DateTime.Now,
                OwnerId = request.OrderId,
                Extension = (int)DocumentExtension.Pdf,
                OwnerType = (int)DocumentOwner.ExportedOrder,
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
          public async Task<bool> CreateCommercialOfferPdfAsync(CreateCommercialOfferPdfRequest request, ClaimsPrincipal user)
        {
            var byteArr = await _generatorOffer.CreateOfferAsync(request, user);
            var pathToDirectory = Path.Combine(_mainPathToPrivateDirectory, request.OrderId.ToString());
            if (!Directory.Exists(pathToDirectory))
            {
                Directory.CreateDirectory(pathToDirectory);
            }
            var countDoc = await GetCountDocumentByType(DocumentTypes.CommercialOffer, request.OrderId);
            var fileToDbName = $"КП от {DateTime.Now.Date}-{countDoc}.pdf";
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
            var document = new InfoSetToDocument()
            {
                Name = fileToDbName,
                FileType = (int)FileTypes.Documents,
                DateTimeCreated = DateTime.Now,
                OwnerId = request.OrderId,
                OwnerType = (int)DocumentOwner.ExportedOrder,
                Extension = (int)DocumentExtension.Pdf,
                DocumentType = (int)DocumentTypes.CommercialOffer,
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
        /*private async Task<bool> SaveToFile(byte[] pdfBytes, string pathToDirectory,string fullOutPath)
        {
            if (!Directory.Exists(pathToDirectory))
                Directory.CreateDirectory(pathToDirectory);
            if (File.Exists(fullOutPath))
                File.Delete(fullOutPath);
            
            await File.WriteAllBytesAsync(fullOutPath, pdfBytes);
            return File.Exists(fullOutPath);
        }*/
        public async Task<byte[]> AddStampToPdfAsync(AddStampToPdfRequest request)
        {
            try
            {
               // var infoSet = await _infoSetFromDocumentToOrderService.GetOneAsync(request.InfoSetId);
                byte[] byt = new byte[] { };
               // var folderName = Path.Combine("StaticFiles", "Images");
                //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var firstSt = request.StampPositionList.FirstOrDefault();
                if (firstSt != null)
                {
                    var image = firstSt.StampImage;
                    var imageGuid = firstSt.StampImageId;
                    //var image = await 
                    // var document = new Document();
                    var mainPath = _configuration[$"{DirectoryType.PrivateFolder}"];
                    var bytDocument = await GetDocumentBytArrayAsync(request.InfoSetId);
                    var pdfReader = new PdfReader(bytDocument);
                    var tempFilePath = Path.Combine(mainPath!, "tempPdf.pdf");
                    //var fs = new MemoryStream(byt);
                    var fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                    var stamp = new PdfStamper(pdfReader, fs);
                    var img = Image.GetInstance(image);
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
                            var page1 = page;
                            var currentStampPositionInPage = request.StampPositionList
                                .Where(f => f.PageNumber == page1);
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
                }

                return byt;
            }

            catch
            {
                throw new InvalidOperationException();
            }
        }

        public async Task<int> GetCountDocumentByType(DocumentTypes documentType, Guid orderId)
        {
             var indexInstanceDocument = 0;
            var docs = await _infoSetFromDocumentToOrderService
                .GetAllInfoSetsDocumentsToOrderAsync(orderId);
            var infoSetFromDocumentToOrderCollection = docs as InfoSetToDocument[] ?? docs.ToArray();
            var infoSetByType = infoSetFromDocumentToOrderCollection
                .Where(w => w.DocumentType == (int)documentType).ToArray();
            if (infoSetByType.Any())
            {
                indexInstanceDocument = infoSetByType.GroupBy(g => g.FileType)
                    .Select(s => s.Count()).Max(s => s) + 1;
            }
            return indexInstanceDocument;
        }

        public async Task<bool> UploadDocumentFileAsync(UploadDocumentRequest request)
        {
            var pathToDirectory = Path.Combine(PathsToDirectories.FileContainer, PathsToDirectories.Documents, 
                request.OwnerType.ToString(), request.DocumentType.ToString(), request.OwnerId.ToString());
            var tt = await _fileService.SaveFileAsync(new SaveFileRequest()
            {
                DirectoryType = request.DirectoryType,
                FileName = request.FileName,
                IsFullPath = request.IsFullPath,
                OwnerId = request.OwnerId,
                ContentType = FileTypes.Documents,
                PathToDirectory = pathToDirectory,
                Content = request.Content,
                Preview = request.Preview
            });
            var result = 0;
            if (!string.IsNullOrEmpty(tt.FileName))
            {
                var infoSet = 
                    new InfoSetToDocument()
                    {
                        Name = request.FileName,
                        FileType = (int)FileTypes.Documents,
                        DateTimeCreated = DateTime.Now,
                        OwnerId = request.OwnerId,
                        OwnerType = (int)request.OwnerType,
                        Extension = (int)request.Extension,
                        DocumentType = (int)request.DocumentType,
                        PathToFile = Path.Combine(pathToDirectory, tt.FileName)
                    };
                _context.InfoSetsToDocuments.Entry(infoSet).State = EntityState.Added;
                result = await _context.SaveChangesAsync();
            }
            return result > 0;
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

