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
    public class Cliente
    {
        // Methods
        public void Atualizar(ClientePF clientePF)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarClientePF"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ClienteID", DbType.Int32, clientePF.ClienteID);
                    db.AddInParameter(cmd, "@Nome", DbType.String, clientePF.Nome);
                    db.AddInParameter(cmd, "@CPF", DbType.String, clientePF.Cpf);
                    db.AddInParameter(cmd, "@DataNascimento", DbType.Date, clientePF.DataNascimento);
                    db.AddInParameter(cmd, "@Endereco", DbType.String, clientePF.Endereco);
                    db.AddInParameter(cmd, "@Bairro", DbType.String, clientePF.Bairro);
                    db.AddInParameter(cmd, "@Cidade", DbType.String, clientePF.Cidade);
                    db.AddInParameter(cmd, "@Estado", DbType.String, clientePF.Estado);
                    db.AddInParameter(cmd, "@PontoReferencia", DbType.String, clientePF.PontoReferencia);
                    db.AddInParameter(cmd, "@TelefoneResidencial", DbType.String, clientePF.TelefoneResidencial);
                    db.AddInParameter(cmd, "@TelefoneCelular", DbType.String, clientePF.TelefoneCelular);
                    db.AddInParameter(cmd, "@TelefoneResidencial2", DbType.String, clientePF.TelefoneResidencial2);
                    db.AddInParameter(cmd, "@TelefoneCelular2", DbType.String, clientePF.TelefoneCelular2);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, clientePF.SistemaID);
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

        public void Atualizar(ClientePJ clientePJ)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarClientePJ"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ClienteID", DbType.Int32, clientePJ.ClienteID);
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, clientePJ.Cnpj);
                    db.AddInParameter(cmd, "@NomeFantasia", DbType.String, clientePJ.NomeFantasia);
                    db.AddInParameter(cmd, "@RazaoSocial", DbType.String, clientePJ.RazaoSocial);
                    db.AddInParameter(cmd, "@Endereco", DbType.String, clientePJ.Endereco);
                    db.AddInParameter(cmd, "@Bairro", DbType.String, clientePJ.Bairro);
                    db.AddInParameter(cmd, "@Cidade", DbType.String, clientePJ.Cidade);
                    db.AddInParameter(cmd, "@Estado", DbType.String, clientePJ.Estado);
                    db.AddInParameter(cmd, "@PontoReferencia", DbType.String, clientePJ.PontoReferencia);
                    db.AddInParameter(cmd, "@TelefoneResidencial", DbType.String, clientePJ.TelefoneResidencial);
                    db.AddInParameter(cmd, "@TelefoneCelular", DbType.String, clientePJ.TelefoneCelular);
                    db.AddInParameter(cmd, "@TelefoneResidencial2", DbType.String, clientePJ.TelefoneResidencial2);
                    db.AddInParameter(cmd, "@TelefoneCelular2", DbType.String, clientePJ.TelefoneCelular2);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, clientePJ.SistemaID);
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

        public bool EstaAtivo(int clienteId, int sistemaId)
        {
            Database db;
            bool retorno;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarClienteAtivoById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ClienteID", DbType.Int32, clienteId);
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

        public bool EstaAtivoByCnpj(string cnpj, int sistemaId)
        {
            Database db;
            bool retorno;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarClientePJAtivo"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
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

        public bool EstaAtivoByCpf(string cpf, int sistemaId)
        {
            Database db;
            bool retorno;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarClientePFAtivo"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@CPF", DbType.String, cpf);
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

        public void Excluir(int clienteId, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirCliente"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ClienteID", DbType.Int32, clienteId);
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

        public void Inserir(ClientePF clientePF)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirClientePF"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@CPF", DbType.String, clientePF.Cpf);
                    db.AddInParameter(cmd, "@Nome", DbType.String, clientePF.Nome);
                    db.AddInParameter(cmd, "@DataNascimento", DbType.Date, clientePF.DataNascimento);
                    db.AddInParameter(cmd, "@Endereco", DbType.String, clientePF.Endereco);
                    db.AddInParameter(cmd, "@Bairro", DbType.String, clientePF.Bairro);
                    db.AddInParameter(cmd, "@Cidade", DbType.String, clientePF.Cidade);
                    db.AddInParameter(cmd, "@Estado", DbType.String, clientePF.Estado);
                    db.AddInParameter(cmd, "@PontoReferencia", DbType.String, clientePF.PontoReferencia);
                    db.AddInParameter(cmd, "@TelefoneResidencial", DbType.String, clientePF.TelefoneResidencial);
                    db.AddInParameter(cmd, "@TelefoneCelular", DbType.String, clientePF.TelefoneCelular);
                    db.AddInParameter(cmd, "@TelefoneResidencial2", DbType.String, clientePF.TelefoneResidencial2);
                    db.AddInParameter(cmd, "@TelefoneCelular2", DbType.String, clientePF.TelefoneCelular2);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, clientePF.SistemaID);
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

        public void Inserir(ClientePJ clientePJ)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirClientePJ"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, clientePJ.Cnpj);
                    db.AddInParameter(cmd, "@NomeFantasia", DbType.String, clientePJ.NomeFantasia);
                    db.AddInParameter(cmd, "@RazaoSocial", DbType.String, clientePJ.RazaoSocial);
                    db.AddInParameter(cmd, "@Endereco", DbType.String, clientePJ.Endereco);
                    db.AddInParameter(cmd, "@Bairro", DbType.String, clientePJ.Bairro);
                    db.AddInParameter(cmd, "@Cidade", DbType.String, clientePJ.Cidade);
                    db.AddInParameter(cmd, "@Estado", DbType.String, clientePJ.Estado);
                    db.AddInParameter(cmd, "@PontoReferencia", DbType.String, clientePJ.PontoReferencia);
                    db.AddInParameter(cmd, "@TelefoneResidencial", DbType.String, clientePJ.TelefoneResidencial);
                    db.AddInParameter(cmd, "@TelefoneCelular", DbType.String, clientePJ.TelefoneCelular);
                    db.AddInParameter(cmd, "@TelefoneResidencial2", DbType.String, clientePJ.TelefoneResidencial2);
                    db.AddInParameter(cmd, "@TelefoneCelular2", DbType.String, clientePJ.TelefoneCelular2);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, clientePJ.SistemaID);
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

        public DataSet Listar(ClientePF clientePF)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarClienteFiltroPF"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@CPF", DbType.String, clientePF.Cpf);
                    db.AddInParameter(cmd, "@Nome", DbType.String, clientePF.Nome);
                    db.AddInParameter(cmd, "@DataNascimento", DbType.Date, clientePF.DataNascimento);
                    db.AddInParameter(cmd, "@Endereco", DbType.String, clientePF.Endereco);
                    db.AddInParameter(cmd, "@Bairro", DbType.String, clientePF.Bairro);
                    db.AddInParameter(cmd, "@Cidade", DbType.String, clientePF.Cidade);
                    db.AddInParameter(cmd, "@Estado", DbType.String, clientePF.Estado);
                    db.AddInParameter(cmd, "@PontoReferencia", DbType.String, clientePF.PontoReferencia);
                    db.AddInParameter(cmd, "@TelefoneResidencial", DbType.String, clientePF.TelefoneResidencial);
                    db.AddInParameter(cmd, "@TelefoneCelular", DbType.String, clientePF.TelefoneCelular);
                    db.AddInParameter(cmd, "@TelefoneResidencial2", DbType.String, clientePF.TelefoneResidencial2);
                    db.AddInParameter(cmd, "@TelefoneCelular2", DbType.String, clientePF.TelefoneCelular2);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, clientePF.SistemaID);
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, clientePF.FuncionarioID);
                    db.AddInParameter(cmd, "@Mes", DbType.String, clientePF.MesNascimento);
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

        public DataSet Listar(ClientePJ clientePJ)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarClienteFiltroPJ"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, clientePJ.Cnpj);
                    db.AddInParameter(cmd, "@NomeFantasia", DbType.String, clientePJ.NomeFantasia);
                    db.AddInParameter(cmd, "@RazaoSocial", DbType.String, clientePJ.RazaoSocial);
                    db.AddInParameter(cmd, "@Endereco", DbType.String, clientePJ.Endereco);
                    db.AddInParameter(cmd, "@Bairro", DbType.String, clientePJ.Bairro);
                    db.AddInParameter(cmd, "@Cidade", DbType.String, clientePJ.Cidade);
                    db.AddInParameter(cmd, "@Estado", DbType.String, clientePJ.Estado);
                    db.AddInParameter(cmd, "@PontoReferencia", DbType.String, clientePJ.PontoReferencia);
                    db.AddInParameter(cmd, "@TelefoneResidencial", DbType.String, clientePJ.TelefoneResidencial);
                    db.AddInParameter(cmd, "@TelefoneCelular", DbType.String, clientePJ.TelefoneCelular);
                    db.AddInParameter(cmd, "@TelefoneResidencial2", DbType.String, clientePJ.TelefoneResidencial2);
                    db.AddInParameter(cmd, "@TelefoneCelular2", DbType.String, clientePJ.TelefoneCelular2);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, clientePJ.SistemaID);
                    db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, clientePJ.FuncionarioID);
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

        public DataSet Listar(int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarCliente"))
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

        public bool Listar(int clienteId, int sistemaId)
        {
            Database db;
            bool cliente = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarClienteById"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ClienteID", DbType.Int32, clienteId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        cliente = true;
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
            return cliente;
        }

        public int ListarByCnpj(string cnpj, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarClienteByCnpj"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    return Convert.ToInt32(db.ExecuteScalar(cmd));
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
        }

        public int ListarByCpf(string cpf, int sistemaId)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarClienteByCpf"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@Cpf", DbType.String, cpf);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    return Convert.ToInt32(db.ExecuteScalar(cmd));
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
        }
    }
}
