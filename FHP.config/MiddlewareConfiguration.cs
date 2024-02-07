using FHP.datalayer;
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
           
        }

        public static void ConfigureRepository(IServiceCollection services)
        {
          
        }

        public static void ConfigureServices(IServiceCollection services)
        {
           
        }

        
    }
}
