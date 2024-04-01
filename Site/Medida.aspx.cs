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
    public partial class Medida : System.Web.UI.Page
    {
        private void CarregarDados()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.gdvMedida.DataSource = DAL.MedidaDAL.Listar(usuarioSessao.SistemaID);
            this.gdvMedida.DataBind();
        }

        private bool CarregarDados(string medida)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.gdvMedida.DataSource = DAL.MedidaDAL.Listar(medida, usuarioSessao.SistemaID);
            this.gdvMedida.DataBind();
            return (this.gdvMedida.Rows.Count > 0);
        }

        protected void ckbMedidaID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbMedidaID.Checked)
            {
                this.txtMedidaID.Enabled = true;
                this.txtMedidaID.CssClass = "";
                this.imbCadastrar.Visible = false;
                this.imbConsultar.Visible = false;
                this.imbAtualizar.Visible = true;
                this.imbExcluir.Visible = true;
                this.ckbMedidaID.Text = "Consultar/Cadastrar";
            }
            else
            {
                this.txtMedidaID.Enabled = false;
                this.txtMedidaID.CssClass = "desabilitado";
                this.imbCadastrar.Visible = true;
                this.imbConsultar.Visible = true;
                this.imbAtualizar.Visible = false;
                this.imbExcluir.Visible = false;
                this.ckbMedidaID.Text = "Atualizar/Excluir";
            }
            this.LimparFormulario(this.txtMedidaID, this.txtMedida);
        }

        protected void gdvMedida_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.gdvMedida.PageIndex = e.NewPageIndex;
                if (!string.IsNullOrEmpty(this.txtMedida.Text))
                {
                    this.CarregarDados(this.txtMedida.Text);
                }
                else
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

        protected void gdvMedida_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    if (string.IsNullOrEmpty(this.txtMedidaID.Text))
                    {
                        throw new ApplicationException("A atualização de uma Medida só pode ser feita pelo MedidaID.\r\n\r\nDigite-a no campo MedidaID e clique novamente no botão Atualizar.");
                    }
                    if (!DAL.MedidaDAL.Listar(Convert.ToInt32(this.txtMedidaID.Text), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Medida inexistente.\r\n\r\nInforme outra Medida a ser atualizada.");
                    }
                    DAL.MedidaDAL.Atualizar(this.txtMedidaID.Text, this.txtMedida.Text);
                    this.LimparFormulario(this.txtMedidaID, this.txtMedida);
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
                    if (string.IsNullOrEmpty(this.txtMedida.Text.Trim().ToUpper()))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                    }
                    if (DAL.MedidaDAL.ListarDescricao(this.txtMedida.Text.Trim().ToUpper(), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Medida cadastrada.");
                    }
                    DAL.MedidaDAL.Inserir(this.txtMedida.Text.Trim().ToUpper(), usuarioSessao.SistemaID);
                    this.LimparFormulario(this.txtMedidaID, this.txtMedida);
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
                    if (string.IsNullOrEmpty(this.txtMedida.Text))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }
                    if (!this.CarregarDados(this.txtMedida.Text))
                    {
                        throw new ApplicationException("Medida inexistente.");
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
                    if (string.IsNullOrEmpty(this.txtMedidaID.Text))
                    {
                        throw new ApplicationException("A exclusão de uma Medida só pode ser feita pela MedidaID.\r\n\r\nDigite-a no campo MedidaID e clique novamente no botão Excluir.");
                    }
                    if (!DAL.MedidaDAL.Listar(Convert.ToInt32(this.txtMedidaID.Text), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Medida inexistente.\r\n\r\nInforme outra Medida a ser excluída.");
                    }
                    DAL.MedidaDAL.Excluir(this.txtMedidaID.Text);
                    this.LimparFormulario(this.txtMedidaID, this.txtMedida);
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

        private void LimparFormulario(TextBox txtMedidaID, TextBox txtMedida)
        {
            txtMedidaID.Text = string.Empty;
            txtMedida.Text = string.Empty;
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
            this.gdvMedida.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.lblTopo.Text = "CONSULTA - MEDIDA";
            if (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
            {
                this.imbCadastrar.Visible = true;
                this.ckbMedidaID.Visible = true;
                this.lblMedidaID.Visible = true;
                this.txtMedidaID.Visible = true;
            }
            else
            {
                this.imbCadastrar.Visible = false;
                this.imbAtualizar.Visible = false;
                this.imbExcluir.Visible = false;
                this.ckbMedidaID.Visible = false;
                this.lblMedidaID.Visible = false;
                this.txtMedidaID.Visible = false;
            }
        }
    }
}