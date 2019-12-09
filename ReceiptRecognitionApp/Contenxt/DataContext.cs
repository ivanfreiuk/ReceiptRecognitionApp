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

            modelBuilder.Entity<ReceiptImage>()
                .HasOne(x => x.Receipt)
                .WithOne(x => x.ReceiptImage)
                .HasForeignKey<Receipt>(x => x.ReceiptImageId);
        }

        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptImage> ReceiptImages { get; set; }
    }
}
