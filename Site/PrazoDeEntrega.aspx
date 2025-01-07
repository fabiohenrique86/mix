<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="PrazoDeEntrega.aspx.cs"
    Inherits="Site.PrazoDeEntrega" Title="CADASTRO | CONSULTA | ATUALIZAÇÃO | EXCLUSÃO - PRAZO DE ENTREGA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style3 {
            width: 100px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    CONSULTA | CADASTRO | ATUALIZAÇÃO | EXCLUSÃO - PEDIDO AGENDADO - PRAZO DE ENTREGA
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style3">
                                <asp:Label ID="lblPrazoDeEntrega" runat="server" Text="Prazo De Entrega" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtPrazoDeEntrega" runat="server" Width="50px" MaxLength="3" Enabled="true" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: left;">
                                <asp:ImageButton ID="imbSalvar" runat="server" ImageUrl="~/img/atualizar.png"
                                    OnClick="imbSalvar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:GridView ID="gdvPrazoDeEntrega" runat="server" SkinID="GridViewFooter">
                        <Columns>
                            <asp:BoundField DataField="PrazoDeEntrega" HeaderText="Prazo De Entrega" ItemStyle-Width="100%" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
