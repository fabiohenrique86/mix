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
    public partial class Contato : System.Web.UI.Page
    {
        protected void imbEnviar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (this.Page.IsValid)
                {
                    EmailBLL.Enviar(this.txtNome.Text, this.txtEmail.Text, this.txtMensagem.Text, "Contato - Sistema MiX");
                    this.LimparFormulario(this.txtNome, this.txtEmail, this.txtMensagem);
                    UtilitarioBLL.ExibirMensagemAjax(this.Page, "E-mail enviado com sucesso!");
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message);
            }
        }

        protected void imbLimpar_Click(object sender, ImageClickEventArgs e)
        {
            this.LimparFormulario(this.txtNome, this.txtEmail, this.txtMensagem);
        }

        private void LimparFormulario(TextBox nome, TextBox email, TextBox mensagem)
        {
            nome.Text = string.Empty;
            email.Text = string.Empty;
            mensagem.Text = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
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
        }
    }
}