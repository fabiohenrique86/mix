using AjaxControlToolkit;
using BLL;
using ClosedXML.Excel;
using DAL;
using DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site
{
    public partial class Produto : Page
    {
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static ProdutoDAO[] GetProduct(string term, string lojaId, string sistemaId)
        {
            var retorno = new List<ProdutoDAO>();
            var listaProduto = new ProdutoDAL().ListarGridView(lojaId, Convert.ToInt32(sistemaId), term).Tables[0];

            if (listaProduto != null)
            {
                for (int i = 0; i < listaProduto.Rows.Count; i++)
                {
                    retorno.Add(new ProdutoDAO()
                    {
                        ProdutoID = Convert.ToInt64(listaProduto.Rows[i]["ProdutoID"]),
                        Descricao = listaProduto.Rows[i]["Descricao"].ToString()
                    });
                }
            }

            return retorno.ToArray();
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
                        VisualizarFormulario();
                        CarregarDropDownListLinha();
                        CarregarDropDownListMedida();
                        CarregarGridLoja();
                    }
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
            }
        }

        private void CarregarDados(bool filtro)
        {
            ViewState["filtro"] = true;
            CarregarRepeaterLoja();
        }

        private void CarregarDropDownListLinha()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((Session["dsDropDownListLinha"] == null) || (Session["bdLinha"] != null))
            {
                Session["dsDropDownListLinha"] = DAL.LinhaDAL.ListarDropDownList(usuarioSessao.SistemaID);
                Session["bdLinha"] = null;
            }
            ddlLinha.DataSource = Session["dsDropDownListLinha"];
            ddlLinha.DataBind();
        }

        private void CarregarDropDownListMedida()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            ddlMedida.DataSource = DAL.MedidaDAL.ListarDropDownList(usuarioSessao.SistemaID);
            ddlMedida.DataBind();
        }

        private void CarregarGridLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((Session["dsLoja"] == null) || (Session["bdLoja"] != null))
            {
                Session["dsLoja"] = new DAL.LojaDAL().Listar(usuarioSessao.SistemaID);
                Session["bdLoja"] = null;
            }
            gdvLoja.DataSource = Session["dsLoja"];
            gdvLoja.DataBind();
        }

        private void CarregarRepeaterLoja()
        {
            var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            if ((Session["dsLoja"] == null) || (Session["bdLoja"] != null))
            {
                Session["dsLoja"] = new LojaDAL().Listar(usuarioSessao.SistemaID);
                Session["bdLoja"] = null;
            }

            var lista = ((DataSet)Session["dsLoja"]).
                        Tables[0].
                        AsEnumerable().
                        Select(dataRow => new LojaDAO
                        {
                            LojaID = dataRow.Field<int>("LojaID"),
                            CNPJ = dataRow.Field<string>("Cnpj"),
                            RazaoSocial = dataRow.Field<string>("RazaoSocial"),
                            NomeFantasia = dataRow.Field<string>("NomeFantasia"),
                            Telefone = dataRow.Field<string>("Telefone"),
                            Cota = dataRow.Field<double>("Cota"),
                            SistemaID = usuarioSessao.SistemaID
                        }).ToList();

            foreach (GridViewRow gdrLoja in gdvLoja.Rows)
            {
                if (!((CheckBox)gdrLoja.FindControl("ckbLoja")).Checked)
                {
                    var lojaId = Convert.ToInt32(((Label)gdrLoja.FindControl("lblLojaID")).Text);
                    lista.Remove(lista.FirstOrDefault(x => x.LojaID == lojaId));
                }
            }

            acdProduto.DataSource = lista;
            acdProduto.DataBind();
        }

        protected void ckbProdutoID_CheckedChanged(object sender, EventArgs e)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            LimparFormulario();

            if (ckbProdutoID.Checked)
            {
                txtProdutoID.Enabled = true;
                txtProdutoID.CssClass = string.Empty;

                if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode())
                {
                    imbCadastrar.Visible = true;
                    imbConsultar.Visible = false;
                    imbAtualizar.Visible = false;
                    imbExcluir.Visible = false;
                }
                else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode())
                {
                    imbCadastrar.Visible = true;
                    imbConsultar.Visible = false;
                    imbAtualizar.Visible = false;
                    imbExcluir.Visible = false;
                }
                else
                {
                    imbCadastrar.Visible = true;
                    imbConsultar.Visible = false;
                    imbAtualizar.Visible = true;
                    imbExcluir.Visible = true;
                }
                ckbProdutoID.Text = "Consultar";
            }
            else
            {
                txtProdutoID.Enabled = false;
                txtProdutoID.CssClass = "desabilitado";
                imbCadastrar.Visible = false;
                imbConsultar.Visible = true;
                imbAtualizar.Visible = false;
                imbExcluir.Visible = false;

                if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode())
                {
                    ckbProdutoID.Text = "Cadastrar";
                }
                else
                {
                    ckbProdutoID.Text = "Cadastrar/Atualizar/Excluir";
                }
            }
        }

        protected void gdvProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if (e.Row.RowType == DataControlRowType.Header)
            {
                switch (usuarioSessao.TipoUsuarioID)
                {
                    case 2:
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        break;
                    case 3:
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        break;
                    case 4:
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                        e.Row.Cells[10].Visible = false;
                        e.Row.Cells[11].Visible = false;
                        break;
                    case 5:
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                        e.Row.Cells[10].Visible = false;
                        e.Row.Cells[11].Visible = false;
                        break;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].ToolTip = "Reserva: " + e.Row.Cells[8].Text + " | A Programar: " + e.Row.Cells[11].Text;
                
                switch (usuarioSessao.TipoUsuarioID)
                {
                    case 2:
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        break;
                    case 3:
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        break;
                    case 4:
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                        e.Row.Cells[10].Visible = false;
                        e.Row.Cells[11].Visible = false;
                        break;
                    case 5:
                        e.Row.Cells[5].Visible = false;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                        e.Row.Cells[10].Visible = false;
                        e.Row.Cells[11].Visible = false;
                        break;
                }

                if (Convert.ToInt32(e.Row.Cells[6].Text) < 0)
                {
                    e.Row.ForeColor = Color.Red;
                }
            }
        }

        protected void imbZerar_Click(object sender, ImageClickEventArgs e)
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
                    var lojaId = 0;
                    var produtoBLL = new ProdutoBLL();
                    var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                    foreach (GridViewRow gdrLoja in this.gdvLoja.Rows)
                    {
                        if (((CheckBox)gdrLoja.FindControl("ckbLoja")).Checked)
                        {
                            lojaId = Convert.ToInt32(((Label)gdrLoja.FindControl("lblLojaID")).Text);

                            produtoBLL.Zerar(new ProdutoDAO() { SistemaID = usuarioSessao.SistemaID, LojaID = lojaId });
                        }
                    }

                    if (lojaId <= 0)
                    {
                        throw new ApplicationException("Selecione ao menos uma loja para zerar o estoque.");
                    }

                    CarregarRepeaterLoja();

                    UtilitarioBLL.ExibirMensagemAjax(this.Page, "Estoque zerado com sucesso");
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
                    DAL.ProdutoDAL produtoDAL = new DAL.ProdutoDAL();
                    string produtoId = txtProdutoID.Text.Replace(".", "").Replace(",", "").Replace("_", "").Replace("-", "");
                    bool stQuantidade = false;

                    if (string.IsNullOrEmpty(produtoId))
                    {
                        throw new ApplicationException("Informe o ProdutoID para efetuar a atualização do produto.");
                    }
                    List<ProdutoDAO> listaProduto = new List<ProdutoDAO>();
                    foreach (GridViewRow gdrLoja in this.gdvLoja.Rows)
                    {
                        if (((CheckBox)gdrLoja.FindControl("ckbLoja")).Checked)
                        {
                            string lojaId = ((Label)gdrLoja.FindControl("lblLojaID")).Text;
                            string nomeFantasia = ((Label)gdrLoja.FindControl("lblNomeFantasia")).Text;
                            short quantidade;
                            short.TryParse(((TextBox)gdrLoja.FindControl("txtQuantidade")).Text.Trim(), out quantidade);
                            stQuantidade = true;
                            if (!produtoDAL.ExisteNaLoja(produtoId, lojaId, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException(string.Format("Produto inexistente na loja {0}", nomeFantasia));
                            }
                            listaProduto.Add(new ProdutoDAO(Convert.ToInt64(produtoId), Convert.ToInt32(lojaId), Convert.ToInt32(this.ddlLinha.SelectedValue), string.IsNullOrEmpty(this.txtComissaoFuncionario.Text.Trim().ToUpper()) ? Convert.ToInt16(0) : Convert.ToInt16(this.txtComissaoFuncionario.Text.Trim().ToUpper()), string.IsNullOrEmpty(this.txtComissaoFranqueado.Text.Trim().ToUpper()) ? Convert.ToInt16(0) : Convert.ToInt16(this.txtComissaoFranqueado.Text.Trim().ToUpper()), this.txtDescricao.Text.Trim().ToUpper(), Convert.ToInt32(this.ddlMedida.SelectedValue), quantidade, usuarioSessao.SistemaID));
                        }
                    }
                    if (!stQuantidade)
                    {
                        listaProduto.Add(new ProdutoDAO(Convert.ToInt64(produtoId), 0, Convert.ToInt32(this.ddlLinha.SelectedValue), string.IsNullOrEmpty(this.txtComissaoFuncionario.Text.Trim().ToUpper()) ? Convert.ToInt16(0) : Convert.ToInt16(this.txtComissaoFuncionario.Text.Trim().ToUpper()), string.IsNullOrEmpty(this.txtComissaoFranqueado.Text.Trim().ToUpper()) ? Convert.ToInt16(0) : Convert.ToInt16(this.txtComissaoFranqueado.Text.Trim().ToUpper()), this.txtDescricao.Text.Trim().ToUpper(), Convert.ToInt32(this.ddlMedida.SelectedValue), Convert.ToInt16(0), usuarioSessao.SistemaID));
                    }
                    else if (listaProduto.Count <= 0)
                    {
                        throw new ApplicationException("Selecione ao menos uma loja para efetuar a atualização do Produto.");
                    }

                    produtoDAL.Atualizar(listaProduto);

                    Session["bdProduto"] = true;
                    LimparFormulario();
                    CarregarRepeaterLoja();
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
                    DAL.ProdutoDAL produtoDAL = new DAL.ProdutoDAL();
                    string produtoId = this.txtProdutoID.Text.Replace(".", "").Replace(",", "").Replace("_", "").Replace("-", "");
                    List<ProdutoDAO> listaProduto = new List<ProdutoDAO>();

                    if (((string.IsNullOrEmpty(produtoId) || (this.ddlLinha.SelectedIndex <= 0)) || (string.IsNullOrEmpty(this.txtComissaoFuncionario.Text) || !(this.txtComissaoFuncionario.Text != "0"))) || ((string.IsNullOrEmpty(this.txtComissaoFranqueado.Text) || !(this.txtComissaoFranqueado.Text != "0")) || (string.IsNullOrEmpty(this.txtDescricao.Text) || (this.ddlMedida.SelectedIndex <= 0))))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                    }

                    foreach (GridViewRow gdrLoja in this.gdvLoja.Rows)
                    {
                        if (((CheckBox)gdrLoja.FindControl("ckbLoja")).Checked)
                        {
                            string lojaId = ((Label)gdrLoja.FindControl("lblLojaID")).Text;
                            string nomeFantasia = ((Label)gdrLoja.FindControl("lblNomeFantasia")).Text;
                            short quantidade;
                            short.TryParse(((TextBox)gdrLoja.FindControl("txtQuantidade")).Text.Trim(), out quantidade);

                            if (produtoDAL.ExisteNaLoja(produtoId, lojaId, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException(string.Format("Produto cadastrado na loja {0}.", nomeFantasia));
                            }

                            listaProduto.Add(new ProdutoDAO(Convert.ToInt64(produtoId), Convert.ToInt32(lojaId), Convert.ToInt32(this.ddlLinha.SelectedValue), Convert.ToInt16(this.txtComissaoFuncionario.Text.Trim().ToUpper()), Convert.ToInt16(this.txtComissaoFranqueado.Text.Trim().ToUpper()), this.txtDescricao.Text.Trim().ToUpper(), Convert.ToInt32(this.ddlMedida.SelectedValue), quantidade, usuarioSessao.SistemaID));
                        }
                    }

                    if (listaProduto.Count <= 0)
                    {
                        throw new ApplicationException("Selecione ao menos uma loja para efetuar o cadastramento do produto.");
                    }

                    produtoDAL.Inserir(listaProduto);

                    Session["bdProduto"] = true;
                    LimparFormulario();
                    CarregarRepeaterLoja();
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
                    var temLojaSelecionada = false;
                    foreach (GridViewRow gdrLoja in gdvLoja.Rows)
                    {
                        if (((CheckBox)gdrLoja.FindControl("ckbLoja")).Checked)
                        {
                            temLojaSelecionada = true;
                            break;
                        }
                    }

                    if (!temLojaSelecionada && this.ddlLinha.SelectedIndex <= 0 && string.IsNullOrEmpty(this.txtComissaoFuncionario.Text) && string.IsNullOrEmpty(this.txtComissaoFranqueado.Text) && string.IsNullOrEmpty(this.txtDescricao.Text) && this.ddlMedida.SelectedIndex <= 0 && !ckbFlgExibirForaDeLinha.Checked)
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }

                    this.CarregarDados(true);
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
                    var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                    var produtoDAL = new ProdutoDAL();
                    var listaProduto = new List<ProdutoDAO>();

                    foreach (AccordionPane loja in acdProduto.Panes)
                    {
                        string lojaId = ((Label)loja.FindControl("lblLojaID")).Text;
                        GridView gdvProduto = (GridView)loja.FindControl("gdvProduto");

                        foreach (GridViewRow gdrProduto in gdvProduto.Rows)
                        {
                            if (((CheckBox)gdrProduto.FindControl("ckbProduto")).Checked)
                            {
                                string produtoId = gdrProduto.Cells[1].Text;

                                listaProduto.Add(new ProdutoDAO(Convert.ToInt64(produtoId), Convert.ToInt32(lojaId), usuarioSessao.SistemaID));
                            }
                        }
                    }

                    if (listaProduto.Count <= 0)
                    {
                        throw new ApplicationException("Selecione ao menos um produto para excluir.");
                    }

                    produtoDAL.Excluir(listaProduto);

                    Session["bdProduto"] = true;
                    LimparFormulario();
                    CarregarRepeaterLoja();
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
            }
        }

        private void LimparFormulario()
        {
            txtProdutoID.Text = string.Empty;
            foreach (GridViewRow gdrLoja in gdvLoja.Rows)
            {
                ((CheckBox)gdrLoja.FindControl("ckbLoja")).Checked = false;
                ((TextBox)gdrLoja.FindControl("txtQuantidade")).Text = string.Empty;
            }
            ddlLinha.SelectedIndex = 0;
            txtComissaoFuncionario.Text = string.Empty;
            txtComissaoFranqueado.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            ddlMedida.SelectedIndex = 0;
        }

        protected void acdProduto_ItemDataBound(object sender, AccordionItemEventArgs e)
        {
            try
            {
                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                if (e.ItemType == AccordionItemType.Content)
                {
                    string lojaId = ((Label)e.AccordionItem.Parent.FindControl("lblLojaID")).Text;
                    ((Label)e.AccordionItem.Parent.FindControl("lblNomeFantasia")).Text = ((Label)e.AccordionItem.Parent.FindControl("lblNomeFantasia")).Text.ToUpper();

                    var gdvProduto = (GridView)e.AccordionItem.FindControl("gdvProduto");
                    gdvProduto.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);

                    var produtoDAL = new ProdutoDAL();

                    if (Convert.ToBoolean(ViewState["filtro"]))
                    {
                        gdvProduto.DataSource = produtoDAL.Listar(lojaId, this.ddlLinha.SelectedValue, this.txtComissaoFuncionario.Text, this.txtComissaoFranqueado.Text, this.txtDescricao.Text, this.ddlMedida.SelectedValue, usuarioSessao.SistemaID, 0, ckbFlgExibirForaDeLinha.Checked);
                        gdvProduto.DataBind();
                    }
                    else
                    {
                        gdvProduto.DataSource = produtoDAL.Listar(lojaId, usuarioSessao.SistemaID);
                        gdvProduto.DataBind();
                    }

                    if ((gdvProduto.Rows.Count <= 0) && (ViewState["filtro"] != null))
                    {
                        ((Label)e.AccordionItem.FindControl("lblNomeFantasia")).Visible = false;
                        gdvProduto.Visible = false;
                    }
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
            }
        }

        protected void imbImportarColeta_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var produtoDAL = new ProdutoDAL();
                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                List<string> arquivoColeta = null;
                var listaProduto = new List<ProdutoDAO>();
                HttpPostedFile file = null;

                try
                {
                    file = Request.Files[0];
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Informe o arquivo de coleta");
                }

                if (file == null || file.ContentLength <= 0)
                    throw new ApplicationException("Informe o arquivo de coleta");

                if (!file.FileName.ToLower().Contains(".txt"))
                    throw new ApplicationException("Arquivo de coleta informado não está no formato TXT");


                using (StreamReader sr = new StreamReader(file.InputStream))
                {
                    var a = sr.ReadToEnd();
                    arquivoColeta = a.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                }

                var lojas = new List<int>();
                foreach (GridViewRow gdrLoja in gdvLoja.Rows)
                {
                    if (((CheckBox)gdrLoja.FindControl("ckbLoja")).Checked)
                    {
                        lojas.Add(Convert.ToInt32(((Label)gdrLoja.FindControl("lblLojaID")).Text));
                    }
                }

                if (lojas == null || lojas.Count() <= 0)
                {
                    throw new ApplicationException("Selecione ao menos uma loja");
                }

                // loop nos produtos do arquivo de coleta
                foreach (var linhaArquivoColeta in arquivoColeta)
                {
                    var campos = linhaArquivoColeta.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    // loop nas lojas
                    foreach (int lojaId in lojas)
                    {
                        var existeProduto = campos.ElementAtOrDefault(1) != null;
                        var existeQuantidade = campos.ElementAtOrDefault(5) != null;

                        if (!existeProduto || !existeQuantidade)
                        {
                            continue;
                        }

                        long produtoId = 0;
                        long.TryParse(campos[1], out produtoId);

                        if (produtoId <= 0)
                        {
                            continue;
                        }

                        short quantidade = 0;
                        short.TryParse(campos[5], out quantidade);

                        if (quantidade <= 0)
                        {
                            continue;
                        }

                        var produtoDAO = new ProdutoDAO();

                        produtoDAO.ProdutoID = produtoId;
                        produtoDAO.Quantidade = quantidade;
                        produtoDAO.SistemaID = new BLL.Modelo.Usuario(Session["Usuario"]).SistemaID;
                        produtoDAO.LojaID = lojaId;

                        listaProduto.Add(produtoDAO);
                    }
                }

                if (listaProduto == null || listaProduto.Count() <= 0)
                {
                    throw new ApplicationException("Nenhum produto foi encontrado no arquivo de coleta");
                }

                produtoDAL.AtualizarViaArquivoDeColeta(listaProduto);

                CarregarRepeaterLoja();

                UtilitarioBLL.ExibirMensagemAjax(this.Page, "Arquivo de coleta importado com sucesso");
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
            }
        }

        protected void imbImportarBancoDados_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var produtoDAL = new ProdutoDAL();
                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                List<string> arquivoBancoDados = null;
                var listaProduto = new List<ProdutoDAO>();
                var produtoBLL = new ProdutoBLL();
                HttpPostedFile file = null;

                try
                {
                    file = Request.Files[1];
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Informe o arquivo de banco de dados");
                }

                if (file == null || file.ContentLength <= 0)
                {
                    throw new ApplicationException("Informe o arquivo de banco de dados");
                }

                if (!file.FileName.ToLower().Contains(".txt"))
                {
                    throw new ApplicationException("Arquivo de banco de dados informado não está no formato TXT");
                }

                using (StreamReader sr = new StreamReader(file.InputStream))
                {
                    var a = sr.ReadToEnd();
                    arquivoBancoDados = a.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                }

                var lojas = new List<int>();
                foreach (GridViewRow gdrLoja in gdvLoja.Rows)
                {
                    if (((CheckBox)gdrLoja.FindControl("ckbLoja")).Checked)
                    {
                        lojas.Add(Convert.ToInt32(((Label)gdrLoja.FindControl("lblLojaID")).Text));
                    }
                }

                if (lojas == null || lojas.Count() <= 0)
                {
                    throw new ApplicationException("Selecione ao menos uma loja");
                }

                // loop nos produtos do arquivo de banco de dados
                // formato do TXT: ProdutoID | Linha | Descrição | Medida | % Func | % Franq
                foreach (var linha in arquivoBancoDados)
                {
                    var campos = linha.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    // loop nas lojas
                    foreach (int lojaId in lojas)
                    {
                        var existeProduto = campos.ElementAtOrDefault(0) != null;

                        if (!existeProduto)
                        {
                            continue;
                        }

                        long produtoId = 0;
                        long.TryParse(campos[0], out produtoId);
                        if (produtoId <= 0)
                        {
                            continue;
                        }

                        var produtoDAO = new ProdutoDAO();

                        string descricaoLinha = campos[1];
                        if (!string.IsNullOrEmpty(descricaoLinha))
                        {
                            produtoDAO.Linha = descricaoLinha.Trim();
                            var l = LinhaDAL.Listar(descricaoLinha.Trim(), string.Empty, usuarioSessao.SistemaID).Tables[0].Rows;
                            if (l != null && l.Count > 0)
                            {
                                produtoDAO.LinhaID = Convert.ToInt32(l[0]["LinhaID"]);
                            }
                        }

                        string descricaoProduto = campos[2];

                        string descricaoMedida = campos[3];
                        if (!string.IsNullOrEmpty(descricaoMedida))
                        {
                            produtoDAO.Medida = descricaoMedida.Trim();
                            var m = MedidaDAL.Listar(descricaoMedida.Trim(), usuarioSessao.SistemaID).Tables[0].Rows;
                            if (m != null && m.Count > 0)
                            {
                                produtoDAO.MedidaID = Convert.ToInt32(m[0]["MedidaID"]);
                            }
                        }

                        short comissaoFuncionario = 0;
                        short.TryParse(campos[4], out comissaoFuncionario);

                        short comissaoFranqueado = 0;
                        short.TryParse(campos[5], out comissaoFranqueado);

                        produtoDAO.ProdutoID = produtoId;
                        produtoDAO.Descricao = string.IsNullOrEmpty(descricaoProduto) ? string.Empty : descricaoProduto.Trim();
                        produtoDAO.Quantidade = 0;
                        produtoDAO.ComissaoFuncionario = comissaoFuncionario;
                        produtoDAO.ComissaoFranqueado = comissaoFranqueado;
                        produtoDAO.SistemaID = new BLL.Modelo.Usuario(Session["Usuario"]).SistemaID;
                        produtoDAO.LojaID = lojaId;

                        listaProduto.Add(produtoDAO);
                    }
                }

                if (listaProduto == null || listaProduto.Count() <= 0)
                {
                    throw new ApplicationException("Nenhum produto foi encontrado no arquivo de banco de dados");
                }

                produtoBLL.AtualizarViaArquivoBancoDados(listaProduto);

                CarregarRepeaterLoja();

                UtilitarioBLL.ExibirMensagemAjax(this.Page, "Arquivo de banco de dados importado com sucesso");
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
            }
        }

        protected void imbImportarEstoque_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                HttpPostedFile file = null;

                try
                {
                    file = Request.Files[2];
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Informe o arquivo de estoque");
                }

                if (file == null || file.ContentLength <= 0)
                    throw new ApplicationException("Informe o arquivo de estoque");

                if (!file.FileName.ToLower().Contains(".xlsx"))
                    throw new ApplicationException("Arquivo de estoque informado não está no formato XLSX");

                var workbook = new XLWorkbook(file.InputStream);
                var produtosDao = new List<ProdutoDAO>();
                ProdutoDAO produtoDao = null;
                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                var lojaDao = ((DataSet)Session["dsLoja"]).
                       Tables[0].
                       AsEnumerable().
                       Select(dataRow => new LojaDAO
                       {
                           LojaID = dataRow.Field<int>("LojaID"),
                           CNPJ = dataRow.Field<string>("Cnpj"),
                           RazaoSocial = dataRow.Field<string>("RazaoSocial"),
                           NomeFantasia = dataRow.Field<string>("NomeFantasia"),
                           Telefone = dataRow.Field<string>("Telefone"),
                           Cota = dataRow.Field<double>("Cota"),
                           SistemaID = usuarioSessao.SistemaID
                       }).FirstOrDefault(x => x.NomeFantasia.ToUpper() == "DEPÓSITO");

                foreach (var cellUsed in workbook.Worksheets.FirstOrDefault().Cells().Where(x => x.Address.ColumnLetter.ToUpper() == "A" || x.Address.ColumnLetter.ToUpper() == "C"))
                {
                    if (cellUsed.Address.ColumnLetter.ToUpper() == "A")
                    {
                        produtoDao = new ProdutoDAO();

                        long numero;
                        cellUsed.TryGetValue(out numero);

                        if (numero <= 0)
                            continue;

                        produtoDao.ProdutoID = numero;
                    }
                    else if (cellUsed.Address.ColumnLetter.ToUpper() == "C")
                    {
                        short quantidade;
                        cellUsed.TryGetValue(out quantidade);

                        if (produtoDao != null && produtoDao.ProdutoID > 0)
                        {
                            produtoDao.Quantidade = quantidade;
                            produtoDao.LojaID = lojaDao.LojaID;
                            produtoDao.SistemaID = usuarioSessao.SistemaID;

                            produtosDao.Add(produtoDao);
                        }
                    }
                }

                var listaRetorno = new ProdutoDAL().Atualizar(produtosDao);

                CarregarRepeaterLoja();

                var mensagem = string.Empty;

                mensagem += "Produtos atualizados:\r\n\r\n" + string.Join("\r\n", listaRetorno.Where(x => x.Contains("atualizado com sucesso"))) + "\r\n\r\n";
                mensagem += "Produtos não atualizados:\r\n\r\n" + string.Join("\r\n", listaRetorno.Where(x => x.Contains("não foi atualizado"))) + "\r\n";
                mensagem += "\r\nTotal de produtos importados: " + listaRetorno.Where(x => x.Contains("atualizado com sucesso")).Count().ToString();
                mensagem += "\r\nTotal de produtos não importados: " + listaRetorno.Where(x => x.Contains("não foi atualizado")).Count().ToString();

                UtilitarioBLL.ExibirMensagemAjax(Page, mensagem);
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
            }
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            lblTopo.Text = "CONSULTA - PRODUTO";
            if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode())
            {
                ckbProdutoID.Visible = true;
                lblProdutoID.Visible = true;
                txtProdutoID.Visible = true;
                imbCadastrar.Visible = false;
                imbAtualizar.Visible = false;
                imbExcluir.Visible = false;
                imbZerar.Visible = true;
                ckbProdutoID.Text = "Atualizar/Excluir/Cadastrar";
                btnAtualizarEstoque.Visible = true;
                btnRelatorioEstoque.Visible = true;
            }
            else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode())
            {
                ckbProdutoID.Visible = false;
                lblProdutoID.Visible = false;
                txtProdutoID.Visible = false;
                imbCadastrar.Visible = false;
                imbAtualizar.Visible = false;
                imbExcluir.Visible = false;
                imbZerar.Visible = false;
                ckbProdutoID.Text = "Cadastrar";
                lblComissaoFuncionario.Visible = false;
                txtComissaoFuncionario.Visible = false;
                lblComissaoFranqueado.Visible = false;
                txtComissaoFranqueado.Visible = false;
                btnAtualizarEstoque.Visible = false;
                btnRelatorioEstoque.Visible = false;
            }
            else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode())
            {
                ckbProdutoID.Visible = true;
                lblProdutoID.Visible = true;
                txtProdutoID.Visible = true;
                imbCadastrar.Visible = false;
                imbAtualizar.Visible = false;
                imbExcluir.Visible = false;
                imbZerar.Visible = false;
                ckbProdutoID.Text = "Cadastrar";
                btnAtualizarEstoque.Visible = false;
                btnRelatorioEstoque.Visible = false;
            }
            else
            {
                imbCadastrar.Visible = false;
                imbAtualizar.Visible = false;
                imbExcluir.Visible = false;
                imbZerar.Visible = false;
                ckbProdutoID.Visible = false;
                lblProdutoID.Visible = false;
                txtProdutoID.Visible = false;
                lblComissaoFuncionario.Visible = false;
                txtComissaoFuncionario.Visible = false;
                lblComissaoFranqueado.Visible = false;
                txtComissaoFranqueado.Visible = false;
                btnAtualizarEstoque.Visible = false;
                btnRelatorioEstoque.Visible = false;
            }
        }

        protected void btnAtualizarEstoque_Click(object sender, EventArgs e)
        {
            this.LimparMensagemVerificarCarga();
            this.mpeAtualizarEstoque.Show();
        }

        private void LimparMensagemVerificarCarga()
        {
            txtNumeroCarga.Text = string.Empty;
            lblMensagemCarga.Text = string.Empty;
            lblMensagemCarga.Visible = false;
        }

        protected void btnImportarCarga_Click(object sender, EventArgs e)
        {
            try
            {
                lblMensagemCarga.Visible = true;
                lblMensagemCarga.Text = string.Empty;
                lblMensagemCarga.ForeColor = Color.Red;
                this.mpeAtualizarEstoque.Show();

                HttpPostedFile file = null;

                try
                {
                    file = Request.Files["ctl00$ContentPlaceHolder1$fupImportarArquivoEstoque"];
                }
                catch (Exception ex)
                {
                    lblMensagemCarga.Text = "Informe o arquivo de estoque";
                    return;
                }

                if (file == null || file.ContentLength <= 0)
                {
                    lblMensagemCarga.Text = "Informe o arquivo de estoque";
                    return;
                }

                if (!file.FileName.ToLower().Contains(".xlsx"))
                {
                    lblMensagemCarga.Text = "Arquivo de estoque informado não está no formato XLSX";
                    return;
                }

                var workbook = new XLWorkbook(file.InputStream);
                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                var numeroCarga = txtNumeroCarga.Text.Trim();

                if (string.IsNullOrEmpty(numeroCarga))
                {
                    lblMensagemCarga.Text = "Número de carga é obrigatório";
                    return;
                }

                var cargaBLL = new CargaBLL();

                var carga = cargaBLL.Obter(new CargaDAO() { NumeroCarga = numeroCarga, SistemaID = usuarioSessao.SistemaID });

                if (carga != null && carga.CargaID > 0)
                {
                    lblMensagemCarga.Text = $"Carga {numeroCarga} já existe.";
                    return;
                }

                var cargaDao = new CargaDAO() { NumeroCarga = numeroCarga, DataCadastro = DateTime.Now, SistemaID = usuarioSessao.SistemaID };

                var lojaDao = ((DataSet)Session["dsLoja"]).Tables[0].AsEnumerable().Select(dataRow => new LojaDAO
                {
                    LojaID = dataRow.Field<int>("LojaID"),
                    CNPJ = dataRow.Field<string>("Cnpj"),
                    RazaoSocial = dataRow.Field<string>("RazaoSocial"),
                    NomeFantasia = dataRow.Field<string>("NomeFantasia"),
                    Telefone = dataRow.Field<string>("Telefone"),
                    Cota = dataRow.Field<double>("Cota"),
                    SistemaID = usuarioSessao.SistemaID
                }).FirstOrDefault(x => x.NomeFantasia.ToUpper() == "DEPÓSITO");

                foreach (var row in workbook.Worksheets.FirstOrDefault().Rows().Skip(1))
                {
                    var produtoDao = new ProdutoDAO();
                    var notaFiscalDAO = new NotaFiscalDAO() { LojaID = lojaDao.LojaID, SistemaID = usuarioSessao.SistemaID, Estoque = 1, PedidoMaeID = "" };

                    DateTime dataNF;
                    row.Cell("A").TryGetValue(out dataNF);
                    if (dataNF == DateTime.MinValue)
                        continue;
                    notaFiscalDAO.DataNotaFiscal = dataNF;

                    long produtoId;
                    row.Cell("C").TryGetValue(out produtoId);
                    if (produtoId <= 0)
                        continue;
                    produtoDao.ProdutoID = produtoId;

                    short quantidade;
                    row.Cell("E").TryGetValue(out quantidade);
                    if (quantidade > 0)
                        produtoDao.Quantidade = quantidade;

                    string numeroNotaFiscal;
                    row.Cell("J").TryGetValue(out numeroNotaFiscal);

                    if (numeroNotaFiscal.Contains(",")) // múltiplas nfs
                    {
                        var nfs = numeroNotaFiscal.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        foreach (var nf in nfs)
                        {
                            var notaFiscalAux = new NotaFiscalDAO() { LojaID = lojaDao.LojaID, SistemaID = usuarioSessao.SistemaID, Estoque = 1, PedidoMaeID = "", DataNotaFiscal = notaFiscalDAO.DataNotaFiscal };
                            var produtoAux = new ProdutoDAO() { ProdutoID = produtoDao.ProdutoID, Quantidade = produtoDao.Quantidade };

                            int numeroNF;
                            int.TryParse(nf, out numeroNF);
                            if (numeroNF <= 0)
                                continue;
                            notaFiscalAux.NotaFiscalID = numeroNF;

                            if (nfs.ToList().IndexOf(nf) > 0) // se não for a 1ª nf
                                produtoAux.Quantidade = 0;

                            notaFiscalAux.Produto = produtoAux;

                            cargaDao.NotaFiscalDao.Add(notaFiscalAux);
                        }
                    }
                    else // única nf
                    {
                        int numeroNF;
                        row.Cell("J").TryGetValue(out numeroNF);
                        if (numeroNF <= 0)
                            continue;
                        notaFiscalDAO.NotaFiscalID = numeroNF;

                        notaFiscalDAO.Produto = produtoDao;

                        cargaDao.NotaFiscalDao.Add(notaFiscalDAO);
                    }
                }

                var listaRetorno = cargaBLL.Incluir(cargaDao);

                CarregarRepeaterLoja();

                var mensagem = string.Empty;

                mensagem += "Produtos atualizados:\r\n\r\n" + string.Join("\r\n", listaRetorno.Where(x => x.Contains("atualizado com sucesso"))) + "\r\n\r\n";
                mensagem += "Produtos não atualizados:\r\n\r\n" + string.Join("\r\n", listaRetorno.Where(x => x.Contains("não foi atualizado"))) + "\r\n";
                mensagem += "\r\nTotal de produtos importados: " + listaRetorno.Where(x => x.Contains("atualizado com sucesso")).Count().ToString();
                mensagem += "\r\nTotal de produtos não importados: " + listaRetorno.Where(x => x.Contains("não foi atualizado")).Count().ToString();

                this.LimparMensagemVerificarCarga();

                this.mpeAtualizarEstoque.Hide();

                UtilitarioBLL.ExibirMensagemAjax(Page, mensagem);
            }
            catch (ApplicationException ex)
            {
                lblMensagemCarga.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblMensagemCarga.Text = ex.Message;
            }
        }

        protected void btnVerificarCarga_Click(object sender, EventArgs e)
        {
            try
            {
                this.mpeAtualizarEstoque.Show();
                lblMensagemCarga.Visible = true;
                lblMensagemCarga.Text = string.Empty;

                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                var cargaBLL = new CargaBLL();

                var numeroCarga = txtNumeroCarga.Text.Trim();

                if (string.IsNullOrEmpty(numeroCarga))
                {
                    lblMensagemCarga.Text = "Número de Carga é obrigatório";
                    lblMensagemCarga.ForeColor = Color.Orange;
                    return;
                }

                var cargaDAO = cargaBLL.Obter(new CargaDAO() { NumeroCarga = numeroCarga, SistemaID = usuarioSessao.SistemaID });

                var mensagem = string.Empty;

                if (cargaDAO != null && cargaDAO.CargaID > 0)
                {
                    mensagem = $"Carga {numeroCarga} já existe.";
                    lblMensagemCarga.ForeColor = Color.Red;
                }
                else
                {
                    mensagem = $"Carga {numeroCarga} não existe.";
                    lblMensagemCarga.ForeColor = Color.Green;
                }

                lblMensagemCarga.Text = mensagem;
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
        }

        protected void btnRelatorioEstoque_Click(object sender, EventArgs e)
        {
            try
            {
                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                var dataTable = RelatorioDAL.ListarRelatorioEstoque(usuarioSessao.SistemaID);

                using (var wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dataTable);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment;filename=relatorio-estoque-deposito-" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");

                    using (var stream = new MemoryStream())
                    {
                        wb.Worksheet(1).Column(1).CellsUsed().SetDataType(XLCellValues.Text);
                        wb.SaveAs(stream);
                        stream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
        }
    }
}