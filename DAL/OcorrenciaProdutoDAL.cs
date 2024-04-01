using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using DAO;

namespace DAL
{
    public class OcorrenciaProdutoDAL
    {
        public void Inserir(ProdutoDAO produtoDAO)
        {
            Database db;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirOcorrenciaProduto"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@OcorrenciaID", DbType.Int32, produtoDAO.OcorrenciaID);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoDAO.ProdutoID);
                    db.AddInParameter(cmd, "@Quantidade", DbType.Int16, produtoDAO.Quantidade);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produtoDAO.SistemaID);

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

        public List<ProdutoDAO> Listar(ProdutoDAO produtoDAO)
        {
            Database db;
            List<ProdutoDAO> produtos = new List<ProdutoDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");

                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarOcorrenciaProduto"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@OcorrenciaID", DbType.Int32, produtoDAO.OcorrenciaID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produtoDAO.SistemaID);

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ProdutoDAO produto = new ProdutoDAO();

                        produto.OcorrenciaID = Convert.ToInt32(item["OcorrenciaID"]);
                        produto.ProdutoID = Convert.ToInt64(item["ProdutoID"]);
                        produto.Descricao = item["Produto"].ToString();
                        produto.Quantidade = Convert.ToInt16(item["Quantidade"]);
                        produto.Medida = item["Medida"].ToString();
                        produto.SistemaID = Convert.ToInt32(item["SistemaID"]);

                        produtos.Add(produto);
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

            return produtos;
        }
    }
}
