using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReceiptRecognitionApp.Entities;
using ReceiptRecognitionApp.Services.Interfaces;

namespace ReceiptRecognitionApp.Services
{
    public class ReceiptImageService : IReceiptImageService
    {
        private readonly DbContext _context;

        public ReceiptImageService(DbContext context)
        {
            _context = context;
        }

        public List<ReceiptImage> GetAll()
        {
            return _context.Set<ReceiptImage>().ToList();
        }

        public void Add(ReceiptImage receiptImage)
        {
            _context.Set<ReceiptImage>().Add(receiptImage);
            _context.SaveChanges();
        }
    }
}
