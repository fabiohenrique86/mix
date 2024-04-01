using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;

namespace BLL
{
    public class OrcamentoBLL
    {
        public DataSet Listar(DAO.OrcamentoDAO orcamento)
        {
            DAL.OrcamentoDAL orcamentoDAL = new DAL.OrcamentoDAL();
            return orcamentoDAL.Listar(orcamento);
        }

        public DataSet Listar(DAO.OrcamentoDAO orcamento, bool top)
        {
            DAL.OrcamentoDAL orcamentoDAL = new DAL.OrcamentoDAL();
            return orcamentoDAL.Listar(orcamento, top);
        }

        public DataSet ListarProduto(DAO.OrcamentoDAO orcamento)
        {
            DAL.OrcamentoDAL orcamentoDAL = new DAL.OrcamentoDAL();
            return orcamentoDAL.ListarProduto(orcamento);
        }

        public bool ExisteOrcamento(DAO.OrcamentoDAO orcamento)
        {
            DAL.OrcamentoDAL orcamentoDAL = new DAL.OrcamentoDAL();
            return orcamentoDAL.ExisteOrcamento(orcamento);
        }        

        public void Inserir(DAO.OrcamentoDAO orcamento)
        {
            DAL.OrcamentoDAL orcamentoDAL = new DAL.OrcamentoDAL();
            orcamentoDAL.Inserir(orcamento);
        }

        public void Excluir(DAO.OrcamentoDAO orcamento)
        {
            DAL.OrcamentoDAL orcamentoDAL = new DAL.OrcamentoDAL();
            orcamentoDAL.Excluir(orcamento);
        }
    }
}
