using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using DAO;
using DAL;

namespace Site
{
    public partial class PedidoDetalhe : System.Web.UI.Page
    {
        decimal totalPago = 0;

        private void CarregarDados()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            DataSet ds = new PedidoDAL().ListarDetalhe(Session["PedidoDetalheID"].ToString(), usuarioSessao.SistemaID);
            this.gdvPedidoDetalhe.DataSource = ds;
            this.gdvPedidoDetalhe.DataBind();
            if (this.gdvPedidoDetalhe.Rows.Count <= 0)
            {
                throw new ApplicationException("Pedido / Reserva inexistente.");
            }
            this.lblPedidoID.Text = this.Session["PedidoDetalheID"].ToString();
            this.lblObservacao.Text = ds.Tables[0].Rows[0]["Observacao"].ToString();
        }

        protected void gdvPedidoDetalhe_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    this.totalPago += Convert.ToDecimal(e.Row.Cells[3].Text.Replace("R", "").Replace("$", ""));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    this.txtTotalPago.Text = string.Format("{0:c}", this.totalPago);
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message, ex);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!UtilitarioBLL.PermissaoUsuario(Session["Usuario"]) || (this.Session["PedidoDetalheID"] == null))
                {
                    UtilitarioBLL.Sair();
                    if (base.Request.Url.Segments.Length == 3)
                    {
                        base.Response.Redirect("../Default.aspx", false);
                    }
                    else
                    {
                        base.Response.Redirect("Default.aspx", false);
                    }
                }
                if (!base.IsPostBack)
                {
                    this.CarregarDados();
                    this.gdvPedidoDetalhe.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(this.Page, ex.Message, ex);
            }
        }
    }
}