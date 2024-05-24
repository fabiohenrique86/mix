using DAO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;

namespace DAL
{
    public class ProdutoDAL
    {
        public void Zerar(ProdutoDAO produtoDAO)
        {
            Database db;
            try
            {
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.Timeout = TimeSpan.FromMinutes(5);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    db = DatabaseFactory.CreateDatabase("Mix");
                    using (DbCommand cmd = db.GetStoredProcCommand("dbo.spZerarEstoque"))
                    {
                        cmd.CommandTimeout = 300;

                        if (produtoDAO.LojaID > 0)
                        {
                            db.AddInParameter(cmd, "@LojaID", DbType.Int32, produtoDAO.LojaID);
                        }

                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produtoDAO.SistemaID);

                        db.ExecuteNonQuery(cmd);
                    }

                    scope.Complete();
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

        public int Atualizar(ProdutoDAO produtoDAO)
        {
            var linhasAfetadas = 0;

            Database db;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarProduto"))
                {
                    cmd.CommandTimeout = 300;

                    if (produtoDAO.LojaID == 0)
                        produtoDAO.LojaID = null;

                    if (produtoDAO.LinhaID == 0)
                        produtoDAO.LinhaID = null;

                    if (produtoDAO.ComissaoFuncionario == 0)
                        produtoDAO.ComissaoFuncionario = null;

                    if (produtoDAO.ComissaoFranqueado == 0)
                        produtoDAO.ComissaoFranqueado = null;

                    if (string.IsNullOrEmpty(produtoDAO.Descricao))
                        produtoDAO.Descricao = null;

                    if (produtoDAO.MedidaID == 0)
                        produtoDAO.MedidaID = null;

                    if (produtoDAO.Quantidade == 0)
                        produtoDAO.Quantidade = null;
                    
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoDAO.ProdutoID);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, produtoDAO.LojaID);
                    db.AddInParameter(cmd, "@LinhaID", DbType.Int32, produtoDAO.LinhaID);
                    db.AddInParameter(cmd, "@ComissaoFuncionario", DbType.Int16, produtoDAO.ComissaoFuncionario);
                    db.AddInParameter(cmd, "@ComissaoFranqueado", DbType.Int16, produtoDAO.ComissaoFranqueado);
                    db.AddInParameter(cmd, "@Descricao", DbType.String, produtoDAO.Descricao);
                    db.AddInParameter(cmd, "@MedidaID", DbType.Int32, produtoDAO.MedidaID);
                    db.AddInParameter(cmd, "@Quantidade", DbType.Int16, produtoDAO.Quantidade);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produtoDAO.SistemaID);

                    linhasAfetadas = db.ExecuteNonQuery(cmd);
                }

                return linhasAfetadas;
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

        public void AtualizarViaArquivoDeColeta(ProdutoDAO produtoDAO)
        {
            Database db;
            try
            {
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.Timeout = TimeSpan.FromMinutes(5);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    db = DatabaseFactory.CreateDatabase("Mix");
                    using (DbCommand cmd = db.GetStoredProcCommand("dbo.spAtualizarProdutoViaArquivoDeColeta"))
                    {
                        cmd.CommandTimeout = 300;

                        db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoDAO.ProdutoID);
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, produtoDAO.LojaID);
                        db.AddInParameter(cmd, "@Quantidade", DbType.Int16, produtoDAO.Quantidade);
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produtoDAO.SistemaID);

