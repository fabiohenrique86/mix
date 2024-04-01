using BLL;
using DAL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site
{
    public partial class Default : Page
    {
        protected void imbLogin_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                DataSet ds = new DAL.UsuarioDAL().ValidarLogin(this.txtLogin.Text, this.txtSenha.Text, Convert.ToInt32(this.Session["SistemaID"]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    switch (Convert.ToInt32(ds.Tables[0].Rows[0]["TipoUsuarioID"]))
                    {
                        case 1:
                            Session["Usuario"] = new BLL.Modelo.Usuario(Convert.ToInt32(ds.Tables[0].Rows[0]["TipoUsuarioID"]), ds.Tables[0].Rows[0]["TipoUsuario"].ToString(), Convert.ToInt32(this.Session["SistemaID"]), Convert.ToInt32(this.Session["StatusSistemaID"])); ;
                            base.Response.Redirect("Default.aspx", false);
                            return;

                        case 2:
                            Session["Usuario"] = new BLL.Modelo.Usuario(Convert.ToInt32(ds.Tables[0].Rows[0]["TipoUsuarioID"]), ds.Tables[0].Rows[0]["TipoUsuario"].ToString(), Convert.ToInt32(this.Session["SistemaID"]), Convert.ToInt32(this.Session["StatusSistemaID"]));
                            base.Response.Redirect("Default.aspx", false);
                            return;

                        case 3:
                            Session["Usuario"] = new BLL.Modelo.Usuario(Convert.ToInt32(ds.Tables[0].Rows[0]["TipoUsuarioID"]), ds.Tables[0].Rows[0]["TipoUsuario"].ToString(), Convert.ToInt32(this.Session["SistemaID"]), Convert.ToInt32(this.Session["StatusSistemaID"]), Convert.ToInt32(ds.Tables[0].Rows[0]["LojaID"]), ds.Tables[0].Rows[0]["NomeFantasia"].ToString());
                            base.Response.Redirect("Default.aspx", false);
                            return;

                        case 4:
                            Session["Usuario"] = new BLL.Modelo.Usuario(Convert.ToInt32(ds.Tables[0].Rows[0]["TipoUsuarioID"]), ds.Tables[0].Rows[0]["TipoUsuario"].ToString(), Convert.ToInt32(this.Session["SistemaID"]), Convert.ToInt32(this.Session["StatusSistemaID"]), Convert.ToInt32(ds.Tables[0].Rows[0]["LojaID"]), ds.Tables[0].Rows[0]["NomeFantasia"].ToString());
                            base.Response.Redirect("Default.aspx", false);
                            return;

                        case 5:
                            Session["Usuario"] = new BLL.Modelo.Usuario(Convert.ToInt32(ds.Tables[0].Rows[0]["TipoUsuarioID"]), ds.Tables[0].Rows[0]["TipoUsuario"].ToString(), Convert.ToInt32(this.Session["SistemaID"]), Convert.ToInt32(this.Session["StatusSistemaID"]));
                            base.Response.Redirect("Default.aspx", false);
                            return;
                        case 6:
                            Session["Usuario"] = new BLL.Modelo.Usuario(Convert.ToInt32(ds.Tables[0].Rows[0]["TipoUsuarioID"]), ds.Tables[0].Rows[0]["TipoUsuario"].ToString(), Convert.ToInt32(this.Session["SistemaID"]), Convert.ToInt32(this.Session["StatusSistemaID"]));
                            base.Response.Redirect("Default.aspx", false);
                            return;
                    }
                }
                else
                {
                    this.lblMensagem.Visible = true;
                    this.lblMensagem.Text = "Login e/ou Senha inválidos.";
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

        protected void imbSair_Click(object sender, ImageClickEventArgs e)
        {
            UtilitarioBLL.Sair();
            this.Session.Abandon();
            base.Response.Redirect("../Default.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["Usuario"] != null && ((BLL.Modelo.Usuario)Session["Usuario"]).StatusSistemaID == UtilitarioBLL.StatusSistema.Ativo.GetHashCode() && AplicacaoBLL.Empresa != null)
                {
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);                    
                    
                    this.tabela.Visible = false;
                    this.listaEmpresa.Visible = false;
                    this.home.Visible = true;

                    if (usuarioSessao.NomeLoja != null)
                    {
                        this.lblMensagemHome.Text = "<p>Bem vindo, <strong>" + usuarioSessao.TipoUsuario.ToUpper() + " - " + usuarioSessao.NomeLoja.ToUpper() + "</strong></p>";
                    }
                    else
                    {
                        this.lblMensagemHome.Text = "<p>Bem vindo, <strong>" + usuarioSessao.TipoUsuario.ToUpper() + "</strong></p>";
                    }

                    this.lblMensagemHome.Text = this.lblMensagemHome.Text + "<p>Acesse as funcionalidades no menu acima para o uso do sistema.</p>";
                    this.lblMensagemHome.Text = this.lblMensagemHome.Text + "<p>Qualquer d\x00favida, sugestão ou reclamação entre em contato enviando um e-mail na página de contato.</p>";
                    
                    if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode())
                    {
                        this.lblMensagemHome.Text = this.lblMensagemHome.Text + "<p style='font-size:16px; font-weight:bold;'>Faturamento Mensal</p>";
                        this.gdvFaturamento.Visible = true;
                    }
                }
                else if (AplicacaoBLL.Empresa != null)
                {
                    this.tabela.Visible = true;
                    this.listaEmpresa.Visible = false;
                    this.txtLogin.Focus();
                }
                else
                {
                    this.lblTopoCabeca.Text = "SISTEMA MiX";
                    this.tabela.Visible = false;
                    this.listaEmpresa.Visible = true;

                    if (!base.IsPostBack)
                    {
                        DataSet ds = SistemaDAL.Listar();
                        this.rblEmpresa.DataSource = ds;
                        this.rblEmpresa.DataBind();
                        for (int cont = 0; cont < this.rblEmpresa.Items.Count; cont++)
                        {
                            if (Convert.ToInt32(ds.Tables[0].Rows[cont]["StatusID"]) == 4)
                            {
                                ListItem item1 = this.rblEmpresa.Items[cont];
                                item1.Text = item1.Text + (this.rblEmpresa.Items[cont].Text = " - Inativo");
                                this.rblEmpresa.Items[cont].Enabled = false;
                            }
                        }
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

        protected void rblEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int sistemaId = Convert.ToInt32(this.rblEmpresa.SelectedValue);
                DataSet ds = SistemaDAL.Listar(sistemaId);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    throw new ApplicationException("Sistema não cadastrado.");
                }
                this.Session["SistemaID"] = sistemaId;
                this.Session["StatusSistemaID"] = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusID"]);
                base.Response.Redirect(this.rblEmpresa.SelectedItem.Text + "/Default.aspx", false);
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