using System;
using System.Linq;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;
using BLL;
using DAL;
using DAO;

namespace Site
{
    public partial class OcorrenciaRel : Page
    {
        private MotivoOcorrenciaBLL MotivoOcorrenciaBLL;

        public OcorrenciaRel()
        {
            MotivoOcorrenciaBLL = new MotivoOcorrenciaBLL();
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

                    CarregarDropDownListLoja();
                    CarregarDropDownListMotivo();
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

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            ddlLoja.DataSource = new LojaDAL().ListarOrcamentoDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
            ddlLoja.DataBind();
        }

        private void CarregarDropDownListMotivo()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            var motivoOcorrencias = MotivoOcorrenciaBLL.Listar(new MotivoOcorrenciaDAO() { SistemaID = usuarioSessao.SistemaID });

            // valor padrão
            motivoOcorrencias.Add(new MotivoOcorrenciaDAO() { MotivoOcorrenciaID = 0, Descricao = " SELECIONE", SistemaID = usuarioSessao.SistemaID });

            ddlMotivoOcorrencia.DataSource = motivoOcorrencias.OrderBy(x => x.Descricao);
            ddlMotivoOcorrencia.DataBind();
        }
        
        protected void imbVisualizar_Click(object sender, ImageClickEventArgs e)
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
                    rpvOcorrencia.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "Ocorrencia.rdlc";

                    int ocorrenciaId;
                    int.TryParse(txtOcorrenciaID.Text, out ocorrenciaId);

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

                    OcorrenciaDAO ocorrenciaDAO = new OcorrenciaDAO()
                    {
                        OcorrenciaID = ocorrenciaId,
                        LojaID = lojaId,
                        MotivoOcorrenciaID = motivoId,
                        DataOcorrenciaInicial = dataOcorrenciaInicial,
                        DataOcorrenciaFinal = dataOcorrenciaFinal,
                        StatusOcorrenciaID = statusOcorrenciaId,
                        SistemaID = usuarioSessao.SistemaID
                    };

                    ReportDataSource rdsTeste = new ReportDataSource
                    {
                        Name = "dsComandaOcorrencia_spListarComandaOcorrencia",
                        Value = RelatorioDAL.ListarComandaOcorrencia(ocorrenciaDAO)
                    };

                    rpvOcorrencia.LocalReport.DataSources.Clear();
                    rpvOcorrencia.LocalReport.DataSources.Add(rdsTeste);
                    rpvOcorrencia.LocalReport.Refresh();

                    mpeOcorrencia.Show();
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
    }
}