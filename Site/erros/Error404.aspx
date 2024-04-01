<%@ Page Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="Erro404.aspx.cs"
    Inherits="Site.Erro404" Title="MiX - Página Não Encontrada" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="conteudo">
        <div id="topo_cabeca">
            Erro 404
        </div>
        <div style="text-align: center; font-size: 12px;">
            <p>
                Página Inexistente!
            </p>
            <p>
                Verifique se foi a página foi digitada corretamente na barra de endereço.
            </p>
        </div>
    </div>
</asp:Content>
