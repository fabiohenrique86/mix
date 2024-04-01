using System;
using System.Web.UI;
using BLL;
using BLL.Modelo;

namespace Site
{
    public partial class Base : MasterPage
    {
        protected void lkbSair_Click(object sender, EventArgs e)
        {
            UtilitarioBLL.Sair();
            base.Session.Abandon();
            base.Response.Redirect("../Default.aspx");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            string empresa = this.Context.Items["empresa"] as string;
            if (!string.IsNullOrEmpty(empresa))
            {
                AplicacaoBLL.Empresa = new Empresa(empresa);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!base.IsPostBack)
                {
                    if (Session["Usuario"] != null && AplicacaoBLL.Empresa != null)
                    {
                        BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                        Menu.Visible = true;
                        switch (usuarioSessao.TipoUsuarioID)
                        {
                            case (int)UtilitarioBLL.TipoUsuario.Administrador:

                                lblConsulta.Text = "Consulta | Cadastro | Atualização | Exclusão";
                                break;

                            case (int)UtilitarioBLL.TipoUsuario.Estoquista:

                                MenuLoja.Visible = false;
                                MenuUsuario.Visible = false;
                                MenuFuncionario.Visible = false;
                                MenuTipoPagamento.Visible = false;
                                MenuParcela.Visible = false;
                                MenuLinha.Visible = false;
                                MenuMedida.Visible = false;
                                MenuProduto.Visible = true;
                                MenuBrinde.Visible = false;
                                MenuPedidoAgendado.Visible = true;
                                MenuLimiteReserva.Visible = false;
                                MenuCliente.Visible = true;
                                MenuPedidoMae.Visible = false;
                                MenuCancelamento.Visible = false;
                                MenuNotaFiscalManual.Visible = true;
                                MenuNotaFiscalAutomatica.Visible = true;
                                MenuLancamento.Visible = true;
                                MenuOrcamento.Visible = false;
                                MenuPedido.Visible = true;
                                MenuTransferencia.Visible = true;
                                MenuTroca.Visible = true;
                                MenuAlteracaoPedido.Visible = false;
                                MenuRelatorio.Visible = true;
                                MenuRelatorioCancelamento.Visible = false;
                                MenuRelatorioComissao.Visible = false;
                                MenuRelatorioEntrega.Visible = true;

                                if (usuarioSessao.SistemaID == 5)
                                {
                                    MenuRelatorioEstoque.Visible = true;
                                }
                                else
                                {
                                    MenuRelatorioEstoque.Visible = false;
                                }

                                MenuRelatorioEstoqueIdeal.Visible = false;
                                MenuRelatorioNotaFiscal.Visible = false;
                                MenuOcorrencia.Visible = true;
                                MenuRelatorioOcorrencia.Visible = true;
                                MenuRelatorioQuadroClassificacao.Visible = false;
                                MenuRelatorioVenda.Visible = false;
                                MenuRelatorioFrete.Visible = false;
                                lblConsulta.Text = "Consulta | Cadastro";
                                break;

                            case (int)UtilitarioBLL.TipoUsuario.Gerente:

                                MenuUsuario.Visible = false;
                                MenuPedidoAgendado.Visible = true;
                                MenuLimiteReserva.Visible = false;
                                MenuPedidoMae.Visible = false;
                                MenuCancelamento.Visible = false;
                                MenuNotaFiscalManual.Visible = true;
                                MenuNotaFiscalAutomatica.Visible = true;
                                MenuTransferencia.Visible = true;
                                MenuTroca.Visible = true;
                                MenuAlteracaoPedido.Visible = false;
                                MenuRelatorio.Visible = true;
                                MenuRelatorioCancelamento.Visible = false;
                                MenuRelatorioComissao.Visible = false;
                                MenuRelatorioEntrega.Visible = true;
                                MenuRelatorioEstoque.Visible = true;
                                MenuRelatorioEstoqueIdeal.Visible = false;
                                MenuRelatorioNotaFiscal.Visible = true;
                                MenuOcorrencia.Visible = false;
                                MenuRelatorioOcorrencia.Visible = false;
                                MenuRelatorioQuadroClassificacao.Visible = false;
                                MenuRelatorioVenda.Visible = false;
                                MenuRelatorioFrete.Visible = false;
                                lblConsulta.Text = "Consulta | Cadastro";
                                break;

                            case (int)UtilitarioBLL.TipoUsuario.Vendedor:

                                MenuUsuario.Visible = false;
                                MenuPedidoAgendado.Visible = false;
                                MenuLimiteReserva.Visible = false;
                                MenuPedidoMae.Visible = false;
                                MenuCancelamento.Visible = false;
                                MenuNotaFiscalManual.Visible = false;
                                MenuNotaFiscalAutomatica.Visible = false;
                                MenuTransferencia.Visible = false;
                                MenuTroca.Visible = false;
                                MenuAlteracaoPedido.Visible = false;
                                MenuRelatorio.Visible = true;
                                MenuRelatorioCancelamento.Visible = false;
                                MenuRelatorioComissao.Visible = false;
                                MenuRelatorioEntrega.Visible = true;
                                MenuRelatorioEstoque.Visible = true;
                                MenuRelatorioEstoqueIdeal.Visible = false;
                                MenuRelatorioNotaFiscal.Visible = false;
                                MenuOcorrencia.Visible = false;
                                MenuRelatorioOcorrencia.Visible = false;
                                MenuRelatorioQuadroClassificacao.Visible = false;
                                MenuRelatorioVenda.Visible = true;
                                MenuRelatorioVendaProduto.Visible = true;
                                MenuRelatorioVendaTipoPagamento.Visible = false;
                                MenuRelatorioFrete.Visible = false;
                                lblConsulta.Text = "Consulta | Cadastro";
                                break;

                            case (int)UtilitarioBLL.TipoUsuario.Conferentista:

                                MenuLoja.Visible = false;
                                MenuUsuario.Visible = false;
                                MenuFuncionario.Visible = false;
                                MenuTipoPagamento.Visible = false;
                                MenuParcela.Visible = false;
                                MenuLinha.Visible = false;
                                MenuMedida.Visible = false;
                                MenuProduto.Visible = true;
                                MenuBrinde.Visible = false;
                                MenuPedidoAgendado.Visible = true;
                                MenuLimiteReserva.Visible = false;
                                MenuBordero.Visible = true;
                                MenuCliente.Visible = true;
                                MenuPedidoMae.Visible = false;
                                MenuCancelamento.Visible = false;
                                MenuNotaFiscalManual.Visible = true;
                                MenuNotaFiscalAutomatica.Visible = true;
                                MenuLancamento.Visible = true;
                                MenuOrcamento.Visible = false;
                                MenuPedido.Visible = true;
                                MenuTransferencia.Visible = true;
                                MenuTroca.Visible = false;
                                MenuAlteracaoPedido.Visible = false;
                                MenuRelatorio.Visible = true;
                                MenuRelatorioCancelamento.Visible = false;
                                MenuRelatorioComissao.Visible = false;
                                MenuRelatorioEntrega.Visible = true;
                                MenuRelatorioEstoque.Visible = true;
                                MenuRelatorioEstoqueIdeal.Visible = true;
                                MenuRelatorioNotaFiscal.Visible = true;
                                MenuOcorrencia.Visible = false;
                                MenuRelatorioOcorrencia.Visible = false;
                                MenuRelatorioQuadroClassificacao.Visible = false;
                                MenuRelatorioVenda.Visible = true;
                                MenuRelatorioFrete.Visible = false;
                                lblConsulta.Text = "Consulta | Cadastro";
                                break;

                            case (int)UtilitarioBLL.TipoUsuario.Logistica:

                                // Consulta
                                MenuConsulta.Visible = true;
                                MenuLoja.Visible = false;
                                MenuUsuario.Visible = false;
                                MenuFuncionario.Visible = false;
                                MenuTipoPagamento.Visible = false;
                                MenuParcela.Visible = false;
                                MenuLinha.Visible = false;
                                MenuMedida.Visible = false;
                                MenuProduto.Visible = true;
                                MenuBrinde.Visible = false;
                                MenuCliente.Visible = true;
                                MenuPedidoAgendado.Visible = true;
                                MenuLimiteReserva.Visible = false;
                                MenuBordero.Visible = false;
                                MenuPedidoMae.Visible = true;

                                // Lançamento
                                MenuLancamento.Visible = true;
                                MenuCancelamento.Visible = false;
                                MenuNotaFiscalManual.Visible = true;
                                MenuNotaFiscalAutomatica.Visible = false;
                                MenuOcorrencia.Visible = true;
                                MenuOrcamento.Visible = false;
                                MenuPedido.Visible = true;
                                MenuTransferencia.Visible = true;
                                MenuTroca.Visible = true;

                                // Alteração
                                MenuAlteracaoPedido.Visible = true;

                                // Relatório
                                MenuRelatorio.Visible = true;
                                MenuRelatorioCancelamento.Visible = true;
                                MenuRelatorioComissao.Visible = false;
                                MenuRelatorioEntrega.Visible = true;
                                MenuRelatorioEstoque.Visible = true;
                                MenuRelatorioEstoqueIdeal.Visible = true;
                                MenuRelatorioNotaFiscal.Visible = true;
                                MenuRelatorioOcorrencia.Visible = true;
                                MenuRelatorioQuadroClassificacao.Visible = false;
                                MenuRelatorioVenda.Visible = false;
                                MenuRelatorioFrete.Visible = true;

                                lblConsulta.Text = "Consulta | Cadastro";
                                break;
                        }
                    }
                    else
                    {
                        Menu.Visible = false;
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
    }
}