using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ReceiptRecognitionApp.Entities
{
    public class Receipt
    {
        public int Id { get; set; }

        public int ReceiptImageId { get; set; }

        public string Json { get; set; }

        public string ReceiptDate { get; set; }

        public string ReceiptTotal { get; set; }

        public string Text { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ReceiptImage ReceiptImage { get; set; }
    }
}
