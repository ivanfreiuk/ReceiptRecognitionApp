using System.Collections.Generic;
using ReceiptRecognitionApp.Entities;

namespace ReceiptRecognitionApp.Services.Interfaces
{
    public interface IReceiptImageService
    {
        List<ReceiptImage> GetAll();
        int Add(ReceiptImage receiptImage);
        ReceiptImage Get(int id);
    }
}
