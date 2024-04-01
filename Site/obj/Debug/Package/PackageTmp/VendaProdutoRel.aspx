<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="VendaProdutoRel.aspx.cs"
    Inherits="Site.VendaProdutoRel" Title="RELATÓRIO - VENDA - POR PRODUTO" %>

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
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="imbVisualizar">
                <div id="conteudo">
                    <div id="topo_cabeca">
                        RELATÓRIO - VENDA - POR PRODUTO
                    </div>
                    <uc1:DiaMesAno ID="DiaMesAno1" runat="server" />
                    <div>
                        <table width="100%">
                            <tr style="background-color: #FFD700; font-weight: bold; color: #003366; text-align: center;">
                                <td colspan="2">
                                    FILTRO
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
                                    <asp:Label ID="lblFuncionario" runat="server" Text="Funcionário" SkinID="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFuncionario" runat="server" SkinID="DropDownList" DataTextField="Nome"
                                        DataValueField="FuncionarioID" Width="600px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblLinha" runat="server" Text="Linha" SkinID="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLinha" runat="server" SkinID="DropDownList" DataTextField="Descricao"
                                        DataValueField="LinhaID" Width="600px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblProduto" runat="server" Text="Produto" SkinID="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProduto" runat="server" SkinID="DropDownList" DataTextField="Descricao"
                                        DataValueField="ProdutoID" Width="600px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:ImageButton ID="imbVisualizar" runat="server" ImageUrl="~/img/visualizar.png"
                                        OnClick="imbVisualizar_Click" />
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="2" style="text-align: center;">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/ajax-loader.gif" />
                                            &nbsp; <b>&nbsp;Aguarde...</b>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>--%>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeVendaProduto" runat="server" CancelControlID="imbFechar"
        TargetControlID="hdfGerar" PopupControlID="pnlRelatorio" BackgroundCssClass="background_modal"
        DropShadow="false">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlRelatorio" runat="server" CssClass="window_modal" Width="740px"
        Height="430px" Style="display: none;">
        <rsweb:ReportViewer ID="rpvVendaProduto" runat="server" Width="740px" Font-Names="Verdana"
            Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="true" ShowRefreshButton="False" ShowZoomControl="False"
            ShowExportControls="true" ShowBackButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
