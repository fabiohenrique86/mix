using BLL;
using DAL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.UI;

namespace Site
{
    public partial class EstoqueRel : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!base.IsPostBack)
                {
                    if (!UtilitarioBLL.PermissaoUsuario(Session["Usuario"]))
                    {
                        UtilitarioBLL.Sair();
                        if (base.Request.Url.Segments.Length == 3)
                        {
                            base.Response.Redirect("../Default.aspx", false);
                        }
                        else
                        {
                            base.Response.Redirect("Default.aspx", false);
                        }
                    }
                    else
                    {
                        BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                        if (usuarioSessao.SistemaID != 5 && usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode())
                        {
                            UtilitarioBLL.Sair();
                            if (base.Request.Url.Segments.Length == 3)
                            {
                                base.Response.Redirect("../Default.aspx", false);
                            }
                            else
                            {
                                base.Response.Redirect("Default.aspx", false);
                            }
                        }
                        else
                        {
                            CarregarDropDownListLoja();
                        }
                    }
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
            }
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            ddlLoja.DataSource = new LojaDAL().ListarDropDownList(usuarioSessao.SistemaID);
            ddlLoja.DataBind();
        }

        protected void imbVisualizar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    BLL.AplicacaoBLL.Empresa = null;

                    if (base.Request.Url.Segments.Length == 3)
                    {
                        base.Response.Redirect("../Default.aspx", true);
                    }
                    else
                    {
                        base.Response.Redirect("Default.aspx", true);
                    }
                }
                else
                {
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                    rpvEstoque.ProcessingMode = ProcessingMode.Local;
                    rpvEstoque.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "Estoque.rdlc";

                    int? lojaId = null;
                    int l;
                    lojaId = int.TryParse(ddlLoja.SelectedValue, out l) ? (int?)l : null;

                    var linhas = RelatorioDAL.ListarEstoque(usuarioSessao.SistemaID, lojaId);
                    List<dynamic> produtos = new List<dynamic>();
                    List<string> arquivoColeta = null;

                    var files = Request.Files;
                    if (files != null && files.Count > 0)
                    {
                        using (StreamReader sr = new StreamReader(files[0].InputStream))
                        {
                            var a = sr.ReadToEnd();
                            arquivoColeta = a.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                        }
                    }

                    foreach (DataRow linhaBancoDeDados in linhas.Rows)
                    {
                        var qtdColeta = 0;

                        if (arquivoColeta != null && arquivoColeta.Count > 0)
                        {
                            foreach (var linhaArquivoColeta in arquivoColeta)
                            {
                                var campos = linhaArquivoColeta.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                if (Convert.ToInt64(linhaBancoDeDados["ProdutoID"]) == Convert.ToInt64(campos[1]))
                                {
                                    qtdColeta = Convert.ToInt32(campos[5]);
                                    break;
                                }
                            }
                        }

                        if (qtdColeta > 0)
                        {
                            dynamic p1 = new
                            {
                                LojaID = Convert.ToInt32(linhaBancoDeDados["LojaID"]),
                                NomeFantasia = linhaBancoDeDados["NomeFantasia"].ToString(),
                                LinhaID = Convert.ToInt32(linhaBancoDeDados["LinhaID"]),
                                Linha = linhaBancoDeDados["Linha"].ToString(),
                                ProdutoID = Convert.ToInt64(linhaBancoDeDados["ProdutoID"]),
                                Produto = linhaBancoDeDados["Produto"].ToString(),
                                QuantidadeEstoque = Convert.ToInt32(linhaBancoDeDados["QuantidadeEstoque"]),
                                QuantidadeReservada = Convert.ToInt32(linhaBancoDeDados["QuantidadeReservada"]),
                                QuantidadeDanificada = Convert.ToInt32(linhaBancoDeDados["QuantidadeDanificada"]),
                                QuantidadeColeta = qtdColeta,
                                QuantidadeTroca = Convert.ToInt32(linhaBancoDeDados["QuantidadeTroca"]),
                            };

                            produtos.Add(p1);
                        }
                        else
                        {
                            dynamic p2 = new
                            {
                                LojaID = Convert.ToInt32(linhaBancoDeDados["LojaID"]),
                                NomeFantasia = linhaBancoDeDados["NomeFantasia"].ToString(),
                                LinhaID = Convert.ToInt32(linhaBancoDeDados["LinhaID"]),
                                Linha = linhaBancoDeDados["Linha"].ToString(),
                                ProdutoID = Convert.ToInt64(linhaBancoDeDados["ProdutoID"]),
                                Produto = linhaBancoDeDados["Produto"].ToString(),
                                QuantidadeEstoque = Convert.ToInt32(linhaBancoDeDados["QuantidadeEstoque"]),
                                QuantidadeReservada = Convert.ToInt32(linhaBancoDeDados["QuantidadeReservada"]),
                                QuantidadeDanificada = Convert.ToInt32(linhaBancoDeDados["QuantidadeDanificada"]),
                                QuantidadeColeta = 0,
                                QuantidadeTroca = Convert.ToInt32(linhaBancoDeDados["QuantidadeTroca"]),
                            };

                            produtos.Add(p2);
                        }
                    }

                    ReportDataSource rdsTeste = new ReportDataSource
                    {
                        Name = "dsEstoque_spListarEstoque",
                        Value = produtos
                    };

                    rpvEstoque.LocalReport.SetParameters(new ReportParameter("P_ArquivoColeta", (arquivoColeta != null && arquivoColeta.Count > 0) ? "1" : "0"));
                    rpvEstoque.LocalReport.SetParameters(new ReportParameter("P_SistemaID", usuarioSessao.SistemaID.ToString()));

                    rpvEstoque.LocalReport.DataSources.Clear();
                    rpvEstoque.LocalReport.DataSources.Add(rdsTeste);
                    rpvEstoque.LocalReport.Refresh();

                    mpeEstoque.Show();
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message, ex);
            }
        }
    }
}