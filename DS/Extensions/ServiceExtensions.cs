using DS.Bll;
using DS.Bll.Context;
using DS.Bll.Interfaces;
using DS.Bll.Models;
using DS.Data;
using DS.Data.Repository.Interfaces;
using DS.Helper;
using DS.Helper.Interfaces;
using DS.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace DS.Extensions
{
    public static class ServiceExtensions
    {

        /// <summary>
        /// Dependency Injection Repository and UnitOfWork.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="Configuration">The configuration from settinfile.</param>
        public static void ConfigureRepository(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddEntityFrameworkSqlServer()
             .AddDbContext<DSDBContext>(options =>
              options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddTransient<IUnitOfWork, DSUnitOfWork>();
        }

        /// <summary>
        /// Dependency Injection Class Business Logic Layer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureBll(this IServiceCollection services)
        {
            services.AddScoped<IValueHelp, ValueHelpBll>();
            services.AddScoped<IAttachment, AttachmentBll>();
            services.AddScoped<ICa, CaBll>();
            services.AddScoped<IManageToken, ManageToken>();
            services.AddScoped<IBusinessPlace, BusinessPlaceBll>();
            services.AddScoped<ICompany, CompanyBll>();
            services.AddScoped<IEmployee, EmployeeBll>();
            services.AddScoped<IMenu, MenuBll>();
            services.AddScoped<ILogin, LoginBll>();
        }

        /// <summary>
        /// Dependency Injection Httpcontext.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Add Singleton Logger Class.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        /// <summary>
        /// Add Singleton ElasticSearch Class.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureElasticSearch(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IElasticSearch<>), typeof(ElasticSearch<>));
        }

        /// <summary>
        /// Add Swagger.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "Header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);

            });
        }

        /// <summary>
        /// Add Swagger.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        /// <summary>
        /// Add Middleware when request bein and end.
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<Middleware>();
        }

        /// <summary>
        /// Config handle message status code 403.
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureHandlerStatusPages(this IApplicationBuilder app)
        {
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 403 || context.HttpContext.Response.StatusCode == 401)
                {
                    string message = string.Empty;
                    switch (context.HttpContext.Response.StatusCode)
                    {
                        case 403:
                            message = "Not Permissino.";
                            break;
                        case 401:
                            message = "Unauthorized.";
                            break;
                    }
                    var model = new ValidationResultViewModel
                    {
                        ErrorFlag = true,
                        Message = message
                    };
                    string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    context.HttpContext.Response.ContentType = ConstantValue.CONTENT_TYPEJSON;
                    await context.HttpContext.Response.WriteAsync(json);
                }
            });
        }

        /// <summary>
        /// Add CORS Configuration.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        /// <summary>
        /// Add Policy Configuration.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigurePolicy(this IServiceCollection services)
        {
            //Add Policy
            var roleList = new List<string> { "CA_MA_Role","CA_DS_Role",
                                              "PV_MA_Role","PV_DS_Role",
                                              "BS_MA_Role","BS_DS_Role",
                                              "XX_MA_Role","XX_DS_Role"};

            foreach (var role in roleList)
            {
                //Add Policy
                services.AddAuthorization(options =>
                {
                    options.AddPolicy(role, policy => policy.Requirements.Add(new RoleRequirement(role)));
                });
            }

            services.AddSingleton<IAuthorizationHandler, RoleHandler>();

        }

        /// <summary>
        /// Add Jwt Authentication and Setting.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void ConfigureJwtAuthen(this IServiceCollection services, IConfiguration Configuration)
        {
            var option = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = System.TimeSpan.Zero,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = option;
                 options.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                         var model = new ValidationResultViewModel
                         {
                             ErrorFlag = true,
                             Message = "Unauthorized."
                         };
                         string json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                         {
                             ContractResolver = new CamelCasePropertyNamesContractResolver()
                         });
                         context.Response.OnStarting(async () =>
                         {
                             context.Response.ContentType = ConstantValue.CONTENT_TYPEJSON;
                             await context.Response.WriteAsync(json);
                         });
                         return System.Threading.Tasks.Task.CompletedTask;
                     },
                 };
             });
        }

    }
}
