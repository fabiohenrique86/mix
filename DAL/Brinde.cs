using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public static class Brinde
    {
        // Methods
        public static void Excluir(string produtoId, string brindeId, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirBrinde"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, Convert.ToInt64(produtoId));
                    db.AddInParameter(cmd, "@BrindeID", DbType.Int64, Convert.ToInt64(brindeId));
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

        public static void Inserir(string produtoId, string brindeId, int sistemaId, int quantidade, string preco)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirBrinde"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, Convert.ToInt64(produtoId));
                    db.AddInParameter(cmd, "@BrindeID", DbType.Int64, Convert.ToInt64(brindeId));
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    db.AddInParameter(cmd, "@Quantidade", DbType.Int16, quantidade);
                    db.AddInParameter(cmd, "@Preco", DbType.Decimal, preco);
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

        public static DataSet Listar(int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarBrinde"))
                {
                    cmd.CommandTimeout = 300;
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

        public static DataSet Listar(string produtoId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoBrinde"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, Convert.ToInt64(produtoId));
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

        public static bool Listar(long produtoId, long brindeId, int sistemaId)
        {
            Database db;
            bool brinde = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoBrindeById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
                    db.AddInParameter(cmd, "@BrindeID", DbType.Int64, brindeId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        brinde = true;
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
            return brinde;
        }

        public static DataSet Listar(string produtoId, string brindeId, string quantidade, string preco, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarBrindeFiltro"))
                {
                    cmd.CommandTimeout = 300;
                    if (produtoId == "0")
                    {
                        produtoId = null;
                    }
                    if (brindeId == "0")
                    {
                        brindeId = null;
                    }
                    if (string.IsNullOrEmpty(quantidade) || (quantidade == "0"))
                    {
                        quantidade = null;
                    }
                    if (string.IsNullOrEmpty(preco) || (preco == "0,00"))
                    {
                        preco = null;
                    }
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
                    db.AddInParameter(cmd, "@BrindeID", DbType.Int64, brindeId);
                    db.AddInParameter(cmd, "@Quantidade", DbType.Int16, quantidade);
                    db.AddInParameter(cmd, "@Preco", DbType.Decimal, preco);
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

        public static DataSet ListarTodos(string produtoId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarBrindeTodos"))
                {
                    cmd.CommandTimeout = 300;
                    char[] p = ",".ToCharArray();
                    string[] produto = produtoId.Split(p, StringSplitOptions.RemoveEmptyEntries);
                    if (produto.Length > 0)
                    {
                        db.AddInParameter(cmd, "@ProdutoID1", DbType.Int64, Convert.ToInt64(produto[0]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID1", DbType.Int64, 0);
                    }
                    if (produto.Length > 1)
                    {
                        db.AddInParameter(cmd, "@ProdutoID2", DbType.Int64, Convert.ToInt64(produto[1]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID2", DbType.Int64, 0);
                    }
                    if (produto.Length > 2)
                    {
                        db.AddInParameter(cmd, "@ProdutoID3", DbType.Int64, Convert.ToInt64(produto[2]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID3", DbType.Int64, 0);
                    }
                    if (produto.Length > 3)
                    {
                        db.AddInParameter(cmd, "@ProdutoID4", DbType.Int64, Convert.ToInt64(produto[3]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID4", DbType.Int64, 0);
                    }
                    if (produto.Length > 4)
                    {
                        db.AddInParameter(cmd, "@ProdutoID5", DbType.Int64, Convert.ToInt64(produto[4]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID5", DbType.Int64, 0);
                    }
                    if (produto.Length > 5)
                    {
                        db.AddInParameter(cmd, "@ProdutoID6", DbType.Int64, Convert.ToInt64(produto[5]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID6", DbType.Int64, 0);
                    }
                    if (produto.Length > 6)
                    {
                        db.AddInParameter(cmd, "@ProdutoID7", DbType.Int64, Convert.ToInt64(produto[6]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID7", DbType.Int64, 0);
                    }
                    if (produto.Length > 7)
                    {
                        db.AddInParameter(cmd, "@ProdutoID8", DbType.Int64, Convert.ToInt64(produto[7]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID8", DbType.Int64, 0);
                    }
                    if (produto.Length > 8)
                    {
                        db.AddInParameter(cmd, "@ProdutoID9", DbType.Int64, Convert.ToInt64(produto[8]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID9", DbType.Int64, 0);
                    }
                    if (produto.Length > 9)
                    {
                        db.AddInParameter(cmd, "@ProdutoID10", DbType.Int64, Convert.ToInt64(produto[9]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID10", DbType.Int64, 0);
                    }
                    if (produto.Length > 10)
                    {
                        db.AddInParameter(cmd, "@ProdutoID11", DbType.Int64, Convert.ToInt64(produto[10]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID11", DbType.Int64, 0);
                    }
                    if (produto.Length > 11)
                    {
                        db.AddInParameter(cmd, "@ProdutoID12", DbType.Int64, Convert.ToInt64(produto[11]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID12", DbType.Int64, 0);
                    }
                    if (produto.Length > 12)
                    {
                        db.AddInParameter(cmd, "@ProdutoID13", DbType.Int64, Convert.ToInt64(produto[12]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID13", DbType.Int64, 0);
                    }
                    if (produto.Length > 13)
                    {
                        db.AddInParameter(cmd, "@ProdutoID14", DbType.Int64, Convert.ToInt64(produto[13]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID14", DbType.Int64, 0);
                    }
                    if (produto.Length > 14)
                    {
                        db.AddInParameter(cmd, "@ProdutoID15", DbType.Int64, Convert.ToInt64(produto[14]));
                    }
                    else
                    {
                        db.AddInParameter(cmd, "@ProdutoID15", DbType.Int64, 0);
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
    }
}
