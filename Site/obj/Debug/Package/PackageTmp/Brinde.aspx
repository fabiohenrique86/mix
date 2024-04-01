<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Brinde.aspx.cs"
    Inherits="Site.Brinde" Title="CADASTRO | CONSULTA - BRINDE" %>

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
                    <table style="width: 100%;">                        
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblProduto" runat="server" Text="Produto" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProduto" runat="server" Width="700px" DataTextField="Descricao"
                                    DataValueField="ProdutoID" SkinID="DropDownList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblBrinde" runat="server" Text="Brinde" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProdutoBrinde" runat="server" Width="700px" DataTextField="Descricao"
                                    DataValueField="ProdutoID" SkinID="DropDownList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblQuantidade" runat="server" Text="Quantidade" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQuantidade" runat="server" Width="30px" MaxLength="3" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtQuantidade_FilteredTextBoxExtender" runat="server"
                                    FilterType="Numbers" TargetControlID="txtQuantidade">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblPreco" runat="server" Text="Preço Brinde (R$)" SkinID="Label"></asp:Label>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtPreco" runat="server" MaxLength="10" SkinID="TextBox" CssClass="decimal"
                                    Width="70px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                                    OnClick="imbCadastrar_Click" />
                                <asp:ImageButton ID="imbCadastrarBrinde" runat="server" ImageUrl="~/img/cadastrar_mais.png"
                                    Visible="false" OnClick="imbCadastrarBrinde_Click" />
                                <asp:ImageButton ID="imbFinalizarBrinde" runat="server" ImageUrl="~/img/finalizar.png"
                                    Visible="false" OnClick="imbFinalizarBrinde_Click" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" OnClick="imbExcluir_Click" />
                            </td>
                        </tr>
                    </table>
                    <div>
                        <asp:Repeater ID="rptProduto" runat="server" OnItemDataBound="rptProduto_ItemDataBound">
                            <ItemTemplate>
                                <div class="topo_grid">
                                    <asp:Label ID="lblProdutoID" runat="server" Text='<%# Bind("ProdutoID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblDescricao" runat="server" Text='<%# Bind("Descricao") %>'></asp:Label>
                                </div>
                                <div>
                                    <asp:GridView ID="gdvBrinde" runat="server" SkinID="GridView">
                                        <Columns>
                                            <asp:BoundField DataField="Descricao" HeaderText="Brinde" ItemStyle-Width="80%" />
                                            <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" ItemStyle-Width="10%" />
                                            <asp:BoundField DataField="Preco1X" HeaderText="Preço" ItemStyle-Width="10%" DataFormatString="{0:c}" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
