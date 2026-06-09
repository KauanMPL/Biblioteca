using BibliotecaV1.Data;
using BibliotecaV1.Filters;
using BibliotecaV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaV1.Controllers
{
    [AutorizacaoFilter]
    public class LivrosController : Controller
    {
        private readonly BibliotecaContext _context;

        public LivrosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Livros
        public async Task<IActionResult> Index(string busca)
        {
            var livros = _context.Livros.AsQueryable();

            if (!string.IsNullOrEmpty(busca))
            {
                livros = livros.Where(l =>
                    l.NomeLivro.Contains(busca) ||
                    l.Autor.Contains(busca) ||
                    l.Categoria.Contains(busca));
            }

            ViewBag.Busca = busca;

            return View(await livros.ToListAsync());
        }

        // GET: Livros Disponíveis
        public async Task<IActionResult> Disponiveis()
        {
            var livros = await _context.Livros
                .Where(l => l.QuantidadeEstoque > 0)
                .ToListAsync();

            return View("Index", livros);
        }

        // GET: Livros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var livro = await _context.Livros
                .FirstOrDefaultAsync(m => m.Id == id);

            if (livro == null)
                return NotFound();

            return View(livro);
        }

        // GET: Livros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Livros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,NomeLivro,Autor,QuantidadeEstoque,FaixaEtariaPermitida,Categoria,AnoPublicacao")]
            Livro livro)
        {
            if (!ModelState.IsValid)
                return View(livro);

            _context.Add(livro);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Livros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var livro = await _context.Livros.FindAsync(id);

            if (livro == null)
                return NotFound();

            return View(livro);
        }

        // POST: Livros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,NomeLivro,Autor,QuantidadeEstoque,FaixaEtariaPermitida,Categoria,AnoPublicacao")]
            Livro livro)
        {
            if (id != livro.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(livro);

            try
            {
                _context.Update(livro);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LivroExists(livro.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Livros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var livro = await _context.Livros
                .FirstOrDefaultAsync(m => m.Id == id);

            if (livro == null)
                return NotFound();

            return View(livro);
        }

        // POST: Livros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = await _context.Livros.FindAsync(id);

            if (livro != null)
            {
                _context.Livros.Remove(livro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LivroExists(int id)
        {
            return _context.Livros.Any(e => e.Id == id);
        }


    }
}