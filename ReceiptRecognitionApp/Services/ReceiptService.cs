using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReceiptRecognitionApp.Entities;
using ReceiptRecognitionApp.Services.Interfaces;

namespace ReceiptRecognitionApp.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly DbContext _context;

        public ReceiptService(DbContext context)
        {
            _context = context;
        }
        
        public void Add(Receipt receipt)
        {
            _context.Set<Receipt>().Add(receipt);
            _context.SaveChanges();
        }
        public List<Receipt> GetAll()
        {
            return _context.Set<Receipt>().Include(t => t.ReceiptImage).ToList();
        }

        public Receipt FirstOrDefault(int receiptImageId)
        {
            return _context.Set<Receipt>().FirstOrDefault(x => x.ReceiptImageId == receiptImageId);
        }
    }
}
