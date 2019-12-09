using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using System.Drawing;

namespace ReceiptRecognitionApp.Controllers
{
    public class FileController : Controller
    {
        public string FileExtension { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                await file.OpenReadStream().CopyToAsync(stream);
                var image = stream.ToArray();
                byte[] data = GetScannedReceipt(image).Result;

                ByteArrayToImage(data);
            }

            return Ok();
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

        public async Task<byte[]> GetScannedReceipt(byte[] image)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(new MemoryStream(image)), "ImageFile", "receipt.jpg");

                    using (var response = await client.PostAsync("http://localhost:7071/api/documentscanner", content))
                    {
                        FileExtension = response.Headers.GetValues("file-extension").First().ToString();
                        var result = await response.Content.ReadAsByteArrayAsync();

                        return result;
                    }
                }
            }
        }
    }
}