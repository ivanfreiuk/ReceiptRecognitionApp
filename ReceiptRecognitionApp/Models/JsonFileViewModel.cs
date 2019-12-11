using ReceiptRecognitionApp.Entities;

namespace ReceiptRecognitionApp.Models
{
    public class JsonFileViewModel
    {
        public ReceiptImage ReceiptImage { get; set; }
        public string Json { get; set; }
        public Receipt Receipt { get; set; }
    }
}
