
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
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("City");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CountryId).IsRequired();
            builder.Property(x=>x.StateId).IsRequired();
            builder.Property(x=>x.CityName).IsRequired();
            builder.Property(x=>x.Status).IsRequired();
            builder.Property(x=>x.CreatedOn).IsRequired();
            builder.Property(x=>x.UpdatedOn).IsRequired(false);
            builder.HasOne(x => x.Country).WithMany().HasForeignKey(x => x.CountryId);
            builder.HasOne(x => x.State).WithMany().HasForeignKey(x => x.StateId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
