using Amazon.S3;
using Identity.BLL.Abstractions.Externals;
using Identity.BLL.Abstractions.Internals.Repositories;
using Identity.DAL.Data;
using Identity.DAL.Externals.Caches;
using Identity.DAL.Implementations.Externals.Emails;
using Identity.DAL.Implementations.Externals.Files;
using Identity.DAL.Internals;
using Identity.DTO.Accounts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Identity.DAL.ServiceRegistration
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddDALServices(this IServiceCollection services,ConfigurationManager configuration)
        {
            //---------------------------Internals---------------------------------------------------------------------
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserVerificationRepository, UserVerificationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //-----------------------------External services-------------------------------------------------------

            services.AddScoped<IFileService, AmazonS3Service>();
            services.AddScoped<IEmailService, SmtpEmailService>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:ConnectionString"];
                options.InstanceName = "Identity";
            });
            services.AddScoped<ICacheService, RedisService>();
            var jwtSection = configuration.GetRequiredSection("JwtSettings");
            var jwtSettings = jwtSection.Get<JwtSetting>()
                ?? throw new InvalidOperationException("JwtSettings is missing.");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateIssuerSigningKey = true,
                       ValidateLifetime = true,

                       ValidIssuer = jwtSettings.Issuer,
                       ValidAudience = jwtSettings.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                       ClockSkew = TimeSpan.Zero,
                   });
            services.AddSingleton<IAmazonS3>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var accessKey = config["AWS:AccessKey"];
                var secretKey = config["AWS:SecretKey"];
                var region = config["AWS:Region"];

                var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
                var awsConfig = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
                };
                return new AmazonS3Client(awsCredentials, awsConfig);
            });
            return services;
        }
    }
}
