using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;
using BLL;
using DAO;
using DAL;

namespace Site
{
    public partial class Reserva : System.Web.UI.Page
    {
        private bool CarregarDados(DAO.Reserva reserva)
        {
            this.ViewState["filtroReserva"] = true;
            this.rptReserva.DataSource = new DAL.Reserva().ListarByFiltro(reserva);
            this.rptReserva.DataBind();
            return (this.rptReserva.Items.Count > 0);
        }

        private void CarregarDropDownListFuncionario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            if ((this.Session["dsDropDownListFuncionario"] == null) || (this.Session["bdFuncionario"] != null))
            {
                this.Session["dsDropDownListFuncionario"] = DAL.Funcionario.ListarDropDownList(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
                this.Session["bdFuncionario"] = null;
            }

            this.ddlFuncionario.DataSource = this.Session["dsDropDownListFuncionario"];
            this.ddlFuncionario.DataBind();
        }

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            if ((this.Session["dsDropDownListLoja"] == null) || (this.Session["bdLoja"] != null))
            {
                this.Session["dsDropDownListLoja"] = new DAL.Loja().ListarDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
                this.Session["bdLoja"] = null;
            }

            this.ddlLoja.DataSource = this.Session["dsDropDownListLoja"];
            this.ddlLoja.DataBind();
        }

        private void CarregarGridTipoPagamento()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            if ((this.Session["dsGridTipoPagamento"] == null) || (this.Session["bdTipoPagamento"] != null))
            {
                this.Session["dsGridTipoPagamento"] = DAL.TipoPagamento.Listar(usuarioSessao.SistemaID);
                this.Session["bdTipoPagamento"] = null;
            }

