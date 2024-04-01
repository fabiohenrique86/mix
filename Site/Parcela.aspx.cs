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
    public partial class Parcela : System.Web.UI.Page
    {
        private void CarregarDados()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.gdvParcela.DataSource = DAL.ParcelaDAL.Listar(usuarioSessao.SistemaID);
            this.gdvParcela.DataBind();
        }

        private bool CarregarDados(string parcelaId, string prazoMedio)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.gdvParcela.DataSource = DAL.ParcelaDAL.Listar(parcelaId, prazoMedio, usuarioSessao.SistemaID);
            this.gdvParcela.DataBind();
            return (this.gdvParcela.Rows.Count > 0);
        }

        protected void ckbParcelaID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbParcelaID.Checked)
            {
                this.txtParcelaID.Enabled = true;
                this.txtParcelaID.CssClass = "";
                this.imbCadastrar.Visible = true;
                this.imbConsultar.Visible = false;
                this.imbAtualizar.Visible = true;
                this.imbExcluir.Visible = true;
                this.ckbParcelaID.Text = "Consultar";
            }
            else
            {
                this.txtParcelaID.Enabled = false;
                this.txtParcelaID.CssClass = "desabilitado";
                this.imbCadastrar.Visible = false;
                this.imbConsultar.Visible = true;
                this.imbAtualizar.Visible = false;
                this.imbExcluir.Visible = false;
                this.ckbParcelaID.Text = "Atualizar/Excluir/Cadastrar";
            }
            this.LimparFormulario(this.txtParcelaID, this.txtPrazoMedio);
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
                    if (string.IsNullOrEmpty(this.txtParcelaID.Text))
                    {
                        throw new ApplicationException("A atualização de uma Parcela só pode ser feita pelo ParcelaID.\r\n\r\nDigite-o no campo ParcelaID e clique novamente no botão Atualizar.");
                    }
                    if (!DAL.ParcelaDAL.Listar(this.txtParcelaID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Parcela inexistente.\r\n\r\nInforme outra Parcela a ser atualizada.");
                    }
                    DAL.ParcelaDAL.Atualizar(this.txtParcelaID.Text, this.txtPrazoMedio.Text, usuarioSessao.SistemaID);
                    this.LimparFormulario(this.txtParcelaID, this.txtPrazoMedio);
                    this.CarregarDados();
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
                    if (string.IsNullOrEmpty(this.txtParcelaID.Text) || string.IsNullOrEmpty(this.txtPrazoMedio.Text))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                    }
                    if (DAL.ParcelaDAL.Listar(this.txtParcelaID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Parcela cadastrada.");
                    }
                    DAL.ParcelaDAL.Inserir(this.txtParcelaID.Text.Trim().ToUpper(), this.txtPrazoMedio.Text.Trim().ToUpper(), usuarioSessao.SistemaID);
                    this.LimparFormulario(this.txtParcelaID, this.txtPrazoMedio);
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
                    if (string.IsNullOrEmpty(this.txtParcelaID.Text) && string.IsNullOrEmpty(this.txtPrazoMedio.Text))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }
                    if (!this.CarregarDados(this.txtParcelaID.Text, this.txtPrazoMedio.Text))
                    {
                        throw new ApplicationException("Parcela inexistente.");
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
                    if (string.IsNullOrEmpty(this.txtParcelaID.Text))
                    {
                        throw new ApplicationException("A exclusão de uma Parcela só pode ser feita pela ParcelaID.\r\n\r\nDigite-o no campo ParcelaID e clique novamente no botão Excluir.");
                    }
                    if (!DAL.ParcelaDAL.Listar(this.txtParcelaID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Parcela inexistente.\r\n\r\nInforme outra Parcela a ser excluída.");
                    }
                    DAL.ParcelaDAL.Excluir(this.txtParcelaID.Text, usuarioSessao.SistemaID);
                    this.LimparFormulario(this.txtParcelaID, this.txtPrazoMedio);
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

        private void LimparFormulario(TextBox txtParcelaID, TextBox txtPrazoMedio)
        {
            txtParcelaID.Text = string.Empty;
            txtPrazoMedio.Text = string.Empty;
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
            this.gdvParcela.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.lblTopo.Text = "CONSULTA - PARCELA";
            if (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
            {
                this.ckbParcelaID.Visible = true;
                this.lblParcelaID.Visible = true;
                this.txtParcelaID.Visible = true;
            }
            else
            {
                this.imbAtualizar.Visible = false;
                this.imbExcluir.Visible = false;
                this.ckbParcelaID.Visible = false;
                this.lblParcelaID.Visible = false;
                this.txtParcelaID.Visible = false;
            }
        }
    }
}