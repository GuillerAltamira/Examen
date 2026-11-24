using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Tutor> Tutores => Set<Tutor>();
        public DbSet<Tesis> Tesis => Set<Tesis>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tutor>()
                .HasIndex(t => t.Correo)
                .IsUnique();

            modelBuilder.Entity<Tesis>()
                .HasIndex(t => t.Titulo)
                .IsUnique();

            modelBuilder.Entity<Tesis>()
                .HasOne(t => t.Tutor)
                .WithMany()
                .HasForeignKey(t => t.IdTutor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NombreUsuario)
                .IsUnique();
        }
    }
}