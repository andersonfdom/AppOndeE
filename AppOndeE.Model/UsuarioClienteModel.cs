using System;
using System.Collections.Generic;
using System.Text;

namespace AppOndeE.Model
{
    public class UsuarioClienteModel
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }

        public string Email { get; set; }
        public string Senha { get; set; }
        public string UltimoAcesso { get; set; }
    }
}
