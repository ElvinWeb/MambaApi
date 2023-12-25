using MambaApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MambaApi.Configurations
{
    public class ProfessionConfiguration : IEntityTypeConfiguration<Profession>
    {
        public void Configure(EntityTypeBuilder<Profession> builder)
        {
            builder.Property(profession => profession.Name).IsRequired().HasMaxLength(50);
        }
    }
}
