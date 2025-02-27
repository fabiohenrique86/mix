﻿using DAO;
using System;
using System.Transactions;

namespace BLL
{
    public class TransferenciaBLL
    {
        DAL.TransferenciaDAL _transferenciaDAL;

        public TransferenciaBLL()
        {
            _transferenciaDAL = new DAL.TransferenciaDAL();
        }

        public int Inserir(TransferenciaDAO transferenciaDAO)
        {
            int transferenciaId;

            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(5) }))
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
                    _transferenciaDAL.AtualizarQuantidadeEstoque(transferenciaDAO, "V");
                }

                scope.Complete();
            }

            return transferenciaId;
        }

        private void ValidarValidarComandaTransferencia(TransferenciaDAO transferenciaDAO)
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

        public void ValidarComandaTransferencia(TransferenciaDAO transferenciaDAO)
        {
            ValidarValidarComandaTransferencia(transferenciaDAO);

            // Validar para ver se existe o transferenciaId
            var existeTransferencia = _transferenciaDAL.Listar(transferenciaDAO.SistemaID, false, transferenciaDAO.TransferenciaID).Tables[0].Rows.Count > 0 ? true : false;

            if (!existeTransferencia)
            {
                throw new ApplicationException("Comanda de transferência não cadastrada");
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(5) }))
            {
                _transferenciaDAL.AtualizarQuantidadeEstoque(transferenciaDAO, "V");

                _transferenciaDAL.ValidarComandaTransferencia(transferenciaDAO);

                scope.Complete();
            }
        }

        private int InserirTransferencia(TransferenciaDAO transferenciaDAO)
        {
            return _transferenciaDAL.InserirTransferencia(transferenciaDAO);
        }

        private void InserirTransferenciaProduto(ProdutoDAO produtoDAO)
        {
            _transferenciaDAL.InserirTransferenciaProduto(produtoDAO);
        }

        private void ValidarExcluir(TransferenciaDAO transferenciaDAO)
        {
            if (transferenciaDAO == null)
            {
                throw new ApplicationException("Transferência não informada");
            }

            if (transferenciaDAO.TransferenciaID <= 0)
            {
                throw new ApplicationException("TransferênciaID é obrigatório");
            }

            if (transferenciaDAO.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID é obrigatório");
            }
        }

        private void ValidarReabrir(TransferenciaDAO transferenciaDAO)
        {
            if (transferenciaDAO == null)
            {
                throw new ApplicationException("Transferência não informada");
            }

            if (transferenciaDAO.TransferenciaID <= 0)
            {
                throw new ApplicationException("TransferênciaID é obrigatório");
            }
            
            if (transferenciaDAO.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID é obrigatório");
            }
        }

        public void Excluir(TransferenciaDAO transferenciaDAO)
        {
            ValidarExcluir(transferenciaDAO);
            
            // Validar para ver se existe o transferenciaId
            var existeTransferencia = _transferenciaDAL.Listar(transferenciaDAO.SistemaID, null, transferenciaDAO.TransferenciaID).Tables[0].Rows.Count > 0 ? true : false;

            if (!existeTransferencia)
            {
                throw new ApplicationException("Comanda de transferência não cadastrada");
            }

            _transferenciaDAL.Excluir(transferenciaDAO);
        }

        public void Reabrir(TransferenciaDAO transferenciaDAO)
        {
            ValidarReabrir(transferenciaDAO);

            // Validar para ver se existe o transferenciaId
            var existeTransferencia = _transferenciaDAL.Listar(transferenciaDAO.SistemaID, null, transferenciaDAO.TransferenciaID).Tables[0].Rows.Count > 0 ? true : false;

            if (!existeTransferencia)
            {
                throw new ApplicationException("Comanda de transferência não cadastrada");
            }
            
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(5) }))
            {
                _transferenciaDAL.AtualizarQuantidadeEstoque(transferenciaDAO, "R");

                _transferenciaDAL.Reabrir(transferenciaDAO);

                scope.Complete();
            }
        }
    }
}
