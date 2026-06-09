using System.ComponentModel.DataAnnotations;

namespace BibliotecaV1.Models
{
    public class Emprestimo
    {
        public int Id { get; set; }

        [Required]
        public DateTime DataEmprestimo { get; set; } = DateTime.Now;

        [Required]
        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        [Required]
        public int LivroId { get; set; }

        public Livro? Livro { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataPrevistaDevolucao { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataRealDevolucao { get; set; }

        [Range(0, 999999)]
        public decimal Multa { get; set; }

        [Required]
        public string Status { get; set; } = "Emprestado";
    }
}