namespace BibliotecaV1.ViewModels
{
    public class RelatorioEmprestimoViewModel
    {
        public string Usuario { get; set; } = string.Empty;
        public string Livro { get; set; } = string.Empty;
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public decimal Multa { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}