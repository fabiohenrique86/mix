using BLL;
using DAL;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site
{
    public partial class NotaFiscal : Page
    {
        private bool CarregarDados(bool filtro)
        {
            ViewState["filtroNotaFiscal"] = true;
            return CarregarRepeaterNotaFiscal();
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            ddlLoja.DataSource = new LojaDAL().ListarSemFabrica(usuarioSessao.SistemaID);
            ddlLoja.DataBind();
        }

        private bool CarregarRepeaterNotaFiscal()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            int lojaId = 0;
            int.TryParse(ddlLoja.SelectedValue, out lojaId);

            if (lojaId <= 0)
            {
                lojaId = usuarioSessao.LojaID;
            }

            rptLoja.DataSource = new LojaDAL().ListarById(lojaId.ToString(), usuarioSessao.SistemaID);
            rptLoja.DataBind();

            return (rptLoja.Items.Count > 0);
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
                    List<NotaFiscalDAO> listaNotaFiscal = new List<NotaFiscalDAO>();
                    ProdutoDAL produtoDAL = new ProdutoDAL();

                    if (((string.IsNullOrEmpty(txtNotaFiscalID.Text) || string.IsNullOrEmpty(txtPedidoMaeID.Text)) || (!(txtPedidoMaeID.Text != "0") || (ddlLoja.SelectedIndex <= 0))) || (string.IsNullOrEmpty(txtDataNotaFiscal.Text) || !(txtDataNotaFiscal.Text != "__/__/____")))
                    {
                        throw new ApplicationException("É necessário informar todos os campos para cadastrar.");
                    }
                    int notaFiscalId = Convert.ToInt32(txtNotaFiscalID.Text);
                    if (new NotaFiscalDAL().Listar(new NotaFiscalDAO() { NotaFiscalID = notaFiscalId, PedidoMaeID = txtPedidoMaeID.Text, SistemaID = usuarioSessao.SistemaID }))
                    {
                        throw new ApplicationException("Nota Fiscal ou Pedido Mãe cadastrado.\r\n\r\nInforme outra NotaFiscalID ou PedidoMãeID a ser cadastrado.");
                    }

                    for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
                    {
                        if (Request.Form["txtProdutoID_" + i] != null && Request.Form["txtQuantidade_" + i] != null)
                        {
                            string produtoId = Request.Form.GetValues("txtProdutoID_" + i).FirstOrDefault();
                            string quantidade = Request.Form.GetValues("txtQuantidade_" + i).FirstOrDefault();

                            if (string.IsNullOrEmpty(produtoId))
                            {
                                throw new ApplicationException(string.Format("Informe o produto {0}.", produtoId));
                            }

                            if ((string.IsNullOrEmpty(quantidade) || !(quantidade != "0")) || (!(quantidade != "00") || !(quantidade != "000")))
                            {
                                throw new ApplicationException(string.Format("Informe a quantidade do produto {0}.", produtoId));
                            }

                            if (!produtoDAL.ExisteNaLoja(produtoId, ddlLoja.SelectedValue, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException(string.Format("Produto {0} não está cadastrado na loja {1}.", produtoId, ddlLoja.SelectedItem.Text));
                            }

                            listaNotaFiscal.Add(new NotaFiscalDAO(Convert.ToInt32(notaFiscalId), txtPedidoMaeID.Text, Convert.ToInt32(ddlLoja.SelectedValue), Convert.ToInt64(produtoId), Convert.ToInt16(quantidade), Convert.ToDateTime(txtDataNotaFiscal.Text).Date, usuarioSessao.SistemaID));
                        }
                    }

                    if (listaNotaFiscal.Count <= 0)
                    {
                        throw new ApplicationException("Selecione ao menos um produto para cadastrar a nota fiscal.");
                    }

                    new NotaFiscalDAL().Inserir(listaNotaFiscal);

                    LimparFormulario(txtNotaFiscalID, txtPedidoMaeID, ddlLoja, txtDataNotaFiscal);

                    CarregarRepeaterNotaFiscal();
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida.");
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Source.ToUpper() == "SYSTEM.DATA")
                {
                    UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida.");
                }
                else
                {
                    UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
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
                    if (((string.IsNullOrEmpty(txtNotaFiscalID.Text) && (ddlLoja.SelectedIndex <= 0)) && (string.IsNullOrEmpty(txtDataNotaFiscal.Text) || (txtDataNotaFiscal.Text == "__/__/____"))) && string.IsNullOrEmpty(txtPedidoMaeID.Text))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }
                    if (!CarregarDados(true))
                    {
                        throw new ApplicationException("Nota Fiscal inexistente.");
                    }
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida.");
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Source.ToUpper() == "SYSTEM.DATA")
                {
                    UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida.");
                }
                else
                {
                    UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
                }
            }
        }

        private void LimparFormulario(TextBox txtNotaFiscalID, TextBox txtPedidoMae, DropDownList ddlLoja, TextBox txtDataNotaFiscal)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            txtNotaFiscalID.Text = string.Empty;
            txtPedidoMae.Text = string.Empty;
            if (
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()) ||
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode()) ||
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode())
                )
            {
                ddlLoja.SelectedIndex = 0;
                ddlLoja.Enabled = true;
            }
            else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode())
            {
                ddlLoja.SelectedValue = usuarioSessao.LojaID.ToString();
                ddlLoja.Enabled = false;
            }
            txtDataNotaFiscal.Text = string.Empty;
            ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "LimparGrid", "document.getElementById('ctl00_ContentPlaceHolder1_hdfProdutoValores').value = ''; clearGrid();", true);
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
                    else if (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == UtilitarioBLL.TipoUsuario.Vendedor.GetHashCode())
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
                        CarregarDropDownListLoja();
                        CarregarRepeaterNotaFiscal();
                        VisualizarFormulario();
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

        protected void rptLoja_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    int lojaId = 0;
                    int.TryParse(ddlLoja.SelectedValue, out lojaId);

                    if (lojaId <= 0)
                    {
                        lojaId = Convert.ToInt32(((Label)e.Item.FindControl("lblLojaID")).Text);
                    }

                    ((Label)e.Item.FindControl("lblNomeFantasia")).Text = ((Label)e.Item.FindControl("lblNomeFantasia")).Text.ToUpper();
                    Repeater rptData = (Repeater)e.Item.FindControl("rptData");

                    string dataNotaFiscal = string.Empty;

                    if (!string.IsNullOrEmpty(txtDataNotaFiscal.Text) && txtDataNotaFiscal.Text != "__/__/____")
                    {
                        dataNotaFiscal = txtDataNotaFiscal.Text.Trim();
                    }

                    rptData.DataSource = new NotaFiscalDAL().ListarData(lojaId, dataNotaFiscal, txtNotaFiscalID.Text, txtPedidoMaeID.Text, usuarioSessao.SistemaID);
                    rptData.DataBind();

                    if (rptData.Items.Count <= 0)
                    {
                        ((Label)e.Item.FindControl("lblNomeFantasia")).Visible = false;
                        rptData.Visible = false;
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

        protected void rptData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    int lojaId = 0;
                    int.TryParse(ddlLoja.SelectedValue, out lojaId);

                    if (lojaId <= 0)
                    {
                        lojaId = Convert.ToInt32(((Label)e.Item.Parent.Parent.FindControl("lblLojaID")).Text);
                    }

                    string dataNotaFiscal = string.Empty;

                    if (string.IsNullOrEmpty(txtDataNotaFiscal.Text) || txtDataNotaFiscal.Text == "__/__/____")
                    {
                        dataNotaFiscal = ((Label)e.Item.FindControl("lblDataNotaFiscal")).Text;
                    }
                    else
                    {
                        dataNotaFiscal = txtDataNotaFiscal.Text.Trim();
                    }

                    Repeater rptNotaFiscal = (Repeater)e.Item.FindControl("rptNotaFiscal");

                    if (ViewState["filtroNotaFiscal"] != null)
                    {
                        rptNotaFiscal.DataSource = new NotaFiscalDAL().Listar(txtNotaFiscalID.Text, txtPedidoMaeID.Text, lojaId.ToString(), dataNotaFiscal, usuarioSessao.SistemaID, false);
                    }
                    else
                    {
                        rptNotaFiscal.DataSource = new NotaFiscalDAL().Listar(txtNotaFiscalID.Text, txtPedidoMaeID.Text, lojaId.ToString(), dataNotaFiscal, usuarioSessao.SistemaID, true);
                    }

                    rptNotaFiscal.DataBind();

                    if (rptNotaFiscal.Items.Count <= 0)
                    {
                        rptNotaFiscal.Visible = false;
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

        protected void rptNotaFiscal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    int notaFiscalId = Convert.ToInt32(((Label)e.Item.FindControl("lblNotaFiscalID")).Text);
                    GridView gdvNotaFiscalProduto = (GridView)e.Item.FindControl("gdvNotaFiscalProduto");

                    gdvNotaFiscalProduto.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);

                    gdvNotaFiscalProduto.DataSource = new NotaFiscalDAL().ListarProduto(notaFiscalId, usuarioSessao.SistemaID);
                    gdvNotaFiscalProduto.DataBind();

                    if (gdvNotaFiscalProduto.Rows.Count <= 0)
                    {
                        gdvNotaFiscalProduto.Visible = false;
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

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            if (
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()) ||
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode())
                )
            {
                ddlLoja.Enabled = true;
                imbCadastrar.Visible = true;
            }
            else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode())
            {
                ddlLoja.Enabled = true;
                imbCadastrar.Visible = false;
            }
            else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode())
            {
                ddlLoja.Enabled = false;
                ddlLoja.SelectedValue = usuarioSessao.LojaID.ToString();
                imbCadastrar.Visible = true;
            }
        }

        protected void imbImportar_Click(object sender, ImageClickEventArgs e)
        {
            var files = Request.Files;
            string mensagem = string.Empty;

            try
            {
                var notaFiscalDAO = new NotaFiscalDAO();
                var notaFiscalBLL = new NotaFiscalBLL();

                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    var notaFiscalXML = new NotaFiscalXML();

                    notaFiscalXML.ContentType = file.ContentType;
                    notaFiscalXML.InputStream = file.InputStream;

                    notaFiscalDAO.NotasFiscaisXML.Add(notaFiscalXML);
                }

                notaFiscalDAO.Estoque = 1;
                notaFiscalDAO.SistemaID = new BLL.Modelo.Usuario(Session["Usuario"]).SistemaID;

                var validarPedidoMae = notaFiscalDAO.SistemaID == 2 ? true : false; // Paulo = Validar, Alex = Não Validar

                var mensagens = notaFiscalBLL.Importar(notaFiscalDAO, validarPedidoMae, true);

                mensagem = string.Join(@"\r\n", mensagens.ToArray());

                CarregarRepeaterNotaFiscal();

                if (mensagens != null && mensagens.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ImportarXML", "alert('" + mensagem + "');", true);
                }
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ImportarXML", "alert('" + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ImportarXML", "alert('Ocorreu um erro ao tentar o arquivo XML: " + ex.Message + "');", true);
            }
        }
    }
}