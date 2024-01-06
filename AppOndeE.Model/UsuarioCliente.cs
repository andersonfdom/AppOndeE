using System;
using System.Collections.Generic;
using System.Text;

namespace AppOndeE.Model
{
    public class UsuarioCliente
    {
        public int Id { get; set; }

        public int IdCliente { get; set; }

        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class LoginCliente
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }

    public class UsuariosClienteModel
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string Senha { get; set; }
    }
}
