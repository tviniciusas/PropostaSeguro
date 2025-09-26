using ContratacaoService.Application.Ports;
using ContratacaoService.Application.UseCases;
using ContratacaoService.Infrastructure.Configuration;
using ContratacaoService.Infrastructure.Data;
using ContratacaoService.Infrastructure.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Database=contratacoes_db;Username=postgres;Password=postgres";

var propostaServiceUrl = builder.Configuration.GetValue<string>("PropostaServiceUrl")
    ?? "http://localhost:5001";

builder.Services.AddDatabase(connectionString);

builder.Services.AddHttpClient<IPropostaServiceClient, PropostaServiceClient>(client =>
{
    client.BaseAddress = new Uri(propostaServiceUrl);
});

builder.Services.AddScoped<IContratacaoRepository>(provider => new ContratacaoRepository(connectionString));
builder.Services.AddScoped<CriarContratacaoUseCase>();
builder.Services.AddScoped<ListarContratacoesUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Services.MigrateDatabase();

app.Run();