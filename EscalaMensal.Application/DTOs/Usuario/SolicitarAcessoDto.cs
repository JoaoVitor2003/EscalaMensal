using System.ComponentModel.DataAnnotations;

namespace EscalaMensal.Application.DTOs.Usuario
{
    public class SolicitarAcessoDto
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [MinLength(3, ErrorMessage = "O nome deve ter pelo menos 3 caracteres.")]
        [MaxLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [MaxLength(150, ErrorMessage = "O e-mail não pode exceder 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [MaxLength(20, ErrorMessage = "O telefone não pode exceder 20 caracteres.")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [MaxLength(100, ErrorMessage = "A senha não pode exceder 100 caracteres.")]
        public string Senha { get; set; } = string.Empty;
    }
}
