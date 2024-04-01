<%@ Page Title="CONSULTA | CADASTRO - PEDIDO AGENDADO - LIMITE" Language="C#" MasterPageFile="~/Base.Master"
    AutoEventWireup="true" CodeBehind="LimiteReserva.aspx.cs" Inherits="Site.LimiteReserva" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="imbAtualizar">
                <div id="conteudo">
                    <div id="topo_cabeca">
                        <asp:Label ID="lblTopo" runat="server" Text=""></asp:Label>
                    </div>
                    <div id="cabeca">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 60px">
                                    <asp:Label ID="lblLimite" runat="server" Text="Limite" SkinID="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLimite" runat="server" Width="55px" MaxLength="3" SkinID="TextBox"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="ftetxtLimite" runat="server" TargetControlID="txtLimite"
                                        FilterType="Numbers">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ImageButton ID="imbAtualizar" runat="server" 
                                        ImageUrl="~/img/atualizar.png" onclick="imbAtualizar_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="font-size: 10px;">
                        <asp:GridView ID="gdvLimiteReserva" runat="server" SkinID="GridView">
                            <Columns>
                                <asp:BoundField DataField="Sistema" HeaderText="Sistema" ItemStyle-Width="80%" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="LimiteReserva" HeaderText="Limite" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
