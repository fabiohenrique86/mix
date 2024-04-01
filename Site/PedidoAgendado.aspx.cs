using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BLL;
using DAO;
using System.Collections.Generic;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;

namespace Site
{
    public partial class PedidoAgendado : Page
    {
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
                    else if (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == UtilitarioBLL.TipoUsuario.Vendedor.GetHashCode())
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
                        CarregarDropDownListLojaGeral();
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

        private bool CarregarDados(bool validar)
        {
            var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            string reservaId = txtReservaID.Text.Trim();

            DateTime dtReservaInicio;
            DateTime.TryParse(txtDataReservaInicio.Text.Trim(), out dtReservaInicio);

            DateTime dtReservaFim;
            DateTime.TryParse(txtDataReservaFim.Text.Trim(), out dtReservaFim);

            DateTime dtEntregaInicio;
            DateTime.TryParse(txtDataEntregaInicio.Text.Trim(), out dtEntregaInicio);

            DateTime dtEntregaFim;
            DateTime.TryParse(txtDataEntregaFim.Text.Trim(), out dtEntregaFim);

            //int lojaId = 0;

            if (validar)
            {
                if (string.IsNullOrEmpty(reservaId)
                    && dtReservaInicio == DateTime.MinValue
                    && dtReservaFim == DateTime.MinValue
                    && dtEntregaInicio == DateTime.MinValue
                    && dtEntregaFim == DateTime.MinValue
                    && !ckbSemDataEntrega.Checked
                    && string.IsNullOrEmpty(txtNomeCarreto.Text.Trim())
                    && ddlStatus.SelectedIndex <= 0)
                {
                    throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                }
            }

            // Se estoquista, lista reservas do deposito
            // Se houver loja associada, lista somente reservas da loja associada
            //if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode())
            //{
            //    var loja_deposito = new LojaBLL().Obter(new LojaDAO() { CNPJ = "00000000000000", SistemaID = usuarioSessao.SistemaID });

            //    if (loja_deposito != null)
            //    {
            //        lojaId = loja_deposito.LojaID;
            //    }
            //}
            //else if (usuarioSessao.LojaID > 0)
            //{
            //    lojaId = usuarioSessao.LojaID;
            //}

            rptReserva.DataSource = new DAL.ReservaDAL().ListarReservaAgendada(reservaId, dtReservaInicio, dtReservaFim, dtEntregaInicio, dtEntregaFim, txtNomeCarreto.Text.Trim(), Convert.ToInt32(ddlStatus.SelectedValue), usuarioSessao.LojaID, usuarioSessao.SistemaID, ckbSemDataEntrega.Checked);
            rptReserva.DataBind();

            return (rptReserva.Items.Count > 0);
        }

        protected void rptReserva_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    int statusId = Convert.ToInt32(((Label)e.Item.FindControl("lblStatusID")).Text);

                    var repeaterReserva = (HtmlContainerControl)e.Item.FindControl("repeaterReserva");

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

                    string reservaId = ((Label)e.Item.FindControl("lblReservaID")).Text;

                    Label lblDataEntrega = (Label)e.Item.FindControl("lblDataEntrega");

                    if (string.IsNullOrEmpty(lblDataEntrega.Text))
                    {
                        lblDataEntrega.Text = UtilitarioBLL.A_PROGRAMAR.ToString();
                    }

                    GridView gdvReservaProduto = (GridView)e.Item.FindControl("gdvReservaProduto");

                    gdvReservaProduto.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);

                    int? lojaId = null;

                    if (usuarioSessao.LojaID > 0)
                    {
                        lojaId = usuarioSessao.LojaID;
                    }

                    gdvReservaProduto.DataSource = new DAL.ReservaDAL().ListarReservaLojaFiltro(reservaId, lojaId, null, null, null, null, null, usuarioSessao.SistemaID, usuarioSessao.TipoUsuarioID, null);
                    gdvReservaProduto.DataBind();

