using BibliotecaV1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaV1.Controllers
{
    public class HomeController : Controller
    {
        private readonly BibliotecaContext _context;

        public HomeController(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalUsuarios =
                await _context.Usuarios.CountAsync();

            ViewBag.TotalLivros =
                await _context.Livros.CountAsync();

            ViewBag.EmprestimosAtivos =
                await _context.Emprestimos
                    .CountAsync(e => e.Status == "Emprestado");

            ViewBag.Atrasados =
                await _context.Emprestimos
                    .CountAsync(e => e.Status == "Atrasado");

            ViewBag.TotalMultas =
                await _context.Emprestimos
                    .SumAsync(e => e.Multa);

            return View();
        }
    }
}