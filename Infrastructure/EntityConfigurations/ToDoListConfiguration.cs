using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoListEntity>
    {
        public void Configure(EntityTypeBuilder<ToDoListEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.UserId);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.ToDoLists)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