            this.gdvTipoPagamento.DataSource = this.Session["dsGridTipoPagamento"];
            this.gdvTipoPagamento.DataBind();
        }

        private void CarregarRepeaterReserva()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            this.rptReserva.DataSource = new DAL.Reserva().ListarByLoja(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID, true);
            this.rptReserva.DataBind();
        }

        protected void gdvTipoPagamento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlParcela = (DropDownList)e.Row.FindControl("ddlParcela");
                    ddlParcela.DataSource = DAL.Parcela.ListarDropDownList(usuarioSessao.SistemaID);
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
                DAO.Reserva reservaDAO = new DAO.Reserva();

                if (Session["Usuario"] == null)
                {
                    BLL.Aplicacao.Empresa = null;

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

                    int? pedidoId = null;
                    int? lojaId = null;
                    int? lojaOrigemId = null;
                    int? funcionarioId = null;
                    string cpfFormatado = string.IsNullOrEmpty(this.txtCpf.Text.Trim().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : this.txtCpf.Text.Trim().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                    string cnpjFormatado = string.IsNullOrEmpty(this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")) ? null : this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                    string dataReserva = this.txtDataReserva.Text.Trim().Replace("_", "").Replace("-", "").Replace("/", "");
                    string dataEntrega = this.txtDataEntrega.Text.Trim().Replace("_", "").Replace("-", "").Replace("/", "");
                    DateTime? dtReserva = null;
                    DateTime? dtEntrega = null;
                    List<DAO.Produto> listaProduto = new List<DAO.Produto>();

                    if (!string.IsNullOrEmpty(this.txtPedidoID.Text.Trim()))
                    {
                        pedidoId = Convert.ToInt32(this.txtPedidoID.Text.Trim());
                    }

                    if (this.ddlLoja.SelectedValue != "0")
                    {
                        lojaId = Convert.ToInt32(this.ddlLoja.SelectedValue);
                    }

                    if (usuarioSessao.LojaID != 0)
                    {
                        lojaOrigemId = usuarioSessao.LojaID;
                    }

                    if (this.ddlFuncionario.SelectedValue != "0")
                    {
                        funcionarioId = Convert.ToInt32(this.ddlFuncionario.SelectedValue);
                    }

                    for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
                    {
                        if (Request.Form["txtProdutoID_" + i] != null)
                        {
                            string produtoId = Request.Form.GetValues("txtProdutoID_" + i).FirstOrDefault();
                            string sobMedida = Request.Form.GetValues("txtSobMedida_" + i).FirstOrDefault();

                            if (string.IsNullOrEmpty(produtoId))
                            {
                                throw new ApplicationException(string.Format("Informe o produto {0}.", produtoId));
                            }

                            listaProduto.Add(new DAO.Produto(Convert.ToInt64(produtoId), Convert.ToInt16(0), sobMedida, Convert.ToDecimal(0), usuarioSessao.SistemaID));
                        }
                    }

                    if (((listaProduto.Count() == 0) && (!pedidoId.HasValue && !lojaId.HasValue) && (ddlStatus.SelectedValue == "0" || ddlStatus.SelectedValue == string.Empty) && (!funcionarioId.HasValue && string.IsNullOrEmpty(cpfFormatado))) && ((string.IsNullOrEmpty(cnpjFormatado) && string.IsNullOrEmpty(dataReserva)) && (string.IsNullOrEmpty(dataEntrega) && !this.ckbDataEntrega.Checked)))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }

                    if (listaProduto.Count() > 0)
                    {
                        reservaDAO.ListaProduto = listaProduto;
                    }

                    if (this.ckbDataEntrega.Checked && !string.IsNullOrEmpty(dataEntrega))
                    {
                        throw new ApplicationException("Informe a data da entrega ou sem data entrega para consultar.");
                    }

                    if (!string.IsNullOrEmpty(cpfFormatado) && !string.IsNullOrEmpty(cnpjFormatado))
                    {
                        throw new ApplicationException("Informe CPF ou CNPJ para para consultar.");
                    }

                    if (!string.IsNullOrEmpty(dataReserva))
                    {
                        if (!BLL.UtilitarioBLL.ValidarData(dataReserva))
                        {
                            throw new ApplicationException("DataReserva inválida.");
                        }

                        dtReserva = Convert.ToDateTime(this.txtDataReserva.Text.Trim());
                    }

                    if (!string.IsNullOrEmpty(dataEntrega))
                    {
                        if (!BLL.UtilitarioBLL.ValidarData(dataEntrega))
                        {
                            throw new ApplicationException("DataEntrega inválida.");
                        }

                        dtEntrega = Convert.ToDateTime(this.txtDataEntrega.Text.Trim());
                    }

                    if (!string.IsNullOrEmpty(ddlStatus.SelectedValue) && ddlStatus.SelectedValue != "0")
                    {
                        reservaDAO.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);
                    }

                    reservaDAO.ReservaID = pedidoId;
                    reservaDAO.LojaID = lojaId;
                    reservaDAO.LojaOrigemID = lojaOrigemId;
                    reservaDAO.FuncionarioID = funcionarioId;
                    reservaDAO.Cpf = cpfFormatado;
                    reservaDAO.Cnpj = cnpjFormatado;
                    reservaDAO.DataReserva = dtReserva;
                    reservaDAO.DataEntrega = dtEntrega;
                    reservaDAO.SemDataEntrega = this.ckbDataEntrega.Checked;
                    reservaDAO.SistemaID = usuarioSessao.SistemaID;

                    if (!this.CarregarDados(reservaDAO))
                    {
                        throw new ApplicationException("Reserva inexistente.");
                    }
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

        protected void imbFechar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    BLL.Aplicacao.Empresa = null;

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
                    new DAL.Reserva().AtualizarStatusImprimido(Convert.ToInt32(this.Session["PedidoID"]), usuarioSessao.SistemaID, "R");

                    int? pedidoId = null;
                    if (!string.IsNullOrEmpty(this.txtPedidoID.Text.Trim().ToUpper()))
                    {
                        pedidoId = Convert.ToInt32(this.txtPedidoID.Text.Trim());
                    }

                    int? lojaId = null;
                    if (this.ddlLoja.SelectedValue != "0")
                    {
                        lojaId = Convert.ToInt32(this.ddlLoja.SelectedValue);
                    }

                    int? funcionarioId = null;
                    if (this.ddlFuncionario.SelectedValue != "0")
                    {
                        funcionarioId = Convert.ToInt32(this.ddlFuncionario.SelectedValue);
                    }

                    string cpfFormatado = string.IsNullOrEmpty(this.txtCpf.Text.Trim().ToUpper().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : this.txtCpf.Text.Trim().ToUpper().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                    string cnpjFormatado = string.IsNullOrEmpty(this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")) ? null : this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                    string dataReserva = this.txtDataReserva.Text.Trim().ToUpper().Replace("_", "").Replace("-", "").Replace("/", "");
                    DateTime? dtReserva = null;
                    string dataEntrega = this.txtDataEntrega.Text.Trim().ToUpper().Replace("_", "").Replace("-", "").Replace("/", "");
                    DateTime? dtEntrega = null;
                    bool semDataEntrega = this.ckbDataEntrega.Checked;
                    string statusId = this.ddlStatus.SelectedValue;

                    if (((pedidoId.HasValue || lojaId.HasValue) || (statusId != "0") || (funcionarioId.HasValue || !string.IsNullOrEmpty(cpfFormatado))) || ((!string.IsNullOrEmpty(cnpjFormatado) || !string.IsNullOrEmpty(dataReserva)) || (!string.IsNullOrEmpty(dataEntrega) || semDataEntrega)))
                    {
                        this.Session["Filtro"] = true;
                        this.Session["PedidoID"] = pedidoId;
                        this.Session["LojaID"] = lojaId;
                        this.Session["FuncionarioID"] = funcionarioId;
                        this.Session["Cpf"] = cpfFormatado;
                        this.Session["Cnpj"] = cnpjFormatado;
                        this.Session["DataReserva"] = string.IsNullOrEmpty(dataReserva) ? dtReserva : Convert.ToDateTime(this.txtDataReserva.Text.Trim().ToUpper());
                        this.Session["DataEntrega"] = string.IsNullOrEmpty(dataEntrega) ? dtEntrega : Convert.ToDateTime(this.txtDataEntrega.Text.Trim().ToUpper());
                        this.Session["StatusID"] = statusId;
                        if (semDataEntrega)
                        {
                            this.Session["ckbDataEntrega"] = semDataEntrega;
                        }
                    }
                    else
                    {
                        this.Session["Filtro"] = null;
                        this.Session["PedidoID"] = null;
                        this.Session["LojaID"] = null;
                        this.Session["FuncionarioID"] = null;
                        this.Session["Cpf"] = null;
                        this.Session["Cnpj"] = null;
                        this.Session["DataReserva"] = null;
                        this.Session["DataEntrega"] = null;
                        this.Session["ckbDataEntrega"] = null;
                        this.Session["StatusID"] = null;
                    }

                    base.Response.Redirect("Reserva.aspx", false);
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

        protected void imbInserirReserva_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                BLL.Aplicacao.Empresa = null;

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

                int pedidoId = 0;
                int lojaId = 0;
                int lojaOrigemId = 0;
                int funcionarioId = 0;
                string cpfFormatado = string.IsNullOrEmpty(this.txtCpf.Text.Trim().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : this.txtCpf.Text.Trim().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                string cnpjFormatado = string.IsNullOrEmpty(this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")) ? null : this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                string dataReserva = this.txtDataReserva.Text.Trim().Replace("_", "").Replace("-", "").Replace("/", "");
                string dataEntrega = this.txtDataEntrega.Text.Trim().Replace("_", "").Replace("-", "").Replace("/", "");
                int sistemaId = usuarioSessao.SistemaID;

                if (!string.IsNullOrEmpty(this.txtPedidoID.Text.Trim()))
                {
                    pedidoId = Convert.ToInt32(this.txtPedidoID.Text.Trim());
                }

                if (this.ddlLoja.SelectedValue != "0")
                {
                    lojaId = Convert.ToInt32(this.ddlLoja.SelectedValue);
                }

                if (usuarioSessao.LojaID != 0)
                {
                    lojaOrigemId = usuarioSessao.LojaID;
                }

                if (this.ddlFuncionario.SelectedValue != "0")
                {
                    funcionarioId = Convert.ToInt32(this.ddlFuncionario.SelectedValue);
                }

                List<DAO.Produto> listaProduto = new List<DAO.Produto>();
                List<DAO.TipoPagamento> listaTipoPagamento = new List<DAO.TipoPagamento>();

                try
                {
                    if ((((pedidoId <= 0) || (lojaId <= 0)) || ((funcionarioId <= 0) || string.IsNullOrEmpty(cpfFormatado))) && (string.IsNullOrEmpty(cnpjFormatado) || string.IsNullOrEmpty(dataReserva)))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para inserir reserva.");
                    }

                    DAL.Produto produtoDAL = new DAL.Produto();
                    DAO.Reserva ReservaDAO = new DAO.Reserva();
                    DateTime dtReserva = Convert.ToDateTime(this.txtDataReserva.Text);
                    DateTime? dtEntrega = null;

                    if (!this.ckbDataEntrega.Checked)
                    {
                        if (string.IsNullOrEmpty(dataEntrega))
                        {
                            throw new ApplicationException("É necessário informar todos os campos obrigatórios para inserir reserva.");
                        }
                        dtEntrega = Convert.ToDateTime(this.txtDataEntrega.Text);
                    }
                    else if (!string.IsNullOrEmpty(dataEntrega))
                    {
                        throw new ApplicationException("Informar um dos campos : data da entrega ou sem data entrega.");
                    }

                    if (!string.IsNullOrEmpty(cpfFormatado) && !string.IsNullOrEmpty(cnpjFormatado))
                    {
                        throw new ApplicationException("Informe CPF ou CNPJ para inserir reserva.");
                    }

                    if (!string.IsNullOrEmpty(cpfFormatado))
                    {
                        if (!new BLL.UtilsBLL().ValidarCpf(cpfFormatado))
                        {
                            throw new ApplicationException("CPF inválido.");
                        }
                        ReservaDAO.Cpf = cpfFormatado;
                    }
                    else if (!string.IsNullOrEmpty(cnpjFormatado))
                    {
                        if (!new BLL.UtilsBLL().ValidarCnpj(cnpjFormatado))
                        {
                            throw new ApplicationException("CNPJ inválido.");
                        }
                        ReservaDAO.Cnpj = cnpjFormatado;
                    }
                    if (!this.txtTotalPedido.Text.Equals(this.txtTotalPago.Text))
                    {
                        throw new ApplicationException("Total Pedido deve igual ao Total Pago.");
                    }

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
                                throw new ApplicationException(string.Format("Produto {0} não está cadastrado na loja {1}.", produtoId, this.ddlLoja.SelectedItem.Text));
                            }

                            listaProduto.Add(new DAO.Produto(Convert.ToInt64(produtoId), Convert.ToInt16(quantidade), sobMedida, Convert.ToDecimal(preco), sistemaId));
                        }
                    }

                    if (listaProduto.Count <= 0)
                    {
                        throw new ApplicationException("Selecione ao menos um produto para inserir reserva.");
                    }

                    foreach (GridViewRow gdrTipoPagamento in this.gdvTipoPagamento.Rows)
                    {
                        if (((CheckBox)gdrTipoPagamento.FindControl("ckbTipoPagamento")).Checked)
                        {
                            string tipoPagamento = ((Label)gdrTipoPagamento.FindControl("lblTipoPagamento")).Text;
                            string tipoPagamentoId = ((Label)gdrTipoPagamento.FindControl("lblTipoPagamentoID")).Text;
                            DropDownList ddlParcela = (DropDownList)gdrTipoPagamento.FindControl("ddlParcela");

                            if (ddlParcela.SelectedIndex == 0)
                            {
                                throw new ApplicationException("Informe a Parcela do Tipo de Pagamento:\r\n\r\n" + tipoPagamento);
                            }

                            string parcelaId = ddlParcela.SelectedValue;
                            string valorPago = ((TextBox)gdrTipoPagamento.FindControl("txtValorPago")).Text;

                            if (string.IsNullOrEmpty(valorPago) || (valorPago == "0,00"))
                            {
                                throw new ApplicationException("Informe o Valor Pago do Tipo de Pagamento:\r\n\r\n" + tipoPagamento);
                            }

                            listaTipoPagamento.Add(new DAO.TipoPagamento(Convert.ToInt32(tipoPagamentoId), Convert.ToInt32(parcelaId), Convert.ToDecimal(valorPago), sistemaId));
                        }
                    }

                    if (listaTipoPagamento.Count <= 0)
                    {
                        throw new ApplicationException("Selecione ao menos um tipo de pagamento para inserir reserva.");
                    }

                    ReservaDAO.ReservaID = pedidoId;
                    ReservaDAO.LojaID = lojaId;
                    ReservaDAO.LojaOrigemID = lojaOrigemId;
                    ReservaDAO.FuncionarioID = funcionarioId;
                    ReservaDAO.Observacao = this.txtObservacao.Text.Trim().ToUpper();
                    ReservaDAO.DataReserva = dtReserva;
                    ReservaDAO.DataEntrega = dtEntrega;
                    ReservaDAO.SistemaID = sistemaId;
                    ReservaDAO.StatusID = UtilitarioBLL.StatusEntregaReserva.Pendente.GetHashCode();
                    ReservaDAO.ListaProduto = listaProduto;
                    ReservaDAO.ListaTipoPagamento = listaTipoPagamento;
                    ReservaDAO.ValorFrete = Convert.ToDecimal(txtValorFrete.Text);

                    if (!string.IsNullOrEmpty(txtCV.Text))
                    {
                        ReservaDAO.CV = Convert.ToInt64(txtCV.Text);
                    }                    

                    new BLL.ReservaBLL().Inserir(ReservaDAO);

                    this.CarregarRepeaterReserva();
                    this.LimparFormulario();
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
                    if (ex.Source.ToUpper() == "SYSTEM.DATA")
                    {
                        UtilitarioBLL.ExibirMensagemAjax(this.Page, "Data inválida.");
                    }
                    else
                    {
                        ex.Data.Add("pedidoId", pedidoId);
                        ex.Data.Add("lojaId", lojaId);
                        ex.Data.Add("lojaOrigemId", lojaOrigemId);
                        ex.Data.Add("funcionarioId", funcionarioId);
                        ex.Data.Add("cpfFormatado", cpfFormatado);
                        ex.Data.Add("cnpjFormatado", cnpjFormatado);
                        ex.Data.Add("dataReserva", dataReserva);
                        ex.Data.Add("dataEntrega", dataEntrega);
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

                        if (listaTipoPagamento != null && listaTipoPagamento.Count > 0)
                        {
                            foreach (var item in listaTipoPagamento)
                            {
                                ex.Data.Add("TipoPagamentoID_" + Guid.NewGuid().ToString(), item.TipoPagamentoID);
                                ex.Data.Add("Descricao_" + Guid.NewGuid().ToString(), item.Descricao);
                                ex.Data.Add("ParcelaID_" + Guid.NewGuid().ToString(), item.ParcelaID);
                                ex.Data.Add("Valor_" + Guid.NewGuid().ToString(), item.Valor);
                                ex.Data.Add("SistemaID_" + Guid.NewGuid().ToString(), item.SistemaID);
                            }
                        }

                        UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message, ex);
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
            txtValorFrete.Text = "0,00";
            txtCV.Text = string.Empty;

            foreach (GridViewRow gdrTipoPagamento in this.gdvTipoPagamento.Rows)
            {
                ((CheckBox)gdrTipoPagamento.FindControl("ckbTipoPagamento")).Checked = false;
                if (!((Label)gdrTipoPagamento.FindControl("lblTipoPagamento")).Text.ToUpper().Contains("DINHEIRO"))
                {
                    ((DropDownList)gdrTipoPagamento.FindControl("ddlParcela")).SelectedIndex = 0;
                }
                ((TextBox)gdrTipoPagamento.FindControl("txtValorPago")).Text = string.Empty;
            }

            txtTotalPago.Text = string.Empty;
            txtDataReserva.Text = string.Empty;
            txtDataEntrega.Text = string.Empty;
            txtObservacao.Text = string.Empty;
            ckbDataEntrega.Checked = false;

            ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "LimparGrid", "document.getElementById('ctl00_ContentPlaceHolder1_hdfProdutoValores').value = ''; clearGrid();", true);
        }

        protected void lkbDetalhe_Click(object sender, EventArgs e)
        {
            try
            {
                this.Session["PedidoDetalheID"] = ((LinkButton)sender).CommandArgument;
                ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "PedidoDetalhe", "var win = window.open('PedidoDetalhe.aspx','_blank','width=625,height=400'); win.moveTo(((screen.width - 625)/2),((screen.height - 400)/2));", true);
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

        protected void lkbPedidoID_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    BLL.Aplicacao.Empresa = null;

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
                    this.rpvComandaEntrega.ProcessingMode = ProcessingMode.Local;
                    this.rpvComandaEntrega.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "ComandaEntrega.rdlc";
                    ReportDataSource rdsTeste = new ReportDataSource
                    {
                        Name = "dsComandaEntrega_spListarComandaEntrega",
                        Value = new DAL.Reserva().ListarComanda(Convert.ToInt32(((LinkButton)sender).Text), usuarioSessao.SistemaID, "R")
                    };
                    this.rpvComandaEntrega.LocalReport.DataSources.Clear();
                    this.rpvComandaEntrega.LocalReport.DataSources.Add(rdsTeste);
                    this.rpvComandaEntrega.LocalReport.Refresh();
                    this.mpeComanda.Show();
                    this.Session["PedidoID"] = ((LinkButton)sender).Text;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UtilitarioBLL.SetarMascaraValor(this.Page);
                DAO.Reserva reservaDAO = new DAO.Reserva();

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
                        this.VisualizarFormulario();
                        if (this.Session["Filtro"] != null)
                        {
                            if ((this.Session["dsDropDownListLoja"] == null) || (this.Session["bdLoja"] != null))
                            {
                                this.Session["dsDropDownListLoja"] = new DAL.Loja().ListarDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
                                this.Session["bdLoja"] = null;
                            }
                            this.ddlLoja.DataSource = this.Session["dsDropDownListLoja"];
                            this.ddlLoja.DataBind();

                            if ((this.Session["dsDropDownListFuncionario"] == null) || (this.Session["bdFuncionario"] != null))
                            {
                                this.Session["dsDropDownListFuncionario"] = DAL.Funcionario.ListarDropDownList(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
                                this.Session["bdFuncionario"] = null;
                            }
                            this.ddlFuncionario.DataSource = this.Session["dsDropDownListFuncionario"];
                            this.ddlFuncionario.DataBind();

                            if ((this.Session["dsGridTipoPagamento"] == null) || (this.Session["bdTipoPagamento"] != null))
                            {
                                this.Session["dsGridTipoPagamento"] = DAL.TipoPagamento.Listar(usuarioSessao.SistemaID);
                                this.Session["bdTipoPagamento"] = null;
                            }
                            this.gdvTipoPagamento.DataSource = this.Session["dsGridTipoPagamento"];
                            this.gdvTipoPagamento.DataBind();

                            if (this.Session["PedidoID"] != null)
                            {
                                reservaDAO.ReservaID = Convert.ToInt32(this.Session["PedidoID"]);
                                this.txtPedidoID.Text = Convert.ToInt32(this.Session["PedidoID"]).ToString();
                            }

                            if (this.Session["LojaID"] != null)
                            {
                                reservaDAO.LojaID = Convert.ToInt32(this.Session["LojaID"]);
                                this.ddlLoja.SelectedValue = Convert.ToInt32(this.Session["LojaID"]).ToString();
                            }

                            if (usuarioSessao.LojaID != 0)
                            {
                                reservaDAO.LojaOrigemID = usuarioSessao.LojaID;
                            }

                            if (this.Session["FuncionarioID"] != null)
                            {
                                reservaDAO.FuncionarioID = Convert.ToInt32(this.Session["FuncionarioID"]);
                                this.ddlFuncionario.SelectedValue = Convert.ToInt32(this.Session["FuncionarioID"]).ToString();
                            }

                            if (this.Session["Cpf"] != null)
                            {
                                reservaDAO.Cpf = this.Session["Cpf"].ToString();
                                this.txtCpf.Text = this.Session["Cpf"].ToString();
                            }

                            if (this.Session["Cnpj"] != null)
                            {
                                reservaDAO.Cnpj = this.Session["Cnpj"].ToString();
                                this.txtCnpj.Text = this.Session["Cnpj"].ToString();
                            }

                            if (this.Session["DataReserva"] != null)
                            {
                                reservaDAO.DataReserva = Convert.ToDateTime(this.Session["DataReserva"]);
                                this.txtDataReserva.Text = Convert.ToDateTime(this.Session["DataReserva"]).ToString();
                            }

                            if (this.Session["DataEntrega"] != null)
                            {
                                reservaDAO.DataEntrega = Convert.ToDateTime(this.Session["DataEntrega"]);
                                this.txtDataEntrega.Text = Convert.ToDateTime(this.Session["DataEntrega"]).ToString();
                            }

                            if (this.Session["ckbDataEntrega"] != null)
                            {
                                reservaDAO.SemDataEntrega = Convert.ToBoolean(this.Session["ckbDataEntrega"]);
                                this.ckbDataEntrega.Checked = Convert.ToBoolean(this.Session["ckbDataEntrega"]);
                            }

                            if (this.Session["StatusID"] != null)
                            {
                                reservaDAO.StatusID = Convert.ToInt32(this.Session["StatusID"]);
                                this.ddlStatus.SelectedValue = this.Session["StatusID"].ToString();
                            }

                            reservaDAO.SistemaID = usuarioSessao.SistemaID;

                            this.CarregarDados(reservaDAO);

                            this.Session["Filtro"] = null;
                            this.Session["PedidoID"] = null;
                            this.Session["LojaID"] = null;
                            this.Session["FuncionarioID"] = null;
                            this.Session["Cpf"] = null;
                            this.Session["Cnpj"] = null;
                            this.Session["DataReserva"] = null;
                            this.Session["DataEntrega"] = null;
                            this.Session["ckbDataEntrega"] = null;
                        }
                        else
                        {
                            this.CarregarDropDownListLoja();
                            this.CarregarDropDownListFuncionario();
                            this.CarregarGridTipoPagamento();
                            this.CarregarRepeaterReserva();
                        }
                        this.SetarBordaGridView();
                    }
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

        protected void rptReserva_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                DAL.Reserva reservaDAL = new DAL.Reserva();
                List<DAO.Produto> listaProduto = new List<DAO.Produto>();

                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    string pedidoId = ((Label)e.Item.FindControl("lblPedidoID")).Text;
                    if ((usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador)) || (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Estoquista)))
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
                    Label lblDataEntrega = (Label)e.Item.FindControl("lblDataEntregaID");
                    if (string.IsNullOrEmpty(lblDataEntrega.Text))
                    {
                        lblDataEntrega.Text = UtilitarioBLL.A_PROGRAMAR;
                    }
                    if (((pedidoId != this.txtPedidoID.Text) && !string.IsNullOrEmpty(this.txtPedidoID.Text)) && (this.ViewState["filtroReserva"] != null))
                    {
                        e.Item.FindControl("repeaterReserva").Visible = false;
                    }
                    else
                    {
                        GridView gdvReservaProdutoAux = (GridView)e.Item.FindControl("gdvReservaProduto");

                        gdvReservaProdutoAux.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);

                        for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
                        {
                            if (Request.Form["txtProdutoID_" + i] != null)
                            {
                                string produtoId = Request.Form.GetValues("txtProdutoID_" + i).FirstOrDefault();
                                string sobMedida = Request.Form.GetValues("txtSobMedida_" + i).FirstOrDefault();

                                listaProduto.Add(new DAO.Produto(Convert.ToInt64(produtoId), Convert.ToInt16(0), sobMedida, Convert.ToDecimal(0), usuarioSessao.SistemaID));
                            }
                        }

                        if (Convert.ToBoolean(this.ViewState["filtroReserva"]))
                        {
                            int? lojaId = null;
                            int? lojaOrigemId = null;
                            int? funcionarioId = null;
                            string cpfFormatado = null;
                            string cnpjFormatado = null;
                            DateTime? dtReserva = null;
                            DateTime? dtEntrega = null;
                            bool semDataEntrega = false;
                            int statusId = 0;

                            if (this.Session["Filtro"] != null)
                            {
                                if (this.Session["LojaID"] != null)
                                {
                                    lojaId = Convert.ToInt32(this.Session["LojaID"]);
                                }
                                if (usuarioSessao.LojaID != 0)
                                {
                                    lojaOrigemId = usuarioSessao.LojaID;
                                }
                                if (this.Session["FuncionarioID"] != null)
                                {
                                    funcionarioId = Convert.ToInt32(this.ddlFuncionario.SelectedValue);
                                }
                                if (this.Session["Cpf"] != null)
                                {
                                    cpfFormatado = this.Session["Cpf"].ToString();
                                }
                                if (this.Session["Cnpj"] != null)
                                {
                                    cnpjFormatado = this.Session["Cnpj"].ToString();
                                }
                                if (this.Session["DataReserva"] != null)
                                {
                                    dtReserva = Convert.ToDateTime(this.Session["DataReserva"]);
                                }
                                if (this.Session["DataEntrega"] != null)
                                {
                                    dtEntrega = Convert.ToDateTime(this.Session["DataEntrega"]);
                                }
                                if (this.Session["ckbDataEntrega"] != null)
                                {
                                    semDataEntrega = Convert.ToBoolean(this.Session["ckbDataEntrega"]);
                                }

                                if (this.Session["StatusID"] != null)
                                {
                                    statusId = Convert.ToInt32(this.Session["StatusID"]);
                                }

                                gdvReservaProdutoAux.DataSource = reservaDAL.Listar(Convert.ToInt32(pedidoId), lojaId, lojaOrigemId, funcionarioId, cpfFormatado, cnpjFormatado, dtReserva, dtEntrega, semDataEntrega, usuarioSessao.SistemaID, listaProduto);
                                gdvReservaProdutoAux.DataBind();
                            }
                            else
                            {
                                if (this.ddlLoja.SelectedValue != "0")
                                {
                                    lojaId = Convert.ToInt32(this.ddlLoja.SelectedValue);
                                }
                                if (usuarioSessao.LojaID != 0)
                                {
                                    lojaOrigemId = usuarioSessao.LojaID;
                                }
                                if (this.ddlFuncionario.SelectedValue != "0")
                                {
                                    funcionarioId = Convert.ToInt32(this.ddlFuncionario.SelectedValue);
                                }
                                if (!string.IsNullOrEmpty(this.txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")))
                                {
                                    cpfFormatado = this.txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                                }
                                if (!string.IsNullOrEmpty(this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")))
                                {
                                    cnpjFormatado = this.txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                                }
                                if (!string.IsNullOrEmpty(this.txtDataReserva.Text.Trim().ToUpper().Replace("_", "").Replace("-", "").Replace("/", "")))
                                {
                                    dtReserva = Convert.ToDateTime(this.txtDataReserva.Text.Trim());
                                }
                                if (!string.IsNullOrEmpty(this.txtDataEntrega.Text.Trim().ToUpper().Replace("_", "").Replace("-", "").Replace("/", "")))
                                {
                                    dtEntrega = Convert.ToDateTime(this.txtDataEntrega.Text.Trim());
                                }

                                semDataEntrega = this.ckbDataEntrega.Checked;

                                gdvReservaProdutoAux.DataSource = reservaDAL.Listar(Convert.ToInt32(pedidoId), lojaId, lojaOrigemId, funcionarioId, cpfFormatado, cnpjFormatado, dtReserva, dtEntrega, this.ckbDataEntrega.Checked, usuarioSessao.SistemaID, listaProduto);
                                gdvReservaProdutoAux.DataBind();
                            }
                        }
                        else
                        {
                            gdvReservaProdutoAux.DataSource = reservaDAL.Listar(usuarioSessao.LojaID, pedidoId, usuarioSessao.SistemaID);
                            gdvReservaProdutoAux.DataBind();
                        }
                        if (gdvReservaProdutoAux.Rows.Count <= 0)
                        {
                            e.Item.FindControl("repeaterReserva").Visible = false;
                        }
                    }
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

        private void SetarBordaGridView()
        {
            this.gdvTipoPagamento.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if (
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()) ||
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode()) ||
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode())
                )
            {
                this.imbInserirReserva.Visible = false;
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static DAO.Reserva Teste(string reservaId, string sistemaId)
        {
            var reservaDAO = new DAO.Reserva();

            try
            {
                reservaDAO = new BLL.ReservaBLL().Listar(Convert.ToInt32(reservaId), Convert.ToInt32(sistemaId));

                reservaDAO.DataReservaFormatada = reservaDAO.DataReserva.GetValueOrDefault().ToString("dd/MM/yyyy");
                reservaDAO.DataEntregaFormatada = reservaDAO.DataEntrega.GetValueOrDefault().ToString("dd/MM/yyyy");

                return reservaDAO;
            }
            catch (ApplicationException ex)
            {
                return reservaDAO;
            }
            catch (Exception ex)
            {
                return reservaDAO;
            }
        }        
    }
}