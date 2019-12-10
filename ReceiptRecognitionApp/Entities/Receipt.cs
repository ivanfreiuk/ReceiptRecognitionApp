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

        [JsonIgnore]
        [IgnoreDataMember]
        public ReceiptImage ReceiptImage { get; set; }
    }
}
