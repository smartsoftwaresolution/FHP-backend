
using FHP.datalayer.EntityConfiguration.UserManagement;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FHP.entity.UserManagement;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
        }

    }
}
