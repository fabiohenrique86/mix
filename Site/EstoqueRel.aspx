<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="EstoqueRel.aspx.cs"
    Inherits="Site.EstoqueRel" Title="RELATÓRIO - ESTOQUE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="imbVisualizar" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="imbVisualizar">
                <div id="conteudo">
                    <div id="topo_cabeca">
                        RELATÓRIO - ESTOQUE
                    </div>
                    <table id="tabela" width="100%">
                        <tr style="text-align: center;">
                            <td colspan="2">
                                <asp:Label ID="lblMsgTopo" runat="server" Text="Clique no botão abaixo para visualizar o relatório"
                                    SkinID="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="background-color: #FFD700; font-weight: bold; color: #003366; text-align: center;">
                            <td colspan="2">FILTRO
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
                            <td class="style2">Arquivo COLETA
                            </td>
                            <td colspan="9">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <asp:ImageButton ID="imbVisualizar" runat="server" ImageUrl="~/img/visualizar.png"
                                    OnClick="imbVisualizar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeEstoque" runat="server" CancelControlID="imbFechar"
        TargetControlID="hdfGerar" PopupControlID="pnlRelatorio" BackgroundCssClass="background_modal"
        DropShadow="false">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlRelatorio" runat="server" CssClass="window_modal" Width="740px"
        Height="430px" Style="display: none;">
        <rsweb:ReportViewer ID="rpvEstoque" runat="server" Width="740px" Font-Names="Verdana"
            Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="true" ShowRefreshButton="False" ShowZoomControl="False"
            ShowExportControls="true" ShowBackButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
