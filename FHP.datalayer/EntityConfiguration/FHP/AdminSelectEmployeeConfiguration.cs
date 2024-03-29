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
    public class AdminSelectEmployeeConfiguration : IEntityTypeConfiguration<AdminSelectEmployee>
    {
        public void Configure(EntityTypeBuilder<AdminSelectEmployee> builder)
        {
            builder.ToTable("AdminSelectEmployee"); // Table name in the database

            builder.HasKey(x => x.Id); // Primary key definition


            // Define property configurations
            builder.Property(x => x.Id).ValueGeneratedOnAdd(); // Auto-generated ID
           
            builder.Property(x => x.JobId).IsRequired(); 
            builder.Property(x => x.EmployeeId).IsRequired(); 
            builder.Property(x => x.InProbationCancel).IsRequired(); 
            builder.Property(x => x.IsSelected).IsRequired(); 
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.Status).IsRequired();
        }
    }

}
