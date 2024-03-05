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
    public class EmployeeProfessionalDetailConfiguration : IEntityTypeConfiguration<EmployeeProfessionalDetail>
    {
        public void Configure(EntityTypeBuilder<EmployeeProfessionalDetail> builder)
        {
            builder.ToTable("EmployeeProfessionalDetail"); // Table name in the database

            builder.HasKey(x => x.Id); // Primary key definition

            builder.HasQueryFilter(x => x.Status != Constants.RecordStatus.Deleted);


            // Define property configurations
            builder.Property(x => x.Id).ValueGeneratedOnAdd(); // Auto-generated ID
           
            builder.Property(x => x.UserId).IsRequired(); 
            builder.Property(x => x.JobDescription).IsRequired(); 
            builder.Property(x => x.StartDate).IsRequired(); 
            builder.Property(x => x.EndDate).IsRequired(); 
            builder.Property(x => x.CompanyName).IsRequired(); 
            builder.Property(x => x.CompanyLocation).IsRequired(); 
            builder.Property(x => x.Designation).IsRequired(); 
            builder.Property(x => x.EmploymentStatus).IsRequired(); 
            builder.Property(x => x.YearsOfExperience).IsRequired(); 
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);
            builder.Property(x => x.Status).IsRequired();
        }
    }

}
