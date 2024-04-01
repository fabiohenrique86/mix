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
    public class Produto
    {
        // Methods
        public void Atualizar(DAO.Produto p)
        {
            Database db;
            try
            {
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.Timeout = TimeSpan.FromMinutes(5);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    db = DatabaseFactory.CreateDatabase("Mix");
                    using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarProduto"))
                    {
                        cmd.CommandTimeout = 300;
                        if (p.LojaID == 0)
                        {
                            p.LojaID = null;
                        }
                        if (p.LinhaID == 0)
                        {
                            p.LinhaID = null;
                        }
                        if (p.ComissaoFuncionario == 0)
                        {
                            p.ComissaoFuncionario = null;
                        }
                        if (p.ComissaoFranqueado == 0)
                        {
                            p.ComissaoFranqueado = null;
                        }
                        if (string.IsNullOrEmpty(p.Descricao))
                        {
                            p.Descricao = null;
                        }
                        if (p.MedidaID == 0)
                        {
                            p.MedidaID = null;
                        }
                        if (p.Quantidade == 0)
                        {
                            p.Quantidade = null;
                        }
                        db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, p.ProdutoID);
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, p.LojaID);
                        db.AddInParameter(cmd, "@LinhaID", DbType.Int32, p.LinhaID);
                        db.AddInParameter(cmd, "@ComissaoFuncionario", DbType.Int16, p.ComissaoFuncionario);
                        db.AddInParameter(cmd, "@ComissaoFranqueado", DbType.Int16, p.ComissaoFranqueado);
                        db.AddInParameter(cmd, "@Descricao", DbType.String, p.Descricao);
                        db.AddInParameter(cmd, "@MedidaID", DbType.Int32, p.MedidaID);
                        db.AddInParameter(cmd, "@Quantidade", DbType.Int16, p.Quantidade);
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, p.SistemaID);
                        db.ExecuteNonQuery(cmd);
                    }
                    scope.Complete();
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

        public void Atualizar(List<DAO.Produto> Produtos)
        {
            try
            {
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.Timeout = TimeSpan.FromMinutes(5);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    foreach (DAO.Produto produto in Produtos)
                    {
                        Atualizar(produto);
                    }
                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao atualizar registro", ex);
            }
        }

        public void Excluir(List<DAO.Produto> Produtos)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (DAO.Produto produto in Produtos)
                    {
                        Excluir(produto);
                    }
                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao atualizar registro", ex);
            }
        }

        public void Excluir(DAO.Produto p)
        {
            Database db;
            try
            {
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.Timeout = TimeSpan.FromMinutes(5);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    db = DatabaseFactory.CreateDatabase("Mix");
                    using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirProduto"))
                    {
                        cmd.CommandTimeout = 300;
                        db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, p.ProdutoID);
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, p.LojaID);
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, p.SistemaID);
                        db.ExecuteNonQuery(cmd);
                    }
                    scope.Complete();
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

        public bool ExisteNaLoja(string produtoId, string lojaId, int sistemaId)
        {
            Database db;
            bool produto = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoLoja"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(lojaId))
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, Convert.ToInt64(produtoId));
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        produto = true;
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
            return produto;
        }

        public void Inserir(List<DAO.Produto> Produtos)
        {
            try
            {
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.Timeout = TimeSpan.FromMinutes(5);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    foreach (DAO.Produto produto in Produtos)
                    {
                        Inserir(produto);
                    }
                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao inserir registro", ex);
            }
        }

        public void Inserir(DAO.Produto p)
        {
            Database db;
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    db = DatabaseFactory.CreateDatabase("Mix");
                    using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirProduto"))
                    {
                        cmd.CommandTimeout = 300;
                        db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, p.ProdutoID);
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, p.LojaID);
                        db.AddInParameter(cmd, "@LinhaID", DbType.Int32, p.LinhaID);
                        db.AddInParameter(cmd, "@ComissaoFuncionario", DbType.Int16, p.ComissaoFuncionario);
                        db.AddInParameter(cmd, "@ComissaoFranqueado", DbType.Int16, p.ComissaoFranqueado);
                        db.AddInParameter(cmd, "@Descricao", DbType.String, p.Descricao);
                        db.AddInParameter(cmd, "@MedidaID", DbType.Int32, p.MedidaID);
                        db.AddInParameter(cmd, "@Quantidade", DbType.Int16, p.Quantidade);
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, p.SistemaID);
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

        public DataSet Listar(string lojaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProduto"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
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

        public bool Listar(long produtoId, string cnpj, int sistemaId)
        {
            Database db;
            bool produto = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoLojaByCnpj"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
                    db.AddInParameter(cmd, "@Cnpj", DbType.String, cnpj);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        produto = true;
                    }
                }
                //return produto;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return produto;
        }

        public DataSet Listar(string lojaId, string linhaId, string comissaoFuncionario, string comissaoFranqueado, string descricao, string medidaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoFiltro"))
                {
                    cmd.CommandTimeout = 300;
                    if (linhaId == "0")
                    {
                        linhaId = null;
                    }
                    if (string.IsNullOrEmpty(comissaoFuncionario))
                    {
                        comissaoFuncionario = null;
                    }
                    if (string.IsNullOrEmpty(comissaoFranqueado))
                    {
                        comissaoFranqueado = null;
                    }
                    if (string.IsNullOrEmpty(descricao))
                    {
                        descricao = null;
                    }
                    if (medidaId == "0")
                    {
                        medidaId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@LinhaID", DbType.Int32, linhaId);
                    db.AddInParameter(cmd, "@ComissaoFuncionario", DbType.Int16, comissaoFuncionario);
                    db.AddInParameter(cmd, "@ComissaoFranqueado", DbType.Int16, comissaoFranqueado);
                    db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
                    db.AddInParameter(cmd, "@MedidaID", DbType.Int32, medidaId);
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

        public DataSet ListarDropDownList(string lojaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoDropDownList"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(lojaId) || (lojaId == "0"))
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
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

        public DataSet ListarGridView(string lojaId, int sistemaId, string descricao = null)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoGridView"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(lojaId) || (lojaId == "0"))
                    {
                        lojaId = null;
                    }
                    if (!string.IsNullOrEmpty(descricao))
                    {
                        db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
                    }                        
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
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

        public bool ExisteNaLoja(Int64 produtoId, string lojaId, int sistemaId)
        {
            Database db;
            bool produto = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoLoja"))
                {
                    if (string.IsNullOrEmpty(lojaId))
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        produto = true;
                    }
                }
                //return produto;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return produto;
        }
    }
}
