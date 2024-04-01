<%@ Page Language="C#" MasterPageFile="~/administrador/BaseAdm.Master" AutoEventWireup="true"
    CodeBehind="Sistema.aspx.cs" Inherits="Site.administrador.Sistema" Title="ADMINISTRADOR - SISTEMA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style3
        {
            width: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="conteudoadm">
                <div>
                    <table style="width: 100%;">
                        <tr>
                            <td class="style3">
                                <asp:Label ID="lblSistemaID" runat="server" Text="SistemaID"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtSistemaID" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="lblDescricao" runat="server" Text="Descrição"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDescricao" runat="server" MaxLength="400" Width="500px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="lblStatusID" runat="server" Text="StatusID"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:RadioButtonList ID="rblStatus" runat="server" Width="100px" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Ativo" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Inativo" Value="4"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: left;">
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                                    OnClick="imbCadastrar_Click" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" OnClick="imbExcluir_Click" />
                                <asp:ImageButton ID="imbAtualizar" runat="server" ImageUrl="~/img/atualizar.png"
                                    OnClick="imbAtualizar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:GridView ID="gdvSistema" runat="server" Width="100%" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="SistemaID" HeaderText="SistemaID" ItemStyle-Width="20%" />
                            <asp:BoundField DataField="Descricao" HeaderText="Descrição" ItemStyle-Width="60%" />
                            <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="20%" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
