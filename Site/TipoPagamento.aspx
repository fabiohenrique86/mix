<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="TipoPagamento.aspx.cs"
    Inherits="Site.TipoPagamento" Title="CADASTRO | CONSULTA | ATUALIZAÇÃO | EXCLUSÃO - TIPO DE PAGAMENTO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
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
                    <asp:Label ID="lblTopo" runat="server" Text=""></asp:Label>
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblTipoPagamentoID" runat="server" Text="TipoPagamentoID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtTipoPagamentoID" runat="server" Width="50px" MaxLength="3" CssClass="desabilitado"
                                    Enabled="false" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtTipoPagamentoID_FilteredTextBoxExtender" runat="server"
                                    FilterType="Numbers" TargetControlID="txtTipoPagamentoID">
                                </cc1:FilteredTextBoxExtender>
                                <asp:CheckBox ID="ckbTipoPagamentoID" runat="server" Text="Atualizar/Excluir" AutoPostBack="true"
                                    OnCheckedChanged="ckbTipoPagamentoID_CheckedChanged" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblDescricao" runat="server" Text="Tipo Pagamento" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDescricao" runat="server" Width="350px" MaxLength="100" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbCadastrar" runat="server" OnClick="imbCadastrar_Click" ImageUrl="~/img/cadastrar.png"
                                    Visible="false" />
                                <asp:ImageButton ID="imbAtualizar" runat="server" ImageUrl="~/img/atualizar.png"
                                    OnClick="imbAtualizar_Click" Visible="false" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" OnClick="imbExcluir_Click"
                                    Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <asp:GridView ID="gdvTipoPagamento" runat="server" SkinID="GridView" OnRowDataBound="gdvTipoPagamento_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="TipoPagamentoID" HeaderText="ID" ItemStyle-Width="5%" />
                            <asp:BoundField DataField="Descricao" HeaderText="Tipo Pagamento" ItemStyle-Width="95%" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
