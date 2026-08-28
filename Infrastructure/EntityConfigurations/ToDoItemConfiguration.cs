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
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.ToDoItems)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.IsCompleted);
        }
    }
}
