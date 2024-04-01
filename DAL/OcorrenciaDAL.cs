using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using DAO;

namespace DAL
{
    public class OcorrenciaDAL
    {
        public int Inserir(OcorrenciaDAO ocorrenciaDAO)
        {
            int ocorrenciaId = 0;
            Database db;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirOcorrencia"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, ocorrenciaDAO.NotaFiscalID);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, ocorrenciaDAO.LojaID);
                    db.AddInParameter(cmd, "@DataOcorrencia", DbType.DateTime, ocorrenciaDAO.DataOcorrencia);
                    db.AddInParameter(cmd, "@MotivoOcorrenciaID", DbType.Int32, ocorrenciaDAO.MotivoOcorrenciaID);
                    if (!string.IsNullOrEmpty(ocorrenciaDAO.NomeMotorista))
                    {
                        db.AddInParameter(cmd, "@NomeMotorista", DbType.String, ocorrenciaDAO.NomeMotorista);
                    }
                    if (!string.IsNullOrEmpty(ocorrenciaDAO.PlacaCaminhao))
                    {
                        db.AddInParameter(cmd, "@PlacaCaminhao", DbType.String, ocorrenciaDAO.PlacaCaminhao);
                    }
                    if (!string.IsNullOrEmpty(ocorrenciaDAO.Observacao))
                    {
                        db.AddInParameter(cmd, "@Observacao", DbType.String, ocorrenciaDAO.Observacao);
                    }
                    db.AddInParameter(cmd, "@StatusOcorrenciaID", DbType.Int32, ocorrenciaDAO.StatusOcorrenciaID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, ocorrenciaDAO.SistemaID);

                    ocorrenciaId = Convert.ToInt32(db.ExecuteScalar(cmd));
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

            return ocorrenciaId;
        }

        public void DarBaixaOcorrencia(OcorrenciaDAO ocorrenciaDAO)
        {
            Database db;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spDarBaixaOcorrencia"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@OcorrenciaID", DbType.Int32, ocorrenciaDAO.OcorrenciaID);
                    if (ocorrenciaDAO.NumeroTroca != null && ocorrenciaDAO.NumeroTroca > 0)
                    {
                        db.AddInParameter(cmd, "@NumeroTroca", DbType.Int32, ocorrenciaDAO.NumeroTroca);
                    }
                    db.AddInParameter(cmd, "@StatusOcorrenciaID", DbType.Int32, ocorrenciaDAO.StatusOcorrenciaID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, ocorrenciaDAO.SistemaID);

                    db.ExecuteScalar(cmd);
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

        public void DarBaixaProduto(ProdutoDAO produtoDAO)
        {
            Database db;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spDarBaixaOcorrenciaProduto"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoDAO.ProdutoID);
                    db.AddInParameter(cmd, "@Quantidade", DbType.Int16, produtoDAO.Quantidade);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produtoDAO.SistemaID);

                    db.ExecuteScalar(cmd);
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

        public List<OcorrenciaDAO> Listar(OcorrenciaDAO ocorrenciaDAO)
        {
            Database db;
            List<OcorrenciaDAO> ocorrencias = new List<OcorrenciaDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");

                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarOcorrencia"))
                {
                    cmd.CommandTimeout = 300;

                    if (ocorrenciaDAO.OcorrenciaID > 0)
                    {
                        db.AddInParameter(cmd, "@OcorrenciaID", DbType.Int32, ocorrenciaDAO.OcorrenciaID);
                    }

                    if (ocorrenciaDAO.NotaFiscalID > 0)
                    {
                        db.AddInParameter(cmd, "@NotaFiscalID", DbType.Int32, ocorrenciaDAO.NotaFiscalID);
                    }

                    if (ocorrenciaDAO.LojaID > 0)
                    {
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, ocorrenciaDAO.LojaID);
                    }

                    if (ocorrenciaDAO.DataOcorrenciaInicial != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataOcorrenciaInicial", DbType.DateTime, ocorrenciaDAO.DataOcorrenciaInicial);
                    }

                    if (ocorrenciaDAO.DataOcorrenciaFinal != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DataOcorrenciaFinal", DbType.DateTime, ocorrenciaDAO.DataOcorrenciaFinal.AddHours(23).AddMinutes(59).AddSeconds(59));
                    }

                    if (!string.IsNullOrEmpty(ocorrenciaDAO.NomeMotorista))
                    {
                        db.AddInParameter(cmd, "@NomeMotorista", DbType.String, ocorrenciaDAO.NomeMotorista);
                    }

                    if (!string.IsNullOrEmpty(ocorrenciaDAO.PlacaCaminhao))
                    {
                        db.AddInParameter(cmd, "@PlacaCaminhao", DbType.String, ocorrenciaDAO.PlacaCaminhao);
                    }

                    if (ocorrenciaDAO.MotivoOcorrenciaID > 0)
                    {
                        db.AddInParameter(cmd, "@MotivoOcorrenciaID", DbType.Int32, ocorrenciaDAO.MotivoOcorrenciaID);
                    }

                    if (ocorrenciaDAO.NumeroTroca != null && ocorrenciaDAO.NumeroTroca > 0)
                    {
                        db.AddInParameter(cmd, "@NumeroTroca", DbType.Int32, ocorrenciaDAO.NumeroTroca);
                    }

                    if (ocorrenciaDAO.StatusOcorrenciaID > 0)
                    {
                        db.AddInParameter(cmd, "@StatusOcorrenciaID", DbType.Int32, ocorrenciaDAO.StatusOcorrenciaID);
                    }

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, ocorrenciaDAO.SistemaID);

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        OcorrenciaDAO ocorrencia = new OcorrenciaDAO();

                        ocorrencia.OcorrenciaID = Convert.ToInt32(item["OcorrenciaID"]);
                        ocorrencia.NotaFiscalID = Convert.ToInt32(item["NotaFiscalID"]);
                        ocorrencia.LojaID = Convert.ToInt32(item["LojaID"]);
                        ocorrencia.NomeFantasia = item["NomeFantasia"].ToString();
                        ocorrencia.MotivoOcorrenciaID = Convert.ToInt32(item["MotivoOcorrenciaID"]);
                        ocorrencia.MotivoOcorrencia = item["MotivoOcorrencia"].ToString();
                        ocorrencia.PlacaCaminhao = item["PlacaCaminhao"].ToString();
                        ocorrencia.NomeMotorista = item["NomeMotorista"].ToString();
                        if (item["DataOcorrencia"] != DBNull.Value)
                        {
                            ocorrencia.DataOcorrencia = Convert.ToDateTime(item["DataOcorrencia"]);
                        }
                        ocorrencia.Observacao = item["Observacao"].ToString();
                        if (item["NumeroTroca"] != DBNull.Value)
                        {
                            ocorrencia.NumeroTroca = Convert.ToInt32(item["NumeroTroca"]);
                        }
                        ocorrencia.StatusOcorrenciaID = Convert.ToInt32(item["StatusOcorrenciaID"]);
                        ocorrencia.SistemaID = Convert.ToInt32(item["SistemaID"]);

                        ocorrencias.Add(ocorrencia);
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

            return ocorrencias;
        }
    }
}
