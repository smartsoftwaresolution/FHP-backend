using FHP.entity.FHP;
using FHP.utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.EntityConfiguration.FHP
{
    public class EmployeeSkillDetailsConfiguration : IEntityTypeConfiguration<EmployeeSkillDetail>
    {
        public void Configure(EntityTypeBuilder<EmployeeSkillDetail> builder)
        {
            builder.ToTable("EmployeeSkillDetail"); // Table name in the database

            builder.HasKey(x => x.Id); // Primary key definition

            builder.HasQueryFilter(x => x.Status != Constants.RecordStatus.Deleted);


            // Define property configurations
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.SkillId).IsRequired();
           
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);
            builder.Property(x => x.Status).IsRequired();


            builder.HasOne(x => x.User)
            .WithMany(c => c.SkillDetails)
            .HasForeignKey(cf => cf.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
