using Microsoft.OpenApi.Models;
using Persistence.Extensions;
using Services.Extensions;
using Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.Controllers.UserController).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Secret Book Club API", Version = "v1" });
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "Presentation.xml");
    c.IncludeXmlComments(filePath);
});

builder.Services.AddDataServices(builder.Configuration);
builder.Services.AddCoreServices();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

// I'm here in order for Program to be available in integration tests
public partial class Program;
