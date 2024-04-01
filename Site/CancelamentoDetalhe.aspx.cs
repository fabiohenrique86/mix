using System;
using System.Web.UI;
using System.Data;
using BLL;

namespace Site
{
    public partial class CancelamentoDetalhe : Page
    {
        private void CarregarDados()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            DataSet ds = DAL.CancelamentoDAL.ListarDetalhe(Session["PedidoID"].ToString(), usuarioSessao.SistemaID);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                throw new ApplicationException("Pedido inexistente.");
            }
            this.lblPedidoID.Text = this.Session["PedidoID"].ToString().ToUpper();
            this.lblMotivo.Text = ds.Tables[0].Rows[0]["Motivo"].ToString().ToUpper();
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