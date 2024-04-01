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
    public class Transferencia
    {
        public int InserirTransferencia(DAO.Transferencia transferencia)
        {
            Database db;
            try
            {
                DbCommand cmd;
                db = DatabaseFactory.CreateDatabase("Mix");
                using (cmd = db.GetStoredProcCommand("dbo.spInserirTransferencia"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@LojaDeID", DbType.Int32, transferencia.LojaDeID);
                    db.AddInParameter(cmd, "@LojaParaID", DbType.Int32, transferencia.LojaParaID);
                    db.AddInParameter(cmd, "@DataTransferencia", DbType.Date, transferencia.DataTransferencia);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, transferencia.SistemaID);
                    db.AddInParameter(cmd, "@Valida", DbType.Byte, transferencia.Valida);

                    return Convert.ToInt32(db.ExecuteScalar(cmd));
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

        public void InserirTransferenciaProduto(DAO.Produto produtoDAO)
        {
            Database db;
            try
            {
                DbCommand cmd;
                db = DatabaseFactory.CreateDatabase("Mix");
                using (cmd = db.GetStoredProcCommand("dbo.spInserirTransferenciaProduto"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@TransferenciaID", DbType.Int32, produtoDAO.TransferenciaID);
                    db.AddInParameter(cmd, "@LojaDeID", DbType.Int32, produtoDAO.LojaDeID);
                    db.AddInParameter(cmd, "@LojaParaID", DbType.Int32, produtoDAO.LojaParaID);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoDAO.ProdutoID);
                    db.AddInParameter(cmd, "@Quantidade", DbType.Int16, produtoDAO.Quantidade);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produtoDAO.SistemaID);
                    
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

        public void AtualizarQuantidadeEstoque(DAO.Transferencia transferenciaDAO)
        {
            Database db;
            try
            {
                DbCommand cmd;
                db = DatabaseFactory.CreateDatabase("Mix");
                using (cmd = db.GetStoredProcCommand("dbo.spAtualizarQuantidadeEstoque"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@TransferenciaID", DbType.Int32, transferenciaDAO.TransferenciaID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, transferenciaDAO.SistemaID);

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

        public void ValidarComandaTransferencia(DAO.Transferencia transferenciaDAO)
        {
            Database db;

            try
            {
                DbCommand cmd;
                db = DatabaseFactory.CreateDatabase("Mix");
                using (cmd = db.GetStoredProcCommand("dbo.spValidarComandaTransferencia"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@TransferenciaID", DbType.Int32, transferenciaDAO.TransferenciaID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, transferenciaDAO.SistemaID);

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

        public DataSet Listar(int sistemaId, bool valida, int transferenciaId = 0)
        {
            Database db;
            DataSet ds;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTransferencia"))
                {
                    cmd.CommandTimeout = 300;
                    
                    if (transferenciaId > 0)
                    {
                        db.AddInParameter(cmd, "@TransferenciaID", DbType.Int32, transferenciaId);
                    }

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@Valida", DbType.Byte, valida);
                    
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

        public DataSet Listar(int transferenciaId, int lojaDeId, int lojaParaId, DateTime dataTransferencia, int sistemaId)
        {
            Database db;
            DataSet ds;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTransferenciaFiltro"))
                {
                    cmd.CommandTimeout = 300;

                    if (transferenciaId > 0)
                    {
                        db.AddInParameter(cmd, "@TransferenciaID", DbType.Int32, transferenciaId);
                    }

                    if (lojaDeId > 0)
                    {
                        db.AddInParameter(cmd, "@LojaDeID", DbType.Int32, lojaDeId);
                    }

                    if (lojaParaId > 0)
                    {
                        db.AddInParameter(cmd, "@LojaParaID", DbType.Int32, lojaParaId);
                    }

                    if (dataTransferencia != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataTransferencia", DbType.Date, dataTransferencia);
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
        
        public DataSet ListarProduto(int transferenciaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTransferenciaProduto"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@TransferenciaID", DbType.Int32, transferenciaId);
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
