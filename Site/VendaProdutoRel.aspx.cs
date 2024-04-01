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
    public partial class VendaProdutoRel : System.Web.UI.Page
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

        private void CarregarDropDownListLinha()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((this.Session["dsDropDownListLinha"] == null) || (this.Session["bdLinha"] != null))
            {
                this.Session["dsDropDownListLinha"] = DAL.LinhaDAL.ListarDropDownList(usuarioSessao.SistemaID);
                this.Session["bdLinha"] = null;
            }
            this.ddlLinha.DataSource = this.Session["dsDropDownListLinha"];
            this.ddlLinha.DataBind();
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.ddlLoja.DataSource = new DAL.LojaDAL().ListarOrcamentoDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
            this.ddlLoja.DataBind();
        }

        private void CarregarDropDownListProduto()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.ddlProduto.DataSource = new DAL.ProdutoDAL().ListarDropDownList(string.Empty, usuarioSessao.SistemaID);
            this.ddlProduto.DataBind();
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
                    this.rpvVendaProduto.ProcessingMode = ProcessingMode.Local;
                    this.rpvVendaProduto.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "VendaProduto.rdlc";
                    ReportDataSource rdsTeste = new ReportDataSource
                    {
                        Name = "dsVendaProduto_spListarVendaProduto",
                        Value = RelatorioDAL.ListarVendaProduto(Convert.ToDateTime(this.Session["DiaMesAnoInicial"]), Convert.ToDateTime(this.Session["DiaMesAnoFinal"]), usuarioSessao.SistemaID, new int?(string.IsNullOrWhiteSpace(this.ddlLoja.SelectedValue) ? 0 : Convert.ToInt32(this.ddlLoja.SelectedValue)), new int?(string.IsNullOrWhiteSpace(this.ddlLinha.SelectedValue) ? 0 : Convert.ToInt32(this.ddlLinha.SelectedValue)), new long?(string.IsNullOrWhiteSpace(this.ddlProduto.SelectedValue) ? 0L : Convert.ToInt64(this.ddlProduto.SelectedValue)), new int?(string.IsNullOrWhiteSpace(this.ddlFuncionario.SelectedValue) ? 0 : Convert.ToInt32(this.ddlFuncionario.SelectedValue)))
                    };
                    this.rpvVendaProduto.LocalReport.DataSources.Clear();
                    this.rpvVendaProduto.LocalReport.DataSources.Add(rdsTeste);
                    this.rpvVendaProduto.LocalReport.Refresh();
                    this.mpeVendaProduto.Show();
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
                    else if (
                            (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode()) && 
                            (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode())
                            )
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
                    this.CarregarDropDownListLinha();
                    this.CarregarDropDownListProduto();
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