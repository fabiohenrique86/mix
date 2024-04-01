using System;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using DAO;
using System.Transactions;

namespace DAL
{
    public class PedidoDAL
    {
        public void Atualizar(string pedidoId, string produtoIId, string quantidadeI, string medidaI, string preco, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarPedido2"))
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
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public void Atualizar(string pedidoId, string produtoEId, string quantidadeE, string medidaE, string produtoIId, string quantidadeI, string medidaI, string preco, string observacao, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarPedido"))
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
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirPedido"))
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

        public void Excluir(string pedidoId, int tipoPagamentoId, int parcelaId, decimal valor, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirPedidoTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
                    db.AddInParameter(cmd, "@TipoPagamentoID", DbType.Int32, tipoPagamentoId);
                    db.AddInParameter(cmd, "@ParcelaID", DbType.Int32, parcelaId);
                    db.AddInParameter(cmd, "@Valor", DbType.Decimal, valor);
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

        public bool Existe(string pedidoId, int sistemaId)
        {
            Database db;
            bool pedido = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoById"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        pedido = true;
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
            return pedido;
        }

        //public bool Existe(string pedidoId, int sistemaId)
        //{
        //    Database db;
        //    bool pedido = false;
        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("Mix");
        //        using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoById"))
        //        {
        //            cmd.CommandTimeout = 300;

        //            db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
        //            db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

        //            if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
        //            {
        //                pedido = true;
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new ApplicationException("Erro ao listar registro", ex);
        //    }
        //    finally
        //    {
        //        db = null;
        //    }
        //    return pedido;
        //}

        public void Inserir(DAO.PedidoDAO pedido)
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
                    using (cmd = db.GetStoredProcCommand("dbo.spInserirPedido"))
                    {
                        cmd.CommandTimeout = 300;
                        db.AddInParameter(cmd, "@PedidoID", DbType.String, pedido.PedidoID);
                        db.AddInParameter(cmd, "@LojaOrigemID", DbType.Int32, pedido.LojaOrigemID);
                        db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, pedido.FuncionarioID);
                        db.AddInParameter(cmd, "@ClienteID", DbType.Int32, pedido.ClienteID);
                        db.AddInParameter(cmd, "@DataPedido", DbType.Date, pedido.DataPedido);
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedido.SistemaID);
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, pedido.LojaID);
                        db.AddInParameter(cmd, "@Observacao", DbType.String, pedido.Observacao);
                        db.ExecuteNonQuery(cmd);
                    }
                    using (cmd = db.GetStoredProcCommand("dbo.spInserirPedidoProduto"))
                    {
                        cmd.CommandTimeout = 300;
                        foreach (DAO.ProdutoDAO produto in pedido.ListaProduto)
                        {
                            cmd.Parameters.Clear();
                            db.AddInParameter(cmd, "@PedidoID", DbType.String, pedido.PedidoID);
                            db.AddInParameter(cmd, "@LojaID", DbType.Int32, pedido.LojaID);
                            db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produto.ProdutoID);
                            db.AddInParameter(cmd, "@Quantidade", DbType.Int16, produto.Quantidade);
                            db.AddInParameter(cmd, "@Medida", DbType.String, produto.Medida);
                            db.AddInParameter(cmd, "@Preco", DbType.Decimal, produto.Preco);
                            db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produto.SistemaID);
                            db.ExecuteNonQuery(cmd);
                        }
                    }
                    //using (cmd = db.GetStoredProcCommand("dbo.spInserirPedidoTipoPagamento"))
                    //{
                    //    cmd.CommandTimeout = 300;
                    //    foreach (DAO.TipoPagamentoDAO tipoPagamento in pedido.ListaTipoPagamento)
                    //    {
                    //        cmd.Parameters.Clear();
                    //        db.AddInParameter(cmd, "@PedidoID", DbType.String, pedido.PedidoID);
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

        public void Inserir(string pedidoId, int tipoPagamentoId, string parcelaId, string valor, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirPedidoTipoPagamento"))
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

        public void Inserir(int orcamentoId, string pedidoId, int clienteId, string dataPedido, string observacao, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirPedidoDoOrcamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@OrcamentoID", DbType.Int32, orcamentoId);
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
                    db.AddInParameter(cmd, "@ClienteID", DbType.Int32, clienteId);
                    db.AddInParameter(cmd, "@DataPedido", DbType.Date, dataPedido);
                    db.AddInParameter(cmd, "@Observacao", DbType.String, observacao);
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

        public DataSet Listar(int? lojaId, string pedidoId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoLoja"))
                {
                    cmd.CommandTimeout = 300;
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public bool Listar(string pedidoId, string produtoId, string medida, int sistemaId)
        {
            Database db;
            bool pedidoProduto = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoProduto"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, Convert.ToInt64(produtoId));
                    db.AddInParameter(cmd, "@Medida", DbType.String, medida);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        pedidoProduto = true;
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
            return pedidoProduto;
        }

        public bool Listar(string pedidoId, string produtoId, string medida, string quantidadeE, int sistemaId)
        {
            Database db;
            bool quantidadeProduto = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoQuantidadeProduto"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public DataSet Listar(string pedidoId, int? lojaId, int? lojaOrigemId, int? funcionarioId, string cpf, string cnpj, DateTime? dataPedido, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoFiltro"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionarioId);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@LojaOrigemID", DbType.Int32, lojaOrigemId);
                    db.AddInParameter(cmd, "@CPF", DbType.String, cpf);
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
                    db.AddInParameter(cmd, "@DataPedido", DbType.Date, dataPedido);
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

        public DataSet ListarByFiltro(PedidoDAO pedido)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoByFiltro"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedido.PedidoID);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, pedido.LojaID);
                    db.AddInParameter(cmd, "@LojaOrigemID", DbType.Int32, pedido.LojaOrigemID);
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, pedido.FuncionarioID);
                    db.AddInParameter(cmd, "@CPF", DbType.String, pedido.Cpf);
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, pedido.Cnpj);
                    db.AddInParameter(cmd, "@DataPedido", DbType.Date, pedido.DataPedido);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedido.SistemaID);
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

        public DataSet ListarByLoja(string lojaId, int sistemaId, bool top)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedido"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(lojaId) || (lojaId == "0"))
                    {
                        lojaId = null;
                    }
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

        public DataTable ListarComanda(string pedidoId, int sistemaId, string pedidoOuReserva)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarComandaEntrega"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public DataSet ListarDetalhe(string pedidoId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoDetalhe"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public bool ListarTipoPagamento(string pedidoId, int tipoPagamentoId, int sistemaId)
        {
            Database db;
            bool tipoPagamento = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public DataSet ListarValorPedido(string pedidoId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarValorPedido"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public DataSet ListarValorPedidoAlterado(string pedidoId, decimal totalPedido, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarValorPedidoAlterado"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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
    }
}
