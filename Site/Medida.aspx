<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Medida.aspx.cs"
    Inherits="Site.Medida" Title="CADASTRO | CONSULTA | ATUALIZAÇÃO | EXCLUSÃO - MEDIDA" %>

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
                                <asp:Label ID="lblMedidaID" runat="server" Text="MedidaID" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedidaID" runat="server" CssClass="desabilitado" Enabled="false"
                                    Width="50px" MaxLength="3" SkinID="TextBox"></asp:TextBox>
                                <asp:CheckBox ID="ckbMedidaID" runat="server" AutoPostBack="true" Text="Atualizar/Excluir"
                                    Visible="false" OnCheckedChanged="ckbMedidaID_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblMedida" runat="server" Text="Medida" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedida" runat="server" MaxLength="14" Width="83px" SkinID="TextBox" onkeypress="return FormatarSobMedida(this,'#,##X#,##X#,##')"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                                    OnClick="imbCadastrar_Click" />
                                <asp:ImageButton ID="imbAtualizar" runat="server" ImageUrl="~/img/atualizar.png"
                                    Visible="false" OnClick="imbAtualizar_Click" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" Visible="false"
                                    OnClick="imbExcluir_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <asp:GridView ID="gdvMedida" runat="server" OnRowDataBound="gdvMedida_RowDataBound"
                        SkinID="GridViewFooter" OnPageIndexChanging="gdvMedida_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="MedidaID" HeaderText="ID" ItemStyle-Width="5%" />
                            <asp:BoundField DataField="Descricao" HeaderText="Medida" ItemStyle-Width="95%" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
