using Backend.Data;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Connect to PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Register your Service (Crucial for the Controller to work)
builder.Services.AddScoped<RequirementService>(); 

builder.Services.AddControllers();

var app = builder.Build();

// 3. Map the controllers so your URLs work
app.MapControllers(); 

app.Run();