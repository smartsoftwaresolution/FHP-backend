
using FHP.datalayer.EntityConfiguration.UserManagement;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHP.entity.UserManagement;
using FHP.entity.FHP;
using FHP.datalayer.EntityConfiguration.FHP;

namespace FHP.datalayer
{
    public class DataContext:IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
                
        }
        #region UserManagement
        public DbSet<Company> Companies { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Screen> Screen { get; set; }
        public DbSet<EmailSetting> EmailSetting { get; set; }
        public DbSet<LoginModule> LoginModule { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserScreenAccess> UserScreenAccess { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        #endregion

        #region FHP
        public DbSet<SkillsDetail> SkillsDetails { get; set; }
        public DbSet<EmployeeDetail> EmployeeDetails { get; set; }
        public DbSet<EmployerDetail> EmployerDetails { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<EmployeeEducationalDetail> EmployeeEducationalDetails { get; set; }
        public DbSet<EmployeeProfessionalDetail> EmployeeProfessionalDetails { get; set; }
        public DbSet<EmployeeSkillDetail> EmployeeSkillDetails { get; set; }
        public DbSet<AdminSelectEmployee> AdminSelectEmployees { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<EmployeeAvailability> EmployeeAvailabilities { get; set; }
        public DbSet<EmployerContractConfirmation> EmployerContractConfirmations { get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region UserManagement
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ScreenConfiguration());
            modelBuilder.ApplyConfiguration(new EmailSettingConfiguration());  
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserScreenAccessConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new StateConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
            #endregion

            #region FHP
           
            modelBuilder.ApplyConfiguration(new SkillsDetailConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeDetailConfiguration());
            modelBuilder.ApplyConfiguration(new EmployerDetailConfiguration());
            modelBuilder.ApplyConfiguration(new JobPostingConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeEducationalDetailConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeProfessionalDetailConfiguration());
            modelBuilder.ApplyConfiguration(new AdminSelectEmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new ContractConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeSkillDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeAvailabilityConfiguration());
            modelBuilder.ApplyConfiguration(new EmployerContractConfirmationConfiguration());

            #endregion
        }

    }
}
