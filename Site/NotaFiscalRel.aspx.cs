using BLL;
using ClosedXML.Excel;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace Site
{
    public partial class NotaFiscalRel : Page
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

                    Session["DiaMesAnoInicial"] = null;
                    Session["DiaMesAnoFinal"] = null;
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

        protected void imbGerar_Click(object sender, ImageClickEventArgs e)
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
                    var dataTable = this.ListarRelatorio();

                    if (dataTable.Rows.Count <= 0)
                    {
                        UtilitarioBLL.ExibirMensagemAjax(this.Page, "Não existem notas fiscais com esses critérios de pesquisa");
                        return;
                    }

                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dataTable);

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", "attachment;filename=relatorio-notafiscal-" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");

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
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, "Data inválida");
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
                    var dataTable = this.ListarRelatorio();

                    gdvNotaFiscal.DataSource = dataTable;
                    gdvNotaFiscal.DataBind();

                    if (dataTable.Rows.Count <= 0)
                        UtilitarioBLL.ExibirMensagemAjax(this.Page, "Não existem notas fiscais com esses critérios de pesquisa");
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, "Data inválida");
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

        private DataTable ListarRelatorio()
        {
            var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            DateTime dtNfInicial;
            DateTime.TryParse(txtDataNotaFiscalDe.Text, out dtNfInicial);

            DateTime dtNfFinal;
            DateTime.TryParse(txtDataNotaFiscalAte.Text, out dtNfFinal);

            int nfId;
            int.TryParse(txtNotaFiscalID.Text, out nfId);

            var produtos = new List<string>();

            for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
            {
                if (Request.Form["txtProdutoID_" + i] != null)
                {
                    string produtoId = Request.Form.GetValues("txtProdutoID_" + i).FirstOrDefault();

                    if (string.IsNullOrEmpty(produtoId))
                        throw new ApplicationException(string.Format("Informe o produto {0}.", produtoId));

                    produtos.Add(produtoId);
                }
            }

            var dataTable = RelatorioDAL.ListarNotaFiscal(usuarioSessao.SistemaID, dtNfInicial, dtNfFinal, nfId, txtNumeroCarga.Text.Trim(), string.Join(",", produtos));

            return dataTable;
        }
    }
}