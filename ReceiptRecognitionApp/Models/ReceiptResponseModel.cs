using System.Collections.Generic;

namespace ReceiptRecognitionApp.Models
{
    public class ReceiptResponseModel
    {
        public string Language { get; set; }

        public string ABN { get; set; }

        public string Businessname { get; set; }

        public string ReceiptDate { get; set; }

        public string ReceiptTotal { get; set; }

        public string Text { get; set; }

        public List<string> Lines { get; set; }
    }
}
