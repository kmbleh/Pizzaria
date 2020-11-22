using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzaria
{
    class Vendas
    {
        public int Id { get; set; }
        public int CodCliente { get; set; }
        public int CodVendas { get; set; }
        public string Vendedor { get; set; }
        public DateTime Hora { get; set; }
        public int CodTipo { get; set; }
        public string Tipo { get; set; }
        public int CodProd { get; set; }
        public string Produto { get; set; }
        public bool Vegetariano { get; set; }
        public int CodTam { get; set; }
        public string Tamanho { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }
}
