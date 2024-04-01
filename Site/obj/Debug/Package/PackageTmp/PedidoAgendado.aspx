<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="PedidoAgendado.aspx.cs"
    Inherits="Site.PedidoAgendado" Title="CADASTRO | CONSULTA - PEDIDO AGENDADO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1 {
            width: 100px;
        }

        .style2 {
            width: 100px;
            text-align: left;
        }

        .label_repeater {
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
                    CONSULTA - PEDIDO AGENDADO
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblReservaID" runat="server" Text="PedidoID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtReservaID" runat="server" Width="375px" MaxLength="115" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblDataReservaInicio" runat="server" Text="Data Reserva" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataReservaInicio" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataReservaInicio_MaskedEditExtender" runat="server" TargetControlID="txtDataReservaInicio"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                                &nbsp;a&nbsp;                                
                                <asp:TextBox ID="txtDataReservaFim" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataReservaFim_MaskedEditExtender" runat="server" TargetControlID="txtDataReservaFim"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblDataEntregaInicio" runat="server" Text="Data Entrega" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataEntregaInicio" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataEntregaInicio_MaskedEditExtender" runat="server" TargetControlID="txtDataEntregaInicio"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                                &nbsp;a&nbsp;                                
                                <asp:TextBox ID="txtDataEntregaFim" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataEntregaFim_MaskedEditExtender" runat="server" TargetControlID="txtDataEntregaFim"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                                <asp:CheckBox ID="ckbSemDataEntrega" runat="server" Text="A PROGRAMAR" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblStatusID" runat="server" Text="Status (Entrega)" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlStatus" runat="server" SkinID="DropDownListRelatorio" Width="100px">
                                    <asp:ListItem Text="SELECIONE" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Pendente" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Trânsito" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Efetuada" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblNomeCarreteiro" runat="server" Text="Carreto" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtNomeCarreto" runat="server" Width="350px" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="label2" runat="server" Text="Loja Saída" SkinID="Label"></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="ddlLojaSaidaGeral" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID" SkinID="DropDownList">
                                </asp:DropDownList>
                                <span class="campo_opcional">informar somente se for dar baixa geral</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">Legenda :&nbsp;&nbsp;
                                <img src="../img/vermelho.png" alt="" />&nbsp;PENDENTE&nbsp;&nbsp;&nbsp;
                                <img src="../img/laranja.png" alt="" />&nbsp;TRÂNSITO&nbsp;&nbsp;&nbsp;
                                <img src="../img/verde.png" alt="" />&nbsp;EFETUADA                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png" OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbProrrogar" runat="server" ImageUrl="~/img/prorrogar.png" OnClick="imbProrrogar_Click" />
                                <asp:ImageButton ID="imbTransito" runat="server" ImageUrl="~/img/transito.png" OnClick="imbTransito_Click" />
                                <asp:ImageButton ID="imbDarBaixaGeral" runat="server" ImageUrl="~/img/dar_baixa.png" OnClick="imbDarBaixaGeral_Click" />
                                <asp:ImageButton ID="imgRelatorioSintetico" runat="server" ImageUrl="~/img/relatorio-sintetico.png" OnClick="imgRelatorioSintetico_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <div id="divScrollReserva" style="overflow-x: hidden; overflow-y: scroll; height: 400px;">
                        <asp:Repeater ID="rptReserva" runat="server" OnItemDataBound="rptReserva_ItemDataBound">
                            <ItemTemplate>
                                <div id="repeaterReserva" class="topo_grid" style="text-align: left;" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbBaixa" runat="server" ToolTip='<%# Bind("ReservaID") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="labelReserva" runat="server" Text="PedidoID:"></asp:Label>
                                            </td>
                                            <td class="label_pedido">
                                                <asp:Label ID="lblReservaID" runat="server" Text='<%# Bind("ReservaID") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="labelNomeFantasia" runat="server" Text="Loja Origem:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblNomeFantasia" runat="server" Text='<%# Bind("NomeFantasia") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="labelDataReserva" runat="server" Text="Data Reserva:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblDataReserva" runat="server" Text='<%# Bind("DataReserva","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="labelDataEntrega" runat="server" Text="Data Entrega:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblDataEntrega" runat="server" Text='<%# Bind("DataEntrega","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="labelNomeCarreto" runat="server" Text="Carreto:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblNomeCarreto" runat="server" Text='<%# Bind("NomeCarreteiro") %>'></asp:Label>
                                                <asp:Label ID="lblStatusID" runat="server" Text='<%# Bind("StatusID") %>' Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:GridView ID="gdvReservaProduto" runat="server" SkinID="GridView" OnRowDataBound="gdvReservaProduto_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Produto" HeaderText="Produto" ItemStyle-Width="50%" />
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPedidoID" runat="server" Text='<%# Bind("ReservaID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProdutoID" runat="server" Text='<%# Bind("ProdutoID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantidade" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuantidade" runat="server" Text='<%# Bind("Quantidade") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Loja Saída" ItemStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlLojaSaida" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID" Visible="false" />
                                                    <asp:Label ID="lblLojaID" runat="server" Text='<%# Bind("LojaID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblNomeFantasia" runat="server" Text='<%# Bind("NomeFantasia") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblBaixa" runat="server" Text='<%# Bind("Baixa") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dar Baixa" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imbDarBaixa" runat="server" ImageUrl="~/img/dar_baixa.png" OnClick="imbDarBaixa_Click" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeRelatorioSintetico" runat="server" CancelControlID="imbFechar" TargetControlID="hdfGerar" PopupControlID="pnlRelatorio" BackgroundCssClass="background_modal" DropShadow="false"></cc1:ModalPopupExtender>
    <asp:Panel ID="pnlRelatorio" runat="server" CssClass="window_modal" Width="740px" Height="430px" Style="display: none;">
        <rsweb:ReportViewer ID="rpvRelatorioSintetico" runat="server" Width="740px" Font-Names="Verdana" Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False" ShowPageNavigationControls="true" ShowRefreshButton="False" ShowZoomControl="False" ShowExportControls="true" ShowBackButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
