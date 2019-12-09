using System.Collections.Generic;
using ReceiptRecognitionApp.Entities;

namespace ReceiptRecognitionApp.Services.Interfaces
{
    public interface IReceiptImageService
    {
        List<ReceiptImage> GetAll();
        void Add(ReceiptImage receiptImage);
    }
}
