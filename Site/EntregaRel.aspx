<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="EntregaRel.aspx.cs" Inherits="Site.EntregaRel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="DiaMesAno.ascx" TagName="DiaMesAno" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="imbVisualizar" />
            <asp:PostBackTrigger ControlID="imbGerar" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="imbVisualizar">
                <div id="conteudo">
                    <div id="topo_cabeca">
                        RELATÓRIO - ENTREGA / TROCA
                    </div>
                    <uc1:DiaMesAno ID="DiaMesAno1" runat="server" />
                    <div>
                        <table width="100%">
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
                                <td style="width: 100px">
                                    <asp:Label ID="lblPedidoID" runat="server" Text="PedidoID / TrocaID" SkinID="Label"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtPedidoID" runat="server" SkinID="TextBox" Width="155px" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblNome" runat="server" Text="Nome Cliente" SkinID="Label"></asp:Label>
                                </td>
                                <td style="text-align: left;" colspan="7">
                                    <asp:TextBox ID="txtNomeCliente" runat="server" Width="400px" MaxLength="200" SkinID="TextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblBairro" runat="server" Text="Bairro" SkinID="Label"></asp:Label>
                                </td>
                                <td style="text-align: left; width: 220px;">
                                    <asp:TextBox ID="txtBairro" runat="server" Width="180px" MaxLength="50" SkinID="TextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblStatus" runat="server" Text="Status" SkinID="Label"></asp:Label>
                                </td>
                                <td style="text-align: left; width: 220px;">
                                    <asp:DropDownList ID="ddlStatus" runat="server" SkinID="DropDownListRelatorio" Width="100px">
                                        <asp:ListItem Text="SELECIONE" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Pendente" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Efetuada" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Trânsito" Value="5"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:ImageButton ID="imbVisualizar" runat="server"
                                        ImageUrl="~/img/visualizar.png" OnClick="imbVisualizar_Click" />
                                    <asp:ImageButton ID="imbGerar" runat="server"
                                        ImageUrl="~/img/gerar.png" OnClick="imbGerar_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeEntrega" runat="server" CancelControlID="imbFechar"
        TargetControlID="hdfGerar" PopupControlID="pnlRelatorio" BackgroundCssClass="background_modal"
        DropShadow="false">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlRelatorio" runat="server" CssClass="window_modal" Width="740px"
        Height="430px" Style="display: none;">
        <rsweb:ReportViewer ID="rpvEntrega" runat="server" Width="740px" Font-Names="Verdana"
            Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="true" ShowRefreshButton="False" ShowZoomControl="False"
            ShowExportControls="true" ShowBackButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
