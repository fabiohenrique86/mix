using DAO;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class PedidoMaeTrocaBLL
    {
        PedidoMaeTrocaDAL _pedidoMaeTrocaDAL;
        PedidoMaeTrocaProdutoDAL _pedidoMaeTrocaProdutoDAL;

        public PedidoMaeTrocaBLL()
        {
            _pedidoMaeTrocaDAL = new PedidoMaeTrocaDAL();
            _pedidoMaeTrocaProdutoDAL = new PedidoMaeTrocaProdutoDAL();
        }

        private void ValidarIncluir(PedidoMaeTrocaDAO pedidoMaeTrocaDao)
        {
            if (pedidoMaeTrocaDao == null)
            {
                throw new ApplicationException("Pedido Mãe Troca não informado");
            }

            if (string.IsNullOrEmpty(pedidoMaeTrocaDao.PedidoMaeID))
            {
                throw new ApplicationException("PedidoMãeID não informado");
            }

            if (pedidoMaeTrocaDao.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID não informado");
            }

            if (pedidoMaeTrocaDao.PedidoMaeTrocaProdutosDAO == null || pedidoMaeTrocaDao.PedidoMaeTrocaProdutosDAO.Count <= 0)
            {
                throw new ApplicationException("Produto não informado");
            }
        }

        public void Inserir(PedidoMaeTrocaDAO pedidoMaeTrocaDao)
        {
            ValidarIncluir(pedidoMaeTrocaDao);

            var pedidoMaeTrocaId = _pedidoMaeTrocaDAL.Inserir(pedidoMaeTrocaDao);

            foreach (var pedidoMaeTrocaProdutoDao in pedidoMaeTrocaDao.PedidoMaeTrocaProdutosDAO)
            {
                pedidoMaeTrocaProdutoDao.PedidoMaeTrocaID = pedidoMaeTrocaId;

                _pedidoMaeTrocaProdutoDAL.Inserir(pedidoMaeTrocaProdutoDao);
            }
        }

        public List<PedidoMaeTrocaDAO> Listar(PedidoMaeTrocaDAO pedidoMaeTrocaDao)
        {
            return _pedidoMaeTrocaDAL.Listar(pedidoMaeTrocaDao);
        }
    }
}
