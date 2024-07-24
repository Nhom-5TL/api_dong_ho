using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using api_dong_ho.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<api_dong_hoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("api_dong_hoContext") ?? throw new InvalidOperationException("Connection string 'api_dong_hoContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder =>
{
    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
