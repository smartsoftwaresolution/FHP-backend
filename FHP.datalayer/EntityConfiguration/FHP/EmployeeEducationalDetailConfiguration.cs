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
    public class EmployeeEducationalDetailConfiguration : IEntityTypeConfiguration<EmployeeEducationalDetail>
    {
        public void Configure(EntityTypeBuilder<EmployeeEducationalDetail> builder)
        {
            builder.ToTable("EmployeeEducationalDetail"); // Table name in the database

            builder.HasKey(x => x.Id); // Primary key definition

            // Define property configurations
            builder.Property(x => x.Id).ValueGeneratedOnAdd(); // Auto-generated ID
            
            builder.Property(x => x.UserId).IsRequired(); 
            builder.Property(x => x.Education).IsRequired(); 
            builder.Property(x => x.NameOfBoardOrUniversity).IsRequired(); 
            builder.Property(x => x.YearOfCompletion).IsRequired(); 
            builder.Property(x => x.MarksObtained).IsRequired(false); 
            builder.Property(x => x.GPA).IsRequired(); 
            
            builder.Property(x => x.CreatedOn).IsRequired(); 
            builder.Property(x => x.UpdatedOn).IsRequired(false); 
            builder.Property(x => x.Status).IsRequired();
        }
    }

}
