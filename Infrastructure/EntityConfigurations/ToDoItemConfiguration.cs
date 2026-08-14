using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class ToDoItemConfiguration : IEntityTypeConfiguration<ToDoItemEntity>
    {
        public void Configure(EntityTypeBuilder<ToDoItemEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(x => x.ToDoList)
                .WithMany(x => x.ToDoItemsList)
                .HasForeignKey(x => x.ToDoListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.ParentToDoItem)
                .WithMany(x => x.SubTasks)
                .HasForeignKey(x => x.ParentToDoItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.ToDoListId);
            builder.HasIndex(x => x.ParentToDoItemId);
            builder.HasIndex(x => x.IsCompleted);
        }
    }
}
