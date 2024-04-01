<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Troca.aspx.cs"
    Inherits="Site.Troca" Title="LANÇAMENTO - TROCA" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Import Namespace="System.Data" %>

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
        }

        function clearGrid() {
            $('#tblProduto tbody > tr').remove();
        }

        function getValoresProduto() {
            var hdf = "";
            var tblProduto = document.getElementById("tblProduto");
            var quantidade;

            if (tblProduto != null && tblProduto.rows.length > 0) {
                for (var i = 1; i < tblProduto.rows.length; i++) {
                    quantidade = tblProduto.rows[i].cells[2].children[0].value; // quantidade - 3

                    hdf = hdf + i + ";"; // linha - 0
                    hdf = hdf + tblProduto.rows[i].cells[3].children[1].value + ";"; // produtoId - 4
                    hdf = hdf + tblProduto.rows[i].cells[0].innerText + ";"; // descrição - 1
                    hdf = hdf + tblProduto.rows[i].cells[1].children[0].value + ";"; // sob medida - 2

                    if (quantidade > 0) {
                        hdf = hdf + quantidade + ";";
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
                            jAlert("Produto " + ui.item.label + " já foi adicionado ao pedido.", "Alerta", function () { $("#ctl00_ContentPlaceHolder1_txtProduto").focus(); });
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfProdutoValores" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="imbConsultar" />
            <asp:AsyncPostBackTrigger ControlID="imbInserirTroca" />
        </Triggers>
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    LANÇAMENTO - TROCA
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="Label32" runat="server" Text="TrocaID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtTrocaID" runat="server" Width="80px" MaxLength="8" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtTrocaID_FilteredTextBoxExtender" runat="server"
                                    FilterType="Numbers" TargetControlID="txtTrocaID">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="_lbl" runat="server" Text="SVT" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtSVT" runat="server" Width="150px" MaxLength="25" SkinID="TextBox"></asp:TextBox>
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
                                <asp:Label ID="label3" runat="server" Text="Loja da Troca" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlLoja" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID"
                                    SkinID="DropDownList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="labal_DataEntrega" runat="server" Text="Data Entrega" SkinID="Label"></asp:Label>&nbsp;&nbsp;&nbsp;
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataEntrega" runat="server" Width="65px" Height="15px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataEntrega_MaskedEditExtender" runat="server" TargetControlID="txtDataEntrega"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="label_status_filtro" runat="server" Text="Status" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlStatus" runat="server" SkinID="DropDownListRelatorio" Width="100px">
                                    <asp:ListItem Text="Todos" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Pendente" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Baixada" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <span class="campo_opcional">* [Só informe para CONSULTAR]</span>
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
                            <td class="style1">
                                <asp:Label ID="Label123" runat="server" Text="Produto" SkinID="Label"></asp:Label>
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
                                            <td style="width: 45%">Produto
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
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbInserirTroca" runat="server" ImageUrl="~/img/cadastrar.png"
                                    OnClick="imbInserirTroca_Click" />
                                <asp:ImageButton ID="imbDarBaixa" runat="server" ImageUrl="~/img/dar_baixa.png"
                                    OnClick="imbDarBaixa_Click" />
                                <asp:ImageButton ID="imbCancelar" runat="server" ImageUrl="~/img/cancelar.png" OnClick="imbCancelar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <div id="divScrollTroca" style="overflow-x: hidden; overflow-y: scroll; height: 400px;">
                        <asp:Repeater ID="rptTroca" runat="server" OnItemDataBound="rptTroca_ItemDataBound">
                            <ItemTemplate>
                                <div id="repeaterTroca" class="topo_grid" style="text-align: left;" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbCheck" runat="server" ToolTip='<%# Bind("TrocaID") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTroca" runat="server" Text="TrocaID:"></asp:Label>
                                            </td>
                                            <td class="label_pedido">
                                                <asp:LinkButton ID="lkbTrocaID" runat="server" Text='<%# Bind("TrocaID") %>' OnClick="lkbTrocaID_Click" Font-Size="12px"></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNomeFantasia" runat="server" Text="Loja:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblNomeFantasiaID" runat="server" Text='<%# Bind("NomeFantasia") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCliente" runat="server" Text="Cliente:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblClienteID" runat="server" Text='<%# Bind("NomeCliente") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDataTroca" runat="server" Text="Data Troca:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblDataTrocaID" runat="server" Text='<%# Bind("DataEntrega","{0:dd/MM/yyyy}") %>'></asp:Label>
                                                <asp:Label ID="lblAtivo" runat="server" Text='<%# Bind("Ativo") %>' Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="_lblSvt" runat="server" Text="SVT:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblSvt" runat="server" Text='<%# Bind("Svt") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:GridView ID="gdvTrocaProduto" runat="server" SkinID="GridView">
                                        <Columns>
                                            <asp:BoundField DataField="ProdutoDAO.Produto" HeaderText="Produto" ItemStyle-Width="80%" />
                                            <asp:BoundField DataField="ProdutoDAO.Quantidade" HeaderText="Quantidade" ItemStyle-Width="20%" />
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
    <!-- COMANDA DE TROCA -->
    <cc1:ModalPopupExtender ID="mpeTroca" runat="server" CancelControlID="imbFechar" TargetControlID="hdfGerar" PopupControlID="pnlTroca" BackgroundCssClass="background_modal" DropShadow="false">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlTroca" runat="server" CssClass="window_modal" Width="735px" Height="430px"
        Style="display: none;">
        <rsweb:ReportViewer ID="rpvComandaTroca" runat="server" Width="735px" Font-Names="Verdana"
            Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="False" ShowRefreshButton="False" ShowZoomControl="False"
            ShowExportControls="true" ShowBackButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
