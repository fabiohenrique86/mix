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
    public static class Bordero
    {
        // Methods
        public static DataSet Listar(int lojaId, DateTime dataBorderoInicial, DateTime dataBorderoFinal, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarBordero"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@dataBorderoInicial", DbType.DateTime, dataBorderoInicial);
                    db.AddInParameter(cmd, "@DataBorderoFinal", DbType.DateTime, dataBorderoFinal);
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
