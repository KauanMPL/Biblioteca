using BibliotecaV1.Data;
using BibliotecaV1.Filters;
using BibliotecaV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaV1.Controllers
{
    [AutorizacaoFilter]
    public class EmprestimoesController : Controller
    {
        private readonly BibliotecaContext _context;

        public EmprestimoesController(BibliotecaContext context)
        {
            _context = context;
        }

        // LISTAR
        public async Task<IActionResult> Index()
        {
            var emprestimos = _context.Emprestimos
                .Include(e => e.Usuario)
                .Include(e => e.Livro);

            return View(await emprestimos.ToListAsync());
        }

        // DETALHES
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var emprestimo = await _context.Emprestimos
                .Include(e => e.Usuario)
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (emprestimo == null)
                return NotFound();

            return View(emprestimo);
        }

        // CREATE GET
        public IActionResult Create()
        {
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "NomeLivro");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeCompleto");

            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,UsuarioId,LivroId,DataPrevistaDevolucao")]
            Emprestimo emprestimo)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == emprestimo.UsuarioId);

            var livro = await _context.Livros
                .FirstOrDefaultAsync(l => l.Id == emprestimo.LivroId);

            if (usuario == null || livro == null)
            {
                ModelState.AddModelError("", "Usuário ou livro inválido.");
            }

            // REGRA 1 - ESTOQUE
            if (livro != null && livro.QuantidadeEstoque <= 0)
            {
                ModelState.AddModelError("", "Livro sem estoque disponível.");
            }

            // REGRA 2 - LIVRO 18+
            if (usuario != null && livro != null)
            {
                int idade = DateTime.Today.Year - usuario.DataNascimento.Year;

                if (usuario.DataNascimento.Date > DateTime.Today.AddYears(-idade))
                    idade--;

                if (livro.FaixaEtariaPermitida >= 18 && idade < 18)
                {
                    ModelState.AddModelError("", "Usuário menor de idade não pode pegar este livro.");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "NomeLivro", emprestimo.LivroId);
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeCompleto", emprestimo.UsuarioId);

                return View(emprestimo);
            }

            emprestimo.DataEmprestimo = DateTime.Now;
            emprestimo.Status = "Emprestado";
            emprestimo.Multa = 0;

            // REGRA 3 - BAIXAR ESTOQUE
            livro!.QuantidadeEstoque--;

            _context.Emprestimos.Add(emprestimo);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DEVOLUÇÃO

        // EDIT GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var emprestimo = await _context.Emprestimos.FindAsync(id);

            if (emprestimo == null)
                return NotFound();

            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "NomeLivro", emprestimo.LivroId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "NomeCompleto", emprestimo.UsuarioId);

            return View(emprestimo);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,DataEmprestimo,UsuarioId,LivroId,DataPrevistaDevolucao,DataRealDevolucao,Multa,Status")]
            Emprestimo emprestimo)
        {
            if (id != emprestimo.Id)
                return NotFound();

            try
            {
                _context.Update(emprestimo);
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (!EmprestimoExists(emprestimo.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var emprestimo = await _context.Emprestimos
                .Include(e => e.Usuario)
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (emprestimo == null)
                return NotFound();

            return View(emprestimo);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);

            if (emprestimo != null)
            {
                _context.Emprestimos.Remove(emprestimo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EmprestimoExists(int id)
        {
            return _context.Emprestimos.Any(e => e.Id == id);
        }
    }
}