                        db.ExecuteNonQuery(cmd);
                    }

                    scope.Complete();
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

        public List<string> Atualizar(List<ProdutoDAO> produtosDAO)
        {
            var listaRetorno = new List<string>();

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(5) }))
                {
                    foreach (var produtoDAO in produtosDAO)
                    {
                        var linhasAfetadas = Atualizar(produtoDAO);

                        if (linhasAfetadas <= 0)
                            listaRetorno.Add("Produto " + produtoDAO.ProdutoID.ToString() + " não foi atualizado");
                        else
                            listaRetorno.Add("Produto " + produtoDAO.ProdutoID.ToString() + " atualizado com sucesso");
                    }

                    scope.Complete();
                }

                return listaRetorno;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao atualizar registro", ex);
            }
        }

        public void AtualizarViaArquivoDeColeta(List<ProdutoDAO> ProdutosDAO)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(5) }))
                {
                    foreach (ProdutoDAO produtoDAO in ProdutosDAO)
                    {
                        AtualizarViaArquivoDeColeta(produtoDAO);
                    }

                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao atualizar registro", ex);
            }
        }

        public void Excluir(List<DAO.ProdutoDAO> Produtos)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (ProdutoDAO produtoDAO in Produtos)
                    {
                        Excluir(produtoDAO);
                    }
                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao atualizar registro", ex);
            }
        }

        public void Excluir(DAO.ProdutoDAO produtoDAO)
        {
            Database db;
            try
            {
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.Timeout = TimeSpan.FromMinutes(5);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    db = DatabaseFactory.CreateDatabase("Mix");
                    using (DbCommand cmd = db.GetStoredProcCommand("dbo.spExcluirProduto"))
                    {
                        cmd.CommandTimeout = 300;
                        db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoDAO.ProdutoID);
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, produtoDAO.LojaID);
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produtoDAO.SistemaID);
                        db.ExecuteNonQuery(cmd);
                    }
                    scope.Complete();
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

        public bool ExisteNaLoja(string produtoId, string lojaId, int sistemaId)
        {
            Database db;
            bool produto = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoLoja"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(lojaId))
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, Convert.ToInt64(produtoId));
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        produto = true;
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
            return produto;
        }

        public void Inserir(List<DAO.ProdutoDAO> Produtos)
        {
            try
            {
                TransactionOptions transactionOptions = new TransactionOptions();
                transactionOptions.Timeout = TimeSpan.FromMinutes(5);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    foreach (ProdutoDAO produtoDAO in Produtos)
                    {
                        Inserir(produtoDAO);
                    }
                    scope.Complete();
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao inserir registro", ex);
            }
        }

        public void Inserir(ProdutoDAO produtoDAO)
        {
            Database db;
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    db = DatabaseFactory.CreateDatabase("Mix");
                    using (DbCommand cmd = db.GetStoredProcCommand("dbo.spInserirProduto"))
                    {
                        cmd.CommandTimeout = 300;
                        db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoDAO.ProdutoID);
                        db.AddInParameter(cmd, "@LojaID", DbType.Int32, produtoDAO.LojaID);
                        db.AddInParameter(cmd, "@LinhaID", DbType.Int32, produtoDAO.LinhaID);
                        db.AddInParameter(cmd, "@ComissaoFuncionario", DbType.Int16, produtoDAO.ComissaoFuncionario);
                        db.AddInParameter(cmd, "@ComissaoFranqueado", DbType.Int16, produtoDAO.ComissaoFranqueado);
                        db.AddInParameter(cmd, "@Descricao", DbType.String, produtoDAO.Descricao);
                        db.AddInParameter(cmd, "@MedidaID", DbType.Int32, produtoDAO.MedidaID);
                        db.AddInParameter(cmd, "@Quantidade", DbType.Int16, produtoDAO.Quantidade);
                        db.AddInParameter(cmd, "@SistemaID", DbType.Int32, produtoDAO.SistemaID);
                        db.ExecuteNonQuery(cmd);
                    }
                    scope.Complete();
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

        public DataSet Listar(string lojaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProduto"))
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

        public bool Listar(long produtoId, string cnpj, int sistemaId)
        {
            Database db;
            bool produto = false;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoLojaByCnpj"))
                {
                    cmd.CommandTimeout = 300;
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
                    db.AddInParameter(cmd, "@Cnpj", DbType.String, cnpj);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        produto = true;
                    }
                }
                //return produto;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return produto;
        }

        public DataSet Listar(string lojaId, string linhaId, string comissaoFuncionario, string comissaoFranqueado, string descricao, string medidaId, int sistemaId, long produtoId = 0, bool flgExibirForaDeLinha = false)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoFiltro"))
                {
                    cmd.CommandTimeout = 300;

                    if (linhaId == "0")
                        linhaId = null;

                    if (string.IsNullOrEmpty(comissaoFuncionario))
                        comissaoFuncionario = null;

                    if (string.IsNullOrEmpty(comissaoFranqueado))
                        comissaoFranqueado = null;

                    if (string.IsNullOrEmpty(descricao))
                        descricao = null;

                    if (medidaId == "0")
                        medidaId = null;

                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@LinhaID", DbType.Int32, linhaId);
                    db.AddInParameter(cmd, "@ComissaoFuncionario", DbType.Int16, comissaoFuncionario);
                    db.AddInParameter(cmd, "@ComissaoFranqueado", DbType.Int16, comissaoFranqueado);
                    db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
                    db.AddInParameter(cmd, "@MedidaID", DbType.Int32, medidaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

                    if (produtoId > 0)
                        db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);

                    if (flgExibirForaDeLinha)
                        db.AddInParameter(cmd, "@FlgExibirForaDeLinha", DbType.Boolean, flgExibirForaDeLinha);

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

        public DataSet ListarDropDownList(string lojaId, int sistemaId)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoDropDownList"))
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

        public DataSet ListarGridView(string lojaId, int sistemaId, string descricao = null)
        {
            Database db;
            DataSet ds;
            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoGridView"))
                {
                    cmd.CommandTimeout = 300;
                    if (string.IsNullOrEmpty(lojaId) || (lojaId == "0"))
                    {
                        lojaId = null;
                    }
                    if (!string.IsNullOrEmpty(descricao))
                    {
                        db.AddInParameter(cmd, "@Descricao", DbType.String, descricao);
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

        public bool ExisteNaLoja(long produtoId, string lojaId, int sistemaId)
        {
            Database db;
            var produto = false;

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoLoja"))
                {
                    if (string.IsNullOrEmpty(lojaId))
                    {
                        lojaId = null;
                    }
                    db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
                    db.AddInParameter(cmd, "@LojaID", DbType.Int32, lojaId);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);
                    if (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0)
                    {
                        produto = true;
                    }
                }
                //return produto;
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Erro ao listar registro", ex);
            }
            finally
            {
                db = null;
            }
            return produto;
        }

        public List<ProdutoDAO> Listar(string descricao, string medida, int sistemaId)
        {
            Database db;
            var retorno = new List<ProdutoDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");
                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoFiltro2"))
                {
                    cmd.CommandTimeout = 300;

                    db.AddInParameter(cmd, "@Descricao", DbType.String, descricao.Replace("  ", " ")); // retira os espaços duplos
                    db.AddInParameter(cmd, "@Medida", DbType.String, medida);
                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

                    var dr = db.ExecuteReader(cmd);

                    while (dr.Read())
                    {
                        var produtoDAO = new ProdutoDAO();

                        produtoDAO.ProdutoID = Convert.ToInt64(dr["ProdutoID"]);
                        produtoDAO.LinhaID = Convert.ToInt32(dr["LinhaID"]);
                        produtoDAO.Descricao = dr["Descricao"].ToString();
                        produtoDAO.MedidaID = Convert.ToInt32(dr["MedidaID"]);
                        produtoDAO.ComissaoFuncionario = Convert.ToInt16(dr["ComissaoFuncionario"]);
                        produtoDAO.ComissaoFranqueado = Convert.ToInt16(dr["ComissaoFranqueado"]);
                        produtoDAO.SistemaID = Convert.ToInt32(dr["SistemaID"]);

                        retorno.Add(produtoDAO);
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

            return retorno;
        }

        //public ProdutoDAO ListarProdutoLojaById(long produtoId, int sistemaId)
        //{
        //    Database db;
        //    var produtoDAO = new ProdutoDAO();

        //    try
        //    {
        //        db = DatabaseFactory.CreateDatabase("Mix");
        //        using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarProdutoLojaById"))
        //        {
        //            cmd.CommandTimeout = 300;

        //            db.AddInParameter(cmd, "@ProdutoID", DbType.Int64, produtoId);
        //            db.AddInParameter(cmd, "@SistemaID", DbType.Int32, sistemaId);

        //            var dr = db.ExecuteReader(cmd);

        //            while (dr.Read())
        //            {
        //                produtoDAO.ProdutoID = Convert.ToInt64(dr["ProdutoID"]);
        //                produtoDAO.LinhaID = Convert.ToInt32(dr["LinhaID"]);
        //                produtoDAO.Descricao = dr["Descricao"].ToString();
        //                produtoDAO.MedidaID = Convert.ToInt32(dr["MedidaID"]);
        //                produtoDAO.ComissaoFuncionario = Convert.ToInt16(dr["ComissaoFuncionario"]);
        //                produtoDAO.ComissaoFranqueado = Convert.ToInt16(dr["ComissaoFranqueado"]);
        //                produtoDAO.SistemaID = Convert.ToInt32(dr["SistemaID"]);
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new ApplicationException("Erro ao listar registro", ex);
        //    }
        //    finally
        //    {
        //        db = null;
        //    }
        //    return produtoDAO;
        //}
    }
}
