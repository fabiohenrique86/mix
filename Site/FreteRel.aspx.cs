using System;
using System.Web.UI;
using BLL;
using DAO;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;
using System.Globalization;

namespace Site
{
    public partial class FreteRel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!UtilitarioBLL.PermissaoUsuario(Session["Usuario"]))
                    {
                        UtilitarioBLL.Sair();
                        if (Request.Url.Segments.Length == 3)
                        {
                            Response.Redirect("../Default.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("Default.aspx", false);
                        }
                    }
                    else if (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID != Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador) 
                            && new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID != Convert.ToInt32(UtilitarioBLL.TipoUsuario.Logistica))
                    {
                        UtilitarioBLL.Sair();
                        if (Request.Url.Segments.Length == 3)
                        {
                            Response.Redirect("../Default.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("Default.aspx", false);
                        }
                    }

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

            ddlFuncionario.DataSource = this.Session["dsDropDownListFuncionario"];
            ddlFuncionario.DataBind();
        }

        protected void imbVisualizar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    BLL.AplicacaoBLL.Empresa = null;

                    if (Request.Url.Segments.Length == 3)
                    {
                        Response.Redirect("../Default.aspx", true);
                    }
                    else
                    {
                        Response.Redirect("Default.aspx", true);
                    }
                }
                else
                {
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                    var freteBLL = new FreteBLL();
                    var freteDAO = new FreteDAO();

                    freteDAO.SistemaID = usuarioSessao.SistemaID;
                    freteDAO.NmCidade = txtCidade.Text.Trim();
                    freteDAO.NomeCarreteiro = txtCarreteiro.Text.Trim();

                    if (!string.IsNullOrEmpty(ddlFuncionario.SelectedItem.Value) && ddlFuncionario.SelectedItem.Value != "0")
                    {
                        freteDAO.FuncionarioID = Convert.ToInt32(ddlFuncionario.SelectedItem.Value);
                    }

                    if (!string.IsNullOrEmpty(txtDataReservaInicial.Text.Trim()) && txtDataReservaInicial.Text != "__/__/____")
                    {
                        freteDAO.DtReservaInicial = DateTime.ParseExact(txtDataReservaInicial.Text.Trim(), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                    }

                    if (!string.IsNullOrEmpty(txtDataReservaFinal.Text.Trim()) && txtDataReservaFinal.Text != "__/__/____")
                    {
                        freteDAO.DtReservaFinal = DateTime.ParseExact(txtDataReservaFinal.Text.Trim(), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                    }

                    if (!string.IsNullOrEmpty(txtDataEntregaInicial.Text.Trim()) && txtDataEntregaInicial.Text != "__/__/____")
                    {
                        freteDAO.DtEntregaInicial = DateTime.ParseExact(txtDataEntregaInicial.Text.Trim(), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                    }

                    if (!string.IsNullOrEmpty(txtDataEntregaFinal.Text.Trim()) && txtDataEntregaFinal.Text != "__/__/____")
                    {
                        freteDAO.DtEntregaFinal = DateTime.ParseExact(txtDataEntregaFinal.Text.Trim(), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                    }

                    if (string.IsNullOrEmpty(freteDAO.NmCidade) &&
                        string.IsNullOrEmpty(freteDAO.NomeCarreteiro) &&
                        freteDAO.FuncionarioID <= 0 &&
                        freteDAO.DtReservaInicial == DateTime.MinValue &&
                        freteDAO.DtReservaFinal == DateTime.MinValue &&
                        freteDAO.DtEntregaInicial == DateTime.MinValue &&
                        freteDAO.DtEntregaFinal == DateTime.MinValue)
                    {
                        throw new ApplicationException("É necessário informar ao menos um campo para visualizar o relatório.");
                    }

                    var listaFrete = freteBLL.Listar(freteDAO);

                    rpvFrete.ProcessingMode = ProcessingMode.Local;
                    rpvFrete.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "FreteRel.rdlc";
                    rpvFrete.LocalReport.DataSources.Clear();
                    rpvFrete.LocalReport.DataSources.Add(new ReportDataSource { Name = "ds_frete", Value = listaFrete });
                    rpvFrete.LocalReport.Refresh();

                    mpeFrete.Show();
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, "Data inválida.");
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Source.ToUpper() == "SYSTEM.DATA")
                {
                    UtilitarioBLL.ExibirMensagemAjax(this.Page, "Data inválida.");
                }
                else
                {
                    UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message, ex);
                }
            }
        }
    }
}