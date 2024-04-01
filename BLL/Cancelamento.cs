using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using DAO;

namespace BLL
{
    public class Cancelamento
    {
        // Methods
        public void SeNaoExisteCancelamentoComIdInformado(int? pedidoId, int sistemaId)
        {
            if (DAL.Cancelamento.PedidoCancelado(pedidoId, sistemaId).Tables[0].Rows.Count > 0)
            {
                throw new ApplicationException("Pedido cancelado.\r\n\r\nInforme outro PedidoID a ser cadastrado.");
            }
        }
    }
}
