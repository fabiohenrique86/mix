<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComandaEntrega.aspx.cs"
    Inherits="Site.ComandaEntrega" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>COMANDA DE ENTREGA</title>
    <style type="text/css">
        .conteudo
        {
            padding: 0;
            margin: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="conteudo">
        <div style="text-align: left;">
            <asp:ImageButton ID="imbImprimir" runat="server" ImageUrl="~/img/imprimir.gif" OnClick="imbImprimir_Click" />
        </div>
        <div>
            <rsweb:ReportViewer ID="rpvComandaEntrega" runat="server" Width="100%" Font-Names="Verdana"
                Font-Size="8pt" Height="600px" ShowDocumentMapButton="False" ShowFindControls="False"
                ShowPageNavigationControls="False" ShowRefreshButton="False" ShowZoomControl="False"
                ShowPrintButton="false" ShowToolBar="false">
                <LocalReport ReportPath="ComandaEntrega.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="sdsComandaEntrega" Name="dsComandaEntrega_spListarComandaEntrega" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:SqlDataSource ID="sdsComandaEntrega" runat="server" ConnectionString="<%$ ConnectionStrings:Mix %>"
                SelectCommand="spListarComandaEntrega" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="PedidoID" SessionField="PedidoID" Type="Int32" />
                    <asp:SessionParameter Name="SistemaID" SessionField="SistemaID" Type="Int32" />
                    <asp:SessionParameter Name="PedidoOuReserva" SessionField="PedidoOuReserva" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </div>
    </form>
</body>
</html>
