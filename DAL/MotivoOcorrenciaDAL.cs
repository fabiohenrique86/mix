using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using DAO;

namespace DAL
{
    public class MotivoOcorrenciaDAL
    {
        public List<MotivoOcorrenciaDAO> Listar(MotivoOcorrenciaDAO motivoOcorrenciaDAO)
        {
            Database db;
            List<MotivoOcorrenciaDAO> motivoOcorrencias = new List<MotivoOcorrenciaDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");

                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarMotivoOcorrencia"))
                {
                    cmd.CommandTimeout = 300;
                    
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, motivoOcorrenciaDAO.SistemaID);

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        MotivoOcorrenciaDAO motivoOcorrencia = new MotivoOcorrenciaDAO();

                        motivoOcorrencia.MotivoOcorrenciaID = Convert.ToInt32(item["MotivoOcorrenciaID"]);
                        motivoOcorrencia.Descricao = item["Descricao"].ToString();
                        motivoOcorrencia.SistemaID = Convert.ToInt32(item["SistemaID"]);

                        motivoOcorrencias.Add(motivoOcorrencia);
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

            return motivoOcorrencias;
        }
    }
}
