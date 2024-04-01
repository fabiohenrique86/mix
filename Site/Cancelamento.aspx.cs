using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAO;
using DAL;

namespace Site
{
    public partial class Cancelamento : Page
    {
        private void CarregarRepeaterLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((Session["dsLoja"] == null) || (this.Session["bdLoja"] != null))
            {
                Session["dsLoja"] = new LojaDAL().Listar(usuarioSessao.SistemaID);
                Session["bdLoja"] = null;
            }
            rptLoja.DataSource = Session["dsLoja"];
            rptLoja.DataBind();
        }

        protected void imbCancelarPedido_Click(object sender, ImageClickEventArgs e)
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
                    string pedidoId = txtPedidoID.Text.Trim().ToUpper();
                    bool existePedido = false;
                    bool existeReserva = false;

                    if (string.IsNullOrEmpty(pedidoId) || string.IsNullOrEmpty(txtMotivo.Text.Trim().ToUpper()))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para cancelar pedido.");
                    }

                    existePedido = new PedidoDAL().Existe(pedidoId, usuarioSessao.SistemaID);
                    existeReserva = new ReservaDAL().Existe(txtPedidoID.Text.Trim().ToUpper(), usuarioSessao.SistemaID);

                    if (!existePedido && !existeReserva)
                    {
                        throw new ApplicationException("Pedido inexistente.");
                    }

                    if (CancelamentoDAL.Listar(pedidoId, usuarioSessao.SistemaID).Tables[0].Rows.Count > 0)
                    {
                        throw new ApplicationException("Pedido cancelado.");
                    }

                    CancelamentoDAL.Inserir(pedidoId, txtMotivo.Text.Trim().ToUpper(), usuarioSessao.SistemaID);
                    this.LimparFormulario(txtPedidoID, txtMotivo);
                    this.CarregarRepeaterLoja();
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

        private void LimparFormulario(TextBox txtPedidoID, TextBox txtMotivo)
        {
            txtPedidoID.Text = string.Empty;
            txtMotivo.Text = string.Empty;
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
                    else
                    {
                        this.CarregarRepeaterLoja();
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

        protected void rptCancelamentoLoja_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                    Label lblPedidoId = (Label)e.Item.FindControl("lblPedidoID");
                    lblPedidoId.ToolTip = string.Format("Motivo: {0}", ((Label)e.Item.FindControl("lblMotivo")).Text.Trim().ToUpper());
                    string pedidoId = lblPedidoId.Text.Trim().ToUpper();
                    GridView gdvCancelamentoLojaAux = (GridView)e.Item.FindControl("gdvCancelamento");

                    gdvCancelamentoLojaAux.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
                    gdvCancelamentoLojaAux.DataSource = CancelamentoDAL.Listar(pedidoId, usuarioSessao.SistemaID);
                    gdvCancelamentoLojaAux.DataBind();

                    if ((gdvCancelamentoLojaAux.Rows.Count <= 0) || (this.ViewState["filtro"] != null))
                    {
                        e.Item.FindControl("DivRepeaterLoja").Visible = false;
                    }
                }
                else
                {
                    e.Item.FindControl("DivRepeaterLoja").Visible = false;
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

        protected void rptLoja_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                    string lojaId = ((Label)e.Item.FindControl("lblLojaID")).Text.Trim().ToUpper();
                    ((Label)e.Item.FindControl("lblNomeFantasia")).Text = ((Label)e.Item.FindControl("lblNomeFantasia")).Text.ToUpper();
                    Repeater rptCancelamentoLojaAux = (Repeater)e.Item.FindControl("rptCancelamentoLoja");
                    rptCancelamentoLojaAux.DataSource = DAL.CancelamentoDAL.Listar(Convert.ToInt32(lojaId), usuarioSessao.SistemaID);
                    rptCancelamentoLojaAux.DataBind();
                    if (rptCancelamentoLojaAux.Items.Count <= 0)
                    {
                        ((Label)e.Item.FindControl("lblNomeFantasia")).Visible = false;
                    }
                }
                else
                {
                    ((Label)e.Item.FindControl("lblNomeFantasia")).Visible = false;
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