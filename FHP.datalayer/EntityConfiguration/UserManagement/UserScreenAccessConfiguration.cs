using FHP.entity.UserManagement;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHP.utilities;

namespace FHP.datalayer.EntityConfiguration.UserManagement
{
    public class UserScreenAccessConfiguration : IEntityTypeConfiguration<UserScreenAccess>
    {
        public void Configure(EntityTypeBuilder<UserScreenAccess> builder)
        {
            builder.ToTable("UserScreenAccess");

            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x => x.Status != Constants.RecordStatus.Deleted);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.RoleId).IsRequired();
            builder.Property(x => x.ScreenId).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.HasOne(x => x.Screen).WithMany().HasForeignKey(x => x.ScreenId);
            builder.HasOne(x => x.UserRole).WithMany().HasForeignKey(x => x.RoleId);
        }
    }
}
