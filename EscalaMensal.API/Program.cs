using EscalaMensal.Application.Services;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using EscalaMensal.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyFrontendPolicy", policy =>
    {
        // Agora podemos voltar para a versão segura, especificando a origem
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddScoped<IEscalaRepository, EscalaRepository>();
builder.Services.AddScoped<IRestricaoRepository, RestricaoRepository>();
builder.Services.AddScoped<IItemEscalaRepository, EscalaItemRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IFuncaoRepository, FuncaoRepository>();
builder.Services.AddScoped<IHistoricoEscalaRepository, HistoricoEscalaRepository>();
builder.Services.AddScoped<ICargoNivelFuncaoPermitidaRepository, CargoNivelFuncaoPermitidaRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRestricaoService, RestricaoService>();
builder.Services.AddScoped<IItemEscalaService, ItemEscalaService>();
builder.Services.AddScoped<IHistoricoEscalaService, HistoricoEscalaService>();
builder.Services.AddScoped<IFuncaoService, FuncaoService>(); // <-- A linha que estava faltando para este erro
builder.Services.AddScoped<IEscalaService, EscalaService>();
builder.Services.AddScoped<ICargoNivelFuncaoPermitidaService, CargoNivelFuncaoPermitidaService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var policyName = "corsblazor";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName, policy =>
    {
        policy.WithOrigins("https://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

app.UseCors(policyName);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowMyFrontendPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
