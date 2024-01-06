using AppOndeE.Cliente.Models;
using AppOndeE.Dao;
using AppOndeE.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AppOndeE.Cliente.Controllers
{
    public class HomeController : Controller
    {
        ConfigEmail mailUtils = new ConfigEmail();
        string cabecalhoEmail = "https://clientes.ondee.app.br/assets/img/logo.jpg";

        #region Views
        public IActionResult Index()
        {
            ClienteDao clienteDao = new ClienteDao();
            int idCliente = Convert.ToInt32(HttpContext.Session.GetString("idCliente"));

            UsuariosClienteModel model = new UsuariosClienteModel
            {
                Id = Convert.ToInt32(HttpContext.Session.GetString("idUsuCliente")),
                IdCliente = idCliente,
                Senha = ""
            };

            var dados = clienteDao.CarregarDadosClienteLogado(model);

            if (dados != null)
            {
                ViewBag.BemVindoI = $"Bem vindo Usuário: {dados["Usuario"].ToString()}";
                ViewBag.BemVindoII = $"Código Cliente: {dados["CodigoCliente"].ToString()} Cliente: {dados["NomeCliente"].ToString()}";
            }

            AnuncioDao anuncioDao = new AnuncioDao();
            int qtdePulicacoes = anuncioDao.QtdeAnuncios(idCliente);

            ViewBag.QtdeAnuncios = $"{qtdePulicacoes} Publicações feitas";

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult RecuperacaoSenha(string code)
        {
            return View();
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult ConfiguracoesSistema()
        {
            return View();
        }
        #endregion

        #region Calls
        [HttpPost]
        public string LoginValido([FromBody] AppOndeE.Model.LoginCliente model)
        {
            ClienteDao clienteDao = new ClienteDao();

            var dados = clienteDao.DadosLogin(model);

            if (dados != null)
            {
                HttpContext.Session.SetString("idCliente", dados["IdCliente"].ToString());
                HttpContext.Session.SetString("idUsuCliente", dados["IdUsuario"].ToString());

                return "sucesso";
            }
            else
            {
                return "Login e/ou Senha inválidos.";
            }
        }

        [HttpPost]
        public string RecuperarEmailSenha(string email)
        {
            ClienteDao clienteDao = new ClienteDao();
            var serialRecovery = clienteDao.RecuperarSerial(email);

            if (serialRecovery != "")
            {
                bool emailEnviado = mailUtils.EmailEnviado(email, "Recuperação de Senha", MontarCorpoEmailRecSenhaCliente(serialRecovery), "Recuperação de Senha Cliente");
                return $"Foi enviado um e-mail para {email}. Verifique sua Caixa de Entrada, caso não esteja, verifique no SPAM";
            }
            else
            {
                return "E-mail não cadastrado";
            }
        }

        [HttpPost]
        public UsuarioCliente DadosRecuperacaoSenha(string code)
        {
            ClienteDao clienteDao = new ClienteDao();
            return clienteDao.DadosRecuperacaoSenha(code);
        }

        [HttpPost]
        public string AlterarSenha([FromBody] UsuariosClienteModel model)
        {
            ClienteDao clienteDao = new ClienteDao();
            return clienteDao.AlterarSenha(model);
        }

        [HttpPost]
        public string CadastrarNovoCliente([FromBody] CadastroNovoCli model)
        {
            ClienteDao clienteDao = new ClienteDao();

            if (clienteDao.ExisteEmail(model.Email) == true)
            {
                return "E-mail já cadastrado!";
            }

            if (clienteDao.ExisteCnpj(model.Cnpj) == true)
            {
                return "Cnpj já cadastrado!";
            }

            int id = clienteDao.CadastrarNovoCliente(model);

            UsuarioCliente u = new UsuarioCliente
            {
                Email = model.Email,
                Id = 0,
                IdCliente = id,
                Senha = model.Senha
            };

            if (clienteDao.CadastrarNovoUsuarioCliente(u) == true)
            {
                if(mailUtils.EmailEnviado(u.Email, "Cadastro App Onde É", MontarCorpoEmailCadastroCliente(model), "Cadastro App Onde É"))
                {
                    return "Dados Cliente cadastrado com sucesso!";
                }
                else
                {
                    return "Erro ao enviar e-mail confirmação de cadastro";
                }
            }
            else
            {
                return "Erro ao Cadastrar Novo Usuário";
            }

        }

        [HttpPost]
        public ClienteModel CarregarDadosConfiguracoesCliente(int id)
        {
            ClienteDao clienteDao = new ClienteDao();
            return clienteDao.CarregarDadosConfiguracoesCliente(id);
        }

        [HttpPost]
        public string SalvarDadosConfiguracoesCliente([FromBody] ClienteModel model)
        {
            ClienteDao clienteDao = new ClienteDao();
            return clienteDao.SalvarDadosConfiguracoesCliente(model);
        }

        [HttpPost]
        public void Logout()
        {
            UsuariosClienteModel model = new UsuariosClienteModel
            {
                Id = Convert.ToInt32(HttpContext.Session.GetString("idUsuCliente").ToString()),
                IdCliente = Convert.ToInt32(HttpContext.Session.GetString("idCliente").ToString())
            };

            ClienteDao dao = new ClienteDao();
            dao.Logoff(model);

            HttpContext.Session.SetString("idCliente", "0");
            HttpContext.Session.SetString("idUsuCliente", "0");
        }
        #endregion

        private string MontarCorpoEmailCadastroCliente(CadastroNovoCli model)
        {
            string corpoEmail = string.Empty;

            corpoEmail += "<div style='text-align:center'><div style='max-width: 600px; margin: 0 auto;'>" +
                      $"<img src='{cabecalhoEmail}' width=300 heigth=300>" + $"<br /></div><br />";
            corpoEmail += $"<h2>Prezado Usuário {model.Email} cliente {model.NomeFantasia}</h2>";
            corpoEmail += "<p>Informamos que seus dados no App Onde é foi realizado com sucesso.</p>";
            corpoEmail += "<p>Se precisar de ajuda, conte com a gente!*</p><br />";
            corpoEmail += "<p>Equipe App Onde É</p>";

            return corpoEmail;
        }

        private string MontarCorpoEmailRecSenhaCliente(string serial)
        {
            string corpoEmail = string.Empty;

            corpoEmail += "<div style='text-align:center'><div style='max-width: 600px; margin: 0 auto;'>" +
                      $"<img src='{cabecalhoEmail}' width=300 heigth=300>" + $"<br /></div><br />";
            corpoEmail += "<h2>Olá</h2>";
            corpoEmail += "<p>Recebemos sua solicitação de recuperação de senha.</p>";
            corpoEmail += $"<p>Para criar uma nova,<a href='https://clientes.ondee.app.br/Home/RecuperacaoSenha?code={serial}'>Clique aqui</a></p>";
            corpoEmail += "<p><b>Importante:</b>o link é válido por 24 horas.</p>";
            corpoEmail += "<p>Após este período, você deverá solicitar um novo, tá bem?!</p>";
            corpoEmail += "<p>Caso não tenha sido você, por gentileza, desconsidere este e-mail.</p>";
            corpoEmail += "<p>Se precisar de ajuda, conte com a gente!*</p><br />";
            corpoEmail += "<p>Equipe App Onde É</p>";

            return corpoEmail;
        }
    }

    public class LoginCliente
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
    }
}
