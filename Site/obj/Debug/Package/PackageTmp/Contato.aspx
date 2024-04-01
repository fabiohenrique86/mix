<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Contato.aspx.cs"
    Inherits="Site.Contato" Title="CONTATO" %>

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
                    CONTATO
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblNome" runat="server" Text="Nome" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtNome" runat="server" Width="300px" MaxLength="100" SkinID="TextBox"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNome" runat="server" ErrorMessage="campo obrigatório"
                                    ControlToValidate="txtNome"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblEmail" runat="server" Text="E-mail" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtEmail" runat="server" Width="220px" MaxLength="100" SkinID="TextBox"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="campo obrigatório"
                                    ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="E-mail inválido" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblMensagem" runat="server" Text="Mensagem" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtMensagem" runat="server" Width="820px" TextMode="MultiLine" Height="200px"
                                    SkinID="TextBox"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMensagem" runat="server" ErrorMessage="campo obrigatório"
                                    ControlToValidate="txtMensagem"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:ImageButton ID="imbEnviar" runat="server" ImageUrl="~/img/enviar.png" OnClick="imbEnviar_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="imbLimpar" runat="server" ImageUrl="~/img/limpar.png" OnClick="imbLimpar_Click"
                                    CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
