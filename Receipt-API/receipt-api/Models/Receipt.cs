using System.Collections.Generic;

namespace receipt_api
{
    public class Receipt
    {
        public string Language { get; set; }

        public string ABN { get; set; }

        public string Currency { get; set; }

        public string Businessname { get; set; }

        public string ReceiptDate { get; set; }

        public string ReceiptTotal { get; set; }

        public string Text { get; set; }

        public List<string> Lines { get; set; }
    }
}
