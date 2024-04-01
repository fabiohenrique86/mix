﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAO;
using DAL;

namespace Site
{
    public partial class LimiteReserva : System.Web.UI.Page
    {
        private void CarregarGridReserva()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            this.gdvLimiteReserva.DataSource = SistemaDAL.ListarLimiteReserva(usuarioSessao.SistemaID);
            this.gdvLimiteReserva.DataBind();
        }

        protected void imbAtualizar_Click(object sender, ImageClickEventArgs e)
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
                    if (string.IsNullOrEmpty(this.txtLimite.Text.Trim().ToUpper()) || (this.txtLimite.Text.Trim().ToUpper() == "0"))
                    {
                        throw new ApplicationException("É necessário informar o limite para atualizar.");
                    }
                    SistemaDAL.Atualizar(Convert.ToInt32(this.txtLimite.Text.Trim().ToUpper()), usuarioSessao.SistemaID);
                    this.CarregarGridReserva();
                    this.LimparFormulario();
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

        private void LimparFormulario()
        {
            this.txtLimite.Text = string.Empty;
        }

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
                            base.Response.Redirect("../Default.aspx");
                        }
                        else
                        {
                            base.Response.Redirect("Default.aspx");
                        }
                    }
                    else
                    {
                        this.VisualizarFormulario();
                        this.CarregarGridReserva();
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

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if (usuarioSessao.TipoUsuarioID == Convert.ToInt32(UtilitarioBLL.TipoUsuario.Administrador))
            {
                this.lblTopo.Text = "CONSULTA | CADASTRO | ATUALIZAÇÃO | EXCLUSÃO - PEDIDO AGENDADO - LIMITE";
            }
            else
            {
                this.lblTopo.Text = "CONSULTA | CADASTRO - PEDIDO AGENDADO - LIMITE";
            }
        }

    }
}