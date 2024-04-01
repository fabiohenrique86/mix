using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAO;

namespace Site
{
    public partial class MesAno : System.Web.UI.UserControl
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
            if ((this.ddlMesDe.SelectedIndex <= 0) || (this.ddlMesPara.SelectedIndex <= 0))
            {
                throw new ApplicationException("É necessário informar todos os meses para visualizar o relatório.");
            }
            DateTime dtInicial = Convert.ToDateTime(string.Concat(new object[] { "01", '/', this.ddlMesDe.SelectedValue, '/', this.ddlAnoDe.SelectedValue }));
            DateTime dtFinal = Convert.ToDateTime(string.Concat(new object[] { "01", '/', this.ddlMesPara.SelectedValue, '/', this.ddlAnoPara.SelectedValue }));
            if (dtInicial > dtFinal)
            {
                throw new ApplicationException("O mês inicial deve ser menor que o mês final para visualizar o relatório.");
            }
            if (dtFinal.Subtract(dtInicial).Days >= 90)
            {
                throw new ApplicationException("É necessário informar o trimestre para visualizar o relatório.");
            }
            base.Session["MesAnoInicial"] = this.ddlMesDe.SelectedValue + this.ddlAnoDe.SelectedValue;
            base.Session["MesAnoFinal"] = this.ddlMesPara.SelectedValue + this.ddlAnoPara.SelectedValue;
        }
    }
}