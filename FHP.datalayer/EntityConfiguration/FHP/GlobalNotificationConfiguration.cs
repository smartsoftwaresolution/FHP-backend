using FHP.entity.FHP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.EntityConfiguration.FHP
{
    public class GlobalNotificationConfiguration : IEntityTypeConfiguration<GlobalNotification>
    {
        public void Configure(EntityTypeBuilder<GlobalNotification> builder)
        {
            builder.ToTable("GlobalNotification"); // Table name in the database.

            builder.HasKey(x => x.Id); //primary key defination.


            // Define property configurations
            builder.Property(x => x.Id).ValueGeneratedOnAdd(); //auto generated Id.

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired(false);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);
            builder.Property(x => x.IsRead).IsRequired();
        }
    }
}
