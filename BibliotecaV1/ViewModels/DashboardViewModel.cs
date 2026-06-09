namespace BibliotecaV1.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalUsuarios { get; set; }
        public int TotalLivros { get; set; }
        public int EmprestimosAtivos { get; set; }
        public int LivrosDisponiveis { get; set; }
        public decimal TotalMultas { get; set; }
    }
}