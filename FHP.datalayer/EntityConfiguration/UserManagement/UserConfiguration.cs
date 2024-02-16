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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CompanyId).IsRequired();
            builder.Property(x=>x.RoleId).IsRequired();
            builder.Property(x => x.GovernmentId).IsRequired(false);
            builder.Property(x=>x.FullName).IsRequired();
            builder.Property(x=>x.Address).IsRequired(false);
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x=>x.MobileNumber).IsRequired();
            builder.Property(x=>x.Status).IsRequired();
            builder.Property(x=>x.CreatedOn).IsRequired();
            builder.Property(x => x.LastLogInTime).IsRequired(false);
            builder.Property(x=>x.LastLogOutTime).IsRequired(false);
            builder.Property(x => x.UpdatedOn).IsRequired(false);
        }
    }
}
