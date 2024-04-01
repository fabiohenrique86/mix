using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;

namespace BLL
{
    public class Orcamento
    {
        public DataSet Listar(DAO.Orcamento orcamento)
        {
            DAL.Orcamento orcamentoDAL = new DAL.Orcamento();
            return orcamentoDAL.Listar(orcamento);
        }

        public DataSet Listar(DAO.Orcamento orcamento, bool top)
        {
            DAL.Orcamento orcamentoDAL = new DAL.Orcamento();
            return orcamentoDAL.Listar(orcamento, top);
        }

        public DataSet ListarProduto(DAO.Orcamento orcamento)
        {
            DAL.Orcamento orcamentoDAL = new DAL.Orcamento();
            return orcamentoDAL.ListarProduto(orcamento);
        }

        public bool ExisteOrcamento(DAO.Orcamento orcamento)
        {
            DAL.Orcamento orcamentoDAL = new DAL.Orcamento();
            return orcamentoDAL.ExisteOrcamento(orcamento);
        }        

        public void Inserir(DAO.Orcamento orcamento)
        {
            DAL.Orcamento orcamentoDAL = new DAL.Orcamento();
            orcamentoDAL.Inserir(orcamento);
        }

        public void Excluir(DAO.Orcamento orcamento)
        {
            DAL.Orcamento orcamentoDAL = new DAL.Orcamento();
            orcamentoDAL.Excluir(orcamento);
        }
    }
}
