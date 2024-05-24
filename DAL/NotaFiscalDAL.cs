using DAO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;

namespace DAL
{
    public class NotaFiscalDAL
    {
        public void Excluir(int notaFiscalId, int sistemaId)
        {
            Database db;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirNotaFiscal"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, notaFiscalId);
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

        public void Inserir(NotaFiscalDAO notaFiscalDAO)
        {
            Database db;

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(10) }))
                {
                    db = DatabaseFactory.CreateDatabase("Mix");
                    using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirNotaFiscal"))
                    {
                        cmd.CommandTimeout = 300;

                        db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, notaFiscalDAO.NotaFiscalID);
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, notaFiscalDAO.LojaID);
                        db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, notaFiscalDAO.PedidoMaeID);

                        if (notaFiscalDAO.Produto != null)
                        {
                            db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, notaFiscalDAO.Produto.ProdutoID);
                            db.AddInParameter(cmd, "@Quantidade", DbType.Int16, notaFiscalDAO.Produto.Quantidade);
                        }
                        else
                        {
                            db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, notaFiscalDAO.ProdutoID);
                            db.AddInParameter(cmd, "@Quantidade", DbType.Int16, notaFiscalDAO.Quantidade);
                        }

                        db.AddInParameter(cmd, "@DataNotaFiscal", DbType.DateTime, notaFiscalDAO.DataNotaFiscal);
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, notaFiscalDAO.SistemaID);
                        db.AddInParameter(cmd, "@Estoque", DbType.Int32, notaFiscalDAO.Estoque);
                        db.AddInParameter(cmd, "@CargaID", DbType.Int32, notaFiscalDAO.CargaID);

                        db.ExecuteNonQuery(cmd);
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

        public void Inserir(List<DAO.NotaFiscalDAO> NotasFiscais)
        {
            try
            {
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.Timeout = TimeSpan.FromMinutes(5);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    foreach (NotaFiscalDAO notaFiscal in NotasFiscais)
                    {
                        Inserir(notaFiscal);
                    }
                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao inserir registro", ex);
            }
        }

        public void Inserir(int notaFiscalId, long produtoId, string quantidade, string dataNotaFiscal, int sistemaId, int lojaId, string pedidoMaeId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirNotaFiscal"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, notaFiscalId);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
                    db.AddInParameter(cmd, "@Quantidade", DbType.Int16, quantidade);
                    db.AddInParameter(cmd, "@DataNotaFiscal", DbType.DateTime, dataNotaFiscal);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeId);

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

        public bool Listar(NotaFiscalDAO notaFiscalDao)
        {
            Database db;
            bool notaFiscal = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarNotaFiscalById"))
                {
                    cmd.CommandTimeout = 300;

                    if (notaFiscalDao.NotaFiscalID > 0)
                        db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, notaFiscalDao.NotaFiscalID);

                    if (!string.IsNullOrEmpty(notaFiscalDao.PedidoMaeID))
                        db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, notaFiscalDao.PedidoMaeID);

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, notaFiscalDao.SistemaID);

                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        notaFiscal = true;
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
            return notaFiscal;
        }

        public bool Listar(int notaFiscalId, long produtoId, int sistemaId)
        {
            Database db;
            bool produtoNotaFiscal = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarNotaFiscalProdutoById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, notaFiscalId);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        produtoNotaFiscal = true;
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
            return produtoNotaFiscal;
        }

        public DataSet Listar(string notaFiscalId, string pedidoMaeId, string lojaId, string dataNotaFiscal, int sistemaId, bool top)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLojaNotaFiscal"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(notaFiscalId))
                    {
                        notaFiscalId = null;
                    }
                    if (string.IsNullOrEmpty(pedidoMaeId))
                    {
                        pedidoMaeId = null;
                    }
                    if (string.IsNullOrEmpty(lojaId))
                    {
                        lojaId = null;
                    }
                    if (string.IsNullOrEmpty(dataNotaFiscal) || (dataNotaFiscal == "__/__/____"))
                    {
                        dataNotaFiscal = null;
                    }

                    db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, notaFiscalId);
                    db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeId);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@DataNotaFiscal", DbType.Date, dataNotaFiscal);
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

        public DataSet ListarData(int lojaId, string dataNotaFiscal, string notaFiscalId, string pedidoMaeId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarNotaFiscalData"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    if (!string.IsNullOrEmpty(dataNotaFiscal) && dataNotaFiscal != "__/__/____")
                    {
                        db.AddInParameter(cmd, "@DataNotaFiscal", DbType.Date, dataNotaFiscal);
                    }
                    if (!string.IsNullOrEmpty(notaFiscalId))
                    {
                        db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, notaFiscalId);
                    }
                    if (!string.IsNullOrEmpty(pedidoMaeId))
                    {
                        db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeId);
                    }
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
        public DataSet ListarProduto(int notaFiscalId, int sistemaId, long produtoId = 0)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarNotaFiscalProduto"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, notaFiscalId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

                    if (produtoId > 0)
                    {
                        db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
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
    }
}
