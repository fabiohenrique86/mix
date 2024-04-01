<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="FreteRel.aspx.cs" Inherits="Site.FreteRel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="imbVisualizar" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="imbVisualizar">
                <div id="conteudo">
                    <div id="topo_cabeca">
                        <asp:Label ID="lblTopo" runat="server" Text=""></asp:Label>
                    </div>
                    <div id="cabeca">
                        <table style="width: 100%;">
                            <tr>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblCidade" runat="server" Text="Cidade" SkinID="Label"></asp:Label>
                                    &nbsp;
                                    <asp:TextBox ID="txtCidade" runat="server" Width="450px" SkinID="TextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblCarreteiro" runat="server" Text="Carreteiro" SkinID="Label"></asp:Label>
                                    &nbsp;
                                    <asp:TextBox ID="txtCarreteiro" runat="server" Width="450px" SkinID="TextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblFuncionario" runat="server" Text="Funcionário" SkinID="Label"></asp:Label>
                                    &nbsp;
                                    <asp:DropDownList ID="ddlFuncionario" runat="server" SkinID="DropDownList" DataTextField="Nome"
                                        DataValueField="FuncionarioID" Width="600px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblDataReserva" runat="server" Text="Data Reserva" SkinID="Label"></asp:Label>
                                    &nbsp;
                                    <asp:TextBox ID="txtDataReservaInicial" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="txtDataReservaInicial_MaskedEditExtender1" runat="server" TargetControlID="txtDataReservaInicial"
                                        MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                    </cc1:MaskedEditExtender>
                                    &nbsp;a&nbsp;
                                    <asp:TextBox ID="txtDataReservaFinal" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="txtDataReservaFinal_MaskedEditExtender2" runat="server" TargetControlID="txtDataReservaFinal"
                                        MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblDataEntrega" runat="server" Text="Data Entrega" SkinID="Label"></asp:Label>
                                    &nbsp;
                                    <asp:TextBox ID="txtDataEntregaInicial" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="txtDataEntregaInicial_MaskedEditExtender" runat="server" TargetControlID="txtDataEntregaInicial"
                                        MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                    </cc1:MaskedEditExtender>
                                    &nbsp;a&nbsp;
                                    <asp:TextBox ID="txtDataEntregaFinal" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="txtDataEntregaFinal_MaskedEditExtender" runat="server" TargetControlID="txtDataEntregaFinal"
                                        MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="text-align: center;">
                        <asp:ImageButton ID="imbVisualizar" runat="server" ImageUrl="~/img/visualizar.png" OnClick="imbVisualizar_Click" />
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeFrete" runat="server" CancelControlID="imbFechar"
        TargetControlID="hdfGerar" PopupControlID="pnlRelatorio" BackgroundCssClass="background_modal"
        DropShadow="false">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlRelatorio" runat="server" CssClass="window_modal" Width="740px"
        Height="430px" Style="display: none;">
        <rsweb:ReportViewer ID="rpvFrete" runat="server" Width="740px" Font-Names="Verdana"
            Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="true" ShowRefreshButton="False" ShowZoomControl="False"
            ShowExportControls="true" ShowBackButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
