<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MesAno.ascx.cs" Inherits="Site.MesAno" %>
<table id="tabela" width="100%">
    <tr style="text-align: center;">
        <td>
            <asp:Label ID="lblMsgTopo" runat="server" Text="Escolha os meses abaixo para visualizar o relatório"
                SkinID="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="background-color: #003366; font-weight: bold; color: #FFD700; text-align: center;">
            DE
        </td>
    </tr>
    <tr style="text-align: center;">
        <td>
            <asp:Label ID="lblMesDe" runat="server" Text="Mês:" SkinID="Label"></asp:Label>
            <asp:DropDownList ID="ddlMesDe" runat="server" SkinID="DropDownListRelatorio">
                <asp:ListItem Value="0">SELECIONE</asp:ListItem>
                <asp:ListItem Value="01">Janeiro</asp:ListItem>
                <asp:ListItem Value="02">Fevereiro</asp:ListItem>
                <asp:ListItem Value="03">Março</asp:ListItem>
                <asp:ListItem Value="04">Abril</asp:ListItem>
                <asp:ListItem Value="05">Maio</asp:ListItem>
                <asp:ListItem Value="06">Junho</asp:ListItem>
                <asp:ListItem Value="07">Julho</asp:ListItem>
                <asp:ListItem Value="08">Agosto</asp:ListItem>
                <asp:ListItem Value="09">Setembro</asp:ListItem>
                <asp:ListItem Value="10">Outubro</asp:ListItem>
                <asp:ListItem Value="11">Novembro</asp:ListItem>
                <asp:ListItem Value="12">Dezembro</asp:ListItem>
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblAnoDe" runat="server" Text="Ano:" SkinID="Label"></asp:Label>
            <asp:DropDownList ID="ddlAnoDe" runat="server" SkinID="DropDownListRelatorio">
                <asp:ListItem Value="0">SELECIONE</asp:ListItem>
                <asp:ListItem Value="2011">2011</asp:ListItem>
                <asp:ListItem Value="2012">2012</asp:ListItem>
                <asp:ListItem Value="2013">2013</asp:ListItem>
                <asp:ListItem Value="2014">2014</asp:ListItem>
                <asp:ListItem Value="2015">2015</asp:ListItem>
                <asp:ListItem Value="2016">2016</asp:ListItem>
                <asp:ListItem Value="2017">2017</asp:ListItem>
                <asp:ListItem Value="2018">2018</asp:ListItem>
                <asp:ListItem Value="2019">2019</asp:ListItem>
                <asp:ListItem Value="2020">2020</asp:ListItem>
                <asp:ListItem Value="2021">2021</asp:ListItem>
                <asp:ListItem Value="2022">2022</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="background-color: #003366; font-weight: bold; color: #FFD700; text-align: center;">
            A
        </td>
    </tr>
    <tr style="text-align: center;">
        <td>
            <asp:Label ID="lblMesPara" runat="server" Text="Mês:" SkinID="Label"></asp:Label>
            <asp:DropDownList ID="ddlMesPara" runat="server" SkinID="DropDownListRelatorio">
                <asp:ListItem Value="0">SELECIONE</asp:ListItem>
                <asp:ListItem Value="01">Janeiro</asp:ListItem>
                <asp:ListItem Value="02">Fevereiro</asp:ListItem>
                <asp:ListItem Value="03">Março</asp:ListItem>
                <asp:ListItem Value="04">Abril</asp:ListItem>
                <asp:ListItem Value="05">Maio</asp:ListItem>
                <asp:ListItem Value="06">Junho</asp:ListItem>
                <asp:ListItem Value="07">Julho</asp:ListItem>
                <asp:ListItem Value="08">Agosto</asp:ListItem>
                <asp:ListItem Value="09">Setembro</asp:ListItem>
                <asp:ListItem Value="10">Outubro</asp:ListItem>
                <asp:ListItem Value="11">Novembro</asp:ListItem>
                <asp:ListItem Value="12">Dezembro</asp:ListItem>
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblAnoPara" runat="server" Text="Ano:" SkinID="Label"></asp:Label>
            <asp:DropDownList ID="ddlAnoPara" runat="server" SkinID="DropDownListRelatorio">
                <asp:ListItem Value="0">SELECIONE</asp:ListItem>
                <asp:ListItem Value="2011">2011</asp:ListItem>
                <asp:ListItem Value="2012">2012</asp:ListItem>
                <asp:ListItem Value="2013">2013</asp:ListItem>
                <asp:ListItem Value="2014">2014</asp:ListItem>
                <asp:ListItem Value="2015">2015</asp:ListItem>
                <asp:ListItem Value="2016">2016</asp:ListItem>
                <asp:ListItem Value="2017">2017</asp:ListItem>
                <asp:ListItem Value="2018">2018</asp:ListItem>
                <asp:ListItem Value="2019">2019</asp:ListItem>
                <asp:ListItem Value="2020">2020</asp:ListItem>
                <asp:ListItem Value="2021">2021</asp:ListItem>
                <asp:ListItem Value="2022">2022</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
