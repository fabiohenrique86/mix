<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="NotaFiscal.aspx.cs"
    Inherits="Site.NotaFiscal" Title="LANÇAMENTO - NOTA FISCAL" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2 {
            width: 100px;
        }

        .label_repeater {
            font-weight: normal;
            font-style: italic;
        }
    </style>
    <script type="text/javascript">

        function ListarProduto() {

            var gdvProduto = document.getElementById("tblProduto");
            var listaProduto = "<div style='overflow-x: hidden; overflow-y: scroll; height: 200px;'>";
            var produtoSelecionado = false;
            var descricaoProduto = "";
            var browserMozilla = $.browser.mozilla;

            if (gdvProduto != null) {

                // começa em "1" porque 0 é header
                for (var i = 1; i < gdvProduto.rows.length; i++) {

                    produtoSelecionado = true;

                    if (browserMozilla) {
                        descricaoProduto = gdvProduto.rows[i].cells[0].innerHTML;
                    }
                    else {
                        descricaoProduto = gdvProduto.rows[i].cells[0].innerText;
                    }

                    listaProduto += "<span style='text-align: left; font-weight: bold;'>Produto: </span>" + descricaoProduto + "\r\n";

                    if (gdvProduto.rows[i].cells[1].children[0].value > "0") {
                        listaProduto += "<span style='text-align: left; font-weight: bold;'>Quantidade: </span>" + gdvProduto.rows[i].cells[1].children[0].value + "\r\n";
                    }
                    else {
                        listaProduto += "<span style='text-align: left; font-weight: bold; color: Red;'>Quantidade: " + gdvProduto.rows[i].cells[1].children[0].value + "</span>\r\n";
                    }
                    listaProduto += "\r\n";

                }
                listaProduto += "</div>";
            }

            if (produtoSelecionado == true) {
                var msg = "<span style='text-align: left; font-weight: bold;'>NotaFiscalID: </span>" + document.getElementById("ctl00_ContentPlaceHolder1_txtNotaFiscalID").value + "\r\n\r\nEssa nota fiscal contém os seguintes produtos:\r\n\r\n" + listaProduto + "Tem certeza que deseja cadastrar nota fiscal?";
                var titulo = "Confirmação";
                jConfirm(msg, titulo, function (r) {
                    if (r) {
                        __doPostBack((document.getElementById("ctl00_ContentPlaceHolder1_imbCadastrar")).name, 'OnClick');
                    }
                });
            }
            return false;
        }

        function createRow(produtoId, descricao, index) {
            var row = "<tr>";
            row = row + "<td>" + descricao + "</td>";
            row = row + "<td><input type='text' maxlength='3' style='width: 30px' name='txtQuantidade_" + index + "' /></td>";
            row = row + "<td><img src='../img/excluir_produto.png' title='Excluir Produto' style='width: 25px; heigth: 25px' onclick='deleteRow(this)' /><input type='hidden' name='txtProdutoID_" + index + "' value='" + produtoId + "' /></td>"
            row = row + "</tr>";
            return row;
        }

        function deleteRow(r) {
            var i = r.parentNode.parentNode.rowIndex;
            document.getElementById("tblProduto").deleteRow(i);
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
                    quantidade = tblProduto.rows[i].cells[1].children[0].value;
                    hdf = hdf + i + ";"; // linha - 0
                    hdf = hdf + tblProduto.rows[i].cells[2].children[1].value + ";"; // produtoId - 1
                    hdf = hdf + tblProduto.rows[i].cells[0].innerText + ";"; // descrição - 2
                    if (quantidade > 0) {
                        hdf = hdf + quantidade + ";"; // quantidade - 3
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
                        row = row + "<td><input type='text' maxlength='3' style='width: 30px' value='" + cell[3] + "' name='txtQuantidade_" + j + "' /></td>";
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
                            jAlert("Produto " + ui.item.label + " já foi adicionado a nota fiscal.", "Alerta", function () { $("#ctl00_ContentPlaceHolder1_txtProduto").focus(); });
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
        <Triggers>
            <asp:PostBackTrigger ControlID="imbImportar" />
        </Triggers>
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    LANÇAMENTO - NOTA FISCAL
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblNotaFiscalID" runat="server" Text="NotaFiscalID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtNotaFiscalID" runat="server" Width="70px" MaxLength="9" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtNotaFiscalID"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblPedidoMae" runat="server" Text="PedidoMãeID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtPedidoMaeID" runat="server" Width="155px" MaxLength="30" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblLoja" runat="server" Text="Loja (Origem)" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlLoja" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID"
                                    SkinID="DropDownList" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblDataNotaFiscal1" runat="server" Text="Data Nota Fiscal" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataNotaFiscal" runat="server" Width="65px" SkinID="TextBox"
                                    CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataNotaFiscal_MaskedEditExtender" runat="server"
                                    TargetControlID="txtDataNotaFiscal" MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="Label125" runat="server" Text="Produto" SkinID="Label"></asp:Label>
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
                                            <td style="width: 75%">Produto
                                            </td>
                                            <td style="width: 20%">Quantidade
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
                                <span style="font-weight: bold">Importar arquivo XML</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:FileUpload ID="FileUpload1" Multiple="Multiple" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                                    OnClick="imbCadastrar_Click" OnClientClick="return ListarProduto();" />
                                <asp:ImageButton ID="imbImportar" runat="server" ImageUrl="~/img/importar.png"
                                    OnClick="imbImportar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <div style="overflow-x: hidden; overflow-y: scroll; height: 400px;">
                        <asp:Repeater ID="rptLoja" runat="server" OnItemDataBound="rptLoja_ItemDataBound">
                            <ItemTemplate>
                                <div class="topo_grid">
                                    <asp:Label ID="lblLojaID" runat="server" Text='<%# Bind("LojaID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblNomeFantasia" runat="server" Text='<%# Bind("NomeFantasia") %>'></asp:Label>
                                </div>
                                <asp:Repeater ID="rptData" runat="server"  OnItemDataBound="rptData_ItemDataBound">
                                    <ItemTemplate>
                                        <div style="background-color: #800000; color: #FFFFFF; font-size: 9px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label123" runat="server" Text="Data Nota Fiscal: " CssClass="negrito"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblDataNotaFiscal" runat="server" Text='<%# Bind("DataNotaFiscal","{0:dd/MM/yyyy}") %>' CssClass="label_repeater"></asp:Label></td>
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <asp:Label ID="Label234" runat="server" Text="Quantidade: " CssClass="negrito"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblQuantidade" runat="server" Text='<%# Bind("Quantidade") %>' CssClass="label_repeater"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <asp:Repeater ID="rptNotaFiscal" runat="server" OnItemDataBound="rptNotaFiscal_ItemDataBound">
                                            <ItemTemplate>
                                                <div style="background-color: #228B22; color: #FFFFFF; font-size: 9px;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label1" runat="server" Text="NotaFiscalID: " CssClass="negrito"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblNotaFiscalID" runat="server" Text='<%# Bind("NotaFiscalID") %>'
                                                                    CssClass="label_repeater"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label3" runat="server" Text="PedidoMãeID: " CssClass="negrito"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPedidoMaeID" runat="server" Text='<%# Bind("PedidoMaeID") %>' CssClass="label_repeater"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div>
                                                    <asp:GridView ID="gdvNotaFiscalProduto" runat="server" SkinID="GridView">
                                                        <Columns>
                                                            <asp:BoundField DataField="Produto" HeaderText="Produto" ItemStyle-Width="90%" />
                                                            <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" ItemStyle-Width="10%" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
