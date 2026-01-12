using MiniApp.API.Middlewares;
using MiniApp.BLL.ServiceRegistration;
using MiniApp.DAL.ServiceRegistration;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception();
builder.Services.AddBLLServices();
builder.Services.AddDALServices(connectionString);
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
