using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using DAO;
using DAL;

namespace Site
{
    public partial class PedidoAlteracao : System.Web.UI.Page
    {
        private void CarregarDados()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            DataSet ds = new DAL.ProdutoDAL().ListarDropDownList(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
            this.ddlProdutoE.DataSource = ds;
            this.ddlProdutoE.DataBind();
            this.ddlProdutoI.DataSource = ds;
            this.ddlProdutoI.DataBind();
            this.gdvTipoPagamento.DataSource = DAL.TipoPagamentoDAL.Listar(usuarioSessao.SistemaID);
            this.gdvTipoPagamento.DataBind();
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

        private void HabilitarFormulario()
        {
            this.txtPedidoID.CssClass = string.Empty;
            this.txtPedidoID.Enabled = true;
            this.txtPedidoID.Text = string.Empty;
            this.ddlProdutoE.SelectedIndex = 0;
            this.txtQuantidadeE.Text = string.Empty;
            this.ddlProdutoI.SelectedIndex = 0;
            this.txtSobMedidaE.Text = string.Empty;
            this.txtSobMedidaI.Text = string.Empty;
            this.txtQuantidadeI.Text = string.Empty;
            this.txtPreco.Text = string.Empty;
            this.txtObservacao.CssClass = string.Empty;
            this.txtObservacao.Enabled = true;
            this.txtObservacao.Text = string.Empty;
            this.imbAtualizar.Visible = true;
            this.imbExcluir.Visible = true;
            this.imbCadastrarMais.Visible = false;
            this.imbFinalizarProduto.Visible = false;
            foreach (GridViewRow gdrTipoPagamento in this.gdvTipoPagamento.Rows)
            {
                ((CheckBox)gdrTipoPagamento.FindControl("ckbTipoPagamento")).Checked = false;
                if (!((Label)gdrTipoPagamento.FindControl("lblTipoPagamento")).Text.ToUpper().Contains("DINHEIRO"))
                {
                    ((DropDownList)gdrTipoPagamento.FindControl("ddlParcela")).SelectedIndex = 0;
                }
                ((TextBox)gdrTipoPagamento.FindControl("txtValorPago")).Text = string.Empty;
            }
        }

        protected void imbAtualizar_Click(object sender, ImageClickEventArgs e)
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
                    DAL.PedidoDAL pedidoDAL = new DAL.PedidoDAL();

                    if (string.IsNullOrEmpty(this.txtPedidoID.Text))
                    {
                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                    }
                    if (((this.ddlProdutoE.SelectedIndex > 0) && !string.IsNullOrEmpty(this.txtQuantidadeE.Text)) && (this.txtQuantidadeE.Text != "0"))
                    {
                        if (((this.ddlProdutoI.SelectedIndex <= 0) || string.IsNullOrEmpty(this.txtQuantidadeI.Text)) || !(this.txtQuantidadeI.Text != "0"))
                        {
                            throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                        }
                        string medidaE = string.Empty;
                        string medidaI = string.Empty;
                        if (this.ddlProdutoE.SelectedItem.Text.ToUpper().Contains("MEDIDA"))
                        {
                            if (string.IsNullOrEmpty(this.txtSobMedidaE.Text.Trim().ToUpper().ToUpper()))
                            {
                                throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                            }
                            medidaE = this.txtSobMedidaE.Text.Trim().ToUpper().ToUpper();
                        }
                        if (this.ddlProdutoI.SelectedItem.Text.ToUpper().Contains("MEDIDA"))
                        {
                            if (string.IsNullOrEmpty(this.txtSobMedidaI.Text.Trim().ToUpper().ToUpper()))
                            {
                                throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                            }
                            medidaI = this.txtSobMedidaI.Text.Trim().ToUpper().ToUpper();
                        }
                        if (!string.IsNullOrEmpty(medidaE) && !string.IsNullOrEmpty(medidaI))
                        {
                            if (this.ddlProdutoE.SelectedValue.Equals(this.ddlProdutoI.SelectedValue) && medidaE.Equals(medidaI))
                            {
                                throw new ApplicationException("É necessário informar produtos diferentes para atualizar.");
                            }
                        }
                        else if (this.ddlProdutoE.SelectedValue.Equals(this.ddlProdutoI.SelectedValue))
                        {
                            throw new ApplicationException("É necessário informar produtos diferentes para atualizar.");
                        }
                        if (!pedidoDAL.Existe(this.txtPedidoID.Text, usuarioSessao.SistemaID))
                        {
                            if (!reservaDAL.Existe(this.txtPedidoID.Text, usuarioSessao.SistemaID))
                            {
                                if (DAL.CancelamentoDAL.Listar(this.txtPedidoID.Text, usuarioSessao.SistemaID).Tables[0].Rows.Count > 0)
                                {
                                    throw new ApplicationException("Pedido cancelado.");
                                }
                                throw new ApplicationException("Pedido inexistente.");
                            }
                            if (!reservaDAL.Listar(txtPedidoID.Text, Convert.ToInt64(this.ddlProdutoE.SelectedValue), medidaE, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException("Produto a ser excluído inexistente ao Pedido.");
                            }
                            if (!reservaDAL.Listar(txtPedidoID.Text, this.ddlProdutoE.SelectedValue, medidaE, this.txtQuantidadeE.Text, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException("Quantidade informada do produto excluído é maior do que a quantidade existente no pedido.");
                            }
                            if (!reservaDAL.Listar(txtPedidoID.Text, Convert.ToInt64(this.ddlProdutoI.SelectedValue), medidaI, usuarioSessao.SistemaID))
                            {
                                if (string.IsNullOrEmpty(this.txtPreco.Text) || (this.txtPreco.Text == "0,00"))
                                {
                                    throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                                }
                                this.ViewState["TotalPedido"] = Convert.ToDecimal(reservaDAL.ListarValorReserva(txtPedidoID.Text, usuarioSessao.SistemaID).Tables[0].Rows[0]["ValorPedido"]);
                                reservaDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoE.SelectedValue, this.txtQuantidadeE.Text, medidaE, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, this.txtObservacao.Text, usuarioSessao.SistemaID);
                                this.TravarFormulario();
                                this.ViewState["stPedido"] = false;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.txtPreco.Text) && (this.txtPreco.Text != "0,00"))
                                {
                                    throw new ApplicationException("Não é possível modificar o preço de um Produto existente no Pedido.\r\n\r\nInforme o valor 0,00 no campo Preço Unitário.");
                                }
                                this.ViewState["TotalPedido"] = Convert.ToDecimal(reservaDAL.ListarValorReserva(txtPedidoID.Text, usuarioSessao.SistemaID).Tables[0].Rows[0]["ValorPedido"]);
                                reservaDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoE.SelectedValue, this.txtQuantidadeE.Text, medidaE, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, this.txtObservacao.Text, usuarioSessao.SistemaID);
                                this.TravarFormulario();
                                this.ViewState["stPedido"] = false;
                            }
                        }
                        else
                        {
                            if (!pedidoDAL.Listar(this.txtPedidoID.Text, this.ddlProdutoE.SelectedValue, medidaE, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException("Produto a ser excluído inexistente ao Pedido.");
                            }
                            if (!pedidoDAL.Listar(this.txtPedidoID.Text, this.ddlProdutoE.SelectedValue, medidaE, this.txtQuantidadeE.Text, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException("Quantidade informada do produto excluído é maior do que a quantidade existente no pedido.");
                            }
                            if (!pedidoDAL.Listar(this.txtPedidoID.Text, this.ddlProdutoI.SelectedValue, medidaI, usuarioSessao.SistemaID))
                            {
                                if (string.IsNullOrEmpty(this.txtPreco.Text) || (this.txtPreco.Text == "0,00"))
                                {
                                    throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                                }
                                this.ViewState["TotalPedido"] = Convert.ToDecimal(pedidoDAL.ListarValorPedido(txtPedidoID.Text, usuarioSessao.SistemaID).Tables[0].Rows[0]["ValorPedido"]);
                                pedidoDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoE.SelectedValue, this.txtQuantidadeE.Text, medidaE, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, this.txtObservacao.Text, usuarioSessao.SistemaID);
                                this.TravarFormulario();
                                this.ViewState["stPedido"] = true;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.txtPreco.Text) && (this.txtPreco.Text != "0,00"))
                                {
                                    throw new ApplicationException("Não é possível modificar o preço de um Produto existente no Pedido.\r\n\r\nInforme o valor 0,00 no campo Preço Unitário.");
                                }
                                this.ViewState["TotalPedido"] = Convert.ToDecimal(pedidoDAL.ListarValorPedido(txtPedidoID.Text, usuarioSessao.SistemaID).Tables[0].Rows[0]["ValorPedido"]);
                                pedidoDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoE.SelectedValue, this.txtQuantidadeE.Text, medidaE, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, this.txtObservacao.Text, usuarioSessao.SistemaID);
                                this.TravarFormulario();
                                this.ViewState["stPedido"] = true;
                            }
                        }
                    }
                    else
                    {
                        string medidaI = string.Empty;
                        if (this.ddlProdutoI.SelectedItem.Text.ToUpper().Contains("MEDIDA"))
                        {
                            if (string.IsNullOrEmpty(this.txtSobMedidaI.Text.Trim().ToUpper().ToUpper()))
                            {
                                throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                            }
                            medidaI = this.txtSobMedidaI.Text.Trim().ToUpper().ToUpper();
                        }
                        if (((this.ddlProdutoI.SelectedIndex > 0) && !string.IsNullOrEmpty(this.txtQuantidadeI.Text)) && (this.txtQuantidadeI.Text != "0"))
                        {
                            if (!pedidoDAL.Existe(this.txtPedidoID.Text, usuarioSessao.SistemaID))
                            {
                                if (!reservaDAL.Existe(this.txtPedidoID.Text, usuarioSessao.SistemaID))
                                {
                                    if (DAL.CancelamentoDAL.Listar(this.txtPedidoID.Text, usuarioSessao.SistemaID).Tables[0].Rows.Count > 0)
                                    {
                                        throw new ApplicationException("Pedido cancelado.");
                                    }
                                    throw new ApplicationException("Pedido inexistente.");
                                }
                                if (reservaDAL.Listar(txtPedidoID.Text, Convert.ToInt64(ddlProdutoI.SelectedValue), medidaI, usuarioSessao.SistemaID))
                                {
                                    if (!string.IsNullOrEmpty(this.txtPreco.Text) && (this.txtPreco.Text != "0,00"))
                                    {
                                        throw new ApplicationException("Não é possível modificar o preço de um Produto existente no Pedido.\r\n\r\nInforme o valor 0,00 no campo Preço Unitário.");
                                    }
                                    this.ViewState["TotalPedido"] = Convert.ToDecimal(reservaDAL.ListarValorReserva(txtPedidoID.Text, usuarioSessao.SistemaID).Tables[0].Rows[0]["ValorPedido"]);
                                    reservaDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, usuarioSessao.SistemaID);
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(this.txtPreco.Text) || (this.txtPreco.Text == "0,00"))
                                    {
                                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                                    }
                                    this.ViewState["TotalPedido"] = Convert.ToDecimal(reservaDAL.ListarValorReserva(txtPedidoID.Text, usuarioSessao.SistemaID).Tables[0].Rows[0]["ValorPedido"]);
                                    reservaDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, usuarioSessao.SistemaID);
                                }
                                this.TravarFormulario();
                                this.ViewState["stPedido"] = false;
                            }
                            else
                            {
                                if (pedidoDAL.Listar(this.txtPedidoID.Text, this.ddlProdutoI.SelectedValue, medidaI, usuarioSessao.SistemaID))
                                {
                                    if (!string.IsNullOrEmpty(this.txtPreco.Text) && (this.txtPreco.Text != "0,00"))
                                    {
                                        throw new ApplicationException("Não é possível modificar o preço de um Produto existente no Pedido.\r\n\r\nInforme o valor 0,00 no campo Preço Unitário.");
                                    }
                                    this.ViewState["TotalPedido"] = Convert.ToDecimal(pedidoDAL.ListarValorPedido(txtPedidoID.Text, usuarioSessao.SistemaID).Tables[0].Rows[0]["ValorPedido"]);
                                    pedidoDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, usuarioSessao.SistemaID);
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(this.txtPreco.Text) || (this.txtPreco.Text == "0,00"))
                                    {
                                        throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                                    }
                                    this.ViewState["TotalPedido"] = Convert.ToDecimal(pedidoDAL.ListarValorPedido(txtPedidoID.Text, usuarioSessao.SistemaID).Tables[0].Rows[0]["ValorPedido"]);
                                    pedidoDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, usuarioSessao.SistemaID);
                                }
                                this.TravarFormulario();
                                this.ViewState["stPedido"] = true;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(this.txtObservacao.Text))
                            {
                                throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                            }
                            string medidaE = string.Empty;
                            medidaI = string.Empty;
                            if (pedidoDAL.Existe(this.txtPedidoID.Text, usuarioSessao.SistemaID))
                            {
                                pedidoDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoE.SelectedValue, this.txtQuantidadeE.Text, medidaE, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, this.txtObservacao.Text, usuarioSessao.SistemaID);
                            }
                            else if (reservaDAL.Existe(this.txtPedidoID.Text, usuarioSessao.SistemaID))
                            {
                                reservaDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoE.SelectedValue, this.txtQuantidadeE.Text, medidaE, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, this.txtObservacao.Text, usuarioSessao.SistemaID);
                            }
                            else
                            {
                                if (DAL.CancelamentoDAL.Listar(this.txtPedidoID.Text, usuarioSessao.SistemaID).Tables[0].Rows.Count > 0)
                                {
                                    throw new ApplicationException("Pedido cancelado.");
                                }
                                throw new ApplicationException("Pedido inexistente.");
                            }
                            this.HabilitarFormulario();
                            UtilitarioBLL.ExibirMensagemAjax(this.Page, "Pedido alterado com sucesso.");
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

        protected void imbCadastrarMais_Click(object sender, ImageClickEventArgs e)
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
                    DAL.PedidoDAL pedidoDAL = new DAL.PedidoDAL();

                    if (((this.ddlProdutoE.SelectedIndex > 0) && !string.IsNullOrEmpty(this.txtQuantidadeE.Text)) && (this.txtQuantidadeE.Text != "0"))
                    {
                        if (((this.ddlProdutoI.SelectedIndex <= 0) || string.IsNullOrEmpty(this.txtQuantidadeI.Text)) || !(this.txtQuantidadeI.Text != "0"))
                        {
                            throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                        }
                        string medidaI = string.Empty;
                        string medidaE = string.Empty;
                        if (this.ddlProdutoE.SelectedItem.Text.ToUpper().Contains("MEDIDA"))
                        {
                            if (string.IsNullOrEmpty(this.txtSobMedidaE.Text.Trim().ToUpper().ToUpper()))
                            {
                                throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                            }
                            medidaE = this.txtSobMedidaE.Text.Trim().ToUpper().ToUpper();
                        }
                        if (this.ddlProdutoI.SelectedItem.Text.ToUpper().Contains("MEDIDA"))
                        {
                            if (string.IsNullOrEmpty(this.txtSobMedidaI.Text.Trim().ToUpper()))
                            {
                                throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                            }
                            medidaI = this.txtSobMedidaI.Text.Trim().ToUpper();
                        }
                        if (!string.IsNullOrEmpty(medidaE) && !string.IsNullOrEmpty(medidaI))
                        {
                            if (this.ddlProdutoE.SelectedValue.Equals(this.ddlProdutoI.SelectedValue) && medidaE.Equals(medidaI))
                            {
                                throw new ApplicationException("É necessário informar produtos diferentes para atualizar.");
                            }
                        }
                        else if (this.ddlProdutoE.SelectedValue.Equals(this.ddlProdutoI.SelectedValue))
                        {
                            throw new ApplicationException("É necessário informar produtos diferentes para atualizar.");
                        }
                        if (Convert.ToBoolean(this.ViewState["stPedido"]))
                        {
                            if (!pedidoDAL.Listar(this.txtPedidoID.Text, this.ddlProdutoE.SelectedValue, medidaE, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException("Produto a ser excluído inexistente ao Pedido.");
                            }
                            if (!pedidoDAL.Listar(this.txtPedidoID.Text, this.ddlProdutoE.SelectedValue, medidaE, this.txtQuantidadeE.Text, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException("Quantidade informada do produto excluído é maior do que a quantidade existente no pedido.");
                            }
                            if (!pedidoDAL.Listar(this.txtPedidoID.Text, this.ddlProdutoI.SelectedValue, medidaI, usuarioSessao.SistemaID))
                            {
                                if (string.IsNullOrEmpty(this.txtPreco.Text) || (this.txtPreco.Text == "0,00"))
                                {
                                    throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                                }
                                pedidoDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoE.SelectedValue, this.txtQuantidadeE.Text, medidaE, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, this.txtObservacao.Text, usuarioSessao.SistemaID);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.txtPreco.Text) && (this.txtPreco.Text != "0,00"))
                                {
                                    throw new ApplicationException("Não é possível modificar o preço de um Produto existente no Pedido.\r\n\r\nInforme o valor 0,00 no campo Preço Unitário.");
                                }
                                pedidoDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoE.SelectedValue, this.txtQuantidadeE.Text, medidaE, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, this.txtObservacao.Text, usuarioSessao.SistemaID);
                            }
                        }
                        else
                        {
                            if (!reservaDAL.Listar(txtPedidoID.Text, Convert.ToInt64(this.ddlProdutoE.SelectedValue), medidaE, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException("Produto a ser excluído inexistente ao Pedido.");
                            }
                            if (!reservaDAL.Listar(this.txtPedidoID.Text, this.ddlProdutoE.SelectedValue, medidaE, this.txtQuantidadeE.Text, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException("Quantidade informada do produto excluído é maior do que a quantidade existente no pedido.");
                            }
                            if (reservaDAL.Listar(txtPedidoID.Text, Convert.ToInt64(this.ddlProdutoI.SelectedValue), medidaI, usuarioSessao.SistemaID))
                            {
                                if (!string.IsNullOrEmpty(this.txtPreco.Text) && (this.txtPreco.Text != "0,00"))
                                {
                                    throw new ApplicationException("Não é possível modificar o preço de um Produto existente no Pedido.\r\n\r\nInforme o valor 0,00 no campo Preço Unitário.");
                                }
                                reservaDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoE.SelectedValue, this.txtQuantidadeE.Text, medidaE, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, this.txtObservacao.Text, usuarioSessao.SistemaID);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(this.txtPreco.Text) || (this.txtPreco.Text == "0,00"))
                                {
                                    throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                                }
                                reservaDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoE.SelectedValue, this.txtQuantidadeE.Text, medidaE, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, this.txtObservacao.Text, usuarioSessao.SistemaID);
                            }
                        }
                        this.TravarFormulario();
                    }
                    else
                    {
                        if (((this.ddlProdutoI.SelectedIndex <= 0) || string.IsNullOrEmpty(this.txtQuantidadeI.Text)) || !(this.txtQuantidadeI.Text != "0"))
                        {
                            throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                        }
                        string medidaI = string.Empty;
                        if (this.ddlProdutoI.SelectedItem.Text.ToUpper().Contains("MEDIDA"))
                        {
                            if (string.IsNullOrEmpty(this.txtSobMedidaI.Text.Trim().ToUpper()))
                            {
                                throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                            }
                            medidaI = this.txtSobMedidaI.Text.Trim().ToUpper();
                        }
                        if (Convert.ToBoolean(this.ViewState["stPedido"]))
                        {
                            if (!pedidoDAL.Listar(this.txtPedidoID.Text, this.ddlProdutoI.SelectedValue, medidaI, usuarioSessao.SistemaID))
                            {
                                if (string.IsNullOrEmpty(this.txtPreco.Text) || (this.txtPreco.Text == "0,00"))
                                {
                                    throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                                }
                                pedidoDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, usuarioSessao.SistemaID);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.txtPreco.Text) && (this.txtPreco.Text != "0,00"))
                                {
                                    throw new ApplicationException("Não é possível modificar o preço de um Produto existente no Pedido.\r\n\r\nInforme o valor 0,00 no campo Preço Unitário.");
                                }
                                pedidoDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, usuarioSessao.SistemaID);
                            }
                        }
                        else if (reservaDAL.Listar(txtPedidoID.Text, Convert.ToInt64(this.ddlProdutoI.SelectedValue), medidaI, usuarioSessao.SistemaID))
                        {
                            if (!string.IsNullOrEmpty(this.txtPreco.Text) && (this.txtPreco.Text != "0,00"))
                            {
                                throw new ApplicationException("Não é possível modificar o preço de um Produto existente no Pedido.\r\n\r\nInforme o valor 0,00 no campo Preço Unitário.");
                            }
                            reservaDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, usuarioSessao.SistemaID);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(this.txtPreco.Text) || (this.txtPreco.Text == "0,00"))
                            {
                                throw new ApplicationException("É necessário informar todos os campos obrigatórios para atualizar.");
                            }
                            pedidoDAL.Atualizar(txtPedidoID.Text, this.ddlProdutoI.SelectedValue, this.txtQuantidadeI.Text, medidaI, this.txtPreco.Text, usuarioSessao.SistemaID);
                        }
                        this.TravarFormulario();
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

        protected void imbExcluir_Click(object sender, ImageClickEventArgs e)
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
                    DAL.PedidoDAL pedidoDAL = new DAL.PedidoDAL();

                    if (!(this.txtPedidoID.Text.Trim().ToUpper() != string.Empty))
                    {
                        throw new ApplicationException("É necessário informar o campo PedidoID para excluir.");
                    }
                    if (pedidoDAL.Existe(this.txtPedidoID.Text, usuarioSessao.SistemaID))
                    {
                        pedidoDAL.Excluir(txtPedidoID.Text, usuarioSessao.SistemaID);
                    }
                    else
                    {
                        if (!reservaDAL.Existe(this.txtPedidoID.Text, usuarioSessao.SistemaID))
                        {
                            throw new ApplicationException("Pedido inexistente.");
                        }
                        reservaDAL.Excluir(txtPedidoID.Text, usuarioSessao.SistemaID);
                    }
                    this.HabilitarFormulario();
                    UtilitarioBLL.ExibirMensagemAjax(this.Page, "Pedido excluído com sucesso.");
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

        protected void imbFinalizarProduto_Click(object sender, ImageClickEventArgs e)
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
                    DAL.PedidoDAL pedidoDAL = new DAL.PedidoDAL();
                    int tipoPagamentoInserido = 0;
                    decimal totalPago = 0;
                    decimal diferenca = 0;
                    bool tipoPagamentoSelecionado = false;
                    List<int> lstTipoPagamento = new List<int>();
                    List<int> lstParcela = new List<int>();
                    List<decimal> lstValor = new List<decimal>();

                    if (this.ViewState["stPedido"] != null)
                    {
                        if (Convert.ToBoolean(this.ViewState["stPedido"]))
                        {
                            diferenca = Convert.ToDecimal(pedidoDAL.ListarValorPedidoAlterado(txtPedidoID.Text, Convert.ToDecimal(this.ViewState["TotalPedido"]), usuarioSessao.SistemaID).Tables[0].Rows[0]["Diferenca"]);
                        }
                        else
                        {
                            diferenca = Convert.ToDecimal(new DAL.ReservaDAL().ListarValorReservaAlterado(txtPedidoID.Text, Convert.ToDecimal(this.ViewState["TotalPedido"]), usuarioSessao.SistemaID).Tables[0].Rows[0]["Diferenca"]);
                        }
                    }
                    if (diferenca > 0)
                    {
                        foreach (GridViewRow gdrTipoPagamento in this.gdvTipoPagamento.Rows)
                        {
                            if (((CheckBox)gdrTipoPagamento.FindControl("ckbTipoPagamento")).Checked)
                            {
                                Label lblTipoPagamento = (Label)gdrTipoPagamento.FindControl("lblTipoPagamento");
                                tipoPagamentoSelecionado = true;
                                DropDownList ddlParcela = (DropDownList)gdrTipoPagamento.FindControl("ddlParcela");
                                if (ddlParcela.SelectedIndex <= 0)
                                {
                                    throw new ApplicationException("Informe a Parcela do Tipo de Pagamento:\r\n\r\n" + lblTipoPagamento.Text);
                                }
                                TextBox txtValorPago = (TextBox)gdrTipoPagamento.FindControl("txtValorPago");
                                if (string.IsNullOrEmpty(txtValorPago.Text) || !(txtValorPago.Text != "0,00"))
                                {
                                    throw new ApplicationException("Informe o Valor Pago do Tipo de Pagamento:\r\n\r\n" + lblTipoPagamento.Text);
                                }
                                int tipoPagamentoId = Convert.ToInt32(((Label)gdrTipoPagamento.FindControl("lblTipoPagamentoID")).Text);
                                if (Convert.ToBoolean(this.ViewState["stPedido"]))
                                {
                                    pedidoDAL.Inserir(txtPedidoID.Text, tipoPagamentoId, ddlParcela.SelectedValue, txtValorPago.Text, usuarioSessao.SistemaID);
                                }
                                else
                                {
                                    reservaDAL.Inserir(txtPedidoID.Text, tipoPagamentoId, ddlParcela.SelectedValue, txtValorPago.Text, usuarioSessao.SistemaID);
                                }
                                lstTipoPagamento.Add(tipoPagamentoId);
                                lstParcela.Add(Convert.ToInt32(ddlParcela.SelectedValue));
                                lstValor.Add(Convert.ToDecimal(txtValorPago.Text));
                                tipoPagamentoInserido++;
                                totalPago += Convert.ToDecimal(txtValorPago.Text) * Convert.ToInt32(ddlParcela.SelectedValue);
                            }
                        }
                        if (!tipoPagamentoSelecionado)
                        {
                            throw new ApplicationException("É necessário incluir pagamento no valor de R$ " + diferenca.ToString());
                        }
                        if (!diferenca.Equals(totalPago))
                        {
                            if (tipoPagamentoInserido > 0)
                            {
                                for (int i = 0; i < tipoPagamentoInserido; i++)
                                {
                                    if (Convert.ToBoolean(this.ViewState["stPedido"]))
                                    {
                                        pedidoDAL.Excluir(txtPedidoID.Text, lstTipoPagamento.ElementAt<int>(i), lstParcela.ElementAt<int>(i), lstValor.ElementAt<decimal>(i), usuarioSessao.SistemaID);
                                    }
                                    else
                                    {
                                        reservaDAL.Excluir(txtPedidoID.Text, lstTipoPagamento.ElementAt<int>(i), lstParcela.ElementAt<int>(i), lstValor.ElementAt<decimal>(i), usuarioSessao.SistemaID);
                                    }
                                }
                            }
                            if (totalPago > diferenca)
                            {
                                decimal credito = totalPago - diferenca;
                                throw new ApplicationException("Restam R$ " + credito.ToString() + " de crédito.");
                            }
                            throw new ApplicationException("É necessário incluir pagamento no valor de R$ " + diferenca.ToString());
                        }
                        this.HabilitarFormulario();
                        UtilitarioBLL.ExibirMensagemAjax(this.Page, "Pedido alterado com sucesso.");
                    }
                    else
                    {
                        if (diferenca < 0)
                        {
                            decimal credito = diferenca * -1;
                            throw new ApplicationException("Restam R$ " + credito.ToString() + " de crédito.");
                        }
                        this.HabilitarFormulario();
                        UtilitarioBLL.ExibirMensagemAjax(this.Page, "Pedido alterado com sucesso.");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!base.IsPostBack)
                {
                    int tipoUsuarioId = new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID;

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
                    else if (tipoUsuarioId != UtilitarioBLL.TipoUsuario.Administrador.GetHashCode() && tipoUsuarioId != UtilitarioBLL.TipoUsuario.Logistica.GetHashCode())
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
                        this.SetarBordaGridView();
                        this.CarregarDados();
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

        private void TravarFormulario()
        {
            this.txtPedidoID.CssClass = "desabilitado";
            this.txtPedidoID.Enabled = false;
            this.ddlProdutoE.SelectedIndex = 0;
            this.txtQuantidadeE.Text = string.Empty;
            this.txtSobMedidaE.Text = string.Empty;
            this.ddlProdutoI.SelectedIndex = 0;
            this.txtQuantidadeI.Text = string.Empty;
            this.txtSobMedidaI.Text = string.Empty;
            this.txtPreco.Text = string.Empty;
            this.txtObservacao.CssClass = "desabilitado";
            this.txtObservacao.Enabled = false;
            this.imbAtualizar.Visible = false;
            this.imbCadastrarMais.Visible = true;
            this.imbFinalizarProduto.Visible = true;
        }
    }
}