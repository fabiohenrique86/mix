<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="NotaFiscalRel.aspx.cs"
    Inherits="Site.NotaFiscalRel" Title="RELATÓRIO - NOTA FISCAL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="imbGerar" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="imbGerar">
                <div id="conteudo">
                    <div id="topo_cabeca">
                        RELATÓRIO - NOTA FISCAL
                    </div>
                    <div>
                        <table width="100%">                            
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblNotaFiscalID" runat="server" Text="NotaFiscalID" SkinID="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNotaFiscalID" runat="server" SkinID="TextBox" Width="100px" MaxLength="9"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="txtNotaFiscalID_FilteredTextBoxExtender" runat="server"
                                        TargetControlID="txtNotaFiscalID" FilterType="Numbers">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblDataNotaFiscal" runat="server" Text="Data Nota Fiscal" SkinID="Label"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtDataNotaFiscalDe" runat="server" Width="65px" Height="15px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="txtDataNotaFiscalDe_MaskedEditExtender" runat="server" TargetControlID="txtDataNotaFiscalDe"
                                        MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                    </cc1:MaskedEditExtender>
                                    &nbsp;&nbsp;a&nbsp;&nbsp;
                                    <asp:TextBox ID="txtDataNotaFiscalAte" runat="server" Width="65px" Height="15px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="txtDataNotaFiscalAte_MaskedEditExtender" runat="server" TargetControlID="txtDataNotaFiscalAte"
                                        MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblNumeroCarga" runat="server" Text="Número Carga" SkinID="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumeroCarga" runat="server" SkinID="TextBox" Width="155px"></asp:TextBox>
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
                                    <asp:ImageButton ID="imbGerar" runat="server" ImageUrl="~/img/gerar.png"
                                        OnClick="imbGerar_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
