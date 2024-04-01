<%@ Page Language="C#" MasterPageFile="~/administrador/BaseAdm.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Site.administrador.Default" Title="Administrador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            height: 17px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="conteudoadm">
                <div id="tabela" runat="server">
                    <table>
                        <tr>
                            <td colspan="3" class="style1">
                                Informe o Login e Senha para entrar no sistema
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblLogin" runat="server" Text="Login"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtLogin" runat="server" Width="200px" MaxLength="30"></asp:TextBox>
                            </td>
                            <td class="style2">
                                <asp:RequiredFieldValidator ID="rfvLogin" runat="server" ErrorMessage="Informe o Login"
                                    ControlToValidate="txtLogin"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblSenha" runat="server" Text="Senha"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtSenha" runat="server" Width="200px" MaxLength="30" TextMode="Password"></asp:TextBox>
                            </td>
                            <td class="style2">
                                <asp:RequiredFieldValidator ID="rfvSenha" runat="server" ErrorMessage="Informe a Senha"
                                    ControlToValidate="txtSenha"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                &nbsp;
                            </td>
                            <td>
                                <asp:ImageButton ID="imbLogin" runat="server" ImageUrl="~/img/entrar.png" OnClick="imbLogin_Click" />
                            </td>
                            <td class="style2">
                                <asp:Label ID="lblMensagem" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="bemvindo" runat="server">
                    <p>
                        BEM VINDO AO SISTEMA MiX
                    </p>
                    <p>
                        CRIE TIPOS DE USUÁRIOS
                    </p>
                    <p>
                        CRIE, ATUALIZE OU EXCLUA SISTEMAS
                    </p>
                    <p>
                        ACESSANDO AS FUNCIONALIDADES DO MENU ACIMA
                    </p>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
