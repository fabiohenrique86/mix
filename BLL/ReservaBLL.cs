using System;
using System.Collections.Generic;
using DAL;
using DAO;

namespace BLL
{
    public class ReservaBLL
    {
        private DAL.ReservaDAL _reservaDAL;
        private ClienteBLL _clienteBLL;
        private PedidoBLL _pedidoBLL;
        private CancelamentoBLL _cancelamentoBLL;

        public ReservaBLL()
        {
            _reservaDAL = new DAL.ReservaDAL();
            _clienteBLL = new ClienteBLL();
            _pedidoBLL = new PedidoBLL();
            _cancelamentoBLL = new CancelamentoBLL();
        }

        public string Inserir(ReservaDAO reservaDao)
        {
            SeDataEntregaMaiorDoQueDataReserva(reservaDao.DataReserva, reservaDao.DataEntrega);

            SeLimiteDeEntregaGeralOuLimitePorLinhaNaoFoiAtingido(reservaDao);

            if (reservaDao.Cpf != null)
            {
                reservaDao.ClienteID = new int?(_clienteBLL.SeExisteClienteComCpfInformado(reservaDao.Cpf, reservaDao.SistemaID));
            }
            else if (reservaDao.Cnpj != null)
            {
                reservaDao.ClienteID = new int?(_clienteBLL.SeExisteClienteComCnpjInformado(reservaDao.Cnpj, reservaDao.SistemaID));
            }

            _pedidoBLL.SeNaoExistePedidoComIdInformado(reservaDao.ReservaID, reservaDao.SistemaID);

            SeNaoExisteReservaComIdInformado(reservaDao.ReservaID, reservaDao.SistemaID);

            _cancelamentoBLL.SeNaoExisteCancelamentoComIdInformado(reservaDao.ReservaID, reservaDao.SistemaID);

            _reservaDAL.Inserir(reservaDao);

            return reservaDao.ReservaID;
        }

        public void SeDataEntregaMaiorDoQueDataReserva(DateTime? dataReserva, DateTime? dataEntrega)
        {
            DateTime? dtEntrega = dataEntrega;
            DateTime? dtReserva = dataReserva;

            if (dtEntrega.HasValue && dtReserva.HasValue)
            {
                if (dtEntrega < dtReserva)
                {
                    throw new ApplicationException("A data da entrega deve ser maior ou igual do que a data do pedido.");
                }
            }
        }
        
        public void SeLimiteDeEntregaGeralOuLimitePorLinhaNaoFoiAtingido(ReservaDAO reservaDao)
        {
            int limiteReservaGeral = Convert.ToInt32(SistemaDAL.ListarLimiteReserva(reservaDao.SistemaID).Tables[0].Rows[0]["LimiteReserva"]);

            foreach (var produto in reservaDao.ListaProduto)
            {
                if (produto.NomeFantasia.Trim().ToLower().Equals("depósito"))
                {
                    var quantidadeDeReservas = _reservaDAL.ObterQuantidadeDeReservas(reservaDao.DataEntrega, reservaDao.SistemaID, produto.LojaID.GetValueOrDefault());

                    if (limiteReservaGeral > 0 && quantidadeDeReservas > limiteReservaGeral)
                    {
                        throw new ApplicationException("O limite geral de reservas para a data de entrega informada foi atingido.\r\n\r\nInforme outra data de entrega para inserir pedido.");
                    }

                    var quantidadeReservadaDoProdutoNaData = _reservaDAL.ObterQuantidadeReservadaDoProduto(reservaDao.DataEntrega, reservaDao.SistemaID, produto.LojaID.GetValueOrDefault(), produto.ProdutoID);

                    var limiteReservaLinha = LinhaDAL.ObterLimiteReservaLinha(produto.ProdutoID, produto.SistemaID);

                    if (limiteReservaLinha > 0 && quantidadeReservadaDoProdutoNaData > limiteReservaLinha)
                    {
                        throw new ApplicationException("O limite de reservas por linha para a data de entrega informada foi atingido.\r\n\r\nInforme outra data de entrega para inserir pedido ou diminua a quantidade.");
                    }
                }
            }
        }

        public void SeNaoExisteReservaComIdInformado(string reservaId, int sistemaId)
        {
            if (new DAL.ReservaDAL().Existe(reservaId, sistemaId))
            {
                throw new ApplicationException("Pedido reservado.\r\n\r\nInforme outro PedidoID a ser cadastrado.");
            }
        }

        public ReservaDAO Listar(string reservaId, int sistemaId)
        {
            ReservaDAO reservaDao = new ReservaDAO();

            reservaDao.Cliente = new ClienteDAO();
            reservaDao.ListaProduto = new List<ProdutoDAO>();

            // lista a reserva
            var dtReserva = _reservaDAL.ListarByLoja(string.Empty, sistemaId, reservaId).Tables[0].Rows[0];

            reservaDao.ReservaID = dtReserva["ReservaID"].ToString();
            //reservaDao.LojaDestinoNomeFantasia = dtReserva["NomeFantasia"].ToString();
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
            if (dtReserva["DataEntrega"] != DBNull.Value)
            {
                reservaDao.DataEntrega = Convert.ToDateTime(dtReserva["DataEntrega"]);
            }
            reservaDao.StatusID = Convert.ToInt32(dtReserva["StatusID"]);
            reservaDao.Observacao = dtReserva["Observacao"].ToString();

            // lista os produtos
            var dtReservaProduto = _reservaDAL.ListarReservaLoja(0, reservaId.ToString(), sistemaId, 0).Tables[0];

            foreach (System.Data.DataRow item in dtReservaProduto.Rows)
            {
                ProdutoDAO produtoDAO = new ProdutoDAO();

                produtoDAO.ProdutoID = Convert.ToInt64(item["ProdutoID"]);
                produtoDAO.Descricao = item["Produto"].ToString();
                produtoDAO.Quantidade = Convert.ToInt16(item["Quantidade"]);
                produtoDAO.Preco = Convert.ToDecimal(item["PrecoUnitario"]);

                if (item["LojaID"] != DBNull.Value)
                {
                    produtoDAO.LojaID = Convert.ToInt32(item["LojaID"]);
                }

                if (item["NomeFantasia"] != DBNull.Value)
                {
                    produtoDAO.NomeFantasia = item["NomeFantasia"].ToString();
                }

                reservaDao.ListaProduto.Add(produtoDAO);
            }

            return reservaDao;
        }
    }
}
