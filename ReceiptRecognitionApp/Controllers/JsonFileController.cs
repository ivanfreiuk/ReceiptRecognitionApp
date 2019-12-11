using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
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
        private const string ApiUrl = "http://localhost:54292/api/receipts";
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
                    Text = receipt.Text,
                    Json = receipt.Json,
                }
            });
        }
        
        public async Task<IActionResult> FormJson(int id)
        {
            //here should be called api to get response and convert it to ReceiptResponseModel
            //var response = new ReceiptResponseModel
            //{
            //    ABN = "ABN",
            //    Businessname = "Businessname",
            //    Language = "Language",
            //    Lines = new List<string>
            //    {
            //        "Line 1",
            //        "Line 2",
            //        "Line 3"
            //    },
            //    ReceiptDate = "29/03/11 23:12:12",
            //    ReceiptTotal = "12212,12",
            //    Text = "Some text"
            //};
            var receiptImage = _receiptImageService.Get(id);
            var url = await RequestReceiptProcessing(receiptImage.OriginalImage);
            Task.Delay(5000).Wait();
            var receipt = await GetReceiptResult(url);
            var data = new JsonFileViewModel
            {
                ReceiptImage = receiptImage,
                Receipt = new Receipt
                {
                    ReceiptDate = receipt.ReceiptDate,
                    ReceiptTotal = receipt.ReceiptTotal,
                    ReceiptImageId = id,
                    Text = receipt.Text,
                    Json = JsonConvert.SerializeObject(receipt)
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

        private static async Task<string> RequestReceiptProcessing(byte[] receiptImage)
        {
            var client = new HttpClient();
            var content = new ByteArrayContent(receiptImage);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType: "application/octet-stream");
            var response = await client.PostAsync(ApiUrl, content);
            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<object>(json);
            return result.operationLocation;
        }

        private static async Task<ReceiptResponseModel> GetReceiptResult(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"{ApiUrl}?uri={url}");
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ReceiptResponseModel>(json);
            return result;
        }
    }
}