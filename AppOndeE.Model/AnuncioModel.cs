using System;
using System.Collections.Generic;
using System.Text;

namespace AppOndeE.Model
{
    public class AnuncioModel
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int TipoAnuncio { get; set; }
        public string TituloAnuncio { get; set; }
        public string DescricaoAnuncio { get; set; }
        public byte[] Midia { get; set; }

        public string DataAnuncio { get; set; }
    }
}
