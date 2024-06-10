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
    public partial class VendaProdutoRel : Page
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
                    else if ((new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode()) && (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode()))
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

                    this.CarregarDropDownListLoja();
                    this.CarregarDropDownListFuncionario();
                    this.CarregarDropDownListLinha();
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

        private void CarregarDropDownListLinha()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((this.Session["dsDropDownListLinha"] == null) || (this.Session["bdLinha"] != null))
            {
                this.Session["dsDropDownListLinha"] = LinhaDAL.ListarDropDownList(usuarioSessao.SistemaID);
                this.Session["bdLinha"] = null;
            }
            this.ddlLinha.DataSource = this.Session["dsDropDownListLinha"];
            this.ddlLinha.DataBind();
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.ddlLoja.DataSource = new LojaDAL().ListarOrcamentoDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
            this.ddlLoja.DataBind();
        }
        private DataTable ListarVendaProduto(bool adicionarTotal)
        {
            var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            DateTime dtPedidoInicial;
            DateTime.TryParse(txtDataPedidoDe.Text, out dtPedidoInicial);

            DateTime dtPedidoFinal;
            DateTime.TryParse(txtDataPedidoAte.Text, out dtPedidoFinal);

            if (dtPedidoInicial == DateTime.MinValue || dtPedidoFinal == DateTime.MinValue)
                throw new ApplicationException("É necessário informar todas as datas para visualizar o relatório.");

            int lojaId;
            int.TryParse(this.ddlLoja.SelectedValue, out lojaId);

            int linhaId;
            int.TryParse(this.ddlLinha.SelectedValue, out linhaId);

            int funcionarioId;
            int.TryParse(this.ddlFuncionario.SelectedValue, out funcionarioId);

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

            var dt = RelatorioDAL.ListarVendaProduto(dtPedidoInicial, dtPedidoFinal, usuarioSessao.SistemaID, lojaId, linhaId, string.Join(",", produtos), funcionarioId);

            if (adicionarTotal)
            {
                var drTotal = dt.NewRow();

                drTotal[0] = DBNull.Value;
                drTotal[1] = DBNull.Value;
                drTotal[2] = DBNull.Value;
                drTotal[3] = DBNull.Value;
                drTotal[4] = DBNull.Value;
                drTotal[5] = DBNull.Value;
                drTotal[6] = DBNull.Value;
                drTotal[7] = DBNull.Value;
                drTotal[8] = DBNull.Value;
                drTotal[9] = DBNull.Value;
                drTotal[10] = "Total";
                drTotal[11] = DBNull.Value;
                drTotal[12] = dt.AsEnumerable().Sum(x => x.Field<double?>("Preco") * x.Field<int?>("Quantidade"));

                dt.Rows.Add(drTotal);
            }

            return dt;
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
                    var dataTable = this.ListarVendaProduto(false);

                    gdvProduto.DataSource = dataTable;
                    gdvProduto.DataBind();

                    if (dataTable.Rows.Count <= 0)
                        UtilitarioBLL.ExibirMensagemAjax(this.Page, "Não existem produtos com esses critérios de pesquisa");

                    this.txtTotalPreco.Text = string.Format("{0:c}", dataTable.AsEnumerable().Sum(x => x.Field<double?>("Preco") * x.Field<int?>("Quantidade")));
                }
            }
            catch (FormatException ex)
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
                    var dataTable = this.ListarVendaProduto(true);

                    if (dataTable.Rows.Count <= 0)
                    {
                        UtilitarioBLL.ExibirMensagemAjax(this.Page, "Não existem produtos com esses critérios de pesquisa");
                        return;
                    }

                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dataTable);

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("Content-Disposition", "attachment;filename=relatorio-vendaproduto-" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");

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
    }
}