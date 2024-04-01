using System;
using System.Collections.Generic;
using DAL;
using DAO;

namespace BLL
{
    public class OcorrenciaProdutoBLL
    {
        private OcorrenciaProdutoDAL OcorrenciaProdutoDAL;

        public OcorrenciaProdutoBLL()
        {
            OcorrenciaProdutoDAL = new OcorrenciaProdutoDAL();
        }
        
        public void Inserir(ProdutoDAO produtoDAO)
        {
            OcorrenciaProdutoDAL.Inserir(produtoDAO);
        }

        public List<ProdutoDAO> Listar(ProdutoDAO produtoDAO)
        {
            List<ProdutoDAO> produtos = new List<ProdutoDAO>();

            produtos = OcorrenciaProdutoDAL.Listar(produtoDAO);

            return produtos;
        }
    }
}
