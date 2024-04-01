using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DAO;
using BLL;
using Newtonsoft.Json;

namespace Site
{
    public partial class Transferencia : Page
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
                        VisualizarFormulario();
                        CarregarDropDownListLoja();
                        CarregarRepeaterTransferencia(true);
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

        private bool CarregarDados(int transferenciaId, int lojaDe, int lojaPara, DateTime dataTransferencia, int valida)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            rptTransferencia.DataSource = new DAL.TransferenciaDAL().Listar(transferenciaId, lojaDe, lojaPara, dataTransferencia, usuarioSessao.SistemaID, valida);
            rptTransferencia.DataBind();

            return (rptTransferencia.Items.Count > 0);
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            if ((Session["dsDropDownListLoja"] == null) || (Session["bdLoja"] != null))
            {
                Session["dsDropDownListLoja"] = new DAL.LojaDAL().ListarDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
                Session["bdLoja"] = null;
            }

            ddlLojaDe.DataSource = Session["dsDropDownListLoja"];            
            ddlLojaDe.DataBind();
            
            ddlLojaPara.DataSource = new DAL.LojaDAL().ListarDropDownList(0, usuarioSessao.SistemaID);
            ddlLojaPara.DataBind();
        }

        private bool CarregarRepeaterTransferencia(bool valida)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            rptTransferencia.DataSource = new DAL.TransferenciaDAL().Listar(usuarioSessao.SistemaID, valida);
            rptTransferencia.DataBind();
            return (rptTransferencia.Items.Count > 0);
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
                    int transferenciaId = 0;
                    int lojaDeId = 0;
                    int lojaParaId = 0;
                    DateTime dataTransferencia = DateTime.MinValue;
                    int valida = -1;

                    if (!string.IsNullOrEmpty(txtTransferenciaId.Text))
                    {
                        transferenciaId = Convert.ToInt32(txtTransferenciaId.Text);
                    }

                    if (ddlLojaDe.SelectedIndex > 0)
                    {
                        lojaDeId = Convert.ToInt32(ddlLojaDe.SelectedIndex);
                    }

                    if (ddlLojaPara.SelectedIndex > 0)
                    {
                        lojaParaId = Convert.ToInt32(ddlLojaPara.SelectedIndex);
                    }

                    if (!string.IsNullOrEmpty(txtDataTransferencia.Text) && txtDataTransferencia.Text != "__/__/____")
                    {
                        dataTransferencia = Convert.ToDateTime(txtDataTransferencia.Text);
                    }

                    valida = Convert.ToInt32(rblValida.SelectedValue);

                    if (transferenciaId == 0 && lojaDeId == 0 && lojaParaId == 0 && dataTransferencia == DateTime.MinValue && valida < 0)
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }

                    Session["Filtro_Transferencia"] = true;

                    if (!CarregarDados(transferenciaId, lojaDeId, lojaParaId, dataTransferencia, valida))
                    {
                        throw new ApplicationException("Transferência inexistente.");
                    }
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida");
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

        protected void imbTransferir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Transferir(true);
                CarregarRepeaterTransferencia(true);
                LimparFormulario();
                ScriptManager.RegisterStartupScript(this, GetType(), "LimparGrid", "document.getElementById('ctl00_ContentPlaceHolder1_hdfProdutoValores').value = ''; clearGrid();", true);
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida");
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

        protected void imbGerarComanda_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var transferenciaDAO = Transferir(false);
                ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "LimparGrid", "ImprimirComandaTransferencia(" + JsonConvert.SerializeObject(transferenciaDAO) + ");", true);
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida");
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

        private TransferenciaDAO Transferir(bool transferenciaValida)
        {
            TransferenciaDAO transferenciaDAO = new TransferenciaDAO();
            
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

                return transferenciaDAO;
            }
            else
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                DAL.ProdutoDAL produtoDAL = new DAL.ProdutoDAL();
                TransferenciaBLL transferenciaBLL = new TransferenciaBLL();
                transferenciaDAO.ListaProduto = new List<ProdutoDAO>();

                if (((ddlLojaDe.SelectedIndex <= 0) || (ddlLojaPara.SelectedIndex <= 0)) || (string.IsNullOrEmpty(txtDataTransferencia.Text) || !(txtDataTransferencia.Text != "__/__/____")))
                {
                    if (transferenciaValida)
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para transferir.");
                    }
                    else
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para gerar comanda.");
                    }
                }

                if (!string.IsNullOrEmpty(txtTransferenciaId.Text))
                {
                    if (transferenciaValida)
                    {
                        throw new ApplicationException("Não é necessário informar o campo TransferenciaId para transferir.");
                    }
                    else
                    {
                        throw new ApplicationException("Não é necessário informar o campo TransferenciaId para gerar comanda.");
                    }
                }

                if (!(Convert.ToDateTime(txtDataTransferencia.Text) == DateTime.Today))
                {
                    throw new ApplicationException("A data da transferência deve ser a data de hoje.");
                }

                if (!(ddlLojaDe.SelectedValue != ddlLojaPara.SelectedValue))
                {
                    if (transferenciaValida)
                    {
                        throw new ApplicationException("É necessário informar lojas diferentes para finalizar a transferência.");
                    }
                    else
                    {
                        throw new ApplicationException("É necessário informar lojas diferentes para gerar a comanda.");
                    }
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

                        if (!produtoDAL.ExisteNaLoja(produtoId, ddlLojaDe.SelectedValue, usuarioSessao.SistemaID))
                        {
                            throw new ApplicationException(string.Format("Produto {0} inexistente na loja de origem {1}.", produtoId, ddlLojaDe.SelectedItem.Text));
                        }

                        if (!produtoDAL.ExisteNaLoja(produtoId, ddlLojaPara.SelectedValue, usuarioSessao.SistemaID))
                        {
                            throw new ApplicationException(string.Format("Produto {0} inexistente na loja de destino {1}.", produtoId, ddlLojaPara.SelectedItem.Text));
                        }

                        var produtoDAO = new ProdutoDAO();

                        produtoDAO.ProdutoID = Convert.ToInt64(produtoId);
                        produtoDAO.Quantidade = Convert.ToInt16(quantidade);
                        produtoDAO.LojaDeID = Convert.ToInt32(ddlLojaDe.SelectedValue);
                        produtoDAO.LojaParaID = Convert.ToInt32(ddlLojaPara.SelectedValue);
                        produtoDAO.SistemaID = usuarioSessao.SistemaID;

                        var produto = produtoDAL.Listar(produtoDAO.LojaDeID.ToString(), null, null, null, null, null, usuarioSessao.SistemaID, produtoDAO.ProdutoID).Tables[0];
                        if (produto != null)
                        {
                            produtoDAO.Descricao = produto.Rows[0]["Descricao"].ToString();
                            produtoDAO.Medida = produto.Rows[0]["Medida"].ToString();
                        }                        

                        transferenciaDAO.ListaProduto.Add(produtoDAO);
                    }
                }

                if (transferenciaDAO.ListaProduto.Count <= 0)
                {
                    if (transferenciaValida)
                    {
                        throw new ApplicationException("Selecione ao menos um produto para efetuar a transferência.");
                    }
                    else
                    {
                        throw new ApplicationException("Selecione ao menos um produto para gerar comanda.");
                    }
                }

                transferenciaDAO.LojaDeID = Convert.ToInt32(ddlLojaDe.SelectedValue);
                transferenciaDAO.LojaParaID = Convert.ToInt32(ddlLojaPara.SelectedValue);
                transferenciaDAO.DataTransferencia = DateTime.Now.Date;
                transferenciaDAO.SistemaID = usuarioSessao.SistemaID;
                transferenciaDAO.Valida = transferenciaValida;

                transferenciaDAO.TransferenciaID = transferenciaBLL.Inserir(transferenciaDAO);
            }

            return transferenciaDAO;
        }

        private void LimparFormulario()
        {
            ddlLojaDe.SelectedIndex = 0;
            ddlLojaPara.SelectedIndex = 0;
            txtDataTransferencia.Text = string.Empty;
        }

        protected void rptTransferencia_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                bool? valida = null;
                if (Session["Filtro_Transferencia"] != null)
                {
                    valida = rblValida.SelectedValue == "0" ? false : true;
                }

                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    int tranferenciaId = Convert.ToInt32(((Label)e.Item.FindControl("lblTransferenciaID")).Text);
                    var repeaterTransferencia = (HtmlContainerControl)e.Item.FindControl("repeaterTransferencia");
                    var comandaValidada = Convert.ToBoolean(((Label)e.Item.FindControl("lblValida")).Text);

                    if (comandaValidada)
                    {
                        repeaterTransferencia.Style.Add("background-color", BLL.UtilitarioBLL.COR_STATUS_OK);
                    }
                    else
                    {
                        repeaterTransferencia.Style.Add("background-color", BLL.UtilitarioBLL.COR_STATUS_PENDENTE);
                    }

                    GridView gdvProduto = (GridView)e.Item.FindControl("gdvProduto");

                    gdvProduto.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
                    gdvProduto.DataSource = new DAL.TransferenciaDAL().ListarProduto(tranferenciaId, usuarioSessao.SistemaID, valida);
                    gdvProduto.DataBind();

                    if (gdvProduto.Rows.Count <= 0)
                    {
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
            finally
            {
                Session["Filtro_Transferencia"] = null;
            }
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            trComanda.Visible = false;

            if ((usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()) ||
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode()) ||
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode()))
            {
                imbGerarComanda.Visible = true;
                imbTransferir.Visible = true;

                if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode())
                {
                    trComanda.Visible = true;
                }
            }
            else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode())
            {
                imbGerarComanda.Visible = true;
                imbTransferir.Visible = false;
            }

            if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Vendedor.GetHashCode())
            {
                imbValidar.Visible = false;
            }
        }

        protected void imbValidar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                TransferenciaDAO transferenciaDAO = new TransferenciaDAO();
                TransferenciaBLL transferenciaBLL = new TransferenciaBLL();

                if (!string.IsNullOrEmpty(txtTransferenciaId.Text))
                {
                    transferenciaDAO.TransferenciaID = Convert.ToInt32(txtTransferenciaId.Text);
                }

                transferenciaDAO.SistemaID = usuarioSessao.SistemaID;

                transferenciaBLL.ValidarComandaTransferencia(transferenciaDAO);

                txtTransferenciaId.Text = string.Empty;

                CarregarRepeaterTransferencia(true);
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

        protected void imbExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                TransferenciaDAO transferenciaDAO = new TransferenciaDAO();
                TransferenciaBLL transferenciaBLL = new TransferenciaBLL();

                int transferenciaId = 0;
                int.TryParse(txtTransferenciaId.Text, out transferenciaId);
                transferenciaDAO.TransferenciaID = transferenciaId;
                transferenciaDAO.SistemaID = usuarioSessao.SistemaID;

                transferenciaBLL.Excluir(transferenciaDAO);

                txtTransferenciaId.Text = string.Empty;

                CarregarRepeaterTransferencia(true);
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

        protected void imbReabrir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                TransferenciaDAO transferenciaDAO = new TransferenciaDAO();
                TransferenciaBLL transferenciaBLL = new TransferenciaBLL();

                int transferenciaId = 0;
                int.TryParse(txtTransferenciaId.Text, out transferenciaId);
                transferenciaDAO.TransferenciaID = transferenciaId;
                transferenciaDAO.Valida = false;
                transferenciaDAO.SistemaID = usuarioSessao.SistemaID;

                transferenciaBLL.Reabrir(transferenciaDAO);

                txtTransferenciaId.Text = string.Empty;

                CarregarRepeaterTransferencia(true);
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
    }
}