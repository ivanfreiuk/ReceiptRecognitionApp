using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using System.Drawing;
using ReceiptRecognitionApp.Entities;
using ReceiptRecognitionApp.Models;
using ReceiptRecognitionApp.Services.Interfaces;

namespace ReceiptRecognitionApp.Controllers
{
    public class FileController : Controller
    {
        private readonly IReceiptImageService _receiptImageService;

        public FileController(IReceiptImageService receiptImageService)
        {
            _receiptImageService = receiptImageService;
        }

        public string FileExtension { get; set; }

        public IActionResult Index()
        {
            var images = _receiptImageService.GetAll();
            return View(images);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
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
                    FileExtension = ".png"
                };

                var receiptImage = new ReceiptImage
                {
                    OriginalImageName = file.FileName,
                    OriginalImage = image,
                    ScannedImageName = string.Concat(Guid.NewGuid().ToString("N").Substring(0, 5), response.FileExtension),
                    ScannedImage = response.ScannedFile
                };

                _receiptImageService.Add(receiptImage);

            }

            return RedirectToAction("Index", "Home");
        }
        
        public Image ByteArrayToImage(byte[] content)
        {
            using (var stream = new MemoryStream(content))
            {
                var image = Image.FromStream(stream);
                var fileName = $"{Guid.NewGuid().ToString()}{FileExtension}";
                var filePath = $"C:\\Users\\Ivan_Freiuk\\Desktop\\scanned\\{fileName}";
                image.Save(filePath, ImageFormat.Jpeg);
                return image;
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