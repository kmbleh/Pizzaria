using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzaria
{
    class Produto
    {
        public int Codigo { get; set; }
        public int IdTipo { get; set; }
        public int IdTam { get; set; }
        public bool Vegetariano { get; set;}
        public string Tamanho { get; set; }
        public virtual int CodTam { get; set; }
        public string Tipo { get; set; }
        public virtual int CodTipo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Imagem { get; set; }
    }
}
