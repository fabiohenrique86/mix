using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace BLL
{
    public class Transferencia
    {
        DAL.Transferencia _transferenciaDAL;

        public Transferencia()
        {
            _transferenciaDAL = new DAL.Transferencia();
        }

        public int Inserir(DAO.Transferencia transferenciaDAO)
        {
            int transferenciaId;

            TransactionOptions transactionOptions = new TransactionOptions();

            transactionOptions.Timeout = TimeSpan.FromMinutes(5);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                transferenciaId = InserirTransferencia(transferenciaDAO);

                foreach (var produtoDAO in transferenciaDAO.ListaProduto)
                {
                    produtoDAO.TransferenciaID = transferenciaId;

                    InserirTransferenciaProduto(produtoDAO);                    
                }

                transferenciaDAO.TransferenciaID = transferenciaId;

                if (transferenciaDAO.Valida)
                {
                    AtualizarQuantidadeEstoque(transferenciaDAO);
                }

                scope.Complete();
            }

            return transferenciaId;
        }

        private void validarValidarComandaTransferencia(DAO.Transferencia transferenciaDAO)
        {
            if (transferenciaDAO == null)
            {
                throw new ApplicationException("Informe transferencia");
            }

            if (transferenciaDAO.TransferenciaID <= 0)
            {
                throw new ApplicationException("Campo transferenciaID é obrigatório");
            }

            if (transferenciaDAO.SistemaID <= 0)
            {
                throw new ApplicationException("Campo sistemaID é obrigatório");
            }
        }

        public void ValidarComandaTransferencia(DAO.Transferencia transferenciaDAO)
        {
            TransactionOptions transactionOptions = new TransactionOptions();

            transactionOptions.Timeout = TimeSpan.FromMinutes(5);

            validarValidarComandaTransferencia(transferenciaDAO);

            // Validar para ver se existe o transferenciaId
            var existeTransferencia = _transferenciaDAL.Listar(transferenciaDAO.SistemaID, false, transferenciaDAO.TransferenciaID).Tables[0].Rows.Count > 0 ? true : false;

            if (!existeTransferencia)
            {
                throw new ApplicationException("Comanda de transferência não cadastrada");
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                AtualizarQuantidadeEstoque(transferenciaDAO);

                _transferenciaDAL.ValidarComandaTransferencia(transferenciaDAO);
                
                scope.Complete();
            }
        }

        private int InserirTransferencia(DAO.Transferencia transferenciaDAO)
        {
            return _transferenciaDAL.InserirTransferencia(transferenciaDAO);
        }

        private void InserirTransferenciaProduto(DAO.Produto produtoDAO)
        {
            _transferenciaDAL.InserirTransferenciaProduto(produtoDAO);
        }

        private void AtualizarQuantidadeEstoque(DAO.Transferencia transferenciaDAO)
        {
            _transferenciaDAL.AtualizarQuantidadeEstoque(transferenciaDAO);
        }
    }
}
