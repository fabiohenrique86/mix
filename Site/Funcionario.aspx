<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Funcionario.aspx.cs"
    Inherits="Site.Funcionario" Title="CADASTRO | CONSULTA | ATUALIZAÇÃO | EXCLUSÃO - FUNCIONÁRIO" %>

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
                    <table width="100%">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblFuncionarioID" runat="server" Text="FuncionárioID" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFuncionarioID" runat="server" Width="50px" MaxLength="5" CssClass="desabilitado"
                                    Enabled="false" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtFuncionarioID_FilteredTextBoxExtender" runat="server"
                                    FilterType="Numbers" TargetControlID="txtFuncionarioID">
                                </cc1:FilteredTextBoxExtender>
                                <asp:CheckBox ID="ckbFuncionarioID" AutoPostBack="true" Text="Atualizar/Inativar/Cadastrar"
                                    runat="server" OnCheckedChanged="ckbFuncionarioID_CheckedChanged" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblNome" runat="server" Text="Nome Funcionário" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtNome" runat="server" Width="350px" MaxLength="150" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblLoja" runat="server" Text="Loja" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlLoja" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID"
                                    SkinID="DropDownList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTelefone" runat="server" Text="Telefone" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelefone" runat="server" MaxLength="10" Width="80px" SkinID="TextBox"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtTelefone_MaskedEditExtender" runat="server" TargetControlID="txtTelefone"
                                    Mask="(99)9999-9999" MaskType="Number" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblEmail" runat="server" Text="E-mail" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtEmail" runat="server" Width="220px" MaxLength="100" SkinID="TextBox"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="E-mail inválido" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ValidationGroup="formulario" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                                    OnClick="imbCadastrar_Click" Visible="false" ValidationGroup="formulario" />
                                <asp:ImageButton ID="imbAtualizar" runat="server" ImageUrl="~/img/atualizar.png"
                                    Visible="false" OnClick="imbAtualizar_Click" ValidationGroup="formulario" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/inativar.png" Visible="false"
                                    OnClick="imbExcluir_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="font-size: 10px; overflow-x: hidden; overflow-y: scroll; height: 325px;">
                    <asp:Repeater ID="rptLoja" runat="server" OnItemDataBound="rptLoja_ItemDataBound">
                        <ItemTemplate>
                            <div class="topo_grid">
                                <asp:Label ID="lblLojaID" runat="server" Text='<%# Bind("LojaID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblLoja" runat="server" Text='<%# Bind("NomeFantasia") %>'></asp:Label>
                            </div>
                            <div>
                                <asp:GridView ID="gdvFuncionario" runat="server" SkinID="GridView" OnRowDataBound="gdvFuncionario_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="FuncionarioID" HeaderText="FuncionarioID" ItemStyle-Width="10%" />
                                        <asp:BoundField DataField="Nome" HeaderText="Nome" ItemStyle-Width="60%" />
                                        <asp:BoundField DataField="Telefone" HeaderText="Telefone" ItemStyle-Width="15%" />
                                        <asp:BoundField DataField="Email" HeaderText="E-mail" ItemStyle-Width="15%" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
