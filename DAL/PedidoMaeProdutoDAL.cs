using DAO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DAL
{
    public class PedidoMaeProdutoDAL
    {
        public void Inserir(PedidoMaeProdutoDAO pedidoMaeProdutoDao)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirPedidoMaeProduto"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeProdutoDao.PedidoMaeID);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, pedidoMaeProdutoDao.ProdutoID);
                    db.AddInParameter(cmd, "@Quantidade", DbType.Int16, pedidoMaeProdutoDao.Quantidade);
                    if (!string.IsNullOrEmpty(pedidoMaeProdutoDao.Medida))
                    {
                        db.AddInParameter(cmd, "@Medida", DbType.String, pedidoMaeProdutoDao.Medida);
                    }
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedidoMaeProdutoDao.SistemaID);

                    // comentado até subir a versão de "pedido mãe automático" por e-mail
                    //db.AddInParameter(cmd, "@EmailID", DbType.String, pedidoMaeProdutoDao.EmailID);

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

        public void Excluir(PedidoMaeDAO pedidoMaeDao)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirPedidoMaeProduto"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeDao.PedidoMaeID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedidoMaeDao.SistemaID);

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

        public List<PedidoMaeProdutoDAO> Listar(PedidoMaeProdutoDAO pedidoMaeProdutoDao)
        {
            Database db;
            List<PedidoMaeProdutoDAO> pedidosMaeProdutoDao = new List<PedidoMaeProdutoDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoMaeProduto"))
                {
                    cmd.CommandTimeout = 300;

                    if (!string.IsNullOrEmpty(pedidoMaeProdutoDao.PedidoMaeID))
                    {
                        db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeProdutoDao.PedidoMaeID);
                    }

                    if (pedidoMaeProdutoDao.ProdutoID > 0)
                    {
                        db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, pedidoMaeProdutoDao.ProdutoID);
                    }

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedidoMaeProdutoDao.SistemaID);

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedidosMaeProdutoDao.Add(new PedidoMaeProdutoDAO()
                        {
                            PedidoMaeID = item["PedidoMaeID"].ToString(),
                            ProdutoID = Convert.ToInt64(item["ProdutoID"]),
                            Produto = item["Produto"].ToString(),
                            Quantidade = Convert.ToInt16(item["Quantidade"]),
                            Medida = item["Medida"].ToString(),
                            SistemaID = Convert.ToInt32(item["SistemaID"])
                        });
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

            return pedidosMaeProdutoDao;
        }
    }
}
