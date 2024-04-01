using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAO;
using BLL;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;
using DAL;

namespace Site
{
    public partial class Pedido2 : Page
    {
        private bool CarregarDados(PedidoDAO pedido)
        {
            ViewState["filtroPedido"] = true;
            rptPedido.DataSource = new PedidoDAL().ListarByFiltro(pedido);
            rptPedido.DataBind();
            return (rptPedido.Items.Count > 0);
        }

        private void CarregarDropDownListFuncionario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((Session["dsDropDownListFuncionario"] == null) || (Session["bdFuncionario"] != null))
            {
                Session["dsDropDownListFuncionario"] = DAL.FuncionarioDAL.ListarDropDownList(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
                Session["bdFuncionario"] = null;
            }
            ddlFuncionario.DataSource = Session["dsDropDownListFuncionario"];
            ddlFuncionario.DataBind();
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

        //private void CarregarGridTipoPagamento()
        //{
        //    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
        //    if ((Session["dsGridTipoPagamento"] == null) || (Session["bdTipoPagamento"] != null))
        //    {
        //        Session["dsGridTipoPagamento"] = DAL.TipoPagamentoDAL.Listar(usuarioSessao.SistemaID);
        //        Session["bdTipoPagamento"] = null;
        //    }
        //    gdvTipoPagamento.DataSource = Session["dsGridTipoPagamento"];
        //    gdvTipoPagamento.DataBind();
        //}

        private void CarregarRepeaterPedido()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            rptPedido.DataSource = new DAL.PedidoDAL().ListarByLoja(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID, true);
            rptPedido.DataBind();
        }
        
        protected void gdvTipoPagamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlParcela = (DropDownList)e.Row.FindControl("ddlParcela");
                    ddlParcela.DataSource = DAL.ParcelaDAL.ListarDropDownList(usuarioSessao.SistemaID);
                    ddlParcela.DataBind();
                    if (((Label)e.Row.FindControl("lblTipoPagamento")).Text.ToUpper().Contains("DINHEIRO"))
                    {
                        ddlParcela.SelectedIndex = 1;
                        ddlParcela.CssClass = "desabilitado";
                        ddlParcela.Enabled = false;
                    }
                    ((TextBox)e.Row.FindControl("txtValorPago")).Attributes.Add("onblur", "CalcularPagamentoTotal();");
                    ddlParcela.Attributes.Add("onblur", "CalcularPagamentoTotal();");
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
                    string pedidoId = string.Empty;
                    if (!string.IsNullOrEmpty(txtPedidoID.Text.Trim().ToUpper()))
                    {
                        pedidoId = txtPedidoID.Text.Trim().ToUpper();
                    }
                    int? lojaId = null;
                    if (ddlLoja.SelectedValue != "0")
                    {
                        lojaId = new int?(Convert.ToInt32(ddlLoja.SelectedValue));
                    }
                    int? lojaOrigemId = null;
                    if (usuarioSessao.LojaID != 0)
                    {
                        lojaOrigemId = new int?(usuarioSessao.LojaID);
                    }
                    int? funcionarioId = null;
                    if (ddlFuncionario.SelectedValue != "0")
                    {
                        funcionarioId = new int?(Convert.ToInt32(ddlFuncionario.SelectedValue));
                    }
                    string cpfFormatado = string.IsNullOrEmpty(txtCpf.Text.Trim().ToUpper().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCpf.Text.Trim().ToUpper().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                    string cnpjFormatado = string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")) ? null : txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                    string dataPedido = txtDataPedido.Text.Trim().ToUpper().Replace("_", "").Replace("-", "").Replace("/", "");
                    DateTime? dtPedido = null;

                    if (((!string.IsNullOrEmpty(pedidoId) && !lojaId.HasValue) && (!funcionarioId.HasValue && string.IsNullOrEmpty(cpfFormatado))) && (string.IsNullOrEmpty(cnpjFormatado) && string.IsNullOrEmpty(dataPedido)))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }

