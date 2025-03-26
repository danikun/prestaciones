using Microsoft.EntityFrameworkCore;
using Prestaciones.Domain.Models;

namespace Prestaciones.Infrastructure.Data
{
    public class PrestacionesDbContext : DbContext
    {
        public PrestacionesDbContext(DbContextOptions<PrestacionesDbContext> options)
            : base(options)
        {
        }

        public DbSet<ReclamacionPrevia> ReclamacionesPrevias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReclamacionPrevia>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Estado)
                    .HasConversion<string>();
            });
        }
    }
} 