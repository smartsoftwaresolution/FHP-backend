using FHP.entity.UserManagement;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.EntityConfiguration.UserManagement
{
    public class ScreenConfiguration : IEntityTypeConfiguration<Screen>
    {
        public void Configure(EntityTypeBuilder<Screen> builder)
        {
            builder.ToTable("Screen");

            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x => x.Status != Constants.RecordStatus.Deleted);


            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x=>x.ScreenName).IsRequired();
            builder.Property(x=>x.ScreenCode).IsRequired();
            builder.Property(x=>x.Status).IsRequired();
            builder.Property(x=>x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);
        }
    }
}
