using BibliotecaV1.Data;
using BibliotecaV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaV1.Controllers
{
    public class LoginController : Controller
    {
        private readonly BibliotecaContext _context;

        public LoginController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Email == model.Email &&
                    u.Senha == model.Senha &&
                    u.Ativo);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuário ou senha inválidos.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("UsuarioNome", usuario.NomeCompleto);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }
    }
}