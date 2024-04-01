<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PedidoVenda.aspx.cs" Inherits="Site.PedidoVenda" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>IMPRESSÃO PEDIDO VENDA</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <rsweb:ReportViewer ID="rpvPedidoVenda" runat="server" Width="100%" Font-Names="Verdana"
            Font-Size="8pt" Height="650px" ShowDocumentMapButton="False" ShowExportControls="True"
            ShowFindControls="False" ShowPageNavigationControls="False" ShowPromptAreaButton="False"
            ShowRefreshButton="False" ShowZoomControl="False">
            <LocalReport ReportPath="PedidoVenda.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="sdsListarVenda" Name="dsListarVenda_spListarVenda1" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:SqlDataSource ID="sdsListarVenda" runat="server" ConnectionString="<%$ ConnectionStrings:Mix %>"
            SelectCommand="spListarVenda1" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="PedidoID" SessionField="PedidoID" Type="Int32" />
                <asp:SessionParameter Name="SistemaID" SessionField="SistemaID" Type="Int32" />
                <asp:SessionParameter Name="Tipo" SessionField="Tipo" Type="Byte" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
