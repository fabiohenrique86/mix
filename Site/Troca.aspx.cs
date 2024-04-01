using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;
using BLL;
using DAO;
using DAL;

namespace Site
{
    public partial class Troca : Page
    {
        private TrocaBLL trocaBLL;
        private TrocaProdutoBLL trocaProdutoBLL;

        public Troca()
        {
            trocaBLL = new TrocaBLL();
            trocaProdutoBLL = new TrocaProdutoBLL();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!UtilitarioBLL.PermissaoUsuario(Session["Usuario"]))
                    {
                        UtilitarioBLL.Sair();
                        if (Request.Url.Segments.Length == 3)
                        {
                            Response.Redirect("../Default.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("Default.aspx", false);
                        }
                    }
                    else
                    {
                        CarregarDropDownListLoja();
                        CarregarRepeaterReserva();
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

        private bool CarregarDados(TrocaDAO trocaDAO)
        {
            rptTroca.DataSource = trocaBLL.Listar(trocaDAO);
            rptTroca.DataBind();

            return (rptTroca.Items.Count > 0);
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            if ((Session["dsDropDownListLoja"] == null) || (Session["bdLoja"] != null))
            {
                Session["dsDropDownListLoja"] = new LojaDAL().ListarDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
                Session["bdLoja"] = null;
            }

            ddlLoja.DataSource = Session["dsDropDownListLoja"];
            ddlLoja.DataBind();
        }

        private void CarregarRepeaterReserva()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            rptTroca.DataSource = trocaBLL.Listar(new TrocaDAO() { SistemaID = usuarioSessao.SistemaID, LojaID = usuarioSessao.LojaID });
            rptTroca.DataBind();
        }

        protected void imbConsultar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    AplicacaoBLL.Empresa = null;

                    if (Request.Url.Segments.Length == 3)
                    {
                        Response.Redirect("../Default.aspx", true);
                    }
                    else
                    {
                        Response.Redirect("Default.aspx", true);
                    }
                }
                else
                {
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                    var trocaDAO = new TrocaDAO();

                    int trocaId = 0;
                    int.TryParse(txtTrocaID.Text, out trocaId);

                    int lojaId = usuarioSessao.LojaID;
                    if (ddlLoja.SelectedValue != "0")
                    {
                        int.TryParse(ddlLoja.SelectedValue, out lojaId);
                    }

                    string cpfFormatado = string.IsNullOrEmpty(txtCpf.Text.Trim().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCpf.Text.Trim().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                    string cnpjFormatado = string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")) ? null : txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                    int sistemaId = usuarioSessao.SistemaID;

                    trocaDAO.TrocaID = trocaId;
                    trocaDAO.LojaID = lojaId;
                    trocaDAO.CPF = cpfFormatado;
                    trocaDAO.CNPJ = cnpjFormatado;
                    trocaDAO.SistemaID = sistemaId;
                    trocaDAO.Status = ddlStatus.SelectedValue;
                    trocaDAO.Svt = txtSVT.Text;

                    DateTime dtEntrega;
                    DateTime.TryParse(txtDataEntrega.Text, out dtEntrega);
                    trocaDAO.DataEntrega = dtEntrega;

                    if (!CarregarDados(trocaDAO))
                    {
                        throw new ApplicationException("Troca inexistente.");
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

        protected void imbInserirTroca_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                BLL.AplicacaoBLL.Empresa = null;

                if (Request.Url.Segments.Length == 3)
                {
                    Response.Redirect("../Default.aspx", true);
                }
                else
                {
                    Response.Redirect("Default.aspx", true);
                }
            }
            else
            {
                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                var trocaDAO = new TrocaDAO();

                int lojaId = 0;
                int.TryParse(ddlLoja.SelectedValue, out lojaId);
                string cpfFormatado = string.IsNullOrEmpty(txtCpf.Text.Trim().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCpf.Text.Trim().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                string cnpjFormatado = string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")) ? null : txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                int sistemaId = usuarioSessao.SistemaID;

                try
                {
                    if (!string.IsNullOrEmpty(cpfFormatado))
                    {
                        if (!new UtilsBLL().ValidarCpf(cpfFormatado))
                        {
                            throw new ApplicationException("CPF inválido.");
                        }
                    }
                    else if (!string.IsNullOrEmpty(cnpjFormatado))
                    {
                        if (!new UtilsBLL().ValidarCnpj(cnpjFormatado))
                        {
                            throw new ApplicationException("CNPJ inválido.");
                        }
                    }

                    for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
                    {
                        if (Request.Form["txtProdutoID_" + i] != null && Request.Form["txtQuantidade_" + i] != null)
                        {
                            string produtoId = Request.Form.GetValues("txtProdutoID_" + i).FirstOrDefault();
                            string quantidade = Request.Form.GetValues("txtQuantidade_" + i).FirstOrDefault();
                            string sobMedida = Request.Form.GetValues("txtSobMedida_" + i).FirstOrDefault();

                            if (string.IsNullOrEmpty(produtoId))
                            {
                                throw new ApplicationException(string.Format("Informe o produto {0}.", produtoId));
                            }

                            if ((string.IsNullOrEmpty(quantidade) || !(quantidade != "0")) || (!(quantidade != "00") || !(quantidade != "000")))
                            {
                                throw new ApplicationException(string.Format("Informe a quantidade do produto {0}.", produtoId));
                            }

                            if (lojaId > 0)
                            {
                                var nomeFantasia = new LojaDAL().ListarById(lojaId.ToString(), sistemaId).Tables[0].Rows[0]["NomeFantasia"].ToString();

                                if (!new ProdutoDAL().ExisteNaLoja(produtoId, lojaId.ToString(), sistemaId))
                                {
                                    throw new ApplicationException(string.Format("Produto {0} não está cadastrado na loja {1}.", produtoId, nomeFantasia));
                                }
                            }

                            trocaDAO.TrocaProdutoDAO.Add(new TrocaProdutoDAO()
                            {
                                ProdutoDAO = new ProdutoDAO()
                                {
                                    ProdutoID = Convert.ToInt64(produtoId),
                                    Quantidade = Convert.ToInt16(quantidade),
                                    Medida = sobMedida,
                                    SistemaID = sistemaId,
                                    LojaID = lojaId
                                }
                            });
                        }
                    }

                    trocaDAO.LojaID = lojaId;
                    trocaDAO.CPF = cpfFormatado;
                    trocaDAO.CNPJ = cnpjFormatado;
                    trocaDAO.DataTroca = DateTime.Today;
                    trocaDAO.SistemaID = sistemaId;
                    trocaDAO.Ativo = true;

                    DateTime dtEntrega;
                    DateTime.TryParse(txtDataEntrega.Text, out dtEntrega);
                    trocaDAO.DataEntrega = dtEntrega;

                    trocaDAO.Observacao = txtObservacao.Text;
                    trocaDAO.Svt = txtSVT.Text;

                    trocaBLL.Incluir(trocaDAO);

                    CarregarRepeaterReserva();
                    LimparFormulario();
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
        }

        protected void imbDarBaixa_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                var checkboxSelecionado = false;
                var mensagem = string.Empty;
                var mensagensErro = new List<string>();
                var mensagensSucesso = new List<string>();

                foreach (RepeaterItem item in rptTroca.Items)
                {
                    try
                    {
                        var cbCheck = (CheckBox)item.FindControl("cbCheck");
                        if (cbCheck.Checked)
                        {
                            checkboxSelecionado = true;

                            var trocaDAO = new TrocaDAO()
                            {
                                TrocaID = Convert.ToInt32(((CheckBox)item.FindControl("cbCheck")).ToolTip),
                                SistemaID = usuarioSessao.SistemaID
                            };

                            trocaBLL.DarBaixa(trocaDAO);

                            mensagensSucesso.Add(string.Format("Troca {0} baixada com sucesso", trocaDAO.TrocaID));
                        }
                    }
                    catch (Exception ex)
                    {
                        mensagensErro.Add(ex.Message);
                    }
                }

                if (!checkboxSelecionado)
                {
                    throw new ApplicationException("Selecione ao menos uma troca para dar baixa");
                }

                CarregarRepeaterReserva();
                LimparFormulario();
                
                mensagem += "Baixas efetuadas com sucesso: \r\n\r\n" + string.Join("\r\n", mensagensSucesso);
                mensagem += "\r\n\r\nBaixas não efetuadas com erro: \r\n\r\n" + string.Join("\r\n", mensagensErro);

                UtilitarioBLL.ExibirMensagemAjax(Page, mensagem);
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
        }

        private void LimparFormulario()
        {
            var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            txtTrocaID.Text = string.Empty;
            txtSVT.Text = string.Empty;
            txtCpf.Text = string.Empty;
            txtCnpj.Text = string.Empty;

            if (usuarioSessao.SistemaID != 2)
            {
                ddlLoja.SelectedIndex = 0;
            }

            txtDataEntrega.Text = string.Empty;
            txtObservacao.Text = string.Empty;
            ddlStatus.SelectedIndex = 0;

            ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "LimparGrid", "document.getElementById('ctl00_ContentPlaceHolder1_hdfProdutoValores').value = ''; clearGrid();", true);
        }

        protected void rptTroca_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    var trocaId = Convert.ToInt32(((LinkButton)e.Item.FindControl("lkbTrocaID")).Text);
                    var repeaterTroca = (HtmlContainerControl)e.Item.FindControl("repeaterTroca");
                    var ativo = ((Label)e.Item.FindControl("lblAtivo")).Text;
                    var gdvTrocaProduto = (GridView)e.Item.FindControl("gdvTrocaProduto");

                    if (ativo == "True")
                    {
                        repeaterTroca.Style.Add("background-color", UtilitarioBLL.COR_STATUS_PENDENTE);
                    }
                    else
                    {
                        repeaterTroca.Style.Add("background-color", UtilitarioBLL.COR_STATUS_OK);
                    }

                    gdvTrocaProduto.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);

                    gdvTrocaProduto.DataSource = trocaProdutoBLL.Listar(new TrocaDAO() { TrocaID = trocaId, SistemaID = usuarioSessao.SistemaID });
                    gdvTrocaProduto.DataBind();

                    if (gdvTrocaProduto.Rows.Count <= 0)
                    {
                        repeaterTroca.Visible = false;
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

        protected void lkbTrocaID_Click(object sender, EventArgs e)
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

                    rpvComandaTroca.ProcessingMode = ProcessingMode.Local;
                    rpvComandaTroca.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "ComandaTroca.rdlc";

                    ReportDataSource rds = new ReportDataSource
                    {
                        Name = "dsComandaTroca",
                        Value = new TrocaBLL().ListarComandaTroca(new TrocaDAO()
                        {
                            TrocaID = Convert.ToInt32(((LinkButton)sender).Text),
                            SistemaID = usuarioSessao.SistemaID
                        })
                    };

                    rpvComandaTroca.LocalReport.DataSources.Clear();
                    rpvComandaTroca.LocalReport.DataSources.Add(rds);
                    rpvComandaTroca.LocalReport.Refresh();

                    mpeTroca.Show();
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

        protected void imbCancelar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var trocaDAO = new TrocaDAO();
                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                int trocaId = 0;
                int.TryParse(txtTrocaID.Text, out trocaId);
                trocaDAO.TrocaID = trocaId;

                trocaDAO.SistemaID = usuarioSessao.SistemaID;

                trocaBLL.Excluir(trocaDAO);

                CarregarRepeaterReserva();

                LimparFormulario();
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

            if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode())
            {
                imbConsultar.Visible = true;
                imbDarBaixa.Visible = true;
                imbInserirTroca.Visible = true;
                imbCancelar.Visible = true;
            }
            else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode())
            {
                imbConsultar.Visible = true;
                imbDarBaixa.Visible = true;
                imbInserirTroca.Visible = true;
                imbCancelar.Visible = false;
            }
            else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Logistica.GetHashCode())
            {
                imbConsultar.Visible = true;
                imbDarBaixa.Visible = false;
                imbInserirTroca.Visible = true;
                imbCancelar.Visible = false;
            }
            else
            {
                imbConsultar.Visible = false;
                imbDarBaixa.Visible = false;
                imbInserirTroca.Visible = false;
                imbCancelar.Visible = false;
            }

            // deve setar loja depósito, sempre
            if (usuarioSessao.SistemaID == 2)
            {
                ddlLoja.SelectedValue = "3";
                ddlLoja.Enabled = false;
            }
        }
    }
}