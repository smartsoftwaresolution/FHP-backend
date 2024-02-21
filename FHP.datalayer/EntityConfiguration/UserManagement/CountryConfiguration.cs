using FHP.entity.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.EntityConfiguration.UserManagement
{

    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Country");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CountryName).IsRequired();
            builder.Property(x=>x.Status).IsRequired();
            builder.Property(x=>x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);
        }
    }
}
