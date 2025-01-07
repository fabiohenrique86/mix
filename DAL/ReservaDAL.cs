using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using DAO;
using System.Transactions;

namespace DAL
{
    public class ReservaDAL
    {
        public void Atualizar(string pedidoId, DateTime dataEntrega, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarPedidoAgendado" + sistemaId))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);

                    if (dataEntrega != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataEntrega", DbType.Date, dataEntrega);
                    }

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

        public void Atualizar(string reservaId, string produtoIId, string quantidadeI, string medidaI, string preco, int sistemaId)
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
                    db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
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

        public void Atualizar(string reservaId, string produtoEId, string quantidadeE, string medidaE, string produtoIId, string quantidadeI, string medidaI, string preco, string observacao, int sistemaId)
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
                    db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
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

        public void AtualizarStatusImprimido(string pedidoId, int sistemaId, string pedidoOuReserva)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarStatusImprimido"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public void Excluir(string pedidoId, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirReserva"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public void Excluir(string reservaId, int tipoPagamentoId, int parcelaId, decimal valor, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirReservaTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
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
                    db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
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

        public void Inserir(ReservaDAO reserva)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(5) }))
                {
                    DbCommand cmd;
                    using (cmd = db.GetStoredProcCommand("dbo.spInserirReserva"))
                    {
                        cmd.CommandTimeout = 300;

                        db.AddInParameter(cmd, "@ReservaID", DbType.String, reserva.ReservaID);
                        db.AddInParameter(cmd, "@LojaOrigemID", DbType.Int32, reserva.LojaOrigemID);
                        db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, reserva.FuncionarioID);
                        db.AddInParameter(cmd, "@ClienteID", DbType.Int32, reserva.ClienteID);
                        db.AddInParameter(cmd, "@DataReserva", DbType.Date, reserva.DataReserva);
                        db.AddInParameter(cmd, "@DataEntrega", DbType.Date, reserva.DataEntrega == DateTime.MinValue ? null : reserva.DataEntrega);
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, reserva.SistemaID);
                        db.AddInParameter(cmd, "@Observacao", DbType.String, reserva.Observacao);
                        db.AddInParameter(cmd, "@StatusID", DbType.Int32, reserva.StatusID);
                        db.AddInParameter(cmd, "@ValorFrete", DbType.Decimal, reserva.ValorFrete);
                        db.AddInParameter(cmd, "@CV", DbType.Int64, reserva.CV);

                        if (!string.IsNullOrEmpty(reserva.NomeCarreteiro))
                        {
                            db.AddInParameter(cmd, "@NomeCarreteiro", DbType.String, reserva.NomeCarreteiro);
                        }

