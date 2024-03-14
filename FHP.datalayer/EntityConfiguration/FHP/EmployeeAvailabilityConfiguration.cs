using FHP.entity.FHP;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHP.utilities;

namespace FHP.datalayer.EntityConfiguration.FHP
{
    public class EmployeeAvailabilityConfiguration : IEntityTypeConfiguration<EmployeeAvailability>
    {
        public void Configure(EntityTypeBuilder<EmployeeAvailability> builder)
        {
            // Set table name
            builder.ToTable("EmployeeAvailability");

            // Set primary key
            builder.HasKey(ea => ea.Id);

            builder.HasQueryFilter(x => x.Status != Constants.RecordStatus.Deleted);

            builder.Property(ea => ea.Id).ValueGeneratedOnAdd(); // Auto-generated ID

            // Configure properties
            builder.Property(ea => ea.UserId).IsRequired();
            builder.Property(ea => ea.EmployeeId).IsRequired();
            builder.Property(ea => ea.JobId).IsRequired();
            builder.Property(ea => ea.IsAvailable).IsRequired();
            builder.Property(ea => ea.Status).IsRequired();
            builder.Property(ea => ea.CreatedOn).IsRequired();
            builder.Property(ea => ea.AdminJobTitle).IsRequired(false);
            builder.Property(ea => ea.AdminJobDescription).IsRequired(false);
            builder.Property(ea => ea.CancelReasons).IsRequired(false);
            builder.Property(ea => ea.UpdatedOn).IsRequired(false);
        }
    }
}
