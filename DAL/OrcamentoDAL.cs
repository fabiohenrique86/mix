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
    public class OrcamentoDAL
    {
        public void Excluir(DAO.OrcamentoDAO orcamento)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirOrcamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@OrcamentoID", DbType.Int32, orcamento.OrcamentoID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, orcamento.SistemaID);
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

        public void Inserir(DAO.OrcamentoDAO orcamento)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.Timeout = TimeSpan.FromMinutes(5);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirOrcamento"))
                    {
                        cmd.CommandTimeout = 300;
                        db.AddInParameter(cmd, "@OrcamentoID", DbType.Int32, orcamento.OrcamentoID);
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, orcamento.LojaID);
                        db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, orcamento.FuncionarioID);
                        db.AddInParameter(cmd, "@StatusID", DbType.Int32, orcamento.StatusID);
                        db.AddInParameter(cmd, "@DataOrcamento", DbType.DateTime, orcamento.DataOrcamento);                        
                        db.AddInParameter(cmd, "@Observacao", DbType.String, orcamento.Observacao);
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, orcamento.SistemaID);
                        db.ExecuteNonQuery(cmd);
                    }
                    using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirOrcamentoProduto"))
                    {
                        cmd.CommandTimeout = 300;
                        foreach (DAO.ProdutoDAO produto in orcamento.ListaProduto)
                        {
                            cmd.Parameters.Clear();
                            db.AddInParameter(cmd, "@OrcamentoID", DbType.Int64, orcamento.OrcamentoID);
                            db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produto.ProdutoID);
                            db.AddInParameter(cmd, "@Quantidade", DbType.Int16, produto.Quantidade);
                            db.AddInParameter(cmd, "@SobMedida", DbType.String, produto.Medida);                            
                            db.AddInParameter(cmd, "@Preco", DbType.Decimal, produto.Preco);
                            db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produto.SistemaID);
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

        public bool ExisteOrcamento(DAO.OrcamentoDAO orcamento)
        {
            Database db;
            bool retorno = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarOrcamentoId"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@OrcamentoID", DbType.Int32, orcamento.OrcamentoID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, orcamento.SistemaID);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        retorno = true;
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
            return retorno;
        }

        public DataSet Listar(DAO.OrcamentoDAO orcamento, bool top)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarOrcamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, orcamento.LojaID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, orcamento.SistemaID);
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

        public DataSet Listar(DAO.OrcamentoDAO orcamento)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarOrcamentoFiltro"))
                {
                    cmd.CommandTimeout = 300;
                    if (orcamento.OrcamentoID > 0)
                    {
                        db.AddInParameter(cmd, "@OrcamentoID", DbType.Int32, orcamento.OrcamentoID);
                    }

                    if (orcamento.LojaID > 0)
                    {
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, orcamento.LojaID);
                    }

                    if (orcamento.FuncionarioID > 0)
                    {
                        db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, orcamento.FuncionarioID);
                    }

                    if (orcamento.DataOrcamento != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataOrcamento", DbType.DateTime, orcamento.DataOrcamento);
                    }

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, orcamento.SistemaID);

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

        public DataSet ListarProduto(DAO.OrcamentoDAO orcamento)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarOrcamentoProduto"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@OrcamentoID", DbType.Int32, orcamento.OrcamentoID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, orcamento.SistemaID);
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
