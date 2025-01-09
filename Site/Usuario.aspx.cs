using BLL;
using DAL;
using DAO;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site
{
    public partial class Usuario : Page
    {
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
                        this.Consultar();
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

        private void Consultar()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            int usuarioId = 0;
            int.TryParse(txtUsuarioID.Text, out usuarioId);

            int lojaId = 0;
            int.TryParse(ddlLoja.SelectedValue, out lojaId);

            int tipoUsuarioId = 0;
            int.TryParse(ddlTipoUsuario.SelectedValue, out tipoUsuarioId);

            string login = txtLogin.Text.Trim();

            bool? ativo = null;
            if (string.IsNullOrWhiteSpace(rblStatus.SelectedValue))
            {
                ativo = null;
            }
            else if (bool.TryParse(rblStatus.SelectedValue, out bool parsedValue))
            {
                ativo = parsedValue;
            }

            this.gdvUsuario.DataSource = new UsuarioDAL().Listar(new UsuarioDAO() { UsuarioID = usuarioId, LojaID = lojaId, TipoUsuarioID = tipoUsuarioId, Login = login, SistemaID = usuarioSessao.SistemaID, Ativo = ativo });
            this.gdvUsuario.DataBind();
        }

        private void CarregarDados()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            this.ddlLoja.DataSource = new DAL.LojaDAL().ListarUsuarioDropDownList(usuarioSessao.SistemaID);
            this.ddlLoja.DataBind();

            if (this.imbCadastrar.Visible)
            {
                this.ddlTipoUsuario.DataSource = TipoUsuarioDAL.ListarDropDownListAdmEst();
            }
            else
            {
                this.ddlTipoUsuario.DataSource = TipoUsuarioDAL.ListarDropDownList();
            }
            this.ddlTipoUsuario.DataBind();
        }

        protected void ckbUsuarioID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbUsuarioID.Checked)
            {
                this.txtUsuarioID.Enabled = true;
                this.txtUsuarioID.CssClass = "";
                this.imbCadastrar.Visible = false;
                this.imbAtualizar.Visible = true;
                this.ckbUsuarioID.Text = "Cadastrar";
                this.ddlLoja.Enabled = false;
                this.ddlLoja.CssClass = "desabilitado";
                this.ddlTipoUsuario.DataSource = DAL.TipoUsuarioDAL.ListarDropDownList();
            }
            else
            {
                this.txtUsuarioID.Enabled = false;
                this.txtUsuarioID.CssClass = "desabilitado";
                this.imbCadastrar.Visible = true;
                this.imbAtualizar.Visible = false;
                this.ddlLoja.Enabled = true;
                this.ddlLoja.CssClass = "";
                this.ckbUsuarioID.Text = "Atualizar";
                this.ddlTipoUsuario.DataSource = DAL.TipoUsuarioDAL.ListarDropDownListAdmEst();
            }
            this.ddlTipoUsuario.DataBind();
            this.LimparFormulario(this.txtUsuarioID, this.ddlTipoUsuario, this.ddlLoja, this.txtLogin, this.txtSenha);
        }

        protected void ddlLoja_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.ddlTipoUsuario.SelectedItem.Text.ToUpper() == "ADMINISTRADOR") || (this.ddlTipoUsuario.SelectedItem.Text.ToUpper() == "ESTOQUISTA"))
            {
                this.ddlLoja.SelectedIndex = 0;
            }
        }

        protected void ddlTipoUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.ddlTipoUsuario.SelectedItem.Text.ToUpper() == "ADMINISTRADOR") || (this.ddlTipoUsuario.SelectedItem.Text.ToUpper() == "ESTOQUISTA"))
            {
                this.ddlLoja.SelectedIndex = 0;
            }
        }

        protected void gdvUsuario_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                this.gdvUsuario.PageIndex = e.NewPageIndex;
                this.Consultar();
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
                    this.Consultar();
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
                    DAL.UsuarioDAL usuarioDAL = new DAL.UsuarioDAL();

                    if (string.IsNullOrEmpty(this.txtUsuarioID.Text))
                    {
                        throw new ApplicationException("É necessário informar o UsuarioID a ser atualizado.");
                    }

                    if (!usuarioDAL.Listar(Convert.ToInt32(this.txtUsuarioID.Text), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Usuário inexistente.");
                    }

                    usuarioDAL.Atualizar(this.txtUsuarioID.Text, this.ddlTipoUsuario.SelectedValue, this.ddlLoja.SelectedValue, this.txtLogin.Text, this.txtSenha.Text);

                    this.LimparFormulario(this.txtUsuarioID, this.ddlTipoUsuario, this.ddlLoja, this.txtLogin, this.txtSenha);
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
                    DAL.UsuarioDAL usuarioDAL = new DAL.UsuarioDAL();

                    if (((this.ddlTipoUsuario.SelectedIndex <= 0) || (this.ddlLoja.SelectedIndex <= 0)) || (string.IsNullOrEmpty(this.txtLogin.Text) || string.IsNullOrEmpty(this.txtSenha.Text)))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                    }
                    if (TipoUsuarioDAL.Listar(Convert.ToInt32(this.ddlTipoUsuario.SelectedValue), Convert.ToInt32(this.ddlLoja.SelectedValue)))
                    {
                        throw new ApplicationException("Tipo de usuário cadastrado na loja informada.");
                    }
                    if (usuarioDAL.ValidarLogin(this.txtLogin.Text.Trim().ToUpper(), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Login cadastrado.");
                    }
                    usuarioDAL.Inserir(this.ddlTipoUsuario.SelectedValue, this.ddlLoja.SelectedValue, this.txtLogin.Text, this.txtSenha.Text, usuarioSessao.SistemaID);
                    this.LimparFormulario(this.txtUsuarioID, this.ddlTipoUsuario, this.ddlLoja, this.txtLogin, this.txtSenha);
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

        protected void gdvUsuario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AtivarInativar")
            {
                int usuarioId = Convert.ToInt32(e.CommandArgument);
                DAL.UsuarioDAL usuarioDAL = new DAL.UsuarioDAL();

                usuarioDAL.Excluir(usuarioId);
                this.Consultar();
            }
        }

        protected void gdvUsuario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (usuarioSessao.TipoUsuarioID != UtilitarioBLL.TipoUsuario.Administrador.GetHashCode())
                {
                    var btnAtivarInativar = (Button)e.Row.FindControl("btnAtivarInativar");
                    if (btnAtivarInativar != null)
                    {
                        btnAtivarInativar.Visible = false;
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.Footer)
            {
                if (usuarioSessao.TipoUsuarioID != UtilitarioBLL.TipoUsuario.Administrador.GetHashCode())
                {
                    e.Row.Cells[5].Visible = false;
                }
            }
        }

        private void LimparFormulario(TextBox UsuarioID, DropDownList TipoUsuarioID, DropDownList ddlLojaID, TextBox Login, TextBox Senha)
        {
            UsuarioID.Text = string.Empty;
            TipoUsuarioID.SelectedIndex = 0;
            ddlLojaID.SelectedIndex = 0;
            Login.Text = string.Empty;
            Senha.Text = string.Empty;
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if (usuarioSessao.TipoUsuarioID != UtilitarioBLL.TipoUsuario.Administrador.GetHashCode())
            {
                this.imbCadastrar.Visible = false;
                this.imbAtualizar.Visible = false;
            }
        }

        private void SetarBordaGridView()
        {
            this.gdvUsuario.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
        }
    }
}