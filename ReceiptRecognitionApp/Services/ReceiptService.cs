﻿using Microsoft.EntityFrameworkCore;
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
    }
}
