using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAO;

namespace Site
{
    public partial class PedidoVenda : System.Web.UI.Page
    {
        private void CarregarDados()
        {
            this.rpvPedidoVenda.LocalReport.Refresh();
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