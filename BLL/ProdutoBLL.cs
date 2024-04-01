using DAL;
using DAO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

namespace BLL
{
    public class ProdutoBLL
    {
        private ProdutoDAL produtoDAL;

        public ProdutoBLL()
        {
            produtoDAL = new ProdutoDAL();
        }

        /// <summary>
        /// Verifica se o produto existe na loja
        /// </summary>
        /// <param name="produtoDAO"></param>
        /// <returns></returns>
        public bool ExisteNaLoja(ProdutoDAO produtoDAO)
        {
            bool retorno = false;

            retorno = produtoDAL.ExisteNaLoja(produtoDAO.ProdutoID, produtoDAO.LojaID.ToString(), produtoDAO.SistemaID);

            return retorno;
        }

        public void Zerar(ProdutoDAO produtoDAO)
        {
            produtoDAL.Zerar(produtoDAO);
        }

        public List<ProdutoDAO> Listar(string descricao, string medida, int sistemaId)
        {
            return produtoDAL.Listar(descricao, medida, sistemaId);
        }

        //public ProdutoDAO ListarProdutoLojaById(long produtoId, int sistemaId)
        //{
        //    return produtoDAL.ListarProdutoLojaById(produtoId, sistemaId);
        //}

        public void AtualizarViaArquivoBancoDados(List<ProdutoDAO> ProdutosDAO)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(5) }))
                {
                    foreach (var produtoDAO in ProdutosDAO)
                    {
                        if (produtoDAO.MedidaID == null || produtoDAO.MedidaID <= 0)
                        {
                            // insert medida
                            var medidaId = MedidaDAL.Inserir(produtoDAO.Medida, produtoDAO.SistemaID);
                            produtoDAO.MedidaID = medidaId;
                        }

                        if (produtoDAO.LinhaID == null || produtoDAO.LinhaID <= 0)
                        {
                            // insert linha
                            var linhaId = LinhaDAL.Inserir(produtoDAO.Linha, string.Empty, produtoDAO.SistemaID);
                            produtoDAO.LinhaID = linhaId;
                        }

                        // insert produto
                        produtoDAL.Inserir(produtoDAO);
                    }

                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao atualizar registro", ex);
            }
        }
    }
}
