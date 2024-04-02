using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHP.entity.FHP;
using FHP.utilities;

namespace FHP.datalayer.EntityConfiguration.FHP
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("Contract"); // Table name in the database

            builder.HasKey(x => x.Id); // Primary key definition
            builder.HasQueryFilter(x => x.Status != Constants.RecordStatus.Deleted);

            // Define property configurations
            builder.Property(x => x.Id).ValueGeneratedOnAdd(); // Auto-generated ID
           
            builder.Property(x => x.EmployeeId).IsRequired(); 
            builder.Property(x => x.JobId).IsRequired(); 
            builder.Property(x => x.EmployerId).IsRequired();
            builder.Property(x => x.Duration).IsRequired(); 
            builder.Property(x => x.Description); 
            builder.Property(x => x.EmployeeSignature); 
            builder.Property(x => x.EmployerSignature);
            builder.Property(x => x.StartContract).IsRequired(false); 
            builder.Property(x => x.RequestToChangeContract).IsRequired(false); 
            builder.Property(x => x.IsRequestToChangeAccepted).IsRequired(); 
            builder.Property(x => x.IsSignedByEmployee).IsRequired(); 
            builder.Property(x => x.IsSignedByEmployer).IsRequired();
            
            builder.Property(x => x.CreatedOn).IsRequired(); 
            builder.Property(x => x.UpdatedOn).IsRequired(false);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Title).IsRequired(false);

        }
    }

}
