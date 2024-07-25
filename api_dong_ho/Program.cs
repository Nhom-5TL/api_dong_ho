using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;
using api_dong_ho.Data;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<api_dong_hoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("api_dong_hoContext") ?? throw new InvalidOperationException("Connection string 'api_dong_hoContext' not found.")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure CORS
app.UseCors(builder =>
{
    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});

// Serve static files from wwwroot/media
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "/media"
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
