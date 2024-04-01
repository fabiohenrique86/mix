<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="PedidoAlteracao.aspx.cs"
    Inherits="Site.PedidoAlteracao" Title="ALTERAÇÃO DE PEDIDO/RESERVA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
        {
            width: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    ALTERAÇÃO / EXCLUSÃO - PEDIDO
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblPedidoID" runat="server" Text="PedidoID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtPedidoID" runat="server" Width="155px" SkinID="TextBox" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="background-color: #003366; font-weight: bold; color: #FFD700;
                                text-align: center;">
                                EXCLUIR
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblProdutoE" runat="server" Text="Produto" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlProdutoE" runat="server" SkinID="DropDownList" DataTextField="Descricao"
                                    DataValueField="ProdutoID">
                                </asp:DropDownList>
                                <asp:Label ID="lblSobMedidaE" runat="server" Text="Sob Medida" SkinID="Label"></asp:Label>
                                <asp:TextBox ID="txtSobMedidaE" runat="server" Text="" Width="80px" MaxLength="20"
                                    SkinID="TextBox" />
                                <span class="campo_opcional">*</span>&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblQuantidadeE" runat="server" Text="Quantidade" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtQuantidadeE" runat="server" Text="" Width="30px" MaxLength="3"
                                    SkinID="TextBox">
                                </asp:TextBox>
                                <span class="campo_opcional">*</span>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtQuantidadeE" runat="server"
                                    TargetControlID="txtQuantidadeE" FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="background-color: #003366; font-weight: bold; color: #FFD700;
                                text-align: center;">
                                INCLUIR
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblProdutoI" runat="server" Text="Produto" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlProdutoI" runat="server" SkinID="DropDownList" DataTextField="Descricao"
                                    DataValueField="ProdutoID">
                                </asp:DropDownList>
                                <asp:Label ID="lblSobMedidaI" runat="server" Text="Sob Medida" SkinID="Label"></asp:Label>
                                <asp:TextBox ID="txtSobMedidaI" runat="server" Text="" Width="80px" MaxLength="20"
                                    SkinID="TextBox" />
                                <span class="campo_opcional">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblQuantidadeI" runat="server" Text="Quantidade" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtQuantidadeI" runat="server" Text="" Width="30px" MaxLength="3"
                                    SkinID="TextBox">
                                </asp:TextBox>
                                <span class="campo_opcional">*</span>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtQuantidadeI" runat="server"
                                    TargetControlID="txtQuantidadeI" FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblPreco" runat="server" Text="Preço Unitário" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPreco" runat="server" MaxLength="10" SkinID="TextBox" CssClass="decimal"
                                    Width="70px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="gdvTipoPagamento" runat="server" SkinID="GridView" OnRowDataBound="gdvTipoPagamento_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Selecione" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckbTipoPagamento" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tipo Pagamento" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="65%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTipoPagamentoID" runat="server" Text='<%# Bind("TipoPagamentoID") %>'
                                                    Visible="false">
                                                </asp:Label>
                                                <asp:Label ID="lblTipoPagamento" runat="server" Text='<%# Bind("Descricao") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Parcela" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlParcela" runat="server" DataTextField="Parcela" DataValueField="ParcelaID"
                                                    Height="20px" SkinID="DropDownListRelatorio">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtValorPago" runat="server" Text="" Width="70px" MaxLength="10"
                                                    Height="14px" SkinID="TextBox" CssClass="decimal"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblObservacao" runat="server" Text="Observação" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtObservacao" runat="server" Width="750px" MaxLength="350" TextMode="MultiLine"
                                    Height="80px" SkinID="TextBox">
                                </asp:TextBox><span class="campo_opcional">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="campo_opcional">
                                * campo opcional
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbAtualizar" runat="server" ImageUrl="~/img/atualizar.png"
                                    OnClick="imbAtualizar_Click" />
                                <asp:ImageButton ID="imbCadastrarMais" runat="server" ImageUrl="~/img/cadastrar_mais.png"
                                    Visible="false" OnClick="imbCadastrarMais_Click" />
                                <asp:ImageButton ID="imbFinalizarProduto" runat="server" ImageUrl="~/img/finalizar.png"
                                    Visible="false" OnClick="imbFinalizarProduto_Click" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" OnClick="imbExcluir_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
