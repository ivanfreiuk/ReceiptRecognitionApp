using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using receipt_api.Helpers;

namespace receipt_api.Controllers
{
    [Route("api/receipts")]
    public class ReceiptsController : Controller
    {
        // GET api/receipts
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string uri)
        {
            var ocr = new OCRHelper();

            var receipt = await ocr.GetReceiptAsync(uri);

            return Ok(receipt);
        }

        // POST api/receipts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] byte[] content)
        {
            var ocr = new OCRHelper();

            var response = await ocr.CallComputerVisionOCRAsync(content);

            return Ok(new { OperationLocation = response});            
        }
    }
}