<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="NotaFiscalRel.aspx.cs"
    Inherits="Site.NotaFiscalRel" Title="RELATÓRIO - NOTA FISCAL" %>

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
                        RELATÓRIO - NOTA FISCAL
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
                                    <asp:Label ID="lblNotaFiscalID" runat="server" Text="NotaFiscalID" SkinID="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNotaFiscalID" runat="server" SkinID="TextBox" Width="70px" MaxLength="9"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="txtNotaFiscalID_FilteredTextBoxExtender" runat="server"
                                        TargetControlID="txtNotaFiscalID" FilterType="Numbers">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblPedidoMae" runat="server" Text="PedidoMãeID" SkinID="Label"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtPedidoMaeID" runat="server" SkinID="TextBox" Width="155px" MaxLength="30"></asp:TextBox>
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
                           <%-- <tr>
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
                    <%--<div>
                <rsweb:ReportViewer ID="rpvNotaFiscal" runat="server" Width="100%" Font-Names="Segoe UI,Verdana"
                    Font-Size="8pt" Visible="False" Height="400px">
                    <LocalReport ReportPath="NotaFiscal.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="sdsNotaFiscal" Name="dsNotaFiscal_spListarNotaFiscal" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                <asp:SqlDataSource ID="sdsNotaFiscal" runat="server" SelectCommand="spListarNotaFiscal"
                    SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:Mix %>">
                    <SelectParameters>
                        <asp:SessionParameter DbType="Date" SessionField="DiaMesAnoInicial" Name="DataNotaFiscalInicial" />
                        <asp:SessionParameter DbType="Date" SessionField="DiaMesAnoFinal" Name="DataNotaFiscalFinal" />
                        <asp:SessionParameter Name="SistemaID" SessionField="SistemaID" Type="Int32" />
                        <asp:ControlParameter ControlID="txtNotaFiscalID" Name="NotaFiscalID" PropertyName="Text"
                            Type="Int32" DefaultValue="0" />
                        <asp:ControlParameter ControlID="txtPedidoMaeID" Name="PedidoMaeID" PropertyName="Text"
                            Type="Int32" DefaultValue="0" />
                        <asp:ControlParameter ControlID="ddlLoja" Name="LojaID" PropertyName="SelectedValue"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="ddlLinha" Name="LinhaID" PropertyName="SelectedValue"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="ddlProduto" Name="ProdutoID" PropertyName="SelectedValue"
                            Type="Int64" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>--%>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeNotaFiscal" runat="server" CancelControlID="imbFechar"
        TargetControlID="hdfGerar" PopupControlID="pnlRelatorio" BackgroundCssClass="background_modal"
        DropShadow="false">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlRelatorio" runat="server" CssClass="window_modal" Width="740px"
        Height="430px" Style="display: none;">
        <rsweb:ReportViewer ID="rpvNotaFiscal" runat="server" Width="740px" Font-Names="Verdana"
            Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="true" ShowRefreshButton="False" ShowZoomControl="False"
            ShowExportControls="true" ShowBackButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
