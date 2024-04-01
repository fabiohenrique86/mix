using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using DAO;

namespace DAL
{
    public class FreteDAL
    {
        public List<FreteDAO> Listar(FreteDAO freteDAO)
        {
            Database db;
            var listaFrete = new List<FreteDAO>();

            try
            {
                db = DatabaseFactory.CreateDatabase("Mix");

                using (DbCommand cmd = db.GetStoredProcCommand("dbo.spListarFrete"))
                {
                    cmd.CommandTimeout = 300;

                    if (!string.IsNullOrEmpty(freteDAO.NmCidade))
                    {
                        db.AddInParameter(cmd, "@NmCidade", DbType.String, freteDAO.NmCidade);
                    }

                    if (!string.IsNullOrEmpty(freteDAO.NomeCarreteiro))
                    {
                        db.AddInParameter(cmd, "@NmCarreteiro", DbType.String, freteDAO.NomeCarreteiro);
                    }

                    if (freteDAO.FuncionarioID > 0)
                    {
                        db.AddInParameter(cmd, "@FuncionarioID", DbType.Int32, freteDAO.FuncionarioID);
                    }

                    if (freteDAO.DtReservaInicial != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DtReservaInicial", DbType.DateTime, freteDAO.DtReservaInicial);
                    }

                    if (freteDAO.DtReservaFinal != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DtReservaFinal", DbType.DateTime, freteDAO.DtReservaFinal);
                    }

                    if (freteDAO.DtEntregaInicial != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DtEntregaInicial", DbType.DateTime, freteDAO.DtEntregaInicial);
                    }

                    if (freteDAO.DtEntregaFinal != DateTime.MinValue)
                    {
                        db.AddInParameter(cmd, "@DtEntregaFinal", DbType.DateTime, freteDAO.DtEntregaFinal);
                    }

                    db.AddInParameter(cmd, "@SistemaID", DbType.Int32, freteDAO.SistemaID);

                    var ds = db.ExecuteDataSet(cmd);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var frete = new FreteDAO();

                        frete.PedidoID = item["PedidoID"].ToString();
                        frete.LojaOrigemID = Convert.ToInt32(item["LojaOrigemID"]);
                        frete.DscLojaOrigem = item["DscLojaOrigem"].ToString();
                        frete.FuncionarioID = Convert.ToInt32(item["FuncionarioID"]);
                        frete.NmFuncionario = item["NmFuncionario"].ToString();

                        if (item["ValorFrete"] != DBNull.Value)
                        {
                            frete.ValorFrete = Convert.ToDecimal(item["ValorFrete"]);
                        }

                        if (item["CV"] != DBNull.Value && item["CV"].ToString() != "0")
                        {
                            frete.CV = item["CV"].ToString();
                        }

                        frete.NmCidade = item["NmCidade"].ToString();

                        if (item["DtReserva"] != DBNull.Value)
                        {
                            frete.DtReserva = Convert.ToDateTime(item["DtReserva"]);
                        }

                        if (item["DtEntrega"] != DBNull.Value)
                        {
                            frete.DtEntrega = Convert.ToDateTime(item["DtEntrega"]);
                        }

                        frete.StEntregaID = Convert.ToInt32(item["StEntregaID"]);
                        frete.DscEntrega = item["DscEntrega"].ToString();
                        frete.NomeCarreteiro = item["NomeCarreteiro"].ToString();

                        listaFrete.Add(frete);
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

            return listaFrete;
        }
    }
}
