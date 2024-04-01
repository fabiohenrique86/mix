using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using BLL;
using System.Web.Hosting;
using DAL;
using DAO;

namespace Site
{
    public partial class EstoqueIdealRel : System.Web.UI.Page
    {
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
                    if ((this.Session["MesAnoInicial"] == null) || (this.Session["MesAnoFinal"] == null))
                    {
                        throw new ApplicationException("É necessário informar todas as datas para visualizar o relatório.");
                    }
                    this.rpvEstoqueIdeal.ProcessingMode = ProcessingMode.Local;
                    this.rpvEstoqueIdeal.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "EstoqueIdeal.rdlc";
                    ReportDataSource rdsTeste = new ReportDataSource
                    {
                        Name = "dsEstoqueIdeal_spListarEstoqueIdeal",
                        Value = RelatorioDAL.ListarEstoqueIdeal(this.Session["MesAnoInicial"].ToString(), this.Session["MesAnoFinal"].ToString(), usuarioSessao.SistemaID)
                    };
                    this.rpvEstoqueIdeal.LocalReport.DataSources.Clear();
                    this.rpvEstoqueIdeal.LocalReport.DataSources.Add(rdsTeste);
                    this.rpvEstoqueIdeal.LocalReport.Refresh();
                    this.mpeEstoqueIdeal.Show();
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
                            (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode()) ||
                            (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode()) ||
                            (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == UtilitarioBLL.TipoUsuario.Vendedor.GetHashCode())
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