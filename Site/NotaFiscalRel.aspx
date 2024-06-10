<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="NotaFiscalRel.aspx.cs"
    Inherits="Site.NotaFiscalRel" Title="RELATÓRIO - NOTA FISCAL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        $(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function BeginRequestHandler(sender, args) {
                console.log('BeginRequestHandler');
                getValoresProduto();
            }

            function EndRequestHandler(sender, args) {
                console.log('EndRequestHandler');
                setValoresProduto();
                getAutocomplete();
            }

            console.log('getAutocomplete');
            getAutocomplete();

        });

        function getValoresProduto() {
            var hdf = "";
            var tblProduto = document.getElementById("tblProduto");
            if (tblProduto != null && tblProduto.rows.length > 0) {
                for (var i = 1; i < tblProduto.rows.length; i++) {
                    hdf = hdf + i + ";"; // linha - 0
                    hdf = hdf + tblProduto.rows[i].cells[1].children[1].value + ";"; // produtoId
                    hdf = hdf + tblProduto.rows[i].cells[0].innerText + ";"; // descrição
                    hdf = hdf + "~~";
                }
                document.getElementById("ctl00_ContentPlaceHolder1_hdfProdutoValores").value = hdf.substr(0, hdf.length - 2);                
            }
            console.log('get', document.getElementById("ctl00_ContentPlaceHolder1_hdfProdutoValores").value);
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
                        row = row + "<td><img style='width: 25px; heigth: 25px' src='../img/excluir_produto.png' title='Excluir Produto' onclick='deleteRow(this)' /><input type='hidden' name='txtProdutoID_" + j + "' value='" + cell[1] + "' /></td>";
                        row = row + "</tr>";
                        $("#tblProdutoBody").append(row);
                        j++;
                    }
                }
                document.getElementById("ctl00_ContentPlaceHolder1_hdfProdutoValores").value = "";
            }
            console.log('set', document.getElementById("ctl00_ContentPlaceHolder1_hdfProdutoValores").value);
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

        function createRow(produtoId, descricao, index) {
            var row = "<tr>";
            row = row + "<td>" + descricao + "</td>";
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfProdutoValores" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="imbGerar" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="imbGerar">
                <div id="conteudo">
                    <div id="topo_cabeca">
                        RELATÓRIO - NOTA FISCAL
                    </div>
                    <div>
                        <table width="100%">
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblNotaFiscalID" runat="server" Text="NotaFiscalID" SkinID="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNotaFiscalID" runat="server" SkinID="TextBox" Width="100px" MaxLength="9"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="txtNotaFiscalID_FilteredTextBoxExtender" runat="server"
                                        TargetControlID="txtNotaFiscalID" FilterType="Numbers">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblDataNotaFiscal" runat="server" Text="Data Nota Fiscal" SkinID="Label"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtDataNotaFiscalDe" runat="server" Width="65px" Height="15px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="txtDataNotaFiscalDe_MaskedEditExtender" runat="server" TargetControlID="txtDataNotaFiscalDe"
                                        MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                    </cc1:MaskedEditExtender>
                                    &nbsp;&nbsp;a&nbsp;&nbsp;
                                    <asp:TextBox ID="txtDataNotaFiscalAte" runat="server" Width="65px" Height="15px" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="txtDataNotaFiscalAte_MaskedEditExtender" runat="server" TargetControlID="txtDataNotaFiscalAte"
                                        MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="false">
                                    </cc1:MaskedEditExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblNumeroCarga" runat="server" Text="Número Carga" SkinID="Label"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumeroCarga" runat="server" SkinID="TextBox" Width="155px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lblProduto" runat="server" Text="Produto" SkinID="Label"></asp:Label>
                                </td>
                                <td>
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
                                                <td style="width: 95%">Produto
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
                                <td colspan="2" style="text-align: center;">
                                    <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                        OnClick="imbConsultar_Click" />
                                    <asp:ImageButton ID="imbGerar" runat="server" ImageUrl="~/img/gerar.png"
                                        OnClick="imbGerar_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="corpo">
                        <asp:GridView ID="gdvNotaFiscal" runat="server" SkinID="GridView">
                            <Columns>
                                <asp:BoundField DataField="Data" HeaderText="Data" ItemStyle-Width="10%" />
                                <asp:BoundField DataField="Hora" HeaderText="Hora" ItemStyle-Width="10%" />
                                <asp:BoundField DataField="Codigo" HeaderText="Codigo" ItemStyle-Width="10%" />
                                <asp:BoundField DataField="Descricao" HeaderText="Descrição" ItemStyle-Width="40%" />
                                <asp:BoundField DataField="QuantidadeRecebida" HeaderText="Qtd Rec." ItemStyle-Width="7.5%" />
                                <asp:BoundField DataField="NumeroCarga" HeaderText="Carga" ItemStyle-Width="7.5%" />
                                <asp:BoundField DataField="NotaFiscal" HeaderText="Notas Fiscais" ItemStyle-Width="15%" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>