using FHP.datalayer;
using FHP.datalayer.Repository.FHP;
using FHP.datalayer.Repository.UserManagement;
using FHP.infrastructure.Manager.FHP;
using FHP.infrastructure.Manager.UserManagement;
using FHP.infrastructure.Repository.FHP;
using FHP.infrastructure.Repository.UserManagement;
using FHP.infrastructure.Service;
using FHP.manager.FHP;
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
            services.AddScoped<ICountryManager, CountryManager>();
            services.AddScoped<IStateManager, StateManager>();
            services.AddScoped<ICityManager, CityManager>();
            services.AddScoped<IEmployeeDetailManager, EmployeeDetailManager>();
            services.AddScoped<ISkillsDetailManager, SkillsDetailManager>();
            services.AddScoped<IEmployeeEducationalDetailManager, EmployeeEducationalDetailManager>();
            services.AddScoped<IEmployeeProfessionalDetailManager, EmployeeProfessionalDetailManager>();
            services.AddScoped<IEmployerDetailManager,EmployerDetailManager>();
            services.AddScoped<IEmployerDetailManager, EmployerDetailManager>();
            services.AddScoped<IJobPostingManager, JobPostingManager>();
            services.AddScoped<IContractManager, ContractManager>();
            services.AddScoped<IAdminSelectEmployeeManager, AdminSelectEmployeeManager>();
            services.AddScoped<IEmployeeSkillDetailManager, EmployeeSkillDetailManager>();
            services.AddScoped<IEmployeeAvailabilityManager, EmployeeAvailabilityManager>();
            services.AddScoped<IEmployerContractConfirmationManager,EmployerContractConfirmationManager>(); 
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
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IStateRepository,StateRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IEmployeeDetailRepository,EmployeeDetailRepository>();
            services.AddScoped<ISkillsDetailRepository,SkillsDetailsRepository>();  
            services.AddScoped<IEmployeeEducationalDetailRepository, EmployeeEducationalDetailRepository>();
            services.AddScoped<IEmployeeProfessionalDetailRepository, EmployeeProfessionalDetailRepository>();
            services.AddScoped<IEmployerDetailRepository,EmployerDetailRepository>();
            services.AddScoped<IJobPostingRepoitory, JobPostingRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IAdminSelectEmployeeRepository, AdminSelectEmployeeRepository>();   
            services.AddScoped<IEmployeeSkillDetailRepository, EmployeeSkillDetailRepository>();
            services.AddScoped<IEmployeeAvailabilityRepository, EmployeeAvailabilityRepository>();
            services.AddScoped<IEmployerContractConfirmationRepository, EmployerContractConfirmationRepository>();
        }


        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IExceptionHandleService, ExceptionHandleService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileUploadService, FileUploadService>();

        }

    }
}
