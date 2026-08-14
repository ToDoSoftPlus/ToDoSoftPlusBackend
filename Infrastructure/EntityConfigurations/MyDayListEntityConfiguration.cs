using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    public class MyDayListEntityConfiguration : IEntityTypeConfiguration<MyDayListEntity>
    {
        public void Configure(EntityTypeBuilder<MyDayListEntity> builder)
        {
            builder.HasKey(m => new { m.UserId, m.ToDoItemId, m.Date });

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.MyDayList)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(x => x.ToDoItem)
                .WithMany(x => x.MyDayLists)
                .HasForeignKey(x => x.ToDoItemId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
