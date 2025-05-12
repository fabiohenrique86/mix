using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using DAO;

namespace DAL
{
    public static class FuncionarioDAL
    {
        // Methods
        public static void Atualizar(DAO.FuncionarioDAO funcionario)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarFuncionario"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionario.FuncionarioID);
                    db.AddInParameter(cmd, "@Nome", DbType.String, funcionario.Nome);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, funcionario.LojaID);
                    db.AddInParameter(cmd, "@Telefone", DbType.String, funcionario.Telefone);
                    db.AddInParameter(cmd, "@Email", DbType.String, funcionario.Email);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, funcionario.SistemaID);
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

        public static bool EstaAtivo(int funcionarioId, int sistemaId)
        {
            Database db;
            bool retorno;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarFuncionarioAtivo"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionarioId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    retorno = Convert.ToBoolean(db.ExecuteScalar(cmd));
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

        public static void Excluir(DAO.FuncionarioDAO funcionario)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirFuncionario"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionario.FuncionarioID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, funcionario.SistemaID);
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

        public static void Inserir(DAO.FuncionarioDAO funcionario)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirFuncionario"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, funcionario.FuncionarioID);
                    db.AddInParameter(cmd, "@Nome", DbType.String, funcionario.Nome);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, funcionario.LojaID);
                    db.AddInParameter(cmd, "@Telefone", DbType.String, funcionario.Telefone);
                    db.AddInParameter(cmd, "@Email", DbType.String, funcionario.Email);
                    db.AddInParameter(cmd, "@SistemaID", DbType.String, funcionario.SistemaID);
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

        public static DataSet Listar(int lojaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarFuncionario"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
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

        public static bool Listar(string funcionarioId, int sistemaId)
        {
            Database db;
            bool funcionario = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarFuncionarioById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, Convert.ToInt32(funcionarioId));
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        funcionario = true;
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
            return funcionario;
        }

        public static DataSet Listar(string nome, string lojaId, string telefone, string email, bool? ativo)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarFuncionarioFiltro"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(nome))
                    {
                        nome = null;
                    }
                    if (lojaId == "0")
                    {
                        lojaId = null;
                    }
                    if (string.IsNullOrEmpty(telefone))
                    {
                        telefone = null;
                    }
                    db.AddInParameter(cmd, "@Nome", DbType.String, nome);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@Telefone", DbType.String, telefone);
                    db.AddInParameter(cmd, "@Email", DbType.String, email);

                    if (ativo.HasValue)
                        db.AddInParameter(cmd, "@Ativo", DbType.Boolean, ativo);

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

        public static DataSet ListarDropDownList(string lojaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarFuncionarioDropDownList"))
                {
                    cmd.CommandTimeout = 300;
                    if ((lojaId == "0") || string.IsNullOrEmpty(lojaId))
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
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
