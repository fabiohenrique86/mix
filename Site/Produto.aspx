<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Produto.aspx.cs"
    Inherits="Site.Produto" Title="CADASTRO | CONSULTA | ATUALIZAÇÃO | EXCLUSÃO - PRODUTO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style2 {
            width: 145px;
        }

        .acd-header {
            font-family: Segoe UI, Verdana, Microsoft Sans Serif;
            font-size: 9px;
            color: Black;
            background-color: #FFD700;
            height: 30px;
            cursor: pointer;
            text-align: center;
            padding-top: 10px;
            margin: 0;
            border-bottom: 1px solid gray;
            font-weight: bold;
        }

        .acd-content {
            background-color: white;
        }

        .produto-id {
            cursor: pointer;
            color: #6495ed;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="imbConsultar" />
            <asp:PostBackTrigger ControlID="imbCadastrar" />
            <asp:PostBackTrigger ControlID="imbAtualizar" />
            <asp:PostBackTrigger ControlID="imbExcluir" />
            <asp:PostBackTrigger ControlID="imbZerar" />
            <asp:PostBackTrigger ControlID="imbImportarColeta" />
            <asp:PostBackTrigger ControlID="imbImportarBancoDados" />
            <asp:PostBackTrigger ControlID="imbImportarEstoque" />
            <asp:PostBackTrigger ControlID="btnRelatorioEstoque" />
        </Triggers>
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    <asp:Label ID="lblTopo" runat="server" Text=""></asp:Label>
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblProdutoID" runat="server" Text="ProdutoID" SkinID="Label"></asp:Label>
                            </td>
                            <td colspan="9" style="text-align: left;">
                                <asp:TextBox ID="txtProdutoID" runat="server" Width="85px" CssClass="desabilitado"
                                    Enabled="false" SkinID="TextBox"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtProdutoID_MaskedEditExtender" runat="server" ClearMaskOnLostFocus="False"
                                    Mask="999999,9999" TargetControlID="txtProdutoID">
                                </cc1:MaskedEditExtender>
                                <asp:CheckBox ID="ckbProdutoID" runat="server" AutoPostBack="true" Text="Atualizar/Excluir/Cadastrar"
                                    OnCheckedChanged="ckbProdutoID_CheckedChanged" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10" style="text-align: left;">
                                <asp:GridView ID="gdvLoja" runat="server" SkinID="GridView" Width="200px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Selecione" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckbLoja" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLojaID" runat="server" Text='<%# Bind("LojaID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Loja">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNomeFantasia" runat="server" Text='<%# Bind("NomeFantasia") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quantidade" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQuantidade" runat="server" Text="" Width="30px" MaxLength="4"
                                                    Height="14px" SkinID="TextBox">
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblLinha" runat="server" Text="Linha" SkinID="Label"></asp:Label>
                            </td>
                            <td colspan="9" style="text-align: left;">
                                <asp:DropDownList ID="ddlLinha" runat="server" DataTextField="Descricao" DataValueField="LinhaID"
                                    SkinID="DropDownList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <asp:Label ID="lblComissaoFuncionario" runat="server" Text="Comissão Funcionário (%)"
                                    SkinID="Label"></asp:Label>
                                &nbsp;&nbsp;
                                <asp:TextBox ID="txtComissaoFuncionario" runat="server" Width="30px" MaxLength="2"
                                    SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtComissaoFuncionario_FilteredTextBoxExtender"
                                    runat="server" FilterType="Numbers" TargetControlID="txtComissaoFuncionario">
                                </cc1:FilteredTextBoxExtender>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblComissaoFranqueado" runat="server" Text="Comissão Franqueado (%)"
                                    SkinID="Label"></asp:Label>&nbsp;&nbsp;
                                <asp:TextBox ID="txtComissaoFranqueado" runat="server" Width="30px" MaxLength="2"
                                    SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtComissaoFranqueado_FilteredTextBoxExtender" runat="server"
                                    FilterType="Numbers" TargetControlID="txtComissaoFranqueado" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                <asp:Label ID="lblDescricao" runat="server" Text="Descrição" SkinID="Label"></asp:Label>
                            </td>
                            <td colspan="9" style="text-align: left;">
                                <asp:TextBox ID="txtDescricao" runat="server" Width="415px" MaxLength="300" SkinID="TextBox"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblMedida" runat="server" Text="Medida"></asp:Label>&nbsp;&nbsp;&nbsp;<asp:DropDownList
                                    ID="ddlMedida" runat="server" DataTextField="Descricao" DataValueField="MedidaID"
                                    SkinID="DropDownListRelatorio">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Arquivo COLETA
                            </td>
                            <td style="text-align: left; width: 180px">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                            </td>
                            <td colspan="7" style="text-align: left; width: 180px">
                                <asp:ImageButton ID="imbImportarColeta" runat="server" ImageUrl="~/img/importar.png"
                                    OnClick="imbImportarColeta_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Arquivo BANCO DE DADOS
                            </td>
                            <td style="text-align: left; width: 180px">
                                <asp:FileUpload ID="FileUpload2" runat="server" />
                            </td>
                            <td colspan="7" style="text-align: left; width: 180px">
                                <asp:ImageButton ID="imbImportarBancoDados" runat="server" ImageUrl="~/img/importar.png"
                                    OnClick="imbImportarBancoDados_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">Arquivo de ESTOQUE (Depósito):</td>
                            <td style="text-align: left; width: 180px">
                                <asp:FileUpload ID="fupImportarEstoque" runat="server" />
                            </td>
                            <td colspan="7" style="text-align: left; width: 180px">
                                <asp:ImageButton ID="imbImportarEstoque" runat="server" ImageUrl="~/img/importar.png" OnClick="imbImportarEstoque_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2"></td>
                            <td>
                                <asp:CheckBox ID="ckbFlgExibirForaDeLinha" runat="server" Text="Exibir FORA DE LINHA" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png"
                                    OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png"
                                    Visible="false" OnClick="imbCadastrar_Click" />
                                <asp:ImageButton ID="imbAtualizar" runat="server" ImageUrl="~/img/atualizar.png"
                                    Visible="false" OnClick="imbAtualizar_Click" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" Visible="false"
                                    OnClick="imbExcluir_Click" />
                                <asp:ImageButton ID="imbZerar" runat="server" ImageUrl="~/img/zerar_estoque.png" Visible="false"
                                    OnClick="imbZerar_Click" />
                                <asp:Button ID="btnAtualizarEstoque" runat="server" Visible="false" OnClick="btnAtualizarEstoque_Click" Text="Atualização de Estoque Por Carga" />
                                <asp:Button ID="btnRelatorioEstoque" runat="server" Visible="false" OnClick="btnRelatorioEstoque_Click" Text="Relatório Estoque" />
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="corpo">
                    <cc1:Accordion ID="acdProduto" runat="server" SuppressHeaderPostbacks="true"
                        HeaderCssClass="acd-header" ContentCssClass="acd-content" SelectedIndex="-1"
                        FadeTransitions="true" FramesPerSecond="150" RequireOpenedPane="false" TransitionDuration="150"
                        OnItemDataBound="acdProduto_ItemDataBound">
                        <HeaderTemplate>
                            <asp:Label ID="lblLojaID" runat="server" Text='<%# Eval("LojaID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblNomeFantasia" runat="server" Text='<%# Eval("NomeFantasia") %>'></asp:Label>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div style="overflow-x: hidden; overflow-y: scroll; height: 250px;">
                                <asp:GridView ID="gdvProduto" runat="server" SkinID="GridView" OnRowDataBound="gdvProduto_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckbProduto" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ProdutoID" HeaderText="ProdutoID" ItemStyle-Width="9%" ItemStyle-CssClass="produto-id" />
                                        <asp:BoundField DataField="Linha" HeaderText="Linha" ItemStyle-Width="9%" />
                                        <asp:BoundField DataField="Descricao" HeaderText="Descrição" ItemStyle-Width="20%" />
                                        <asp:BoundField DataField="Medida" HeaderText="Medida" ItemStyle-Width="8%" />
                                        <asp:BoundField DataField="ComissaoFuncionario" HeaderText="% Func" ItemStyle-Width="6%" />
                                        <asp:BoundField DataField="ComissaoFranqueado" HeaderText="% Franq" ItemStyle-Width="6%" />
                                        <asp:BoundField DataField="Quantidade" HeaderText="Estoque" ItemStyle-Width="8%" />
                                        <asp:BoundField DataField="QuantidadeReservada" HeaderText="Reserva" ItemStyle-Width="8%" />
                                        <asp:BoundField DataField="QuantidadeDanificada" HeaderText="Danificada" ItemStyle-Width="8%" />
                                        <asp:BoundField DataField="QuantidadeTroca" HeaderText="Troca" ItemStyle-Width="8%" />
                                        <asp:BoundField DataField="QuantidadeProgramada" HeaderText="A Programar" ItemStyle-Width="9%" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </cc1:Accordion>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upAtualizarEstoque" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnVerificarCarga" />
            <asp:AsyncPostBackTrigger ControlID="btnAtualizarEstoque" />
            <asp:PostBackTrigger ControlID="btnImportarCarga" />
        </Triggers>
        <ContentTemplate>
            <cc1:ModalPopupExtender ID="mpeAtualizarEstoque" runat="server" CancelControlID="imbFechar" TargetControlID="hdfGerar" PopupControlID="pnlAtualizarEstoque" BackgroundCssClass="background_modal" DropShadow="false">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlAtualizarEstoque" runat="server" CssClass="window_modal" Width="735px" Height="430px" Style="display: none; overflow-x: hidden; overflow-y: scroll;">
                <div style="text-align: center">
                    <table style="width: 100%">
                         <tr>
                            <td colspan="2">
                                <h3>Atualização de Estoque Por Carga</h3>
                            </td>
                        </tr>
                        <tr>
                            <td>Número de Carga</td>
                            <td>
                                <asp:TextBox ID="txtNumeroCarga" runat="server" SkinID="TextBox" Width="300px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Arquivo XLSX</td>
                            <td>
                                <asp:FileUpload ID="fupImportarArquivoEstoque" runat="server" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnVerificarCarga" runat="server" Text="Verificar Carga" OnClick="btnVerificarCarga_Click" />&nbsp;&nbsp;
                                <asp:Button ID="btnImportarCarga" runat="server" Text="Importar Carga" OnClick="btnImportarCarga_Click" />                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label id="lblMensagemCarga" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="imbFechar" runat="server" ImageUrl="~/img/fechar.png" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:HiddenField ID="hdfGerar" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