                    if (!string.IsNullOrEmpty(cpfFormatado) && !string.IsNullOrEmpty(cnpjFormatado))
                    {
                        throw new ApplicationException("Informe CPF ou CNPJ para consultar.");
                    }

                    if (!string.IsNullOrEmpty(dataPedido))
                    {
                        if (!BLL.UtilitarioBLL.ValidarData(dataPedido))
                        {
                            throw new ApplicationException("DataPedido inválida.");
                        }

                        dtPedido = Convert.ToDateTime(txtDataPedido.Text.Trim());
                    }

                    if (!CarregarDados(new PedidoDAO(pedidoId, lojaId, lojaOrigemId, funcionarioId, cpfFormatado, cnpjFormatado, dtPedido, usuarioSessao.SistemaID)))
                    {
                        throw new ApplicationException("Pedido inexistente.");
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
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                    new PedidoDAL().AtualizarStatusImprimido(Session["PedidoID"].ToString(), usuarioSessao.SistemaID, "P");

                    int? pedidoId = null;
                    if (!string.IsNullOrEmpty(txtPedidoID.Text.Trim().ToUpper()))
                    {
                        pedidoId = Convert.ToInt32(txtPedidoID.Text.Trim().ToUpper());
                    }
                    int? lojaId = null;
                    if (ddlLoja.SelectedValue != "0")
                    {
                        lojaId = Convert.ToInt32(ddlLoja.SelectedValue);
                    }
                    int? funcionarioId = null;
                    if (ddlFuncionario.SelectedValue != "0")
                    {
                        funcionarioId = Convert.ToInt32(ddlFuncionario.SelectedValue);
                    }
                    string cpfFormatado = string.IsNullOrEmpty(txtCpf.Text.Trim().ToUpper().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCpf.Text.Trim().ToUpper().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                    string cnpjFormatado = string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")) ? null : txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                    string dataPedido = txtDataPedido.Text.Trim().ToUpper().Replace("_", "").Replace("-", "").Replace("/", "");
                    DateTime? dtPedido = null;

                    if (((pedidoId.HasValue || lojaId.HasValue) || (funcionarioId.HasValue || !string.IsNullOrEmpty(cpfFormatado))) || (!string.IsNullOrEmpty(cnpjFormatado) || !string.IsNullOrEmpty(dataPedido)))
                    {
                        Session["Filtro"] = true;
                        Session["PedidoID"] = pedidoId;
                        Session["LojaID"] = lojaId;
                        Session["FuncionarioID"] = funcionarioId;
                        Session["Cpf"] = cpfFormatado;
                        Session["Cnpj"] = cnpjFormatado;
                        Session["DataPedido"] = string.IsNullOrEmpty(dataPedido) ? dtPedido : Convert.ToDateTime(txtDataPedido.Text.Trim().ToUpper());
                    }
                    else
                    {
                        Session["Filtro"] = null;
                        Session["PedidoID"] = null;
                        Session["LojaID"] = null;
                        Session["FuncionarioID"] = null;
                        Session["Cpf"] = null;
                        Session["Cnpj"] = null;
                        Session["DataPedido"] = null;
                    }

                    base.Response.Redirect("Pedido.aspx", false);
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

        protected void imbInserirPedido_Click(object sender, ImageClickEventArgs e)
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

                string pedidoId = string.Empty;
                if (!string.IsNullOrEmpty(txtPedidoID.Text.Trim().ToUpper()))
                {
                    pedidoId = txtPedidoID.Text.Trim().ToUpper();
                }
                int lojaId = 0;
                if (ddlLoja.SelectedValue != "0")
                {
                    lojaId = Convert.ToInt32(ddlLoja.SelectedValue);
                }
                int lojaOrigemId = 0;
                if (usuarioSessao.LojaID != 0)
                {
                    lojaOrigemId = usuarioSessao.LojaID;
                }
                int funcionarioId = 0;
                if (ddlFuncionario.SelectedValue != "0")
                {
                    funcionarioId = Convert.ToInt32(ddlFuncionario.SelectedValue);
                }

                string cpfFormatado = string.IsNullOrEmpty(txtCpf.Text.Trim().ToUpper().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCpf.Text.Trim().ToUpper().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                string cnpjFormatado = string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")) ? null : txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                string dataPedido = txtDataPedido.Text.Trim().ToUpper().Replace("_", "").Replace("-", "").Replace("/", "");
                int sistemaId = usuarioSessao.SistemaID;
                List<ProdutoDAO> listaProduto = new List<ProdutoDAO>();
                //List<TipoPagamentoDAO> listaTipoPagamento = new List<TipoPagamentoDAO>();

                try
                {
                    if (((((string.IsNullOrEmpty(pedidoId)) || (lojaId <= 0)) || (funcionarioId <= 0)) || (string.IsNullOrEmpty(cpfFormatado) && string.IsNullOrEmpty(cnpjFormatado))) || string.IsNullOrEmpty(dataPedido))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para inserir pedido.");
                    }

                    ProdutoDAL produtoDAL = new ProdutoDAL();
                    DateTime dtPedido = Convert.ToDateTime(txtDataPedido.Text);
                    PedidoDAO PedidoDAO = new PedidoDAO();

                    if (!string.IsNullOrEmpty(cpfFormatado) && !string.IsNullOrEmpty(cnpjFormatado))
                    {
                        throw new ApplicationException("Informe CPF ou CNPJ para inserir pedido.");
                    }
                    if (!string.IsNullOrEmpty(cpfFormatado))
                    {
                        if (!new BLL.UtilsBLL().ValidarCpf(cpfFormatado))
                        {
                            throw new ApplicationException("CPF inválido.");
                        }
                        PedidoDAO.Cpf = cpfFormatado;
                    }
                    else if (!string.IsNullOrEmpty(cnpjFormatado))
                    {
                        if (!new BLL.UtilsBLL().ValidarCnpj(cnpjFormatado))
                        {
                            throw new ApplicationException("CNPJ inválido.");
                        }
                        PedidoDAO.Cnpj = cnpjFormatado;
                    }

                    //if (!txtTotalPedido.Text.Equals(txtTotalPago.Text))
                    //{
                    //    throw new ApplicationException("Total Pedido deve igual ao Total Pago.");
                    //}
                    
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

                            if (!produtoDAL.ExisteNaLoja(produtoId, lojaId.ToString(), sistemaId))
                            {
                                throw new ApplicationException(string.Format("Produto {0} não está cadastrado na loja {1}.", produtoId, ddlLoja.SelectedItem.Text));
                            }

                            listaProduto.Add(new ProdutoDAO(Convert.ToInt64(produtoId), Convert.ToInt16(quantidade), sobMedida, Convert.ToDecimal(preco), sistemaId));
                        }
                    }
                    
                    if (listaProduto.Count <= 0)
                    {
                        throw new ApplicationException("Informe ao menos um produto para inserir pedido.");
                    }

                    //foreach (GridViewRow gdrTipoPagamento in gdvTipoPagamento.Rows)
                    //{
                    //    if (((CheckBox)gdrTipoPagamento.FindControl("ckbTipoPagamento")).Checked)
                    //    {
                    //        string tipoPagamento = ((Label)gdrTipoPagamento.FindControl("lblTipoPagamento")).Text;
                    //        string tipoPagamentoId = ((Label)gdrTipoPagamento.FindControl("lblTipoPagamentoID")).Text;
                    //        DropDownList ddlParcela = (DropDownList)gdrTipoPagamento.FindControl("ddlParcela");
                    //        if (ddlParcela.SelectedIndex <= 0)
                    //        {
                    //            throw new ApplicationException("Informe a parcela do tipo de pagamento:\r\n\r\n" + tipoPagamento);
                    //        }
                    //        string parcelaId = ddlParcela.SelectedValue;
                    //        string valorPago = ((TextBox)gdrTipoPagamento.FindControl("txtValorPago")).Text;
                    //        if (string.IsNullOrEmpty(valorPago) || (valorPago == "0,00"))
                    //        {
                    //            throw new ApplicationException("Informe o valor pago do tipo de pagamento:\r\n\r\n" + tipoPagamento);
                    //        }
                    //        listaTipoPagamento.Add(new DAO.TipoPagamentoDAO(Convert.ToInt32(tipoPagamentoId), Convert.ToInt32(parcelaId), Convert.ToDecimal(valorPago), sistemaId));
                    //    }
                    //}

                    //if (listaTipoPagamento.Count <= 0)
                    //{
                    //    throw new ApplicationException("Selecione ao menos um tipo de pagamento para inserir pedido.");
                    //}

                    PedidoDAO.PedidoID = pedidoId;
                    PedidoDAO.LojaID = lojaId;
                    PedidoDAO.LojaOrigemID = lojaOrigemId;
                    PedidoDAO.FuncionarioID = funcionarioId;
                    PedidoDAO.Observacao = txtObservacao.Text.Trim().ToUpper();
                    PedidoDAO.DataPedido = dtPedido;
                    PedidoDAO.SistemaID = sistemaId;
                    PedidoDAO.ListaProduto = listaProduto;
                    //PedidoDAO.ListaTipoPagamento = listaTipoPagamento;

                    new BLL.PedidoBLL().Inserir(PedidoDAO);

                    CarregarRepeaterPedido();
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
                        ex.Data.Add("pedidoId", pedidoId);
                        ex.Data.Add("lojaId", lojaId);
                        ex.Data.Add("lojaOrigemId", lojaOrigemId);
                        ex.Data.Add("funcionarioId", funcionarioId);
                        ex.Data.Add("cpfFormatado", cpfFormatado);
                        ex.Data.Add("cnpjFormatado", cnpjFormatado);
                        ex.Data.Add("dataPedido", dataPedido);
                        ex.Data.Add("sistemaId", sistemaId);

                        if (listaProduto != null && listaProduto.Count > 0)
                        {
                            foreach (var item in listaProduto)
                            {
                                ex.Data.Add("ProdutoID_" + Guid.NewGuid().ToString(), item.ProdutoID);
                                ex.Data.Add("Descricao_" + Guid.NewGuid().ToString(), item.Descricao);
                                ex.Data.Add("LinhaID_" + Guid.NewGuid().ToString(), item.LinhaID);
                                ex.Data.Add("LojaID_" + Guid.NewGuid().ToString(), item.LojaID);
                                ex.Data.Add("MedidaID_" + Guid.NewGuid().ToString(), item.MedidaID);
                                ex.Data.Add("Medida_" + Guid.NewGuid().ToString(), item.Medida);
                                ex.Data.Add("Preco_" + Guid.NewGuid().ToString(), item.Preco);
                                ex.Data.Add("Quantidade_" + Guid.NewGuid().ToString(), item.Quantidade);
                                ex.Data.Add("SistemaID_" + Guid.NewGuid().ToString(), item.SistemaID);
                                ex.Data.Add("ComissaoFuncionario_" + Guid.NewGuid().ToString(), item.ComissaoFuncionario);
                                ex.Data.Add("ComissaoFranqueado_" + Guid.NewGuid().ToString(), item.ComissaoFranqueado);
                            }
                        }

                        //if (listaTipoPagamento != null && listaTipoPagamento.Count > 0)
                        //{
                        //    foreach (var item in listaTipoPagamento)
                        //    {
                        //        ex.Data.Add("TipoPagamentoID_" + Guid.NewGuid().ToString(), item.TipoPagamentoID);
                        //        ex.Data.Add("Descricao_" + Guid.NewGuid().ToString(), item.Descricao);
                        //        ex.Data.Add("ParcelaID_" + Guid.NewGuid().ToString(), item.ParcelaID);
                        //        ex.Data.Add("Valor_" + Guid.NewGuid().ToString(), item.Valor);
                        //        ex.Data.Add("SistemaID_" + Guid.NewGuid().ToString(), item.SistemaID);
                        //    }
                        //}

                        UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
                    }
                }
            }
        }

        private void LimparFormulario()
        {
            txtPedidoID.Text = string.Empty;
            ddlLoja.SelectedIndex = 0;
            ddlFuncionario.SelectedIndex = 0;
            txtCpf.Text = string.Empty;
            txtCnpj.Text = string.Empty;
            txtTotalPedido.Text = string.Empty;

            //foreach (GridViewRow gdrTipoPagamento in gdvTipoPagamento.Rows)
            //{
            //    ((CheckBox)gdrTipoPagamento.FindControl("ckbTipoPagamento")).Checked = false;
            //    if (!((Label)gdrTipoPagamento.FindControl("lblTipoPagamento")).Text.ToUpper().Contains("DINHEIRO"))
            //    {
            //        ((DropDownList)gdrTipoPagamento.FindControl("ddlParcela")).SelectedIndex = 0;
            //    }
            //    ((TextBox)gdrTipoPagamento.FindControl("txtValorPago")).Text = string.Empty;
            //}
            //txtTotalPago.Text = string.Empty;

            txtDataPedido.Text = string.Empty;
            txtObservacao.Text = string.Empty;
            ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "LimparGrid", "document.getElementById('ctl00_ContentPlaceHolder1_hdfProdutoValores').value = ''; clearGrid();", true);
        }

        protected void lkbDetalhe_Click(object sender, EventArgs e)
        {
            try
            {
                Session["PedidoDetalheID"] = ((LinkButton)sender).CommandArgument;
                ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "PedidoDetalhe", "var win = window.open('PedidoDetalhe.aspx','_blank','width=625,height=400'); win.moveTo(((screen.width - 625)/2),((screen.height - 400)/2));", true);
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

        protected void lkbPedidoID_Click(object sender, EventArgs e)
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
                    rpvComandaEntrega.ProcessingMode = ProcessingMode.Local;
                    rpvComandaEntrega.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "ComandaEntrega.rdlc";
                    ReportDataSource rdsTeste = new ReportDataSource
                    {
                        Name = "dsComandaEntrega_spListarComandaEntrega",
                        Value = new PedidoDAL().ListarComanda(((LinkButton)sender).Text, usuarioSessao.SistemaID, "P")
                    };
                    rpvComandaEntrega.LocalReport.DataSources.Clear();
                    rpvComandaEntrega.LocalReport.DataSources.Add(rdsTeste);
                    rpvComandaEntrega.LocalReport.Refresh();
                    mpeComanda.Show();
                    Session["PedidoID"] = ((LinkButton)sender).Text;
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
                UtilitarioBLL.SetarMascaraValor(Page);
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
                        BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                        VisualizarFormulario();
                        if (Session["Filtro"] != null)
                        {
                            if ((Session["dsDropDownListLoja"] == null) || (Session["bdLoja"] != null))
                            {
                                Session["dsDropDownListLoja"] = new DAL.LojaDAL().ListarDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
                                Session["bdLoja"] = null;
                            }
                            ddlLoja.DataSource = Session["dsDropDownListLoja"];
                            ddlLoja.DataBind();
                            if ((Session["dsDropDownListFuncionario"] == null) || (Session["bdFuncionario"] != null))
                            {
                                Session["dsDropDownListFuncionario"] = DAL.FuncionarioDAL.ListarDropDownList(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
                                Session["bdFuncionario"] = null;
                            }
                            ddlFuncionario.DataSource = Session["dsDropDownListFuncionario"];
                            ddlFuncionario.DataBind();
                            
                            //if ((Session["dsGridTipoPagamento"] == null) || (Session["bdTipoPagamento"] != null))
                            //{
                            //    Session["dsGridTipoPagamento"] = DAL.TipoPagamentoDAL.Listar(usuarioSessao.SistemaID);
                            //    Session["bdTipoPagamento"] = null;
                            //}
                            //gdvTipoPagamento.DataSource = Session["dsGridTipoPagamento"];
                            //gdvTipoPagamento.DataBind();

                            string pedidoId = string.Empty;
                            if (Session["PedidoID"] != null)
                            {
                                pedidoId = Session["PedidoID"].ToString();
                                txtPedidoID.Text = pedidoId.ToString();
                            }
                            int? lojaId = null;
                            if (Session["LojaID"] != null)
                            {
                                lojaId = new int?(Convert.ToInt32(Session["LojaID"]));
                                ddlLoja.SelectedValue = lojaId.ToString();
                            }
                            int? lojaOrigemId = null;
                            if (usuarioSessao.LojaID != 0)
                            {
                                lojaOrigemId = new int?(usuarioSessao.LojaID);
                            }
                            int? funcionarioId = null;
                            if (Session["FuncionarioID"] != null)
                            {
                                funcionarioId = new int?(Convert.ToInt32(Session["FuncionarioID"]));
                                ddlFuncionario.SelectedValue = funcionarioId.ToString();
                            }
                            string cpfFormatado = null;
                            if (Session["Cpf"] != null)
                            {
                                cpfFormatado = Session["Cpf"].ToString();
                                txtCpf.Text = cpfFormatado.ToString();
                            }
                            string cnpjFormatado = null;
                            if (Session["Cnpj"] != null)
                            {
                                cnpjFormatado = Session["Cnpj"].ToString();
                                txtCnpj.Text = cnpjFormatado.ToString();
                            }
                            DateTime? dtPedido = null;
                            if (Session["DataPedido"] != null)
                            {
                                dtPedido = new DateTime?(Convert.ToDateTime(Session["DataPedido"]));
                                txtDataPedido.Text = dtPedido.ToString();
                            }
                            CarregarDados(new PedidoDAO(pedidoId, lojaId, lojaOrigemId, funcionarioId, cpfFormatado, cnpjFormatado, dtPedido, usuarioSessao.SistemaID));
                            Session["Filtro"] = null;
                            Session["PedidoID"] = null;
                            Session["LojaID"] = null;
                            Session["FuncionarioID"] = null;
                            Session["Cpf"] = null;
                            Session["Cnpj"] = null;
                            Session["DataPedido"] = null;
                        }
                        else
                        {
                            CarregarDropDownListLoja();
                            CarregarDropDownListFuncionario();
                            //CarregarGridTipoPagamento();
                            CarregarRepeaterPedido();
                        }

                        //SetarBordaGridView();
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

        protected void rptPedido_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                DAL.PedidoDAL pedidoDAL = new DAL.PedidoDAL();

                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    string pedidoId;
                    if (
                        (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()) ||
                        (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Estoquista))
                        )
                    {
                        ((LinkButton)e.Item.FindControl("lkbPedidoID")).Visible = true;
                        pedidoId = ((LinkButton)e.Item.FindControl("lkbPedidoID")).Text;
                    }
                    else
                    {
                        ((Label)e.Item.FindControl("lblPedidoID")).Visible = true;
                        pedidoId = ((Label)e.Item.FindControl("lblPedidoID")).Text;
                    }
                    ((LinkButton)e.Item.FindControl("lkbDetalhe")).ToolTip = string.Format("Observação: {0}", ((Label)e.Item.FindControl("lblObservacao")).Text);
                    if (((pedidoId != txtPedidoID.Text) && !string.IsNullOrEmpty(txtPedidoID.Text)) && (ViewState["filtroPedido"] != null))
                    {
                        e.Item.FindControl("repeaterPedido").Visible = false;
                    }
                    else
                    {
                        GridView gdvPedidoProdutoAux = (GridView)e.Item.FindControl("gdvPedidoProduto");
                        gdvPedidoProdutoAux.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
                        if (Convert.ToBoolean(ViewState["filtroPedido"]))
                        {
                            int? lojaId = null;
                            int? lojaOrigemId = null;
                            int? funcionarioId = null;
                            string cpfFormatado = null;
                            string cnpjFormatado = null;
                            DateTime? dtPedido = null;
                            if (Session["Filtro"] != null)
                            {
                                if (Session["LojaID"] != null)
                                {
                                    lojaId = new int?(Convert.ToInt32(Session["LojaID"]));
                                }
                                if (usuarioSessao.LojaID != 0)
                                {
                                    lojaOrigemId = new int?(usuarioSessao.LojaID);
                                }
                                if (Session["FuncionarioID"] != null)
                                {
                                    funcionarioId = new int?(Convert.ToInt32(ddlFuncionario.SelectedValue));
                                }
                                if (Session["Cpf"] != null)
                                {
                                    cpfFormatado = Session["Cpf"].ToString();
                                }
                                if (Session["Cnpj"] != null)
                                {
                                    cnpjFormatado = Session["Cnpj"].ToString();
                                }
                                if (Session["DataPedido"] != null)
                                {
                                    dtPedido = new DateTime?(Convert.ToDateTime(Session["DataPedido"]));
                                }
                                gdvPedidoProdutoAux.DataSource = pedidoDAL.Listar(pedidoId, lojaId, lojaOrigemId, funcionarioId, cpfFormatado, cnpjFormatado, dtPedido, usuarioSessao.SistemaID);
                                gdvPedidoProdutoAux.DataBind();
                            }
                            else
                            {
                                if (ddlLoja.SelectedValue != "0")
                                {
                                    lojaId = new int?(Convert.ToInt32(ddlLoja.SelectedValue));
                                }
                                if (usuarioSessao.LojaID != 0)
                                {
                                    lojaOrigemId = new int?(usuarioSessao.LojaID);
                                }
                                if (ddlFuncionario.SelectedValue != "0")
                                {
                                    funcionarioId = new int?(Convert.ToInt32(ddlFuncionario.SelectedValue));
                                }
                                if (!string.IsNullOrEmpty(txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")))
                                {
                                    cpfFormatado = txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                                }
                                if (!string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")))
                                {
                                    cnpjFormatado = txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                                }
                                if (!string.IsNullOrEmpty(txtDataPedido.Text.Trim().ToUpper().Replace("_", "").Replace("-", "").Replace("/", "")))
                                {
                                    dtPedido = new DateTime?(Convert.ToDateTime(txtDataPedido.Text.Trim().ToUpper()));
                                }
                                gdvPedidoProdutoAux.DataSource = pedidoDAL.Listar(pedidoId, lojaId, lojaOrigemId, funcionarioId, cpfFormatado, cnpjFormatado, dtPedido, usuarioSessao.SistemaID);
                                gdvPedidoProdutoAux.DataBind();
                            }
                        }
                        else
                        {
                            gdvPedidoProdutoAux.DataSource = pedidoDAL.Listar(usuarioSessao.LojaID, pedidoId, usuarioSessao.SistemaID);
                            gdvPedidoProdutoAux.DataBind();
                        }
                        if (gdvPedidoProdutoAux.Rows.Count <= 0)
                        {
                            e.Item.FindControl("repeaterPedido").Visible = false;
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

        //private void SetarBordaGridView()
        //{
        //    gdvTipoPagamento.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
        //}

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if (
                (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador)) ||
                (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Estoquista)) ||
                (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Conferentista))
                )
            {
                imbInserirPedido.Visible = false;
            }
        }
    }
}