                    if (gdvReservaProduto.Rows.Count <= 0)
                    {
                        e.Item.FindControl("repeaterReserva").Visible = false;
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
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlLojaSaida = (DropDownList)e.Row.FindControl("ddlLojaSaida");
                    Label lblLojaID = (Label)e.Row.FindControl("lblLojaID");
                    Label lblNomeFantasia = (Label)e.Row.FindControl("lblNomeFantasia");
                    ImageButton imbDarBaixa = (ImageButton)e.Row.FindControl("imbDarBaixa");
                    bool baixa = false;
                    bool.TryParse(((Label)e.Row.FindControl("lblBaixa")).Text, out baixa);

                    if (baixa)
                    {
                        lblNomeFantasia.Visible = true;
                        ddlLojaSaida.Visible = false;
                        imbDarBaixa.Visible = false;
                    }
                    else
                    {
                        lblNomeFantasia.Visible = false;
                        ddlLojaSaida.Visible = true;
                        imbDarBaixa.Visible = true;

                        List<LojaDAO> lojas = new List<LojaDAO>();

                        if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode())
                        {
                            lojas.Add(new LojaDAO() { LojaID = 0, NomeFantasia = "SELECIONE" });
                            lojas.Add(new LojaBLL().Obter(new LojaDAO() { CNPJ = "00000000000000", SistemaID = usuarioSessao.SistemaID }));

                            ddlLojaSaida.DataSource = lojas;
                        }
                        else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode())
                        {
                            lojas.Add(new LojaDAO() { LojaID = 0, NomeFantasia = "SELECIONE" });
                            lojas.Add(new LojaDAO() { LojaID = usuarioSessao.LojaID, Nome = usuarioSessao.NomeLoja, NomeFantasia = usuarioSessao.NomeLoja });

                            ddlLojaSaida.DataSource = lojas;
                        }
                        else
                        {
                            if ((Session["dsDropDownListLoja"] == null) || (Session["bdLoja"] != null))
                            {
                                Session["dsDropDownListLoja"] = new DAL.LojaDAL().ListarDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
                                Session["bdLoja"] = null;
                            }

                            ddlLojaSaida.DataSource = Session["dsDropDownListLoja"];
                        }

                        ddlLojaSaida.DataBind();
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

        private void CarregarDropDownListLojaGeral()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            List<LojaDAO> lojas = new List<LojaDAO>();

