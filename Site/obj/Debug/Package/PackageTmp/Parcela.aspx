<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Parcela.aspx.cs"
    Inherits="Site.Parcela" Title="CADASTRO | CONSULTA | ATUALIZAÇÃO | EXCLUSÃO - PARCELA" %>

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
                                <asp:Label ID="lblParcelaID" runat="server" Text="ParcelaID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtParcelaID" runat="server" Width="50px" MaxLength="3" CssClass="desabilitado"
                                    Enabled="false" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtParcelaID_FilteredTextBoxExtender" runat="server"
                                    FilterType="Numbers" TargetControlID="txtParcelaID">
                                </cc1:FilteredTextBoxExtender>
                                <asp:CheckBox ID="ckbParcelaID" AutoPostBack="true" Text="Atualizar/Excluir/Cadastrar"
                                    runat="server" OnCheckedChanged="ckbParcelaID_CheckedChanged" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblPrazoMedio" runat="server" Text="Prazo Médio (Dias)" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtPrazoMedio" runat="server" Width="50px" MaxLength="3" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtPrazoMedio_FilteredTextBoxExtender" runat="server"
                                    FilterType="Numbers" TargetControlID="txtPrazoMedio">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                                    OnClick="imbCadastrar_Click" Visible="false" />
                                <asp:ImageButton ID="imbAtualizar" runat="server" ImageUrl="~/img/atualizar.png"
                                    OnClick="imbAtualizar_Click" Visible="false" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" OnClick="imbExcluir_Click"
                                    Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <asp:GridView ID="gdvParcela" runat="server" SkinID="GridView">
                        <Columns>
                            <asp:BoundField DataField="ParcelaID" HeaderText="ParcelaID" ItemStyle-Width="80%" />
                            <asp:BoundField DataField="PrazoMedio" HeaderText="Prazo Médio (Dias)" ItemStyle-Width="20%" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
