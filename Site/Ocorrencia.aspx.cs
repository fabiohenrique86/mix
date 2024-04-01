using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;
using BLL;
using DAO;
using DAL;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Site
{
    public partial class Ocorrencia : Page
    {
        private OcorrenciaBLL OcorrenciaBLL;
        private OcorrenciaProdutoBLL OcorrenciaProdutoBLL;
        private MotivoOcorrenciaBLL MotivoOcorrenciaBLL;

        public Ocorrencia()
        {
            OcorrenciaBLL = new OcorrenciaBLL();
            OcorrenciaProdutoBLL = new OcorrenciaProdutoBLL();
            MotivoOcorrenciaBLL = new MotivoOcorrenciaBLL();
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
                        CarregarMotivo();
                        CarregarDropDownListLoja();
                        CarregarOcorrencia();
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

        private void CarregarMotivo()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            var motivoOcorrencias = MotivoOcorrenciaBLL.Listar(new MotivoOcorrenciaDAO() { SistemaID = usuarioSessao.SistemaID });

            // valor padrão
            motivoOcorrencias.Add(new MotivoOcorrenciaDAO() { MotivoOcorrenciaID = 0, Descricao = " SELECIONE", SistemaID = usuarioSessao.SistemaID });

            ddlMotivoOcorrencia.DataSource = motivoOcorrencias.OrderBy(x => x.Descricao);
            ddlMotivoOcorrencia.DataBind();
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((Session["dsDropDownListLoja"] == null) || (Session["bdLoja"] != null))
            {
                Session["dsDropDownListLoja"] = new DAL.LojaDAL().ListarDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
                Session["bdLoja"] = null;
            }
            ddlLoja.DataSource = Session["dsDropDownListLoja"];
            ddlLoja.DataBind();
        }

        private void CarregarOcorrencia()
        {
            OcorrenciaDAO ocorrenciaDAO = ObterOcorrencia();

            rptOcorrencia.DataSource = OcorrenciaBLL.Listar(ocorrenciaDAO);
            rptOcorrencia.DataBind();
        }

        protected void imbConsultar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    AplicacaoBLL.Empresa = null;

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
                    CarregarOcorrencia();
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

        protected void imbFechar_Click(object sender, ImageClickEventArgs e)
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

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static string GerarOcorrencia(string sistemaId, ProdutoDAO[] produtosDAO, string notaFiscalId, string arquivoColeta)
        {
            var retorno = string.Empty;
            var sucesso = false;

            try
            {
                if (string.IsNullOrEmpty(sistemaId))
                {
                    retorno = "Sistema não informado";
                    return retorno;
                }

                if (produtosDAO == null || produtosDAO.Count() <= 0)
                {
                    retorno = "Produto não informado";
                    return retorno;
                }

                if (string.IsNullOrEmpty(notaFiscalId))
                {
                    retorno = "Nota Fiscal não informada";
                    return retorno;
                }

                if (string.IsNullOrEmpty(arquivoColeta))
                {
                    retorno = "Arquivo de coleta não informado";
                    return retorno;
                }

                int ocorrenciasGeradas = 0;
                List<string> arquivosColetas = new List<string>();
                List<dynamic> produtos = new List<dynamic>();
                OcorrenciaBLL ocorrenciaBLL = new OcorrenciaBLL();
                NotaFiscalBLL notaFiscalBLL = new NotaFiscalBLL();
                List<NotaFiscalDAO> notasFiscaisDAO = new List<NotaFiscalDAO>();
                List<ProdutoDAO> produtosTotalDAO = new List<ProdutoDAO>();

                var notas = notaFiscalId.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (var nota in notas)
                {
                    notasFiscaisDAO.Add(new NotaFiscalDAO() { NotaFiscalID = Convert.ToInt32(nota) });
                }

                arquivosColetas = arquivoColeta.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList(); // pula a 1ª linha (cabeçalho)

                foreach (var linhaArquivoColeta in arquivosColetas)
                {
                    var campos = linhaArquivoColeta.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    var codigoProduto = string.Empty;
                    var quantidadeRealProduto = campos[5];

                    // obtém o produto passado por parâmetro baseado no codigo do produto do arquivo de coleta
                    var p = produtosDAO.FirstOrDefault(x => x.ProdutoID == Convert.ToInt64(campos[1]));

                    if (p == null)
                    {
                        continue;
                    }
                    
                    p.ProdutoID = p.SobMedida == 1 ? Convert.ToInt64(p.CodigoSobMedida) : Convert.ToInt64(campos[1]); // se produto é "sob medida", usa o código do produto "sob medida"
                    p.SistemaID = Convert.ToInt32(sistemaId);

                    var mensagem = string.Empty;
                    var quantidadeDiferente = 0;
                    var sobMedida = 0;
                    var geraOcorrencia = false;

                    notaFiscalBLL.ImportarArquivoColeta(p, notasFiscaisDAO, out mensagem, out quantidadeDiferente, out sobMedida, out geraOcorrencia);
                    
                    if (geraOcorrencia)
                    {
                        produtos.Add(new
                        {
                            ProdutoID = p.ProdutoID,
                            Quantidade = p.Quantidade,
                            Mensagem = mensagem,
                            SobMedida = sobMedida,
                            LojaID = p.LojaID,
                            NotaFiscalID = p.NotaFiscalID
                        });
                    }
                }

                // gera uma ocorrência para cada produto
                foreach (var produto in produtos)
                {
                    OcorrenciaDAO ocorrenciaDAO = new OcorrenciaDAO();

                    ocorrenciaDAO.DataOcorrencia = DateTime.Now;
                    ocorrenciaDAO.MotivoOcorrenciaID = (int)EMotivoOcorrencia.FALTA_DE_MERCADORIA;
                    ocorrenciaDAO.LojaID = Convert.ToInt32(produto.LojaID);
                    ocorrenciaDAO.SistemaID = Convert.ToInt32(sistemaId);
                    ocorrenciaDAO.ProdutoDAO.Add(new ProdutoDAO() { ProdutoID = Convert.ToInt64(produto.ProdutoID), Quantidade = Convert.ToInt16(produto.Quantidade), SistemaID = Convert.ToInt32(sistemaId) });
                    ocorrenciaDAO.NotaFiscalID = produto.NotaFiscalID;

                    ocorrenciaBLL.Inserir(ocorrenciaDAO);

                    ocorrenciasGeradas++;
                }

                retorno = string.Format("{0} ocorrências foram geradas com sucesso.", ocorrenciasGeradas);
                sucesso = true;
            }
            catch (ApplicationException ex)
            {
                retorno = ex.Message;
            }
            catch (Exception ex)
            {
                retorno = "Ocorreu um erro ao gerar ocorrências. Tente novamente";
            }

            var sJSON = JsonConvert.SerializeObject(new { Mensagem = retorno, Sucesso = sucesso });
            return sJSON;
        }

        protected void imbGerarOcorrencia_Click(object sender, ImageClickEventArgs e)
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
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                try
                {
                    OcorrenciaDAO ocorrenciaDAO = ObterOcorrencia();

                    ocorrenciaDAO.DataOcorrencia = DateTime.Now;
                    ocorrenciaDAO.StatusOcorrenciaID = EStatusOcorrencia.Pendente.GetHashCode();

                    for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
                    {
                        if (Request.Form["txtProdutoID_" + i] != null && Request.Form["txtQuantidade_" + i] != null)
                        {
                            long produtoId;
                            long.TryParse(Request.Form.GetValues("txtProdutoID_" + i).FirstOrDefault(), out produtoId);

                            short quantidade;
                            short.TryParse(Request.Form.GetValues("txtQuantidade_" + i).FirstOrDefault(), out quantidade);

                            ocorrenciaDAO.ProdutoDAO.Add(new ProdutoDAO()
                            {
                                ProdutoID = produtoId,
                                Quantidade = quantidade,
                                SistemaID = usuarioSessao.SistemaID
                            });
                        }
                    }

                    OcorrenciaBLL.Inserir(ocorrenciaDAO);
                    LimparFormulario();
                    CarregarOcorrencia();
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
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                try
                {
                    OcorrenciaDAO ocorrenciaDAO = ObterOcorrencia();

                    OcorrenciaBLL.DarBaixa(ocorrenciaDAO);

                    LimparFormulario();

                    CarregarOcorrencia();
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

        private void LimparFormulario()
        {
            txtOcorrenciaID.Text = string.Empty;
            TxtNumeroTroca.Text = string.Empty;
            txtNotaFiscalID.Text = string.Empty;
            ddlLoja.SelectedIndex = 0;
            txtDataOcorrenciaInicial.Text = string.Empty;
            txtDataOcorrenciaInicial.Text = string.Empty;
            ddlMotivoOcorrencia.SelectedIndex = 0;
            txtNomeMotorista.Text = string.Empty;
            txtPlacaCaminhao.Text = string.Empty;
            txtObservacao.Text = string.Empty;
            txtProduto.Text = string.Empty;
            ddlStatusOcorrencia.SelectedIndex = 0;
        }

        private OcorrenciaDAO ObterOcorrencia()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            int ocorrenciaId;
            int.TryParse(txtOcorrenciaID.Text, out ocorrenciaId);

            int numeroTroca;
            int.TryParse(TxtNumeroTroca.Text, out numeroTroca);

            int notaFiscalId;
            int.TryParse(txtNotaFiscalID.Text, out notaFiscalId);

            int lojaId;
            int.TryParse(ddlLoja.SelectedValue, out lojaId);

            int motivoId;
            int.TryParse(ddlMotivoOcorrencia.SelectedValue, out motivoId);

            DateTime dataOcorrenciaInicial = DateTime.MinValue;
            DateTime.TryParse(txtDataOcorrenciaInicial.Text.Trim(), out dataOcorrenciaInicial);

            DateTime dataOcorrenciaFinal = DateTime.MinValue;
            DateTime.TryParse(txtDataOcorrenciaFinal.Text.Trim(), out dataOcorrenciaFinal);

            int statusOcorrenciaId;
            int.TryParse(ddlStatusOcorrencia.SelectedValue, out statusOcorrenciaId);

            // se nenhum filtro foi aplicado, busca as ocorrências dos últimos 15 dias
            if (ocorrenciaId <= 0 &&
                numeroTroca <= 0 &&
                notaFiscalId <= 0 &&
                lojaId <= 0 &&
                motivoId <= 0 &&
                dataOcorrenciaInicial == DateTime.MinValue &&
                dataOcorrenciaFinal == DateTime.MinValue &&
                statusOcorrenciaId <= 0)
            {
                dataOcorrenciaInicial = DateTime.Now.Date.AddDays(-15);
                dataOcorrenciaFinal = DateTime.Now;
            }

            OcorrenciaDAO ocorrenciaDAO = new OcorrenciaDAO()
            {
                OcorrenciaID = ocorrenciaId,
                NumeroTroca = numeroTroca,
                NotaFiscalID = notaFiscalId,
                LojaID = lojaId,
                DataOcorrenciaInicial = dataOcorrenciaInicial,
                DataOcorrenciaFinal = dataOcorrenciaFinal,
                MotivoOcorrenciaID = motivoId,
                NomeMotorista = txtNomeMotorista.Text.Trim(),
                PlacaCaminhao = txtPlacaCaminhao.Text.Trim(),
                Observacao = txtObservacao.Text.Trim(),
                StatusOcorrenciaID = statusOcorrenciaId,
                SistemaID = usuarioSessao.SistemaID
            };

            return ocorrenciaDAO;
        }

        protected void rptOcorrencia_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    var repeaterOcorrencia = (HtmlContainerControl)e.Item.FindControl("repeaterOcorrencia");

                    int statusOcorrenciaID = Convert.ToInt32(((Label)e.Item.FindControl("lblStatusOcorrenciaID")).Text);

                    if (statusOcorrenciaID == EStatusOcorrencia.Pendente.GetHashCode())
                    {
                        repeaterOcorrencia.Style.Add("background-color", BLL.UtilitarioBLL.COR_STATUS_PENDENTE);
                    }
                    else if (statusOcorrenciaID == EStatusOcorrencia.Resolvida.GetHashCode())
                    {
                        repeaterOcorrencia.Style.Add("background-color", BLL.UtilitarioBLL.COR_STATUS_OK);
                    }

                    int ocorrenciaId = 0;

                    if ((usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()) || (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode()))
                    {
                        var lkbOcorrenciaID = ((LinkButton)e.Item.FindControl("lkbOcorrenciaID"));
                        lkbOcorrenciaID.Visible = true;
                        ocorrenciaId = Convert.ToInt32(lkbOcorrenciaID.Text);
                    }
                    else
                    {
                        var lblOcorrenciaID = ((Label)e.Item.FindControl("lblOcorrencia"));
                        lblOcorrenciaID.Visible = true;
                        ocorrenciaId = Convert.ToInt32(lblOcorrenciaID.Text);
                    }

                    GridView gdvProduto = (GridView)e.Item.FindControl("gdvProduto");
                    gdvProduto.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);

                    gdvProduto.DataSource = OcorrenciaProdutoBLL.Listar(new ProdutoDAO() { OcorrenciaID = ocorrenciaId, SistemaID = usuarioSessao.SistemaID });
                    gdvProduto.DataBind();
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

        protected void lkbOcorrenciaID_Click(object sender, EventArgs e)
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
                    rpvOcorrencia.ProcessingMode = ProcessingMode.Local;
                    rpvOcorrencia.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "ComandaOcorrencia.rdlc";

                    var ocorrenciaId = ((LinkButton)sender).Text;
                    var ocorrencia = RelatorioDAL.ListarComandaOcorrencia(new OcorrenciaDAO() { OcorrenciaID = Convert.ToInt32(ocorrenciaId), SistemaID = usuarioSessao.SistemaID });

                    ReportDataSource rdsOcorrencia = new ReportDataSource
                    {
                        Name = "dsComandaOcorrencia_spListarComandaOcorrencia",
                        Value = ocorrencia
                    };

                    rpvOcorrencia.LocalReport.SetParameters(new ReportParameter("P_OcorrenciaID", ocorrenciaId));
                    rpvOcorrencia.LocalReport.SetParameters(new ReportParameter("P_NotaFiscalID", ocorrencia.Rows[0]["NotaFiscalID"].ToString()));
                    rpvOcorrencia.LocalReport.SetParameters(new ReportParameter("P_MotivoOcorrenciaID", ocorrencia.Rows[0]["MotivoOcorrenciaID"].ToString()));
                    rpvOcorrencia.LocalReport.SetParameters(new ReportParameter("P_Observacao", ocorrencia.Rows[0]["Observacao"].ToString()));
                    rpvOcorrencia.LocalReport.SetParameters(new ReportParameter("P_DataOcorrencia", Convert.ToDateTime(ocorrencia.Rows[0]["DataOcorrencia"]).ToString("dd/MM/yyyy")));
                    rpvOcorrencia.LocalReport.SetParameters(new ReportParameter("P_NomeMotorista", ocorrencia.Rows[0]["NomeMotorista"].ToString()));
                    rpvOcorrencia.LocalReport.SetParameters(new ReportParameter("P_PlacaCaminhao", ocorrencia.Rows[0]["PlacaCaminhao"].ToString()));
                    rpvOcorrencia.LocalReport.SetParameters(new ReportParameter("P_NomeFantasia", ocorrencia.Rows[0]["NomeFantasia"].ToString()));

                    rpvOcorrencia.LocalReport.DataSources.Clear();
                    rpvOcorrencia.LocalReport.DataSources.Add(rdsOcorrencia);
                    rpvOcorrencia.LocalReport.Refresh();

                    mpeOcorrencia.Show();
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

            if (usuarioSessao.TipoUsuarioID != UtilitarioBLL.TipoUsuario.Administrador.GetHashCode() &&
                usuarioSessao.TipoUsuarioID != UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode())
            {
                imbGerarOcorrencia.Visible = false;
                imbDarBaixa.Visible = false;
            }
        }
    }
}