using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;
using BLL;
using DAO;
using DAL;

namespace Site
{
    public partial class QuadroClassificacaoRel : System.Web.UI.Page
    {
        private void CarregarDropDownListFuncionario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((this.Session["dsDropDownListFuncionario"] == null) || (this.Session["bdFuncionario"] != null))
            {
                this.Session["dsDropDownListFuncionario"] = DAL.FuncionarioDAL.ListarDropDownList(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
                this.Session["bdFuncionario"] = null;
            }
            this.ddlFuncionario.DataSource = this.Session["dsDropDownListFuncionario"];
            this.ddlFuncionario.DataBind();
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.ddlLoja.DataSource = new DAL.LojaDAL().ListarDropDownList(usuarioSessao.SistemaID);
            this.ddlLoja.DataBind();
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
                    if ((this.Session["DiaMesAnoInicial"] == null) || (this.Session["DiaMesAnoFinal"] == null))
                    {
                        throw new ApplicationException("É necessário informar todas as datas para visualizar o relatório.");
                    }
                    this.rpvQuadroClassificacao.ProcessingMode = ProcessingMode.Local;
                    this.rpvQuadroClassificacao.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "QuadroClassificacao.rdlc";
                    ReportDataSource rdsTeste = new ReportDataSource
                    {
                        Name = "dsQuadroClassificacao_spListarQuadroClassificacao",
                        Value = RelatorioDAL.ListarQuadroClassificacao(Convert.ToDateTime(this.Session["DiaMesAnoInicial"]), Convert.ToDateTime(this.Session["DiaMesAnoFinal"]), usuarioSessao.SistemaID, new int?(string.IsNullOrWhiteSpace(this.ddlLoja.SelectedValue) ? 0 : Convert.ToInt32(this.ddlLoja.SelectedValue)), new int?(string.IsNullOrWhiteSpace(this.ddlFuncionario.SelectedValue) ? 0 : Convert.ToInt32(this.ddlFuncionario.SelectedValue)))
                    };
                    ReportDataSource rdsTeste2 = new ReportDataSource
                    {
                        Name = "dsQuadroClassificacao2_spListarQuadroClassificacao2",
                        Value = RelatorioDAL.ListarQuadroClassificacao2(Convert.ToDateTime(this.Session["DiaMesAnoInicial"]), Convert.ToDateTime(this.Session["DiaMesAnoFinal"]), usuarioSessao.SistemaID, new int?(string.IsNullOrWhiteSpace(this.ddlLoja.SelectedValue) ? 0 : Convert.ToInt32(this.ddlLoja.SelectedValue)), new int?(string.IsNullOrWhiteSpace(this.ddlFuncionario.SelectedValue) ? 0 : Convert.ToInt32(this.ddlFuncionario.SelectedValue)))
                    };
                    this.rpvQuadroClassificacao.LocalReport.DataSources.Clear();
                    this.rpvQuadroClassificacao.LocalReport.DataSources.Add(rdsTeste);
                    this.rpvQuadroClassificacao.LocalReport.DataSources.Add(rdsTeste2);
                    this.rpvQuadroClassificacao.LocalReport.Refresh();
                    this.mpeQuadroClassificacao.Show();
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, "Data inválida");
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
                    else if (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID != Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
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
                    this.CarregarDropDownListLoja();
                    this.CarregarDropDownListFuncionario();
                    this.Session["DiaMesAnoInicial"] = null;
                    this.Session["DiaMesAnoFinal"] = null;
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