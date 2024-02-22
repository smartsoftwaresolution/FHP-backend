using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHP.entity.FHP;

namespace FHP.datalayer.EntityConfiguration.FHP
{
    public class SkillsDetailConfiguration : IEntityTypeConfiguration<SkillsDetail>
    {
        public void Configure(EntityTypeBuilder<SkillsDetail> builder)
        {
            builder.ToTable("SkillsDetail");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.SkillName).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);
        }
    }
}
