﻿using FHP.entity.FHP;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.datalayer.EntityConfiguration.FHP
{
    public class JobPostingConfiguration : IEntityTypeConfiguration<JobPosting>
    {
        public void Configure(EntityTypeBuilder<JobPosting> builder)
        {
            builder.ToTable("JobPosting"); // Table name in the database

            builder.HasKey(x => x.Id); // Primary key definition

            // Define property configurations
            builder.Property(x => x.Id).ValueGeneratedOnAdd(); // Auto-generated ID
           
            builder.Property(x => x.UserId).IsRequired(); 
            builder.Property(x => x.JobTitle).IsRequired(); 
            builder.Property(x => x.Description).IsRequired(); 
            builder.Property(x => x.Experience).IsRequired(); 
            builder.Property(x => x.RolesAndResponsibilities).IsRequired(); 
            builder.Property(x => x.ContractDuration).IsRequired(); 
            builder.Property(x => x.ContractStartTime).IsRequired(false); 
            builder.Property(x => x.Skills); 
            builder.Property(x => x.Address).IsRequired(false); 
            builder.Property(x => x.Payout).IsRequired(); 
            builder.Property(x => x.InProbationCancel).IsRequired(); 
            builder.Property(x => x.Status).IsRequired(); 
            builder.Property(x => x.CreatedOn).IsRequired(); 
            builder.Property(x => x.UpdatedOn).IsRequired(false);
            builder.Property(x => x.JobStatus).IsRequired();
            builder.Property(x => x.CancelReason).IsRequired();
            builder.Property(x => x.JobProcessingStatus).IsRequired();
        }
    }

}
