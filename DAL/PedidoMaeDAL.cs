using DAO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DAL
{
    public class PedidoMaeDAL
    {
        public void Inserir(PedidoMaeDAO pedidoMaeDao)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirPedidoMae"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeDao.PedidoMaeID);
                    db.AddInParameter(cmd, "@DataCadastro", DbType.DateTime, pedidoMaeDao.DataCadastro);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedidoMaeDao.SistemaID);

                    // comentado até subir a versão de "pedido mãe automático" por e-mail
                    //db.AddInParameter(cmd, "@Origem", DbType.String, pedidoMaeDao.Origem);
                    //db.AddInParameter(cmd, "@EmailID", DbType.String, pedidoMaeDao.EmailID);

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
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirPedidoMae"))
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

        public List<PedidoMaeDAO> Listar(PedidoMaeDAO pedidoMaeDao)
        {
            Database db;
            List<PedidoMaeDAO> pedidosMaeDao = new List<PedidoMaeDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoMae"))
                {
                    cmd.CommandTimeout = 300;

                    if (!string.IsNullOrEmpty(pedidoMaeDao.PedidoMaeID))
                    {
                        db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeDao.PedidoMaeID);
                    }

                    if (pedidoMaeDao.DataCadastro != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataCadastro", DbType.DateTime, pedidoMaeDao.DataCadastro);
                    }

                    if (pedidoMaeDao.SistemaID > 0)
                    {
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedidoMaeDao.SistemaID);
                    }

                    // comentado até subir a versão de "pedido mãe automático" por e-mail
                    //if (pedidoMaeDao.Lincado)
                    //{
                    //    db.AddInParameter(cmd, "@Lincado", DbType.Byte, 1);
                    //}

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedidosMaeDao.Add(new PedidoMaeDAO()
                        {
                            PedidoMaeID = item["PedidoMaeID"].ToString(),
                            DataCadastro = Convert.ToDateTime(item["DataCadastro"]),
                            SistemaID = Convert.ToInt32(item["SistemaID"]),
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

            return pedidosMaeDao;
        }

        public List<PedidoMaeDAO> ListarByPedido(PedidoMaeDAO pedidoMaeDao)
        {
            Database db;
            var pedidosMaeDao = new List<PedidoMaeDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoMaeByPedido"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeDao.PedidoMaeID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedidoMaeDao.SistemaID);

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedidosMaeDao.Add(new PedidoMaeDAO()
                        {
                            PedidoMaeID = item["PedidoMaeID"].ToString(),
                            DataCadastro = Convert.ToDateTime(item["DataCadastro"]),
                            SistemaID = Convert.ToInt32(item["SistemaID"]),
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

            return pedidosMaeDao;
        }
    }
}
