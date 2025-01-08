<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Loja.aspx.cs"
    Inherits="Site.Loja" Title="CADASTRO | CONSULTA | ATUALIZAÇÃO | EXCLUSÃO - LOJA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2
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
                            <td class="style2">
                                <asp:Label ID="lblLojaID" runat="server" Text="LojaID" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLojaID" runat="server" CssClass="desabilitado" Enabled="false"
                                    Width="40px" MaxLength="3" SkinID="TextBox"></asp:TextBox>
                                <asp:CheckBox ID="ckbLojaID" runat="server" AutoPostBack="true" Text="Atualizar/Inativar"
                                    OnCheckedChanged="ckbLojaID_CheckedChanged" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblCnpj" runat="server" Text="CNPJ" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCnpj" runat="server" MaxLength="14" Width="110px" SkinID="TextBox"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtCnpj_MaskedEditExtender" runat="server" TargetControlID="txtCnpj"
                                    ClearMaskOnLostFocus="False" Mask="99,999,999/9999-99">
                                </cc1:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblRazaoSocial" runat="server" Text="Razão Social" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRazaoSocial" runat="server" MaxLength="150" Width="450px" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblNomeFantasia" runat="server" Text="Nome Fantasia" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNomeFantasia" runat="server" MaxLength="150" Width="250px" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
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
                            <td class="style2">
                                <asp:Label ID="lblCota" runat="server" Text="Cota (R$)" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCota" runat="server" CssClass="decimal" MaxLength="10" Width="70px"
                                    SkinID="TextBox"></asp:TextBox>
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
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/inativar.png" OnClick="imbExcluir_Click"
                                    Visible="false" />
                                <cc1:ConfirmButtonExtender ID="imbExcluir_ConfirmButtonExtender" runat="server" ConfirmText="A inativação de uma Loja implica em inativar todos os Funcionários e Usuários relacionados a ela. Tem certeza que deseja continuar?"
                                    TargetControlID="imbExcluir">
                                </cc1:ConfirmButtonExtender>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <asp:GridView ID="gdvLoja" runat="server" OnRowDataBound="gdvLoja_RowDataBound" SkinID="GridViewFooter"
                        OnPageIndexChanging="gdvLoja_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="LojaID" HeaderText="LojaID" ItemStyle-Width="5%" />
                            <asp:BoundField DataField="Cnpj" HeaderText="CNPJ" ItemStyle-Width="10%" />
                            <asp:BoundField DataField="RazaoSocial" HeaderText="Razão Social" ItemStyle-Width="25%" />
                            <asp:BoundField DataField="NomeFantasia" HeaderText="Nome Fantasia" ItemStyle-Width="20%" />
                            <asp:BoundField DataField="Telefone" HeaderText="Telefone" ItemStyle-Width="9%" />
                            <asp:BoundField DataField="Cota" HeaderText="Cota" DataFormatString="{0:c}" ItemStyle-Width="15%" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
