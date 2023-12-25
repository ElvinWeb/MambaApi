using MambaApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MambaApi.Configurations
{
    public class WorkerProfessionConfiguration : IEntityTypeConfiguration<WorkerProfession>
    {
        public void Configure(EntityTypeBuilder<WorkerProfession> builder)
        {
            builder.HasOne(wp => wp.Profession).WithMany(wp => wp.WorkerProfessions).HasForeignKey(wp => wp.ProfessionId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(wp => wp.Worker).WithMany(wp => wp.WorkerProfessions).HasForeignKey(wp => wp.WorkerId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
