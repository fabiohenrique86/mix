using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAO;
using DAL;

namespace Site.administrador
{
    public partial class TipoUsuario : System.Web.UI.Page
    {
        private void CarregarDados()
        {
            this.gdvTipoUsuario.DataSource = DAL.TipoUsuarioDAL.Listar();
            this.gdvTipoUsuario.DataBind();
        }

        protected void imbCadastrar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtTipoUsuarioID.Text) && !string.IsNullOrEmpty(this.txtDescricao.Text))
                {
                    DAL.TipoUsuarioDAL.Inserir(Convert.ToInt32(this.txtTipoUsuarioID.Text), this.txtDescricao.Text);
                    this.LimparFormulario(this.txtTipoUsuarioID, this.txtDescricao);
                    this.CarregarDados();
                }
                else
                {
                    UtilitarioBLL.ExibirMensagemAjax(this.Page, "Campo vazio");
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

        protected void imbExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtTipoUsuarioID.Text))
                {
                    DAL.TipoUsuarioDAL.Excluir(this.txtTipoUsuarioID.Text);
                    this.LimparFormulario(this.txtTipoUsuarioID, this.txtDescricao);
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

        private void LimparFormulario(TextBox TipoUsuarioID, TextBox Descricao)
        {
            TipoUsuarioID.Text = string.Empty;
            Descricao.Text = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Session["Owner"] == null)
                {
                    base.Response.Redirect("../Default.aspx");
                }
                if (!base.IsPostBack)
                {
                    this.CarregarDados();
                }
                this.gdvTipoUsuario.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
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