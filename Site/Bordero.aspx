<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Bordero.aspx.cs"
    Inherits="Site.Bordero" Title="CONSULTA | CADASTRO - BORDERÔ" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="DiaMesAno.ascx" TagName="DiaMesAno" TagPrefix="uc1" %>
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
            <asp:Panel ID="Panel1" runat="server" DefaultButton="imbConsultar">
                <div id="conteudo">
                    <div id="topo_cabeca">
                        <asp:Label ID="lblTopo" runat="server" Text=""></asp:Label>
                    </div>
                    <div id="cabeca">
                        <uc1:DiaMesAno ID="DiaMesAno1" runat="server" />
                        <table style="width: 100%;">
                            <tr>
                                <td style="text-align: center;">
                                    <asp:Label ID="lblLoja" runat="server" Text="Loja" SkinID="Label"></asp:Label>
                                    &nbsp;
                                    <asp:DropDownList ID="ddlLoja" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID"
                                        Font-Names="Segoe UI" Font-Size="10px" ForeColor="Black" Width="385px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="text-align: center;">
                        <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                            OnClick="imbConsultar_Click" />
                        <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                            Visible="false" />
                    </div>
                    <div style="font-size: 10px; overflow-x: hidden; overflow-y: scroll; height: 300px;">
                        <asp:GridView ID="gdvBordero" runat="server" SkinID="GridView" OnRowDataBound="gdvBordero_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="PedidoID" HeaderText="PedidoID" ItemStyle-Width="50%" />
                                <asp:BoundField DataField="TotalPedido" HeaderText="Valor" ItemStyle-Width="50%"
                                    DataFormatString="{0:c}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="DivTotalPago" style="text-align: right;" runat="server" visible="false">
                        <asp:Label ID="lblTotalPago" runat="server" Text="Total : " class="negrito"></asp:Label>
                        <asp:TextBox ID="txtTotalPago" runat="server" Enabled="false" Width="80px" SkinID="TextBox" />
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
