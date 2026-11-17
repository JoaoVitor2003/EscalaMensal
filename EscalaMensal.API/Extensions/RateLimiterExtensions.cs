using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.RateLimiting;

namespace EscalaMensal.API.Extensions
{
    /// <summary>
    /// Configuração centralizada de Rate Limiting para proteção contra ataques de Força Bruta, DoS e Flood.
    /// </summary>
    public static class RateLimiterExtensions
    {
        public static IServiceCollection ConfigurarRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                // Resposta HTTP 429 Too Many Requests padronizada em JSON com cabeçalho Retry-After
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.ContentType = "application/json";

                    var segundosEspera = 60;
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfterTimeSpan))
                    {
                        segundosEspera = Math.Max(1, (int)retryAfterTimeSpan.TotalSeconds);
                    }
                    context.HttpContext.Response.Headers.RetryAfter = segundosEspera.ToString();

                    var resposta = new
                    {
                        sucesso = false,
                        mensagem = $"Muitas requisições em pouco tempo. Por segurança, aguarde {segundosEspera} segundos antes de tentar novamente.",
                        segundosEspera = segundosEspera
                    };

                    await context.HttpContext.Response.WriteAsJsonAsync(resposta, cancellationToken);
                };

                // 1. LIMITADOR GLOBAL (Proteção geral contra DoS / Scraping): 120 requisições por minuto por IP
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    var path = httpContext.Request.Path;

                    // Não limita endpoints internos de documentação Swagger nem o handshake de SignalR
                    if (path.StartsWithSegments("/swagger") || path.StartsWithSegments("/hubs"))
                    {
                        return RateLimitPartition.GetNoLimiter("sem_limite_interno");
                    }

                    var clientIp = ObterIpCliente(httpContext);
                    return RateLimitPartition.GetSlidingWindowLimiter(
                        partitionKey: clientIp,
                        factory: _ => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 120,
                            Window = TimeSpan.FromMinutes(1),
                            SegmentsPerWindow = 4,
                            QueueLimit = 0
                        });
                });

                // 2. POLÍTICA DE LOGIN (Proteção rigorosa contra Força Bruta de senhas): Máximo 5 tentativas por minuto por IP
                options.AddPolicy("LoginPolicy", httpContext =>
                {
                    var clientIp = ObterIpCliente(httpContext);
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: clientIp,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });

                // 3. POLÍTICA DE CÓDIGO DE PARÓQUIA (Proteção contra brute-force de códigos de acesso): Máximo 5 tentativas por minuto por IP
                options.AddPolicy("CodigoParoquiaPolicy", httpContext =>
                {
                    var clientIp = ObterIpCliente(httpContext);
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: clientIp,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });

                // 4. POLÍTICA DE REGISTRO / SOLICITAÇÃO (Proteção contra spam de cadastros): Máximo 3 cadastros por minuto por IP
                options.AddPolicy("RegistroPolicy", httpContext =>
                {
                    var clientIp = ObterIpCliente(httpContext);
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: clientIp,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 3,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });

                // 5. POLÍTICA DE GERAÇÃO ALEATÓRIA (Operação pesada de CPU/BD): Máximo 10 gerações por minuto por IP
                options.AddPolicy("GerarAleatorioPolicy", httpContext =>
                {
                    var clientIp = ObterIpCliente(httpContext);
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: clientIp,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        });
                });
            });

            return services;
        }

        private static string ObterIpCliente(HttpContext context)
        {
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(forwardedFor))
            {
                return forwardedFor.Split(',')[0].Trim();
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "ip_desconhecido";
        }
    }
}
