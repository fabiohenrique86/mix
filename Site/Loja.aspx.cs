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
    public partial class Loja : System.Web.UI.Page
    {
        private bool CarregarDados(string cnpj, string razaoSocial, string nomeFantasia, string telefone, string cota, int sistemaId)
        {
            this.gdvLoja.DataSource = new DAL.LojaDAL().Listar(cnpj, razaoSocial, nomeFantasia, telefone, cota, sistemaId);
            this.gdvLoja.DataBind();
            return (this.gdvLoja.Rows.Count > 0);
        }

        private bool CarregarGridLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((this.Session["dsLoja"] == null) || (this.Session["bdLoja"] != null))
            {
                this.Session["dsLoja"] = new DAL.LojaDAL().Listar(usuarioSessao.SistemaID);
                this.Session["bdLoja"] = null;
            }
            this.gdvLoja.DataSource = this.Session["dsLoja"];
            this.gdvLoja.DataBind();
            return (this.gdvLoja.Rows.Count > 0);
        }

        protected void ckbLojaID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbLojaID.Checked)
            {
                this.txtLojaID.Enabled = true;
                this.txtLojaID.CssClass = "";
                this.imbCadastrar.Visible = false;
                this.imbConsultar.Visible = false;
                this.imbAtualizar.Visible = true;
                this.imbExcluir.Visible = true;
                this.ckbLojaID.Text = "Consultar/Cadastrar";
            }
            else
            {
                this.txtLojaID.Enabled = false;
                this.txtLojaID.CssClass = "desabilitado";
                this.imbCadastrar.Visible = true;
                this.imbConsultar.Visible = true;
                this.imbAtualizar.Visible = false;
                this.imbExcluir.Visible = false;
                this.ckbLojaID.Text = "Atualizar/Excluir";
            }
            this.LimparFormulario(this.txtLojaID, this.txtCnpj, this.txtNomeFantasia, this.txtRazaoSocial, this.txtTelefone, this.txtCota);
        }

        protected void gdvLoja_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                this.gdvLoja.PageIndex = e.NewPageIndex;
                if (((!string.IsNullOrEmpty(this.txtCnpj.Text) && (this.txtCnpj.Text != "__.___.___/____-__")) || ((!string.IsNullOrEmpty(this.txtRazaoSocial.Text) || !string.IsNullOrEmpty(this.txtNomeFantasia.Text)) || (!string.IsNullOrEmpty(this.txtTelefone.Text) && (this.txtTelefone.Text != "(__)____-____")))) || !string.IsNullOrEmpty(this.txtCota.Text))
                {
                    string cota = this.txtCota.Text;
                    if (cota == "0,00")
                    {
                        cota = "";
                    }
                    this.CarregarDados(this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", ""), this.txtRazaoSocial.Text, this.txtNomeFantasia.Text, this.txtTelefone.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", ""), cota, usuarioSessao.SistemaID);
                }
                else
                {
                    this.CarregarGridLoja();
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

        protected void gdvLoja_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (usuarioSessao.TipoUsuarioID != Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[5].Visible = false;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (usuarioSessao.TipoUsuarioID != Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[5].Visible = false;
                }
                string telefone = e.Row.Cells[4].Text;
                string cnpjFormatado = string.Format(@"{0:##\.###\.###/####-##}", Convert.ToInt64(e.Row.Cells[1].Text));
                if ((cnpjFormatado.Length > 4) && (cnpjFormatado.Length < 0x12))
                {
                    int quantidadeZeros = 0x12 - cnpjFormatado.Length;
                    if (quantidadeZeros > 0)
                    {
                        for (int i = 0; i < quantidadeZeros; i++)
                        {
                            cnpjFormatado = cnpjFormatado.Insert(i, "0");
                        }
                    }
                }
                e.Row.Cells[1].Text = cnpjFormatado;
                if (telefone.Length == 8)
                {
                    e.Row.Cells[4].Text = string.Format("{0:####-####}", Convert.ToInt64(telefone));
                }
                else if (telefone.Length == 10)
                {
                    e.Row.Cells[4].Text = string.Format("{0:(##)####-####}", Convert.ToInt64(telefone));
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
                    DAL.LojaDAL lojaDAL = new DAL.LojaDAL();

                    if (string.IsNullOrEmpty(this.txtLojaID.Text))
                    {
                        throw new ApplicationException("A atualização de uma Loja só pode ser feita pelo LojaID.\r\n\r\nDigite-o no campo LojaID e clique novamente no botão Atualizar.");
                    }
                    if (!string.IsNullOrEmpty(this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")))
                    {
                        throw new ApplicationException("Não é possível atualizar o CNPJ.");
                    }
                    if ((string.IsNullOrEmpty(this.txtRazaoSocial.Text.Trim().ToUpper()) && string.IsNullOrEmpty(this.txtNomeFantasia.Text.Trim().ToUpper())) && string.IsNullOrEmpty(this.txtTelefone.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para atualizar.");
                    }
                    if (!lojaDAL.Listar(this.txtLojaID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Loja inexistente. Informe outro LojaID a ser atualizada.");
                    }
                    string cota = this.txtCota.Text;
                    if (cota == "0,00")
                    {
                        cota = "";
                    }

                    lojaDAL.Atualizar(this.txtLojaID.Text, this.txtRazaoSocial.Text.Trim().ToUpper(), this.txtNomeFantasia.Text.Trim().ToUpper(), this.txtTelefone.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", ""), cota, usuarioSessao.SistemaID);

                    this.Session["bdLoja"] = true;
                    this.LimparFormulario(this.txtLojaID, this.txtCnpj, this.txtNomeFantasia, this.txtRazaoSocial, this.txtTelefone, this.txtCota);
                    this.CarregarGridLoja();
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
                    DAL.LojaDAL lojaDAL = new DAL.LojaDAL();

                    if (((string.IsNullOrEmpty(this.txtCnpj.Text) || string.IsNullOrEmpty(this.txtRazaoSocial.Text)) || (string.IsNullOrEmpty(this.txtNomeFantasia.Text) || string.IsNullOrEmpty(this.txtTelefone.Text))) || ((!(this.txtTelefone.Text != "(__)____-____") || string.IsNullOrEmpty(this.txtCota.Text)) || !(this.txtCota.Text != "0,00")))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                    }

                    string cnpjFormatado = this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");

                    if (!new BLL.UtilsBLL().ValidarCnpj(cnpjFormatado))
                    {
                        throw new ApplicationException("CNPJ inválido.");
                    }
                    if (lojaDAL.ListarFiltro(cnpjFormatado, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Loja cadastrada anteriormente. Informe outro CNPJ.");
                    }

                    lojaDAL.Inserir(cnpjFormatado, this.txtRazaoSocial.Text.Trim().ToUpper(), this.txtNomeFantasia.Text.Trim().ToUpper(), this.txtTelefone.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", ""), this.txtCota.Text, usuarioSessao.SistemaID);

                    this.Session["bdLoja"] = true;
                    this.LimparFormulario(this.txtLojaID, this.txtCnpj, this.txtRazaoSocial, this.txtNomeFantasia, this.txtTelefone, this.txtCota);
                    this.CarregarGridLoja();
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
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                    if (((string.IsNullOrEmpty(this.txtCnpj.Text) || (this.txtCnpj.Text == "__.___.___/____-__")) && ((string.IsNullOrEmpty(this.txtRazaoSocial.Text) && string.IsNullOrEmpty(this.txtNomeFantasia.Text)) && (string.IsNullOrEmpty(this.txtTelefone.Text) || (this.txtTelefone.Text == "(__)____-____")))) && string.IsNullOrEmpty(this.txtCota.Text))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }
                    string cnpjFormatado = this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                    string cota = this.txtCota.Text;
                    if (cota == "0,00")
                    {
                        cota = "";
                    }
                    if (!this.CarregarDados(cnpjFormatado, this.txtRazaoSocial.Text, this.txtNomeFantasia.Text, this.txtTelefone.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", ""), cota, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Loja inexistente.");
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
                    DAL.LojaDAL lojaDAL = new DAL.LojaDAL();

                    if (string.IsNullOrEmpty(this.txtLojaID.Text))
                    {
                        throw new ApplicationException("A exclusão de uma loja só pode ser feita pelo LojaID.\r\n\r\nDigite-o no campo LojaID e clique novamente no botão Excluir.");
                    }
                    if (!lojaDAL.Listar(this.txtLojaID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Loja inexistente. Informe outro LojaID a ser excluído.");
                    }

                    lojaDAL.Excluir(this.txtLojaID.Text, usuarioSessao.SistemaID);

                    this.Session["bdLoja"] = true;
                    this.LimparFormulario(this.txtLojaID, this.txtCnpj, this.txtNomeFantasia, this.txtRazaoSocial, this.txtTelefone, this.txtCota);
                    this.CarregarGridLoja();
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

        private void LimparFormulario(TextBox lojaId, TextBox cnpj, TextBox razaoSocial, TextBox nomeFantasia, TextBox telefone, TextBox cota)
        {
            lojaId.Text = string.Empty;
            cnpj.Text = string.Empty;
            razaoSocial.Text = string.Empty;
            nomeFantasia.Text = string.Empty;
            telefone.Text = string.Empty;
            cota.Text = string.Empty;
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
                        this.CarregarGridLoja();
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
            this.gdvLoja.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.lblTopo.Text = "CONSULTA - LOJA";
            if (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
            {
                this.imbCadastrar.Visible = true;
                this.ckbLojaID.Visible = true;
                this.lblLojaID.Visible = true;
                this.txtLojaID.Visible = true;
                this.lblCota.Visible = true;
                this.txtCota.Visible = true;
            }
            else
            {
                this.imbCadastrar.Visible = false;
                this.ckbLojaID.Visible = false;
                this.lblLojaID.Visible = false;
                this.txtLojaID.Visible = false;
                this.lblCota.Visible = false;
                this.txtCota.Visible = false;
            }
        }
    }
}