            if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode())
            {
                lojas.Add(new LojaDAO() { LojaID = 0, NomeFantasia = "SELECIONE" });
                lojas.Add(new LojaBLL().Obter(new LojaDAO() { CNPJ = "00000000000000", SistemaID = usuarioSessao.SistemaID }));

                ddlLojaSaidaGeral.DataSource = lojas;
            }
            else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode())
            {
                lojas.Add(new LojaDAO() { LojaID = 0, NomeFantasia = "SELECIONE" });
                lojas.Add(new LojaDAO() { LojaID = usuarioSessao.LojaID, Nome = usuarioSessao.NomeLoja, NomeFantasia = usuarioSessao.NomeLoja });

                ddlLojaSaidaGeral.DataSource = lojas;
            }
            else
            {
                if ((Session["dsDropDownListLoja"] == null) || (Session["bdLoja"] != null))
                {
                    Session["dsDropDownListLoja"] = new DAL.LojaDAL().ListarDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
                    Session["bdLoja"] = null;
                }

                ddlLojaSaidaGeral.DataSource = Session["dsDropDownListLoja"];
            }

            ddlLojaSaidaGeral.DataBind();
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
                    if (!CarregarDados(true))
                    {
                        throw new ApplicationException("Pedido inexistente.");
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

        protected void imbDarBaixa_Click(object sender, ImageClickEventArgs e)
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
                    DAL.ReservaDAL reservaDAL = new DAL.ReservaDAL();
                    ImageButton imbDarBaixa = (ImageButton)sender;

                    string reservaId = ((Label)imbDarBaixa.Parent.FindControl("lblPedidoID")).Text;

                    int lojaSaidaId;
                    int.TryParse(((DropDownList)imbDarBaixa.Parent.FindControl("ddlLojaSaida")).SelectedValue, out lojaSaidaId);

                    long produtoId;
                    long.TryParse(((Label)imbDarBaixa.Parent.FindControl("lblProdutoID")).Text, out produtoId);

                    int quantidade;
                    int.TryParse(((Label)imbDarBaixa.Parent.FindControl("lblQuantidade")).Text, out quantidade);

                    string nomeFantasia = ((DropDownList)imbDarBaixa.Parent.FindControl("ddlLojaSaida")).SelectedItem.Text;
                    string carreto = ((Label)imbDarBaixa.Parent.Parent.Parent.Parent.Parent.FindControl("lblNomeCarreto")).Text;

                    if (string.IsNullOrEmpty(reservaId))
                    {
                        throw new ApplicationException("É necessário informar o PedidoID para dar baixa na reserva.");
                    }

                    if (produtoId <= 0)
                    {
                        throw new ApplicationException("É necessário informar o ProdutoID para dar baixa na reserva.");
                    }

                    if (lojaSaidaId <= 0)
                    {
                        throw new ApplicationException("É necessário informar a loja saída para dar baixa na reserva.");
                    }

                    if (!string.IsNullOrEmpty(nomeFantasia) && nomeFantasia.Trim().ToLower().Equals("depósito"))
                    {
                        if (string.IsNullOrEmpty(carreto) && string.IsNullOrEmpty(txtNomeCarreto.Text.Trim()))
                        {
                            throw new ApplicationException("É necessário informar o carreto para dar baixa na reserva, se ainda não foi cadastrado.");
                        }
                    }

                    if (!reservaDAL.Existe(reservaId.ToString(), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Reserva inexistente.");
                    }

                    if (Convert.ToInt32(reservaDAL.Listar(reservaId, usuarioSessao.SistemaID).Tables[0].Rows[0]["StatusID"]) == UtilitarioBLL.StatusEntregaReserva.Efetuada.GetHashCode())
                    {
                        throw new ApplicationException("Não é possível dar baixa nessa reserva, pois a baixa já foi efetuada.");
                    }

                    if (!new DAL.ProdutoDAL().ExisteNaLoja(produtoId, lojaSaidaId.ToString(), usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException(string.Format("Produto {0} não está cadastrado na loja {1}.", produtoId, nomeFantasia));
                    }

                    reservaDAL.InserirBaixaReserva(reservaId, string.IsNullOrEmpty(carreto) ? txtNomeCarreto.Text.Trim() : carreto, produtoId, quantidade, lojaSaidaId, usuarioSessao.SistemaID, usuarioSessao.TipoUsuarioID);

                    LimparFormulario();
                    txtReservaID.Text = reservaId.ToString();

                    CarregarDados(false);
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

        protected void imbProrrogar_Click(object sender, ImageClickEventArgs e)
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
                    DAL.ReservaDAL reservaDAL = new DAL.ReservaDAL();

                    string reservaId = txtReservaID.Text.Trim();

                    DateTime dtEntrega;
                    DateTime.TryParse(txtDataEntregaInicio.Text.Trim(), out dtEntrega);

                    if (string.IsNullOrEmpty(reservaId))
                    {
                        throw new ApplicationException("É necessário informar o PedidoID");
                    }

                    if ((ckbSemDataEntrega.Checked && dtEntrega != DateTime.MinValue) || (!ckbSemDataEntrega.Checked && dtEntrega == DateTime.MinValue))
                    {
                        throw new ApplicationException("É necessário informar a data de entrega ou A PROGRAMAR para prorrogar o pedido");
                    }

                    if (dtEntrega != DateTime.MinValue && dtEntrega < DateTime.Today)
                    {
                        throw new ApplicationException("A data da entrega deve ser maior do que a data de hoje");
                    }

                    if (!reservaDAL.Existe(txtReservaID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Reserva inexistente");
                    }

                    int statusId = Convert.ToInt32(reservaDAL.Listar(reservaId, usuarioSessao.SistemaID).Tables[0].Rows[0]["StatusID"]);

                    if (statusId == UtilitarioBLL.StatusEntregaReserva.Efetuada.GetHashCode())
                    {
                        throw new ApplicationException("Não é possível prorrogar o pedido, pois a baixa já foi efetuada");
                    }

                    reservaDAL.Atualizar(reservaId, dtEntrega, usuarioSessao.SistemaID);
                    LimparFormulario();
                    txtReservaID.Text = reservaId.ToString();
                    CarregarDados(false);
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

        protected void imbTransito_Click(object sender, ImageClickEventArgs e)
        {
            try
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
                    DAL.ReservaDAL reservaDAL = new DAL.ReservaDAL();

                    string reservaId = txtReservaID.Text.Trim();

                    if (string.IsNullOrEmpty(reservaId))
                    {
                        throw new ApplicationException("É necessário informar o PedidoID para transitar a reserva.");
                    }
                    if (!reservaDAL.Existe(reservaId, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Reserva inexistente.");
                    }

                    int statusId = Convert.ToInt32(reservaDAL.Listar(reservaId, usuarioSessao.SistemaID).Tables[0].Rows[0]["StatusID"]);

                    if (statusId == UtilitarioBLL.StatusEntregaReserva.Transito.GetHashCode())
                    {
                        throw new ApplicationException(string.Format("Não é possível transitar reserva {0}, pois a mesma já se encontra em trânsito.", reservaId));
                    }

                    if (statusId == UtilitarioBLL.StatusEntregaReserva.Efetuada.GetHashCode())
                    {
                        throw new ApplicationException(string.Format("Não é possível transitar reserva {0}, pois a baixa já foi efetuada.", reservaId));
                    }

                    reservaDAL.InserirTransito(reservaId, usuarioSessao.SistemaID);
                    LimparFormulario();
                    txtReservaID.Text = reservaId.ToString();
                    CarregarDados(false);
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

        protected void imbDarBaixaGeral_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                var reservaDAL = new DAL.ReservaDAL();

                //var reservasId = txtReservaID.Text.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //if (reservasId == null || reservasId.Length <= 0)
                //{
                //    throw new ApplicationException("Informe a ReservaID para dar baixa geral");
                //}

                int lojaSaidaIdGeral;
                int.TryParse(ddlLojaSaidaGeral.SelectedValue, out lojaSaidaIdGeral);
                if (lojaSaidaIdGeral <= 0)
                {
                    throw new ApplicationException("Informe a loja de saída para dar baixa geral");
                }

                var mensagemSucesso = "Baixas efetuadas: \r\n\r\n";
                var mensagemErro = "\r\nBaixas não efetuadas, pois já estão baixadas:\r\n\r\n";
                var itemSelecionado = false;

                foreach (RepeaterItem item in rptReserva.Items)
                {
                    var cbBaixa = (CheckBox)item.FindControl("cbBaixa");
                    if (cbBaixa.Checked)
                    {
                        itemSelecionado = true;
                        var reservasProdutos = reservaDAL.ListarReservaLojaFiltro(cbBaixa.ToolTip, usuarioSessao.LojaID, null, null, null, null, null, usuarioSessao.SistemaID, usuarioSessao.TipoUsuarioID, null);

                        foreach (DataRow reservaProduto in reservasProdutos.Tables[0].Rows)
                        {
                            if (reservaProduto["Baixa"] == null || !Convert.ToBoolean(reservaProduto["Baixa"]))
                            {
                                reservaDAL.InserirBaixaReserva(cbBaixa.ToolTip, txtNomeCarreto.Text.Trim(), Convert.ToInt64(reservaProduto["ProdutoID"]), Convert.ToInt16(reservaProduto["Quantidade"]), lojaSaidaIdGeral, usuarioSessao.SistemaID, usuarioSessao.TipoUsuarioID);
                                mensagemSucesso += string.Format("ReservaID: {0} - Produto: {1} \r\n", reservaProduto["ReservaID"].ToString(), reservaProduto["Produto"].ToString());
                            }
                            else
                            {
                                mensagemErro += string.Format("ReservaID: {0} - Produto: {1} \r\n", reservaProduto["ReservaID"].ToString(), reservaProduto["Produto"].ToString());
                            }
                        }
                    }
                }

                if (!itemSelecionado)
                {
                    throw new ApplicationException("Selecione alguma Reserva para dar baixa geral");
                }

                //foreach (var reservaId in reservasId)
                //{
                //    var reservasProdutos = reservaDAL.ListarReservaLojaFiltro(reservaId, usuarioSessao.LojaID, null, null, null, null, null, usuarioSessao.SistemaID, usuarioSessao.TipoUsuarioID, null);

                //    foreach (DataRow reservaProduto in reservasProdutos.Tables[0].Rows)
                //    {
                //        if (reservaProduto["Baixa"] == null || !Convert.ToBoolean(reservaProduto["Baixa"]))
                //        {
                //            reservaDAL.InserirBaixaReserva(reservaId, txtNomeCarreto.Text.Trim(), Convert.ToInt64(reservaProduto["ProdutoID"]), Convert.ToInt16(reservaProduto["Quantidade"]), lojaSaidaIdGeral, usuarioSessao.SistemaID, usuarioSessao.TipoUsuarioID);
                //            mensagemSucesso += string.Format("ReservaID: {0} - Produto: {1} \r\n", reservaProduto["ReservaID"].ToString(), reservaProduto["Produto"].ToString());
                //        }
                //        else
                //        {
                //            mensagemErro += string.Format("ReservaID: {0} - Produto: {1} \r\n", reservaProduto["ReservaID"].ToString(), reservaProduto["Produto"].ToString());
                //        }
                //    }
                //}

                LimparFormulario();

                CarregarDados(false);

                UtilitarioBLL.ExibirMensagemAjax(Page, mensagemSucesso + mensagemErro);
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

        private void LimparFormulario()
        {
            txtReservaID.Text = string.Empty;
            txtDataReservaInicio.Text = string.Empty;
            txtDataReservaFim.Text = string.Empty;
            txtDataEntregaInicio.Text = string.Empty;
            txtDataEntregaFim.Text = string.Empty;
            txtNomeCarreto.Text = string.Empty;
            ddlStatus.SelectedValue = "0";
            ddlLojaSaidaGeral.SelectedIndex = 0;
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode())
            {
                imbProrrogar.Visible = false;
                imbTransito.Visible = false;
            }
            else
            {
                imbProrrogar.Visible = true;
                imbTransito.Visible = true;
            }
        }

        protected void imgRelatorioSintetico_Click(object sender, ImageClickEventArgs e)
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
                    DateTime dtReservaInicio;
                    DateTime.TryParse(txtDataReservaInicio.Text, out dtReservaInicio);

                    DateTime dtReservaFim;
                    DateTime.TryParse(txtDataReservaFim.Text, out dtReservaFim);

                    if (dtReservaInicio == DateTime.MinValue || dtReservaFim == DateTime.MinValue)
                        throw new ApplicationException("Informe as datas de reserva início e fim ou as datas informadas são inválidas.");

                    var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                    rpvRelatorioSintetico.ProcessingMode = ProcessingMode.Local;
                    rpvRelatorioSintetico.LocalReport.ReportPath = HostingEnvironment.ApplicationPhysicalPath + "RelatorioSintetico.rdlc";

                    ReportDataSource rds = new ReportDataSource
                    {
                        Name = "dsRelatorioSintetico",
                        Value = new DAL.ReservaDAL().ListarRelatorioSintetico(dtReservaInicio, dtReservaFim, usuarioSessao.SistemaID)
                    };

                    rpvRelatorioSintetico.LocalReport.DataSources.Clear();
                    rpvRelatorioSintetico.LocalReport.DataSources.Add(rds);
                    rpvRelatorioSintetico.LocalReport.Refresh();

                    mpeRelatorioSintetico.Show();
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