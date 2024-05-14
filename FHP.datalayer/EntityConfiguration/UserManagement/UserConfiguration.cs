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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x => x.Status != Constants.RecordStatus.Deleted);


            builder.Property(x => x.Id).ValueGeneratedOnAdd();
           
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.RoleId).IsRequired();
            builder.Property(x => x.GovernmentId).IsRequired(false);
            builder.Property(x => x.LastLogInTime).IsRequired(false);
            builder.Property(x => x.LastLogOutTime).IsRequired(false);

            builder.Property(x => x.ContactName).IsRequired(false);
            builder.Property(x => x.CompanyName).IsRequired(false);

            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);
            builder.Property(x => x.IsVerify).IsRequired(false);
            builder.Property(x => x.ProfileImg).IsRequired(false);
            builder.Property(x => x.MobileNumber).IsRequired();
            builder.Property(x => x.IsVerifyByAdmin).IsRequired(false);
            builder.Property(x => x.Otp).IsRequired(false);
            builder.Property(x => x.EmploymentType).IsRequired(false);
        }
    }
}
