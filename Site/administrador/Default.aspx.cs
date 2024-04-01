using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Site.administrador
{
    public partial class Default : System.Web.UI.Page
    {
        protected void imbLogin_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (AdministradorDAL.ValidarLogin(this.txtLogin.Text, this.txtSenha.Text))
                {
                    this.Session["Owner"] = true;
                    base.Response.Redirect("Default.aspx", false);
                }
                else
                {
                    this.lblMensagem.Visible = true;
                    this.lblMensagem.Text = "Login e/ou Senha inválidos.";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Session["Owner"] != null)
            {
                this.tabela.Visible = false;
                this.bemvindo.Visible = true;
            }
            else
            {
                this.tabela.Visible = true;
                this.bemvindo.Visible = false;
            }
        }
    }
}