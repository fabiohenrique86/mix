<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Pedido2.aspx.cs"
    Inherits="Site.Pedido2" Title="LANÇAMENTO - PEDIDO" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 110px;
        }
        .label_repeater
        {
            font-weight: normal;
            font-style: italic;
        }
        .total_detalhe
        {
            text-align: right;
        }
    </style>
    <script type="text/javascript">

        function ListarProduto() {

            var gdvProduto = document.getElementById("tblProduto");
            var listaProduto = "<div style='overflow-x: hidden; overflow-y: scroll; height: 200px;'>";
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
                var msg = "<span style='text-align: left; font-weight: bold;'>PedidoID: </span>" + document.getElementById("ctl00_ContentPlaceHolder1_txtPedidoID").value + "\r\n<span style='text-align: left; font-weight: bold;'>Loja Saída: </span>" + ddlLojaSaida.options[ddlLojaSaida.selectedIndex].text + "\r\n\r\nEsse pedido contém os seguintes produtos:\r\n\r\n" + listaProduto + "Tem certeza que deseja inserir pedido?";
                var titulo = "Confirmação";
                jConfirm(msg, titulo, function (r) {
                    if (r) {
                        __doPostBack((document.getElementById("ctl00_ContentPlaceHolder1_imbInserirPedido")).name, 'OnClick');
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
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                num = num.substring(0, num.length - (4 * i + 3)) + '.' + num.substring(num.length - (4 * i + 3));
            ret = num + ',' + cents;
            if (x == 1)
                ret = ' – ' + ret;
            return ret;
        }

        //function CalcularPagamentoTotal() {
        //    var gdvTipoPagamento = document.getElementById("ctl00_ContentPlaceHolder1_gdvTipoPagamento");
        //    var Total;
        //    var TotalFloat = 0;
        //    var quantidade;
        //    var preco;

        //    for (var i = 1; i < gdvTipoPagamento.rows.length; i++) {
        //        quantidade = gdvTipoPagamento.rows[i].cells[2].children[0].value;
        //        preco = gdvTipoPagamento.rows[i].cells[3].children[0].value;
        //        if ((gdvTipoPagamento.rows[i].cells[0].children[0].checked) && (quantidade > 0) && (preco > "0,00")) {
        //            Total = parseFloat(preco.replace(".", "").replace(",", "."));
        //            if (!isNaN(Total)) {
        //                TotalFloat = parseFloat(TotalFloat) + (parseFloat(Total) * parseInt(quantidade));
        //            }
        //        }
        //    }
        //    if (!isNaN(TotalFloat)) {
        //        document.getElementById("ctl00_ContentPlaceHolder1_txtTotalPago").value = FormatarDecimal(TotalFloat);
        //    }
        //}

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="imbConsultar" />
            <asp:AsyncPostBackTrigger ControlID="imbInserirPedido" />
        </Triggers>
        <ContentTemplate>
            <div id="conteudo">
                <input type="hidden" id="hdnScrollProduto" runat="server" value="0" />
                <input type="hidden" id="hdnScrollPedido" runat="server" value="0" />
                <div id="topo_cabeca">
                    LANÇAMENTO - PEDIDO
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblPedidoID" runat="server" Text="PedidoID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtPedidoID" runat="server" Width="155px" MaxLength="30" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblLoja" runat="server" Text="Loja (Saída)" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlLoja" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID"
                                    SkinID="DropDownList" />
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
                                            <td style="width: 60%">
                                                Produto
                                            </td>
                                            <td style="width: 15%">
                                                Sob Medida
                                            </td>
                                            <td style="width: 10%">
                                                Quantidade
                                            </td>
                                            <td style="width: 10%">
                                                Preço
                                            </td>
                                            <td style="width: 5%">
                                                Ação
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
                                <asp:TextBox ID="txtTotalPedido" runat="server" Text="" Width="80px" CssClass="decimal"
                                    Enabled="false" Height="15px" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<tr>
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
                                                    Height="14px" SkinID="TextBox" CssClass="decimal"></asp:TextBox>
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
                                <asp:TextBox ID="txtTotalPago" runat="server" Text="" Width="80px" CssClass="decimal"
                                    Enabled="false" Height="15px" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblDataPedido" runat="server" Text="Data Pedido" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataPedido" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataPedido_MaskedEditExtender" runat="server" TargetControlID="txtDataPedido"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
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
                            <td colspan="2" class="campo_opcional">
                                * campo opcional
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbInserirPedido" runat="server" ImageUrl="~/img/inserir_pedido.png"
                                    OnClick="imbInserirPedido_Click" OnClientClick="return ListarProduto();" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <div id="divScrollPedido" style="overflow-x: hidden; overflow-y: scroll; height: 400px;">
                        <asp:Repeater ID="rptPedido" runat="server" OnItemDataBound="rptPedido_ItemDataBound">
                            <ItemTemplate>
                                <div id="repeaterPedido" runat="server" class="topo_grid" style="text-align: left;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPedido" runat="server" Text="PedidoID:"></asp:Label>
                                            </td>
                                            <td class="label_pedido">
                                                <asp:LinkButton ID="lkbPedidoID" runat="server" Text='<%# Bind("PedidoID") %>' OnClick="lkbPedidoID_Click"
                                                    Visible="false" />
                                                <asp:Label ID="lblPedidoID" runat="server" Text='<%# Bind("PedidoID") %>' Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNomeFantasia" runat="server" Text="Loja Origem:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblNomeFantasiaID" runat="server" Text='<%# Bind("LojaOrigem") %>'></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLojaOrigem" runat="server" Text="Loja Saída:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblLojaOrigemID" runat="server" Text='<%# Bind("NomeFantasia") %>'></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFuncionario" runat="server" Text="Funcionário:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblFuncionarioID" runat="server" Text='<%# Bind("Funcionario") %>'></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCliente" runat="server" Text="Cliente:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblClienteID" runat="server" Text='<%# Bind("Cliente") %>'></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDataPedido" runat="server" Text="Data Pedido:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblDataPedidoID" runat="server" Text='<%# Bind("DataPedido","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblStatusImprimido" runat="server" Text="Impresso:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblStatusImprimidoID" runat="server" Text='<%# Bind("StatusImprimido") %>'></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:LinkButton ID="lkbDetalhe" runat="server" Text="Detalhes" OnClick="lkbDetalhe_Click"
                                                    CommandArgument='<%# Bind("PedidoID") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblObservacao" runat="server" Text='<%# Bind("Observacao") %>' Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:GridView ID="gdvPedidoProduto" runat="server" SkinID="GridView">
                                        <Columns>
                                            <asp:BoundField DataField="Produto" HeaderText="Produto" ItemStyle-Width="65%" />
                                            <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" ItemStyle-Width="5%" />
                                            <asp:BoundField DataField="PrecoUnitario" HeaderText="Preço Unitário" ItemStyle-Width="15%"
                                                DataFormatString="{0:c}" />
                                            <asp:BoundField DataField="PrecoTotal" HeaderText="Preço Total" ItemStyle-Width="15%"
                                                DataFormatString="{0:c}" />
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
            ShowExportControls="true" ShowBackButton="false" ShowPrintButton="true">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" OnClick="imbFechar_Click"
            OnClientClick="return ConfirmaImpressao();" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
