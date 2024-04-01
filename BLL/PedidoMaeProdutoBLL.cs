using DAO;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class PedidoMaeProdutoBLL
    {
        PedidoMaeProdutoDAL _pedidoMaeProdutoDAL;

        public PedidoMaeProdutoBLL()
        {
            _pedidoMaeProdutoDAL = new PedidoMaeProdutoDAL();
        }
        
        private void ValidarListar(PedidoMaeProdutoDAO pedidoMaeProdutoDao)
        {
            if (pedidoMaeProdutoDao == null)
            {
                throw new ApplicationException("Pedido Mãe não informado");
            }
            
            if (pedidoMaeProdutoDao.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID não informado");
            }
        }

        public List<PedidoMaeProdutoDAO> Listar(PedidoMaeProdutoDAO pedidoMaeProdutoDao)
        {
            ValidarListar(pedidoMaeProdutoDao);

            return _pedidoMaeProdutoDAL.Listar(pedidoMaeProdutoDao);
        }

        public void Inserir(PedidoMaeProdutoDAO pedidoMaeProdutoDao)
        {
            _pedidoMaeProdutoDAL.Inserir(pedidoMaeProdutoDao);
        }
    }
}
