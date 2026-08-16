using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class ToDoSubItemConfiguration : IEntityTypeConfiguration<ToDoSubItemEntity>
    {
        public void Configure(EntityTypeBuilder<ToDoSubItemEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.ToDoItem)
                .WithMany(x => x.SubToDoItems)
                .HasForeignKey(x => x.ToDoItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ToDoItemId);
        }
    }
}
