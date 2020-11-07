using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BeanlancerAPI.Services;
using BeanlancerAPI2.Models;
using BeanlancerAPI2.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;

namespace BeanlancerAPI2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //FirebaseApp.Create(new AppOptions()
            //{
            //    Credential = GoogleCredential.GetApplicationDefault(),
            //});
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            string securityKey = "this_is_a_super_long_and_hard_Key_And_I_Am_Super_man$smesk.in";
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                      //.AddJwtBearer(options =>
                      //{
                      //    options.Authority = "https://securetoken.google.com/beanlancer-auth";

                      //    options.TokenValidationParameters = new TokenValidationParameters
                      //    {
                      //        // What to validate.
                      //        ValidateIssuer = true,
                      //        ValidateAudience = true,
                      //        ValidAudience = "beanlancer-auth",
                      //        // Setup validate data
                      //        ValidIssuer = "https://securetoken.google.com/beanlancer-auth",
                      //        ValidateLifetime = true
                      //    };


                      //}
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("thisisissssssss")),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "https://securetoken.google.com/beanlancer-auth",
                    ValidAudience = "https://securetoken.google.com/beanlancer-auth"
                };
            });
            services.AddMemoryCache();
            //services.AddDistributedMemoryCache();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSingleton<IConnectionMultiplexer>(provider =>
                ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RedisConnection")));
            //   services.AddStackExchangeRedisCache(options => options.Configuration = this.Configuration.GetConnectionString("RedisConnection"));
            services.AddSwaggerGen(c => {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer iJIUzI1NiIsInR5cCI6IkpXVCGlzIElzc2'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            services.AddDbContext<BeanlancersContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("BeanlancersContext")));
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWalletService,WalletSerice>();
            services.AddScoped<IRequestHistoryService,RequestHistoryService>();
            services.AddScoped<IRequestSkillService,RequestSkillService>();
            services.AddScoped<IAppliesService,AppliesService>();
            services.AddScoped<IDesignerService,DesignerService>();
            services.AddScoped<ISkillService,SkillService>();
            services.AddScoped<ISkillOfDesignerService, SkillOfDesignerService>();
            services.AddScoped<IRoleService,RoleService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ICateOfDesignerService, CateOfDesignerService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IResponseCacheService, ResponseCacheService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger(c => {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Beanlancer API");
                c.RoutePrefix = string.Empty;
             
            });

            app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }

    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            bool hasAuth = (context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
                || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any())
                && !context.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();

            if (hasAuth)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }
                        ] = new string[]{ }
                    }
                };
            }
        }
    }
}
