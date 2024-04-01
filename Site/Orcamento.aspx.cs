using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;
using BLL;
using DAO;
using DAL;

namespace Site
{
    public partial class Orcamento : System.Web.UI.Page
    {
        private bool CarregarDados(bool filtro)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            DAL.LojaDAL lojaDAL = new DAL.LojaDAL();
            this.ViewState["filtroOrcamento"] = true;
            if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode())
            {
                this.rptLoja.DataSource = lojaDAL.ListarById(this.ddlLoja.SelectedValue, usuarioSessao.SistemaID);
                this.rptLoja.DataBind();
            }
            else if ((usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode()) || (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Vendedor.GetHashCode()))
            {
                this.rptLoja.DataSource = lojaDAL.ListarById(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
                this.rptLoja.DataBind();
            }
            return (this.rptLoja.Items.Count > 0);
        }

        private void CarregarDropDownListFuncionario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((this.Session["dsDropDownListFuncionario"] == null) || (this.Session["bdFuncionario"] != null))
            {
                this.Session["dsDropDownListFuncionario"] = DAL.FuncionarioDAL.ListarDropDownList(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
                this.Session["bdFuncionario"] = null;
            }
            this.ddlFuncionario.DataSource = this.Session["dsDropDownListFuncionario"];
            this.ddlFuncionario.DataBind();
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.ddlLoja.DataSource = new DAL.LojaDAL().ListarOrcamentoDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
            this.ddlLoja.DataBind();
        }

        private void CarregarRepeaterLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((this.Session["dsRepeaterLoja"] == null) || (this.Session["bdLoja"] != null))
            {
                this.Session["dsRepeaterLoja"] = new DAL.LojaDAL().ListarById(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
                this.Session["bdLoja"] = null;
            }
            this.rptLoja.DataSource = this.Session["dsRepeaterLoja"];
            this.rptLoja.DataBind();
        }

