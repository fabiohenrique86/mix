using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Site
{
    public partial class ComandaEntrega : System.Web.UI.Page
    {
        private void CarregarDados()
        {
            this.rpvComandaEntrega.LocalReport.Refresh();
        }

        protected void imbImprimir_Click(object sender, ImageClickEventArgs e)
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
                    if (this.Session["PedidoOuReserva"].ToString() != "P")
                    {
                        if (this.Session["PedidoOuReserva"].ToString() != "R")
                        {
                            throw new ApplicationException("Erro ao listar comanda de entrega");
                        }
                        new DAL.ReservaDAL().AtualizarStatusImprimido(this.Session["PedidoID"].ToString(), usuarioSessao.SistemaID, this.Session["PedidoOuReserva"].ToString());
                    }
                    else
                    {
                        new DAL.PedidoDAL().AtualizarStatusImprimido(Session["PedidoID"].ToString(), usuarioSessao.SistemaID, this.Session["PedidoOuReserva"].ToString());
                    }
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagem(this.Page, ex.Message);
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
                if (!UtilitarioBLL.PermissaoUsuario(Session["Usuario"]) || (this.Session["PedidoID"] == null))
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
                else if ((new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID != Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador)) && (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID != Convert.ToInt32(UtilitarioBLL.TipoUsuario.Estoquista)))
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
                if (!base.IsPostBack)
                {
                    this.CarregarDados();
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