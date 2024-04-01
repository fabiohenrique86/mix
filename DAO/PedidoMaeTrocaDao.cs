using System.Collections.Generic;

namespace DAO
{
    public class PedidoMaeTrocaDAO
    {
        public long PedidoMaeTrocaID { get; set; }
        public string PedidoMaeID { get; set; }
        public int SistemaID { get; set; }

        public List<PedidoMaeTrocaProdutoDAO> PedidoMaeTrocaProdutosDAO = new List<PedidoMaeTrocaProdutoDAO>();
    }
}
