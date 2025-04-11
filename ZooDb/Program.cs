using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true; // Pretty JSON output 
    
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseRouting();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
