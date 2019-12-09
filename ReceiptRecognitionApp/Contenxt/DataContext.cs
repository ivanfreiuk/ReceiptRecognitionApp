using Microsoft.EntityFrameworkCore;
using ReceiptRecognitionApp.Entities;

namespace ReceiptRecognitionApp.Contenxt
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);           
        }

        public DbSet<Receipt> Receipts { get; set; }
    }
}
