using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using DAO;

namespace DAL
{
    public class TrocaProdutoDAL
    {
        public void Incluir(TrocaProdutoDAO trocaProdutoDAO)
        {
            Database db;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirTrocaProduto"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@TrocaID", DbType.Int32, trocaProdutoDAO.TrocaID);
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, trocaProdutoDAO.ProdutoDAO.ProdutoID);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, trocaProdutoDAO.ProdutoDAO.LojaID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, trocaProdutoDAO.ProdutoDAO.SistemaID);
                    db.AddInParameter(cmd, "@Quantidade", DbType.Int16, trocaProdutoDAO.ProdutoDAO.Quantidade);
                    db.AddInParameter(cmd, "@Medida", DbType.String, trocaProdutoDAO.ProdutoDAO.Medida);

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

        public List<TrocaProdutoDAO> Listar(TrocaDAO trocaDAO)
        {
            Database db;
            var listaTroca = new List<TrocaProdutoDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");

                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarTrocaProduto"))
                {
                    cmd.CommandTimeout = 300;
                    
                    db.AddInParameter(cmd, "@TrocaID", DbType.Int32, trocaDAO.TrocaID);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, trocaDAO.SistemaID);

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var Troca = new TrocaProdutoDAO();

                        Troca.TrocaID = Convert.ToInt32(item["TrocaID"]);
                        Troca.ProdutoDAO.ProdutoID = Convert.ToInt64(item["ProdutoID"]);
                        Troca.ProdutoDAO.Descricao = item["Descricao"].ToString();
                        Troca.ProdutoDAO.Medida = item["Medida"].ToString();
                        Troca.ProdutoDAO.Quantidade = Convert.ToInt16(item["Quantidade"]);
                        Troca.ProdutoDAO.SistemaID = Convert.ToInt32(item["SistemaID"]);
                        Troca.ProdutoDAO.Produto = item["Produto"].ToString();

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
    }
}
