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
    public static class TipoPagamento
    {
        // Methods
        public static void Atualizar(string linhaId, string descricao)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(descricao))
                    {
                        descricao = null;
                    }
                    db.AddInParameter(cmd, "@TipoPagamentoID", DbType.Int32, Convert.ToInt32(linhaId));
                    db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
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

        public static void Excluir(string tipoPagamentoId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@TipoPagamentoID", DbType.Int32, Convert.ToInt32(tipoPagamentoId));
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

        public static void Inserir(string descricao, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
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
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTipoPagamento"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    ds =db.ExecuteDataSet(cmd);
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

        public static bool Listar(string tipoPagamentoId, int sistemaId)
        {
            Database db;
            bool tipoPagamento = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTipoPagamentoById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@TipoPagamentoID", DbType.String, tipoPagamentoId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        tipoPagamento = true;
                    }
                }
                //return tipoPagamento;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return tipoPagamento;
        }

        public static DataSet ListarFiltro(string descricao, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTipoPagamentoFiltro"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(descricao))
                    {
                        descricao = null;
                    }
                    db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    ds =db.ExecuteDataSet(cmd);
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
