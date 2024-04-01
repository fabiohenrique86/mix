<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="Site.Default" Title="MiX - Sistema Para Franquias" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 35px;
        }
        .style2
        {
            width: 300px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    <asp:Label ID="lblTopoCabeca" runat="server" Text="INÍCIO"></asp:Label>
                </div>
                <div style="padding-left: 350px;">
                    <div id="listaEmpresa" runat="server">
                        Escolha um dos clientes abaixo para acessar o sistema
                        <asp:RadioButtonList ID="rblEmpresa" runat="server" OnSelectedIndexChanged="rblEmpresa_SelectedIndexChanged"
                            AutoPostBack="true" DataTextField="Descricao" DataValueField="SistemaID">
                        </asp:RadioButtonList>
                    </div>
                    <table id="tabela" runat="server">
                        <tr>
                            <td colspan="3">
                                Informe o Login e Senha para entrar no sistema
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblLogin" runat="server" Text="Login"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtLogin" runat="server" Width="200px" MaxLength="30" SkinID="TextBox"></asp:TextBox>
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
                                <asp:TextBox ID="txtSenha" runat="server" Width="200px" MaxLength="30" TextMode="Password"
                                    SkinID="TextBox"></asp:TextBox>
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
                            <td style="text-align: left;">
                                <asp:ImageButton ID="imbLogin" runat="server" ImageUrl="~/img/entrar.png" OnClick="imbLogin_Click" />
                                <asp:ImageButton ID="imbSair" runat="server" ImageUrl="~/img/sair.png" OnClick="imbSair_Click"
                                    CausesValidation="False" />
                            </td>
                            <td class="style2">
                                <asp:Label ID="lblMensagem" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="home" runat="server" style="text-align: center;" visible="false">
                    <asp:Label ID="lblMensagemHome" runat="server"></asp:Label>
                    <asp:SqlDataSource ID="sdsFaturamento" runat="server" SelectCommand="spListarFaturamento"
                        SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:Mix %>">
                        <SelectParameters>
                            <asp:SessionParameter Name="SistemaID" SessionField="SistemaID" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <div>
                        <asp:GridView ID="gdvFaturamento" runat="server" SkinID="GridView" DataSourceID="sdsFaturamento"
                            AutoGenerateColumns="False" Visible="False">
                            <Columns>
                                <asp:BoundField DataField="NomeFantasia" HeaderText="Loja" />
                                <asp:BoundField DataField="Faturamento" DataFormatString="{0:c}" HeaderText="Faturamento" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
