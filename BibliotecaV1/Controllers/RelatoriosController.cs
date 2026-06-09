using BibliotecaV1.Data;
using BibliotecaV1.Filters;
using BibliotecaV1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaV1.Controllers
{
    [AutorizacaoFilter]
    public class RelatoriosController : Controller
    {
        private readonly BibliotecaContext _context;

        public RelatoriosController(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Historico()
        {
            var relatorio = await _context.Emprestimos
                .Include(e => e.Usuario)
                .Include(e => e.Livro)
                .Select(e => new RelatorioEmprestimoViewModel
                {
                    Usuario = e.Usuario!.NomeCompleto,
                    Livro = e.Livro!.NomeLivro,
                    DataEmprestimo = e.DataEmprestimo,
                    DataDevolucao = e.DataRealDevolucao,
                    Multa = e.Multa,
                    Status = e.Status
                })
                .ToListAsync();

            // GRÁFICO PIZZA
            var categorias = await _context.Livros
                .GroupBy(l => l.Categoria)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Quantidade = g.Count()
                })
                .ToListAsync();

            ViewBag.Categorias = categorias;

            // GRÁFICO LINHA
            var emprestimosMes = await _context.Emprestimos
                .GroupBy(e => e.DataEmprestimo.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Total = g.Count()
                })
                .OrderBy(x => x.Mes)
                .ToListAsync();

            ViewBag.EmprestimosMes = emprestimosMes;

            // CARDS
            ViewBag.TotalLivros =
                await _context.Livros.CountAsync();

            ViewBag.TotalUsuarios =
                await _context.Usuarios.CountAsync();

            ViewBag.EmprestimosAtivos =
                await _context.Emprestimos
                    .CountAsync(e => e.Status == "Emprestado");

            ViewBag.TotalMultas =
                await _context.Emprestimos
                    .SumAsync(e => e.Multa);

            return View(relatorio);
        }
    }
}