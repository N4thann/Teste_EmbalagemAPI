using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SrManoelLoja.Data;
using SrManoelLoja.Filters;
using SrManoelLoja.Interfaces;
using SrManoelLoja.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddTransient<IEmbalagemService, EmbalagemService>();
builder.Services.AddScoped<ISalvarNoBanco, SalvarNoBanco>();

var app = builder.Build();

var applyMigrations = builder.Configuration.GetValue<bool>("APPLY_MIGRATIONS");

if (applyMigrations)
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
