namespace DAO
{
    public class PedidoMaeTrocaProdutoDAO
    {
        public long PedidoMaeTrocaProdutoID { get; set; }
        public long PedidoMaeTrocaID { get; set; }
        public long ProdutoID { get; set; }
        public short Quantidade { get; set; }
        public string Medida { get; set; }
        public int SistemaID { get; set; }
    }
}
