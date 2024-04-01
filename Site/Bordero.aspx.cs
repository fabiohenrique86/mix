using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using DAO;

namespace Site
{
    public partial class Bordero : System.Web.UI.Page
    {
        decimal totalPago = 0;

        private void CarregarDados()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.ddlLoja.DataSource = new DAL.LojaDAL().ListarOrcamentoDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
            this.ddlLoja.DataBind();
        }

        private bool CarregarDados(int lojaId, DateTime dataBorderoInicial, DateTime dataBorderoFinal)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.gdvBordero.DataSource = DAL.BorderoDAL.Listar(lojaId, dataBorderoInicial, dataBorderoFinal, usuarioSessao.SistemaID);
            this.gdvBordero.DataBind();
            if (this.gdvBordero.Rows.Count > 0)
            {
                this.DivTotalPago.Visible = true;
                return true;
            }
            this.txtTotalPago.Text = string.Empty;
            this.DivTotalPago.Visible = false;
            return false;
        }

        protected void gdvBordero_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    this.totalPago += Convert.ToDecimal(e.Row.Cells[1].Text.Replace("R", "").Replace("$", ""));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    this.txtTotalPago.Text = string.Format("{0:c}", this.totalPago);
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

        protected void imbConsultar_Click(object sender, ImageClickEventArgs e)
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
                    if (((this.ddlLoja.SelectedValue == "0") || (this.Session["DiaMesAnoInicial"] == null)) || (this.Session["DiaMesAnoFinal"] == null))
                    {
                        throw new ApplicationException("É necessário informar todos os campos para consultar.");
                    }
                    if (!this.CarregarDados(Convert.ToInt32(this.ddlLoja.SelectedValue), Convert.ToDateTime(this.Session["DiaMesAnoInicial"]), Convert.ToDateTime(this.Session["DiaMesAnoFinal"])))
                    {
                        throw new ApplicationException("Borderô inexistente.");
                    }
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
                        this.VisualizarFormulario();
                        this.CarregarDados();
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

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.DiaMesAno1.FindControl("lblMsgTopo").Visible = false;
            this.lblTopo.Text = "CONSULTA - BORDERÔ";
        }
    }
}