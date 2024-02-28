using FHP.entity.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.EntityConfiguration.UserManagement
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permission");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CompanyId).IsRequired();
            builder.Property(x => x.Permissions).IsRequired();
            builder.Property(x => x.PermissionDescription).IsRequired();
            builder.Property(x => x.PermissionCode).IsRequired();
            builder.Property(x => x.ScreenCode).IsRequired();
            builder.Property(x => x.ScreenUrl).IsRequired();
            builder.Property(x => x.ScreenId).IsRequired();
            builder.Property(x => x.Status).IsRequired();
         
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);
            builder.Property(x => x.CreatedBy).IsRequired();


            builder.HasOne(x=> x.Screen).WithMany().HasForeignKey(x => x.ScreenId);
        }
    }
}
