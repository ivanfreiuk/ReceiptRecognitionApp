namespace ReceiptRecognitionApp.Models
{
    public class DocumentScannResponse
    {
        public string FileExtension { get; set; }

        public byte[] ScannedFile { get; set; }
    }
}