                        db.ExecuteNonQuery(cmd);
                    }

                    using (cmd = db.GetStoredProcCommand("dbo.spInserirReservaProduto" + reserva.SistemaID))
                    {
                        cmd.CommandTimeout = 300;
                        foreach (ProdutoDAO produto in reserva.ListaProduto)
                        {
                            cmd.Parameters.Clear();

                            db.AddInParameter(cmd, "@ReservaID", DbType.String, reserva.ReservaID);
                            db.AddInParameter(cmd, "@LojaID", DbType.Int32, produto.LojaID);
                            db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produto.ProdutoID);
                            db.AddInParameter(cmd, "@Quantidade", DbType.Int16, produto.Quantidade);
                            db.AddInParameter(cmd, "@Medida", DbType.String, produto.Medida);
                            db.AddInParameter(cmd, "@Preco", DbType.Decimal, produto.Preco);
                            db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produto.SistemaID);
                            db.AddInParameter(cmd, "@DataReserva", DbType.Date, reserva.DataReserva.GetValueOrDefault().Date);
                            db.AddInParameter(cmd, "@DataEntrega", DbType.Date, reserva.DataEntrega == DateTime.MinValue ? null : reserva.DataEntrega);

                            db.ExecuteNonQuery(cmd);
                        }
                    }
                    //using (cmd = db.GetStoredProcCommand("dbo.spInserirReservaTipoPagamento"))
                    //{
                    //    cmd.CommandTimeout = 300;
                    //    foreach (TipoPagamentoDAO tipoPagamento in reserva.ListaTipoPagamento)
                    //    {
                    //        cmd.Parameters.Clear();
                    //        db.AddInParameter(cmd, "@ReservaID", DbType.String, reserva.ReservaID);
                    //        db.AddInParameter(cmd, "@TipoPagamentoID", DbType.Int32, tipoPagamento.TipoPagamentoID);
                    //        db.AddInParameter(cmd, "@ParcelaID", DbType.Int32, tipoPagamento.ParcelaID);
                    //        db.AddInParameter(cmd, "@Valor", DbType.Int32, tipoPagamento.Valor);
                    //        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, tipoPagamento.SistemaID);
                    //        db.ExecuteNonQuery(cmd);
                    //    }
                    //}
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

        public void InserirBaixaReserva(string reservaId, string nomeCarreteiro, long produtoId, int quantidade, int lojaId, int sistemaId, int tipoUsuarioId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirBaixaReserva"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
                    db.AddInParameter(cmd, "@NomeCarreteiro", DbType.String, nomeCarreteiro);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
                    db.AddInParameter(cmd, "@Quantidade", DbType.Int32, quantidade);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@TipoUsuarioID", DbType.Int32, tipoUsuarioId);

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

        public void Inserir(string pedidoId, int tipoPagamentoId, string parcelaId, string valor, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirReservaTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public void Inserir(int orcamentoId, string pedidoId, int clienteId, string dataReserva, string dataEntrega, string observacao, int statusId, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirReservaDoOrcamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@OrcamentoID", DbType.Int32, orcamentoId);
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public void InserirTransito(string pedidoId, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirTransitoReserva"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public DataSet Listar(string reservaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarDataStatusReserva"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
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

        public DataSet ListarReservaLoja(int? lojaOrigemId, string pedidoId, int sistemaId, int tipoUsuarioId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaLoja"))
                {
                    cmd.CommandTimeout = 300;

                    if (lojaOrigemId == 0)
                    {
                        lojaOrigemId = null;
                    }

                    db.AddInParameter(cmd, "@LojaOrigemID", DbType.Int32, lojaOrigemId);
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@TipoUsuarioID", DbType.Int32, tipoUsuarioId);

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

        public bool Listar(string reservaId, long produtoId, string medida, int sistemaId)
        {
            Database db;
            bool reservaProduto = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaProduto"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
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
                    db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
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

        public DataSet ListarReservaLojaFiltro(string pedidoId, int? lojaOrigemId, int? funcionarioId, string cpf, string cnpj, DateTime? dataReserva, DateTime? dataEntrega, int sistemaId, int tipoUsuarioId, List<DAO.ProdutoDAO> listaProduto = null)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaLojaFiltro"))
                {
                    cmd.CommandTimeout = 300;

                    if (!string.IsNullOrEmpty(pedidoId))
                    {
                        db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
                    }

                    if (funcionarioId > 0)
                    {
                        db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionarioId);
                    }

                    if (lojaOrigemId > 0)
                    {
                        db.AddInParameter(cmd, "@LojaOrigemID", DbType.Int32, lojaOrigemId);
                    }

                    if (!string.IsNullOrEmpty(cpf))
                    {
                        db.AddInParameter(cmd, "@CPF", DbType.String, cpf);
                    }

                    if (!string.IsNullOrEmpty(cnpj))
                    {
                        db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
                    }

                    if (dataReserva != null && dataReserva != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataReserva", DbType.Date, dataReserva);
                    }

                    if (dataReserva != null && dataReserva != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataEntrega", DbType.Date, dataEntrega);
                    }

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

                    db.AddInParameter(cmd, "@TipoUsuarioID", DbType.Int32, tipoUsuarioId);

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

        public DataSet ListarReservaAgendada(string reservaId, DateTime dtReservaInicio, DateTime dtReservaFim, DateTime dtEntregaInicio, DateTime dtEntregaFim, string nomeCarreto, int statusId, int lojaId, int sistemaId, bool semDataEntrega)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaAgendada"))
                {
                    cmd.CommandTimeout = 300;

                    if (!string.IsNullOrEmpty(reservaId))
                    {
                        db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
                    }

                    if (dtReservaInicio != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataReservaInicio", DbType.Date, dtReservaInicio);
                    }

                    if (dtReservaFim != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataReservaFim", DbType.Date, dtReservaFim);
                    }

                    if (dtEntregaInicio != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataEntregaInicio", DbType.Date, dtEntregaInicio);
                    }

                    if (dtEntregaFim != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataEntregaFim", DbType.Date, dtEntregaFim);
                    }

                    if (!string.IsNullOrEmpty(nomeCarreto))
                    {
                        db.AddInParameter(cmd, "@NomeCarreteiro", DbType.String, nomeCarreto);
                    }

                    if (statusId > 0)
                    {
                        db.AddInParameter(cmd, "@StatusID", DbType.Int32, statusId);
                    }

                    if (lojaId > 0)
                    {
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    }

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

                    if (semDataEntrega)
                    {
                        db.AddInParameter(cmd, "@SemDataEntrega", DbType.Int32, semDataEntrega);
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

        public DataTable ListarRelatorioSintetico(DateTime dtReservaInicio, DateTime dtReservaFim, int sistemaId)
        {
            Database db;
            DataTable dt;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarRelatorioSintetico"))
                {
                    cmd.CommandTimeout = 300;

                    if (dtReservaInicio != DateTime.MinValue)
                        db.AddInParameter(cmd, "@DataReservaInicio", DbType.Date, dtReservaInicio);

                    if (dtReservaFim != DateTime.MinValue)
                        db.AddInParameter(cmd, "@DataReservaFim", DbType.Date, dtReservaFim);

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

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

                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public DataSet ListarByFiltro(ReservaDAO reservaDAO)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaByFiltro"))
                {
                    cmd.CommandTimeout = 300;

                    if (!string.IsNullOrEmpty(reservaDAO.ReservaID))
                    {
                        db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaDAO.ReservaID);
                    }

                    if (reservaDAO.LojaOrigemID > 0)
                    {
                        db.AddInParameter(cmd, "@LojaOrigemID", DbType.Int32, reservaDAO.LojaOrigemID);
                    }

                    if (reservaDAO.FuncionarioID > 0)
                    {
                        db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, reservaDAO.FuncionarioID);
                    }

                    if (!string.IsNullOrEmpty(reservaDAO.Cpf))
                    {
                        db.AddInParameter(cmd, "@CPF", DbType.String, reservaDAO.Cpf);
                    }

                    if (!string.IsNullOrEmpty(reservaDAO.Cnpj))
                    {
                        db.AddInParameter(cmd, "@CNPJ", DbType.String, reservaDAO.Cnpj);
                    }

                    if (reservaDAO.DataReserva != null && reservaDAO.DataReserva != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataReserva", DbType.Date, reservaDAO.DataReserva);
                    }

                    if (reservaDAO.DataEntrega != null && reservaDAO.DataEntrega != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataEntrega", DbType.Date, reservaDAO.DataEntrega);
                    }

                    if (reservaDAO.StatusID > 0)
                    {
                        db.AddInParameter(cmd, "@StatusID", DbType.Int32, reservaDAO.StatusID);
                    }

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, reservaDAO.SistemaID);

                    if (reservaDAO.SemDataEntrega)
                    {
                        db.AddInParameter(cmd, "@SemDataEntrega", DbType.Int32, reservaDAO.SemDataEntrega);
                    }

                    if (reservaDAO.LojaSaidaID > 0)
                    {
                        db.AddInParameter(cmd, "@LojaSaidaID", DbType.Int32, reservaDAO.LojaSaidaID);
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

        public DataSet ListarByLoja(string lojaId, int sistemaId, string reservaId = null)
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
                    //db.AddInParameter(cmd, "@TOP", DbType.Boolean, top);

                    if (!string.IsNullOrEmpty(reservaId))
                    {
                        db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
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

        public DataTable ListarComanda(string reservaId, int sistemaId, string pedidoOuReserva, int tipoUsuarioId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarComandaEntrega"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoID", DbType.String, reservaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@PedidoOuReserva", DbType.String, pedidoOuReserva);
                    db.AddInParameter(cmd, "@TipoUsuarioID", DbType.Int32, tipoUsuarioId);

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

        public bool ListarTipoPagamento(string reservaId, int tipoPagamentoId, int sistemaId)
        {
            Database db;
            bool tipoPagamento = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarReservaTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
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

        public DataSet ListarValorReserva(string reservaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarValorReserva"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
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

        public DataSet ListarValorReservaAlterado(string reservaId, decimal totalPedido, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarValorReservaAlterado"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ReservaID", DbType.String, reservaId);
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

        public int QuantidadeReserva(DateTime? dataEntrega, int sistemaId, int lojaId)
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
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);

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