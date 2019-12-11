using ReceiptRecognitionApp.Entities;

namespace ReceiptRecognitionApp.Services.Interfaces
{
    public interface IReceiptService
    {
        void Add(Receipt receipt);
        Receipt FirstOrDefault(int receiptImageId);
    }
}
