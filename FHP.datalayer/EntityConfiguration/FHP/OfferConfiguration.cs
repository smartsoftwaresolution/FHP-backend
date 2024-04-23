using FHP.entity.FHP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FHP.datalayer.EntityConfiguration.FHP
{
    public class OfferConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.ToTable("Offer"); // Table name in the database.

            builder.HasKey(x => x.Id); //primary key defination.


            // Define property configurations
            builder.Property(x => x.Id).ValueGeneratedOnAdd(); //auto generated Id.

            builder.Property(x => x.JobId).IsRequired();
            builder.Property(x => x.EmployerId).IsRequired();
            builder.Property(x => x.EmployerId).IsRequired();
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Status).IsRequired();
          
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired(false);

            builder.Property(x => x.IsAvaliable).IsRequired();
            builder.Property(x => x.CancelReason).IsRequired(false);

        }
    }
}
