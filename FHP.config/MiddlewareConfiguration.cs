using FHP.datalayer;
using FHP.datalayer.Repository.UserManagement;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.UserManagement;
using FHP.infrastructure.Service;
using FHP.manager.UserManagement;
using FHP.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHP.config
{
    public class MiddlewareConfiguration
    {
        public static void ConfigureEf(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);
        }

        public static void ConfigureUow(IServiceCollection services)
        {

        }
        public static void ConfigureManager(IServiceCollection services)
        {
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IUserRoleManager, UserRoleManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IScreenManager, ScreenManager>();
            services.AddScoped<IEmailSettingManager, EmailSettingManager>();
            services.AddScoped<IPermissionManager, PermissionManager>();
            services.AddScoped<IUserScreenAccessManager, UserScreenAccessManager>();
        }

        public static void ConfigureRepository(IServiceCollection services)
        {
            services.AddScoped<ICompanyRepository, CompanyRepository>(); 
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IScreenRepository, ScreenRepository>();
            services.AddScoped<IEmailSettingRepository,EmailSettingRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IUserScreenAccessRepository, UserScreenAccessRepository>();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IExceptionHandleService, ExceptionHandleService>();

        }


    }
}
