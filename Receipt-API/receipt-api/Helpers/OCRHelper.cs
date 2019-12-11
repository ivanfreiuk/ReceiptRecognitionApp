using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace receipt_api.Helpers
{
    public class OCRHelper
    {
        public async Task<string> CallComputerVisionOCRAsync(byte[] image)
        {
            using (var client = new HttpClient())
            {
                string apikey = Secrets.ApiKey;
                string uri = Secrets.ApiendpointOcr;

                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apikey);

                // attach image content passed in via post                
                var content = new ByteArrayContent(content: image);
                content.Headers.ContentType = new MediaTypeHeaderValue(mediaType: "application/octet-stream");

                //send to Computer Vision OCR
                var result = await client.PostAsync(requestUri: uri, content: content);
                result.EnsureSuccessStatusCode();

                var operationLocation = result.Headers.GetValues("Operation-Location").First();

                return operationLocation;
            }
        }


        public async Task<Receipt> GetReceiptAsync(string uri)
        {
            OCRVisionResponse ocrResponse = await GetOperationResult(uri);            

            var receipt = OcrResultToReceipt(ocrResponse);
            receipt = ExtractDataOfInterest(receipt);

            return receipt;
        }

        public async Task<OCRVisionResponse> GetOperationResult(string apiUri)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Secrets.ApiKey);
            var result = await client.GetAsync(requestUri: apiUri);

            // Deserialize OCR response to our model and return
            var json = await result.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<OCRVisionResponse>(json); ;
        }

        public Receipt OcrResultToReceipt(OCRVisionResponse ocr)
        {
            var receipt = new Receipt
            {
                Lines = new List<string>()
            };

            var stringBuilder = new StringBuilder();
            if (ocr != null && ocr.recognitionResult != null)
            {
                foreach (var line in ocr.recognitionResult.lines)
                {
                    receipt.Lines.Add(line.text);

                    foreach (var word in line.words)
                    {
                        stringBuilder.Append(word.text);
                        stringBuilder.Append(" ");
                    }
                }
            }

            receipt.Text = stringBuilder.ToString();
            return receipt;
        }

        Receipt ExtractDataOfInterest(Receipt receipt)
        {

            string receiptText = string.Empty;
            string abn = string.Empty;
            decimal receipttotal = 0.00M;
            DateTime receiptdate = new DateTime(1900, 01, 01);

            receiptText = receipt.Text;

            // ABN : Australian Business Number
            if (receiptText.ToUpper().Contains("ABN") || receiptText.ToUpper().Contains("A.B.N"))
            {
                // try and extract an ABN from the line
                var extractedabn = ExtractABN(receiptText);

                // if the extracted ABN passes validation, lets accept and set it
                if (ValidateABN(extractedabn)) { abn = extractedabn; }
            }

            // Receipt Date
            var extractedDateString = ExtractDate(receiptText);
            DateTime extractedDate = new DateTime(1900, 01, 01);
            if (extractedDateString != "")
            {
                DateTime.TryParse(extractedDateString, System.Globalization.CultureInfo.GetCultureInfo("en-AU"), System.Globalization.DateTimeStyles.None, out extractedDate);

            }
            if (extractedDate > receiptdate && extractedDate <= DateTime.Today)
            {
                // we're going to keep the highest date we find (for now)
                receiptdate = extractedDate;
            }

            // Receipt Total
            if (receiptText.ToUpper().Contains("TOTAL") ||
                receiptText.ToUpper().Contains("SUBTOTAL") ||
                receiptText.ToUpper().Contains("SALE AMOUNT") ||
                receiptText.ToUpper().Contains("CASH") ||
                receiptText.ToUpper().Contains("CREDIT") ||
                receiptText.ToUpper().Contains("PAID") ||
                receiptText.ToUpper().Contains("EFT") ||
                receiptText.ToUpper().Contains("EFTPOS")
                )
            {
                var extractedMoneyString = ExtractMoney(receiptText);
                var extractedMoney = 0.00M;
                if (extractedMoneyString != string.Empty)
                {
                    decimal.TryParse(extractedMoneyString, out extractedMoney);
                    if (extractedMoney > receipttotal) { receipttotal = extractedMoney; }
                }
            }

            receipt.Language = "UK";
            if (abn != string.Empty) { receipt.ABN = abn; }
            if (receiptdate != new DateTime(1900, 01, 01)) { receipt.ReceiptDate = receiptdate.ToString("dd-MMM-yyyy"); }
            if (receipttotal != 0) { receipt.ReceiptTotal = receipttotal.ToString(); }

            // send our final processed receipt object back
            return receipt;
        }

        // Returns the last valid money string found in the line
        static string ExtractMoney(string line)
        {
            string moneystring = "";
            decimal money = 0.00M;
            string pat = @"[0-9]+( ?\. ?[0-9][0-9])"; // also match $21 .80 or 21. 80 (with space)

            foreach (Match match in Regex.Matches(line, pat))
            {
                Decimal extractedMoney = 0.00M;
                moneystring = match.Value.Replace(" ", "");
                Decimal.TryParse(moneystring, out extractedMoney);
                if (extractedMoney > money) { money = extractedMoney; }
            }
            return money.ToString(); //trim spaces from string before returning: fixes 21 .80
        }

        // Returns last valid date string found in the line
        static string ExtractDate(string line)
        {
            string receiptdate = "";
            // match dates "01/05/2018" "01-05-2018" "01-05-18" "01 05 18" "01 05 2018"
            string pat = @"\s*((31([-/ .])((0?[13578])|(1[02]))\3(\d\d)?\d\d)|((([012]?[1-9])|([123]0))([-/ .])((0?[13-9])|(1[0-2]))\12(\d\d)?\d\d)|(((2[0-8])|(1[0-9])|(0?[1-9]))([-/ .])0?2\22(\d\d)?\d\d)|(29([-/ .])0?2\25(((\d\d)?(([2468][048])|([13579][26])|(0[48])))|((([02468][048])|([13579][26]))00))))\s*";
            foreach (Match match in Regex.Matches(line, pat))
            {
                receiptdate = match.Value.Trim();
                receiptdate = receiptdate.Replace("-", "/");
                receiptdate = receiptdate.Replace(".", "/");
                receiptdate = receiptdate.Replace(" ", "/");
            }

            // didnt find date?  now we'll try searching with month names.  03 OCT 2017, 03 October 2017 etc
            if (receiptdate == "")
            {
                pat = @"((31(?![-/ .](Feb(ruary)?|Apr(il)?|June?|(Sep(?=\b|t)t?|Nov)(ember)?)))|((30|29)(?![-/ .]Feb(ruary)?))|(29(?=[-/ .]Feb(ruary)?[-/ .](((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])[-/ .](Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|Ju((ly?)|(ne?))|Aug(ust)?|Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)[-/ .]((1[6-9]|[2-9]\d)\d{2})";

                foreach (Match match in Regex.Matches(line, pat, RegexOptions.IgnoreCase))
                {
                    receiptdate = match.Value.Trim();
                    receiptdate = receiptdate.Replace("/", "-");
                    receiptdate = receiptdate.Replace(".", "-");
                    receiptdate = receiptdate.Replace(" ", "-");
                }
            }

            return receiptdate;
        }

        // Returns the last #valid# ABN number found in the line

        static string ExtractABN(string line)
        {

            // search line for ABN
            // could be 11 digit block, or 11 digits with spaces
            // e.g. 44418573722 or 44 418 573 722
            // only return if passes checksum validation via ValidateABN()
            // 

            string abn = "";
            string pat = @"(\d *?){11}";

            Regex regexObj = new Regex(pat);
            Match matchObj = regexObj.Match(line);
            while (matchObj.Success)
            {
                string extractedABN = matchObj.Value.Replace(" ", "");
                if (ValidateABN(extractedABN)) { abn = extractedABN; }
                matchObj = regexObj.Match(line, matchObj.Index + 1);
            }

            return abn;
        }

        // Validates if an 11 digit number could be an ABN
        //1. Subtract 1 from the first (left) digit to give a new eleven digit number         
        //2. Multiply each of the digits in this new number by its weighting factor         
        //3. Sum the resulting 11 products         
        //4. Divide the total by 89, noting the remainder         
        //5. If the remainder is zero the number is valid          
        static bool ValidateABN(string abn)
        {
            bool isValid = true; int[] weight = { 10, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 }; int weightedSum = 0;
            //0. ABN must be 11 digits long                         
            if (isValid &= (!string.IsNullOrEmpty(abn) && Regex.IsMatch(abn, @"^\d{11}$")))
            {
                //Rules: 1,2,3                                  
                for (int i = 0; i < weight.Length; i++) { weightedSum += (int.Parse(abn[i].ToString()) - ((i == 0) ? 1 : 0)) * weight[i]; }
                //Rules: 4,5                 
                isValid &= ((weightedSum % 89) == 0);
            }
            return isValid;
        }

    }
}
