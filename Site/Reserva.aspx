<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Reserva.aspx.cs"
    Inherits="Site.Reserva" Title="LANÇAMENTO - RESERVA" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1 {
            width: 110px;
        }

        .label_repeater {
            font-weight: normal;
            font-style: italic;
        }
    </style>
    <script type="text/javascript">

        function ControleDeEntrega(reservaId) {

            var tabelaComanda = "";

            $.ajax({
                url: '<%= Page.ResolveUrl("~/Reserva.aspx/Teste") %>',
                data: "{ reservaId: '" + reservaId + "', sistemaId: " + '<%= new BLL.Modelo.Usuario(Session["Usuario"]).SistemaID %>' + "}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    tabelaComanda += "<table id='tbPrimeiraVia' style='width: 900px; font-family: segoe UI, sans-serif;' cellpadding='0'; cellspacing='0'>";
                    tabelaComanda += "<tr>";
                    tabelaComanda += "<td colspan='8'><img src='../img/logo_mix.png' alt='' /></td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='height: 30px;'>";
                    tabelaComanda += "<td colspan='8' style='text-align: center; font-weight: bold; border: solid 1px #000; font-size: 15px; text-decoration: underline'>CONTROLE DE ENTREGA</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>DATA DA VENDA</td>";
                    tabelaComanda += "<td colspan='2' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px; text-align: center;'>" + data.d.DataReservaFormatada + "</td>";
                    tabelaComanda += "<td colspan='2' style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>DATA DA ENTREGA</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px; text-align: center;'>" + data.d.DataEntregaFormatada + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>PC</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px; text-align: center'>" + data.d.ReservaID + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>CONSULTOR</td>";
                    tabelaComanda += "<td colspan='7' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.FuncionarioNome + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>TEL RES</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.TelefoneResidencial + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>TEL RES 2</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.TelefoneResidencial2 + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>TEL CEL</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.TelefoneCelular + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>TEL CEL 2</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.TelefoneCelular2 + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>LOJA</td>";
                    tabelaComanda += "<td colspan='7' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.LojaDestinoNomeFantasia + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>NOME DO CLIENTE</td>";
                    tabelaComanda += "<td colspan='5' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.Nome + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>CPF</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.Cpf + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>ENDERE&Ccedil;O</td>";
                    tabelaComanda += "<td colspan='7' style='width: 200px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.Endereco + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 50px; border: solid 1px #000; text-align: center; font-weight: bold;'>N&Uacute;MERO</td>";
                    tabelaComanda += "<td colspan='3' style='width: 50px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + "" + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>CEP</td>";
                    tabelaComanda += "<td colspan='3' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + "" + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>COMPLEMENTO</td>";
                    tabelaComanda += "<td colspan='7' style='width: 200px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + "" + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 50px; border: solid 1px #000; text-align: center; font-weight: bold;'>BAIRRO</td>";
                    tabelaComanda += "<td colspan='3' style='width: 50px; border: solid 1px #000; text-align: left; padding-left: 5px; text-align: center;'>" + data.d.Cliente.Bairro + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>CIDADE</td>";
                    tabelaComanda += "<td colspan='3' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px; text-align: center;'>" + data.d.Cliente.Cidade + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>REFER&Ecirc;NCIA</td>";
                    tabelaComanda += "<td colspan='7' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.Referencia + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>COD. PRODUTO</td>";
                    tabelaComanda += "<td style='width: 50px; border: solid 1px #000; text-align: center; font-weight: bold;'>QTD</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>VALOR</td>";
                    tabelaComanda += "<td colspan='5' style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>PRODUTO</td>";
                    tabelaComanda += "</tr>";

                    $.each(data.d.ListaProduto, function (index, item) {
                        tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                        tabelaComanda += "<td style='width: 200px; border: solid 1px #000; text-align: center; padding-left: 5px;'>" + item.ProdutoID + "</td>";
                        tabelaComanda += "<td style='width: 50px; border: solid 1px #000; text-align: center; padding-left: 5px;'>" + item.Quantidade + "</td>";
                        tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; padding-left: 5px;'>" + item.Preco + "</td>";
                        tabelaComanda += "<td colspan='5' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + item.Descricao + "</td>";
                        tabelaComanda += "</tr>";
                    });

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td colspan='8' style='width: 100px; border: solid 1px #000; text-align: left; font-weight: bold;'>PAGO POR: CLIENTE NO ATO (&nbsp;&nbsp;&nbsp;) CONSULTOR (&nbsp;&nbsp;&nbsp;) LOJA (&nbsp;&nbsp;&nbsp;) VALOR: </td>";
                    tabelaComanda += "</tr>";
                    tabelaComanda += "</table>";

                    tabelaComanda += "<br /><br />";

                    tabelaComanda += "<table id='tbSegundaVia' style='width: 900px; font-family: segoe UI, sans-serif;' cellpadding='0'; cellspacing='0'>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>DATA DA VENDA</td>";
                    tabelaComanda += "<td colspan='2' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px; text-align: center;'>" + data.d.DataReservaFormatada + "</td>";
                    tabelaComanda += "<td colspan='2' style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>DATA DA ENTREGA</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px; text-align: center;'>" + data.d.DataEntregaFormatada + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>PC</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px; text-align: center'>" + data.d.ReservaID + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>CONSULTOR</td>";
                    tabelaComanda += "<td colspan='7' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.FuncionarioNome + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>TEL RES</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.TelefoneResidencial + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>TEL RES 2</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.TelefoneResidencial2 + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>TEL CEL</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.TelefoneCelular + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>TEL CEL 2</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.TelefoneCelular2 + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>LOJA</td>";
                    tabelaComanda += "<td colspan='7' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.LojaDestinoNomeFantasia + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>NOME DO CLIENTE</td>";
                    tabelaComanda += "<td colspan='5' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.Nome + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>CPF</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.Cpf + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>ENDERE&Ccedil;O</td>";
                    tabelaComanda += "<td colspan='7' style='width: 200px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.Endereco + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 50px; border: solid 1px #000; text-align: center; font-weight: bold;'>N&Uacute;MERO</td>";
                    tabelaComanda += "<td colspan='3' style='width: 50px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + "" + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>CEP</td>";
                    tabelaComanda += "<td colspan='3' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + "" + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>COMPLEMENTO</td>";
                    tabelaComanda += "<td colspan='7' style='width: 200px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + "" + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 50px; border: solid 1px #000; text-align: center; font-weight: bold;'>BAIRRO</td>";
                    tabelaComanda += "<td colspan='3' style='width: 50px; border: solid 1px #000; text-align: left; padding-left: 5px; text-align: center;'>" + data.d.Cliente.Bairro + "</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>CIDADE</td>";
                    tabelaComanda += "<td colspan='3' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px; text-align: center;'>" + data.d.Cliente.Cidade + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>REFER&Ecirc;NCIA</td>";
                    tabelaComanda += "<td colspan='7' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + data.d.Cliente.Referencia + "</td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>COD. PRODUTO</td>";
                    tabelaComanda += "<td style='width: 50px; border: solid 1px #000; text-align: center; font-weight: bold;'>QTD</td>";
                    tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>VALOR</td>";
                    tabelaComanda += "<td colspan='5' style='width: 100px; border: solid 1px #000; text-align: center; font-weight: bold;'>PRODUTO</td>";
                    tabelaComanda += "</tr>";

                    $.each(data.d.ListaProduto, function (index, item) {
                        tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                        tabelaComanda += "<td style='width: 200px; border: solid 1px #000; text-align: center; padding-left: 5px;'>" + item.ProdutoID + "</td>";
                        tabelaComanda += "<td style='width: 50px; border: solid 1px #000; text-align: center; padding-left: 5px;'>" + item.Quantidade + "</td>";
                        tabelaComanda += "<td style='width: 100px; border: solid 1px #000; text-align: center; padding-left: 5px;'>" + item.Preco + "</td>";
                        tabelaComanda += "<td colspan='5' style='width: 100px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + item.Descricao + "</td>";
                        tabelaComanda += "</tr>";
                    });

                    tabelaComanda += "<tr style='font-size: 14px; height: 30px;'>";
                    tabelaComanda += "<td colspan='8' style='width: 100px; border: solid 1px #000; text-align: left; font-weight: bold;'>RECEBER PAGAMENTO: CART&Atilde;O (&nbsp;&nbsp;&nbsp;) CHEQUE (&nbsp;&nbsp;&nbsp;) DINHEIRO (&nbsp;&nbsp;&nbsp;) VALOR: </td>";
                    tabelaComanda += "</tr>";

                    tabelaComanda += "</table>";

                    var janela = window.open("", "", "status=0,toolbar=0,resizable=0,menubar=0,titlebar=0,width=900,height=500");
                    var conteudo = "<html><body onload='window.print(); window.close();'>" + tabelaComanda + "</body></html>";

                    janela.document.open();
                    janela.document.write(conteudo);
                    janela.moveTo(((screen.width - 900) / 2), ((screen.height - 500) / 2));
                    janela.document.close();
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Ocorre um erro ao gerar comanda. Tente novamente")
                }
            });
        }

        function ListarProduto() {

            var gdvProduto = document.getElementById("tblProduto");
            var listaProduto = "<div style='overflow-x: hidden; overflow-y: scroll; height: 175px;'>";
            var produtoSelecionado = false;
            var descricaoProduto = "";
            var browserMozilla = $.browser.mozilla;

            // começa em "1" porque 0 é header
            for (var i = 1; i < gdvProduto.rows.length; i++) {

                produtoSelecionado = true;

                if (browserMozilla) {
                    descricaoProduto = gdvProduto.rows[i].cells[0].innerHTML;
                }
                else {
                    descricaoProduto = gdvProduto.rows[i].cells[0].innerText;
                }

                listaProduto += "<span style='text-align: left; font-weight: bold;'>Produto: </span>" + descricaoProduto;

                //                if (gdvProduto.rows[i].cells[2].children[0] != null) {
                //                    listaProduto += " - " + gdvProduto.rows[i].cells[2].children[0].value;
                //                }

                listaProduto += "\r\n"

                if (gdvProduto.rows[i].cells[2].children[0].value > "0") {
                    listaProduto += "<span style='text-align: left; font-weight: bold;'>Quantidade: </span>" + gdvProduto.rows[i].cells[2].children[0].value + "\r\n";
                }
                else {
                    listaProduto += "<span style='text-align: left; font-weight: bold; color: Red;'>Quantidade: " + gdvProduto.rows[i].cells[2].children[0].value + "</span>\r\n";
                }

                if (gdvProduto.rows[i].cells[3].children[0].value > "0,00") {
                    listaProduto += "<span style='text-align: left; font-weight: bold;'>Preço: </span>" + gdvProduto.rows[i].cells[3].children[0].value + "\r\n";
                }
                else {
                    listaProduto += "<span style='text-align: left; font-weight: bold; color: Red;'>Preço: " + gdvProduto.rows[i].cells[3].children[0].value + "</span>\r\n";
                }
                listaProduto += "\r\n";
            }

            listaProduto += "</div>";

            if (produtoSelecionado == true) {
                var ddlLojaSaida = document.getElementById('ctl00_ContentPlaceHolder1_ddlLoja');
                var msg = "<span style='text-align: left; font-weight: bold;'>PedidoID: </span>" + document.getElementById("ctl00_ContentPlaceHolder1_txtPedidoID").value + "\r\n<span style='text-align: left; font-weight: bold;'>Loja Saída: </span>" + ddlLojaSaida.options[ddlLojaSaida.selectedIndex].text + "\r\n\r\nEsse pedido contém os seguintes produtos:\r\n\r\n" + listaProduto + "<span style='text-align: left; font-weight: bold;'>Frete: </span>" + document.getElementById("ctl00_ContentPlaceHolder1_txtValorFrete").value + "\r\n\r\nTem certeza que deseja inserir pedido?";
                var titulo = "Confirmação";
                jConfirm(msg, titulo, function (r) {
                    if (r) {
                        __doPostBack((document.getElementById("ctl00_ContentPlaceHolder1_imbInserirReserva")).name, 'OnClick');
                    }
                });
            }

            return false;
        }

        function FormatarDecimal(num) {
            x = 0;
            if (num < 0) {
                num = Math.abs(num);
                x = 1;
            }
            if (isNaN(num))
                num = "0";
            cents = Math.floor((num * 100 + 0.5) % 100);
            num = Math.floor((num * 100 + 0.5) / 100).toString();
            if (cents < 10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
                num = num.substring(0, num.length - (4 * i + 3)) + '.' + num.substring(num.length - (4 * i + 3));
            ret = num + ',' + cents;
            if (x == 1)
                ret = ' – ' + ret;
            return ret;
        }

        function CalcularPagamentoTotal() {

            var gdvTipoPagamento = document.getElementById("ctl00_ContentPlaceHolder1_gdvTipoPagamento");
            var Total;
            var TotalFloat = 0;
            var quantidade;
            var preco;

            for (var i = 1; i < gdvTipoPagamento.rows.length; i++) {
                quantidade = gdvTipoPagamento.rows[i].cells[2].children[0].value;
                preco = gdvTipoPagamento.rows[i].cells[3].children[0].value;
                if ((gdvTipoPagamento.rows[i].cells[0].children[0].checked) && (quantidade > 0) && (preco > "0,00")) {
                    Total = parseFloat(preco.replace(".", "").replace(",", "."));
                    if (!isNaN(Total)) {
                        TotalFloat = parseFloat(TotalFloat) + (parseFloat(Total) * parseInt(quantidade));
                    }
                }
            }

            if (!isNaN(TotalFloat)) {
                document.getElementById("ctl00_ContentPlaceHolder1_txtTotalPago").value = FormatarDecimal(TotalFloat);
            }
        }

        function ConfirmaImpressao() {
            var msg = "A COMANDA DE ENTREGA FOI IMPRESSA?";
            var titulo = "Confirmação";
            jConfirm(msg, titulo, function (r) {
                if (r) {
                    __doPostBack((document.getElementById("ctl00_ContentPlaceHolder1_imbFechar")).name, 'OnClick');
                }
            });
            return false;
        }

        function formatSobMedida(src) {
            var mask = "#,##X#,##X#,##";
            var tecla = event.keyCode;
            if ((tecla > 47 && tecla < 58)) {
                var i = src.value.length;
                var saida = mask.substring(0, 1);
                var texto = mask.substring(i);
                if (texto.substring(0, 1) != saida) {
                    src.value += texto.substring(0, 1);
                    return true;
                }
            }
            else {
                src.value = "";
                return false;
            }
        }

        function createRow(produtoId, descricao, index) {
            var row = "<tr>";
            row = row + "<td>" + descricao + "</td>";
            row = row + "<td><input type='text' maxlength='14' style='width: 100px' onkeypress='return formatSobMedida(this)' name='txtSobMedida_" + index + "' /></td>";
            row = row + "<td><input type='text' maxlength='3' style='width: 30px' onblur='calcularTotal()' name='txtQuantidade_" + index + "' /></td>";
            row = row + "<td><input type='text' maxlength='10' style='width: 70px' onblur='calcularTotal()' name='txtPreco_" + index + "' class='decimal' /></td>";
            row = row + "<td><img src='../img/excluir_produto.png' title='Excluir Produto' style='width: 25px; heigth: 25px' onclick='deleteRow(this)' /><input type='hidden' name='txtProdutoID_" + index + "' value='" + produtoId + "' /></td>"
            row = row + "</tr>";
            return row;
        }

        function deleteRow(r) {
            var i = r.parentNode.parentNode.rowIndex;
            document.getElementById("tblProduto").deleteRow(i);
            calcularTotal();
        }

        function clearGrid() {
            $('#tblProduto tbody > tr').remove();
        }

        function calcularTotal() {
            var tblProduto = document.getElementById("tblProduto");
            var Total;
            var TotalFloat = 0;
            var quantidade;
            var preco;
            for (var i = 1; i < tblProduto.rows.length; i++) {
                quantidade = tblProduto.rows[i].cells[2].children[0].value;
                preco = tblProduto.rows[i].cells[3].children[0].value;
                if ((quantidade > 0) && (preco > "0,00")) {
                    Total = parseFloat(preco.replace(".", "").replace(",", "."));
                    if (!isNaN(Total)) {
                        TotalFloat = parseFloat(TotalFloat) + (parseFloat(Total) * parseInt(quantidade));
                    }
                }
            }
            if (!isNaN(TotalFloat)) {
                document.getElementById("ctl00_ContentPlaceHolder1_txtTotalPedido").value = FormatarDecimal(TotalFloat);
            }
        }

        function getValoresProduto() {
            var hdf = "";
            var tblProduto = document.getElementById("tblProduto");
            var quantidade;
            var preco;
            if (tblProduto != null && tblProduto.rows.length > 0) {
                for (var i = 1; i < tblProduto.rows.length; i++) {
                    quantidade = tblProduto.rows[i].cells[2].children[0].value;
                    preco = tblProduto.rows[i].cells[3].children[0].value;
                    hdf = hdf + i + ";"; // linha - 0
                    hdf = hdf + tblProduto.rows[i].cells[4].children[1].value + ";"; // produtoId - 1
                    hdf = hdf + tblProduto.rows[i].cells[0].innerText + ";"; // descrição - 2
                    hdf = hdf + tblProduto.rows[i].cells[1].children[0].value + ";"; // sob medida - 3
                    if (quantidade > 0) {
                        hdf = hdf + quantidade + ";"; // quantidade - 4
                    }
                    if (preco > "0,00") {
                        hdf = hdf + preco + ";"; // preço - 5
                    }
                    hdf = hdf + "~~";
                }
                document.getElementById("ctl00_ContentPlaceHolder1_hdfProdutoValores").value = hdf.substr(0, hdf.length - 2);
            }
        }

        function setValoresProduto() {
            var valoresProduto = document.getElementById("ctl00_ContentPlaceHolder1_hdfProdutoValores").value;
            if (valoresProduto != "") {
                var tblProduto = document.getElementById("tblProduto");
                if (tblProduto != null && tblProduto.rows.length > 0) {
                    var linha = valoresProduto.split('~~');
                    var j = 1;
                    for (var i = 0; i < linha.length; i++) {
                        var cell = linha[i].split(';');
                        var row = "<tr>";
                        row = row + "<td>" + cell[2] + "</td>";
                        row = row + "<td><input type='text' maxlength='14' style='width: 100px' onkeypress='return formatSobMedida(this)' name='txtSobMedida_" + j + "' value='" + cell[3] + "' /></td>";
                        row = row + "<td><input type='text' maxlength='3' style='width: 30px' onblur='calcularTotal()' value='" + cell[4] + "' name='txtQuantidade_" + j + "' /></td>";
                        row = row + "<td><input type='text' maxlength='10' style='width: 70px' onblur='calcularTotal()' value='" + cell[5] + "' name='txtPreco_" + j + "' class='decimal' /></td>";
                        row = row + "<td><img style='width: 25px; heigth: 25px' src='../img/excluir_produto.png' title='Excluir Produto' onclick='deleteRow(this)' /><input type='hidden' name='txtProdutoID_" + j + "' value='" + cell[1] + "' /></td>";
                        row = row + "</tr>";
                        $("#tblProdutoBody").append(row);
                        j++;
                    }
                }
                $('.decimal').priceFormat({ prefix: '', centsSeparator: ',', thousandsSeparator: '.' });
                document.getElementById("ctl00_ContentPlaceHolder1_hdfProdutoValores").value = "";
            }
        }

        function getAutocomplete() {
            $('#<%=txtProduto.ClientID%>').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%= Page.ResolveUrl("~/Produto.aspx/GetProduct")%>',
                        data: "{ term: '" + request.term + "', lojaId: " + $("#ctl00_ContentPlaceHolder1_ddlLoja").val() + ", sistemaId: " + '<%= new BLL.Modelo.Usuario(Session["Usuario"]).SistemaID %>' + "}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            if (data.d.length == 0) {
                                var noData = ["Produto não cadastrado na loja"];
                                response($.map(noData, function (item) {
                                    return { label: item, value: item }
                                }));
                            }
                            else {
                                response($.map(data.d, function (item) {
                                    return { label: item.Descricao, value: item.ProdutoID }
                                }));
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(textStatus);
                        }
                    });
                },
                select: function (event, ui) {
                    var tblProduto = document.getElementById("tblProduto");
                    var produtoAdicionado = false;
                    var produtoNaoCadastradoNaLoja = false;
                    if (ui.item.label == "Produto não cadastrado na loja") {
                        produtoNaoCadastradoNaLoja = true;
                    }
                    else {
                        for (var i = 1; i < tblProduto.rows.length; i++) {
                            if (ui.item.label == tblProduto.rows[i].cells[0].innerText) {
                                produtoAdicionado = true;
                                break;
                            }
                        }
                    }
                    if (produtoNaoCadastradoNaLoja) {

                    }
                    else {
                        if (produtoAdicionado) {
                            jAlert("Produto " + ui.item.label + " já foi adicionado ao pedido.", "Alerta", function () { $("#ctl00_ContentPlaceHolder1_txtProduto").focus(); });
                        }
                        else {
                            $("#tblProdutoBody").append(createRow(ui.item.value, ui.item.label, tblProduto.rows.length));
                        }
                    }
                    $('.decimal').priceFormat({ prefix: '', centsSeparator: ',', thousandsSeparator: '.' });
                    ui.item.label = "";
                    ui.item.value = "";
                }
            });
        }
    </script>
    <script language="javascript" type="text/javascript">
        $(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function BeginRequestHandler(sender, args) {
                getValoresProduto();
            }

            function EndRequestHandler(sender, args) {
                setValoresProduto();
                getAutocomplete();
            }

            getAutocomplete();

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfProdutoValores" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="imbConsultar" />
            <asp:AsyncPostBackTrigger ControlID="imbInserirReserva" />
        </Triggers>
        <ContentTemplate>
            <div id="conteudo">
                <input type="hidden" id="hdnScrollProduto" runat="server" value="0" />
                <input type="hidden" id="hdnScrollReserva" runat="server" value="0" />
                <div id="topo_cabeca">
                    LANÇAMENTO - RESERVA
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblPedidoID" runat="server" Text="PedidoID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtPedidoID" runat="server" Width="55px" MaxLength="7" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtPedidoID"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblLoja" runat="server" Text="Loja (Saída)" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlLoja" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID"
                                    SkinID="DropDownList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblFuncionario" runat="server" Text="Funcionário" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlFuncionario" runat="server" DataTextField="Nome" DataValueField="FuncionarioID"
                                    SkinID="DropDownList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblCliente" runat="server" Text="Cliente (CPF/CNPJ)" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtCpf" runat="server" Width="85px" SkinID="TextBox"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="meeCpf" runat="server" TargetControlID="txtCpf" ClearMaskOnLostFocus="False"
                                    Mask="999,999,999-99">
                                </cc1:MaskedEditExtender>
                                <asp:TextBox ID="txtCnpj" runat="server" MaxLength="14" Width="110px" SkinID="TextBox"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtCnpj_MaskedEditExtender" runat="server" TargetControlID="txtCnpj"
                                    ClearMaskOnLostFocus="False" Mask="99,999,999/9999-99">
                                </cc1:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="Label1" runat="server" Text="Produto" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtProduto" runat="server" Width="450px" SkinID="TextBox" onblur="this.value=''"
                                    placeholder="Pesquise pelo produto..."></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table id="tblProduto" rules="all" cellpadding="0" cellspacing="0" border="1" bordercolor="black"
                                    style="font-family: Segoe UI; font-size: 11px; width: 100%; border-collapse: collapse;">
                                    <thead style="color: Gold; background-color: #003366; font-weight: bold; text-align: center">
                                        <tr>
                                            <td style="width: 60%">Produto
                                            </td>
                                            <td style="width: 15%">Sob Medida
                                            </td>
                                            <td style="width: 10%">Quantidade
                                            </td>
                                            <td style="width: 10%">Preço
                                            </td>
                                            <td style="width: 5%">Ação
                                            </td>
                                        </tr>
                                    </thead>
                                    <tbody id="tblProdutoBody" style="text-align: center">
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: right;">
                                <asp:Label ID="lblTotal" runat="server" Width="100px" Text="Total Pedido : " CssClass="negrito"></asp:Label>
                                <asp:TextBox ID="txtTotalPedido" runat="server" Text="" Width="80px" MaxLength="10"
                                    CssClass="decimal" Enabled="false" Height="15px" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="gdvTipoPagamento" runat="server" SkinID="GridView" OnRowDataBound="gdvTipoPagamento_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Selecione" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckbTipoPagamento" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tipo Pagamento" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="70%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTipoPagamentoID" runat="server" Text='<%# Bind("TipoPagamentoID") %>'
                                                    Visible="false">
                                                </asp:Label>
                                                <asp:Label ID="lblTipoPagamento" runat="server" Text='<%# Bind("Descricao") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Parcela" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlParcela" runat="server" DataTextField="Parcela" DataValueField="ParcelaID"
                                                    Height="20px" SkinID="DropDownListRelatorio">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Pago" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtValorPago" runat="server" Text="" Width="70px" MaxLength="10"
                                                    CssClass="decimal" Height="14px" SkinID="TextBox"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: right;">
                                <asp:Label ID="lblTotalPago" runat="server" Width="100px" Text="Total Pago : " CssClass="negrito"></asp:Label>
                                <asp:TextBox ID="txtTotalPago" runat="server" Text="" Width="80px" MaxLength="10"
                                    CssClass="decimal" Enabled="false" Height="15px" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblDataReserva" runat="server" Text="Data Reserva" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataReserva" runat="server" Width="65px" Height="15px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataReserva_MaskedEditExtender" runat="server" TargetControlID="txtDataReserva"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblDataEntrega" runat="server" Text="Data Entrega" SkinID="Label"></asp:Label>&nbsp;&nbsp;&nbsp;
                                <asp:TextBox ID="txtDataEntrega" runat="server" Width="65px" Height="15px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataEntrega_MaskedEditExtender" runat="server" TargetControlID="txtDataEntrega"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                                <asp:CheckBox ID="ckbDataEntrega" runat="server" Text="Sem Data Entrega (A PROGRAMAR)"
                                    Checked="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblValorFrete" runat="server" Text="Valor Frete" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtValorFrete" runat="server" Text="0,00" Width="80px" MaxLength="10"
                                    CssClass="decimal" Height="15px" SkinID="TextBox"></asp:TextBox><span class="campo_opcional">&nbsp;*</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblCV" runat="server" Text="CV" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtCV" runat="server" Text="" Width="80px" MaxLength="10"
                                    Height="15px" SkinID="TextBox"></asp:TextBox><span class="campo_opcional">&nbsp;*</span>
                                <cc1:FilteredTextBoxExtender ID="fteCV" runat="server" TargetControlID="txtCV" FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblObservacao_" runat="server" Text="Observação" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtObservacao" runat="server" Width="750px" MaxLength="350" TextMode="MultiLine"
                                    Height="80px" SkinID="TextBox">
                                </asp:TextBox>
                                <span class="campo_opcional">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="campo_opcional">* campo opcional
                            </td>
                        </tr>
                        <tr style="background-color: #FFD700; font-weight: bold; color: #003366; text-align: center;">
                            <td colspan="2">FILTRO
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblStatusID" runat="server" Text="Status" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlStatus" runat="server" SkinID="DropDownListRelatorio" Width="100px">
                                    <asp:ListItem Text="SELECIONE" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Pendente" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Efetuada" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbInserirReserva" runat="server" ImageUrl="~/img/inserir_reserva.png"
                                    OnClick="imbInserirReserva_Click" OnClientClick="return ListarProduto();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <div id="divScrollReserva" style="overflow-x: hidden; overflow-y: scroll; height: 400px;">
                        <asp:Repeater ID="rptReserva" runat="server" OnItemDataBound="rptReserva_ItemDataBound">
                            <ItemTemplate>
                                <div id="repeaterReserva" class="topo_grid" style="text-align: left;" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPedido" runat="server" Text="PedidoID:"></asp:Label>
                                            </td>
                                            <td class="label_pedido">
                                                <asp:LinkButton ID="lkbPedidoID" runat="server" Text='<%# Bind("ReservaID") %>' OnClick="lkbPedidoID_Click"
                                                    Visible="false">
                                                </asp:LinkButton>
                                                <asp:Label ID="lblPedidoID" runat="server" Text='<%# Bind("ReservaID") %>' Visible="false"></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNomeFantasia" runat="server" Text="Loja Origem:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblNomeFantasiaID" runat="server" Text='<%# Bind("LojaOrigem") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLojaOrigem" runat="server" Text="Loja Saída:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblLojaOrigemID" runat="server" Text='<%# Bind("NomeFantasia") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFuncionario" runat="server" Text="Funcionário:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblFuncionarioID" runat="server" Text='<%# Bind("Funcionario") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCliente" runat="server" Text="Cliente:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblClienteID" runat="server" Text='<%# Bind("Cliente") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDataReserva" runat="server" Text="Data Reserva:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblDataReservaID" runat="server" Text='<%# Bind("DataReserva","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDataEntrega" runat="server" Text="Data Entrega:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblDataEntregaID" runat="server" Text='<%# Bind("DataEntrega","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStatus" runat="server" Text="Entrega:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblStatusID" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStatusImprimido" runat="server" Text="Impresso:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblStatusImprimidoID" runat="server" Text='<%# Bind("StatusImprimido") %>'></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:LinkButton ID="lkbDetalhe" runat="server" Text="Detalhes" OnClick="lkbDetalhe_Click"
                                                    CommandArgument='<%# Bind("ReservaID") %>' />
                                                <asp:LinkButton ID="lkbComanda" runat="server" Text="Comanda"
                                                    CommandArgument='<%# Bind("ReservaID") %>' OnClientClick='<%# "ControleDeEntrega("+ Eval("ReservaID") + ");" %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblObservacao" runat="server" Text='<%# Bind("Observacao") %>' Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:GridView ID="gdvReservaProduto" runat="server" SkinID="GridView">
                                        <Columns>
                                            <asp:BoundField DataField="Produto" HeaderText="Produto" ItemStyle-Width="65%" />
                                            <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" ItemStyle-Width="5%" />
                                            <asp:BoundField DataField="PrecoUnitario" HeaderText="Preço Unitário" ItemStyle-Width="15%" DataFormatString="{0:c}" />
                                            <asp:BoundField DataField="PrecoTotal" HeaderText="Preço Total" ItemStyle-Width="15%" DataFormatString="{0:c}" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeComanda" runat="server" CancelControlID="imbFechar"
        TargetControlID="hdfGerar" PopupControlID="pnlComanda" BackgroundCssClass="background_modal"
        DropShadow="false">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlComanda" runat="server" CssClass="window_modal" Width="735px" Height="430px"
        Style="display: none;">
        <rsweb:ReportViewer ID="rpvComandaEntrega" runat="server" Width="735px" Font-Names="Verdana"
            Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="False" ShowRefreshButton="False" ShowZoomControl="False"
            ShowExportControls="true" ShowBackButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" OnClick="imbFechar_Click"
            OnClientClick="return ConfirmaImpressao();" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
