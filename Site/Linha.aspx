<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Linha.aspx.cs"
    Inherits="Site.Linha" Title="CADASTRO | CONSULTA | ATUALIZAÇÃO | EXCLUSÃO - LINHA" %>

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
                                <asp:Label ID="lblLinhaID" runat="server" Text="LinhaID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtLinhaID" runat="server" Width="50px" MaxLength="3" Enabled="false"
                                    CssClass="desabilitado" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtLinhaID_FilteredTextBoxExtender" runat="server"
                                    FilterType="Numbers" TargetControlID="txtLinhaID">
                                </cc1:FilteredTextBoxExtender>
                                <asp:CheckBox ID="ckbLinhaID" runat="server" Text="Atualizar/Excluir" OnCheckedChanged="ckbLinhaID_CheckedChanged"
                                    AutoPostBack="true" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblDescricao" runat="server" Text="Linha" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDescricao" runat="server" Width="300px" MaxLength="200" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblDesconto" runat="server" Text="Desconto (%)" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDesconto" runat="server" Width="35px" MaxLength="5" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                                    OnClick="imbCadastrar_Click" Visible="false" />
                                <asp:ImageButton ID="imbAtualizar" runat="server" ImageUrl="~/img/atualizar.png"
                                    Visible="false" OnClick="imbAtualizar_Click" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" OnClick="imbExcluir_Click"
                                    Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <asp:GridView ID="gdvLinha" runat="server" OnRowDataBound="gdvLinha_RowDataBound"
                        SkinID="GridView">
                        <Columns>
                            <asp:BoundField DataField="LinhaID" HeaderText="ID" ItemStyle-Width="5%" />
                            <asp:BoundField DataField="Descricao" HeaderText="Linha" ItemStyle-Width="85%" />
                            <asp:BoundField DataField="Desconto" HeaderText="Desconto (%)" ItemStyle-Width="10%" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
