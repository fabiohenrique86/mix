using BLL;
using DAL;
using DAO;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Site
{
    public partial class Pedido : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UtilitarioBLL.SetarMascaraValor(Page);
                var reservaDAO = new ReservaDAO();

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
                                Session["dsDropDownListLoja"] = new LojaDAL().ListarDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
                                Session["bdLoja"] = null;
                            }

                            if ((Session["dsDropDownListFuncionario"] == null) || (Session["bdFuncionario"] != null))
                            {
                                Session["dsDropDownListFuncionario"] = FuncionarioDAL.ListarDropDownList(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
                                Session["bdFuncionario"] = null;
                            }
                            ddlFuncionario.DataSource = Session["dsDropDownListFuncionario"];
                            ddlFuncionario.DataBind();

                            if (Session["PedidoID"] != null)
                            {
                                string pedido = Session["PedidoID"].ToString();
                                if (!string.IsNullOrEmpty(pedido))
                                {
                                    reservaDAO.ReservaID = pedido;
                                    txtPedidoID.Text = pedido.ToUpper();
                                }
                            }

                            if (usuarioSessao.LojaID != 0)
                            {
                                reservaDAO.LojaOrigemID = usuarioSessao.LojaID;
                            }

                            if (Session["FuncionarioID"] != null)
                            {
                                int funcionario = Convert.ToInt32(Session["FuncionarioID"]);
                                if (funcionario > 0)
                                {
                                    reservaDAO.FuncionarioID = funcionario;
                                    ddlFuncionario.SelectedValue = funcionario.ToString();
                                }
                            }

                            if (Session["Cpf"] != null)
                            {
                                reservaDAO.Cpf = Session["Cpf"].ToString();
                                txtCpf.Text = Session["Cpf"].ToString();
                            }

                            if (Session["Cnpj"] != null)
                            {
                                reservaDAO.Cnpj = Session["Cnpj"].ToString();
                                txtCnpj.Text = Session["Cnpj"].ToString();
                            }

                            if (Session["DataEntrega"] != null)
                            {
                                DateTime dtEntrega = Convert.ToDateTime(Session["DataEntrega"]);
                                if (dtEntrega != DateTime.MinValue)
                                {
                                    reservaDAO.DataEntrega = dtEntrega;
                                    txtDataEntrega.Text = dtEntrega.ToString("dd/MM/yyyy");
                                }
                            }

                            if (Session["StatusID"] != null)
                            {
                                int status = Convert.ToInt32(Session["StatusID"]);
                                if (status > 0)
                                {
                                    reservaDAO.StatusID = status;
                                    ddlStatus.SelectedValue = status.ToString();
                                }
                            }

                            if (Session["SemDataEntrega"] != null)
                            {
                                bool semDataEntrega = Convert.ToBoolean(Session["SemDataEntrega"]);
                                ckbSemDataEntrega.Checked = semDataEntrega;
                            }

                            reservaDAO.SistemaID = usuarioSessao.SistemaID;

                            CarregarDados(reservaDAO);

                            Session["Filtro"] = null;
                            Session["PedidoID"] = null;
                            Session["LojaID"] = null;
                            Session["FuncionarioID"] = null;
                            Session["Cpf"] = null;
                            Session["Cnpj"] = null;
                            Session["DataEntrega"] = null;
                            Session["SemDataEntrega"] = null;
                        }
                        else
                        {
                            CarregarDropDownListLoja();
                            CarregarDropDownListFuncionario();
                            CarregarRepeaterReserva();
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

        private bool CarregarDados(ReservaDAO reservaDao)
        {
            ViewState["filtroReserva"] = true;
            rptReserva.DataSource = new ReservaDAL().ListarByFiltro(reservaDao);
            rptReserva.DataBind();
            return (rptReserva.Items.Count > 0);
        }

        private void CarregarDropDownListFuncionario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            if ((Session["dsDropDownListFuncionario"] == null) || (Session["bdFuncionario"] != null))
            {
                Session["dsDropDownListFuncionario"] = FuncionarioDAL.ListarDropDownList(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
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

            ddlLojaOrigem.DataSource = Session["dsDropDownListLoja"];
            ddlLojaOrigem.DataBind();

            ddlLojaSaida.DataSource = Session["dsDropDownListLoja"];
            ddlLojaSaida.DataBind();
        }

        private void CarregarRepeaterReserva()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            rptReserva.DataSource = new ReservaDAL().ListarByLoja(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
            rptReserva.DataBind();
        }

        protected void imbConsultar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ReservaDAO reservaDAO = new ReservaDAO();

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
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                    string pedidoId = string.Empty;
                    int lojaOrigemId = 0;
                    int funcionarioId = 0;
                    string cpfFormatado = txtCpf.Text.Trim().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                    string cnpjFormatado = txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                    DateTime dtEntrega = DateTime.MinValue;
                    int statusId = 0;
                    int lojaSaidaId = 0;
                    List<ProdutoDAO> listaProduto = new List<ProdutoDAO>();

                    pedidoId = txtPedidoID.Text.ToUpper().Trim();
                    int.TryParse(ddlFuncionario.SelectedValue, out funcionarioId);
                    int.TryParse(ddlStatus.SelectedValue, out statusId);
                    int.TryParse(ddlLojaSaida.SelectedValue, out lojaSaidaId);
                    DateTime.TryParse(txtDataEntrega.Text.Trim(), out dtEntrega);

                    if (usuarioSessao.LojaID != 0)
                    {
                        lojaOrigemId = usuarioSessao.LojaID;
                    }
                    else
                    {
                        int.TryParse(ddlLojaOrigem.SelectedValue, out lojaOrigemId);
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

                            listaProduto.Add(new ProdutoDAO(Convert.ToInt64(produtoId), Convert.ToInt16(0), sobMedida, Convert.ToDecimal(0), usuarioSessao.SistemaID));
                        }
                    }

                    if (listaProduto.Count() == 0 && string.IsNullOrEmpty(pedidoId) && statusId <= 0 && funcionarioId <= 0 && string.IsNullOrEmpty(cpfFormatado) && string.IsNullOrEmpty(cnpjFormatado) && dtEntrega == DateTime.MinValue && !ckbSemDataEntrega.Checked && lojaSaidaId <= 0 && lojaOrigemId <= 0)
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }

                    if (listaProduto.Count() > 0)
                    {
                        reservaDAO.ListaProduto = listaProduto;
                    }

                    reservaDAO.ReservaID = pedidoId;
                    reservaDAO.LojaOrigemID = lojaOrigemId;
                    reservaDAO.FuncionarioID = funcionarioId;
                    reservaDAO.Cpf = cpfFormatado;
                    reservaDAO.Cnpj = cnpjFormatado;
                    reservaDAO.DataEntrega = dtEntrega;
                    reservaDAO.StatusID = statusId;
                    reservaDAO.SistemaID = usuarioSessao.SistemaID;
                    reservaDAO.SemDataEntrega = ckbSemDataEntrega.Checked;
                    reservaDAO.LojaSaidaID = lojaSaidaId;

                    if (!CarregarDados(reservaDAO))
                    {
                        throw new ApplicationException("Pedido inexistente.");
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

        protected void imbFechar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                var reservaDAL = new ReservaDAL();

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
                    foreach (RepeaterItem item in rptReserva.Items)
                    {
                        var cbComanda = (CheckBox)item.FindControl("cbComanda");
                        if (cbComanda.Checked)
                        {
                            reservaDAL.AtualizarStatusImprimido(cbComanda.ToolTip, usuarioSessao.SistemaID, "R");
                        }
                    }

                    //BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                    //new ReservaDAL().AtualizarStatusImprimido(Session["PedidoID"].ToString(), usuarioSessao.SistemaID, "R");

                    int pedidoId;
                    int.TryParse(txtPedidoID.Text.ToUpper().Trim(), out pedidoId);

                    int funcionarioId;
                    int.TryParse(ddlFuncionario.SelectedValue, out funcionarioId);

                    string cpfFormatado = string.IsNullOrEmpty(txtCpf.Text.Trim().ToUpper().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCpf.Text.Trim().ToUpper().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                    string cnpjFormatado = string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")) ? null : txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");

                    int statusId;
                    int.TryParse(ddlStatus.SelectedValue, out statusId);

                    DateTime dtEntrega;
                    DateTime.TryParse(txtDataEntrega.Text.Trim(), out dtEntrega);

                    if (pedidoId > 0 || funcionarioId > 0 || !string.IsNullOrEmpty(cpfFormatado) || !string.IsNullOrEmpty(cnpjFormatado) || statusId > 0 || dtEntrega != DateTime.MinValue || ckbSemDataEntrega.Checked)
                    {
                        Session["Filtro"] = true;
                        Session["PedidoID"] = pedidoId;
                        Session["FuncionarioID"] = funcionarioId;
                        Session["Cpf"] = cpfFormatado;
                        Session["Cnpj"] = cnpjFormatado;
                        Session["DataEntrega"] = dtEntrega;
                        Session["StatusID"] = statusId;
                        Session["SemDataEntrega"] = ckbSemDataEntrega.Checked;
                    }
                    else
                    {
                        Session["Filtro"] = null;
                        Session["PedidoID"] = null;
                        Session["LojaID"] = null;
                        Session["FuncionarioID"] = null;
                        Session["Cpf"] = null;
                        Session["Cnpj"] = null;
                        Session["DataEntrega"] = null;
                        Session["StatusID"] = null;
                        Session["SemDataEntrega"] = null;
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

        protected void imbInserirReserva_Click(object sender, ImageClickEventArgs e)
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

                string pedidoId = string.Empty;
                int lojaOrigemId = 0;
                int funcionarioId = 0;
                string cpfFormatado = string.IsNullOrEmpty(txtCpf.Text.Trim().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCpf.Text.Trim().Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                string cnpjFormatado = string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")) ? null : txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                string dataEntrega = txtDataEntrega.Text.Trim().Replace("_", "").Replace("-", "").Replace("/", "");
                int sistemaId = usuarioSessao.SistemaID;
                int statusId = UtilitarioBLL.StatusEntregaReserva.Efetuada.GetHashCode();
                string nomeFantasia = string.Empty;
                DateTime dtEntrega;

                if (!string.IsNullOrEmpty(txtPedidoID.Text.ToUpper().Trim()))
                {
                    pedidoId = txtPedidoID.Text.ToUpper().Trim();
                }

                if (usuarioSessao.LojaID != 0)
                {
                    lojaOrigemId = usuarioSessao.LojaID;
                }

                if (ddlFuncionario.SelectedValue != "0")
                {
                    funcionarioId = Convert.ToInt32(ddlFuncionario.SelectedValue);
                }

                var listaProduto = new List<ProdutoDAO>();

                try
                {
                    if (((string.IsNullOrEmpty(pedidoId)) || ((funcionarioId <= 0) || string.IsNullOrEmpty(cpfFormatado))) && (string.IsNullOrEmpty(cnpjFormatado)))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para inserir pedido.");
                    }

                    var produtoDAL = new ProdutoDAL();
                    var ReservaDAO = new ReservaDAO();

                    if (!string.IsNullOrEmpty(cpfFormatado) && !string.IsNullOrEmpty(cnpjFormatado))
                    {
                        throw new ApplicationException("Informe CPF ou CNPJ para inserir pedido.");
                    }

                    if (!string.IsNullOrEmpty(cpfFormatado))
                    {
                        if (!new UtilsBLL().ValidarCpf(cpfFormatado))
                        {
                            throw new ApplicationException("CPF inválido.");
                        }
                        ReservaDAO.Cpf = cpfFormatado;
                    }
                    else if (!string.IsNullOrEmpty(cnpjFormatado))
                    {
                        if (!new UtilsBLL().ValidarCnpj(cnpjFormatado))
                        {
                            throw new ApplicationException("CNPJ inválido.");
                        }
                        ReservaDAO.Cnpj = cnpjFormatado;
                    }

                    // loop de produtos
                    for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
                    {
                        if (Request.Form["txtProdutoID_" + i] != null && Request.Form["txtQuantidade_" + i] != null && Request.Form["txtPreco_" + i] != null)
                        {
                            string produtoId = Request.Form.GetValues("txtProdutoID_" + i).FirstOrDefault();
                            string quantidade = Request.Form.GetValues("txtQuantidade_" + i).FirstOrDefault();
                            string preco = Request.Form.GetValues("txtPreco_" + i).FirstOrDefault();
                            string sobMedida = Request.Form.GetValues("txtSobMedida_" + i).FirstOrDefault();
                            int lojaSaidaId;
                            int.TryParse(Request.Form.GetValues("ddlLojaSaida_" + i).FirstOrDefault(), out lojaSaidaId);

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

                            if (lojaSaidaId > 0)
                            {
                                nomeFantasia = new LojaDAL().ListarById(lojaSaidaId.ToString(), sistemaId).Tables[0].Rows[0]["NomeFantasia"].ToString();

                                if (!produtoDAL.ExisteNaLoja(produtoId, lojaSaidaId.ToString(), sistemaId))
                                {
                                    throw new ApplicationException(string.Format("Produto {0} não está cadastrado na loja {1}.", produtoId, nomeFantasia));
                                }
                            }

                            listaProduto.Add(new ProdutoDAO()
                            {
                                ProdutoID = Convert.ToInt64(produtoId),
                                Quantidade = Convert.ToInt16(quantidade),
                                Medida = sobMedida,
                                Preco = Convert.ToDecimal(preco),
                                SistemaID = sistemaId,
                                LojaID = lojaSaidaId,
                                NomeFantasia = nomeFantasia
                            });

                            DateTime.TryParse(txtDataEntrega.Text.Trim(), out dtEntrega);

                            if (ckbSemDataEntrega.Checked)
                            {
                                statusId = UtilitarioBLL.StatusEntregaReserva.Pendente.GetHashCode();
                            }
                            else
                            {
                                if (lojaSaidaId <= 0 || nomeFantasia.Trim().ToLower().Equals("depósito"))
                                {
                                    if (nomeFantasia.Trim().ToLower().Equals("depósito") && dtEntrega == DateTime.MinValue)
                                    {
                                        throw new ApplicationException("Informe a data de entrega ou A PROGRAMAR para inserir pedido.");
                                    }

                                    if (dtEntrega != DateTime.MinValue)
                                    {
                                        ReservaDAO.DataEntrega = dtEntrega;
                                    }

                                    /// regra solicitada por Paulo em 06/01/2025:
                                    //  "se estoque <= 0" E "vendedor infringiu prazo de entrega definido por Paulo"
                                    //      pop up na tela informando o prazo mínimo de dias a programar
                                    //  "se estoque > 0"
                                    //      dias a programar livre definidos pelo Vendedor
                                    if (nomeFantasia.Trim().ToLower().Equals("depósito"))
                                    {
                                        var estoque = produtoDAL.ListarEstoqueProdutoLojaById(lojaSaidaId, Convert.ToInt64(produtoId), sistemaId);

                                        if (estoque <= 0)
                                        {
                                            var prazoDeEntrega = SistemaDAL.ListarPrazoDeEntrega(sistemaId);

                                            if (prazoDeEntrega != null)
                                            {
                                                if (ReservaDAO.DataEntrega.GetValueOrDefault() < DateTime.Today.AddDays(prazoDeEntrega.GetValueOrDefault()))
                                                {
                                                    throw new ApplicationException($"A saída do produto {produtoId} só poderá ser realizada a partir de {DateTime.Today.AddDays(prazoDeEntrega.GetValueOrDefault()).ToString("dd/MM/yyyy")}.\r\nO prazo cadastrado é de {prazoDeEntrega} dias.");
                                                }
                                            }
                                        }
                                    }

                                    statusId = UtilitarioBLL.StatusEntregaReserva.Pendente.GetHashCode();
                                }
                                else
                                {
                                    if (lojaSaidaId > 0)
                                    {
                                        if (dtEntrega == DateTime.MinValue)
                                        {
                                            ReservaDAO.DataEntrega = DateTime.Today;
                                        }
                                        else
                                        {
                                            ReservaDAO.DataEntrega = dtEntrega;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (listaProduto.Count <= 0)
                    {
                        throw new ApplicationException("Selecione ao menos um produto para inserir pedido.");
                    }

                    ReservaDAO.ReservaID = pedidoId;
                    ReservaDAO.LojaOrigemID = lojaOrigemId;
                    ReservaDAO.FuncionarioID = funcionarioId;
                    ReservaDAO.Observacao = txtObservacao.Text.Trim().ToUpper();
                    ReservaDAO.DataReserva = DateTime.Today;
                    ReservaDAO.SistemaID = sistemaId;
                    ReservaDAO.StatusID = statusId;
                    ReservaDAO.ListaProduto = listaProduto;
                    ReservaDAO.ValorFrete = Convert.ToDecimal(txtValorFrete.Text);

                    if (!string.IsNullOrEmpty(txtCV.Text))
                    {
                        ReservaDAO.CV = Convert.ToInt64(txtCV.Text);
                    }

                    //new ReservaBLL().Inserir(ReservaDAO);

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
                        ex.Data.Add("pedidoId", pedidoId);
                        ex.Data.Add("lojaOrigemId", lojaOrigemId);
                        ex.Data.Add("funcionarioId", funcionarioId);
                        ex.Data.Add("cpfFormatado", cpfFormatado);
                        ex.Data.Add("cnpjFormatado", cnpjFormatado);
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

                        UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
                    }
                }
            }
        }

        protected void imbGerarComanda_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var itemSelecionado = false;

                foreach (RepeaterItem item in rptReserva.Items)
                {
                    var cbComanda = (CheckBox)item.FindControl("cbComanda");
                    if (cbComanda.Checked)
                    {
                        var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                        var viewer = new ReportViewer();
                        viewer.Width = 735;
                        viewer.Height = 400;
                        viewer.AsyncRendering = false;
                        viewer.ProcessingMode = ProcessingMode.Local;
                        viewer.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "ComandaEntrega.rdlc";

                        var rdsComandaEntrega = new ReportDataSource
                        {
                            Name = "dsComandaEntrega_spListarComandaEntrega",
                            Value = new ReservaDAL().ListarComanda(cbComanda.ToolTip, usuarioSessao.SistemaID, "R", usuarioSessao.TipoUsuarioID)
                        };

                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(rdsComandaEntrega);
                        viewer.LocalReport.Refresh();

                        pnlComanda.Controls.Add(viewer);
                        itemSelecionado = true;
                    }
                }

                if (!itemSelecionado)
                {
                    UtilitarioBLL.ExibirMensagemAjax(Page, "É necessário selecionar algum item para gerar comanda");
                    return;
                }

                mpeComanda.Show();
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, "Ocorreu um erro ao gerar comanda. Tente novamente.", ex);
            }
        }

        private void LimparFormulario()
        {
            txtPedidoID.Text = string.Empty;
            ddlFuncionario.SelectedIndex = 0;
            txtCpf.Text = string.Empty;
            txtCnpj.Text = string.Empty;
            txtTotalPedido.Text = string.Empty;
            txtValorFrete.Text = "0,00";
            txtCV.Text = string.Empty;
            txtDataEntrega.Text = string.Empty;
            txtObservacao.Text = string.Empty;
            ckbSemDataEntrega.Checked = false;
            ddlStatus.SelectedIndex = 0;

            ScriptManager.RegisterStartupScript((Page)this, base.GetType(), "LimparGrid", "document.getElementById('ctl00_ContentPlaceHolder1_hdfProdutoValores').value = ''; clearGrid();", true);
        }

        //protected void lkbPedidoID_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Session["Usuario"] == null)
        //        {
        //            BLL.AplicacaoBLL.Empresa = null;

        //            if (base.Request.Url.Segments.Length == 3)
        //            {
        //                base.Response.Redirect("../Default.aspx", true);
        //            }
        //            else
        //            {
        //                base.Response.Redirect("Default.aspx", true);
        //            }
        //        }
        //        else
        //        {
        //            var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

        //            ReportViewer rpvComandaEntrega = new ReportViewer();
        //            rpvComandaEntrega.Width = 735;
        //            rpvComandaEntrega.Height = 400;
        //            rpvComandaEntrega.AsyncRendering = false;
        //            rpvComandaEntrega.ProcessingMode = ProcessingMode.Local;
        //            rpvComandaEntrega.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "ComandaEntrega.rdlc";
        //            var rdsTeste = new ReportDataSource
        //            {
        //                Name = "dsComandaEntrega_spListarComandaEntrega",
        //                Value = new ReservaDAL().ListarComanda(((LinkButton)sender).Text, usuarioSessao.SistemaID, "R", usuarioSessao.TipoUsuarioID)
        //            };

        //            rpvComandaEntrega.LocalReport.DataSources.Clear();
        //            rpvComandaEntrega.LocalReport.DataSources.Add(rdsTeste);
        //            rpvComandaEntrega.LocalReport.Refresh();

        //            //ReportViewer viewer = new ReportViewer();
        //            //viewer.Width = 735;
        //            //viewer.Height = 400;                    
        //            //viewer.AsyncRendering = false;
        //            //viewer.ProcessingMode = ProcessingMode.Local;
        //            //viewer.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "ComandaEntrega.rdlc";
        //            //var rdsTeste2 = new ReportDataSource
        //            //{
        //            //    Name = "dsComandaEntrega_spListarComandaEntrega",
        //            //    Value = new ReservaDAL().ListarComanda("A131650", usuarioSessao.SistemaID, "R", usuarioSessao.TipoUsuarioID)
        //            //};

        //            //viewer.LocalReport.DataSources.Clear();
        //            //viewer.LocalReport.DataSources.Add(rdsTeste2);
        //            //viewer.LocalReport.Refresh();

        //            //ReportViewer viewer2 = new ReportViewer();
        //            //viewer2.Width = 735;
        //            //viewer2.Height = 400;
        //            //viewer2.AsyncRendering = false;
        //            //viewer2.ProcessingMode = ProcessingMode.Local;
        //            //viewer2.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "ComandaEntrega.rdlc";
        //            //var rdsTeste3 = new ReportDataSource
        //            //{
        //            //    Name = "dsComandaEntrega_spListarComandaEntrega",
        //            //    Value = new ReservaDAL().ListarComanda("A131639", usuarioSessao.SistemaID, "R", usuarioSessao.TipoUsuarioID)
        //            //};

        //            //viewer2.LocalReport.DataSources.Clear();
        //            //viewer2.LocalReport.DataSources.Add(rdsTeste3);
        //            //viewer2.LocalReport.Refresh();

        //            //pnlComanda.Controls.Add(new Label() { Text = "Teste" });
        //            pnlComanda.Controls.Add(rpvComandaEntrega);
        //            //pnlComanda.Controls.Add(viewer);
        //            //pnlComanda.Controls.Add(viewer2);

        //            mpeComanda.Show();

        //            Session["PedidoID"] = ((LinkButton)sender).Text;
        //        }
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
        //    }
        //}

        protected void rptReserva_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                var reservaDAL = new ReservaDAL();
                var produtos = new List<ProdutoDAO>();

                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    var pedidoId = ((Label)e.Item.FindControl("lblPedidoID")).Text.ToUpper();
                    var repeaterReserva = (HtmlContainerControl)e.Item.FindControl("repeaterReserva");
                    var cbComanda = ((CheckBox)e.Item.FindControl("cbComanda"));

                    if
                    (
                        usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()
                        || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode()
                        || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Logistica.GetHashCode()
                    )
                    {
                        //((CheckBox)e.Item.FindControl("cbComanda")).Visible = true;
                        cbComanda.Visible = true;
                        pedidoId = ((CheckBox)e.Item.FindControl("cbComanda")).ToolTip;

                        //((LinkButton)e.Item.FindControl("lkbPedidoID")).Visible = true;
                        //pedidoId = ((LinkButton)e.Item.FindControl("lkbPedidoID")).Text;
                    }
                    else
                    {
                        cbComanda.Visible = false;
                        //((Label)e.Item.FindControl("lblPedidoID")).Visible = true;
                        pedidoId = ((Label)e.Item.FindControl("lblPedidoID")).Text;
                    }

                    //var lblClienteID = ((Label)e.Item.FindControl("lblClienteID"));

                    //if (lblClienteID != null)
                    //{
                    //    lblClienteID.ToolTip = "Teste de endereço de cliente";
                    //}

                    Label lblDataEntrega = (Label)e.Item.FindControl("lblDataEntregaID");

                    if (string.IsNullOrEmpty(lblDataEntrega.Text))
                    {
                        lblDataEntrega.Text = UtilitarioBLL.A_PROGRAMAR.ToString();
                    }

                    if (((pedidoId != txtPedidoID.Text.ToUpper()) && !string.IsNullOrEmpty(txtPedidoID.Text.ToUpper())) && (ViewState["filtroReserva"] != null))
                    {
                        repeaterReserva.Visible = false;
                    }
                    else
                    {
                        int statusId = Convert.ToInt32(((Label)e.Item.FindControl("lblStatusID")).Text);

                        if (statusId == UtilitarioBLL.StatusEntregaReserva.Efetuada.GetHashCode())
                        {
                            repeaterReserva.Style.Add("background-color", BLL.UtilitarioBLL.COR_STATUS_OK);
                        }
                        else if (statusId == UtilitarioBLL.StatusEntregaReserva.Transito.GetHashCode())
                        {
                            repeaterReserva.Style.Add("background-color", BLL.UtilitarioBLL.COR_STATUS_TRANSITO);
                        }
                        else // PENDENTE
                        {
                            repeaterReserva.Style.Add("background-color", BLL.UtilitarioBLL.COR_STATUS_PENDENTE);
                        }

                        var gdvReservaProdutoAux = (GridView)e.Item.FindControl("gdvReservaProduto");

                        gdvReservaProdutoAux.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);

                        for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
                        {
                            if (Request.Form["txtProdutoID_" + i] != null)
                            {
                                string produtoId = Request.Form.GetValues("txtProdutoID_" + i).FirstOrDefault();
                                string sobMedida = Request.Form.GetValues("txtSobMedida_" + i).FirstOrDefault();

                                produtos.Add(new ProdutoDAO(Convert.ToInt64(produtoId), Convert.ToInt16(0), sobMedida, Convert.ToDecimal(0), usuarioSessao.SistemaID));
                            }
                        }

                        if (Convert.ToBoolean(ViewState["filtroReserva"]))
                        {
                            int? lojaOrigemId = null;
                            int? funcionarioId = null;
                            string cpfFormatado = null;
                            string cnpjFormatado = null;
                            DateTime? dtReserva = null;
                            DateTime? dtEntrega = null;

                            if (Session["Filtro"] != null)
                            {
                                if (usuarioSessao.LojaID != 0)
                                {
                                    lojaOrigemId = usuarioSessao.LojaID;
                                }

                                if (Session["FuncionarioID"] != null)
                                {
                                    funcionarioId = Convert.ToInt32(ddlFuncionario.SelectedValue);
                                }
                                if (Session["Cpf"] != null)
                                {
                                    cpfFormatado = Session["Cpf"].ToString();
                                }
                                if (Session["Cnpj"] != null)
                                {
                                    cnpjFormatado = Session["Cnpj"].ToString();
                                }
                                if (Session["DataEntrega"] != null)
                                {
                                    dtEntrega = Convert.ToDateTime(Session["DataEntrega"]);
                                }

                                gdvReservaProdutoAux.DataSource = reservaDAL.ListarReservaLojaFiltro(pedidoId, lojaOrigemId, funcionarioId, cpfFormatado, cnpjFormatado, dtReserva, dtEntrega, usuarioSessao.SistemaID, usuarioSessao.TipoUsuarioID, produtos);
                                gdvReservaProdutoAux.DataBind();
                            }
                            else
                            {
                                if (usuarioSessao.LojaID != 0)
                                {
                                    lojaOrigemId = usuarioSessao.LojaID;
                                }
                                if (ddlFuncionario.SelectedValue != "0")
                                {
                                    funcionarioId = Convert.ToInt32(ddlFuncionario.SelectedValue);
                                }
                                if (!string.IsNullOrEmpty(txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")))
                                {
                                    cpfFormatado = txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                                }
                                if (!string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "")))
                                {
                                    cnpjFormatado = txtCnpj.Text.Replace(".", "").Replace("/", "").Replace("-", "").Replace(",", "").Replace("_", "");
                                }
                                if (!string.IsNullOrEmpty(txtDataEntrega.Text.Trim().ToUpper().Replace("_", "").Replace("-", "").Replace("/", "")))
                                {
                                    dtEntrega = Convert.ToDateTime(txtDataEntrega.Text.Trim());
                                }

                                gdvReservaProdutoAux.DataSource = reservaDAL.ListarReservaLojaFiltro(pedidoId, lojaOrigemId, funcionarioId, cpfFormatado, cnpjFormatado, dtReserva, dtEntrega, usuarioSessao.SistemaID, usuarioSessao.TipoUsuarioID, produtos);
                                gdvReservaProdutoAux.DataBind();
                            }
                        }
                        else
                        {
                            gdvReservaProdutoAux.DataSource = reservaDAL.ListarReservaLoja(usuarioSessao.LojaID, pedidoId, usuarioSessao.SistemaID, usuarioSessao.TipoUsuarioID);
                            gdvReservaProdutoAux.DataBind();
                        }

                        if (gdvReservaProdutoAux.Rows.Count <= 0)
                        {
                            repeaterReserva.Visible = false;
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

        protected void gdvReservaProduto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[5].Text == "True")
                    {
                        e.Row.Cells[5].Text = "Sim";
                        e.Row.Cells[5].ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        e.Row.Cells[5].Text = "Não";
                        e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
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

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if (
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()) ||
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode()) ||
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode())
                )
            {
                imbInserirReserva.Visible = false;
            }

            if (
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()) ||
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Logistica.GetHashCode()) ||
                (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode())
                )
            {
                imbGerarComanda.Visible = true;
            }
            else
            {
                imbGerarComanda.Visible = false;

                trLojaOrigem.Visible = false;
                trLojaSaida.Visible = false;
            }
        }

        protected void btnImprimirComanda_Click(object sender, EventArgs e)
        {
            try
            {
                Warning[] warnings;
                string mimeType;
                string[] streamids;
                string encoding;
                string filenameExtension;

                // Create a document for the merged result.
                TallComponents.PDF.Document mergedDocument = new TallComponents.PDF.Document();
                List<FileStream> streams = new List<FileStream>();

                foreach (RepeaterItem item in rptReserva.Items)
                {
                    var cbComanda = (CheckBox)item.FindControl("cbComanda");
                    if (cbComanda.Checked)
                    {
                        var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                        var viewer = new ReportViewer();

                        viewer.Width = 735;
                        viewer.Height = 400;
                        viewer.AsyncRendering = false;
                        viewer.ProcessingMode = ProcessingMode.Local;
                        viewer.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "ComandaEntrega.rdlc";

                        var rdsComandaEntrega = new ReportDataSource
                        {
                            Name = "dsComandaEntrega_spListarComandaEntrega",
                            Value = new ReservaDAL().ListarComanda(cbComanda.ToolTip, usuarioSessao.SistemaID, "R", usuarioSessao.TipoUsuarioID)
                        };

                        viewer.LocalReport.DataSources.Clear();
                        viewer.LocalReport.DataSources.Add(rdsComandaEntrega);
                        viewer.LocalReport.Refresh();

                        var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                        var arquivo = HostingEnvironment.ApplicationPhysicalPath + @"\pdfs\" + Guid.NewGuid().ToString() + ".pdf";

                        File.WriteAllBytes(arquivo, bytes);

                        // Open input stream and add to list of streams.
                        FileStream stream = new FileStream(arquivo, FileMode.Open, FileAccess.Read);
                        streams.Add(stream);

                        // Open input document.
                        TallComponents.PDF.Document document = new TallComponents.PDF.Document(stream);

                        mergedDocument.Pages.AddRange(document.Pages.CloneToArray());
                    }
                }

                var outputDocument = HostingEnvironment.ApplicationPhysicalPath + @"\pdfs\output.pdf";

                // Save merged document.
                using (FileStream output = new FileStream(outputDocument, FileMode.Create, FileAccess.Write))
                {
                    mergedDocument.Write(output);
                }

                // Close all input streams.
                streams.ForEach(stream => stream.Close());

                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=ComandaEntrega.pdf");
                Response.WriteFile(outputDocument);
                Response.Flush();

                // delete all generated files
                Array.ForEach(Directory.GetFiles(HostingEnvironment.ApplicationPhysicalPath + @"\pdfs\"), delegate (string path) { File.Delete(path); });
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, "Ocorreu um erro ao imprimir as comandas. Tente novamente.", ex);
            }
        }

        private byte[] Combine(params byte[][] arrays)
        {
            byte[] outputBytes = new byte[arrays[0].Length + arrays[1].Length];
            System.Buffer.BlockCopy(arrays[0], 0, outputBytes, 0, arrays[0].Length);
            System.Buffer.BlockCopy(arrays[1], 0, outputBytes, arrays[0].Length, arrays[1].Length);
            return outputBytes;

            //byte[] rv = new byte[arrays.Sum(a => a.Length)];
            //int offset = 0;
            //foreach (byte[] array in arrays)
            //{
            //    System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
            //    offset += array.Length;
            //}
            //return rv;
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static ReservaDAO ControleDeEntrega(string reservaId, string sistemaId)
        {
            var reservaDAO = new ReservaDAO();

            try
            {
                reservaDAO = new ReservaBLL().Listar(reservaId, Convert.ToInt32(sistemaId));

                reservaDAO.DataReservaFormatada = reservaDAO.DataReserva.GetValueOrDefault().ToString("dd/MM/yyyy");

                if (reservaDAO.DataEntrega.HasValue)
                {
                    reservaDAO.DataEntregaFormatada = reservaDAO.DataEntrega.GetValueOrDefault().ToString("dd/MM/yyyy");
                }

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