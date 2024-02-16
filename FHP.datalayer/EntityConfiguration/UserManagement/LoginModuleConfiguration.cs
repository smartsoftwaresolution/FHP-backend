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
    public class LoginModuleConfiguration : IEntityTypeConfiguration<LoginModule>
    {
        public void Configure(EntityTypeBuilder<LoginModule> builder)
        {
            builder.ToTable("LoginModule");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.RoleId).IsRequired(false);
            builder.Property(x => x.CompanyId).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
        }
    }
}
