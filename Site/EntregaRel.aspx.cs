using BLL;
using ClosedXML.Excel;
using DAL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Web.UI;

namespace Site
{
    public partial class EntregaRel : Page
    {
        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            ddlLoja.DataSource = new DAL.LojaDAL().ListarOrcamentoDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
            ddlLoja.DataBind();
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
                    if ((this.Session["DiaMesAnoInicial"] == null) || (this.Session["DiaMesAnoFinal"] == null))
                    {
                        throw new ApplicationException("É necessário informar todas as datas para visualizar o relatório.");
                    }
                    rpvEntrega.ProcessingMode = ProcessingMode.Local;
                    rpvEntrega.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "Entrega.rdlc";
                    ReportDataSource rdsTeste = new ReportDataSource
                    {
                        Name = "dsEntrega_spListarEntregaRel",
                        Value = RelatorioDAL.ListarEntrega(Convert.ToDateTime(this.Session["DiaMesAnoInicial"]), Convert.ToDateTime(this.Session["DiaMesAnoFinal"]), new int?(string.IsNullOrWhiteSpace(this.ddlLoja.SelectedValue) ? 0 : Convert.ToInt32(this.ddlLoja.SelectedValue)), txtPedidoID.Text.Trim().ToUpper(), string.IsNullOrWhiteSpace(this.txtNomeCliente.Text.Trim().ToUpper()) ? string.Empty : this.txtNomeCliente.Text.Trim().ToUpper(), string.IsNullOrWhiteSpace(this.txtBairro.Text.Trim().ToUpper()) ? string.Empty : this.txtBairro.Text.Trim().ToUpper(), new int?(string.IsNullOrWhiteSpace(this.ddlStatus.SelectedValue) ? 0 : Convert.ToInt32(this.ddlStatus.SelectedValue)), usuarioSessao.SistemaID)
                    };
                    rpvEntrega.LocalReport.DataSources.Clear();
                    rpvEntrega.LocalReport.DataSources.Add(rdsTeste);
                    rpvEntrega.LocalReport.Refresh();
                    mpeEntrega.Show();
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
                    Session["DiaMesAnoInicial"] = null;
                    Session["DiaMesAnoFinal"] = null;
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

        protected void imbGerar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    BLL.AplicacaoBLL.Empresa = null;

                    if (base.Request.Url.Segments.Length == 3)
                        base.Response.Redirect("../Default.aspx", true);
                    else
                        base.Response.Redirect("Default.aspx", true);
                }
                else
                {
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                    if ((this.Session["DiaMesAnoInicial"] == null) || (this.Session["DiaMesAnoFinal"] == null))
                        throw new ApplicationException("É necessário informar todas as datas para visualizar o relatório.");

                    var dataTable = RelatorioDAL.ListarEntregaCSV(Convert.ToDateTime(this.Session["DiaMesAnoInicial"]), Convert.ToDateTime(this.Session["DiaMesAnoFinal"]), new int?(string.IsNullOrWhiteSpace(this.ddlLoja.SelectedValue) ? 0 : Convert.ToInt32(this.ddlLoja.SelectedValue)), txtPedidoID.Text.Trim().ToUpper(), string.IsNullOrWhiteSpace(this.txtNomeCliente.Text.Trim().ToUpper()) ? string.Empty : this.txtNomeCliente.Text.Trim().ToUpper(), string.IsNullOrWhiteSpace(this.txtBairro.Text.Trim().ToUpper()) ? string.Empty : this.txtBairro.Text.Trim().ToUpper(), new int?(string.IsNullOrWhiteSpace(this.ddlStatus.SelectedValue) ? 0 : Convert.ToInt32(this.ddlStatus.SelectedValue)), usuarioSessao.SistemaID);
                    //var builder = new StringBuilder();
                    //var columnNames = new List<string>();
                    //var rows = new List<string>();

                    //foreach (DataColumn column in dataTable.Columns)
                    //    columnNames.Add(column.ColumnName);

                    //builder.Append(string.Join(";", columnNames.ToArray())).Append("\n");

                    //foreach (DataRow row in dataTable.Rows)
                    //{
                    //    var currentRow = new List<string>();

                    //    foreach (DataColumn column in dataTable.Columns)
                    //        currentRow.Add(row[column].ToString());

                    //    rows.Add(string.Join(";", currentRow.ToArray()));
                    //}

                    //builder.Append(string.Join("\n", rows.ToArray()));

                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dataTable);

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", "attachment;filename=relatorio-entrega-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls");

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