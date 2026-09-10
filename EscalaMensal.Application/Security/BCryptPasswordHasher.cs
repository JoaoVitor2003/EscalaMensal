using System;
using BCrypt.Net;

namespace EscalaMensal.Application.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        // Fator de custo 12 (4.096 iterações de chave) com pre-hashing SHA-384
        // Padrão ouro de segurança recomendado pela OWASP para proteção contra ataques de força bruta e GPU.
        private const int WorkFactor = 12;

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A senha não pode ser vazia.", nameof(password));

            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, WorkFactor);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
            }
            catch
            {
                return false;
            }
        }
    }
}
