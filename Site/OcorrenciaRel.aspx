<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="OcorrenciaRel.aspx.cs" Inherits="Site.OcorrenciaRel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="imbVisualizar" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="imbVisualizar">
                <div id="conteudo">
                    <div id="topo_cabeca">
                        RELATÓRIO - OCORRÊNCIA
                    </div>
                    <div>
                        <table width="100%">
                            <tr style="background-color: #FFD700; font-weight: bold; color: #003366; text-align: center;">
                                <td colspan="2">FILTRO
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="label7" runat="server" Text="OcorrenciaID" SkinID="Label"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtOcorrenciaID" runat="server" Width="85px" MaxLength="10" SkinID="TextBox"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtOcorrenciaID"
                                        FilterType="Numbers">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblLoja" runat="server" Text="Loja" SkinID="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLoja" runat="server" SkinID="DropDownList" DataTextField="NomeFantasia"
                                        DataValueField="LojaID" Width="600px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="label11" runat="server" Text="Status Ocorrência" SkinID="Label"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlStatusOcorrencia" runat="server" SkinID="DropDownListRelatorio" Width="100px">
                                        <asp:ListItem Text="SELECIONE" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Pendente" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Realizada" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="label2" runat="server" Text="Motivo da Ocorrência" SkinID="Label"></asp:Label>
                                </td>
                                <td style="text-align: left;" colspan="7">
                                    <asp:DropDownList ID="ddlMotivoOcorrencia" runat="server" SkinID="DropDownList" DataTextField="Descricao" DataValueField="MotivoOcorrenciaID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblDataOcorrencia" runat="server" Text="Data Ocorrência" SkinID="Label"></asp:Label>
                                </td>
                                <td style="text-align: left; width: 220px;">
                                    <asp:TextBox ID="txtDataOcorrenciaInicial" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="txtDataReservaInicial_MaskedEditExtender1" runat="server" TargetControlID="txtDataOcorrenciaInicial"
                                        MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                    </cc1:MaskedEditExtender>
                                    &nbsp;a&nbsp;
                                    <asp:TextBox ID="txtDataOcorrenciaFinal" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="txtDataReservaFinal_MaskedEditExtender2" runat="server" TargetControlID="txtDataOcorrenciaFinal"
                                        MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:ImageButton ID="imbVisualizar" runat="server" ImageUrl="~/img/visualizar.png" OnClick="imbVisualizar_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeOcorrencia" runat="server" CancelControlID="imbFechar"
        TargetControlID="hdfGerar" PopupControlID="pnlRelatorio" BackgroundCssClass="background_modal"
        DropShadow="false">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlRelatorio" runat="server" CssClass="window_modal" Width="740px"
        Height="430px" Style="display: none;">
        <rsweb:ReportViewer ID="rpvOcorrencia" runat="server" Width="740px" Font-Names="Verdana"
            Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="true" ShowRefreshButton="False" ShowZoomControl="False"
            ShowExportControls="true" ShowBackButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
