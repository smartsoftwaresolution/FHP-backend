
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using System.Text.Json.Serialization;
using FHP.datalayer;
using FHP.config;
using NLog.Config;
using NLog;

namespace FHP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           // LogManager.Configuration = new XmlLoggingConfiguration("nlog.config"); // Loading NLog configuration from a file only defined log level
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DataConnection"))
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));


            services.AddControllers()
                   .AddJsonOptions(options =>
                   {
                       options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                       options.JsonSerializerOptions.IgnoreNullValues = true;
                   });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            });

            services.AddHttpClient("AccountClient", c => //Named Http Client
            {
                c.DefaultRequestHeaders.Add("X-Custom-Env", "TEST");
            });

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllers(options => options.EnableEndpointRouting = false);
            services.AddControllersWithViews(options => options.EnableEndpointRouting = false);
            services.AddRazorPages().AddMvcOptions(options => options.EnableEndpointRouting = false);


            services.AddSwaggerGen(setup =>
            {
              //setup.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Main API v1.0", Version = "v1.0" });
              //services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); });
                setup.OperationFilter<CustomHeader>();
              //Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                     {
                         {
                             jwtSecurityScheme, Array.Empty<string>()
                         }
                });

            });




            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters

                    {
                        ValidIssuer = Configuration.GetValue<string>("Jwt:Issuer"),
                        ValidAudience = Configuration.GetValue<string>("Jwt:Audience"),
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>("Jwt:Secret"))),
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });

            services.AddSignalR((options) =>
            {
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                options.MaximumReceiveMessageSize = 102400000;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                    .WithOrigins("http://localhost:3000",
                    "http://localhost:3001",
                    "http://localhost:3002",
                    "http://localhost:3003",

                    "https://contactaholic.com/",

                        "https://contactaholic.com:5001",
                    "https://www.contactaholic.com",
                    "https://www.contactaholic.com",//with www
                    "https://www.contactaholic.com:5001",//with www,
                    "https://www.contactaholic.com:4100")//for email-client
                    .AllowCredentials();
                });
            });




            services.AddHttpContextAccessor();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });



            MiddlewareConfiguration.ConfigureEf(services, Configuration.GetConnectionString("DataConnection"));
            MiddlewareConfiguration.ConfigureUow(services);
            MiddlewareConfiguration.ConfigureManager(services);
            MiddlewareConfiguration.ConfigureRepository(services);
            MiddlewareConfiguration.ConfigureServices(services);



         //   services.AddSingleton<IImapService, ImapService>();

            /*services.AddTransient<IImapService>(provider =>
            {
                // Get the current HttpContext to access user-specific details
                var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
                var user = httpContextAccessor.HttpContext.User;

                // Get the user-specific IMAP configuration details
                var imapServer = GetUserImapServer(user); // Implement this method to get user-specific details
                var imapPort = GetUserImapPort(user);
                var email = GetUserEmail(user);
                var appPassword = GetUserAppPassword(user);

                // Create a new instance of ImapService with user-specific details
                return new ImapService(imapServer, imapPort, email, appPassword);
            });
*/
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage()
                .UseSqlServerStorage(Configuration.GetConnectionString("DataConnection"))
            );



            services.AddHangfireServer();

          //services.AddScoped<IEmailService, EmailService>();


        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
                  //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Versioned API v1.0");
                  //c.DocExpansion("none");
                    c.DocumentTitle = " ";
                    c.DocExpansion(DocExpansion.None);
                });
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });



            app.UseCors("CorsPolicy");
            app.UseRouting();
            /* app.UseCors(builder =>
              builder.WithOrigins("http://localhost:3000")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials());*/
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseHangfireDashboard();
            app.UseWebSockets();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
              //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Versioned API v1.0");
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
              //c.DocExpansion("none");
                c.DocumentTitle = "Title Documentation";
                c.DocExpansion(DocExpansion.None);
            });
            app.UseAuthentication();
          //app.UseSession();
            app.UseMvc();


            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Developed-By", "Your Name");
                await next.Invoke();
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
              //endpoints.MapHub<Controllers.ChatHub>(pattern: "/hubs/chat");     // path will look like this https://localhost:44379/chatsocket 
            });

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var headerName = "OnResultExecuting";
            context.HttpContext.Response.Headers.Add(
            headerName, new string[] { "ResultExecutingSuccessfully" });
          //_logger.LogInformation("Header added: {HeaderName}", headerName);
        }
    }
}
