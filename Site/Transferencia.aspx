<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Transferencia.aspx.cs"
    Inherits="Site.Transferencia" Title="LANÇAMENTO - TRANSFERÊNCIA DE PRODUTO" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1 {
            width: 100px;
        }
    </style>
    <script type="text/javascript" language="javascript">

        function ImprimirComandaTransferencia(transferenciaDao) {
            
            var gdvProduto = document.getElementById("tblProduto");
            var produtoSelecionado = false;
            var tabelaComanda = "";
            var ddlLojaOrigem = document.getElementById('ctl00_ContentPlaceHolder1_ddlLojaDe');
            var ddlLojaSaida = document.getElementById('ctl00_ContentPlaceHolder1_ddlLojaPara');

            tabelaComanda += "<table style='width: 900px; font-family: segoe UI, sans-serif;' cellpadding='0'; cellspacing='0'>";
            tabelaComanda += "<tr>";
            tabelaComanda += "<td colspan='2'><img src='../img/logo_mix.png' alt='' /></td>";
            tabelaComanda += "</tr>";

            tabelaComanda += "<tr style='height: 30px;'>";
            tabelaComanda += "<td colspan='2' style='text-align: center; font-weight: bold; border: solid 1px #000; font-size: 16px;'>COMANDA DE TRANSFERÊNCIA DE PRODUTO</td>"
            tabelaComanda += "</tr>";

            tabelaComanda += "<tr style='font-size: 15px; height: 30px;'>";
            tabelaComanda += "<td style='width: 180px; border: solid 1px #000; text-align: center; font-weight: bold;'>TRANSFERENCIAID</td>";
            tabelaComanda += "<td style='width: 720px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + transferenciaDao.TransferenciaID + "</td>";
            tabelaComanda += "</tr>";

            tabelaComanda += "<tr style='font-size: 15px; height: 30px;'>";
            tabelaComanda += "<td style='width: 180px; border: solid 1px #000; text-align: center; font-weight: bold;'>LOJA ORIGEM</td>";
            tabelaComanda += "<td style='width: 720px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + ddlLojaOrigem.options[ddlLojaOrigem.selectedIndex].text + "</td>";
            tabelaComanda += "</tr>";

            tabelaComanda += "<tr style='font-size: 15px; height: 30px;'>";
            tabelaComanda += "<td style='width: 180px; border: solid 1px #000; text-align: center; font-weight: bold;'>LOJA SAÍDA</td>";
            tabelaComanda += "<td style='width: 720px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + ddlLojaSaida.options[ddlLojaSaida.selectedIndex].text + "</td>";
            tabelaComanda += "</tr>";

            tabelaComanda += "<tr style='font-size: 15px; height: 30px;'>";
            tabelaComanda += "<td style='width: 180px; border: solid 1px #000; text-align: center; font-weight: bold;'>DATA TRANSFERÊNCIA</td>";
            tabelaComanda += "<td style='width: 720px; border: solid 1px #000; text-align: left; padding-left: 5px;'>" + document.getElementById("ctl00_ContentPlaceHolder1_txtDataTransferencia").value + "</td>";
            tabelaComanda += "</tr>";

            tabelaComanda += "</table>";

            tabelaComanda += "<table style='width: 900px; font-family: segoe UI, sans-serif;' cellpadding='0'; cellspacing='0'>";

            tabelaComanda += "<tr style='font-size: 15px; height: 30px;'>";
            tabelaComanda += "<td style='width: 800px; text-align: center; font-weight: bold; border: solid 1px #000;'>PRODUTO</td>";
            tabelaComanda += "<td style='width: 100px; text-align: center; font-weight: bold; border: solid 1px #000;'>QUANTIDADE</td>";
            tabelaComanda += "</tr>";
            
            //var valoresProduto = document.getElementById("ctl00_ContentPlaceHolder1_hdfProdutoValores").value;
            //if (valoresProduto != null && valoresProduto != "")
            //{
            //    produtoSelecionado = true;

            //    var linha = valoresProduto.split('~~');
            //    for (var i = 0; i < linha.length; i++) {
            //        var cell = linha[i].split(';');
            //        tabelaComanda += "<tr style='font-size: 15px; height: 30px;'>";
            //        tabelaComanda += "<td style='width: 800px; text-align: center; border: solid 1px #000;'>" + cell[2] + "</td>";
            //        tabelaComanda += "<td style='width: 100px; text-align: center; border: solid 1px #000;'>" + cell[3] + "</td>";
            //        tabelaComanda += "</tr>";
            //    }
            //}
                        
            $.each(transferenciaDao.ListaProduto, function (i, e) {
                tabelaComanda += "<tr style='font-size: 15px; height: 30px;'>";
                tabelaComanda += "<td style='width: 800px; text-align: center; border: solid 1px #000;'>" + e.ProdutoID + " - " + e.Descricao + " - " + e.Medida + "</td>";
                tabelaComanda += "<td style='width: 100px; text-align: center; border: solid 1px #000;'>" + e.Quantidade + "</td>";
                tabelaComanda += "</tr>";
            });

            tabelaComanda += "</table>";
            
            if (transferenciaDao.ListaProduto != null && transferenciaDao.ListaProduto.length > 0)
            {
                var janela = window.open("", "", "status=0,toolbar=0,resizable=0,menubar=0,titlebar=0,width=910,height=500");
                var conteudo = "<html><body onload='window.print(); window.close();'>" + tabelaComanda + "</body></html>";
                janela.document.open();
                janela.document.write(conteudo);
                janela.moveTo(((screen.width - 910) / 2), ((screen.height - 500) / 2));
                janela.document.close();
            }

            document.getElementById('ctl00_ContentPlaceHolder1_txtTransferenciaId').value = '';
            document.getElementById('ctl00_ContentPlaceHolder1_ddlLojaDe').value = 0;
            document.getElementById('ctl00_ContentPlaceHolder1_ddlLojaPara').value = 0;
            document.getElementById('ctl00_ContentPlaceHolder1_txtDataTransferencia').value = '';
            document.getElementById('ctl00_ContentPlaceHolder1_hdfProdutoValores').value = '';

            clearGrid();
        }

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

                var quantidade = gdvProduto.rows[i].cells[1].children[0].value;

                listaProduto += "<span style='text-align: left; font-weight: bold;'>Produto: </span>" + descricaoProduto + "\r\n";

                if (quantidade > "0") {
                    listaProduto += "<span style='text-align: left; font-weight: bold;'>Quantidade: </span>" + quantidade + "\r\n";
                }
                else {
                    listaProduto += "<span style='text-align: left; font-weight: bold; color: Red;'>Quantidade: </span>" + quantidade + "\r\n";
                }
            }

            listaProduto += "</div>";

            if (produtoSelecionado) {
                var msg = "Essa transferência contém os seguintes produtos:\r\n\r\n" + listaProduto + "Tem certeza que deseja transferir?";
                var titulo = "Confirmação";
                jConfirm(msg, titulo, function (r) {
                    if (r) {
                        __doPostBack((document.getElementById("ctl00_ContentPlaceHolder1_imbTransferir")).name, 'OnClick');
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
                        data: "{ term: '" + request.term + "', lojaId: " + $("#ctl00_ContentPlaceHolder1_ddlLojaDe").val() + ", sistemaId: " + '<%= new BLL.Modelo.Usuario(Session["Usuario"]).SistemaID %>' + "}",
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
                            jAlert("Produto " + ui.item.label + " já foi adicionado a transferência.", "Alerta", function () { $("#ctl00_ContentPlaceHolder1_txtProduto").focus(); });
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
        <ContentTemplate>
            <div id="conteudo">
                <input type="hidden" id="hdnScrollProduto" runat="server" value="0" />
                <input type="hidden" id="hdnScrollTransferencia" runat="server" value="0" />
                <div id="topo_cabeca">
                    LANÇAMENTO - TRANSFERÊNCIA DE PRODUTO
                </div>
                <div id="cabeca">
                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblTransferenciaId" runat="server" Text="TransferenciaID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtTransferenciaId" runat="server" Width="85px" SkinID="TextBox" MaxLength="9"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtTransferenciaId_FilteredTextBoxExtender"
                                    runat="server" FilterType="Numbers" TargetControlID="txtTransferenciaId">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblLojaDe" runat="server" Text="Loja (Origem)" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlLojaDe" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID"
                                    SkinID="DropDownList" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblLojaPara" runat="server" Text="Loja (Destino)" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlLojaPara" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID"
                                    SkinID="DropDownList">
                                </asp:DropDownList>
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
                            <td class="style1">
                                <asp:Label ID="lblDataTransferencia" runat="server" Text="Data Transferência" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataTransferencia" runat="server" Width="65px" SkinID="TextBox"
                                    CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataTransferencia_MaskedEditExtender" runat="server"
                                    TargetControlID="txtDataTransferencia" MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr runat="server" id="trComanda">
                            <td class="style1">Status Comanda</td>
                            <td style="text-align: left;">
                                <asp:RadioButtonList ID="rblValida" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Não validada" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Validada" Value="1" Selected="True"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                        OnClick="imbConsultar_Click" />
                    <asp:ImageButton ID="imbTransferir" runat="server" ImageUrl="~/img/transferir.png"
                        OnClick="imbTransferir_Click" OnClientClick="return ListarProduto();" />
                    <asp:ImageButton ID="imbGerarComanda" runat="server" ImageUrl="~/img/gerar_comanda.png" OnClick="imbGerarComanda_Click" />
                    <asp:ImageButton ID="imbValidar" runat="server" ImageUrl="~/img/validar.png" OnClick="imbValidar_Click" />
                    <%--<asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" OnClick="imbExcluir_Click" />
                    <asp:ImageButton ID="imbReabrir" runat="server" ImageUrl="~/img/reabrir.png" OnClick="imbReabrir_Click" />--%>
                </div>
                <div id="corpo">
                    <div id="divScrollTransferencia" style="overflow-x: hidden; overflow-y: scroll; height: 400px;">
                        <asp:Repeater ID="rptTransferencia" runat="server" OnItemDataBound="rptTransferencia_ItemDataBound">
                            <ItemTemplate>
                                <div id="repeaterTransferencia" runat="server" class="topo_grid" style="text-align: left;">
                                    <table>
                                        <tr>
                                            <td>TransferênciaID:
                                            </td>
                                            <td class="label_pedido">
                                                <asp:Label ID="lblTransferenciaID" runat="server" Text='<%# Bind("TransferenciaID") %>'></asp:Label>
                                            </td>
                                            <td>Loja Origem:
                                            </td>
                                            <td class="label_pedido">
                                                <asp:Label ID="lblLojaDe" runat="server" Text='<%# Bind("De") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>Loja Destino:
                                            </td>
                                            <td class="label_pedido">
                                                <asp:Label ID="lblLojaPara" runat="server" Text='<%# Bind("Para") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;
                                            </td>
                                            <td>Data Transferência:
                                            </td>
                                            <td class="label_pedido">
                                                <asp:Label ID="lblDataTransferencia" runat="server" Text='<%# Bind("DataTransferencia","{0:d}") %>'></asp:Label>                                                
                                                <asp:Label ID="lblValida" runat="server" Text='<%# Bind("Valida") %>' Visible="false"></asp:Label>
                                            </td>
                                            <td>Data Validação:</td>
                                            <td class="label_pedido">
                                                <asp:Label ID="lblDataValida" runat="server" Text='<%# Bind("DataValida","{0:dd/MM/yyyy HH:mm}") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:GridView ID="gdvProduto" runat="server" SkinID="GridView">
                                        <Columns>
                                            <asp:BoundField DataField="Produto" HeaderText="Produto" ItemStyle-Width="90%" />
                                            <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" ItemStyle-Width="10%" />
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
    <asp:Panel ID="pnlComanda" runat="server" CssClass="window_modal" Width="735px" Height="430px" Style="display: none;">
        <rsweb:ReportViewer ID="rpvComandaTransferencia" runat="server" Width="735px" Font-Names="Verdana"
            Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="False" ShowRefreshButton="False" ShowZoomControl="False"
            ShowExportControls="true" ShowBackButton="false" ShowPrintButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbImprimir" runat="server" ImageUrl="~/img/imprimir.gif" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
