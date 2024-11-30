using Microsoft.EntityFrameworkCore;

using SmartMedCC_AndreSousa.Database;
using SmartMedCC_AndreSousa.Repositories;
using SmartMedCC_AndreSousa.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SmartMedCCDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SmartMedCCDb")));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IMedicationsRepository, MedicationsRepository>();
builder.Services.AddScoped<IMedicationsService, MedicationsService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SmartMedCCDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
