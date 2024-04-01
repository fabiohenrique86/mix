<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PedidoDetalhe.aspx.cs"
    Inherits="Site.PedidoDetalhe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DETALHE DO PEDIDO / RESERVA</title>
    <link href="css/estilo.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #geral
        {
            width: 425px;
        }
    </style>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.price_format.js" type="text/javascript"></script>
    <script type="text/javascript">
        $().ready(function () {
            $('.decimal').priceFormat({
                prefix: '',
                centsSeparator: ',',
                thousandsSeparator: '.'
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="geral">
        <div id="topo">
            <img src="../img/logo_mix_small.png" />
        </div>
        <div id="topo_cabeca">
            DETALHE DO PEDIDO / RESERVA :
            <asp:Label ID="lblPedidoID" runat="server"></asp:Label>
        </div>
        <div>
            <asp:GridView ID="gdvPedidoDetalhe" runat="server" SkinID="GridView" OnRowDataBound="gdvPedidoDetalhe_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="TipoPagamento" HeaderText="Tipo Pagamento" ItemStyle-Width="45%" />
                    <asp:BoundField DataField="Parcela" HeaderText="Parcela" ItemStyle-Width="15%" />
                    <asp:BoundField DataField="Valor" HeaderText="Valor Parcela" ItemStyle-Width="20%"
                        DataFormatString="{0:c}" />
                    <asp:BoundField DataField="TotalParcial" HeaderText="Valor Total" ItemStyle-Width="20%"
                        DataFormatString="{0:c}" />
                </Columns>
            </asp:GridView>
        </div>
        <div style="text-align: right; color: #FFFFFF;">
            <asp:Label ID="lblTotalPago" runat="server" Text="Total Pago : " CssClass="negrito" />
            <asp:TextBox ID="txtTotalPago" runat="server" Enabled="false" Width="80px" SkinID="TextBox" />
        </div>
        <div style="text-align: center; color: #FFFFFF; border: 1px solid #FFFFFF;">
            <asp:Label ID="lblObservacao_" runat="server" Text="Observação" CssClass="negrito" />
            <br />
            <div style="text-align: justify; padding: 0px 5px 0px 5px;">
                <asp:Label ID="lblObservacao" runat="server"></asp:Label>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
