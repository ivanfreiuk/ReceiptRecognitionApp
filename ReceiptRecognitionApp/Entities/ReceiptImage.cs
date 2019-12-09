namespace ReceiptRecognitionApp.Entities
{
    public class ReceiptImage
    {
        public int Id { get; set; }

        public string OriginalImageName { get; set; }

        public byte[] OriginalImage { get; set; }

        public string ScannedImageName { get; set; }

        public byte[] ScannedImage { get; set; }
    }
}
