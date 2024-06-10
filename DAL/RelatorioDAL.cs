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
    public static class RelatorioDAL
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

        public static DataTable ListarEntrega(DateTime dataInicial, DateTime dataFinal, int? lojaId, string pedidoId, string nomeCliente, string bairro, int? statusId, int sistemaId)
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
                    if (string.IsNullOrEmpty(pedidoId))
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
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

                    if (lojaId > 0)
                    {
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    }

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

        public static DataTable ListarNotaFiscal(int sistemaId, DateTime dataNotaFiscalInicial, DateTime dataNotaFiscalFinal, int notaFiscalId, string numeroCarga, string produtos)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarNotaFiscal"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

                    if (dataNotaFiscalInicial != DateTime.MinValue)
                        db.AddInParameter(cmd, "@DataNotaFiscalInicial", DbType.Date, dataNotaFiscalInicial);

                    if (dataNotaFiscalFinal != DateTime.MinValue)
                        db.AddInParameter(cmd, "@DataNotaFiscalFinal", DbType.Date, dataNotaFiscalFinal);

                    if (notaFiscalId > 0)
                        db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, notaFiscalId);

                    if (!string.IsNullOrEmpty(numeroCarga))
                        db.AddInParameter(cmd, "@NumeroCarga", DbType.String, numeroCarga);

                    if (!string.IsNullOrEmpty(produtos))
                        db.AddInParameter(cmd, "@ProdutoID", DbType.String, produtos);

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

        public static DataTable ListarVendaProduto(DateTime dataPedidoInicial, DateTime dataPedidoFinal, int sistemaId, int lojaId, int linhaId, string produtos, int funcionarioId)
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

                    if (lojaId > 0)
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);

                    if (linhaId > 0)
                        db.AddInParameter(cmd, "@LinhaID", DbType.Int32, linhaId);

                    if (!string.IsNullOrEmpty(produtos))
                        db.AddInParameter(cmd, "@ProdutoID", DbType.String, produtos);

                    if (funcionarioId > 0)
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

        public static DataTable ListarComandaOcorrencia(OcorrenciaDAO ocorrenciaDAO)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarComandaOcorrencia"))
                {
                    cmd.CommandTimeout = 300;

                    if (ocorrenciaDAO.OcorrenciaID > 0)
                    {
                        db.AddInParameter(cmd, "@OcorrenciaID", DbType.Int32, ocorrenciaDAO.OcorrenciaID);
                    }

                    if (ocorrenciaDAO.LojaID > 0)
                    {
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, ocorrenciaDAO.LojaID);
                    }

                    if (ocorrenciaDAO.MotivoOcorrenciaID > 0)
                    {
                        db.AddInParameter(cmd, "@MotivoOcorrenciaID", DbType.Int32, ocorrenciaDAO.MotivoOcorrenciaID);
                    }

                    if (ocorrenciaDAO.DataOcorrenciaInicial != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataOcorrenciaInicial", DbType.DateTime, ocorrenciaDAO.DataOcorrenciaInicial);
                    }

                    if (ocorrenciaDAO.DataOcorrenciaFinal != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataOcorrenciaFinal", DbType.DateTime, ocorrenciaDAO.DataOcorrenciaFinal);
                    }

                    if (ocorrenciaDAO.StatusOcorrenciaID > 0)
                    {
                        db.AddInParameter(cmd, "@StatusOcorrenciaID", DbType.Int32, ocorrenciaDAO.StatusOcorrenciaID);
                    }

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, ocorrenciaDAO.SistemaID);

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

        public static DataTable ListarComissaoFucionarioSintetico(DateTime dtReservaInicio, DateTime dtReservaFim, int sistemaId, int? lojaId, int? funcionarioId)
        {
            Database db;
            DataTable dt;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarComissaoFuncionarioSintetico"))
                {
                    cmd.CommandTimeout = 300;

                    if (dtReservaInicio != DateTime.MinValue)
                        db.AddInParameter(cmd, "@DataReservaInicial", DbType.Date, dtReservaInicio);

                    if (dtReservaFim != DateTime.MinValue)
                        db.AddInParameter(cmd, "@DataReservaFinal", DbType.Date, dtReservaFim);

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId == 0 ? null : lojaId);
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionarioId == 0 ? null : funcionarioId);

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

        public static DataTable ListarEntregaCSV(DateTime dataInicial, DateTime dataFinal, int? lojaId, string pedidoId, string nomeCliente, string bairro, int? statusId, int sistemaId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarEntregaCSVRel"))
                {
                    cmd.CommandTimeout = 300;
                    if (lojaId == 0)
                    {
                        lojaId = null;
                    }
                    if (string.IsNullOrEmpty(pedidoId))
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
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoId);
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

        public static DataTable ListarRelatorioEstoque(int sistemaId)
        {
            Database db;
            DataTable dt;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarRelatorioEstoque"))
                {
                    cmd.CommandTimeout = 300;

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
    }
}
