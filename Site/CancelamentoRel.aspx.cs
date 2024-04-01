using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;
using BLL;

namespace Site
{
    public partial class CancelamentoRel : System.Web.UI.Page
    {
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
                    this.rpvCancelamento.ProcessingMode = ProcessingMode.Local;
                    this.rpvCancelamento.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "Cancelamento.rdlc";
                    ReportDataSource rdsTeste = new ReportDataSource
                    {
                        Name = "dsCancelamentoPedido_spListarCancelamentoPedido",
                        Value = DAL.RelatorioDAL.ListarCancelamento(Convert.ToDateTime(this.Session["DiaMesAnoInicial"]), Convert.ToDateTime(this.Session["DiaMesAnoFinal"]), usuarioSessao.SistemaID, new int?(string.IsNullOrWhiteSpace(this.ddlLoja.SelectedValue) ? 0 : Convert.ToInt32(this.ddlLoja.SelectedValue)))
                    };
                    this.rpvCancelamento.LocalReport.DataSources.Clear();
                    this.rpvCancelamento.LocalReport.DataSources.Add(rdsTeste);
                    this.rpvCancelamento.LocalReport.Refresh();
                    this.mpeCancelamento.Show();
                    this.Session["DiaMesAnoInicial"] = null;
                    this.Session["DiaMesAnoFinal"] = null;
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
                    else if (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID != Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador) && new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID != Convert.ToInt32(UtilitarioBLL.TipoUsuario.Logistica))
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