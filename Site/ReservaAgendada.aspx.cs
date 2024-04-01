using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Drawing;

namespace Site
{
    public partial class ReservaAgendada : System.Web.UI.Page
    {
        private bool CarregarDados()
        {
            var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            gdvReservaAgendada.DataSource = new DAL.Reserva().ListarAgendada(null, usuarioSessao.SistemaID, false);
            gdvReservaAgendada.DataBind();

            return (gdvReservaAgendada.Rows.Count > 0);
        }

        private bool CarregarDados(string pedidoId, string dataReserva, string dataEntrega, bool dataEntregaAProgramar, string statusId)
        {
            var usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            gdvReservaAgendada.DataSource = new DAL.Reserva().ListarAgendada(pedidoId, dataReserva, dataEntrega, usuarioSessao.SistemaID, dataEntregaAProgramar, statusId, txtNomeCarreteiro.Text.Trim());
            gdvReservaAgendada.DataBind();

            return (gdvReservaAgendada.Rows.Count > 0);
        }

        protected void gdvReservaAgendada_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gdvReservaAgendada.PageIndex = e.NewPageIndex;

                if ((!string.IsNullOrEmpty(txtReservaID.Text) || !string.IsNullOrEmpty(txtNomeCarreteiro.Text) || (txtDataReserva.Text != "__/__/____")) || (((txtDataEntrega.Text != "__/__/____") || ckbDataEntrega.Checked) || (ddlStatus.SelectedIndex > 0)))
                {
                    if (ckbDataEntrega.Checked && (txtDataEntrega.Text != "__/__/____"))
                    {
                        throw new ApplicationException("Informar um dos campos : data da entrega ou sem data entrega.");
                    }

                    CarregarDados(txtReservaID.Text, txtDataReserva.Text, txtDataEntrega.Text, ckbDataEntrega.Checked, ddlStatus.SelectedValue);
                }
                else
                {
                    CarregarDados();
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
            }
        }

