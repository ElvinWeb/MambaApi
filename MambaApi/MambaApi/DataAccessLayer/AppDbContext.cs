using MambaApi.Configurations;
using MambaApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace MambaApi.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkerProfession> WorkerProfessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfessionConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}

