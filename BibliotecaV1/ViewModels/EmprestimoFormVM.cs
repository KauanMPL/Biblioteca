using System.ComponentModel.DataAnnotations;

namespace BibliotecaV1.ViewModels
{
    public class EmprestimoFormVM
    {
        [Required(ErrorMessage = "Selecione um usuário.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Selecione um livro.")]
        public int LivroId { get; set; }

        [Required(ErrorMessage = "Informe a data prevista.")]
        [DataType(DataType.Date)]
        public DateTime DataPrevistaDevolucao { get; set; }
    }
}