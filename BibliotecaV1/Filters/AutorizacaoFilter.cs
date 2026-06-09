using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BibliotecaV1.Filters
{
    public class AutorizacaoFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuarioId = context.HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                context.Result = new RedirectToActionResult(
                    "Index",
                    "Login",
                    null);
            }

            base.OnActionExecuting(context);
        }
    }
}