        protected void gdvReservaAgendada_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblDataEntrega = (Label)e.Row.FindControl("lblDataEntrega");
                    int statusId = Convert.ToInt32(((Label)e.Row.FindControl("lblStatusID")).Text);
                    if (statusId == UtilitarioBLL.StatusEntregaReserva.Efetuada.GetHashCode())
                    {
                        e.Row.ForeColor = Color.Green;
                    }
                    else if (statusId == UtilitarioBLL.StatusEntregaReserva.Transito.GetHashCode())
                    {
                        e.Row.ForeColor = Color.Orange;
                    }
                    else if (string.IsNullOrEmpty(lblDataEntrega.Text))
                    {
                        e.Row.ForeColor = Color.Black;
                        lblDataEntrega.Text = UtilitarioBLL.A_PROGRAMAR;
                    }
                    else if (Convert.ToDateTime(lblDataEntrega.Text).CompareTo(DateTime.Now.Date) == 0)
                    {
                        e.Row.ForeColor = Color.Blue;
                    }
                    else if (Convert.ToDateTime(lblDataEntrega.Text).CompareTo(DateTime.Now.Date) < 0)
                    {
                        e.Row.ForeColor = Color.Red;
                    }
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
            }
        }

        protected void imbConsultar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    BLL.Aplicacao.Empresa = null;

                    if (base.Request.Url.Segments.Length == 3)
                    {
                        base.Response.Redirect("../Default.aspx", true);
                    }
                    else
                    {
                        base.Response.Redirect("Default.aspx", true);
                    }
                }
                else
                {
                    if ((string.IsNullOrEmpty(txtReservaID.Text) && string.IsNullOrEmpty(txtNomeCarreteiro.Text) && !(txtDataReserva.Text != "__/__/____")) && ((!(txtDataEntrega.Text != "__/__/____") && !ckbDataEntrega.Checked) && (ddlStatus.SelectedIndex <= 0)))
                    {
                        throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                    }

                    if (ckbDataEntrega.Checked && (txtDataEntrega.Text != "__/__/____"))
                    {
                        throw new ApplicationException("Informar um dos campos : data da entrega ou sem data entrega para consultar.");
                    }

                    if (!CarregarDados(txtReservaID.Text, txtDataReserva.Text, txtDataEntrega.Text, ckbDataEntrega.Checked, ddlStatus.SelectedValue))
                    {
                        throw new ApplicationException("Reserva Agendada inexistente.");
                    }
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida");
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Source.ToUpper() == "SYSTEM.DATA")
                {
                    UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida.");
                }
                else
                {
                    UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
                }
            }
        }

        protected void imbDarBaixa_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    BLL.Aplicacao.Empresa = null;

                    if (base.Request.Url.Segments.Length == 3)
                    {
                        base.Response.Redirect("../Default.aspx", true);
                    }
                    else
                    {
                        base.Response.Redirect("Default.aspx", true);
                    }
                }
                else
                {
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                    DAL.Reserva reservaDAL = new DAL.Reserva();

                    if (string.IsNullOrEmpty(txtReservaID.Text.Trim()))
                    {
                        throw new ApplicationException("É necessário informar o PedidoID para dar baixa na reserva.");
                    }

                    if (string.IsNullOrEmpty(txtNomeCarreteiro.Text.Trim()))
                    {
                        throw new ApplicationException("É necessário informar o Carreteiro para dar baixa na reserva.");
                    }

                    if (!reservaDAL.Existe(txtReservaID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Reserva inexistente.");
                    }

                    if (Convert.ToInt32(reservaDAL.Listar(Convert.ToInt32(txtReservaID.Text), usuarioSessao.SistemaID).Tables[0].Rows[0]["StatusID"]) == UtilitarioBLL.StatusEntregaReserva.Efetuada.GetHashCode())
                    {
                        throw new ApplicationException("Não é possível dar baixa nessa Reserva, pois a baixa já foi efetuada.");
                    }

                    reservaDAL.Inserir(Convert.ToInt32(txtReservaID.Text), usuarioSessao.SistemaID, txtNomeCarreteiro.Text.Trim());

                    LimparFormulario(txtReservaID, txtDataReserva, txtDataEntrega);

                    CarregarDados();
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida");
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Source.ToUpper() == "SYSTEM.DATA")
                {
                    UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida.");
                }
                else
                {
                    UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
                }
            }
        }

        protected void imbProrrogar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    BLL.Aplicacao.Empresa = null;

                    if (base.Request.Url.Segments.Length == 3)
                    {
                        base.Response.Redirect("../Default.aspx", true);
                    }
                    else
                    {
                        base.Response.Redirect("Default.aspx", true);
                    }
                }
                else
                {
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                    DAL.Reserva reservaDAL = new DAL.Reserva();

                    if (string.IsNullOrEmpty(txtReservaID.Text))
                    {
                        throw new ApplicationException("É necessário informar o PedidoID e um dos campos : data da entrega ou sem data entrega para prorrogar a reserva.");
                    }

                    if (!ckbDataEntrega.Checked)
                    {
                        if (txtDataEntrega.Text == "__/__/____")
                        {
                            throw new ApplicationException("É necessário informar o PedidoID e um dos campos : data da entrega ou sem data entrega para prorrogar a reserva.");
                        }
                        if (Convert.ToDateTime(txtDataEntrega.Text) <= DateTime.Today)
                        {
                            throw new ApplicationException("A data da entrega deve ser maior do que a data de hoje.");
                        }
                    }
                    else if (txtDataEntrega.Text != "__/__/____")
                    {
                        throw new ApplicationException("Informar um dos campos : data da entrega ou sem data entrega.");
                    }

                    if (!reservaDAL.Existe(txtReservaID.Text, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Reserva inexistente.");
                    }

                    int statusId = Convert.ToInt32(reservaDAL.Listar(Convert.ToInt32(txtReservaID.Text), usuarioSessao.SistemaID).Tables[0].Rows[0]["StatusID"]);

                    if (statusId != UtilitarioBLL.StatusEntregaReserva.Pendente.GetHashCode())
                    {
                        if (statusId == UtilitarioBLL.StatusEntregaReserva.Efetuada.GetHashCode())
                        {
                            throw new ApplicationException("Não é possível prorrogar essa Reserva, pois a baixa já foi efetuada.");
                        }
                    }
                    else
                    {
                        reservaDAL.Atualizar(Convert.ToInt32(txtReservaID.Text), (txtDataEntrega.Text != "__/__/____") ? txtDataEntrega.Text : null, ckbDataEntrega.Checked);
                        LimparFormulario(txtReservaID, txtDataReserva, txtDataEntrega);
                        CarregarDados();
                    }
                }
            }
            catch (FormatException)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida");
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.Source.ToUpper() == "SYSTEM.DATA")
                {
                    UtilitarioBLL.ExibirMensagemAjax(Page, "Data inválida.");
                }
                else
                {
                    UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
                }
            }
        }

        protected void imbTransito_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    BLL.Aplicacao.Empresa = null;

                    if (Request.Url.Segments.Length == 3)
                    {
                        Response.Redirect("../Default.aspx", true);
                    }
                    else
                    {
                        Response.Redirect("Default.aspx", true);
                    }
                }
                else
                {
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                    DAL.Reserva reservaDAL = new DAL.Reserva();
                    string reservaId = txtReservaID.Text.Trim().ToUpper();

                    if (string.IsNullOrEmpty(reservaId))
                    {
                        throw new ApplicationException("É necessário informar o PedidoID para transitar a reserva.");
                    }
                    if (!reservaDAL.Existe(reservaId, usuarioSessao.SistemaID))
                    {
                        throw new ApplicationException("Reserva inexistente.");
                    }
                    int statusId = Convert.ToInt32(reservaDAL.Listar(Convert.ToInt32(reservaId), usuarioSessao.SistemaID).Tables[0].Rows[0]["StatusID"]);
                    if (statusId != UtilitarioBLL.StatusEntregaReserva.Pendente.GetHashCode())
                    {
                        if (statusId == UtilitarioBLL.StatusEntregaReserva.Transito.GetHashCode())
                        {
                            throw new ApplicationException(string.Format("Não é possível transitar reserva {0}, pois a mesma já se encontra em tr\x00e2nsito.", reservaId));
                        }
                        if (statusId == UtilitarioBLL.StatusEntregaReserva.Efetuada.GetHashCode())
                        {
                            throw new ApplicationException(string.Format("Não é possível transitar reserva {0}, pois a baixa já foi efetuada.", reservaId));
                        }
                    }
                    else
                    {
                        reservaDAL.InserirTransito(Convert.ToInt32(reservaId), usuarioSessao.SistemaID);
                        LimparFormulario(txtReservaID, txtDataReserva, txtDataEntrega);
                        CarregarDados();
                    }
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
            }
        }

        private void LimparFormulario(TextBox txtReserva, TextBox DataReserva, TextBox txtDataEntrega)
        {
            txtReserva.Text = string.Empty;
            txtDataReserva.Text = string.Empty;
            txtDataEntrega.Text = string.Empty;
            txtNomeCarreteiro.Text = string.Empty;
            ckbDataEntrega.Checked = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!UtilitarioBLL.PermissaoUsuario(Session["Usuario"]))
                    {
                        UtilitarioBLL.Sair();
                        if (Request.Url.Segments.Length == 3)
                        {
                            Response.Redirect("../Default.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("Default.aspx", false);
                        }
                    }
                    else if (new BLL.Modelo.Usuario(Session["Usuario"]).TipoUsuarioID == UtilitarioBLL.TipoUsuario.Vendedor.GetHashCode())
                    {
                        UtilitarioBLL.Sair();
                        if (Request.Url.Segments.Length == 3)
                        {
                            Response.Redirect("../Default.aspx", false);
                        }
                        else
                        {
                            Response.Redirect("Default.aspx", false);
                        }
                    }
                    else
                    {
                        VisualizarFormulario();
                        CarregarDados();
                        SetarBordaGridView();
                    }
                }
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
            }
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode())
            {
                imbDarBaixa.Visible = false;
                imbProrrogar.Visible = false;
                imbTransito.Visible = false;
            }
            else
            {
                imbDarBaixa.Visible = true;
                imbProrrogar.Visible = true;
                imbTransito.Visible = true;
            }
        }

        private void SetarBordaGridView()
        {
            gdvReservaAgendada.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
        }
    }
}