using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServerAPI.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ServerAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ServerAPIContext") ?? throw new InvalidOperationException("Connection string 'ServerAPIContext' not found.")));

// Add services to the container.

// Tillåter alla att accessa maskinparken.
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

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

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
