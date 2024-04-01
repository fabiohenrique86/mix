<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Orcamento.aspx.cs"
    Inherits="Site.Orcamento" Title="LANÇAMENTO - ORÇAMENTO" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100px;
        }
        .label_repeater
        {
            font-weight: normal;
            font-style: italic;
        }
    </style>
    <script type="text/javascript">
        
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
                document.getElementById("ctl00_ContentPlaceHolder1_txtTotalOrcamento").value = FormatarDecimal(TotalFloat);
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
                            jAlert("Produto " + ui.item.label + " já foi adicionado ao orçamento.", "Alerta", function () { $("#ctl00_ContentPlaceHolder1_txtProduto").focus(); });
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    LANÇAMENTO - ORÇAMENTO
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblOrcamentoID" runat="server" Text="OrçamentoID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtOrcamentoID" runat="server" Width="55px" MaxLength="7" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtOrcamentoID"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                                <asp:CheckBox ID="ckbOrcamentoID" AutoPostBack="true" Text="Cadastrar/Excluir" runat="server"
                                    OnCheckedChanged="ckbOrcamentoID_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblLoja" runat="server" Text="Loja" SkinID="Label"></asp:Label>
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
                                <asp:Label ID="lblDataOrcamento" runat="server" Text="Data Orçamento" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataOrcamento" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataOrcamento_MaskedEditExtender" runat="server" TargetControlID="txtDataOrcamento"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
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
                        <tr id="TotalOrcamento" runat="server">
                            <td colspan="2" style="text-align: right;">
                                <asp:Label ID="lblTotal" runat="server" Width="100px" Text="Total Orçamento : " CssClass="negrito"></asp:Label>
                                <asp:TextBox ID="txtTotalOrcamento" runat="server" Text="" Width="80px" CssClass="decimal"
                                    Enabled="false" Height="15px" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                                    Visible="false" OnClick="imbCadastrar_Click" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" Visible="false"
                                    OnClick="imbExcluir_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <div style="overflow-x: hidden; overflow-y: scroll; height: 400px;">
                        <asp:Repeater ID="rptLoja" runat="server" OnItemDataBound="rptLoja_ItemDataBound">
                            <ItemTemplate>
                                <div id="repeaterLoja" class="topo_grid" runat="server">
                                    <asp:Label ID="lblLojaID" runat="server" Text='<%# Bind("LojaID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblNomeFantasia" runat="server" Text='<%# Bind("NomeFantasia") %>'></asp:Label>
                                </div>
                                <asp:Repeater ID="rptOrcamento" runat="server" OnItemDataBound="rptOrcamento_ItemDataBound">
                                    <ItemTemplate>
                                        <div id="repeaterOrcamento" runat="server" style="background-color: #800000; color: #FFFFFF;
                                            font-size: 9px; font-weight: bold;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOrcamento" runat="server" Text="OrçamentoID:"></asp:Label>
                                                    </td>
                                                    <td class="label_repeater">
                                                        <asp:LinkButton ID="lkbOrcamentoID" runat="server" Text='<%# Bind("OrcamentoID") %>'
                                                            OnClick="lkbOrcamentoID_Click" Visible="false" CommandArgument='<%# Bind("OrcamentoID") %>'>
                                                        </asp:LinkButton>
                                                        <asp:Label ID="lblOrcamentoID" runat="server" Text='<%# Bind("OrcamentoID") %>' Visible="false"></asp:Label>
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
                                                        <asp:Label ID="lblDataOrcamento" runat="server" Text="Data Orçamento:"></asp:Label>
                                                    </td>
                                                    <td class="label_repeater">
                                                        <asp:Label ID="lblDataOrcamentoID" runat="server" Text='<%# Bind("DataOrcamento","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div>
                                                <asp:GridView ID="gdvOrcamentoProduto" runat="server" SkinID="GridView">
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
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeOrcamento" runat="server" CancelControlID="imbFechar"
        TargetControlID="hdfGerar" PopupControlID="pnlRelatorio" BackgroundCssClass="background_modal"
        DropShadow="false">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlRelatorio" runat="server" CssClass="window_modal" Width="740px"
        Height="430px" Style="display: none;">
        <rsweb:ReportViewer ID="rpvOrcamento" runat="server" Width="740px" Font-Names="Verdana"
            Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="true" ShowRefreshButton="False" ShowZoomControl="False"
            ShowExportControls="true" ShowBackButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
