using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BLL
{
    public static class Utilitario
    {
        // Fields
        public static string A_PROGRAMAR = "A PROGRAMAR";
        public static string ATRIBUTO_BORDER_COLOR = "bordercolor";
        public static string BORDER_COLOR = "black";

        // Methods
        public static void CarregarData(string mes, string ano, DropDownList ddlMes, DropDownList ddlAno)
        {
            ddlMes.SelectedValue = mes;
            ddlAno.SelectedValue = ano;
        }

        public static void CarregarData(string dataInicial, string dataFinal, DropDownList ddlMesDe, DropDownList ddlAnoDe, DropDownList ddlMesPara, DropDownList ddlAnoPara)
        {
            ddlMesDe.SelectedValue = dataInicial.Substring(3, 2);
            ddlAnoDe.SelectedValue = dataInicial.Substring(6, 4);
            ddlMesPara.SelectedValue = dataFinal.Substring(3, 2);
            ddlAnoPara.SelectedValue = dataFinal.Substring(6, 4);
        }

        public static void CarregarData(string dataInicial, string dataFinal, DropDownList ddlDiaDe, DropDownList ddlMesDe, DropDownList ddlAnoDe, DropDownList ddlDiaPara, DropDownList ddlMesPara, DropDownList ddlAnoPara)
        {
            ddlDiaDe.SelectedValue = dataInicial.Substring(0, 2);
            ddlMesDe.SelectedValue = dataInicial.Substring(3, 2);
            ddlAnoDe.SelectedValue = dataInicial.Substring(6, 4);
            ddlDiaPara.SelectedValue = dataFinal.Substring(0, 2);
            ddlMesPara.SelectedValue = dataFinal.Substring(3, 2);
            ddlAnoPara.SelectedValue = dataFinal.Substring(6, 4);
        }

        public static bool ValidarData(string data)
        {
            bool retorno = true;
            int dia = Convert.ToInt32(data.Substring(0, 2));
            int mes = Convert.ToInt32(data.Substring(2, 2));
            int ano = Convert.ToInt32(data.Substring(4, 4));

            if (dia == 0 || dia >= 32)
            {
                retorno = false;
            }
            else if (mes == 0 || mes >= 13)
            {
                retorno = false;
            }
            else if (ano == 0 || ano <= 1900)
            {
                retorno = false;
            }

            return retorno;
        }

        public static void ExibirMensagem(Page pagina, string mensagem)
        {
            string script = string.Format("jAlert({0},'Alerta');", JavascriptEncode(mensagem));
            RegistrarScript(pagina, "alerta", script);
        }

        public static void ExibirMensagemAjax(Page pagina, string mensagem, Exception ex = null)
        {
            if ((ex != null) && !(ex is System.Threading.ThreadAbortException))
            {
                try
                {
                    string message = string.Empty;

                    message += "Message: <br /><br />" + ex.Message;
                    message += "<br /><br /> Source: <br /><br />" + ex.Source;
                    message += "<br /><br /> HelpLink: <br /><br />" + ex.HelpLink;
                    message += "<br /><br /> StackTrace: <br /><br />" + ex.StackTrace;
                    message += "<br /><br /> Exception: <br /><br />" + ex.ToString();

                    if (ex.Data != null && ex.Data.Count > 0)
                    {
                        message += "<br /><br />Keys: <br /><br />";

                        foreach (var key in ex.Data.Keys)
                        {
                            if (key == null)
                            {
                                message += "NULL" + "<br /><br />";
                            }
                            else
                            {
                                message += key.ToString() + "<br /><br />";
                            }
                        }

                        message += "<br /><br />Values: <br /><br />";

                        foreach (var value in ex.Data.Values)
                        {
                            if (value == null)
                            {
                                message += "NULL" + "<br /><br />";
                            }
                            else
                            {
                                message += value.ToString() + "<br /><br />";
                            }
                        }
                    }

                    Email.Enviar("MiX", "contato@sistemamix.com.br", message, "Erro Crítico");
                }
                catch (Exception)
                {
                    // se o envio de e-mail falhar, segue o fluxo.
                }
            }

            if ((ex == null) || ((ex != null) && !(ex is System.Threading.ThreadAbortException)))
            {
                string script = string.Format("jAlert({0},'Alerta');", JavascriptEncode(mensagem));
                RegistrarScriptAjax(pagina, "alerta", script);
            }
        }

        public static string FormatarTotalPago(string total)
        {
            string nova = "";
            if (((total.Length == 4) || (total.Length == 5)) || (total.Length == 6))
            {
                return total.Replace(".", ",");
            }
            if (total.Length == 8)
            {
                total.Remove(5, 1);
                total.Insert(5, ",");
                return total;
            }
            if (total.Length == 9)
            {
                total.Remove(6, 1);
                total.Insert(6, ",");
                return total;
            }
            if (total.Length == 10)
            {
                total.Remove(7, 1);
                total.Insert(7, ",");
                nova = total;
            }
            return nova;
        }

        private static string JavascriptEncode(string script)
        {
            StringBuilder retorno = new StringBuilder();
            retorno.Append("\"");
            foreach (char caracter in script)
            {
                switch (caracter)
                {
                    case '\b':
                        {
                            retorno.Append(@"\b");
                            continue;
                        }
                    case '\t':
                        {
                            retorno.Append(@"\t");
                            continue;
                        }
                    case '\n':
                        {
                            retorno.Append(@"\n");
                            continue;
                        }
                    case '\f':
                        {
                            retorno.Append(@"\f");
                            continue;
                        }
                    case '\r':
                        {
                            retorno.Append(@"\r");
                            continue;
                        }
                    case '"':
                        {
                            retorno.Append("\\\"");
                            continue;
                        }
                    case '\\':
                        {
                            retorno.Append(@"\\");
                            continue;
                        }
                }
                int i = caracter;
                if ((i < 0x20) || (i > 0x7f))
                {
                    retorno.AppendFormat(@"\u{0:X04}", i);
                }
                else
                {
                    retorno.Append(caracter);
                }
            }
            retorno.Append("\"");
            return retorno.ToString();
        }

        public static bool PermissaoUsuario(object sessao)
        {
            if (sessao == null)
            {
                return false;
            }
            if (((BLL.Modelo.Usuario)sessao).StatusSistemaID == StatusSistema.Inativo.GetHashCode())
            {
                return false;
            }
            return true;
        }

        public static void RegistrarScript(Page pagina, string identificador, string script)
        {
            pagina.ClientScript.RegisterStartupScript(pagina.GetType(), identificador, script, true);
        }

        public static void RegistrarScriptAjax(Page pagina, string identificador, string script)
        {
            ScriptManager.RegisterStartupScript(pagina, pagina.GetType(), identificador, script, true);
        }

        public static void Sair()
        {
            Aplicacao.Empresa = null;
            HttpContext.Current.Session["Usuario"] = null;
        }

        public static void SetarMascaraValor(Page pagina)
        {
            RegistrarScriptAjax(pagina.Page, "jsmoeda", "$().ready(function() { $('.decimal').priceFormat({ prefix: '', centsSeparator: ',', thousandsSeparator: '.' }); });");
        }

        // Nested Types
        public enum StatusEntregaReserva
        {
            Efetuada = 2,
            Pendente = 1,
            Transito = 5
        }

        public enum StatusSistema
        {
            Ativo = 3,
            Inativo = 4
        }

        public enum TipoCliente
        {
            PessoaFisica = 1,
            PessoaJuridica = 2
        }

        public enum TipoUsuario
        {
            Administrador = 1,
            Estoquista = 2,
            Gerente = 3,
            Vendedor = 4,
            Conferentista = 5
        }
    }
}
