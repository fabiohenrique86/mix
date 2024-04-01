using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using DAO;
using DAL;

namespace Site
{
    public partial class Brinde : System.Web.UI.Page
    {
        private void CarregarDados()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            DataSet ds = new DAL.ProdutoDAL().ListarDropDownList(null, usuarioSessao.SistemaID);
            this.ddlProduto.DataSource = ds;
            this.ddlProduto.DataBind();
            this.ddlProdutoBrinde.DataSource = ds;
            this.ddlProdutoBrinde.DataBind();
            this.rptProduto.DataSource = DAL.BrindeDAL.Listar(usuarioSessao.SistemaID);
            this.rptProduto.DataBind();
        }

        private bool CarregarDados(bool filtro)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.ViewState["filtroBrinde"] = true;
            this.rptProduto.DataSource = DAL.BrindeDAL.Listar(usuarioSessao.SistemaID);
            this.rptProduto.DataBind();
            return (this.rptProduto.Items.Count > 0);
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
                    if ((((this.ddlProduto.SelectedIndex <= 0) || (this.ddlProdutoBrinde.SelectedIndex <= 0)) || (string.IsNullOrEmpty(this.txtQuantidade.Text.Trim().ToUpper()) || !(this.txtQuantidade.Text.Trim().ToUpper() != "0"))) || (string.IsNullOrEmpty(this.txtPreco.Text.Trim().ToUpper()) || !(this.txtPreco.Text.Trim().ToUpper() != "0,00")))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                    }
                    if (DAL.BrindeDAL.Listar(Convert.ToInt64(this.ddlProduto.SelectedValue), Convert.ToInt64(this.ddlProdutoBrinde.SelectedValue), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Brinde cadastrado ao produto selecionado.");
                    }
                    DAL.BrindeDAL.Inserir(this.ddlProduto.SelectedValue, this.ddlProdutoBrinde.SelectedValue, usuarioSessao.SistemaID, Convert.ToInt32(this.txtQuantidade.Text.Trim().ToUpper()), this.txtPreco.Text.Trim().ToUpper());
                    this.TravarFormulario(this.ddlProduto, this.ddlProdutoBrinde, this.imbCadastrar, this.imbCadastrarBrinde, this.imbFinalizarBrinde, this.txtQuantidade, this.txtPreco);
                    this.rptProduto.DataSource = DAL.BrindeDAL.Listar(usuarioSessao.SistemaID);
                    this.rptProduto.DataBind();
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

        protected void imbCadastrarBrinde_Click(object sender, ImageClickEventArgs e)
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
                    if ((((this.ddlProduto.SelectedIndex <= 0) || (this.ddlProdutoBrinde.SelectedIndex <= 0)) || (string.IsNullOrEmpty(this.txtQuantidade.Text.Trim().ToUpper()) || !(this.txtQuantidade.Text.Trim().ToUpper() != "0"))) || (string.IsNullOrEmpty(this.txtPreco.Text.Trim().ToUpper()) || !(this.txtPreco.Text.Trim().ToUpper() != "0,00")))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                    }
                    if (DAL.BrindeDAL.Listar(Convert.ToInt64(this.ddlProduto.SelectedValue), Convert.ToInt64(this.ddlProdutoBrinde.SelectedValue), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Brinde cadastrado ao produto selecionado.");
                    }
                    DAL.BrindeDAL.Inserir(this.ddlProduto.SelectedValue, this.ddlProdutoBrinde.SelectedValue, usuarioSessao.SistemaID, Convert.ToInt32(this.txtQuantidade.Text.Trim().ToUpper()), this.txtPreco.Text.Trim().ToUpper());
                    this.TravarFormulario(this.ddlProduto, this.ddlProdutoBrinde, this.imbCadastrar, this.imbCadastrarBrinde, this.imbFinalizarBrinde, this.txtQuantidade, this.txtPreco);
                    this.rptProduto.DataSource = DAL.BrindeDAL.Listar(usuarioSessao.SistemaID);
                    this.rptProduto.DataBind();
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
                    if ((((this.ddlProduto.SelectedIndex <= 0) && (this.ddlProdutoBrinde.SelectedIndex <= 0)) && (string.IsNullOrEmpty(this.txtQuantidade.Text.Trim().ToUpper()) || (this.txtQuantidade.Text.Trim().ToUpper() == "0"))) && (string.IsNullOrEmpty(this.txtPreco.Text.Trim().ToUpper()) || !(this.txtPreco.Text.Trim().ToUpper() != "0,00")))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }
                    if (!this.CarregarDados(true))
                    {
                        throw new ApplicationException("Brinde ou Produto inexistente.");
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
                    if ((this.ddlProduto.SelectedIndex <= 0) || (this.ddlProdutoBrinde.SelectedIndex <= 0))
                    {
                        throw new ApplicationException("É necessário informar o Produto e o Brinde para excluir.");
                    }
                    if (!DAL.BrindeDAL.Listar(Convert.ToInt64(this.ddlProduto.SelectedValue), Convert.ToInt64(this.ddlProdutoBrinde.SelectedValue), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Brinde inexistente ao produto selecionado.");
                    }
                    DAL.BrindeDAL.Excluir(this.ddlProduto.SelectedValue, this.ddlProdutoBrinde.SelectedValue, usuarioSessao.SistemaID);
                    this.LimparFormulario(this.ddlProduto, this.ddlProdutoBrinde, this.imbCadastrar, this.imbCadastrarBrinde, this.imbFinalizarBrinde, this.txtQuantidade, this.txtPreco);
                    this.rptProduto.DataSource = DAL.BrindeDAL.Listar(usuarioSessao.SistemaID);
                    this.rptProduto.DataBind();
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

        protected void imbFinalizarBrinde_Click(object sender, ImageClickEventArgs e)
        {
            this.LimparFormulario(this.ddlProduto, this.ddlProdutoBrinde, this.imbCadastrar, this.imbCadastrarBrinde, this.imbFinalizarBrinde, this.txtQuantidade, this.txtPreco);
        }

        private void LimparFormulario(DropDownList ddlProduto, DropDownList ddlProdutoBrinde, ImageButton imbCadastrar, ImageButton imbCadastrarBrinde, ImageButton imbFinalizarBrinde, TextBox txtQuantidade, TextBox txtPreco)
        {
            ddlProduto.Enabled = true;
            ddlProduto.CssClass = "";
            ddlProduto.SelectedIndex = 0;
            ddlProdutoBrinde.SelectedIndex = 0;
            this.imbConsultar.Visible = true;
            imbCadastrar.Visible = true;
            this.imbExcluir.Visible = true;
            imbCadastrarBrinde.Visible = false;
            imbFinalizarBrinde.Visible = false;
            txtQuantidade.Text = string.Empty;
            txtPreco.Text = string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UtilitarioBLL.SetarMascaraValor(this.Page);
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

        protected void rptProduto_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    string produtoId;
                    GridView gdvBrindeAux = (GridView)e.Item.FindControl("gdvBrinde");
                    gdvBrindeAux.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
                    if (((((this.ddlProduto.SelectedIndex > 0) || (this.ddlProdutoBrinde.SelectedIndex > 0)) || (!string.IsNullOrEmpty(this.txtQuantidade.Text.Trim().ToUpper()) && (this.txtQuantidade.Text.Trim().ToUpper() != "0"))) || (!string.IsNullOrEmpty(this.txtPreco.Text.Trim().ToUpper()) && (this.txtPreco.Text.Trim().ToUpper() != "0,00"))) && (this.ViewState["filtroBrinde"] != null))
                    {
                        produtoId = ((Label)e.Item.FindControl("lblProdutoID")).Text.Trim().ToUpper();
                        gdvBrindeAux.DataSource = DAL.BrindeDAL.Listar(produtoId, this.ddlProdutoBrinde.SelectedValue, this.txtQuantidade.Text.Trim().ToUpper(), this.txtPreco.Text.Trim().ToUpper(), usuarioSessao.SistemaID);
                        gdvBrindeAux.DataBind();
                    }
                    else
                    {
                        produtoId = ((Label)e.Item.FindControl("lblProdutoID")).Text.Trim().ToUpper();
                        gdvBrindeAux.DataSource = DAL.BrindeDAL.Listar(produtoId, usuarioSessao.SistemaID);
                        gdvBrindeAux.DataBind();
                    }
                    if ((gdvBrindeAux.Rows.Count <= 0) || (((this.ddlProduto.SelectedValue != produtoId) && (this.ddlProduto.SelectedIndex > 0)) && (this.ViewState["filtroBrinde"] != null)))
                    {
                        ((Label)e.Item.FindControl("lblDescricao")).Visible = false;
                        gdvBrindeAux.Visible = false;
                    }
                }
                else
                {
                    ((Label)e.Item.FindControl("lblDescricao")).Visible = false;
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

        private void TravarFormulario(DropDownList ddlProduto, DropDownList ddlProdutoBrinde, ImageButton imbCadastrar, ImageButton imbCadastrarBrinde, ImageButton imbFinalizarBrinde, TextBox txtQuantidade, TextBox txtPreco)
        {
            ddlProduto.Enabled = false;
            ddlProduto.CssClass = "desabilitado";
            ddlProdutoBrinde.SelectedIndex = 0;
            this.imbConsultar.Visible = false;
            imbCadastrar.Visible = false;
            this.imbExcluir.Visible = false;
            imbCadastrarBrinde.Visible = true;
            imbFinalizarBrinde.Visible = true;
            txtQuantidade.Text = string.Empty;
            txtPreco.Text = string.Empty;
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.lblTopo.Text = "CONSULTA - BRINDE";
            if (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
            {
                this.imbCadastrar.Visible = true;
                this.imbExcluir.Visible = true;
            }
            else
            {
                this.imbCadastrar.Visible = false;
                this.imbExcluir.Visible = false;
            }
        }
    }
}