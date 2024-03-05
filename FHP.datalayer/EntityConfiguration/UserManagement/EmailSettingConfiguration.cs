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
    public class EmailSettingConfiguration : IEntityTypeConfiguration<EmailSetting>
    {
        public void Configure(EntityTypeBuilder<EmailSetting> builder)
        {
            builder.ToTable("EmailSetting");

            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x => x.Status != Constants.RecordStatus.Deleted);


            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CompanyId).IsRequired();
            builder.Property(x=>x.Email).IsRequired();
            builder.Property(x=>x.Password).IsRequired();
            builder.Property(x=>x.AppPassword).IsRequired();
            builder.Property(x=>x.IMapHost).IsRequired();
            builder.Property(x=>x.IMapPort).IsRequired();
            builder.Property(x=>x.SmtpHost).IsRequired();
            builder.Property(x=>x.SmtpPort).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x=>x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);    
        }
    }
}
