using Microsoft.AspNetCore.Mvc;
using ReceiptRecognitionApp.Entities;
using ReceiptRecognitionApp.Models;
using ReceiptRecognitionApp.Services.Interfaces;

namespace ReceiptRecognitionApp.Controllers
{
    public class JsonFileController : Controller
    {
        private readonly IReceiptImageService _receiptImageService;
        private readonly IReceiptService _receiptService;

        public JsonFileController(IReceiptImageService receiptImageService, IReceiptService receiptService)
        {
            _receiptImageService = receiptImageService;
            _receiptService = receiptService;
        }
        
        public IActionResult Index([FromQuery]int id)
        {
            var receiptImageId = (int)TempData["receiptImageId"];
            var receiptImage = _receiptImageService.Get(receiptImageId);
            TempData["receiptImageId"] = receiptImageId;
            return View(new JsonFileViewModel
            {
                ReceiptImage = receiptImage
            });
        }
        
        public IActionResult FormJson(int id)
        {
            var json = "Json";
            var receiptImageId = (int)TempData["receiptImageId"];
            var receiptImage = _receiptImageService.Get(receiptImageId);
            TempData["receiptImageId"] = receiptImageId;
            var data = new JsonFileViewModel
            {
                ReceiptImage = receiptImage,
                Json = json
            };
            SaveJson(data);
            return View("Index", data);
        }

        private void SaveJson(JsonFileViewModel model)
        {
            _receiptService.Add(new Receipt
            {
                Json = model.Json,
                ReceiptImageId = model.ReceiptImage.Id
            });
        }
    }
}