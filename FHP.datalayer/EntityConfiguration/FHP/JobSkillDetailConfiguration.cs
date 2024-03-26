using FHP.entity.FHP;
using FHP.entity.UserManagement;
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
    public class JobSkillDetailConfiguration : IEntityTypeConfiguration<JobSkillDetail>
    {
        public void Configure(EntityTypeBuilder<JobSkillDetail> builder)
        {
            builder.ToTable("JobSkillDetail");

            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x => x.Status != Constants.RecordStatus.Deleted);


            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.JobId).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);
            builder.Property(x => x.SkillId).IsRequired();
            builder.HasOne(x => x.SkillDetail).WithMany().HasForeignKey(x => x.SkillId);

            builder.HasOne(x => x.JobPosting)
            .WithMany(c => c.JobSkillDetails)
            .HasForeignKey(cf => cf.JobId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
