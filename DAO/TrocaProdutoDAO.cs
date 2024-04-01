namespace DAO
{
    public class TrocaProdutoDAO
    {
        public TrocaProdutoDAO()
        {
            ProdutoDAO = new ProdutoDAO();
        }

        public int TrocaID { get; set; }

        public ProdutoDAO ProdutoDAO { get; set; }
    }
}
