using AppOndeE.Dao;
using AppOndeE.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AppOndeE.Cliente.Controllers
{
    public class UsuariosSistemaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public List<UsuarioClienteModel> ListarUsuarios(int idCliente)
        {
            UsuarioClienteDao dao = new UsuarioClienteDao();

            return dao.ListarUsuarios(idCliente);
        }

        [HttpPost]
        public UsuarioClienteModel CarregarDados(int id)
        {
            UsuarioClienteDao dao = new UsuarioClienteDao();
            return dao.CarregarDadosUsuario(id);
        }

        [HttpPost]
        public string SalvarDadosUsuario([FromBody] UsuarioClienteModel model)
        {
            UsuarioClienteDao dao = new UsuarioClienteDao();

            if (model.Id == 0 && dao.ExisteEmail(model.Email))
            {
                return "E-mail já cadastrado";
            }

            if (dao.SalvarDadosUsuario(model) == true)
            {
                return "sucesso";
            }
            else
            {
                return "Erro ao salvar dados Usuário!";
            }
        }

        [HttpPost]
        public string ExcluirDadosCliente(int id)
        {
            UsuarioClienteDao dao = new UsuarioClienteDao();

            if (dao.UsuarioLogado(id) == true)
            {
                return "Usuário está logado no sistema! Impossível excluir.";
            }

            if (dao.ExcluirDadosCliente(id) == true)
            {
                return "sucesso";
            }
            else
            {
                return "Erro ao excluir dados Usuário!";
            }
        }
    }
}
