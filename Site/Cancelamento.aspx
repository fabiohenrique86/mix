<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Cancelamento.aspx.cs"
    Inherits="Site.Cancelamento" Title="LANÇAMENTO - CANCELAMENTO DE PEDIDO/RESERVA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100px;
        }
        .label_repeater
        {
            font-weight: normal;
            font-style: italic;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    CANCELAMENTO - PEDIDO
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblPedido" runat="server" Text="PedidoID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtPedidoID" runat="server" Width="155px" MaxLength="30" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblMotivo" runat="server" Text="Motivo" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtMotivo" runat="server" MaxLength="250" Width="750px" TextMode="MultiLine"
                                    Height="200px" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:ImageButton ID="imbCancelarPedido" runat="server" ImageUrl="~/img/cancelar_pedido_reserva.png"
                        OnClick="imbCancelarPedido_Click" />
                </div>
                <div id="corpo">
                    <asp:Repeater ID="rptLoja" runat="server" OnItemDataBound="rptLoja_ItemDataBound">
                        <ItemTemplate>
                            <div id="DivRepeaterLoja" runat="server">
                                <div class="topo_grid">
                                    <asp:Label ID="lblLojaID" runat="server" Text='<%# Bind("LojaID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblNomeFantasia" runat="server" Text='<%# Bind("NomeFantasia") %>'></asp:Label>
                                </div>
                                <div>
                                    <asp:Repeater ID="rptCancelamentoLoja" runat="server" OnItemDataBound="rptCancelamentoLoja_ItemDataBound">
                                        <ItemTemplate>
                                            <div style="background-color: #800000; color: #FFFFFF; font-weight: bold; font-size: 9px;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblPedido_" runat="server" Text="PedidoID: "></asp:Label>
                                                        </td>
                                                        <td class="label_repeater" style="cursor: pointer; font-size: 10px; color: #6495ed;">
                                                            <asp:Label ID="lblPedidoID" runat="server" Text='<%# Bind("PedidoID") %>' />                                                           
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDataCancelamento_" runat="server" Text="Data Cancelamento: "></asp:Label>
                                                        </td>
                                                        <td class="label_repeater">
                                                            <asp:Label ID="lblDataCancelamento" runat="server" Text='<%# Bind("DataCancelamento","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMotivo" runat="server" Text='<%# Bind("Motivo") %>' Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <asp:GridView ID="gdvCancelamento" runat="server" SkinID="GridView">
                                                <Columns>
                                                    <asp:BoundField DataField="Produto" HeaderText="Produto" ItemStyle-Width="40%" />
                                                    <asp:BoundField DataField="Motivo" HeaderText="Motivo" ItemStyle-Width="40%" Visible="false" />
                                                    <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" ItemStyle-Width="4%" />
                                                    <asp:BoundField DataField="Medida" HeaderText="Medida" ItemStyle-Width="6%" Visible="false" />
                                                    <asp:BoundField DataField="PrecoUnitario" HeaderText="Preco Unitário" DataFormatString="{0:c}"
                                                        ItemStyle-Width="10%" />
                                                    <asp:BoundField DataField="PrecoTotal" HeaderText="Preco Total" DataFormatString="{0:c}"
                                                        ItemStyle-Width="10%" />
                                                </Columns>
                                            </asp:GridView>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