        protected void ckbOrcamentoID_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbOrcamentoID.Checked)
            {
                this.ckbOrcamentoID.Text = "Consultar";
                this.imbCadastrar.Visible = true;
                this.imbConsultar.Visible = false;
                this.imbExcluir.Visible = true;
            }
            else
            {
                this.ckbOrcamentoID.Text = "Cadastrar/Excluir";
                this.imbCadastrar.Visible = false;
                this.imbConsultar.Visible = true;
                this.imbExcluir.Visible = false;
            }
            this.LimparFormulario();
        }

        protected void imbCadastrar_Click(object sender, ImageClickEventArgs e)
        {
            DAO.OrcamentoDAO orcamento = new DAO.OrcamentoDAO() { StatusID = 3 };

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
                try
                {
                    DAL.ProdutoDAL produtoDAL = new DAL.ProdutoDAL();
                    BLL.OrcamentoBLL orcamentoBLL = new BLL.OrcamentoBLL();

                    if (((string.IsNullOrEmpty(this.txtOrcamentoID.Text) || !(this.ddlLoja.SelectedValue != "0")) || (!(this.ddlFuncionario.SelectedValue != "0") || string.IsNullOrEmpty(this.txtDataOrcamento.Text))) || !(this.txtDataOrcamento.Text != "__/__/____"))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                    }

                    if (Convert.ToDateTime(this.txtDataOrcamento.Text) != DateTime.Today)
                    {
                        throw new ApplicationException("A data do Orçamento deve ser igual a data de hoje.");
                    }

                    orcamento.OrcamentoID = Convert.ToInt32(this.txtOrcamentoID.Text);
                    orcamento.LojaID = Convert.ToInt32(this.ddlLoja.SelectedValue);
                    orcamento.FuncionarioID = Convert.ToInt32(this.ddlFuncionario.SelectedValue);
                    orcamento.DataOrcamento = Convert.ToDateTime(txtDataOrcamento.Text);
                    orcamento.SistemaID = usuarioSessao.SistemaID;

                    if (orcamentoBLL.ExisteOrcamento(orcamento))
                    {
                        throw new ApplicationException("Orçamento cadastrado.\r\n\r\nInforme outro OrcamentoID a ser cadastrado.");
                    }

                    for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
                    {
                        if (Request.Form["txtProdutoID_" + i] != null && Request.Form["txtQuantidade_" + i] != null && Request.Form["txtPreco_" + i] != null)
                        {
                            string produtoId = Request.Form.GetValues("txtProdutoID_" + i).FirstOrDefault();
                            string quantidade = Request.Form.GetValues("txtQuantidade_" + i).FirstOrDefault();
                            string preco = Request.Form.GetValues("txtPreco_" + i).FirstOrDefault();
                            string sobMedida = Request.Form.GetValues("txtSobMedida_" + i).FirstOrDefault();

                            if (string.IsNullOrEmpty(produtoId))
                            {
                                throw new ApplicationException(string.Format("Informe o produto {0}.", produtoId));
                            }

                            if ((string.IsNullOrEmpty(quantidade) || !(quantidade != "0")) || (!(quantidade != "00") || !(quantidade != "000")))
                            {
                                throw new ApplicationException(string.Format("Informe a quantidade do produto {0}.", produtoId));
                            }

                            if (string.IsNullOrEmpty(preco) || (preco == "0,00"))
                            {
                                throw new ApplicationException(string.Format("Informe o preço do produto {0}.", produtoId));
                            }

                            if (!produtoDAL.ExisteNaLoja(produtoId, this.ddlLoja.SelectedValue, orcamento.SistemaID))
                            {
                                throw new ApplicationException(string.Format("Produto {0} não está cadastrado na loja {1}.", produtoId, this.ddlLoja.SelectedItem.Text));
                            }

                            orcamento.ListaProduto.Add(new DAO.ProdutoDAO(Convert.ToInt64(produtoId), Convert.ToInt16(quantidade), sobMedida, Convert.ToDecimal(preco), orcamento.SistemaID));
                        }
                    }

                    if (orcamento.ListaProduto.Count() == 0)
                    {
                        throw new ApplicationException("Selecione os Produtos a serem inseridos no Orçamento.");
                    }

                    orcamentoBLL.Inserir(orcamento);

                    this.CarregarRepeaterLoja();
                    this.LimparFormulario();

                }
                catch (FormatException)
                {
                    UtilitarioBLL.ExibirMensagemAjax(this.Page, "Data inválida.");
                }
                catch (ApplicationException ex)
                {
                    UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message);
                }
                catch (Exception ex)
                {
                    if (ex.Source.ToUpper() == "SYSTEM.DATA")
                    {
                        UtilitarioBLL.ExibirMensagemAjax(this.Page, "Data inválida.");
                    }
                    else
                    {
                        UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message, ex);
                    }
                }
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
                    if ((string.IsNullOrEmpty(this.txtOrcamentoID.Text) && !(this.ddlLoja.SelectedValue != "0")) && ((this.ddlFuncionario.SelectedIndex <= 0) && (string.IsNullOrEmpty(this.txtDataOrcamento.Text) || !(this.txtDataOrcamento.Text != "__/__/____"))))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }
                    if (!this.CarregarDados(true))
                    {
                        throw new ApplicationException("Orçamento inexistente.");
                    }
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, "Data inválida.");
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Source.ToUpper() == "SYSTEM.DATA")
                {
                    UtilitarioBLL.ExibirMensagemAjax(this.Page, "Data inválida.");
                }
                else
                {
                    UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message, ex);
                }
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
                    BLL.OrcamentoBLL orcamentoBLL = new BLL.OrcamentoBLL();
                    DAO.OrcamentoDAO orcamentoDAO = new DAO.OrcamentoDAO();
                    if (string.IsNullOrEmpty(this.txtOrcamentoID.Text))
                    {
                        throw new ApplicationException("A exclusão de um orçamento só pode ser feita pelo OrçamentoID.\r\n\r\nInforme o OrçamentoID a ser excluído");
                    }
                    orcamentoDAO.OrcamentoID = Convert.ToInt32(this.txtOrcamentoID.Text);
                    orcamentoDAO.SistemaID = usuarioSessao.SistemaID;

                    if (!orcamentoBLL.ExisteOrcamento(orcamentoDAO))
                    {
                        throw new ApplicationException("Orçamento inexistente.\r\n\r\nInforme outro OrçamentoID a ser excluído.");
                    }

                    orcamentoBLL.Excluir(orcamentoDAO);

                    this.CarregarRepeaterLoja();
                    this.LimparFormulario();
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, "Data inválida.");
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

        private void LimparFormulario()
        {
            this.txtOrcamentoID.Text = string.Empty;
            if (this.ddlLoja.SelectedIndex != -1)
            {
                this.ddlLoja.SelectedIndex = 0;
            }
            this.ddlFuncionario.SelectedIndex = 0;
            this.txtTotalOrcamento.Text = string.Empty;
            this.txtDataOrcamento.Text = string.Empty;
            ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "LimparGrid", "document.getElementById('ctl00_ContentPlaceHolder1_hdfProdutoValores').value = ''; clearGrid();", true);
        }

        private void LimparPanel(TextBox txtPedidoID, TextBox txtCpf, TextBox txtDataPedido, TextBox txtDataEntrega, TextBox txtPanelObservacao)
        {
            txtPedidoID.Text = string.Empty;
            txtCpf.Text = string.Empty;
            txtDataPedido.Text = string.Empty;
            txtDataEntrega.Text = string.Empty;
            txtPanelObservacao.Text = string.Empty;
        }

        protected void lkbOrcamentoID_Click(object sender, EventArgs e)
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
                    UtilitarioBLL.RegistrarScriptAjax(this.Page, "jsmoeda", "$().ready(function() { $('.decimal').priceFormat({ prefix: '', centsSeparator: ',', thousandsSeparator: '.' }); });");
                    this.rpvOrcamento.ProcessingMode = ProcessingMode.Local;
                    this.rpvOrcamento.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "Orcamento.rdlc";
                    ReportDataSource rdsTeste = new ReportDataSource
                    {
                        Name = "dsOrcamento_spListarOrcamentoById",
                        Value = DAL.RelatorioDAL.ListarOrcamento(Convert.ToInt32(((LinkButton)sender).Text), usuarioSessao.SistemaID)
                    };
                    this.rpvOrcamento.LocalReport.DataSources.Clear();
                    this.rpvOrcamento.LocalReport.DataSources.Add(rdsTeste);
                    this.rpvOrcamento.LocalReport.Refresh();
                    this.mpeOrcamento.Show();
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
                    else if (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Estoquista))
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
                    this.VisualizarFormulario();
                    this.CarregarDropDownListLoja();
                    this.CarregarDropDownListFuncionario();
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

        protected void rptLoja_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                DAO.OrcamentoDAO orcamentoDAO = new DAO.OrcamentoDAO();
                BLL.OrcamentoBLL orcamentoBLL = new BLL.OrcamentoBLL();

                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    ((Label)e.Item.FindControl("lblNomeFantasia")).Text = ((Label)e.Item.FindControl("lblNomeFantasia")).Text.ToUpper();
                    Repeater rptOrcamento = (Repeater)e.Item.FindControl("rptOrcamento");

                    orcamentoDAO.OrcamentoID = string.IsNullOrEmpty(this.txtOrcamentoID.Text) ? 0 : Convert.ToInt32(this.txtOrcamentoID.Text);
                    orcamentoDAO.LojaID = string.IsNullOrEmpty(((Label)e.Item.FindControl("lblLojaID")).Text) ? 0 : Convert.ToInt32(((Label)e.Item.FindControl("lblLojaID")).Text);
                    orcamentoDAO.FuncionarioID = string.IsNullOrEmpty(this.ddlFuncionario.SelectedValue) ? 0 : Convert.ToInt32(this.ddlFuncionario.SelectedValue);
                    orcamentoDAO.DataOrcamento = string.IsNullOrEmpty(this.txtDataOrcamento.Text) || this.txtDataOrcamento.Text == "__/__/____" ? DateTime.MinValue : Convert.ToDateTime(this.txtDataOrcamento.Text);
                    orcamentoDAO.SistemaID = usuarioSessao.SistemaID;

                    if (this.ViewState["filtroOrcamento"] != null)
                    {
                        rptOrcamento.DataSource = orcamentoBLL.Listar(orcamentoDAO);
                        rptOrcamento.DataBind();
                    }
                    else
                    {
                        rptOrcamento.DataSource = orcamentoBLL.Listar(orcamentoDAO, true);
                        rptOrcamento.DataBind();
                    }
                    if (rptOrcamento.Items.Count <= 0)
                    {
                        ((Label)e.Item.FindControl("lblNomeFantasia")).Visible = false;
                        rptOrcamento.Visible = false;
                    }
                    else
                    {
                        if ((this.ViewState["filtroOrcamento"] != null) && (this.ViewState["Orcamento"] == null))
                        {
                            ((Label)e.Item.FindControl("lblNomeFantasia")).Visible = false;
                            rptOrcamento.Visible = false;
                        }
                        this.ViewState["Orcamento"] = null;
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

        protected void rptOrcamento_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                BLL.OrcamentoBLL orcamentoBLL = new BLL.OrcamentoBLL();
                DAO.OrcamentoDAO orcamentoDAO = new DAO.OrcamentoDAO();

                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    string OrcamentoId;
                    LinkButton lkbOrcamento = (LinkButton)e.Item.FindControl("lkbOrcamentoID");
                    Label lblOrcamento = (Label)e.Item.FindControl("lblOrcamentoID");
                    if (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Estoquista))
                    {
                        lkbOrcamento.Visible = false;
                        lblOrcamento.Visible = true;
                        OrcamentoId = this.lblOrcamentoID.Text;
                    }
                    else
                    {
                        lkbOrcamento.Visible = true;
                        lblOrcamento.Visible = false;
                        OrcamentoId = lkbOrcamento.Text;
                    }
                    if (((OrcamentoId != this.txtOrcamentoID.Text) && !string.IsNullOrEmpty(this.txtOrcamentoID.Text)) && (this.ViewState["filtroOrcamento"] != null))
                    {
                        e.Item.FindControl("repeaterOrcamento").Visible = false;
                    }
                    else
                    {
                        GridView gdvOrcamentoProduto = (GridView)e.Item.FindControl("gdvOrcamentoProduto");
                        gdvOrcamentoProduto.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
                        orcamentoDAO.OrcamentoID = Convert.ToInt32(OrcamentoId);
                        orcamentoDAO.SistemaID = usuarioSessao.SistemaID;
                        gdvOrcamentoProduto.DataSource = orcamentoBLL.ListarProduto(orcamentoDAO);
                        gdvOrcamentoProduto.DataBind();
                        this.ViewState["Orcamento"] = gdvOrcamentoProduto.Rows.Count;
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

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            //this.TotalOrcamento.Visible = false;
            if ((usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()) || (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode()))
            {
                this.ckbOrcamentoID.Visible = false;
                this.imbExcluir.Visible = false;
            }
        }
    }
}