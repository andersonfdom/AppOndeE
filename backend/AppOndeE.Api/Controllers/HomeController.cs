using AppOndeE.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppOndeE.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public HomeModel CarregarQtdes()
        {
            HomeModel model = new HomeModel { 
                QtdeClientes = 1,
                QteAnuncios = 10
            };

            return model;

        }
    }
}
