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
    public class EmployeeDetailConfiguration : IEntityTypeConfiguration<EmployeeDetail>
    {
        public void Configure(EntityTypeBuilder<EmployeeDetail> builder)
        {
            builder.ToTable("EmployeeDetail"); // Table name in the database

            builder.HasKey(x => x.Id); // Primary key definition

            // Define property configurations
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.UserId).IsRequired(); 
            builder.Property(x => x.MaritalStatus).IsRequired(); 
            builder.Property(x => x.Gender).IsRequired(); 
            builder.Property(x => x.DOB).IsRequired();
            builder.Property(x => x.CountryId).IsRequired(); 
            builder.Property(x => x.StateId).IsRequired(); 
            builder.Property(x => x.CityId).IsRequired(); 
            builder.Property(x => x.ResumeURL).IsRequired(false); 
            builder.Property(x => x.ProfileImgURL).IsRequired(false); 
            builder.Property(x => x.IsAvailable).IsRequired(); 
            builder.Property(x => x.Hobby).IsRequired(false); 
            builder.Property(x => x.PermanentAddress).IsRequired(); 
            builder.Property(x => x.AlternateAddress).IsRequired(false); 
            builder.Property(x => x.Mobile).IsRequired(); 
            builder.Property(x => x.Phone).IsRequired(false); 
            builder.Property(x => x.AlternatePhone).IsRequired(false);
            builder.Property(x => x.AlternateEmail).IsRequired(false);
            builder.Property(x => x.EmergencyContactName).IsRequired(false);
            builder.Property(x => x.EmergencyContactNumber).IsRequired(false); 
            builder.Property(x => x.CreatedOn).IsRequired(); 
            builder.Property(x => x.UpdatedOn).IsRequired(false); 
            builder.Property(x => x.Status).IsRequired(); 
        }
    }

}