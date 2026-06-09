using System.ComponentModel.DataAnnotations;

namespace BibliotecaV1.Models
{
    public class Livro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do livro é obrigatório")]
        [StringLength(200)]
        public string NomeLivro { get; set; } = string.Empty;

        [Required(ErrorMessage = "O autor é obrigatório")]
        [StringLength(150)]
        public string Autor { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa")]
        public int QuantidadeEstoque { get; set; }

        [Required]
        [Range(0, 18)]
        public int FaixaEtariaPermitida { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória")]
        [StringLength(100)]
        public string Categoria { get; set; } = string.Empty;

        [Required]
        [Range(1000, 3000)]
        public int AnoPublicacao { get; set; }

        public ICollection<Emprestimo>? Emprestimos { get; set; }
    }
}