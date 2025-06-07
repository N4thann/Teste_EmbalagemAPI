using Microsoft.EntityFrameworkCore;
using SrManoelLoja.Data;
using SrManoelLoja.DTOs.Request;
using SrManoelLoja.DTOs.Response;
using SrManoelLoja.Filters; 
using SrManoelLoja.Interfaces;
using SrManoelLoja.Services;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddAuthorization();

builder.Logging.ClearProviders(); // Limpa os providers padrão
builder.Logging.AddConsole(); // Adiciona o console como provider de log
builder.Logging.AddDebug(); // Adiciona o debug output como provider de log

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapPost("LojaManoel/api/empacotar",
    async (PedidosRequest request, IEmbalagemService servicoEmbalagem, ISalvarNoBanco servicoSalvarBanco) =>
    {
        if (request == null || !request.Pedidos.Any())
        {
            return Results.BadRequest(new { message = "A requisição deve conter uma lista de pedidos válida" });
        }

        var embalagemResponse = await servicoEmbalagem.ProcessarPedidos(request);

        if (embalagemResponse == null)
        {
            return Results.BadRequest(new { message = "Erro no serviço de empacotamento" });
        }

        var salvarBancoResponse = await servicoSalvarBanco.CriarRegistrosNoBanco(request);

        if (salvarBancoResponse == null)
        {
            return Results.BadRequest(new { message = "Erro no serviço de salvar no banco" });
        }

        var response = new EmpacotarPedidosResponse(embalagemResponse, salvarBancoResponse);

        return Results.Ok(response);
    })
.WithName("EmpacotarPedidos")
.Produces<EmpacotarPedidosResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status500InternalServerError)
.AddEndpointFilter<ApiLoggingFilter>();

app.Run();