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

        public int Add(ReceiptImage receiptImage)
        {
            var result = _context.Set<ReceiptImage>().Add(receiptImage);
            _context.SaveChanges();
            return result.Entity.Id;
        }

        public ReceiptImage Get(int id)
        {
            return _context.Set<ReceiptImage>().Find(id);
        }
    }
}
