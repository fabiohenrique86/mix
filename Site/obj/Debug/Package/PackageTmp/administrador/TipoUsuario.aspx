<%@ Page Language="C#" MasterPageFile="~/administrador/BaseAdm.Master" AutoEventWireup="true"
    CodeBehind="TipoUsuario.aspx.cs" Inherits="Site.administrador.TipoUsuario" Title="ADMINISTRADOR - TIPO USUÁRIO" %>

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
                                <asp:Label ID="lblTipoUsuarioID" runat="server" Text="TipoUsuarioID"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtTipoUsuarioID" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="lblDescricao" runat="server" Text="Descrição"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDescricao" runat="server" MaxLength="150" Width="450px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="color: Red">
                                1 - Administrador&nbsp;|&nbsp;2 - Estoquista&nbsp;|&nbsp;3 - Gerente&nbsp;|&nbsp;4 - Vendedor&nbsp;|&nbsp;5 - Conferentista
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: left;">
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                                    OnClick="imbCadastrar_Click" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" OnClick="imbExcluir_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:GridView ID="gdvTipoUsuario" runat="server" Width="100%" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="TipoUsuarioID" HeaderText="TipoUsuarioID" />
                            <asp:BoundField DataField="Descricao" HeaderText="Descrição" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
