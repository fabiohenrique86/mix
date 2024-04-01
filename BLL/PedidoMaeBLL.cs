using DAO;
using DAL;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace BLL
{
    public class PedidoMaeBLL
    {
        PedidoMaeDAL _pedidoMaeDAL;
        PedidoMaeProdutoDAL _pedidoMaeProdutoDAL;

        public PedidoMaeBLL()
        {
            _pedidoMaeDAL = new PedidoMaeDAL();
            _pedidoMaeProdutoDAL = new PedidoMaeProdutoDAL();
        }

        private void ValidarIncluir(PedidoMaeDAO pedidoMaeDao, bool inserirProdutos)
        {
            if (pedidoMaeDao == null)
            {
                throw new ApplicationException("Pedido Mãe não informado");
            }

            if (string.IsNullOrEmpty(pedidoMaeDao.PedidoMaeID))
            {
                throw new ApplicationException("PedidoMãeID não informado");
            }

            if (pedidoMaeDao.DataCadastro == DateTime.MinValue)
            {
                throw new ApplicationException("Data Cadastro não informada");
            }

            if (pedidoMaeDao.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID não informado");
            }

            if (inserirProdutos)
            {
                if (pedidoMaeDao.PedidoMaeProdutoDAO == null || pedidoMaeDao.PedidoMaeProdutoDAO.Count <= 0)
                {
                    throw new ApplicationException("Produto não informado");
                }
            }

            //var pedidosMaeDao = Listar(new PedidoMaeDAO() { PedidoMaeID = pedidoMaeDao.PedidoMaeID, SistemaID = pedidoMaeDao.SistemaID });

            //if (pedidosMaeDao != null && pedidosMaeDao.Count > 0)
            //{
            //    throw new ApplicationException(string.Format("PedidoMaeID {0} já cadastrado", pedidoMaeDao.PedidoMaeID));
            //}
        }

        private void ValidarExcluir(PedidoMaeDAO pedidoMaeDao)
        {
            if (pedidoMaeDao == null)
            {
                throw new ApplicationException("Pedido Mãe não informado");
            }

            if (string.IsNullOrEmpty(pedidoMaeDao.PedidoMaeID))
            {
                throw new ApplicationException("PedidoMãeID não informado");
            }

            if (pedidoMaeDao.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID não informado");
            }

            var pedidosMaeDao = ListarByPedido(new PedidoMaeDAO() { PedidoMaeID = pedidoMaeDao.PedidoMaeID, SistemaID = pedidoMaeDao.SistemaID });

            if (pedidosMaeDao == null || pedidosMaeDao.Count <= 0)
            {
                throw new ApplicationException("PedidoMaeID inexistente");
            }
        }

        private void ValidarListar(PedidoMaeDAO pedidoMaeDao)
        {
            if (pedidoMaeDao == null)
            {
                throw new ApplicationException("Pedido Mãe não informado");
            }

            if (pedidoMaeDao.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID não informado");
            }
        }

        private void ValidarListarByPedido(PedidoMaeDAO pedidoMaeDao)
        {
            if (pedidoMaeDao == null)
            {
                throw new ApplicationException("Pedido Mãe não informado");
            }

            if (string.IsNullOrEmpty(pedidoMaeDao.PedidoMaeID))
            {
                throw new ApplicationException("PedidoMãeID não informado");
            }

            if (pedidoMaeDao.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID não informado");
            }
        }

        public void Inserir(PedidoMaeDAO pedidoMaeDao, bool inserirProdutos)
        {
            ValidarIncluir(pedidoMaeDao, inserirProdutos);

            _pedidoMaeDAL.Inserir(pedidoMaeDao);

            if (inserirProdutos)
            {
                foreach (var pedidoMaeProdutoDao in pedidoMaeDao.PedidoMaeProdutoDAO)
                {
                    _pedidoMaeProdutoDAL.Inserir(pedidoMaeProdutoDao);
                }
            }
        }

        public void Excluir(PedidoMaeDAO pedidoMaeDao)
        {
            ValidarExcluir(pedidoMaeDao);

            _pedidoMaeDAL.Excluir(pedidoMaeDao);
        }

        public List<PedidoMaeDAO> Listar(PedidoMaeDAO pedidoMaeDao)
        {
            ValidarListar(pedidoMaeDao);

            return _pedidoMaeDAL.Listar(pedidoMaeDao);
        }

        public List<PedidoMaeDAO> ListarByPedido(PedidoMaeDAO pedidoMaeDao)
        {
            ValidarListarByPedido(pedidoMaeDao);

            return _pedidoMaeDAL.ListarByPedido(pedidoMaeDao);
        }
    }
}