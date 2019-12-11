using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ReceiptRecognitionApp.Models
{
    public class ReceiptResponseModel
    {
        public string Language { get; set; }

        public string ABN { get; set; }

        public string Currency { get; set; }

        public string Businessname { get; set; }

        public string ReceiptDate { get; set; }

        public string ReceiptTotal { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public string Text { get; set; }

        public List<string> Lines { get; set; }
    }
}
