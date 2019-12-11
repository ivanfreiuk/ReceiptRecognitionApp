using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ByteArrayFormatters;
using System.Net.Http;
using System.Net.Http.Headers;
using receipt_api.Helpers;

namespace receipt_api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
            //var ocr = new OCRProcessing();

            //return new JsonResult(await ocr.GetReceiptAsync(uri, Secrets.ApiKey));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] byte[] content)
        {

            //OCRProcessing ocr = new OCRProcessing();

            return Ok(); // new JsonResult(await ocr.CallComputerVisionOCRAsync(content));
            // return new JsonResult(await ocr.ProcessReceiptImage(content));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
