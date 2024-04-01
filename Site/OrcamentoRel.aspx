<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrcamentoRel.aspx.cs" Inherits="Site.OrcamentoRel" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>IMPRESSÃO ORÇAMENTO</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <rsweb:ReportViewer ID="rpvOrcamentoRel" runat="server" Width="100%" Font-Names="Verdana"
            Font-Size="8pt" Height="600px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="False" ShowPromptAreaButton="False" ShowRefreshButton="False"
            ShowZoomControl="False">
            <LocalReport ReportPath="Orcamento.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="sdsOrcamento" Name="dsOrcamento_spListarOrcamentoById" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
    <asp:SqlDataSource ID="sdsOrcamento" runat="server" ConnectionString="<%$ ConnectionStrings:Mix %>"
        SelectCommand="spListarOrcamentoById" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="OrcamentoID" SessionField="OrcamentoID" Type="Int32" />
            <asp:SessionParameter Name="SistemaID" SessionField="SistemaID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>
