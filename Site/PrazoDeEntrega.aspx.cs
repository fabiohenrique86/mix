using BLL;
using DAL;
using System;
using System.Data;
using System.Web.UI;

namespace Site
{
    public partial class PrazoDeEntrega : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!base.IsPostBack)
                {
                    if (!UtilitarioBLL.PermissaoUsuario(Session["Usuario"]))
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
                    else
                    {
                        this.VisualizarFormulario();
                        this.CarregarDados();
                    }
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

        private void CarregarDados()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            DataTable dt = new DataTable();
            dt.Columns.Add("PrazoDeEntrega", typeof(int));
            var prazoDeEntrega = SistemaDAL.ListarPrazoDeEntrega(usuarioSessao.SistemaID);
            dt.Rows.Add(prazoDeEntrega);

            gdvPrazoDeEntrega.DataSource = dt;
            gdvPrazoDeEntrega.DataBind();
        }

        private void LimparCampos()
        {
            txtPrazoDeEntrega.Text = string.Empty;
        }

        protected void imbSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Usuario"] == null)
                {
                    BLL.AplicacaoBLL.Empresa = null;

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

                    int prazoDeEntrega;
                    int.TryParse(txtPrazoDeEntrega.Text, out prazoDeEntrega);

                    SistemaDAL.SalvarPrazoDeEntrega(prazoDeEntrega, usuarioSessao.SistemaID);
                    CarregarDados();
                    LimparCampos();
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

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if (usuarioSessao.TipoUsuarioID != UtilitarioBLL.TipoUsuario.Administrador.GetHashCode())
            {
                this.imbSalvar.Visible = false;
            }
        }
    }
}