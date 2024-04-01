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
    public class UsuarioDAL
    {
        // Methods
        public void Atualizar(string usuarioId, string tipoUsuarioId, string lojaId, string login, string senha)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarUsuario"))
                {
                    cmd.CommandTimeout = 300;
                    if (tipoUsuarioId == "0")
                    {
                        tipoUsuarioId = null;
                    }
                    if (lojaId == "0")
                    {
                        lojaId = null;
                    }
                    if (string.IsNullOrEmpty(login))
                    {
                        login = null;
                    }
                    if (string.IsNullOrEmpty(senha))
                    {
                        senha = null;
                    }
                    db.AddInParameter(cmd, "@UsuarioID", DbType.Int32, usuarioId);
                    db.AddInParameter(cmd, "@TipoUsuarioID", DbType.Int32, tipoUsuarioId);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@Login", DbType.String, login);
                    db.AddInParameter(cmd, "@Senha", DbType.String, senha);
                    db.ExecuteNonQuery(cmd);
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

        public void Excluir(string UsuarioId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirUsuario"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@UsuarioID", DbType.Int32, Convert.ToInt32(UsuarioId));
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

        public void Inserir(string tipoUsuarioId, string lojaId, string login, string senha, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirUsuario"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@TipoUsuarioID", DbType.Int32, tipoUsuarioId);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@Login", DbType.String, login);
                    db.AddInParameter(cmd, "@Senha", DbType.String, senha);
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

        public DataSet Listar(int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarUsuario"))
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

        public bool Listar(int usuarioId, int sistemaId)
        {
            Database db;
            bool usuario = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarUsuarioById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@UsuarioID", DbType.Int32, usuarioId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        usuario = true;
                    }
                }
                //return usuario;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return usuario;
        }

        public bool ValidarLogin(string login, int sistemaId)
        {
            Database db;
            bool loginValido = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLoginSenha"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@Login", DbType.String, login);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        loginValido = true;
                    }
                }
                //return loginValido;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return loginValido;
        }

        public DataSet ValidarLogin(string login, string senha, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spValidarLogin"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@Login", DbType.String, login);
                    db.AddInParameter(cmd, "@Senha", DbType.String, senha);
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
