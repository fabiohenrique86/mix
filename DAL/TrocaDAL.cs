using DAO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DAL
{
    public class TrocaDAL
    {
        public int Incluir(TrocaDAO trocaDAO)
        {
            Database db;
            int trocaId = 0;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirTroca"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, trocaDAO.LojaID);
                    db.AddInParameter(cmd, "@ClienteID", DbType.Int32, trocaDAO.ClienteID);
                    db.AddInParameter(cmd, "@DataTroca", DbType.Date, trocaDAO.DataTroca);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, trocaDAO.SistemaID);
                    db.AddInParameter(cmd, "@Ativo", DbType.Boolean, trocaDAO.Ativo);
                    db.AddInParameter(cmd, "@DataEntrega", DbType.Date, trocaDAO.DataEntrega);
                    db.AddInParameter(cmd, "@Observacao", DbType.String, trocaDAO.Observacao);
                    db.AddInParameter(cmd, "@Svt", DbType.String, trocaDAO.Svt);

                    trocaId = Convert.ToInt32(db.ExecuteScalar(cmd));
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

            return trocaId;
        }

        public List<TrocaDAO> Listar(TrocaDAO trocaDAO)
        {
            Database db;
            var listaTroca = new List<TrocaDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");

                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTroca"))
                {
                    cmd.CommandTimeout = 300;

                    if (trocaDAO.TrocaID > 0)
                    {
                        db.AddInParameter(cmd, "@TrocaID", DbType.Int32, trocaDAO.TrocaID);
                    }

                    if (trocaDAO.LojaID > 0)
                    {
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, trocaDAO.LojaID);
                    }

                    if (trocaDAO.ClienteID > 0)
                    {
                        db.AddInParameter(cmd, "@ClienteID", DbType.Int32, trocaDAO.ClienteID);
                    }

                    if (!string.IsNullOrEmpty(trocaDAO.Status))
                    {
                        db.AddInParameter(cmd, "@Status", DbType.String, trocaDAO.Status);
                    }

                    if (trocaDAO.DataEntrega != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataEntrega", DbType.Date, trocaDAO.DataEntrega.Date);
                    }

                    if (!string.IsNullOrEmpty(trocaDAO.Svt))
                    {
                        db.AddInParameter(cmd, "@Svt", DbType.String, trocaDAO.Svt);
                    }

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, trocaDAO.SistemaID);

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var troca = new TrocaDAO();

                        troca.TrocaID = Convert.ToInt32(item["TrocaID"]);

                        troca.LojaID = Convert.ToInt32(item["LojaID"]);
                        troca.NomeFantasia = item["NomeFantasia"].ToString();

                        troca.ClienteID = Convert.ToInt32(item["ClienteID"]);
                        troca.NomeCliente = item["NomeCliente"].ToString();
                        troca.CPF = item["Cpf"].ToString();
                        troca.CNPJ = item["Cnpj"].ToString();

                        troca.DataTroca = Convert.ToDateTime(item["DataTroca"]);
                        troca.SistemaID = Convert.ToInt32(item["SistemaID"]);
                        troca.Ativo = Convert.ToBoolean(item["Ativo"]);

                        if (item["DataExclusao"] != DBNull.Value)
                        {
                            troca.DataExclusao = Convert.ToDateTime(item["DataExclusao"]);
                        }

                        if (item["DataEntrega"] != DBNull.Value)
                        {
                            troca.DataEntrega = Convert.ToDateTime(item["DataEntrega"]);
                        }

                        troca.Observacao = item["Observacao"].ToString();
                        troca.Svt = item["Svt"].ToString();

                        listaTroca.Add(troca);
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

            return listaTroca;
        }

        public void DarBaixa(TrocaDAO trocaDAO)
        {
            Database db;
            try
            {
                DbCommand cmd;
                db = DatabaseFactory.CreateDatabase("Mix");
                using (cmd = db.GetStoredProcCommand("dbo.spDarBaixaTroca"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@TrocaID", DbType.Int32, trocaDAO.TrocaID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, trocaDAO.SistemaID);

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

        public List<TrocaDAO> ListarComandaTroca(TrocaDAO trocaDAO)
        {
            Database db;
            var listaTroca = new List<TrocaDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");

                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarComandaTroca"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@TrocaID", DbType.Int32, trocaDAO.TrocaID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, trocaDAO.SistemaID);

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TrocaDAO Troca = new TrocaDAO();

                        Troca.TrocaID = Convert.ToInt32(item["TrocaID"]);

                        Troca.LojaID = Convert.ToInt32(item["LojaID"]);
                        Troca.NomeFantasia = item["NomeFantasia"].ToString();

                        Troca.Produto = item["Produto"].ToString();
                        Troca.Quantidade = Convert.ToInt16(item["Quantidade"]);

                        Troca.ClienteID = Convert.ToInt32(item["ClienteID"]);
                        Troca.NomeCliente = item["NomeCliente"].ToString();
                        Troca.CPF = item["Cpf"].ToString();
                        Troca.CNPJ = item["Cnpj"].ToString();
                        Troca.Cidade = item["Cidade"].ToString();
                        Troca.Endereco = item["Endereco"].ToString();
                        Troca.Bairro = item["Bairro"].ToString();
                        Troca.PontoReferencia = item["PontoReferencia"].ToString();
                        Troca.TelefoneResidencial = item["TelefoneResidencial"].ToString();
                        Troca.TelefoneCelular = item["TelefoneCelular"].ToString();
                        Troca.TelefoneResidencial2 = item["TelefoneResidencial2"].ToString();
                        Troca.TelefoneCelular2 = item["TelefoneCelular2"].ToString();

                        Troca.DataTroca = Convert.ToDateTime(item["DataTroca"]);
                        Troca.SistemaID = Convert.ToInt32(item["SistemaID"]);
                        Troca.Ativo = Convert.ToBoolean(item["Ativo"]);

                        if (item["DataExclusao"] != DBNull.Value)
                        {
                            Troca.DataExclusao = Convert.ToDateTime(item["DataExclusao"]);
                        }

                        if (item["DataEntrega"] != DBNull.Value)
                        {
                            Troca.DataEntrega = Convert.ToDateTime(item["DataEntrega"]);
                        }

                        Troca.Observacao = item["Observacao"].ToString();
                        Troca.Svt = item["Svt"].ToString();

                        listaTroca.Add(Troca);
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

            return listaTroca;
        }

        public void Excluir(TrocaDAO trocaDAO)
        {
            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirTroca"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@TrocaID", DbType.Int32, trocaDAO.TrocaID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, trocaDAO.SistemaID);

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
    }
}
