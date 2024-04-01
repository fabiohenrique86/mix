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
    public static class Parcela
    {
        // Methods
        public static void Atualizar(string parcelaId, string prazoMedio, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarParcela"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(prazoMedio))
                    {
                        prazoMedio = null;
                    }
                    db.AddInParameter(cmd, "@ParcelaID", DbType.Int32, Convert.ToInt32(parcelaId));
                    db.AddInParameter(cmd, "@PrazoMedio", DbType.Int16, Convert.ToInt16(prazoMedio));
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
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

        public static void Excluir(string parcelaId, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirParcela"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ParcelaID", DbType.Int32, Convert.ToInt32(parcelaId));
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
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

        public static void Inserir(string parcelaId, string prazoMedio, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirParcela"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ParcelaID", DbType.Int32, Convert.ToInt32(parcelaId));
                    db.AddInParameter(cmd, "@PrazoMedio", DbType.Int16, Convert.ToInt16(prazoMedio));
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
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarParcela"))
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

        public static bool Listar(string parcelaId, int sistemaId)
        {
            Database db;
            bool parcela = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarParcelaById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ParcelaID", DbType.Int32, Convert.ToInt32(parcelaId));
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        parcela = true;
                    }
                }
                //return parcela;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return parcela;
        }

        public static DataSet Listar(string parcelaId, string prazoMedio, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarParcelaFiltro"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(parcelaId))
                    {
                        parcelaId = null;
                    }
                    else
                    {
                        Convert.ToInt32(parcelaId);
                    }
                    if (string.IsNullOrEmpty(prazoMedio))
                    {
                        prazoMedio = null;
                    }
                    else
                    {
                        Convert.ToInt16(prazoMedio);
                    }
                    db.AddInParameter(cmd, "@ParcelaID", DbType.Int32, parcelaId);
                    db.AddInParameter(cmd, "@PrazoMedio", DbType.Int16, prazoMedio);
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

        public static DataSet ListarDropDownList(int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarParcelaDropDownList"))
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
