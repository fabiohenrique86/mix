<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Cliente.aspx.cs"
    Inherits="Site.Cliente" Title="CADASTRO | CONSULTA | ATUALIZAÇÃO | EXCLUSÃO - CLIENTE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1 {
            width: 120px;
        }

        div.tooltip a {
            text-decoration: none;
        }

            div.tooltip a:hover {
                text-decoration: none;
            }

        div.tooltip .toolbox a:hover span.legenda {
            display: block !important;
            background: #003366;
            border: #ffd700 2px solid;
            color: #fff;
            text-decoration: none;
            position: relative;
            border-bottom-left-radius: 5px;
            border-bottom-right-radius: 5px;
            border-top-left-radius: 5px;
            border-top-right-radius: 5px;
            text-align: left;
            padding-left: 1px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="conteudo">
                <div id="topo_cabeca">
                    <asp:Label ID="lblTopo" runat="server" Text=""></asp:Label>
                </div>
                <div id="cabeca">
                    <table style="width: 100%;">
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblClienteID" runat="server" Text="ClienteID" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;" colspan="7">
                                <asp:TextBox ID="txtClienteID" runat="server" Width="60px" MaxLength="10" CssClass="desabilitado"
                                    Enabled="false" SkinID="TextBox"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtClienteID_FilteredTextBoxExtender" runat="server"
                                    FilterType="Numbers" TargetControlID="txtClienteID">
                                </cc1:FilteredTextBoxExtender>
                                <asp:CheckBox ID="ckbClienteID" AutoPostBack="true" Text="Atualizar/Excluir" runat="server"
                                    OnCheckedChanged="ckbClienteID_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTipoCliente" runat="server" Text="Tipo" SkinID="Label"></asp:Label>
                            </td>
                            <td colspan="7">
                                <asp:RadioButtonList ID="rblTipoCliente" runat="server" RepeatDirection="Horizontal"
                                    Width="300px" AutoPostBack="true" OnSelectedIndexChanged="rblTipoCliente_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="Pessoa Física" Value="1" />
                                    <asp:ListItem Selected="False" Text="Pessoa Jurídica" Value="2" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblNome_" runat="server" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;" colspan="7">
                                <asp:TextBox ID="txtNome" runat="server" Width="300px" MaxLength="200" SkinID="TextBox"></asp:TextBox>
                                <asp:TextBox ID="txtNomeFantasia" runat="server" Width="300px" MaxLength="150" SkinID="TextBox"></asp:TextBox>
                                &nbsp;&nbsp;
                                <asp:Label ID="lblRazaoSocial_" runat="server" SkinID="Label" Text="Razão Social"></asp:Label>
                                &nbsp;
                                <asp:TextBox ID="txtRazaoSocial" runat="server" Width="300px" MaxLength="150" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblEmail_" runat="server" Text="E-mail" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;" colspan="7">
                                <asp:TextBox ID="txtEmail" runat="server" Width="400px" MaxLength="300" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblCpfCnpj_" runat="server" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;" colspan="3">
                                <asp:TextBox ID="txtCnpj" runat="server" MaxLength="14" Width="110px" SkinID="TextBox"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtCnpj_MaskedEditExtender" runat="server" TargetControlID="txtCnpj"
                                    ClearMaskOnLostFocus="False" Mask="99,999,999/9999-99" />
                                <asp:TextBox ID="txtCpf" runat="server" Width="85px" SkinID="TextBox"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="meeCpf" runat="server" TargetControlID="txtCpf" ClearMaskOnLostFocus="False"
                                    Mask="999,999,999-99" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblDataNascimento_" runat="server" Text="Data Nascimento" SkinID="Label"></asp:Label>
                                &nbsp;
                                <asp:TextBox ID="txtDataNascimento" runat="server" Width="65px" MaxLength="10" SkinID="TextBox" CssClass="datepicker"></asp:TextBox>
                                <cc1:MaskedEditExtender ID="txtDataNascimento_MaskedEditExtender" runat="server"
                                    MaskType="Date" TargetControlID="txtDataNascimento" Mask="99/99/9999" ClearMaskOnLostFocus="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblCep" runat="server" Text="CEP" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;" colspan="7">
                                <asp:TextBox ID="txtCEP" runat="server" Width="100px" MaxLength="300" SkinID="TextBox"></asp:TextBox>
                                <asp:Button ID="btnConsultar" runat="server" Text="Consultar" OnClick="btnConsultar_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblEndereco" runat="server" Text="Endereço" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;" colspan="7">
                                <asp:TextBox ID="txtEndereco" runat="server" Width="650px" MaxLength="300" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblBairro" runat="server" Text="Bairro" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 220px;">
                                <asp:TextBox ID="txtBairro" runat="server" Width="180px" MaxLength="50" SkinID="TextBox"></asp:TextBox>
                            </td>
                            <td style="width: 70px;">
                                <asp:Label ID="lblCidade" runat="server" Text="Cidade" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left; width: 170px;">
                                <asp:TextBox ID="txtCidade" runat="server" Width="150px" MaxLength="50" SkinID="TextBox"></asp:TextBox>
                            </td>
                            <td style="width: 70px;">
                                <asp:Label ID="lblEstado" runat="server" Text="Estado" SkinID="Label"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEstado" runat="server" Width="100px" MaxLength="50" SkinID="TextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Label ID="lblPontoReferencia" runat="server" Text="Ponto Referência" SkinID="Label"></asp:Label>
                            </td>
                            <td style="text-align: left;" colspan="7">
                                <asp:TextBox ID="txtPontoReferencia" runat="server" Width="650px" MaxLength="200"
                                    SkinID="TextBox"></asp:TextBox><span class="campo_opcional">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <table width="100%">
                                    <tr>
                                        <td style="text-align: left; width: 100px;">
                                            <asp:Label ID="lblTelefoneResidencial" runat="server" SkinID="Label" Text="Telefone Residencial"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 80px;">
                                            <asp:TextBox ID="txtTelefoneResidencial" runat="server" MaxLength="10" SkinID="TextBox"
                                                Width="80px"></asp:TextBox>
                                            <span class="campo_opcional">*</span>
                                        </td>
                                        <td style="text-align: left; width: 100px;">
                                            <asp:Label ID="lblTelefoneResidencial2" runat="server" SkinID="Label" Text="Telefone Residencial"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 80px;">
                                            <asp:TextBox ID="txtTelefoneResidencial2" runat="server" MaxLength="10" SkinID="TextBox"
                                                Width="80px"></asp:TextBox>
                                            <span class="campo_opcional">*</span>
                                        </td>
                                        <td style="text-align: left; width: 80px;">
                                            <asp:Label ID="lblTelefoneCelular" runat="server" SkinID="Label" Text="Telefone Celular"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 90px;">
                                            <asp:TextBox ID="txtTelefoneCelular" runat="server" MaxLength="11" SkinID="TextBox"
                                                Width="90px"></asp:TextBox>
                                        </td>
                                        <td style="text-align: left; width: 80px;">
                                            <asp:Label ID="lblTelefoneCelular2" runat="server" SkinID="Label" Text="Telefone Celular"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 90px;">
                                            <asp:TextBox ID="txtTelefoneCelular2" runat="server" MaxLength="11" SkinID="TextBox"
                                                Width="90px"></asp:TextBox>
                                            <span class="campo_opcional">*</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8" class="campo_opcional">* campo opcional
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <cc1:MaskedEditExtender ID="txtTelefoneResidencial_MaskedEditExtender" runat="server"
                                    TargetControlID="txtTelefoneResidencial" Mask="(99)9999-9999" MaskType="Number"
                                    ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditExtender ID="txtTelefoneResidencial2_MaskedEditExtender" runat="server"
                                    TargetControlID="txtTelefoneResidencial2" Mask="(99)9999-9999" MaskType="Number"
                                    ClearMaskOnLostFocus="false">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditExtender ID="txtTelefoneCelular_MaskedEditExtender" runat="server"
                                    TargetControlID="txtTelefoneCelular" Mask="(99)99999-9999" MaskType="Number" ClearMaskOnLostFocus="false" AutoComplete="false">
                                </cc1:MaskedEditExtender>
                                <cc1:MaskedEditExtender ID="txtTelefoneCelular2_MaskedEditExtender" runat="server"
                                    TargetControlID="txtTelefoneCelular2" Mask="(99)99999-9999" MaskType="Number"
                                    ClearMaskOnLostFocus="false" AutoComplete="false">
                                </cc1:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr style="background-color: #FFD700; font-weight: bold; color: #003366; text-align: center;">
                            <td colspan="8">FILTRO
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFuncionario" runat="server" Text="Funcionário" SkinID="Label"></asp:Label>
                            </td>
                            <td colspan="7">
                                <asp:DropDownList ID="ddlFuncionario" runat="server" DataTextField="Nome" DataValueField="FuncionarioID"
                                    SkinID="DropDownList">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Mês do Nascimento" SkinID="Label"></asp:Label>
                            </td>
                            <td colspan="7">
                                <asp:DropDownList ID="ddlMes" runat="server" SkinID="DropDownListRelatorio">
                                    <asp:ListItem Value="0">SELECIONE</asp:ListItem>
                                    <asp:ListItem Value="01">Janeiro</asp:ListItem>
                                    <asp:ListItem Value="02">Fevereiro</asp:ListItem>
                                    <asp:ListItem Value="03">Março</asp:ListItem>
                                    <asp:ListItem Value="04">Abril</asp:ListItem>
                                    <asp:ListItem Value="05">Maio</asp:ListItem>
                                    <asp:ListItem Value="06">Junho</asp:ListItem>
                                    <asp:ListItem Value="07">Julho</asp:ListItem>
                                    <asp:ListItem Value="08">Agosto</asp:ListItem>
                                    <asp:ListItem Value="09">Setembro</asp:ListItem>
                                    <asp:ListItem Value="10">Outubro</asp:ListItem>
                                    <asp:ListItem Value="11">Novembro</asp:ListItem>
                                    <asp:ListItem Value="12">Dezembro</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:ImageButton ID="imbConsultar" runat="server" ImageUrl="~/img/consultar.png" OnClick="imbConsultar_Click" />
                                <asp:ImageButton ID="imbCadastrar" runat="server" ImageUrl="~/img/cadastrar.png" OnClick="imbCadastrar_Click" />
                                <asp:ImageButton ID="imbAtualizar" runat="server" ImageUrl="~/img/atualizar.png" Visible="false" OnClick="imbAtualizar_Click" />
                                <asp:ImageButton ID="imbExcluir" runat="server" ImageUrl="~/img/excluir.png" Visible="false" OnClick="imbExcluir_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="font-size: 10px; overflow-x: hidden; overflow-y: scroll; height: 220px;">
                    <asp:GridView ID="gdvCliente" runat="server" SkinID="GridViewFooter" OnRowDataBound="gdvCliente_RowDataBound"
                        OnPageIndexChanging="gdvCliente_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="ClienteID" HeaderText="ClienteID" ItemStyle-Width="5px"
                                HeaderStyle-Width="5px" />
                            <asp:TemplateField ItemStyle-Width="180px" HeaderStyle-Width="280px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblNomeHeader" runat="server" Text="Nome/NomeFantasia" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="tooltip">
                                        <p class="toolbox">
                                            <a href="#">
                                                <asp:Label ID="lblNome" runat="server" Text='<%# bind("Nome") %>' />
                                                <asp:Label ID="lblNomeFantasia" runat="server" Text='<%# bind("NomeFantasia") %>' />
                                                <span style="display: none; left: 0px; top: 10px; width: 270px;" class="legenda"><span
                                                    style="font-weight: bold">Endereço:</span>&nbsp;<asp:Label ID="Label2" runat="server"
                                                        Text='<%# bind("Endereco")%>' />
                                                    <br />
                                                    <span style="font-weight: bold">Bairro:</span>&nbsp;<asp:Label ID="Label3" runat="server"
                                                        Text='<%# bind("Bairro")%>' />
                                                    <br />
                                                    <span style="font-weight: bold">Cidade:</span>&nbsp;<asp:Label ID="Label4" runat="server"
                                                        Text='<%# bind("Cidade")%>' />
                                                    <span style="font-weight: bold">Estado:</span>&nbsp;<asp:Label ID="Label5" runat="server"
                                                        Text='<%# bind("Estado")%>' />
                                                    <br />
                                                    <span style="font-weight: bold">P.Referência:</span>&nbsp;<asp:Label ID="Label6"
                                                        runat="server" Text='<%# bind("PontoReferencia")%>' />
                                                    <br />
                                                    <span style="font-weight: bold">Cep:</span>&nbsp;<asp:Label ID="Label7" runat="server"
                                                        Text='<%# bind("Cep")%>' />
                                                    <br />
                                                </span></a>
                                        </p>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="90px" HeaderStyle-Width="90px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblCpfCpnjHeader" runat="server" Text="CPF/CNPJ" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCpf" runat="server" Text='<%# bind("Cpf") %>' />
                                    <asp:Label ID="lblCnpj" runat="server" Text='<%# bind("Cnpj") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="E-mail" HeaderStyle-Width="165px" ItemStyle-Width="65px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmail" runat="server" Text='<%# bind("Email") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="15px" HeaderStyle-Width="15px">
                                <HeaderTemplate>
                                    <asp:Label ID="lblDataNascimentoRazaoSocialHeader" runat="server" Text="Nascimento/R.Social" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDataNascimento" runat="server" Text='<%# bind("DataNascimento","{0:d}") %>' />
                                    <asp:Label ID="lblRazaoSocial" runat="server" Text='<%# bind("RazaoSocial") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Residencial" HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTelefoneResidencial" runat="server" Text='<%# bind("TelefoneResidencial") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Residencial2" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTelefoneResidencial2" runat="server" Text='<%# bind("TelefoneResidencial2") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Celular" HeaderStyle-Width="65px" ItemStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTelefoneCelular" runat="server" Text='<%# bind("TelefoneCelular") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Celular2" HeaderStyle-Width="65px" ItemStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTelefoneCelular2" runat="server" Text='<%# bind("TelefoneCelular2") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
