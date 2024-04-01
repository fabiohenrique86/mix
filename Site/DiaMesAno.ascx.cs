using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Site
{
    public partial class DiaMesAno : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (base.IsPostBack)
                {
                    this.ValidarData();
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagem(this.Page, "Data inválida");
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagem(this.Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message, ex);
            }
        }

        private void ValidarData()
        {
            if ((((this.ddlDiaDe.SelectedIndex <= 0) || (this.ddlMesDe.SelectedIndex <= 0)) || ((this.ddlAnoDe.SelectedIndex <= 0) || (this.ddlDiaPara.SelectedIndex <= 0))) || ((this.ddlMesPara.SelectedIndex <= 0) || (this.ddlAnoPara.SelectedIndex <= 0)))
            {
                throw new ApplicationException("É necessário informar todas as datas para visualizar o relatório.");
            }
            DateTime dtInicial = Convert.ToDateTime(string.Concat(new object[] { this.ddlDiaDe.SelectedValue, '/', this.ddlMesDe.SelectedValue, '/', this.ddlAnoDe.SelectedValue }));
            DateTime dtFinal = Convert.ToDateTime(string.Concat(new object[] { this.ddlDiaPara.SelectedValue, '/', this.ddlMesPara.SelectedValue, '/', this.ddlAnoPara.SelectedValue }));
            if (dtInicial > dtFinal)
            {
                throw new ApplicationException("A data inicial deve ser menor ou igual que a data final para visualizar o relatório.");
            }
            base.Session["DiaMesAnoInicial"] = dtInicial.ToShortDateString();
            base.Session["DiaMesAnoFinal"] = dtFinal.ToShortDateString();
        }
    }
}