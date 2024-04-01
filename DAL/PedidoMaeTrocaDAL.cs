using DAO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DAL
{
    public class PedidoMaeTrocaDAL
    {
        public long Inserir(PedidoMaeTrocaDAO pedidoMaeTrocaDao)
        {
            Database db;
            long pedidoMaeTrocaId = 0;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirPedidoMaeTroca"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeTrocaDao.PedidoMaeID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedidoMaeTrocaDao.SistemaID);

                    pedidoMaeTrocaId = Convert.ToInt64(db.ExecuteScalar(cmd));
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

            return pedidoMaeTrocaId;
        }

        public List<PedidoMaeTrocaDAO> Listar(PedidoMaeTrocaDAO pedidoMaeTrocaDao)
        {
            Database db;
            List<PedidoMaeTrocaDAO> pedidosMaeTrocaDAO = new List<PedidoMaeTrocaDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoMaeTroca"))
                {
                    cmd.CommandTimeout = 300;
                    
                    db.AddInParameter(cmd, "@PedidoMaeID", DbType.String, pedidoMaeTrocaDao.PedidoMaeID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedidoMaeTrocaDao.SistemaID);

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        pedidosMaeTrocaDAO.Add(new PedidoMaeTrocaDAO()
                        {
                            PedidoMaeTrocaID = Convert.ToInt64(item["PedidoMaeTrocaID"]),
                            PedidoMaeID = item["PedidoMaeID"].ToString(),
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

            return pedidosMaeTrocaDAO;
        }
    }
}
