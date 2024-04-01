using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO;
using DAL;

namespace BLL
{
    public class TrocaProdutoBLL
    {
        private TrocaProdutoDAL trocaProdutoDAL;

        public TrocaProdutoBLL()
        {
            trocaProdutoDAL = new TrocaProdutoDAL();
        }

        private void ValidarIncluir(TrocaProdutoDAO trocaProdutoDAO)
        {
            if (trocaProdutoDAO == null)
            {
                throw new ApplicationException("trocaProdutoDAO é obrigatório");
            }

            if (trocaProdutoDAO.TrocaID <= 0)
            {
                throw new ApplicationException("TrocaID é obrigatório");
            }

            if (trocaProdutoDAO.ProdutoDAO == null)
            {
                throw new ApplicationException("ProdutoDAO é obrigatório");
            }

            if (trocaProdutoDAO.ProdutoDAO.ProdutoID <= 0)
            {
                throw new ApplicationException("ProdutoID é obrigatório");
            }

            if (trocaProdutoDAO.ProdutoDAO.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID é obrigatório");
            }

            if (trocaProdutoDAO.ProdutoDAO.Quantidade <= 0)
            {
                throw new ApplicationException("Quantidade é obrigatório");
            }
        }

        public void Incluir(TrocaProdutoDAO trocaProdutoDAO)
        {
            try
            {
                ValidarIncluir(trocaProdutoDAO);

                trocaProdutoDAL.Incluir(trocaProdutoDAO);
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TrocaProdutoDAO> Listar(TrocaDAO trocaDAO)
        {
            List<TrocaProdutoDAO> trocaProdutosDAO = new List<TrocaProdutoDAO>();

            try
            {
                trocaProdutosDAO = trocaProdutoDAL.Listar(trocaDAO);
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return trocaProdutosDAO;
        }
    }
}
