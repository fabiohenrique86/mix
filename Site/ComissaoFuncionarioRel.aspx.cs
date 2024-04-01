using System;
using System.Linq;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;
using BLL;
using DAL;
using System.Data;

namespace Site
{
    public partial class ComissaoFuncionarioRel : Page
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

                    CarregarDropDownListLoja();
                    CarregarDropDownListFuncionario();
                    Session["DiaMesAnoInicial"] = null;
                    Session["DiaMesAnoFinal"] = null;
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

        private void CarregarDropDownListFuncionario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            if ((Session["dsDropDownListFuncionario"] == null) || (Session["bdFuncionario"] != null))
            {
                Session["dsDropDownListFuncionario"] = DAL.FuncionarioDAL.ListarDropDownList(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
                Session["bdFuncionario"] = null;
            }

            ddlFuncionario.DataSource = Session["dsDropDownListFuncionario"];
            ddlFuncionario.DataBind();
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            ddlLoja.DataSource = new DAL.LojaDAL().ListarDropDownList(usuarioSessao.SistemaID);
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

                    if ((Session["DiaMesAnoInicial"] == null) || (Session["DiaMesAnoFinal"] == null))
                        throw new ApplicationException("É necessário informar todas as datas para visualizar o relatório.");

                    rpvComissaoFuncionario.ProcessingMode = ProcessingMode.Local;
                    rpvComissaoFuncionario.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "ComissaoFuncionario.rdlc";

                    ReportDataSource rdsTeste = new ReportDataSource
                    {
                        Name = "dsComissaoFuncionario_spListarComissaoFuncionario",
                        Value = RelatorioDAL.ListarComissaoFuncionario(Convert.ToDateTime(Session["DiaMesAnoInicial"]), Convert.ToDateTime(Session["DiaMesAnoFinal"]), usuarioSessao.SistemaID, new int?(string.IsNullOrWhiteSpace(ddlLoja.SelectedValue) ? 0 : Convert.ToInt32(ddlLoja.SelectedValue)), new int?(string.IsNullOrWhiteSpace(ddlFuncionario.SelectedValue) ? 0 : Convert.ToInt32(ddlFuncionario.SelectedValue)))
                    };
                    
                    rpvComissaoFuncionario.LocalReport.DataSources.Clear();
                    rpvComissaoFuncionario.LocalReport.DataSources.Add(rdsTeste);
                    rpvComissaoFuncionario.LocalReport.Refresh();

                    mpeComissaoFuncionario.Show();
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida");
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

        protected void imgRelatorioSintetico_Click(object sender, ImageClickEventArgs e)
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
                    DateTime dtReservaInicio;
                    DateTime.TryParse(Session["DiaMesAnoInicial"].ToString(), out dtReservaInicio);

                    DateTime dtReservaFim;
                    DateTime.TryParse(Session["DiaMesAnoFinal"].ToString(), out dtReservaFim);

                    if (dtReservaInicio == DateTime.MinValue || dtReservaFim == DateTime.MinValue)
                        throw new ApplicationException("Informe as datas de reserva início e fim ou as datas informadas são inválidas.");

                    var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                    rpvRelatorioSintetico.ProcessingMode = ProcessingMode.Local;
                    rpvRelatorioSintetico.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "RelatorioSintetico_ComissaoFuncionario.rdlc";

                    var dt = RelatorioDAL.ListarComissaoFucionarioSintetico(Convert.ToDateTime(Session["DiaMesAnoInicial"]), Convert.ToDateTime(Session["DiaMesAnoFinal"]), usuarioSessao.SistemaID, new int?(string.IsNullOrWhiteSpace(ddlLoja.SelectedValue) ? 0 : Convert.ToInt32(ddlLoja.SelectedValue)), new int?(string.IsNullOrWhiteSpace(ddlFuncionario.SelectedValue) ? 0 : Convert.ToInt32(ddlFuncionario.SelectedValue)));

                    ReportDataSource rds = new ReportDataSource
                    {
                        Name = "dsRelatorioComissaoFuncionarioSintetico",
                        Value = dt
                    };
                    
                    var totalVendas = dt.AsEnumerable().Sum(x => x.Field<double>("Vendas"));
                    var totalComissao = dt.AsEnumerable().Sum(x => x.Field<decimal>("ValorComissao"));

                    rpvRelatorioSintetico.LocalReport.SetParameters(new ReportParameter("p_TotalVendas", totalVendas.ToString()));
                    rpvRelatorioSintetico.LocalReport.SetParameters(new ReportParameter("p_TotalComissao", totalComissao.ToString()));

                    rpvRelatorioSintetico.LocalReport.DataSources.Clear();
                    rpvRelatorioSintetico.LocalReport.DataSources.Add(rds);
                    rpvRelatorioSintetico.LocalReport.Refresh();

                    mpeRelatorioSintetico.Show();
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
    }
}