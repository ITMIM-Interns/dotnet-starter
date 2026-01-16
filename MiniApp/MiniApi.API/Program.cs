using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using MiniApp.API.Middlewares;
using MiniApp.BLL.ServiceRegistration;
using MiniApp.DAL.ServiceRegistration;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors.Select(e => new
            {
                Field = x.Key,
                Message = e.ErrorMessage
            }))
            .ToList();

        return new BadRequestObjectResult(errors);
    };
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception();
builder.Services.AddBLLServices();
builder.Services.AddDALServices(connectionString);
builder.Services.AddSingleton<IAmazonS3>(sp =>
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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalCustomExceptionHandler>();
app.MapControllers();

app.Run();
