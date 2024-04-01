using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using DAO;

namespace DAL
{
    public static class Relatorio
    {
        // Methods
        public static DataTable ListarCancelamento(DateTime dataPedidoInicial, DateTime dataPedidoFinal, int sistemaId, int? lojaId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarCancelamentoPedido"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@DataPedidoInicial", DbType.Date, dataPedidoInicial);
                    db.AddInParameter(cmd, "@DataPedidoFinal", DbType.Date, dataPedidoFinal);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
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

        public static DataTable ListarComissaoFranqueado(DateTime dataPedidoInicial, DateTime dataPedidoFinal, int sistemaId, int? lojaId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarComissaoFranqueado"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@DataPedidoInicial", DbType.Date, dataPedidoInicial);
                    db.AddInParameter(cmd, "@DataPedidoFinal", DbType.Date, dataPedidoFinal);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
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

        public static DataTable ListarComissaoFuncionario(DateTime dataPedidoInicial, DateTime dataPedidoFinal, int sistemaId, int? lojaId, int? funcionarioId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarComissaoFuncionario"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@DataPedidoInicial", DbType.Date, dataPedidoInicial);
                    db.AddInParameter(cmd, "@DataPedidoFinal", DbType.Date, dataPedidoFinal);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    if (funcionarioId == 0)
                    {
                        funcionarioId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionarioId);
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

        public static DataTable ListarEntrega(DateTime dataInicial, DateTime dataFinal, int? lojaId, int? pedidoId, string nomeCliente, string bairro, int? statusId, int sistemaId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarEntregaRel"))
                {
                    cmd.CommandTimeout = 300;
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    if (pedidoId == 0)
                    {
                        pedidoId = null;
                    }
                    if (statusId == 0)
                    {
                        statusId = null;
                    }
                    db.AddInParameter(cmd, "@DataInicial", DbType.Date, dataInicial);
                    db.AddInParameter(cmd, "@DataFinal", DbType.Date, dataFinal);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@PedidoID", DbType.Int32, pedidoId);
                    db.AddInParameter(cmd, "@NomeCliente", DbType.String, nomeCliente);
                    db.AddInParameter(cmd, "@Bairro", DbType.String, bairro);
                    db.AddInParameter(cmd, "@StatusID", DbType.Int32, statusId);
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

        public static DataTable ListarEstoque(int sistemaId, int? lojaId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarEstoque"))
                {
                    cmd.CommandTimeout = 300;
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
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

        public static DataTable ListarEstoqueIdeal(string mesAnoInicial, string mesAnoFinal, int sistemaId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarEstoqueIdeal"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@MesAnoInicial", DbType.String, mesAnoInicial);
                    db.AddInParameter(cmd, "@MesAnoFinal", DbType.String, mesAnoFinal);
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

        public static DataTable ListarNotaFiscal(DateTime dataPedidoInicial, DateTime dataPedidoFinal, int sistemaId, int? notaFiscalId, int? pedidoMaeId, int? lojaId, int? linhaId, long? produtoId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarNotaFiscal"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@DataNotaFiscalInicial", DbType.Date, dataPedidoInicial);
                    db.AddInParameter(cmd, "@DataNotaFiscalFinal", DbType.Date, dataPedidoFinal);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (notaFiscalId == 0)
                    {
                        notaFiscalId = null;
                    }
                    if (pedidoMaeId == 0)
                    {
                        pedidoMaeId = null;
                    }
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    if (linhaId == 0)
                    {
                        linhaId = null;
                    }
                    if (produtoId == 0L)
                    {
                        produtoId = null;
                    }
                    db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, notaFiscalId);
                    db.AddInParameter(cmd, "@PedidoMaeID", DbType.Int32, pedidoMaeId);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@LinhaID", DbType.Int32, linhaId);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
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

        public static DataTable ListarOrcamento(int orcamentoId, int sistemaId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarOrcamentoById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@OrcamentoID", DbType.Int32, orcamentoId);
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

        public static DataTable ListarQuadroClassificacao(DateTime dataPedidoInicial, DateTime dataPedidoFinal, int sistemaId, int? lojaId, int? funcionarioId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarQuadroClassificacao"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@DataInicial", DbType.Date, dataPedidoInicial);
                    db.AddInParameter(cmd, "@DataFinal", DbType.Date, dataPedidoFinal);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    if (funcionarioId == 0)
                    {
                        funcionarioId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionarioId);
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

        public static DataTable ListarQuadroClassificacao2(DateTime dataPedidoInicial, DateTime dataPedidoFinal, int sistemaId, int? lojaId, int? funcionarioId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarQuadroClassificacao2"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@DataInicial", DbType.Date, dataPedidoInicial);
                    db.AddInParameter(cmd, "@DataFinal", DbType.Date, dataPedidoFinal);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    if (funcionarioId == 0)
                    {
                        funcionarioId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionarioId);
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

        public static DataTable ListarVendaProduto(DateTime dataPedidoInicial, DateTime dataPedidoFinal, int sistemaId, int? lojaId, int? linhaId, long? produtoId, int? funcionarioId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarVendaProduto"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@DataPedidoInicial", DbType.Date, dataPedidoInicial);
                    db.AddInParameter(cmd, "@DataPedidoFinal", DbType.Date, dataPedidoFinal);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    if (linhaId == 0)
                    {
                        linhaId = null;
                    }
                    if (produtoId == 0L)
                    {
                        produtoId = null;
                    }
                    if (funcionarioId == 0)
                    {
                        funcionarioId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@LinhaID", DbType.Int32, linhaId);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionarioId);
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

        public static DataTable ListarVendaTipoPagamento(DateTime dataPedidoInicial, DateTime dataPedidoFinal, int sistemaId, int? lojaId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarVendaTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@DataPedidoInicial", DbType.Date, dataPedidoInicial);
                    db.AddInParameter(cmd, "@DataPedidoFinal", DbType.Date, dataPedidoFinal);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
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
    }
}
