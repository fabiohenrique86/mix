<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiaMesAno.ascx.cs" Inherits="Site.DiaMesAno" %>
<table id="tabela" width="100%">
    <tr style="text-align: center;">
        <td>
            <asp:Label ID="lblMsgTopo" runat="server" Text="Informe as datas abaixo para visualizar o relatório"
                SkinID="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="background-color: #003366; font-weight: bold; color: #FFD700; text-align: center;">DE
        </td>
    </tr>
    <tr style="text-align: center;">
        <td>
            <asp:Label ID="lblDiaDe" runat="server" Text="Dia:" SkinID="Label"></asp:Label>
            &nbsp;
            <asp:DropDownList ID="ddlDiaDe" runat="server" SkinID="DropDownListRelatorio">
                <asp:ListItem Value="0">SELECIONE</asp:ListItem>
                <asp:ListItem Value="01">01</asp:ListItem>
                <asp:ListItem Value="02">02</asp:ListItem>
                <asp:ListItem Value="03">03</asp:ListItem>
                <asp:ListItem Value="04">04</asp:ListItem>
                <asp:ListItem Value="05">05</asp:ListItem>
                <asp:ListItem Value="06">06</asp:ListItem>
                <asp:ListItem Value="07">07</asp:ListItem>
                <asp:ListItem Value="08">08</asp:ListItem>
                <asp:ListItem Value="09">09</asp:ListItem>
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="11">11</asp:ListItem>
                <asp:ListItem Value="12">12</asp:ListItem>
                <asp:ListItem Value="13">13</asp:ListItem>
                <asp:ListItem Value="14">14</asp:ListItem>
                <asp:ListItem Value="15">15</asp:ListItem>
                <asp:ListItem Value="16">16</asp:ListItem>
                <asp:ListItem Value="17">17</asp:ListItem>
                <asp:ListItem Value="18">18</asp:ListItem>
                <asp:ListItem Value="19">19</asp:ListItem>
                <asp:ListItem Value="20">20</asp:ListItem>
                <asp:ListItem Value="21">21</asp:ListItem>
                <asp:ListItem Value="22">22</asp:ListItem>
                <asp:ListItem Value="23">23</asp:ListItem>
                <asp:ListItem Value="24">24</asp:ListItem>
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="26">26</asp:ListItem>
                <asp:ListItem Value="27">27</asp:ListItem>
                <asp:ListItem Value="28">28</asp:ListItem>
                <asp:ListItem Value="29">29</asp:ListItem>
                <asp:ListItem Value="30">30</asp:ListItem>
                <asp:ListItem Value="31">31</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
            <asp:Label ID="lblMesDe" runat="server" Text="Mês:" SkinID="Label"></asp:Label>
            &nbsp;
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
            &nbsp;
            <asp:Label ID="lblAnoDe" runat="server" Text="Ano:" SkinID="Label"></asp:Label>
            &nbsp;
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
                <asp:ListItem Value="2023">2023</asp:ListItem>
                <asp:ListItem Value="2024">2024</asp:ListItem>
                <asp:ListItem Value="2025">2025</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="background-color: #003366; font-weight: bold; color: #FFD700; text-align: center;">A
        </td>
    </tr>
    <tr style="text-align: center;">
        <td>
            <asp:Label ID="lblDiaPara" runat="server" Text="Dia:" SkinID="Label"></asp:Label>
            &nbsp;
            <asp:DropDownList ID="ddlDiaPara" runat="server" SkinID="DropDownListRelatorio">
                <asp:ListItem Value="0">SELECIONE</asp:ListItem>
                <asp:ListItem Value="01">01</asp:ListItem>
                <asp:ListItem Value="02">02</asp:ListItem>
                <asp:ListItem Value="03">03</asp:ListItem>
                <asp:ListItem Value="04">04</asp:ListItem>
                <asp:ListItem Value="05">05</asp:ListItem>
                <asp:ListItem Value="06">06</asp:ListItem>
                <asp:ListItem Value="07">07</asp:ListItem>
                <asp:ListItem Value="08">08</asp:ListItem>
                <asp:ListItem Value="09">09</asp:ListItem>
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="11">11</asp:ListItem>
                <asp:ListItem Value="12">12</asp:ListItem>
                <asp:ListItem Value="13">13</asp:ListItem>
                <asp:ListItem Value="14">14</asp:ListItem>
                <asp:ListItem Value="15">15</asp:ListItem>
                <asp:ListItem Value="16">16</asp:ListItem>
                <asp:ListItem Value="17">17</asp:ListItem>
                <asp:ListItem Value="18">18</asp:ListItem>
                <asp:ListItem Value="19">19</asp:ListItem>
                <asp:ListItem Value="20">20</asp:ListItem>
                <asp:ListItem Value="21">21</asp:ListItem>
                <asp:ListItem Value="22">22</asp:ListItem>
                <asp:ListItem Value="23">23</asp:ListItem>
                <asp:ListItem Value="24">24</asp:ListItem>
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="26">26</asp:ListItem>
                <asp:ListItem Value="27">27</asp:ListItem>
                <asp:ListItem Value="28">28</asp:ListItem>
                <asp:ListItem Value="29">29</asp:ListItem>
                <asp:ListItem Value="30">30</asp:ListItem>
                <asp:ListItem Value="31">31</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
            <asp:Label ID="lblMesPara" runat="server" Text="Mês:" SkinID="Label"></asp:Label>
            &nbsp;
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
            &nbsp;
            <asp:Label ID="lblAnoPara" runat="server" Text="Ano:" SkinID="Label"></asp:Label>
            &nbsp;
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
                <asp:ListItem Value="2023">2023</asp:ListItem>
                <asp:ListItem Value="2024">2024</asp:ListItem>
                <asp:ListItem Value="2025">2025</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
