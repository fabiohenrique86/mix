<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="NotaFiscalAutomatica.aspx.cs"
    Inherits="Site.NotaFiscalAutomatica" Title="LANÇAMENTO - NOTA FISCAL AUTOMÁTICA" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2 {
            width: 150px;
        }

        .label_repeater {
            font-weight: normal;
            font-style: italic;
        }
    </style>
    <script type="text/javascript" language="javascript">
        
        '<%
        var lojas = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(((DataSet)Session["dsDropDownListLoja"]).Tables[0].AsEnumerable().Select(x => new DAO.LojaDAO { LojaID = x.Field<int>("LojaID"), Nome = x.Field<string>("NomeFantasia") }).ToList());
        %>'

        var options = "";
        $.each(<%=lojas%>, function (index, item) 
        {
            options += "<option value='" + item.LojaID + "'>" + item.Nome + "</option>";
        });

        function confirmarOcorrencia() {

            var produtosDAO = [];
            var mensagem = "";

            $(":input.nf").each(function (i, o) {

                var produtoDAO = {};

                produtoDAO.ProdutoID = $(o).attr("data-produto-id");
                produtoDAO.Quantidade = $(o).attr("data-quantidade");
                produtoDAO.NotaFiscalID = isNaN(parseInt($(o).val())) ? 0 : parseInt($(o).val());
                produtoDAO.LojaID = isNaN(parseInt($("#ddlLoja_" + produtoDAO.ProdutoID).val())) ? 0 : parseInt($("#ddlLoja_" + produtoDAO.ProdutoID).val());
                produtoDAO.CodigoSobMedida = $("#txtSobMedida_" + produtoDAO.ProdutoID).val();
                produtoDAO.SobMedida = parseInt($("#txtSobMedida_" + produtoDAO.ProdutoID).attr("data-sob-medida")); // indica se o produto é sob medida ou não
                
                if (produtoDAO.SobMedida == 1)
                {
                    if (produtoDAO.CodigoSobMedida == "")
                    {
                        mensagem = "Informe o Código do Produto Sob Medida do Produto " + produtoDAO.ProdutoID;
                        return false;
                    }
                }
                else
                {
                    if (isNaN(produtoDAO.NotaFiscalID) || produtoDAO.NotaFiscalID <= 0)
                    {
                        mensagem = "Informe o Nº da Nota Fiscal de Origem do produto " + produtoDAO.ProdutoID;
                        return false;
                    }

                    if (isNaN(produtoDAO.LojaID) || produtoDAO.LojaID <= 0)
                    {
                        mensagem = "Informe a Loja do produto " + produtoDAO.ProdutoID;
                        return false;
                    }
                }

                produtosDAO.push(produtoDAO);
            });

            if (mensagem != "")
            {
                alert(mensagem);
                return false;
            }

            $.ajax({
                url: '<%= Page.ResolveUrl("~/Ocorrencia.aspx/GerarOcorrencia") %>',
                data: "{ sistemaId: " + '<%= new BLL.Modelo.Usuario(Session["Usuario"]).SistemaID %>' + ", produtosDAO: " + JSON.stringify(produtosDAO) + ", notaFiscalId: " + $("#ctl00_ContentPlaceHolder1_txtNotaFiscalID").val() + ", arquivoColeta: " + JSON.stringify($("#ctl00_ContentPlaceHolder1_hdfArquivoColeta").val()) + "}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    
                    var j = JSON.parse(data.d);
                    
                    if (j.Sucesso)
                    {
                        $find("mpe").hide();
                        $("#ctl00_ContentPlaceHolder1_txtNotaFiscalID").val("");
                        $("#ctl00_ContentPlaceHolder1_hdfArquivoColeta").val("")
                    }

                    alert(j.Mensagem);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.info(XMLHttpRequest);
                    console.info(textStatus);
                    console.info(errorThrown);
                    alert("Ocorreu um erro ao gerar ocorrências. Tente novamente");
                }
            });

            return false;
        }

        function modalOcorrencia(produto) {
            
            var t = "";
            var m = "";
            $.each(produto, function (i, p) {
                m += "<li>" + p.Mensagem + "</li>";
                t += "<tr>";
                t += "<td style='text-align: center; width: 25px'>ProdutoID</td>";
                t += "<td style='text-align: center; width: 40px'>" + p.ProdutoID + "</td>";
                t += "<td style='text-align: center; width: 150px'><input id='txtNotaFiscalID_" + p.ProdutoID + "' data-produto-id='" + p.ProdutoID + "' data-quantidade='" + p.Quantidade + "' class='nf' type='text' placeholder='Nº Nota Fiscal Origem' /></td>";
                t += "<td style='text-align: center; width: 25px'>Loja</td>";
                t += "<td style='text-align: center; width: 100px'><select id='ddlLoja_" + p.ProdutoID + "'>" + options + "</select></td>";
                if (p.SobMedida)
                {
                    t += "<td style='text-align: center; width: 160px'><input id='txtSobMedida_" + p.ProdutoID  + "' placeholder='Código Produto Sob Medida' data-sob-medida='" + p.SobMedida + "' /></td>";
                }
                t += "</tr>";                
            });
            $("#ulProduto").append(m);
            $("#tbNotaFiscalProduto").append(t);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfArquivoColeta" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="imbImportarXML" />
            <asp:PostBackTrigger ControlID="imbImportarColeta" />
        </Triggers>
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    LANÇAMENTO - NOTA FISCAL AUTOMÁTICA
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style2">Importar Notas Fiscais DROPBOX
                            </td>
                            <td colspan="2">
                                <asp:ImageButton ID="imbImportarXML" runat="server" ImageUrl="~/img/importar.png" OnClick="imbImportarXML_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Importar Arquivo COLETA
                            </td>
                            <td class="style2">
                                <asp:TextBox ID="txtNotaFiscalID" runat="server" Width="400px" SkinID="TextBox" placeholder="Número das Notas Fiscais separadas por vírgulas"></asp:TextBox>
                            </td>
                            <td>
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </td>
                            <td>
                                <asp:ImageButton ID="imbImportarColeta" runat="server" ImageUrl="~/img/importar.png" OnClick="imbImportarColeta_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                    </table>
                </div>
                <div id="corpo">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <cc1:ModalPopupExtender ID="mpeNotaFiscal" BehaviorID="mpe" runat="server" TargetControlID="hdfGerar"
        CancelControlID="imbCancelar" PopupControlID="pnlNotaFiscal" BackgroundCssClass="background_modal"
        DropShadow="false" Enabled="true">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlNotaFiscal" runat="server" CssClass="window_modal" Width="740px" Height="430px" Style="display: none">
        <div style="text-align: center; padding: 10px; margin: 10px; overflow: auto; max-height: 400px; width: 100%;">
            <div>Foram encontrados as seguintes divergências:</div>
            <ul id="ulProduto"></ul>
            <table id="tbNotaFiscalProduto" width="97%" border="1"></table>
            <br />
            <div>Deseja prosseguir (Confirmar) ou informar um novo arquivo de coleta (Cancelar)?</div>
            <br />
            <br />
            <asp:ImageButton ID="imbConfirmar" runat="server" ImageUrl="~/img/confirmar.png" OnClientClick="return confirmarOcorrencia();" />
            <asp:ImageButton ID="imbCancelar" runat="server" ImageUrl="~/img/cancelar.png" />
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hdfGerar" runat="server" />
</asp:Content>
