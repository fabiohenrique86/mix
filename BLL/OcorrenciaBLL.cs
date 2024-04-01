using System;
using System.Collections.Generic;
using DAL;
using DAO;
using System.Transactions;
using System.Linq;

namespace BLL
{
    public class OcorrenciaBLL
    {
        private OcorrenciaDAL OcorrenciaDAL;
        private OcorrenciaProdutoDAL OcorrenciaProdutoDAL;

        public OcorrenciaBLL()
        {
            OcorrenciaDAL = new OcorrenciaDAL();
            OcorrenciaProdutoDAL = new OcorrenciaProdutoDAL();
        }

        public List<OcorrenciaDAO> Listar(OcorrenciaDAO ocorrenciaDAO)
        {
            List<OcorrenciaDAO> ocorrencias = new List<OcorrenciaDAO>();

            ocorrencias = OcorrenciaDAL.Listar(ocorrenciaDAO);

            return ocorrencias;
        }

        private void ValidarInserir(OcorrenciaDAO ocorrenciaDAO)
        {
            if (ocorrenciaDAO.DataOcorrencia == DateTime.MinValue)
            {
                throw new ApplicationException("Data de ocorrência obrigatória");
            }

            if (ocorrenciaDAO.NotaFiscalID <= 0)
            {
                throw new ApplicationException("Nº Nota Fiscal de Origem da ocorrência obrigatório");
            }

            if (ocorrenciaDAO.LojaID <= 0)
            {
                throw new ApplicationException("Loja da ocorrência obrigatória");
            }

            if (ocorrenciaDAO.MotivoOcorrenciaID <= 0)
            {
                throw new ApplicationException("Motivo da ocorrência obrigatória");
            }

            if (ocorrenciaDAO.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID obrigatório");
            }

            if (ocorrenciaDAO.ProdutoDAO == null || ocorrenciaDAO.ProdutoDAO.Count <= 0)
            {
                throw new ApplicationException("Produto obrigatório");
            }

            foreach (var produtoDAO in ocorrenciaDAO.ProdutoDAO)
            {
                if (produtoDAO.ProdutoID <= 0)
                {
                    throw new ApplicationException("ProdutoID obrigatório");
                }

                if (produtoDAO.Quantidade <= 0)
                {
                    throw new ApplicationException("Quantidade obrigatório");
                }

                if (produtoDAO.SistemaID <= 0)
                {
                    throw new ApplicationException("SistemaID obrigatório");
                }
            }
        }

        public void Inserir(OcorrenciaDAO ocorrenciaDAO)
        {
            ValidarInserir(ocorrenciaDAO);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                // Ocorrência
                var ocorrenciaId = OcorrenciaDAL.Inserir(ocorrenciaDAO);

                // Ocorrência Produto
                ocorrenciaDAO.ProdutoDAO.ForEach(x => InserirOcorrenciaProduto(ocorrenciaId, x));

                // confirma a transação
                scope.Complete();
            }
        }

        private void InserirOcorrenciaProduto(int ocorrenciaId, ProdutoDAO produtoDAO)
        {
            produtoDAO.OcorrenciaID = ocorrenciaId;
            OcorrenciaProdutoDAL.Inserir(produtoDAO);
        }

        private void ValidarDarBaixa(OcorrenciaDAO ocorrenciaDAO)
        {
            if (ocorrenciaDAO.OcorrenciaID <= 0)
            {
                throw new ApplicationException("OcorrenciaID obrigatória");
            }

            if (ocorrenciaDAO.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID obrigatória");
            }
        }

        public void DarBaixa(OcorrenciaDAO ocorrenciaDAO)
        {
            ValidarDarBaixa(ocorrenciaDAO);

            OcorrenciaDAO ocorrenciaDAO_BD = OcorrenciaDAL.Listar(new OcorrenciaDAO() { OcorrenciaID = ocorrenciaDAO.OcorrenciaID, SistemaID = ocorrenciaDAO.SistemaID }).FirstOrDefault();

            if (ocorrenciaDAO_BD == null)
            {
                throw new ApplicationException(string.Format("OcorrenciaID {0} não cadastrada", ocorrenciaDAO.OcorrenciaID));
            }

            if (ocorrenciaDAO_BD.StatusOcorrenciaID == EStatusOcorrencia.Resolvida.GetHashCode())
            {
                throw new ApplicationException(string.Format("OcorrenciaID {0} já resolvida", ocorrenciaDAO.OcorrenciaID));
            }

            ocorrenciaDAO.StatusOcorrenciaID = EStatusOcorrencia.Resolvida.GetHashCode();

            TransactionOptions transactionOptions = new TransactionOptions() { Timeout = TimeSpan.FromMinutes(5) };

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                if (ocorrenciaDAO_BD.MotivoOcorrenciaID == (int)EMotivoOcorrencia.PRODUTO_DA_REPOSIÇAO_DANIFICADO || ocorrenciaDAO_BD.MotivoOcorrenciaID == (int)EMotivoOcorrencia.FALTA_DE_MERCADORIA)
                {
                    if (ocorrenciaDAO.NotaFiscalID <= 0)
                    {
                        throw new ApplicationException("Nº Nota Fiscal obrigatório");
                    }

                    // se nota fiscal informada = nota fiscal de origem, a ocorrência fica "resolvida" e há mudança no estoque
                    if (ocorrenciaDAO.NotaFiscalID == ocorrenciaDAO_BD.NotaFiscalID)
                    {
                        DarBaixaProduto(ocorrenciaDAO);
                    }
                }
                else if (ocorrenciaDAO_BD.MotivoOcorrenciaID == EMotivoOcorrencia.TROCA_DE_MERCADORIA.GetHashCode())
                {
                    if (ocorrenciaDAO.NumeroTroca == null || ocorrenciaDAO.NumeroTroca <= 0)
                    {
                        throw new ApplicationException("Número de Troca (SVT) obrigatório");
                    }

                    DarBaixaProduto(ocorrenciaDAO);
                }

                OcorrenciaDAL.DarBaixaOcorrencia(ocorrenciaDAO);

                scope.Complete();
            }
        }

        private void DarBaixaProduto(OcorrenciaDAO ocorrenciaDAO)
        {
            // produto deve retornar ao estoque da loja "depósito"
            ocorrenciaDAO.ProdutoDAO = OcorrenciaProdutoDAL.Listar(new ProdutoDAO() { OcorrenciaID = ocorrenciaDAO.OcorrenciaID, SistemaID = ocorrenciaDAO.SistemaID });

            if (ocorrenciaDAO.ProdutoDAO == null || ocorrenciaDAO.ProdutoDAO.Count() <= 0)
            {
                throw new ApplicationException(string.Format("Nenhum produto cadastrado na OcorrenciaID: {0}", ocorrenciaDAO.OcorrenciaID));
            }

            foreach (ProdutoDAO produtoDAO in ocorrenciaDAO.ProdutoDAO)
            {
                if (produtoDAO.OcorrenciaID <= 0) { throw new ApplicationException("OcorrenciaID do produto obrigatória"); }
                if (produtoDAO.ProdutoID <= 0) { throw new ApplicationException("ProdutoID do produto obrigatório"); }
                if (produtoDAO.Quantidade <= 0) { throw new ApplicationException("Quantidade do produto obrigatória"); }
                if (produtoDAO.SistemaID <= 0) { throw new ApplicationException("SistemaID do produto obrigatório"); }

                OcorrenciaDAL.DarBaixaProduto(produtoDAO);
            }
        }
    }
}