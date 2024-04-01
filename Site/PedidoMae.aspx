<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="PedidoMae.aspx.cs" Inherits="Site.PedidoMae" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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
            var row = "";
            row += "<tr>";
            row += "<td>" + descricao + "</td>";
            row += "<td><input type='text' maxlength='14' style='width: 100px' onkeypress='return formatSobMedida(this)' name='txtSobMedida_" + index + "' /></td>";
            row += "<td><input type='text' maxlength='3' style='width: 30px' name='txtQuantidade_" + index + "' /></td>";
            row += "<td><img src='../img/excluir_produto.png' title='Excluir Produto' style='width: 25px; heigth: 25px' onclick='deleteRow(this)' /><input type='hidden' name='txtProdutoID_" + index + "' value='" + produtoId + "' /></td>"
            row += "</tr>";
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

        function getValoresProduto() {
            var hdf = "";
            var tblProduto = document.getElementById("tblProduto");
            var quantidade;
            var preco;

            if (tblProduto != null && tblProduto.rows.length > 0) {
                for (var i = 1; i < tblProduto.rows.length; i++) {
                    quantidade = tblProduto.rows[i].cells[2].children[0].value;

                    hdf = hdf + i + ";"; // linha - 0
                    hdf = hdf + tblProduto.rows[i].cells[3].children[1].value + ";"; // produtoId - 1
                    hdf = hdf + tblProduto.rows[i].cells[0].innerText + ";"; // descrição - 2
                    hdf = hdf + tblProduto.rows[i].cells[1].children[0].value + ";"; // sob medida - 4
                    
                    if (quantidade > 0) {
                        hdf = hdf + quantidade + ";"; // quantidade - 5
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
                        row = row + "<td><input type='text' maxlength='3' style='width: 30px' value='" + cell[4] + "' name='txtQuantidade_" + j + "' /></td>";
                        row = row + "<td><img style='width: 25px; heigth: 25px' src='../img/excluir_produto.png' title='Excluir Produto' onclick='deleteRow(this)' /><input type='hidden' name='txtProdutoID_" + j + "' value='" + cell[1] + "' /></td>";
                        row = row + "</tr>";
                        $("#tblProdutoBody").append(row);
                        j++;
                    }
                }
                document.getElementById("ctl00_ContentPlaceHolder1_hdfProdutoValores").value = "";
            }
        }

        function getAutocomplete() {
            $('#<%=txtProduto.ClientID%>').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%= Page.ResolveUrl("~/Produto.aspx/GetProduct")%>',
                            data: "{ term: '" + request.term + "', lojaId: " + '0' + ", sistemaId: " + '<%= new BLL.Modelo.Usuario(Session["Usuario"]).SistemaID %>' + "}",
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
                            jAlert("Produto " + ui.item.label + " já foi adicionado ao pedido mãe.", "Alerta", function () { $("#ctl00_ContentPlaceHolder1_txtProduto").focus(); });
                        }
                        else {
                            $("#tblProdutoBody").append(createRow(ui.item.value, ui.item.label, tblProduto.rows.length));
                        }
                    }
                    ui.item.label = "";
                    ui.item.value = "";
                }
            });
            }
            //});
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfProdutoValores" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="imbConsultar" />
            <asp:AsyncPostBackTrigger ControlID="imbCadastrar" />
        </Triggers>
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    CONSULTA - PEDIDO MÃE
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="Label1" runat="server" Text="PedidoMãeID" SkinID="Label"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TxtPedidoMaeID" runat="server" MaxLength="30" Width="155px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblDataNotaFiscal" runat="server" Text="Data Cadastro" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataCadastro" runat="server" Width="65px" SkinID="TextBox"
                                    CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataCadastro_MaskedEditExtender" runat="server"
                                    TargetControlID="txtDataCadastro" MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="Label2" runat="server" Text="Produto" SkinID="Label"></asp:Label>
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
                                            <td style="width: 70%">Produto
                                            </td>
                                            <td style="width: 15%">Sob Medida
                                            </td>
                                            <td style="width: 10%">Quantidade
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
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png" OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png" OnClick="imbCadastrar_Click" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" OnClick="imbExcluir_Click" />
                                <%--<asp:ImageButton ID="imbImportar" runat="server" ImageUrl="~/img/importar.png" OnClick="imbImportar_Click" />--%>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <div id="divScrollReserva" style="overflow-x: hidden; overflow-y: scroll; height: 400px;">
                        <asp:Repeater ID="rptPedidoMae" runat="server" OnItemDataBound="rptPedidoMae_ItemDataBound">
                            <ItemTemplate>
                                <div id="repeaterPedidoMae" class="topo_grid" style="text-align: left;" runat="server">
                                    <table>
                                        <tr>
                                            <td>PedidoMãeID:</td>
                                            <td class="label_pedido"><asp:Label ID="lblPedidoMaeID" runat="server" Text='<%# Bind("PedidoMaeID") %>'></asp:Label></td>
                                            <td>Data Cadastro:</td>
                                            <td class="label_pedido"><asp:Label ID="lblDataCadastro" runat="server" Text='<%# Bind("DataCadastro", "{0:dd/MM/yyyy HH:mm}") %>'></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:GridView ID="gdvPedidoMaeProduto" runat="server" SkinID="GridView">
                                        <Columns>
                                            <asp:BoundField DataField="ProdutoID" HeaderText="ProdutoID" ItemStyle-Width="15%" />
                                            <asp:BoundField DataField="Produto" HeaderText="Produto" ItemStyle-Width="60%" />
                                            <asp:BoundField DataField="Medida" HeaderText="Medida" ItemStyle-Width="15%" />
                                            <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" ItemStyle-Width="10%" />
                                        </Columns>
                                    </asp:GridView>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
