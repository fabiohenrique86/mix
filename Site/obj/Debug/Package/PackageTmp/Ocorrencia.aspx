<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Ocorrencia.aspx.cs"
    Inherits="Site.Ocorrencia" Title="LANÇAMENTO - OCORRÊNCIA" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Import Namespace="System.Data" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1 {
            width: 160px;
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

        function createRow(produtoId, descricao, index) {
            var row = "";

            row += "<tr>";
            row += "<td>" + descricao + "</td>";
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
                                var noData = ["Produto não cadastrado"];
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
                    if (ui.item.label == "Produto não cadastrado") {
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
                            jAlert("Produto " + ui.item.label + " já foi adicionado a ocorrência.", "Alerta", function () { $("#ctl00_ContentPlaceHolder1_txtProduto").focus(); });
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfProdutoValores" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="imbConsultar" />
            <asp:AsyncPostBackTrigger ControlID="imbGerarOcorrencia" />
            <asp:AsyncPostBackTrigger ControlID="imbDarBaixa" />
        </Triggers>
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    LANÇAMENTO - OCORRÊNCIA
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="label7" runat="server" Text="OcorrenciaID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtOcorrenciaID" runat="server" Width="85px" MaxLength="10" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtOcorrenciaID"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                                <span class="campo_opcional">* [Só informe para CONSULTAR/DAR BAIXA da ocorrência]</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="label9" runat="server" Text="Número de Troca (SVT)" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="TxtNumeroTroca" runat="server" Width="85px" MaxLength="10" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="TxtNumeroTroca"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                                <span class="campo_opcional">* [Só informe para CONSULTAR/DAR BAIXA da ocorrência (Motivo: TROCA DE MERCADORIA)]</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="label8" runat="server" Text="Data Ocorrência" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDataOcorrenciaInicial" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataReservaInicial_MaskedEditExtender1" runat="server" TargetControlID="txtDataOcorrenciaInicial"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                                &nbsp;a&nbsp;
                                    <asp:TextBox ID="txtDataOcorrenciaFinal" runat="server" Width="65px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataReservaFinal_MaskedEditExtender2" runat="server" TargetControlID="txtDataOcorrenciaFinal"
                                    MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                                <span class="campo_opcional">* [Só informe para CONSULTAR ocorrência]</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="label11" runat="server" Text="Status Ocorrência" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlStatusOcorrencia" runat="server" SkinID="DropDownListRelatorio" Width="100px">
                                    <asp:ListItem Text="SELECIONE" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Pendente" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Realizada" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                                <span class="campo_opcional">* [Só informe para CONSULTAR ocorrência]</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="label1" runat="server" Text="Nº Nota Fiscal de Origem" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtNotaFiscalID" runat="server" Width="85px" MaxLength="15" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtNotaFiscalID"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblLoja" runat="server" Text="Loja da Ocorrência" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlLoja" runat="server" DataTextField="NomeFantasia" DataValueField="LojaID" SkinID="DropDownList" />
                            </td>
                        </tr>                        
                        <tr>
                            <td class="style1">
                                <asp:Label ID="label2" runat="server" Text="Motivo da Ocorrência" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlMotivoOcorrencia" runat="server" SkinID="DropDownList" DataTextField="Descricao" DataValueField="MotivoOcorrenciaID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="label3" runat="server" Text="Nome do Motorista" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtNomeMotorista" runat="server" Width="300px" SkinID="TextBox" MaxLength="150"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="label4" runat="server" Text="Placa do Caminhão" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtPlacaCaminhao" runat="server" Width="125px" SkinID="TextBox" MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>                        
                        <tr>
                            <td class="style1">
                                <asp:Label ID="label5" runat="server" Text="Produto" SkinID="Label"></asp:Label>
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
                                            <td style="width: 85%">Produto
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
                            <td class="style1">
                                <asp:Label ID="label6" runat="server" Text="Observação" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtObservacao" runat="server" Width="750px" MaxLength="350" TextMode="MultiLine"
                                    Height="80px" SkinID="TextBox">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbGerarOcorrencia" runat="server" ImageUrl="~/img/gerar.png"
                                    OnClick="imbGerarOcorrencia_Click" />
                                <asp:ImageButton ID="imbDarBaixa" runat="server" ImageUrl="~/img/dar_baixa.png"
                                    OnClick="imbDarBaixa_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                    <div id="divScrollReserva" style="overflow-x: hidden; overflow-y: scroll; height: 400px;">
                        <asp:Repeater ID="rptOcorrencia" runat="server" OnItemDataBound="rptOcorrencia_ItemDataBound">
                            <ItemTemplate>
                                <div id="repeaterOcorrencia" class="topo_grid" style="text-align: left;" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblOcorrencia_" runat="server" Text="OcorrenciaID:"></asp:Label>
                                            </td>
                                            <td class="label_pedido">
                                                 <asp:LinkButton ID="lkbOcorrenciaID" runat="server" Text='<%# Bind("OcorrenciaID") %>' OnClick="lkbOcorrenciaID_Click"
                                                    Visible="false">
                                                </asp:LinkButton>
                                                <asp:Label ID="lblOcorrencia" runat="server" Text='<%# Bind("OcorrenciaID") %>' Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNotaFiscalID_" runat="server" Text="Nota Fiscal Origem:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblNotaFiscalID" runat="server" Text='<%# Bind("NotaFiscalID") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblLojaID_" runat="server" Text="Loja:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblLojaID" runat="server" Text='<%# Bind("NomeFantasia") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNomeMotorista_" runat="server" Text="Motorista:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblNomeMotorista" runat="server" Text='<%# Bind("NomeMotorista") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPlacaCaminhao_" runat="server" Text="Placa Caminhão:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblPlacaCaminhao" runat="server" Text='<%# Bind("PlacaCaminhao") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMotivoOcorrencia_" runat="server" Text="Motivo:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblMotivoOcorrencia" runat="server" Text='<%# Bind("MotivoOcorrencia") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDataOcorrencia_" runat="server" Text="Data Ocorrência:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblDataOcorrencia" runat="server" Text='<%# Bind("DataOcorrencia","{0:dd/MM/yyyy}") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text="Número Troca:"></asp:Label>
                                            </td>
                                            <td class="label_repeater">
                                                <asp:Label ID="lblNumeroTroca" runat="server" Text='<%# Bind("NumeroTroca") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblObservacao_" runat="server" Text="Observação" ToolTip='<%# Bind("Observacao") %>'></asp:Label>
                                                <asp:Label ID="lblStatusOcorrenciaID" runat="server" Text='<%# Bind("StatusOcorrenciaID") %>' Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <asp:GridView ID="gdvProduto" runat="server" SkinID="GridView">
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOcorrenciaID" runat="server" Text='<%# Bind("OcorrenciaID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ProdutoID" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPedidoID" runat="server" Text='<%# Bind("ProdutoID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Produto" ItemStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProdutoID" runat="server" Text='<%# Bind("Descricao") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Medida" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMedida" runat="server" Text='<%# Bind("Medida") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Quantidade" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQuantidade" runat="server" Text='<%# Bind("Quantidade") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
    <!-- COMANDA DE OCORRÊNCIA -->
    <cc1:ModalPopupExtender ID="mpeOcorrencia" runat="server" CancelControlID="imbFechar"
        TargetControlID="hdfGerar" PopupControlID="pnlOcorrencia" BackgroundCssClass="background_modal" DropShadow="false">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlOcorrencia" runat="server" CssClass="window_modal" Width="735px" Height="430px" Style="display: none;">
        <rsweb:ReportViewer ID="rpvOcorrencia" runat="server" Width="735px" Font-Names="Verdana"
            Font-Size="8pt" Height="400px" ShowDocumentMapButton="False" ShowFindControls="False"
            ShowPageNavigationControls="False" ShowRefreshButton="False" ShowZoomControl="False"
            ShowExportControls="true" ShowBackButton="false">
        </rsweb:ReportViewer>
        <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" OnClick="imbFechar_Click" />
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
