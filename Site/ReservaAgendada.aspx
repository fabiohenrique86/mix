<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="ReservaAgendada.aspx.cs"
    Inherits="Site.ReservaAgendada" Title="CADASTRO | CONSULTA - RESERVA AGENDADA" %>

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
                    CONSULTA | CADASTRO | ATUALIZAÇÃO | EXCLUSÃO - RESERVA AGENDADA
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblReservaID" runat="server" Text="PedidoID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtReservaID" runat="server" Width="65px" MaxLength="7" SkinID="TextBox"></asp:TextBox>                               
                                <cc1:FilteredTextBoxExtender ID="ftbReservaID" runat="server" FilterType="Numbers"
                                    TargetControlID="txtReservaID">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblDataReserva" runat="server" Text="Data Reserva" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataReserva" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDataReserva"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblDataEntrega" runat="server" Text="Data Entrega" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataEntrega" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataEntrega_MaskedEditExtender" runat="server" TargetControlID="txtDataEntrega"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                                <asp:CheckBox ID="ckbDataEntrega" runat="server" Text="Sem Data Entrega (A PROGRAMAR)"
                                    Checked="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblStatusID" runat="server" Text="Status (Entrega)" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlStatus" runat="server" SkinID="DropDownListRelatorio" Width="100px">
                                    <asp:ListItem Text="SELECIONE" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Antes de Hoje" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Hoje" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Efetuada" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Trânsito" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblNomeCarreteiro" runat="server" Text="Carreteiro" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtNomeCarreteiro" runat="server" Width="350px" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                Legenda :&nbsp;&nbsp;
                                <img src="../img/vermelho.png" alt="" />&nbsp;Antes de hoje&nbsp;&nbsp;&nbsp;
                                <img src="../img/azul.png" alt="" />&nbsp;Hoje&nbsp;&nbsp;&nbsp;
                                <img src="../img/preto.png" alt="" />&nbsp;Maior do que hoje&nbsp;&nbsp;&nbsp;
                                <img src="../img/verde.png" alt="" />&nbsp;Efetuada&nbsp;&nbsp;&nbsp;
                                <img src="../img/laranja.png" alt="" />&nbsp;Trânsito
                            </td>
                        </tr>                        
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png" OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbProrrogar" runat="server" ImageUrl="~/img/prorrogar.png" OnClick="imbProrrogar_Click" />
                                <asp:ImageButton ID="imbDarBaixa" runat="server" ImageUrl="~/img/dar_baixa.png" OnClick="imbDarBaixa_Click" />
                                <asp:ImageButton ID="imbTransito" runat="server" ImageUrl="~/img/transito.png"  onclick="imbTransito_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <asp:GridView ID="gdvReservaAgendada" runat="server" SkinID="GridViewFooter" OnRowDataBound="gdvReservaAgendada_RowDataBound"
                        OnPageIndexChanging="gdvReservaAgendada_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="PedidoID">
                                <ItemTemplate>
                                    <asp:Label ID="lblReservaID" runat="server" Text='<%# Bind("ReservaID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Reserva">
                                <ItemTemplate>
                                    <asp:Label ID="lblDataReserva" runat="server" Text='<%# Bind("DataReserva","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data Entrega">
                                <ItemTemplate>
                                    <asp:Label ID="lblDataEntrega" runat="server" Text='<%# Bind("DataEntrega","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Carreteiro">
                                <ItemTemplate>
                                    <asp:Label ID="lblNmCarreteiro" runat="server" Text='<%# Bind("NomeCarreteiro") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status (Entrega)">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatusID" runat="server" Text='<%# Bind("StatusID") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
