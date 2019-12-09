namespace ReceiptRecognitionApp.Entities
{
    public class Receipt
    {
        public int Id { get; set; }

        public int ReceiptImageId { get; set; }

        public ReceiptImage ReceiptImage { get; set; }
    }
}
