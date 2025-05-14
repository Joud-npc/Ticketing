namespace Ticketing.Data;

using Ticketing.Models;
using Microsoft.EntityFrameworkCore;
using System;

public class AppDbContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Utilisateur> Utilisateur { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            try
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost;Database=Ticket;Integrated Security=True;TrustServerCertificate=True;");
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error configuring DbContext: {ex.Message}");
                throw;
            }
        }
    }
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration pour la table Ticket
        modelBuilder.Entity<Ticket>()
            .HasKey(t => t.Id_Ticket);
            
        // Configuration pour la table Utilisateur
        modelBuilder.Entity<Utilisateur>()
            .HasKey(u => u.Id_Utilisateur);
            
        modelBuilder.Entity<Utilisateur>()
            .Property(u => u.Email)
            .IsRequired();
            
        modelBuilder.Entity<Utilisateur>()
            .Property(u => u.MDP)
            .IsRequired();
            
        modelBuilder.Entity<Utilisateur>()
            .Property(u => u.Rol)
            .IsRequired();
            
        // Vérifier que l'email est unique
        modelBuilder.Entity<Utilisateur>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}