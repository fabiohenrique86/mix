using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    /// <summary>
    /// Produto
    /// </summary>
    public class Produto
    {
        /// <summary>
        /// Verifica se o produto existe na loja
        /// </summary>
        /// <param name="produtoDAO"></param>
        /// <returns></returns>
        public bool ExisteNaLoja(DAO.Produto produtoDAO)
        {
            bool retorno = false;

            DAL.Produto produtoDAL = new DAL.Produto();

            retorno = produtoDAL.ExisteNaLoja(produtoDAO.ProdutoID, produtoDAO.LojaID.ToString(), produtoDAO.SistemaID);

            return retorno;
        }
    }
}
