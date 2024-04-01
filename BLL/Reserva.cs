using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Reserva
    {
        private DAL.Reserva _reservaDAL;

        public Reserva()
        {
            _reservaDAL = new DAL.Reserva();
        }

        // Methods
        public void Inserir(DAO.Reserva reserva)
        {
            Cliente ClienteBLL = new Cliente();
            Pedido PedidoBLL = new Pedido();
            Cancelamento CancelamentoBLL = new Cancelamento();
            this.SeDataReservaIgualAHoje(reserva.DataReserva);
            this.SeDataEntregaMaiorDoQueDataReserva(reserva.DataReserva, reserva.DataEntrega);
            this.SeLimiteDeEntregaNaoFoiAtingido(reserva.DataEntrega, reserva.SistemaID);
            if (reserva.Cpf != null)
            {
                reserva.ClienteID = new int?(ClienteBLL.SeExisteClienteComCpfInformado(reserva.Cpf, reserva.SistemaID));
            }
            else if (reserva.Cnpj != null)
            {
                reserva.ClienteID = new int?(ClienteBLL.SeExisteClienteComCnpjInformado(reserva.Cnpj, reserva.SistemaID));
            }
            PedidoBLL.SeNaoExistePedidoComIdInformado(reserva.ReservaID, reserva.SistemaID);
            this.SeNaoExisteReservaComIdInformado(reserva.ReservaID, reserva.SistemaID);
            CancelamentoBLL.SeNaoExisteCancelamentoComIdInformado(reserva.ReservaID, reserva.SistemaID);
            _reservaDAL.Inserir(reserva);
        }

        public void SeDataEntregaMaiorDoQueDataReserva(DateTime? dataReserva, DateTime? dataEntrega)
        {
            DateTime? dtEntrega = dataEntrega;
            DateTime? dtReserva = dataReserva;
            if ((dtEntrega.HasValue && dtReserva.HasValue) ? (dtEntrega.GetValueOrDefault() <= dtReserva.GetValueOrDefault()) : false)
            {
                throw new ApplicationException("A data da Entrega deve ser maior do que a data da reserva.");
            }
        }

        public void SeDataReservaIgualAHoje(DateTime? dataReserva)
        {
            if (!dataReserva.Equals(DateTime.Today))
            {
                throw new ApplicationException("A data do Reserva deve ser a data de hoje.");
            }
        }

        public void SeLimiteDeEntregaNaoFoiAtingido(DateTime? dataEntrega, int sistemaId)
        {
            int limiteReserva = Convert.ToInt32(DAL.Sistema.ListarLimiteReserva(sistemaId).Tables[0].Rows[0]["LimiteReserva"]);
            if (_reservaDAL.QuantidadeReserva(dataEntrega, sistemaId) >= limiteReserva)
            {
                throw new ApplicationException("O limite de reservas para a data de entrega informada foi atingido.\r\n\r\nInforme outra data de entrega para inserir reserva.");
            }
        }

        public void SeNaoExisteReservaComIdInformado(int? reservaId, int sistemaId)
        {
            if (new DAL.Reserva().Existe(reservaId, sistemaId))
            {
                throw new ApplicationException("Pedido reservado.\r\n\r\nInforme outro PedidoID a ser cadastrado.");
            }
        }

        public DAO.Reserva Listar(int reservaId, int sistemaId)
        {
            DAO.Reserva reservaDao = new DAO.Reserva();

            reservaDao.Cliente = new DAO.Cliente();
            reservaDao.ListaProduto = new List<DAO.Produto>();

            // lista a reserva
            var dtReserva = _reservaDAL.ListarByLoja(string.Empty, sistemaId, true, reservaId).Tables[0].Rows[0];

            reservaDao.ReservaID = Convert.ToInt32(dtReserva["ReservaID"]);
            reservaDao.LojaDestinoNomeFantasia = dtReserva["NomeFantasia"].ToString();
            reservaDao.LojaOrigemNomeFantasia = dtReserva["LojaOrigem"].ToString();
            reservaDao.FuncionarioNome = dtReserva["Funcionario"].ToString();
            reservaDao.Cliente.Nome = dtReserva["Cliente"].ToString();
            reservaDao.Cliente.TelefoneResidencial = dtReserva["TelefoneResidencial"].ToString();
            reservaDao.Cliente.TelefoneResidencial2 = dtReserva["TelefoneResidencial2"].ToString();
            reservaDao.Cliente.TelefoneCelular = dtReserva["TelefoneCelular"].ToString();
            reservaDao.Cliente.TelefoneCelular2 = dtReserva["TelefoneCelular2"].ToString();
            reservaDao.Cliente.Cpf = dtReserva["Cpf"].ToString();
            reservaDao.Cliente.Endereco = dtReserva["Endereco"].ToString();
            // reservaDao.Cliente.Numero = dtReserva["Numero"].ToString();
            // reservaDao.Cliente.Complemento = dtReserva["PontoReferencia"].ToString();
            reservaDao.Cliente.Referencia = dtReserva["PontoReferencia"].ToString();
            reservaDao.Cliente.Bairro = dtReserva["Bairro"].ToString();
            reservaDao.Cliente.Cidade = dtReserva["Cidade"].ToString();
            reservaDao.DataReserva = Convert.ToDateTime(dtReserva["DataReserva"]);
            reservaDao.DataEntrega = Convert.ToDateTime(dtReserva["DataEntrega"]);
            reservaDao.StatusID = Convert.ToInt32(dtReserva["StatusID"]);
            reservaDao.Observacao = dtReserva["Observacao"].ToString();

            // lista os produtos
            var dtReservaProduto = _reservaDAL.Listar(0, reservaId.ToString(), sistemaId).Tables[0];

            foreach (System.Data.DataRow item in dtReservaProduto.Rows)
            {
                DAO.Produto produtoDAO = new DAO.Produto();

                produtoDAO.ProdutoID = Convert.ToInt64(item["ProdutoID"]);
                produtoDAO.Descricao = item["Produto"].ToString();
                produtoDAO.Quantidade = Convert.ToInt16(item["Quantidade"]);
                produtoDAO.Preco = Convert.ToDecimal(item["PrecoUnitario"]);

                reservaDao.ListaProduto.Add(produtoDAO);
            }

            return reservaDao;
        }
    }
}
