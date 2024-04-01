using DAO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DAL
{
    public class PedidoMaeTrocaProdutoDAL
    {
        public long Inserir(PedidoMaeTrocaProdutoDAO pedidoMaeTrocaProdutoDAO)
        {
            Database db;
            long pedidoMaeTrocaProdutoId = 0;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirPedidoMaeTrocaProduto"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@PedidoMaeTrocaID", DbType.Int64, pedidoMaeTrocaProdutoDAO.PedidoMaeTrocaID);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, pedidoMaeTrocaProdutoDAO.ProdutoID);
                    db.AddInParameter(cmd, "@Quantidade", DbType.Int16, pedidoMaeTrocaProdutoDAO.Quantidade);
                    db.AddInParameter(cmd, "@Medida", DbType.String, pedidoMaeTrocaProdutoDAO.Medida);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, pedidoMaeTrocaProdutoDAO.SistemaID);

                    pedidoMaeTrocaProdutoId = Convert.ToInt64(db.ExecuteScalar(cmd));
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

            return pedidoMaeTrocaProdutoId;
        }
    }
}
