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
    public static class Linha
    {
        // Methods
        public static void Atualizar(string linhaId, string descricao, string desconto)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarLinha"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(descricao))
                    {
                        descricao = null;
                    }
                    if (string.IsNullOrEmpty(desconto))
                    {
                        desconto = null;
                    }
                    db.AddInParameter(cmd, "@LinhaID", DbType.Int32, Convert.ToInt32(linhaId));
                    db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
                    db.AddInParameter(cmd, "@Desconto", DbType.Decimal, desconto);
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

        public static void Excluir(string linhaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirLinha"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@LinhaID", DbType.Int32, Convert.ToInt32(linhaId));
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

        public static void Inserir(string descricao, string desconto, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirLinha"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(desconto))
                    {
                        desconto = "0";
                    }
                    db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
                    db.AddInParameter(cmd, "@Desconto", DbType.Decimal, desconto);
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

        public static DataSet Listar(int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLinha"))
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

        public static bool Listar(string linhaId, int sistemaId)
        {
            Database db;
            bool linha = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLinhaById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@LinhaID", DbType.Int32, Convert.ToInt32(linhaId));
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        linha = true;
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
            return linha;
        }

        public static DataSet Listar(string descricao, string desconto, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLinhaFiltro"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(descricao))
                    {
                        descricao = null;
                    }
                    if (string.IsNullOrEmpty(desconto))
                    {
                        desconto = null;
                    }
                    db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
                    db.AddInParameter(cmd, "@Desconto", DbType.Decimal, desconto);
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

        public static bool ListarDescricao(string descricao, int sistemaId)
        {
            Database db;
            bool linha = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLinhaByDescricao"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        linha = true;
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
            return linha;
        }

        public static DataSet ListarDropDownList(int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLinhaDropDownList"))
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
    }
}
