using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO;

namespace BLL
{
    public class PedidoBLL
    {
        // Methods
        public void Inserir(DAO.PedidoDAO pedido)
        {
            ClienteBLL ClienteBLL = new ClienteBLL();
            ReservaBLL ReservaBLL = new ReservaBLL();
            CancelamentoBLL CancelamentoBLL = new CancelamentoBLL();
            this.SeDataPedidoIgualAHoje(pedido.DataPedido);
            this.SeNaoExistePedidoComIdInformado(pedido.PedidoID, pedido.SistemaID);
            ReservaBLL.SeNaoExisteReservaComIdInformado(pedido.PedidoID, pedido.SistemaID);
            CancelamentoBLL.SeNaoExisteCancelamentoComIdInformado(pedido.PedidoID, pedido.SistemaID);
            if (pedido.Cpf != null)
            {
                pedido.ClienteID = new int?(ClienteBLL.SeExisteClienteComCpfInformado(pedido.Cpf, pedido.SistemaID));
            }
            else if (pedido.Cnpj != null)
            {
                pedido.ClienteID = new int?(ClienteBLL.SeExisteClienteComCnpjInformado(pedido.Cnpj, pedido.SistemaID));
            }
            new DAL.PedidoDAL().Inserir(pedido);
        }

        public void SeDataPedidoIgualAHoje(DateTime? dataPedido)
        {
            if (!dataPedido.Equals(DateTime.Today))
            {
                throw new ApplicationException("A data do Pedido deve ser a data de hoje.");
            }
        }

        public void SeNaoExistePedidoComIdInformado(string pedidoId, int sistemaId)
        {
            if (new DAL.PedidoDAL().Existe(pedidoId, sistemaId))
            {
                throw new ApplicationException("Pedido cadastrado.\r\n\r\nInforme outro PedidoID a ser cadastrado.");
            }
        }
    }
}
