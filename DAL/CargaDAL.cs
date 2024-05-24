using DAO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DAL
{
    public class CargaDAL
    {
        public int Inserir(CargaDAO cargaDAO)
        {
            Database db;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirCarga"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@NumeroCarga", DbType.String, cargaDAO.NumeroCarga);
                    db.AddInParameter(cmd, "@DataCadastro", DbType.Date, cargaDAO.DataCadastro);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, cargaDAO.SistemaID);

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

        public CargaDAO Obter(CargaDAO cargaDAO)
        {
            Database db;
            CargaDAO retorno = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarCarga"))
                {
                    cmd.CommandTimeout = 300;

                    if (!string.IsNullOrEmpty(cargaDAO.NumeroCarga))
                        db.AddInParameter(cmd, "@NumeroCarga", DbType.String, cargaDAO.NumeroCarga);

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, cargaDAO.SistemaID);

                    var dr = db.ExecuteReader(cmd);

                    if (dr.Read())
                    {
                        cargaDAO.CargaID = Convert.ToInt32(dr["CargaID"]);
                        cargaDAO.NumeroCarga = dr["NumeroCarga"].ToString();
                        cargaDAO.DataCadastro = Convert.ToDateTime(dr["DataCadastro"]);
                        cargaDAO.SistemaID = Convert.ToInt32(dr["SistemaID"]);
                    }

                    dr.Close();
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
    }
}
