using Microsoft.EntityFrameworkCore;
using Prestaciones.Application.Services;
using Prestaciones.Domain.Repositories;
using Prestaciones.Infrastructure.Data;
using Prestaciones.Infrastructure.Repositories;
using Prestaciones.Api.Services;
using Microsoft.Extensions.FileProviders;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddServiceDefaults();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configurar Entity Framework Core
builder.Services.AddDbContext<PrestacionesDbContext>((serviceProvider, options) =>
{
    var connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("prestaciones");
    options.UseNpgsql(connectionString);
});

// Registrar servicios
builder.Services.AddScoped<IReclamacionPreviaRepository, ReclamacionPreviaRepository>();
builder.Services.AddScoped<ReclamacionPreviaService>();
builder.Services.AddScoped<AlmacenamientoArchivosService>();

var app = builder.Build();

// Aplicar migraciones en desarrollo
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<PrestacionesDbContext>();
        db.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Archivos")),
    RequestPath = "/archivos"
});

app.UseCors();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
