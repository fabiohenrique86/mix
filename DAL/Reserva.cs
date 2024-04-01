using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using DAO;
using System.Transactions;

namespace DAL
{
    public class Reserva
    {
        public void Atualizar(int pedidoId, string dataEntrega, bool dataEntregaAProgramar)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarReserva"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, pedidoId);
                    db.AddInParameter(cmd, "@DataEntrega", DbType.Date, dataEntrega);
                    db.AddInParameter(cmd, "@DataEntregaAProgramar", DbType.Int16, dataEntregaAProgramar);
                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao inserir registro", ex);
            }
            finally
            {
                db = null;
            }
        }

        public void Atualizar(int reservaId, string produtoIId, string quantidadeI, string medidaI, string preco, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarReserva3"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(produtoIId))
                    {
                        produtoIId = null;
                    }
                    if (string.IsNullOrEmpty(quantidadeI))
                    {
                        quantidadeI = null;
                    }
                    if (string.IsNullOrEmpty(preco) || (preco == "0,00"))
                    {
                        preco = null;
                    }
                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reservaId);
                    db.AddInParameter(cmd, "@ProdutoIID", DbType.Int64, Convert.ToInt64(produtoIId));
                    db.AddInParameter(cmd, "@QuantidadeI", DbType.Int16, quantidadeI);
                    db.AddInParameter(cmd, "@MedidaI", DbType.String, medidaI);
                    db.AddInParameter(cmd, "@Preco", DbType.Decimal, preco);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao atualizar registro", ex);
            }
            finally
            {
                db = null;
            }
        }

        public void Atualizar(int reservaId, string produtoEId, string quantidadeE, string medidaE, string produtoIId, string quantidadeI, string medidaI, string preco, string observacao, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarReserva2"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(produtoEId) || (produtoEId == "0"))
                    {
                        produtoEId = null;
                    }
                    if (string.IsNullOrEmpty(quantidadeE) || (quantidadeE == "0"))
                    {
                        quantidadeE = null;
                    }
                    if (string.IsNullOrEmpty(produtoIId) || (produtoIId == "0"))
                    {
                        produtoIId = null;
                    }
                    if (string.IsNullOrEmpty(quantidadeI) || (quantidadeI == "0"))
                    {
                        quantidadeI = null;
                    }
                    if (string.IsNullOrEmpty(preco) || (preco == "0,00"))
                    {
                        preco = null;
                    }
                    if (string.IsNullOrEmpty(observacao))
                    {
                        observacao = null;
                    }
                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reservaId);
                    db.AddInParameter(cmd, "@ProdutoEID", DbType.Int64, Convert.ToInt64(produtoEId));
                    db.AddInParameter(cmd, "@QuantidadeE", DbType.Int16, quantidadeE);
                    db.AddInParameter(cmd, "@MedidaE", DbType.String, medidaE);
                    db.AddInParameter(cmd, "@ProdutoIID", DbType.Int64, Convert.ToInt64(produtoIId));
                    db.AddInParameter(cmd, "@QuantidadeI", DbType.Int16, quantidadeI);
                    db.AddInParameter(cmd, "@MedidaI", DbType.String, medidaI);
                    db.AddInParameter(cmd, "@Preco", DbType.Decimal, preco);
                    db.AddInParameter(cmd, "@Observacao", DbType.String, observacao);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao atualizar registro", ex);
            }
            finally
            {
                db = null;
            }
        }

        public void AtualizarStatusImprimido(int pedidoId, int sistemaId, string pedidoOuReserva)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarStatusImprimido"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, pedidoId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@PedidoOuReserva", DbType.String, pedidoOuReserva);
                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao atualizar registro", ex);
            }
            finally
            {
                db = null;
            }
        }

        public void Excluir(int pedidoId, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirReserva"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, pedidoId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao excluir registro", ex);
            }
            finally
            {
                db = null;
            }
        }

        public void Excluir(int reservaId, int tipoPagamentoId, int parcelaId, decimal valor, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirReservaTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reservaId);
                    db.AddInParameter(cmd, "@TipoPagamentoID", DbType.Int32, tipoPagamentoId);
                    db.AddInParameter(cmd, "@ParcelaID", DbType.Int32, parcelaId);
                    db.AddInParameter(cmd, "@Valor", DbType.Int32, valor);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao excluir registro", ex);
            }
            finally
            {
                db = null;
            }
        }

        public bool Existe(int? reservaId, int sistemaId)
        {
            Database db;
            bool reserva = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reservaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        reserva = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return reserva;
        }

        public bool Existe(string reservaId, int sistemaId)
        {
            Database db;
            bool reserva = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, Convert.ToInt32(reservaId));
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        reserva = true;
                    }
                }
                //return reserva;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return reserva;
        }

        public void Inserir(DAO.Reserva reserva)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.Timeout = TimeSpan.FromMinutes(5);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    DbCommand cmd;
                    using (cmd = db.GetStoredProcCommand("dbo.spInserirReserva"))
                    {
                        cmd.CommandTimeout = 300;

                        db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reserva.ReservaID);
                        db.AddInParameter(cmd, "@LojaOrigemID", DbType.Int32, reserva.LojaOrigemID);
                        db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, reserva.FuncionarioID);
                        db.AddInParameter(cmd, "@ClienteID", DbType.Int32, reserva.ClienteID);
                        db.AddInParameter(cmd, "@DataReserva", DbType.Date, reserva.DataReserva);
                        db.AddInParameter(cmd, "@DataEntrega", DbType.Date, reserva.DataEntrega);
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, reserva.SistemaID);
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, reserva.LojaID);
                        db.AddInParameter(cmd, "@Observacao", DbType.String, reserva.Observacao);
                        db.AddInParameter(cmd, "@StatusID", DbType.Int32, reserva.StatusID);
                        db.AddInParameter(cmd, "@ValorFrete", DbType.Decimal, reserva.ValorFrete);
                        db.AddInParameter(cmd, "@CV", DbType.Int64, reserva.CV);

                        db.ExecuteNonQuery(cmd);
                    }

                    using (cmd = db.GetStoredProcCommand("dbo.spInserirReservaProduto"))
                    {
                        cmd.CommandTimeout = 300;
                        foreach (DAO.Produto produto in reserva.ListaProduto)
                        {
                            cmd.Parameters.Clear();
                            db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reserva.ReservaID);
                            db.AddInParameter(cmd, "@LojaID", DbType.Int32, reserva.LojaID);
                            db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produto.ProdutoID);
                            db.AddInParameter(cmd, "@Quantidade", DbType.Int16, produto.Quantidade);
                            db.AddInParameter(cmd, "@Medida", DbType.String, produto.Medida);
                            db.AddInParameter(cmd, "@Preco", DbType.Decimal, produto.Preco);
                            db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produto.SistemaID);
                            db.ExecuteNonQuery(cmd);
                        }
                    }
                    using (cmd = db.GetStoredProcCommand("dbo.spInserirReservaTipoPagamento"))
                    {
                        cmd.CommandTimeout = 300;
                        foreach (DAO.TipoPagamento tipoPagamento in reserva.ListaTipoPagamento)
                        {
                            cmd.Parameters.Clear();
                            db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reserva.ReservaID);
                            db.AddInParameter(cmd, "@TipoPagamentoID", DbType.Int32, tipoPagamento.TipoPagamentoID);
                            db.AddInParameter(cmd, "@ParcelaID", DbType.Int32, tipoPagamento.ParcelaID);
                            db.AddInParameter(cmd, "@Valor", DbType.Int32, tipoPagamento.Valor);
                            db.AddInParameter(cmd, "@SistemaID", DbType.Int32, tipoPagamento.SistemaID);
                            db.ExecuteNonQuery(cmd);
                        }
                    }
                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao inserir registro", ex);
            }
            finally
            {
                db = null;
            }
        }

        public void Inserir(int pedidoId, int sistemaId, string nomeCarreteiro)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirBaixaReserva"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, Convert.ToInt32(pedidoId));
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@NomeCarreteiro", DbType.String, nomeCarreteiro);

                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao inserir registro", ex);
            }
            finally
            {
                db = null;
            }
        }

        public void Inserir(int pedidoId, int tipoPagamentoId, string parcelaId, string valor, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirReservaTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, pedidoId);
                    db.AddInParameter(cmd, "@TipoPagamentoID", DbType.Int32, tipoPagamentoId);
                    db.AddInParameter(cmd, "@ParcelaID", DbType.Int32, parcelaId);
                    db.AddInParameter(cmd, "@ValorPago", DbType.Decimal, valor);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao inserir registro", ex);
            }
            finally
            {
                db = null;
            }
        }

        public void Inserir(int orcamentoId, int pedidoId, int clienteId, string dataReserva, string dataEntrega, string observacao, int statusId, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirReservaDoOrcamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@OrcamentoID", DbType.Int32, orcamentoId);
                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, pedidoId);
                    db.AddInParameter(cmd, "@ClienteID", DbType.Int32, clienteId);
                    db.AddInParameter(cmd, "@DataReserva", DbType.Date, dataReserva);
                    db.AddInParameter(cmd, "@DataEntrega", DbType.Date, dataEntrega);
                    db.AddInParameter(cmd, "@Observacao", DbType.String, observacao);
                    db.AddInParameter(cmd, "@StatusID", DbType.Int32, statusId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao inserir registro", ex);
            }
            finally
            {
                db = null;
            }
        }

        public void InserirTransito(int pedidoId, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirTransitoReserva"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, Convert.ToInt32(pedidoId));
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao inserir registro", ex);
            }
            finally
            {
                db = null;
            }
        }

        public DataSet Listar(int reservaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarDataStatusReserva"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reservaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    ds = db.ExecuteDataSet(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return ds;
        }

        public DataSet Listar(int? lojaId, string pedidoId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaLoja"))
                {
                    cmd.CommandTimeout = 300;
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, Convert.ToInt32(pedidoId));
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    ds = db.ExecuteDataSet(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return ds;
        }

        public bool Listar(int reservaId, long produtoId, string medida, int sistemaId)
        {
            Database db;
            bool reservaProduto = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaProduto"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reservaId);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
                    db.AddInParameter(cmd, "@Medida", DbType.String, medida);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        reservaProduto = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return reservaProduto;
        }

        public bool Listar(string reservaId, string produtoId, string medida, string quantidadeE, int sistemaId)
        {
            Database db;
            bool quantidadeProduto = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaQuantidadeProduto"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, Convert.ToInt32(reservaId));
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, Convert.ToInt64(produtoId));
                    db.AddInParameter(cmd, "@Medida", DbType.String, medida);
                    db.AddInParameter(cmd, "@QuantidadeE", DbType.Int16, quantidadeE);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        quantidadeProduto = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return quantidadeProduto;
        }

        public DataSet Listar(int pedidoId, int? lojaId, int? lojaOrigemId, int? funcionarioId, string cpf, string cnpj, DateTime? dataReserva, DateTime? dataEntrega, bool dataEntregaAProgramar, int sistemaId, List<DAO.Produto> listaProduto = null)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaLojaFiltro"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, pedidoId);
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionarioId);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@LojaOrigemID", DbType.Int32, lojaOrigemId);
                    db.AddInParameter(cmd, "@CPF", DbType.String, cpf);
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
                    db.AddInParameter(cmd, "@DataReserva", DbType.Date, dataReserva);
                    db.AddInParameter(cmd, "@DataEntrega", DbType.Date, dataEntrega);
                    db.AddInParameter(cmd, "@DataEntregaAProgramar", DbType.Int16, dataEntregaAProgramar);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

                    if (listaProduto != null && listaProduto.Count() > 0)
                    {
                        string listProduto = string.Empty;
                        foreach (var item in listaProduto)
                        {
                            listProduto += string.Format("{0},", item.ProdutoID.ToString());
                        }
                        listProduto = listProduto.Substring(0, listProduto.Length - 1);
                        db.AddInParameter(cmd, "@ProdutoIDList", DbType.String, listProduto);
                    }

                    ds = db.ExecuteDataSet(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return ds;
        }

        public DataSet ListarAgendada(string lojaId, int sistemaId, bool top)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaAgendada"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@TOP", DbType.Boolean, top);
                    ds = db.ExecuteDataSet(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return ds;
        }

        public DataSet ListarAgendada(string pedidoId, string dataReserva, string dataEntrega, int sistemaId, bool dataEntregaAProgramar, string statusId, string nomeCarreteiro)
        {
            Database db;
            DataSet ds;
            try
            {
                bool statusEfetuada = false;
                bool statusPendenteAntesHoje = false;
                bool statusPendenteHoje = false;
                bool statusPendenteTransito = false;
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaAgendadaFiltro"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(pedidoId))
                    {
                        pedidoId = null;
                    }
                    if (string.IsNullOrEmpty(dataReserva) || (dataReserva == "__/__/____"))
                    {
                        dataReserva = null;
                    }
                    if (string.IsNullOrEmpty(dataEntrega) || (dataEntrega == "__/__/____"))
                    {
                        dataEntrega = null;
                    }
                    if (!string.IsNullOrEmpty(statusId) && (statusId != "0"))
                    {
                        if (statusId == "1")
                        {
                            statusPendenteAntesHoje = true;
                        }
                        else if (statusId == "2")
                        {
                            statusPendenteHoje = true;
                        }
                        else if (statusId == "3")
                        {
                            statusEfetuada = true;
                        }
                        else if (statusId == "4")
                        {
                            statusPendenteTransito = true;
                        }
                    }

                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, pedidoId);
                    db.AddInParameter(cmd, "@DataReserva", DbType.Date, dataReserva);
                    db.AddInParameter(cmd, "@DataEntrega", DbType.Date, dataEntrega);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@DataEntregaAProgramar", DbType.Boolean, dataEntregaAProgramar);
                    db.AddInParameter(cmd, "@StatusPendenteAntesHoje", DbType.Boolean, statusPendenteAntesHoje);
                    db.AddInParameter(cmd, "@StatusPendenteHoje", DbType.Boolean, statusPendenteHoje);
                    db.AddInParameter(cmd, "@StatusEfetuada", DbType.Boolean, statusEfetuada);
                    db.AddInParameter(cmd, "@StatusPendenteTransito", DbType.Boolean, statusPendenteTransito);
                    db.AddInParameter(cmd, "@NomeCarreteiro", DbType.String, nomeCarreteiro);

                    ds = db.ExecuteDataSet(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return ds;
        }

        public DataSet ListarByFiltro(DAO.Reserva reserva)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaByFiltro"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reserva.ReservaID);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, reserva.LojaID);
                    db.AddInParameter(cmd, "@LojaOrigemID", DbType.Int32, reserva.LojaOrigemID);
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, reserva.FuncionarioID);
                    db.AddInParameter(cmd, "@CPF", DbType.String, reserva.Cpf);
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, reserva.Cnpj);
                    db.AddInParameter(cmd, "@DataReserva", DbType.Date, reserva.DataReserva);
                    db.AddInParameter(cmd, "@DataEntrega", DbType.Date, reserva.DataEntrega);
                    db.AddInParameter(cmd, "@SemDataEntrega", DbType.Boolean, reserva.SemDataEntrega);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, reserva.SistemaID);

                    if (reserva.StatusID > 0)
                    {
                        db.AddInParameter(cmd, "@StatusID", DbType.Int32, reserva.StatusID);
                    }

                    ds = db.ExecuteDataSet(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return ds;
        }

        public DataSet ListarByLoja(string lojaId, int sistemaId, bool top, int reservaId = 0)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReserva"))
                {
                    cmd.CommandTimeout = 300;

                    if (string.IsNullOrEmpty(lojaId) || (lojaId == "0"))
                    {
                        lojaId = null;
                    }

                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@TOP", DbType.Boolean, top);

                    if (reservaId > 0)
                    {
                        db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reservaId);
                    }

                    ds = db.ExecuteDataSet(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return ds;
        }

        public DataTable ListarComanda(int reservaId, int sistemaId, string pedidoOuReserva)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarComandaEntrega"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, reservaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@PedidoOuReserva", DbType.String, pedidoOuReserva);
                    dt = db.ExecuteDataSet(cmd).Tables[0];
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return dt;
        }

        public bool ListarTipoPagamento(int reservaId, int tipoPagamentoId, int sistemaId)
        {
            Database db;
            bool tipoPagamento = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reservaId);
                    db.AddInParameter(cmd, "@TipoPagamentoID", DbType.Int32, tipoPagamentoId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        tipoPagamento = true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return tipoPagamento;
        }

        public DataSet ListarValorReserva(int reservaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarValorReserva"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reservaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    ds = db.ExecuteDataSet(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return ds;
        }

        public DataSet ListarValorReservaAlterado(int reservaId, decimal totalPedido, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarValorReservaAlterado"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.Int32, reservaId);
                    db.AddInParameter(cmd, "@TotalPedido", DbType.Decimal, totalPedido);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    ds = db.ExecuteDataSet(cmd);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return ds;
        }

        public int QuantidadeReserva(DateTime? dataEntrega, int sistemaId)
        {
            Database db;
            int retorno;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarQuantidadeReserva"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@DataEntrega", DbType.Date, dataEntrega);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    retorno = Convert.ToInt32(db.ExecuteScalar(cmd));
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return retorno;
        }
    }
}
