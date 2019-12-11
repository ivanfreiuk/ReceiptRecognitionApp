using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        
        public IActionResult Index(int id)
        {
            var receiptImage = _receiptImageService.Get(id);
            var receipt = _receiptService.FirstOrDefault(id);
            if (receipt == null)
                return View(new JsonFileViewModel
                {
                    ReceiptImage = receiptImage
                });

            return View(new JsonFileViewModel
            {
                ReceiptImage = receiptImage,
                Receipt = new Receipt
                {
                    ReceiptDate = receipt.ReceiptDate,
                    ReceiptTotal = receipt.ReceiptTotal,
                    ReceiptImageId = id,
                    Text = receipt.Text
                }
            });
        }
        
        public IActionResult FormJson(int id)
        {
            //here should be called api to get response and convert it to ReceiptResponseModel
            var response = new ReceiptResponseModel
            {
                ABN = "ABN",
                Businessname = "Businessname",
                Language = "Language",
                Lines = new List<string>
                {
                    "Line 1",
                    "Line 2",
                    "Line 3"
                },
                ReceiptDate = "29/03/11 23:12:12",
                ReceiptTotal = "12212,12",
                Text = "Some text"
            };
            var receiptImage = _receiptImageService.Get(id);
            var data = new JsonFileViewModel
            {
                ReceiptImage = receiptImage,
                Receipt = new Receipt
                {
                    ReceiptDate = response.ReceiptDate,
                    ReceiptTotal = response.ReceiptTotal,
                    ReceiptImageId = id,
                    Text = response.Text,
                    Json = JsonConvert.SerializeObject(response)
                }
            };
            SaveJson(data);
            return View("Index", data);
        }
        
        private void SaveJson(JsonFileViewModel model)
        {
            _receiptService.Add(new Receipt
            {
                ReceiptImageId = model.ReceiptImage.Id,
                ReceiptDate = model.Receipt.ReceiptDate,
                ReceiptTotal = model.Receipt.ReceiptTotal,
                Text = model.Receipt.Text,
                Json = model.Receipt.Json
            });
        }
    }
}