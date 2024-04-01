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
    public partial class Sistema : System.Web.UI.Page
    {
        private void CarregarDados()
        {
            this.gdvSistema.DataSource = DAL.SistemaDAL.Listar();
            this.gdvSistema.DataBind();
        }

        protected void imbAtualizar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtSistemaID.Text))
                {
                    DAL.SistemaDAL.Atualizar(Convert.ToInt32(this.txtSistemaID.Text), this.txtDescricao.Text, this.rblStatus.SelectedValue);
                    this.LimparFormulario(this.txtSistemaID, this.txtDescricao, this.rblStatus);
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

        protected void imbCadastrar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtDescricao.Text) && (this.rblStatus.SelectedValue != "0"))
                {
                    DAL.SistemaDAL.Inserir(this.txtDescricao.Text, Convert.ToInt32(this.rblStatus.SelectedValue), 1000);
                    this.LimparFormulario(this.txtSistemaID, this.txtDescricao, this.rblStatus);
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
                if (!string.IsNullOrEmpty(this.txtSistemaID.Text))
                {
                    DAL.SistemaDAL.Excluir(this.txtSistemaID.Text);
                    this.LimparFormulario(this.txtSistemaID, this.txtDescricao, this.rblStatus);
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

        private void LimparFormulario(TextBox txtSistemaID, TextBox txtDescricao, RadioButtonList rblStatus)
        {
            txtSistemaID.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            rblStatus.SelectedIndex = -1;
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
                this.gdvSistema.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
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