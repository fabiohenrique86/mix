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
    public partial class TipoPagamento : System.Web.UI.Page
    {
        private bool CarregarDados(string descricao)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.gdvTipoPagamento.DataSource = DAL.TipoPagamentoDAL.ListarFiltro(descricao, usuarioSessao.SistemaID);
            this.gdvTipoPagamento.DataBind();
            return (this.gdvTipoPagamento.Rows.Count > 0);
        }

        private void CarregarGridTipoPagamento()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((this.Session["dsGridTipoPagamento"] == null) || (this.Session["bdTipoPagamento"] != null))
            {
                this.Session["dsGridTipoPagamento"] = DAL.TipoPagamentoDAL.Listar(usuarioSessao.SistemaID);
            }
            this.gdvTipoPagamento.DataSource = this.Session["dsGridTipoPagamento"];
            this.gdvTipoPagamento.DataBind();
        }

        protected void ckbTipoPagamentoID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbTipoPagamentoID.Checked)
            {
                this.txtTipoPagamentoID.Enabled = true;
                this.txtTipoPagamentoID.CssClass = "";
                this.imbCadastrar.Visible = false;
                this.imbConsultar.Visible = false;
                this.imbAtualizar.Visible = true;
                this.imbExcluir.Visible = true;
                this.ckbTipoPagamentoID.Text = "Consultar/Cadastrar";
            }
            else
            {
                this.txtTipoPagamentoID.Enabled = false;
                this.txtTipoPagamentoID.CssClass = "desabilitado";
                this.imbCadastrar.Visible = true;
                this.imbConsultar.Visible = true;
                this.imbAtualizar.Visible = false;
                this.imbExcluir.Visible = false;
                this.ckbTipoPagamentoID.Text = "Atualizar/Excluir";
            }
            this.LimparFormulario(this.txtTipoPagamentoID, this.txtDescricao);
        }

        protected void gdvTipoPagamento_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    if (string.IsNullOrEmpty(this.txtTipoPagamentoID.Text))
                    {
                        throw new ApplicationException("Informe o TipoPagamentoID para efetuar atualização.");
                    }
                    if (!DAL.TipoPagamentoDAL.Listar(this.txtTipoPagamentoID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Tipo Pagamento inexistente.");
                    }
                    DAL.TipoPagamentoDAL.Atualizar(this.txtTipoPagamentoID.Text, this.txtDescricao.Text);
                    this.Session["bdTipoPagamento"] = true;
                    this.LimparFormulario(this.txtTipoPagamentoID, this.txtDescricao);
                    this.CarregarGridTipoPagamento();
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
                    if (string.IsNullOrEmpty(this.txtDescricao.Text))
                    {
                        throw new ApplicationException("É necessário informar o campo Descrição para cadastrar.");
                    }
                    DAL.TipoPagamentoDAL.Inserir(this.txtDescricao.Text.Trim().ToUpper(), usuarioSessao.SistemaID);
                    this.Session["bdTipoPagamento"] = true;
                    this.LimparFormulario(this.txtTipoPagamentoID, this.txtDescricao);
                    this.CarregarGridTipoPagamento();
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
                    if (string.IsNullOrEmpty(this.txtDescricao.Text))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }
                    if (!this.CarregarDados(this.txtDescricao.Text))
                    {
                        throw new ApplicationException("Tipo de Pagamento inexistente.");
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
                    if (string.IsNullOrEmpty(this.txtTipoPagamentoID.Text))
                    {
                        throw new ApplicationException("Informe o TipoPagamentoID para efetuar exclusão.");
                    }
                    if (!DAL.TipoPagamentoDAL.Listar(this.txtTipoPagamentoID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Tipo de Pagamento inexistente.");
                    }
                    DAL.TipoPagamentoDAL.Excluir(this.txtTipoPagamentoID.Text);
                    this.Session["bdTipoPagamento"] = true;
                    this.LimparFormulario(this.txtTipoPagamentoID, this.txtDescricao);
                    this.CarregarGridTipoPagamento();
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

        private void LimparFormulario(TextBox txtTipoPagamentoID, TextBox txtDescricao)
        {
            txtTipoPagamentoID.Text = string.Empty;
            txtDescricao.Text = string.Empty;
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
                        this.CarregarGridTipoPagamento();
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
            this.gdvTipoPagamento.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.lblTopo.Text = "CONSULTA - TIPO PAGAMENTO";
            if (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
            {
                this.imbCadastrar.Visible = true;
                this.ckbTipoPagamentoID.Visible = true;
                this.lblTipoPagamentoID.Visible = true;
                this.txtTipoPagamentoID.Visible = true;
            }
            else
            {
                this.imbCadastrar.Visible = false;
                this.imbAtualizar.Visible = false;
                this.imbExcluir.Visible = false;
                this.ckbTipoPagamentoID.Visible = false;
                this.lblTipoPagamentoID.Visible = false;
                this.txtTipoPagamentoID.Visible = false;
            }
        }
    }
}