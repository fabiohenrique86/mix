using System;
using System.Collections.Generic;

namespace DAO
{
    public class PedidoMaeDAO
    {
        public string PedidoMaeID { get; set; }
        public DateTime DataCadastro { get; set; }
        public int SistemaID { get; set; }
        public string Origem { get; set; }
        public string EmailID { get; set; }
        public bool Lincado { get; set; }

        public List<PedidoMaeProdutoDAO> PedidoMaeProdutoDAO = new List<PedidoMaeProdutoDAO>();
        public List<PedidoMaeFilhoDAO> PedidoMaeFilhoDAO = new List<PedidoMaeFilhoDAO>();
    }
}
