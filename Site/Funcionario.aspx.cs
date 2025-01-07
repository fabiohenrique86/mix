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
    public partial class Funcionario : System.Web.UI.Page
    {
        private bool CarregarDados(bool filtro)
        {
            this.ViewState["filtro"] = true;
            this.CarregarRepeaterLoja();
            return (this.rptLoja.Items.Count > 0);
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.ddlLoja.DataSource = new DAL.LojaDAL().ListarUsuarioDropDownList(usuarioSessao.SistemaID);
            this.ddlLoja.DataBind();
        }

        private void CarregarRepeaterLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((this.Session["dsLoja"] == null) || (this.Session["bdLoja"] != null))
            {
                this.Session["dsLoja"] = new DAL.LojaDAL().Listar(usuarioSessao.SistemaID);
                this.Session["bdLoja"] = null;
            }
            this.rptLoja.DataSource = this.Session["dsLoja"];
            this.rptLoja.DataBind();
        }

        protected void ckbFuncionarioID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbFuncionarioID.Checked)
            {
                this.txtFuncionarioID.Enabled = true;
                this.txtFuncionarioID.CssClass = "";
                this.imbCadastrar.Visible = true;
                this.imbConsultar.Visible = false;
                this.imbAtualizar.Visible = true;
                this.imbExcluir.Visible = true;
                this.ckbFuncionarioID.Text = "Consultar";
            }
            else
            {
                this.txtFuncionarioID.Enabled = false;
                this.txtFuncionarioID.CssClass = "desabilitado";
                this.imbCadastrar.Visible = false;
                this.imbConsultar.Visible = true;
                this.imbAtualizar.Visible = false;
                this.imbExcluir.Visible = false;
                this.ckbFuncionarioID.Text = "Cadastrar/Atualizar/Inativar";
            }
            this.LimparFormulario(this.txtFuncionarioID, this.txtNome, this.ddlLoja, this.txtTelefone, this.txtEmail);
        }

        protected void gdvFuncionario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Text = e.Row.Cells[1].Text.ToUpper();

                string telefone = e.Row.Cells[2].Text;
                if (telefone.Length == 8)
                {
                    e.Row.Cells[2].Text = string.Format("{0:####-####}", Convert.ToInt64(telefone));
                }
                else if (telefone.Length == 10)
                {
                    e.Row.Cells[2].Text = string.Format("{0:(##)####-####}", Convert.ToInt64(telefone));
                }
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
                    int? funcionarioId = 0;
                    if (!string.IsNullOrEmpty(this.txtFuncionarioID.Text.Trim().ToUpper()))
                    {
                        funcionarioId = new int?(Convert.ToInt32(this.txtFuncionarioID.Text.Trim().ToUpper()));
                    }
                    int? lojaId = 0;
                    if (this.ddlLoja.SelectedValue != "0")
                    {
                        lojaId = new int?(Convert.ToInt32(this.ddlLoja.SelectedValue));
                    }
                    string telefone = string.IsNullOrEmpty(this.txtTelefone.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : this.txtTelefone.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                    string email = string.IsNullOrEmpty(this.txtEmail.Text.Trim().ToUpper()) ? null : this.txtEmail.Text.Trim().ToUpper();
                    string nome = string.IsNullOrEmpty(this.txtNome.Text.Trim().ToUpper()) ? null : this.txtNome.Text.Trim().ToUpper();
                    if (funcionarioId <= 0)
                    {
                        throw new ApplicationException("A atualização de um Funcionário só pode ser feita pelo FuncionárioID.\r\n\r\nDigite-o no campo FuncionárioID e clique novamente no botão Atualizar.");
                    }
                    if (string.IsNullOrEmpty(nome) && (((lojaId <= 0) && string.IsNullOrEmpty(telefone)) && string.IsNullOrEmpty(email)))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para atualizar.");
                    }
                    if (!DAL.FuncionarioDAL.Listar(this.txtFuncionarioID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Funcionário inexistente.");
                    }
                    if (!DAL.FuncionarioDAL.EstaAtivo(Convert.ToInt32(this.txtFuncionarioID.Text.Trim().ToUpper()), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Funcionário inexistente.");
                    }
                    DAL.FuncionarioDAL.Atualizar(new DAO.FuncionarioDAO(funcionarioId, nome, lojaId, telefone, email, usuarioSessao.SistemaID));
                    this.Session["bdFuncionario"] = true;
                    this.LimparFormulario(this.txtFuncionarioID, this.txtNome, this.ddlLoja, this.txtTelefone, this.txtEmail);
                    this.CarregarRepeaterLoja();
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
                    int? funcionarioId = 0;
                    if (!string.IsNullOrEmpty(this.txtFuncionarioID.Text.Trim().ToUpper()))
                    {
                        funcionarioId = new int?(Convert.ToInt32(this.txtFuncionarioID.Text.Trim().ToUpper()));
                    }
                    int? lojaId = 0;
                    if (this.ddlLoja.SelectedValue != "0")
                    {
                        lojaId = new int?(Convert.ToInt32(this.ddlLoja.SelectedValue));
                    }
                    string telefone = string.IsNullOrEmpty(this.txtTelefone.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : this.txtTelefone.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                    string email = string.IsNullOrEmpty(this.txtEmail.Text.Trim().ToUpper()) ? null : this.txtEmail.Text.Trim().ToUpper();
                    string nome = string.IsNullOrEmpty(this.txtNome.Text.Trim().ToUpper()) ? null : this.txtNome.Text.Trim().ToUpper();
                    if (((funcionarioId <= 0) || string.IsNullOrEmpty(nome)) || (((lojaId <= 0) || string.IsNullOrEmpty(telefone)) || string.IsNullOrEmpty(email)))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                    }
                    if (!DAL.FuncionarioDAL.Listar(this.txtFuncionarioID.Text, usuarioSessao.SistemaID))
                    {
                        DAL.FuncionarioDAL.Inserir(new DAO.FuncionarioDAO(funcionarioId, nome, lojaId, telefone, email, usuarioSessao.SistemaID));
                    }
                    else
                    {
                        if (DAL.FuncionarioDAL.EstaAtivo(Convert.ToInt32(this.txtFuncionarioID.Text.Trim().ToUpper()), usuarioSessao.SistemaID))
                        {
                            throw new ApplicationException("Funcionário cadastrado.");
                        }
                        DAL.FuncionarioDAL.Atualizar(new DAO.FuncionarioDAO(funcionarioId, nome, lojaId, telefone, email, usuarioSessao.SistemaID));
                    }
                    this.Session["bdFuncionario"] = true;
                    this.LimparFormulario(this.txtFuncionarioID, this.txtNome, this.ddlLoja, this.txtTelefone, this.txtEmail);
                    this.CarregarRepeaterLoja();
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
                    if (((string.IsNullOrEmpty(this.txtNome.Text.Trim().ToUpper()) && (this.ddlLoja.SelectedIndex <= 0)) && (string.IsNullOrEmpty(this.txtTelefone.Text) || (this.txtTelefone.Text == "(__)____-____"))) && string.IsNullOrEmpty(this.txtEmail.Text.Trim().ToUpper()))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }
                    if (!this.CarregarDados(true))
                    {
                        throw new ApplicationException("Funcionário inexistente.");
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
                    int funcionarioId = 0;
                    if (!string.IsNullOrEmpty(this.txtFuncionarioID.Text.Trim().ToUpper()))
                    {
                        funcionarioId = Convert.ToInt32(this.txtFuncionarioID.Text.Trim().ToUpper());
                    }
                    if (funcionarioId <= 0)
                    {
                        throw new ApplicationException("Informe o FuncionárioID para efetuar a exclusão.");
                    }
                    if (!DAL.FuncionarioDAL.Listar(this.txtFuncionarioID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Funcionário inexistente.");
                    }
                    if (!DAL.FuncionarioDAL.EstaAtivo(funcionarioId, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Funcionário inexistente.");
                    }
                    DAL.FuncionarioDAL.Excluir(new DAO.FuncionarioDAO(funcionarioId, usuarioSessao.SistemaID));
                    this.Session["bdFuncionario"] = true;
                    this.LimparFormulario(this.txtFuncionarioID, this.txtNome, this.ddlLoja, this.txtTelefone, this.txtEmail);
                    this.CarregarRepeaterLoja();
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

        private void LimparFormulario(TextBox txtFuncionarioID, TextBox txtNome, DropDownList ddlLoja, TextBox txtTelefone, TextBox txtEmail)
        {
            txtFuncionarioID.Text = string.Empty;
            txtNome.Text = string.Empty;
            ddlLoja.SelectedIndex = 0;
            txtTelefone.Text = string.Empty;
            txtEmail.Text = string.Empty;
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
                        this.CarregarRepeaterLoja();
                        this.CarregarDropDownListLoja();
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

        protected void rptLoja_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    string lojaId = ((Label)e.Item.FindControl("lblLojaID")).Text;
                    ((Label)e.Item.FindControl("lblLoja")).Text = ((Label)e.Item.FindControl("lblLoja")).Text.ToUpper();
                    GridView gdvFuncionarioAux = (GridView)e.Item.FindControl("gdvFuncionario");
                    gdvFuncionarioAux.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
                    if (Convert.ToBoolean(this.ViewState["filtro"]))
                    {
                        gdvFuncionarioAux.DataSource = DAL.FuncionarioDAL.Listar(this.txtNome.Text.ToUpper(), lojaId, this.txtTelefone.Text.Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", ""), this.txtEmail.Text.Trim().ToUpper());
                        gdvFuncionarioAux.DataBind();
                    }
                    else
                    {
                        gdvFuncionarioAux.DataSource = DAL.FuncionarioDAL.Listar(Convert.ToInt32(lojaId));
                        gdvFuncionarioAux.DataBind();
                    }
                    if ((gdvFuncionarioAux.Rows.Count <= 0) || (((lojaId != this.ddlLoja.SelectedValue) && (this.ddlLoja.SelectedValue != "0")) && (this.ViewState["filtro"] != null)))
                    {
                        ((Label)e.Item.FindControl("lblLoja")).Visible = false;
                        gdvFuncionarioAux.Visible = false;
                    }
                }
                else
                {
                    ((Label)e.Item.FindControl("lblLoja")).Visible = false;
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

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.lblTopo.Text = "CONSULTA - FUNCIONÁRIO";
            if (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
            {
                this.ckbFuncionarioID.Visible = true;
                this.lblFuncionarioID.Visible = true;
                this.txtFuncionarioID.Visible = true;
            }
            else
            {
                this.ckbFuncionarioID.Visible = false;
                this.lblFuncionarioID.Visible = false;
                this.txtFuncionarioID.Visible = false;
            }
        }
    }
}