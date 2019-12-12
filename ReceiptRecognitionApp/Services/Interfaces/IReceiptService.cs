using ReceiptRecognitionApp.Entities;
using System.Collections.Generic;

namespace ReceiptRecognitionApp.Services.Interfaces
{
    public interface IReceiptService
    {
        void Add(Receipt receipt);
        Receipt FirstOrDefault(int receiptImageId);
        List<Receipt> GetAll();
    }
}
