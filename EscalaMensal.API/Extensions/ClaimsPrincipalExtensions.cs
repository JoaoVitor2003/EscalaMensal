using System;
using System.Security.Claims;
using EscalaMensal.Domain.Enums;

namespace EscalaMensal.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int ObterUsuarioId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out var id) ? id : 0;
        }

        public static string ObterNome(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }

        public static string ObterEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }

        public static PerfilUsuarioEnum ObterPerfil(this ClaimsPrincipal user)
        {
            var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;
            if (Enum.TryParse<PerfilUsuarioEnum>(roleClaim, true, out var perfil))
            {
                return perfil;
            }
            return PerfilUsuarioEnum.Normal;
        }

        public static bool EhAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("Administrador");
        }

        public static bool EhPremiumOuAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("Administrador") || user.IsInRole("Premium");
        }
    }
}
