using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAO;
using DAL;

namespace Site
{
    public partial class Linha : System.Web.UI.Page
    {
        private void CarregarDados()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.gdvLinha.DataSource = DAL.LinhaDAL.Listar(usuarioSessao.SistemaID);
            this.gdvLinha.DataBind();
        }

        private bool CarregarDados(string descricao, string desconto)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.gdvLinha.DataSource = DAL.LinhaDAL.Listar(descricao, desconto, usuarioSessao.SistemaID);
            this.gdvLinha.DataBind();
            return (this.gdvLinha.Rows.Count > 0);
        }

        protected void ckbLinhaID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbLinhaID.Checked)
            {
                this.txtLinhaID.Enabled = true;
                this.txtLinhaID.CssClass = "";
                this.imbCadastrar.Visible = false;
                this.imbConsultar.Visible = false;
                this.imbAtualizar.Visible = true;
                this.imbExcluir.Visible = true;
                this.ckbLinhaID.Text = "Consultar/Cadastrar";
            }
            else
            {
                this.txtLinhaID.Enabled = false;
                this.txtLinhaID.CssClass = "desabilitado";
                this.imbCadastrar.Visible = true;
                this.imbConsultar.Visible = true;
                this.imbAtualizar.Visible = false;
                this.imbExcluir.Visible = false;
                this.ckbLinhaID.Text = "Atualizar/Excluir";
            }
            this.LimparFormulario(this.txtLinhaID, this.txtDescricao, this.txtDesconto);
        }

        protected void gdvLinha_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (usuarioSessao.TipoUsuarioID != Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
                    {
                        e.Row.Cells[0].Visible = false;
                    }
                }
                else if ((e.Row.RowType == DataControlRowType.DataRow) && (usuarioSessao.TipoUsuarioID != Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador)))
                {
                    e.Row.Cells[0].Visible = false;
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

        protected void imbAtualizar_Click(object sender, ImageClickEventArgs e)
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
                    if (string.IsNullOrEmpty(this.txtLinhaID.Text))
                    {
                        throw new ApplicationException("A atualização de uma Linha só pode ser feita pelo LinhaID.\r\n\r\nDigite-o no campo LinhaID e clique novamente no botão Atualizar.");
                    }
                    if (string.IsNullOrEmpty(this.txtDescricao.Text.Trim().ToUpper()) && string.IsNullOrEmpty(this.txtDesconto.Text.Trim().ToUpper()))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para atualizar.");
                    }
                    if (!DAL.LinhaDAL.Listar(this.txtLinhaID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Linha inexistente.");
                    }
                    DAL.LinhaDAL.Atualizar(this.txtLinhaID.Text, this.txtDescricao.Text.Trim().ToUpper(), this.txtDesconto.Text.Trim().ToUpper());
                    this.Session["bdLinha"] = true;
                    this.LimparFormulario(this.txtLinhaID, this.txtDescricao, this.txtDesconto);
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
                    if (string.IsNullOrEmpty(this.txtDescricao.Text.Trim().ToUpper()) || string.IsNullOrEmpty(this.txtDesconto.Text.Trim().ToUpper()))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                    }
                    if (DAL.LinhaDAL.ListarDescricao(this.txtDescricao.Text.Trim().ToUpper(), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Linha cadastrada.");
                    }
                    DAL.LinhaDAL.Inserir(this.txtDescricao.Text.Trim().ToUpper(), this.txtDesconto.Text.Trim().ToUpper(), usuarioSessao.SistemaID);
                    this.Session["bdLinha"] = true;
                    this.LimparFormulario(this.txtLinhaID, this.txtDescricao, this.txtDesconto);
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
                    if (string.IsNullOrEmpty(this.txtDescricao.Text) && string.IsNullOrEmpty(this.txtDesconto.Text))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }
                    if (!this.CarregarDados(this.txtDescricao.Text, this.txtDesconto.Text))
                    {
                        throw new ApplicationException("Linha inexistente.");
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

        protected void imbExcluir_Click(object sender, ImageClickEventArgs e)
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
                    if (string.IsNullOrEmpty(this.txtLinhaID.Text))
                    {
                        throw new ApplicationException("A exclusão de uma Linha só pode ser feita pelo LinhaID.\r\n\r\nDigite-o no campo LinhaID e clique novamente no botão Excluir.");
                    }
                    if (!DAL.LinhaDAL.Listar(this.txtLinhaID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Linha inexistente.");
                    }
                    DAL.LinhaDAL.Excluir(this.txtLinhaID.Text);
                    this.Session["bdLinha"] = true;
                    this.LimparFormulario(this.txtLinhaID, this.txtDescricao, this.txtDesconto);
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

        private void LimparFormulario(TextBox txtLinhaID, TextBox txtDescricao, TextBox txtDesconto)
        {
            txtLinhaID.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            txtDesconto.Text = string.Empty;
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
                        this.SetarBordaGridView();
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

        private void SetarBordaGridView()
        {
            this.gdvLinha.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.lblTopo.Text = "CONSULTA - LINHA";
            if (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
            {
                this.imbCadastrar.Visible = true;
                this.ckbLinhaID.Visible = true;
                this.lblLinhaID.Visible = true;
                this.txtLinhaID.Visible = true;
            }
            else
            {   
                this.imbCadastrar.Visible = false;
                this.imbAtualizar.Visible = false;
                this.imbExcluir.Visible = false;
                this.ckbLinhaID.Visible = false;
                this.lblLinhaID.Visible = false;
                this.txtLinhaID.Visible = false;
            }
        }
    }
}