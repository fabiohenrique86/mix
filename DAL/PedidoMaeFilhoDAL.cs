using DAO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DAL
{
    public class PedidoMaeFilhoDAL
    {
        public long Inserir(PedidoMaeFilhoDAO pedidoMaeFilhoDAO)
        {
            Database db;
            long pedidoMaeFilhoId = 0;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirPedidoMaeFilho"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeFilhoDAO.PedidoMaeID);
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoMaeFilhoDAO.PedidoID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedidoMaeFilhoDAO.SistemaID);

                    pedidoMaeFilhoId = Convert.ToInt64(db.ExecuteScalar(cmd));
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

            return pedidoMaeFilhoId;
        }

        public List<PedidoMaeFilhoDAO> Listar(PedidoMaeFilhoDAO pedidoMaeFilhoDAO)
        {
            Database db;
            List<PedidoMaeFilhoDAO> pedidosMaeFilhoDAO = new List<PedidoMaeFilhoDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoMaeFilho"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeFilhoDAO.PedidoMaeID);
                    db.AddInParameter(cmd, "@PedidoID", DbType.String, pedidoMaeFilhoDAO.PedidoID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedidoMaeFilhoDAO.SistemaID);

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedidosMaeFilhoDAO.Add(new PedidoMaeFilhoDAO()
                        {
                            PedidoMaeFilhoID = Convert.ToInt64(item["PedidoMaeFilhoID"]),
                            PedidoMaeID = item["PedidoMaeID"].ToString(),
                            PedidoID = item["PedidoID"].ToString(),
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

            return pedidosMaeFilhoDAO;
        }
    }
}
