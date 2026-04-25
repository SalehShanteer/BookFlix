using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(a => a.ID);
            builder.HasIndex(a => a.Name)
                .IsUnique();

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasData(SeedData.LoadRolesData());
        }
    }
}
