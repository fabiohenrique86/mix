using System;
using System.Collections.Generic;
using System.Linq;
using DAO;
using DAL;
using System.Transactions;

namespace BLL
{
    public class TrocaBLL
    {
        private TrocaDAL trocaDAL;
        private TrocaProdutoDAL trocaProdutoDAL;
        private ClienteBLL clienteBLL;

        public TrocaBLL()
        {
            trocaDAL = new TrocaDAL();
            trocaProdutoDAL = new TrocaProdutoDAL();
            clienteBLL = new ClienteBLL();
        }

        private void ValidarIncluir(TrocaDAO trocaDAO)
        {
            if (trocaDAO == null)
            {
                throw new ApplicationException("trocaDAO é obrigatório");
            }

            if (trocaDAO.LojaID <= 0)
            {
                throw new ApplicationException("LojaID é obrigatório");
            }

            if (!string.IsNullOrEmpty(trocaDAO.CPF))
            {
                trocaDAO.ClienteID = clienteBLL.SeExisteClienteComCpfInformado(trocaDAO.CPF, trocaDAO.SistemaID);
            }
            else
            {
                trocaDAO.ClienteID = clienteBLL.SeExisteClienteComCnpjInformado(trocaDAO.CNPJ, trocaDAO.SistemaID);
            }

            if (trocaDAO.ClienteID <= 0)
            {
                throw new ApplicationException("ClienteID é obrigatório");
            }

            if (trocaDAO.DataTroca == DateTime.MinValue)
            {
                throw new ApplicationException("DataTroca é obrigatório");
            }

            if (trocaDAO.DataEntrega == DateTime.MinValue)
            {
                throw new ApplicationException("DataEntrega é obrigatório");
            }

            if (trocaDAO.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID é obrigatório");
            }

            if (trocaDAO.TrocaProdutoDAO == null || trocaDAO.TrocaProdutoDAO.Count <= 0)
            {
                throw new ApplicationException("Selecione ao menos um produto para cadastrar troca");
            }
        }

        private void ValidarExcluir(TrocaDAO trocaDAO)
        {
            if (trocaDAO == null)
            {
                throw new ApplicationException("trocaDAO é obrigatório");
            }

            if (trocaDAO.TrocaID <= 0)
            {
                throw new ApplicationException("TrocaID é obrigatório");
            }

            if (trocaDAO.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID é obrigatório");
            }

            var troca = this.Listar(new TrocaDAO() { TrocaID = trocaDAO.TrocaID, SistemaID = trocaDAO.SistemaID }).FirstOrDefault();

            if (troca == null)
            {
                throw new ApplicationException(string.Format("Troca {0} inexistente", trocaDAO.TrocaID));
            }

            if (!troca.Ativo)
            {
                throw new ApplicationException(string.Format("Troca {0} já foi dada baixa. Não é possível cancelar trocas baixadas", trocaDAO.TrocaID));
            }
        }

        public int Incluir(TrocaDAO trocaDAO)
        {
            int trocaId = 0;

            try
            {
                ValidarIncluir(trocaDAO);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    trocaId = trocaDAL.Incluir(trocaDAO);

                    foreach (var trocaProdutoDAO in trocaDAO.TrocaProdutoDAO)
                    {
                        trocaProdutoDAO.TrocaID = trocaId;
                        trocaProdutoDAL.Incluir(trocaProdutoDAO);
                    }

                    scope.Complete();
                }
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return trocaId;
        }

        private void ValidarListar(TrocaDAO trocaDAO)
        {
            if (!string.IsNullOrEmpty(trocaDAO.CPF))
            {
                trocaDAO.ClienteID = clienteBLL.SeExisteClienteComCpfInformado(trocaDAO.CPF, trocaDAO.SistemaID);
            }
            else if (!string.IsNullOrEmpty(trocaDAO.CNPJ))
            {
                trocaDAO.ClienteID = clienteBLL.SeExisteClienteComCnpjInformado(trocaDAO.CNPJ, trocaDAO.SistemaID);
            }
        }

        public List<TrocaDAO> Listar(TrocaDAO trocaDAO)
        {
            var trocasDAO = new List<TrocaDAO>();

            try
            {
                ValidarListar(trocaDAO);

                trocasDAO = trocaDAL.Listar(trocaDAO);
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return trocasDAO;
        }

        private void ValidarDarBaixa(TrocaDAO trocaDAO)
        {
            if (trocaDAO == null)
            {
                throw new ApplicationException("trocaDAO é obrigatório");
            }

            if (trocaDAO.TrocaID <= 0)
            {
                throw new ApplicationException("TrocaID é obrigatório");
            }

            if (trocaDAO.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID é obrigatório");
            }

            var troca = this.Listar(trocaDAO).FirstOrDefault();

            if (troca == null)
            {
                throw new ApplicationException(string.Format("Troca {0} não encontrada", trocaDAO.TrocaID));
            }

            if (!troca.Ativo)
            {
                throw new ApplicationException(string.Format("Baixa da troca {0} já foi efetuada", trocaDAO.TrocaID));
            }
        }

        public void DarBaixa(TrocaDAO trocaDAO)
        {
            ValidarDarBaixa(trocaDAO);

            trocaDAL.DarBaixa(trocaDAO);
        }

        private void ValidarListarComandaTroca(TrocaDAO trocaDAO)
        {
            if (trocaDAO == null)
            {
                throw new ApplicationException("trocaDAO é obrigatório");
            }

            if (trocaDAO.TrocaID <= 0)
            {
                throw new ApplicationException("TrocaID é obrigatório");
            }

            if (trocaDAO.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID é obrigatório");
            }
        }

        public List<TrocaDAO> ListarComandaTroca(TrocaDAO trocaDAO)
        {
            List<TrocaDAO> trocasDAO = new List<TrocaDAO>();

            try
            {
                ValidarListarComandaTroca(trocaDAO);

                trocasDAO = trocaDAL.ListarComandaTroca(trocaDAO);
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return trocasDAO;
        }

        public void Excluir(TrocaDAO trocaDAO)
        {
            try
            {
                ValidarExcluir(trocaDAO);

                trocaDAL.Excluir(trocaDAO);
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
    }
}
