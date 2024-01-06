using Microsoft.AspNetCore.Mvc;

namespace AppOndeE.Cliente.Controllers
{
    public class UsuariosAppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Consulta(int id)
        {
            return View();
        }
    }
}
