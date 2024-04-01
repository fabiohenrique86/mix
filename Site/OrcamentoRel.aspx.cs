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
    public partial class OrcamentoRel : System.Web.UI.Page
    {
        private void CarregarDados()
        {
            this.rpvOrcamentoRel.LocalReport.Refresh();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                else if (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Estoquista))
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