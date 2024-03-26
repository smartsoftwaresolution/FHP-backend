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
    public class EmployerDetailConfiguration : IEntityTypeConfiguration<EmployerDetail>
    {
        public void Configure(EntityTypeBuilder<EmployerDetail> builder)
        {
            builder.ToTable("EmployerDetail"); // Table name in the database

            builder.HasKey(x => x.Id); // Primary key definition

            builder.HasQueryFilter(x => x.Status != Constants.RecordStatus.Deleted);


            // Define property configurations
            builder.Property(x => x.Id).ValueGeneratedOnAdd(); // Auto-generated ID
           
            builder.Property(x => x.UserId).IsRequired(); 
            builder.Property(x => x.NationalAddress).IsRequired(); 
            builder.Property(x => x.CertificateRegistrationURL).IsRequired(); 
            builder.Property(x => x.VATCertificateURL).IsRequired(); 
            builder.Property(x => x.ContactId).IsRequired(); 
            builder.Property(x => x.CityId).IsRequired(); 
            builder.Property(x => x.CountryId).IsRequired(); 
            builder.Property(x => x.StateId).IsRequired(); 
            builder.Property(x => x.CompanyLogoURL).IsRequired();
            builder.Property(x => x.Telephone).IsRequired(); 
            builder.Property(x => x.Fax).IsRequired(); 
            builder.Property(x => x.TypeOfBusiness).IsRequired(); 
            builder.Property(x => x.PrincipalBusinessActivity).IsRequired(); 
            builder.Property(x => x.WebAddress).IsRequired();
            builder.Property(x => x.PersonToContact).IsRequired(false); 
            
            builder.Property(x => x.CreatedOn).IsRequired(); 
            builder.Property(x => x.UpdatedOn).IsRequired(false); 
            builder.Property(x => x.Status).IsRequired();

           /* builder.HasOne(x => x.Country).WithMany().HasForeignKey(x => x.CountryId);
            builder.HasOne(x => x.State).WithMany().HasForeignKey(x => x.StateIds);*/
            builder.HasOne(x => x.City).WithMany().HasForeignKey(x => x.CityId);

        }
    }

}
