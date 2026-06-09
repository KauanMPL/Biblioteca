using System.ComponentModel.DataAnnotations;

namespace BibliotecaV1.ViewModels
{
    public class DevolucaoVM
    {
        public int EmprestimoId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataDevolucao { get; set; }
    }
}