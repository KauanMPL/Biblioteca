using System.ComponentModel.DataAnnotations;

namespace BibliotecaV1.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        [StringLength(150)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data de nascimento obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha obrigatória")]
        [StringLength(100)]
        public string Senha { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public ICollection<Emprestimo>? Emprestimos { get; set; }
    }
}