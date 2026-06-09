using System.ComponentModel.DataAnnotations;

namespace BibliotecaV1.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o e-mail")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a senha")]
        public string Senha { get; set; } = string.Empty;
    }
}