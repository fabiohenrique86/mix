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
    public class LojaDAL
    {
        // Methods
        public void Atualizar(string lojaId, string razaoSocial, string nomeFantasia, string telefone, string cota, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarLoja"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(razaoSocial))
                    {
                        razaoSocial = null;
                    }
                    if (string.IsNullOrEmpty(nomeFantasia))
                    {
                        nomeFantasia = null;
                    }
                    if (string.IsNullOrEmpty(telefone))
                    {
                        telefone = null;
                    }
                    if (string.IsNullOrEmpty(cota))
                    {
                        cota = null;
                    }
                    db.AddInParameter(cmd, "@LojaID", DbType.String, lojaId);
                    db.AddInParameter(cmd, "@RazaoSocial", DbType.String, razaoSocial);
                    db.AddInParameter(cmd, "@NomeFantasia", DbType.String, nomeFantasia);
                    db.AddInParameter(cmd, "@Telefone", DbType.String, telefone);
                    db.AddInParameter(cmd, "@Cota", DbType.Decimal, cota);
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

        public void Excluir(int lojaId, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirLoja"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
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

        public void Inserir(string cnpj, string razaoSocial, string nomeFantasia, string telefone, string cota, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirLoja"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
                    db.AddInParameter(cmd, "@RazaoSocial", DbType.String, razaoSocial);
                    db.AddInParameter(cmd, "@NomeFantasia", DbType.String, nomeFantasia);
                    db.AddInParameter(cmd, "@Telefone", DbType.String, telefone);
                    db.AddInParameter(cmd, "@Cota", DbType.Decimal, cota);
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
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLoja"))
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

        public bool Listar(string lojaId, int sistemaId)
        {
            Database db;
            bool loja = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLojaById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, Convert.ToInt32(lojaId));
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        loja = true;
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
            return loja;
        }

        public DataSet ListarFiltro(string cnpj, string razaoSocial, string nomeFantasia, string telefone, string cota, int sistemaId, bool? ativo)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLojaFiltro"))
                {
                    cmd.CommandTimeout = 300;

                    if (string.IsNullOrEmpty(cnpj))
                    {
                        cnpj = null;
                    }

                    if (string.IsNullOrEmpty(razaoSocial))
                    {
                        razaoSocial = null;
                    }

                    if (string.IsNullOrEmpty(nomeFantasia))
                    {
                        nomeFantasia = null;
                    }

                    if (string.IsNullOrEmpty(telefone))
                    {
                        telefone = null;
                    }

                    if (string.IsNullOrEmpty(cota))
                    {
                        cota = null;
                    }

                    db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
                    db.AddInParameter(cmd, "@RazaoSocial", DbType.String, razaoSocial);
                    db.AddInParameter(cmd, "@NomeFantasia", DbType.String, nomeFantasia);
                    db.AddInParameter(cmd, "@Telefone", DbType.String, telefone);
                    db.AddInParameter(cmd, "@Cota", DbType.Decimal, cota);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

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

        public DataSet ListarById(string lojaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");

                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLojaId"))
                {
                    cmd.CommandTimeout = 300;

                    if (string.IsNullOrEmpty(lojaId) || (lojaId == "0"))
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

        public DataSet ListarDropDownList(int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLojaDropDownList"))
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

        public DataSet ListarDropDownList(int lojaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarPedidoLojaDropDownList"))
                {
                    cmd.CommandTimeout = 300;

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

        public bool ListarFiltro(string cnpj, int sistemaId)
        {
            Database db;
            bool loja = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLojaByCnpj"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
                    db.AddInParameter(cmd, "@SistemaID", DbType.String, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        loja = true;
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
            return loja;
        }

        public DataSet ListarOrcamentoDropDownList(int lojaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLojaOrcamentoDropDownList"))
                {
                    cmd.CommandTimeout = 300;
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

        public DataSet ListarSemFabrica(int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLojaSemFabrica"))
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

        public DataSet ListarUsuarioDropDownList(int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarLojaUsuarioDropDownList"))
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

        public DAO.LojaDAO ObterLojaByCnpj(DAO.LojaDAO lojaDAO)
        {
            Database db;
            DAO.LojaDAO retorno = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");

                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spObterLojaByCnpj"))
                {
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, lojaDAO.CNPJ);
                    db.AddInParameter(cmd, "@SistemaID", DbType.String, lojaDAO.SistemaID);

                    var dr = db.ExecuteReader(cmd);

                    if (dr.Read())
                    {
                        retorno = new LojaDAO();

                        retorno.LojaID = Convert.ToInt32(dr["LojaID"]);
                        retorno.Nome = dr["NomeFantasia"].ToString();
                        retorno.NomeFantasia = dr["NomeFantasia"].ToString();
                        retorno.CNPJ = dr["CNPJ"].ToString();
                        retorno.SistemaID = Convert.ToInt32(dr["SistemaID"]);
                    }

                    dr.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao obter registro", ex);
            }
            finally
            {
                db = null;
            }

            return retorno;
        }
    }
}
