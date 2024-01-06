using AppOndeE.Dao;
using AppOndeE.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Composition.Convention;

namespace AppOndeE.Cliente.Controllers
{
    public class AnunciosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cadastro(int? id)
        {
            return View();
        }

        [HttpPost]
        public List<AnuncioModel> Listar (int idCliente)
        {
            AnuncioDao dao = new AnuncioDao();
            return dao.Listar(idCliente);
        }

        [HttpPost]
        public AnuncioModel CarregarDados(int id)
        {
            AnuncioDao dao = new AnuncioDao();
            return dao.CarregarDadosAnuncio(id);
        }

        [HttpPost]
        public string Salvar([FromBody] AnuncioModel model)
        {
            AnuncioDao dao = new AnuncioDao();
            
            if (dao.Gravar(model) == true)
            {
                return "Dados Anúncio gravado com sucesso!";
            }
            else
            {
                return "Não foi possível gravar os dados Anúncio! Tente novamente mais tarde.";
            }
        }

        [HttpPost]
        public string Excluir(int id)
        {
            AnuncioDao dao = new AnuncioDao();

            if (dao.Excluir(id) == true)
            {
                return "Dados Anúncio excluído com sucesso!";
            }
            else
            {
                return "Não foi possível excluir os dados Anúncio! Tente novamente mais tarde.";
            }
        }
    }

    public class AnunciosModel
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }

        public int Tipo { get; set; }

        public string TituloAnuncio { get; set; }

        public string DescricaoAnuncio { get; set; }

        public string Midia { get; set; }
    }
}
