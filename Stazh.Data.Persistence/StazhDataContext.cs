using Microsoft.EntityFrameworkCore;
using Stazh.Core.Data.Entities;

namespace Stazh.Data.EFCore
{
    public class StazhDataContext : DbContext
    {
        private readonly string _connectionString;

        public StazhDataContext()
        {
            //_connectionString = @"Server=.\SQLEXPRESS;Database=Stazh;Trusted_Connection=True;";
            _connectionString = @"Server=tcp:stazh.database.windows.net,1433;Initial Catalog=stazhdata;Persist Security Info=False;User ID=vmadmin;Password=SimpleStr0ng..;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        public StazhDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer(_connectionString);
            base.OnConfiguring(optionsBuilder);
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Item>()
                .HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentItemId);
            modelBuilder.Entity<Attachment>().HasIndex(att => att.UniqueAttachmentName).IsUnique(true);
        }
    }
}
