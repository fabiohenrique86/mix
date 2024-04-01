<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CancelamentoDetalhe.aspx.cs"
    Inherits="Site.CancelamentoDetalhe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DETALHE DO CANCELAMENTO DE PEDIDO / RESERVA</title>
    <link href="css/estilo.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            text-align: center;
            width: 100%;
        }
        .style2
        {
            border: solid 2px #FFD700;
        }
        #geral
        {
            width: 425px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="geral">
        <div id="topo">
            <img src="../img/logo_mix_small.png" />
        </div>
        <div id="topo_cabeca">
            CANCELAMENTO DE PEDIDO - DETALHE
        </div>
        <div id="conteudo_detalhe_cancelamento">
            <table class="style1">
                <tr>
                    <td class="style2" style="font-weight: bold;">
                        <asp:Label ID="lblPedido" runat="server" Text="PedidoID: "></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:Label ID="lblPedidoID" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style2" style="font-weight: bold;">
                        <asp:Label ID="lblMotivo_" runat="server" Text="Motivo: "></asp:Label>
                    </td>
                    <td class="style2" style="text-align: justify;">
                        <asp:Label ID="lblMotivo" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
