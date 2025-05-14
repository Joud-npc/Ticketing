namespace Ticketing.Data;

using Ticketing.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Utilisateur> Utilisateur { get; set; }
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=Ticket;Integrated Security=True;TrustServerCertificate=True;");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .HasKey(t => t.Id_Ticket);
        }
}
