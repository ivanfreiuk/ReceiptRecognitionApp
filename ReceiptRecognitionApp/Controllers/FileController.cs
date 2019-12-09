using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReceiptRecognitionApp.Controllers
{
    public class FileController : Controller
    {
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
                var data = stream.ToArray();
            }

            return Ok();
        }
    }
}