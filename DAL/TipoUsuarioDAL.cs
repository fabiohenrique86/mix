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
    public static class TipoUsuarioDAL
    {
        // Methods
        public static void Excluir(string tipoUsuarioId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirTipoUsuario"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@TipoUsuarioID", DbType.Int32, Convert.ToInt32(tipoUsuarioId));
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

        public static void Inserir(int tipoUsuarioId, string descricao)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirTipoUsuario"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@TipoUsuarioID", DbType.Int32, tipoUsuarioId);
                    db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
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

        public static DataSet Listar()
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTipoUsuario"))
                {
                    cmd.CommandTimeout = 300;
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

        public static bool Listar(int tipoUsuarioId, int lojaId)
        {
            Database db;
            bool tipoUsuario = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTipoUsuarioById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@TipoUsuarioID", DbType.Int32, tipoUsuarioId);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        tipoUsuario = true;
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
            return tipoUsuario;
        }

        public static DataSet ListarDropDownList()
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTipoUsuarioDropDownList"))
                {
                    cmd.CommandTimeout = 300;
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

        public static DataSet ListarDropDownListAdmEst()
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTipoUsuarioAdmEstDropDownList"))
                {
                    cmd.CommandTimeout = 300;
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
