using Microsoft.EntityFrameworkCore;
using Storage.Entities;

namespace Storage
{
    public class StorageContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardDeck>()
                .HasIndex(c => c.Name)
                .IsUnique();
            modelBuilder.Entity<Card>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=wt;Database=postgres;";
            optionsBuilder.UseNpgsql(connectionString)
                .UseLazyLoadingProxies();
        }
    }
}