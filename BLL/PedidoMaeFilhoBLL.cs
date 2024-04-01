using DAO;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class PedidoMaeFilhoBLL
    {
        PedidoMaeFilhoDAL _pedidoMaeFilhoDAL;

        public PedidoMaeFilhoBLL()
        {
            _pedidoMaeFilhoDAL = new PedidoMaeFilhoDAL();
        }

        private void ValidarIncluir(PedidoMaeFilhoDAO pedidoMaeFilhoDAO)
        {
            if (pedidoMaeFilhoDAO == null)
            {
                throw new ApplicationException("Pedido Mãe Filho não informado");
            }

            if (string.IsNullOrEmpty(pedidoMaeFilhoDAO.PedidoMaeID))
            {
                throw new ApplicationException("PedidoMaeID não informado");
            }

            if (string.IsNullOrEmpty(pedidoMaeFilhoDAO.PedidoID))
            {
                throw new ApplicationException("PedidoID não informada");
            }

            if (pedidoMaeFilhoDAO.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID não informado");
            }
        }
        
        public long Inserir(PedidoMaeFilhoDAO pedidoMaeFilhoDAO)
        {
            ValidarIncluir(pedidoMaeFilhoDAO);

            return _pedidoMaeFilhoDAL.Inserir(pedidoMaeFilhoDAO);
        }

        public List<PedidoMaeFilhoDAO> Listar(PedidoMaeFilhoDAO pedidoMaeFilhoDAO)
        {
            return _pedidoMaeFilhoDAL.Listar(pedidoMaeFilhoDAO);
        }
    }
}
