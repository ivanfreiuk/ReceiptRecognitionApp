using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReceiptRecognitionApp.Entities;
using ReceiptRecognitionApp.Models;
using ReceiptRecognitionApp.Services.Interfaces;

namespace ReceiptRecognitionApp.Controllers
{
    public class FileController : Controller
    {
        private readonly IReceiptImageService _receiptImageService;
        private readonly IReceiptService _receiptService;

        public FileController(IReceiptImageService receiptImageService, IReceiptService receiptService)
        {
            _receiptImageService = receiptImageService;
            _receiptService = receiptService;
        }
        
        public IActionResult Index()
        {
            var images = _receiptService.GetAll();
            return View(images);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            ReceiptImage receiptImage;
            using (var stream = new MemoryStream())
            {
                await file.OpenReadStream().CopyToAsync(stream);
                var image = stream.ToArray();
                /* This code only for Ivan Freyuk 
                var response = GetScannedReceipt(image, file.FileName).Result;
                ByteArrayToImage(response.ScannedFile);
                */

                /*Comment next variable if you uncomment code above*/
                var response = new DocumentScannResponse
                {
                    FileExtension = ".png",
                    ScannedFile = image
                };

                 receiptImage = new ReceiptImage
                {
                    OriginalImageName = file.FileName,
                    OriginalImage = image,
                    ScannedImageName = string.Concat(Guid.NewGuid().ToString("N").Substring(0, 5), response.FileExtension),
                    ScannedImage = response.ScannedFile
                };
                var id = _receiptImageService.Add(receiptImage);
                return RedirectToAction("Index", "JsonFile", new { id });
            }
        }
        
        public async Task<DocumentScannResponse> GetScannedReceipt(byte[] image, string fileName)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(new MemoryStream(image)), "ImageFile", fileName);

                    using (var response = await client.PostAsync("http://localhost:7071/api/documentscanner", content))
                    {
                        var result = new DocumentScannResponse
                        {
                            FileExtension = response.Headers.GetValues("file-extension").First(),
                            ScannedFile = await response.Content.ReadAsByteArrayAsync()
                        };

                        return result;
                    }
                }
            }
        }
    }
}