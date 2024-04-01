using System;

namespace DAO
{
    public class PedidoMaeProdutoDAO
    {
        public long PedidoMaeProdutoID { get; set; }
        public string PedidoMaeID { get; set; }
        public long ProdutoID { get; set; }
        public short Quantidade { get; set; }
        public string Medida { get; set; }
        public string Produto { get; set; }
        public int SistemaID { get; set; }
        public string EmailID { get; set; }
    }
}
