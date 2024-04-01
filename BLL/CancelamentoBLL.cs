using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using DAO;

namespace BLL
{
    public class CancelamentoBLL
    {
        // Methods
        public void SeNaoExisteCancelamentoComIdInformado(string pedidoId, int sistemaId)
        {
            if (DAL.CancelamentoDAL.PedidoCancelado(pedidoId, sistemaId).Tables[0].Rows.Count > 0)
            {
                throw new ApplicationException("Pedido cancelado.\r\n\r\nInforme outro PedidoID a ser cadastrado.");
            }
        }
    }
}
