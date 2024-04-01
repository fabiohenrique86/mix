<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Usuario.aspx.cs"
    Inherits="Site.Usuario" Title="CADASTRO | CONSULTA | ATUALIZAÇÃO | EXCLUSÃO - USUÁRIO" %>

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
            <div id="conteudo">
                <div id="topo_cabeca">
                    CADASTRO - USUÁRIO
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style3">
                                <asp:Label ID="lblUsuarioID" runat="server" Text="UsuarioID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtUsuarioID" runat="server" Width="50px" MaxLength="3" CssClass="desabilitado"
                                    Enabled="false" SkinID="TextBox"></asp:TextBox>
                                <asp:CheckBox ID="ckbUsuarioID" runat="server" AutoPostBack="true" Text="Atualizar/Excluir"
                                    OnCheckedChanged="ckbUsuarioID_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="lblLoja" runat="server" Text="Loja" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlLoja" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlLoja_SelectedIndexChanged" SkinID="DropDownList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="lblTipoUsuario" runat="server" Text="Tipo Usuário" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlTipoUsuario" runat="server" DataTextField="Descricao" DataValueField="TipoUsuarioID"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlTipoUsuario_SelectedIndexChanged"
                                    SkinID="DropDownList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="lblLogin" runat="server" Text="Login" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtLogin" runat="server" MaxLength="30" Width="250px" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                <asp:Label ID="lblSenha" runat="server" Text="Senha" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtSenha" runat="server" MaxLength="30" Width="250px" TextMode="Password"
                                    SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: left;">
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                                    OnClick="imbCadastrar_Click" />
                                <asp:ImageButton ID="imbAtualizar" runat="server" ImageUrl="~/img/atualizar.png"
                                    OnClick="imbAtualizar_Click" Visible="false" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" OnClick="imbExcluir_Click"
                                    Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:GridView ID="gdvUsuario" runat="server" SkinID="GridViewFooter" OnPageIndexChanging="gdvUsuario_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="UsuarioID" HeaderText="UsuarioID" ItemStyle-Width="5%" />
                            <asp:BoundField DataField="Loja" HeaderText="Loja" ItemStyle-Width="40%" />
                            <asp:BoundField DataField="TipoUsuario" HeaderText="Tipo Usuário" ItemStyle-Width="15%" />
                            <asp:BoundField DataField="Login" HeaderText="Login" ItemStyle-Width="20%" />
                            <asp:BoundField DataField="Senha" HeaderText="Senha" ItemStyle-Width="20%" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
