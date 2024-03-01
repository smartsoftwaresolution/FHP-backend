using FHP.entity.FHP;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.EntityConfiguration.FHP
{
    public class EmployerContractConfirmationConfiguration : IEntityTypeConfiguration<EmployerContractConfirmation>
    {
        public void Configure(EntityTypeBuilder<EmployerContractConfirmation> builder)
        {
            // Set table name
            builder.ToTable("EmployerContractConfirmation");

            // Set primary key
            builder.HasKey(ecc => ecc.Id);
            builder.Property(ea => ea.Id).ValueGeneratedOnAdd(); // Auto-generated ID

            // Configure properties
            builder.Property(ecc => ecc.EmployeeId).IsRequired();
            builder.Property(ecc => ecc.JobId).IsRequired();
            builder.Property(ecc => ecc.EmployerId).IsRequired();
            builder.Property(ecc => ecc.IsSelected).IsRequired();
            builder.Property(ecc => ecc.Status).IsRequired();
            builder.Property(ecc => ecc.CreatedOn).IsRequired();
        }
    }
}
