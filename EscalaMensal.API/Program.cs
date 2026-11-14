using EscalaMensal.Application.DTOs.Funcao;
using EscalaMensal.Application.Mappings;
using EscalaMensal.Application.Services;
using EscalaMensal.Application.Interfaces;
using EscalaMensal.Domain.Interfaces;
using EscalaMensal.Infrastructure.Context;
using EscalaMensal.Infrastructure.Repositories;
using EscalaMensal.Application.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddScoped<IEscalaRepository, EscalaRepository>();
builder.Services.AddScoped<IRestricaoRepository, RestricaoRepository>();
builder.Services.AddScoped<IItemMissaRepository, EscalaItemRepository>();
builder.Services.AddScoped<IMembroRepository, MembroRepository>();
builder.Services.AddScoped<IFuncaoRepository, FuncaoRepository>();
builder.Services.AddScoped<IHistoricoEscalaRepository, HistoricoEscalaRepository>();
builder.Services.AddScoped<ICargoNivelFuncaoPermitidaRepository, CargoNivelFuncaoPermitidaRepository>();
builder.Services.AddScoped<IConfiguracaoRepository, ConfiguracaoRepository>();
builder.Services.AddScoped<IMissasRepository, MissasRepository>();
builder.Services.AddScoped<IHorarioFixoRepository, HorarioFixoRepository>();
builder.Services.AddScoped<IMissaPadraoRepository, MissaPadraoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IParoquiaRepository, ParoquiaRepository>();

builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IMembroService, MembroService>();
builder.Services.AddScoped<IRestricaoService, RestricaoService>();
builder.Services.AddScoped<IItemMissaService, ItemMissaService>();
builder.Services.AddScoped<IHistoricoEscalaService, HistoricoEscalaService>();
builder.Services.AddScoped<IFuncaoService, FuncaoService>();
builder.Services.AddScoped<IEscalaService, EscalaService>();
builder.Services.AddScoped<ICargoNivelFuncaoPermitidaService, CargoNivelFuncaoPermitidaService>();
builder.Services.AddScoped<IMissasService, MissasService>();
builder.Services.AddScoped<IHorarioFixoService, HorarioFixoService>();
builder.Services.AddScoped<IMissaPadraoService, MissaPadraoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IParoquiaService, ParoquiaService>();

// Configuração de Autenticação JWT Rígida
var jwtKey = builder.Configuration["Jwt:Key"] ?? "EscalaMensalSuperSecretKey2026LiturgiaAppSecuritySecretKey#99";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "EscalaMensalAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "EscalaMensalApp";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    // Suporte ao SignalR para ler o token da Query String (?access_token=...)
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EscalaMensal API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT no formato: Bearer {seu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAutoMapper(typeof(MissaProfile));
builder.Services.AddAutoMapper(typeof(ItemMissaProfile));
builder.Services.AddAutoMapper(typeof(FuncaoDto));
builder.Services.AddAutoMapper(typeof(HorarioFixoProfile));
builder.Services.AddAutoMapper(typeof(MissaPadraoProfile));
builder.Services.AddAutoMapper(typeof(MembroProfile));
builder.Services.AddAutoMapper(typeof(ParoquiaProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy => policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("https://localhost:7185", "http://localhost:5293", "http://localhost:5173")
            .AllowCredentials());
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowBlazor");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<EscalaMensal.API.Hubs.EscalaHub>("/hubs/escala-colaborativa");

app.Run();
