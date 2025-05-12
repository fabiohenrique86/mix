using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAO;
using BLL;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Site
{
    public partial class Cliente : Page
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
                        VisualizarFormulario();
                        CarregarDropDownListFuncionario();
                        CarregarGridCliente();
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

        private bool CarregarDadosPF(string nome, string cpf, DateTime? dataNascimento, string endereco, string bairro, string cidade, string estado, string pontoReferencia, string telefoneResidencial, string telefoneCelular, string telefoneResidencial2, string telefoneCelular2, int? funcionarioId, string mes, string email, string cep)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            gdvCliente.DataSource = new DAL.ClienteDAL().Listar(new ClientePFDAO(nome, cpf, dataNascimento, endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial2, telefoneCelular2, usuarioSessao.SistemaID, funcionarioId, mes, email, cep));
            gdvCliente.DataBind();
            return (gdvCliente.Rows.Count > 0);
        }

        private bool CarregarDadosPJ(string cnpj, string nomeFantasia, string razaoSocial, string endereco, string bairro, string cidade, string estado, string pontoReferencia, string telefoneResidencial, string telefoneCelular, string telefoneResidencial2, string telefoneCelular2, int? funcionarioId, string email, string cep)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            gdvCliente.DataSource = new DAL.ClienteDAL().Listar(new ClientePJDAO(cnpj, nomeFantasia, razaoSocial, endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial2, telefoneCelular2, usuarioSessao.SistemaID, funcionarioId, email, cep));
            gdvCliente.DataBind();
            return (gdvCliente.Rows.Count > 0);
        }

        private void CarregarDropDownListFuncionario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if ((Session["dsDropDownListFuncionario"] == null) || (Session["bdFuncionario"] != null))
            {
                Session["dsDropDownListFuncionario"] = DAL.FuncionarioDAL.ListarDropDownList(usuarioSessao.LojaID.ToString(), usuarioSessao.SistemaID);
                Session["bdFuncionario"] = null;
            }
            ddlFuncionario.DataSource = Session["dsDropDownListFuncionario"];
            ddlFuncionario.DataBind();
        }

        private bool CarregarGridCliente()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            gdvCliente.DataSource = new DAL.ClienteDAL().Listar(usuarioSessao.SistemaID);
            gdvCliente.DataBind();
            return (gdvCliente.Rows.Count > 0);
        }

        protected void ckbClienteID_CheckedChanged(object sender, EventArgs e)
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            if (ckbClienteID.Checked)
            {
                txtClienteID.Enabled = true;
                txtClienteID.CssClass = string.Empty;
                imbCadastrar.Visible = false;
                imbConsultar.Visible = false;
                imbAtualizar.Visible = true;
                if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode())
                {
                    imbExcluir.Visible = true;
                }
                else
                {
                    imbExcluir.Visible = false;
                }
                ckbClienteID.Text = "Consultar/Cadastrar";
            }
            else
            {
                txtClienteID.Enabled = false;
                txtClienteID.CssClass = "desabilitado";
                imbCadastrar.Visible = true;
                imbConsultar.Visible = true;
                imbAtualizar.Visible = false;
                imbExcluir.Visible = false;
                ckbClienteID.Text = "Atualizar/Excluir";
            }
            LimparFormulario();
        }

        protected void gdvCliente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gdvCliente.PageIndex = e.NewPageIndex;

                int tipoClienteId = Convert.ToInt32(rblTipoCliente.SelectedValue);
                string endereco = string.IsNullOrEmpty(txtEndereco.Text.Trim().ToUpper()) ? null : txtEndereco.Text.Trim().ToUpper();
                string bairro = string.IsNullOrEmpty(txtBairro.Text.Trim().ToUpper()) ? null : txtBairro.Text.Trim().ToUpper();
                string cidade = string.IsNullOrEmpty(txtCidade.Text.Trim().ToUpper()) ? null : txtCidade.Text.Trim().ToUpper();
                string estado = string.IsNullOrEmpty(txtEstado.Text.Trim().ToUpper()) ? null : txtEstado.Text.Trim().ToUpper();
                string pontoReferencia = string.IsNullOrEmpty(txtPontoReferencia.Text.Trim().ToUpper()) ? null : txtPontoReferencia.Text.Trim().ToUpper();
                string telefoneResidencial = string.IsNullOrEmpty(txtTelefoneResidencial.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneResidencial.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                string telefoneResidencial2 = string.IsNullOrEmpty(txtTelefoneResidencial2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneResidencial2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                string telefoneCelular = string.IsNullOrEmpty(txtTelefoneCelular.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneCelular.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                string telefoneCelular2 = string.IsNullOrEmpty(txtTelefoneCelular2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneCelular2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                string email = string.IsNullOrEmpty(txtEmail.Text.Trim().ToUpper()) ? null : txtEmail.Text.Trim().ToUpper();
                string cep = string.IsNullOrEmpty(txtCEP.Text.Trim().Replace("-", "").ToUpper()) ? null : txtCEP.Text.Trim().Replace("-", "").ToUpper();

                int? funcionarioId = null;

                if (ddlFuncionario.SelectedValue != "0")
                {
                    funcionarioId = Convert.ToInt32(ddlFuncionario.SelectedValue);
                }

                string mesNascimento = (ddlMes.SelectedValue == "0") ? null : ddlMes.SelectedValue;

                if (tipoClienteId == UtilitarioBLL.TipoCliente.PessoaFisica.GetHashCode())
                {
                    string nome = string.IsNullOrEmpty(txtNome.Text.Trim().ToUpper()) ? null : txtNome.Text.Trim().ToUpper();
                    string cpfFormatado = string.IsNullOrEmpty(txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                    string dataNascimento = txtDataNascimento.Text.Trim().ToUpper().Replace("_", "").Replace("/", "").Replace("-", "").Replace(".", "").Replace(",", "").Replace(@"\", "");
                    DateTime? dtNascimento = null;

                    if ((((!string.IsNullOrEmpty(nome) || !string.IsNullOrEmpty(cpfFormatado)) || (!string.IsNullOrEmpty(dataNascimento) || !string.IsNullOrEmpty(endereco))) || ((!string.IsNullOrEmpty(bairro) || !string.IsNullOrEmpty(cidade)) || (!string.IsNullOrEmpty(estado) || !string.IsNullOrEmpty(pontoReferencia)))) || (((!string.IsNullOrEmpty(telefoneResidencial) || !string.IsNullOrEmpty(telefoneCelular)) || (!string.IsNullOrEmpty(telefoneResidencial2) || !string.IsNullOrEmpty(telefoneCelular2))) || (funcionarioId.HasValue || !string.IsNullOrEmpty(mesNascimento))))
                    {
                        CarregarDadosPF(nome, cpfFormatado, string.IsNullOrEmpty(dataNascimento) ? dtNascimento : Convert.ToDateTime(txtDataNascimento.Text.Trim()), endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial2, telefoneCelular2, funcionarioId, mesNascimento, email, cep);
                    }
                    else
                    {
                        CarregarGridCliente();
                    }
                }
                else if (tipoClienteId == UtilitarioBLL.TipoCliente.PessoaJuridica.GetHashCode())
                {
                    string cnpjFormatado = string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCnpj.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                    string nomeFantasia = string.IsNullOrEmpty(txtNomeFantasia.Text.Trim().ToUpper()) ? null : txtNomeFantasia.Text.Trim().ToUpper();
                    string razaoSocial = string.IsNullOrEmpty(txtRazaoSocial.Text.Trim().ToUpper()) ? null : txtRazaoSocial.Text.Trim().ToUpper();

                    if ((((!string.IsNullOrEmpty(nomeFantasia) || !string.IsNullOrEmpty(razaoSocial)) || (!string.IsNullOrEmpty(cnpjFormatado) || !string.IsNullOrEmpty(endereco))) || ((!string.IsNullOrEmpty(bairro) || !string.IsNullOrEmpty(cidade)) || (!string.IsNullOrEmpty(estado) || !string.IsNullOrEmpty(pontoReferencia)))) || ((!string.IsNullOrEmpty(telefoneResidencial) || !string.IsNullOrEmpty(telefoneCelular)) || ((!string.IsNullOrEmpty(telefoneResidencial2) || !string.IsNullOrEmpty(telefoneCelular2)) || funcionarioId.HasValue)))
                    {
                        CarregarDadosPJ(cnpjFormatado, nomeFantasia, razaoSocial, endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial, telefoneCelular2, funcionarioId, email, cep);
                    }
                    else
                    {
                        CarregarGridCliente();
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

        protected void gdvCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if
                    (
                        usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()
                        || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode()
                        || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode()
                        || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Logistica.GetHashCode()
                        || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Vendedor.GetHashCode()
                    )
                    {
                        e.Row.Cells[0].Visible = true;
                    }
                    else
                    {
                        e.Row.Cells[0].Visible = false;
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string tipoCliente = !string.IsNullOrEmpty(((Label)e.Row.FindControl("lblCpf")).Text) ? "PF" : "PJ";
                    if
                    (
                        usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()
                        || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode()
                        || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode()
                        || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Logistica.GetHashCode()
                        || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Vendedor.GetHashCode()
                    )
                    {
                        e.Row.Cells[0].Visible = true;
                    }
                    else
                    {
                        e.Row.Cells[0].Visible = false;
                    }
                    switch (tipoCliente)
                    {
                        case "PF":
                            {
                                ((Label)e.Row.FindControl("lblNome")).Visible = true;
                                ((Label)e.Row.FindControl("lblNomeFantasia")).Visible = false;
                                ((Label)e.Row.FindControl("lblCpf")).Visible = true;
                                ((Label)e.Row.FindControl("lblCnpj")).Visible = false;
                                ((Label)e.Row.FindControl("lblDataNascimento")).Visible = true;
                                ((Label)e.Row.FindControl("lblRazaoSocial")).Visible = false;
                                string cpfFormatado = string.Format(@"{0:###\.###\.###-##}", Convert.ToInt64(((Label)e.Row.FindControl("lblCpf")).Text.Replace("_", "").Replace("-", "").Replace(".", "")));
                                if ((cpfFormatado.Length > 3) && (cpfFormatado.Length < 14))
                                {
                                    int quantidadeZeros = 14 - cpfFormatado.Length;
                                    if (quantidadeZeros > 0)
                                    {
                                        for (int i = 0; i < quantidadeZeros; i++)
                                        {
                                            cpfFormatado = cpfFormatado.Insert(i, "0");
                                        }
                                    }
                                }
                                ((Label)e.Row.FindControl("lblCpf")).Text = cpfFormatado;
                                break;
                            }
                        case "PJ":
                            {
                                ((Label)e.Row.FindControl("lblNome")).Visible = false;
                                ((Label)e.Row.FindControl("lblNomeFantasia")).Visible = true;
                                ((Label)e.Row.FindControl("lblCpf")).Visible = false;
                                ((Label)e.Row.FindControl("lblCnpj")).Visible = true;
                                ((Label)e.Row.FindControl("lblDataNascimento")).Visible = false;
                                ((Label)e.Row.FindControl("lblRazaoSocial")).Visible = true;
                                string cnpjFormatado = string.Format(@"{0:##\.###\.###/####-##}", Convert.ToInt64(((Label)e.Row.FindControl("lblCnpj")).Text.Replace("_", "").Replace("-", "").Replace(".", "")));
                                if ((cnpjFormatado.Length > 4) && (cnpjFormatado.Length < 0x12))
                                {
                                    int quantidadeZeros = 0x12 - cnpjFormatado.Length;
                                    if (quantidadeZeros > 0)
                                    {
                                        for (int i = 0; i < quantidadeZeros; i++)
                                        {
                                            cnpjFormatado = cnpjFormatado.Insert(i, "0");
                                        }
                                    }
                                }
                                ((Label)e.Row.FindControl("lblCnpj")).Text = cnpjFormatado;
                                break;
                            }
                    }

                    string telefoneResidencial = ((Label)e.Row.FindControl("lblTelefoneResidencial")).Text;
                    string telefoneResidencial2 = ((Label)e.Row.FindControl("lblTelefoneResidencial2")).Text;
                    string telefoneCelular = ((Label)e.Row.FindControl("lblTelefoneCelular")).Text;
                    string telefoneCelular2 = ((Label)e.Row.FindControl("lblTelefoneCelular2")).Text;

                    if (telefoneResidencial.Length == 8)
                    {
                        ((Label)e.Row.FindControl("lblTelefoneResidencial")).Text = string.Format("{0:####-####}", Convert.ToInt64(telefoneResidencial));
                    }
                    else if (telefoneResidencial.Length == 10)
                    {
                        ((Label)e.Row.FindControl("lblTelefoneResidencial")).Text = string.Format("{0:(##)####-####}", Convert.ToInt64(telefoneResidencial));
                    }

                    if (telefoneResidencial2.Length == 8)
                    {
                        ((Label)e.Row.FindControl("lblTelefoneResidencial2")).Text = string.Format("{0:####-####}", Convert.ToInt64(telefoneResidencial2));
                    }
                    else if (telefoneResidencial2.Length == 10)
                    {
                        ((Label)e.Row.FindControl("lblTelefoneResidencial2")).Text = string.Format("{0:(##)####-####}", Convert.ToInt64(telefoneResidencial2));
                    }

                    if (telefoneCelular.Length == 8)
                    {
                        ((Label)e.Row.FindControl("lblTelefoneCelular")).Text = string.Format("{0:####-####}", Convert.ToInt64(telefoneCelular));
                    }
                    else if (telefoneCelular.Length == 10)
                    {
                        ((Label)e.Row.FindControl("lblTelefoneCelular")).Text = string.Format("{0:(##)####-####}", Convert.ToInt64(telefoneCelular));
                    }
                    else if (telefoneCelular.Length == 11)
                    {
                        ((Label)e.Row.FindControl("lblTelefoneCelular")).Text = string.Format("{0:(##)#####-####}", Convert.ToInt64(telefoneCelular));
                    }

                    if (telefoneCelular2.Length == 8)
                    {
                        ((Label)e.Row.FindControl("lblTelefoneCelular2")).Text = string.Format("{0:####-####}", Convert.ToInt64(telefoneCelular2));
                    }
                    else if (telefoneCelular2.Length == 10)
                    {
                        ((Label)e.Row.FindControl("lblTelefoneCelular2")).Text = string.Format("{0:(##)####-####}", Convert.ToInt64(telefoneCelular2));
                    }
                    else if (telefoneCelular2.Length == 11)
                    {
                        ((Label)e.Row.FindControl("lblTelefoneCelular2")).Text = string.Format("{0:(##)#####-####}", Convert.ToInt64(telefoneCelular2));
                    }
                }
            }
            catch (FormatException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
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
                    DAL.ClienteDAL clienteDAL = new DAL.ClienteDAL();

                    if (string.IsNullOrEmpty(txtClienteID.Text.Trim().ToUpper()))
                    {
                        throw new ApplicationException("Informe o ClienteID para efetuar atualização.");
                    }

                    int clienteId = Convert.ToInt32(txtClienteID.Text.Trim().ToUpper());
                    int tipoClienteId = Convert.ToInt32(rblTipoCliente.SelectedValue);
                    string endereco = string.IsNullOrEmpty(txtEndereco.Text.Trim().ToUpper()) ? null : txtEndereco.Text.Trim().ToUpper();
                    string bairro = string.IsNullOrEmpty(txtBairro.Text.Trim().ToUpper()) ? null : txtBairro.Text.Trim().ToUpper();
                    string cidade = string.IsNullOrEmpty(txtCidade.Text.Trim().ToUpper()) ? null : txtCidade.Text.Trim().ToUpper();
                    string estado = string.IsNullOrEmpty(txtEstado.Text.Trim().ToUpper()) ? null : txtEstado.Text.Trim().ToUpper();
                    string pontoReferencia = string.IsNullOrEmpty(txtPontoReferencia.Text.Trim().ToUpper()) ? null : txtPontoReferencia.Text.Trim().ToUpper();
                    string telefoneResidencial = string.IsNullOrEmpty(txtTelefoneResidencial.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneResidencial.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                    string telefoneResidencial2 = string.IsNullOrEmpty(txtTelefoneResidencial2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneResidencial2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                    string telefoneCelular = string.IsNullOrEmpty(txtTelefoneCelular.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneCelular.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                    string telefoneCelular2 = string.IsNullOrEmpty(txtTelefoneCelular2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneCelular2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");

                    string email = string.IsNullOrEmpty(txtEmail.Text.Trim().ToUpper()) ? null : txtEmail.Text.Trim().ToUpper();
                    string cep = string.IsNullOrEmpty(txtCEP.Text.Trim().Replace("-", "")) ? null : txtCEP.Text.Trim().Replace("-", "");

                    if (tipoClienteId == UtilitarioBLL.TipoCliente.PessoaFisica.GetHashCode())
                    {
                        string nome = string.IsNullOrEmpty(txtNome.Text.Trim().ToUpper()) ? null : txtNome.Text.Trim().ToUpper();
                        string cpfFormatado = string.IsNullOrEmpty(txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                        string dataNascimento = txtDataNascimento.Text.Trim().ToUpper().Replace("_", "").Replace("/", "").Replace("-", "").Replace(".", "").Replace(",", "").Replace(@"\", "");
                        if ((((string.IsNullOrEmpty(nome) && string.IsNullOrEmpty(cpfFormatado)) && (string.IsNullOrEmpty(dataNascimento) && string.IsNullOrEmpty(endereco))) && ((string.IsNullOrEmpty(bairro) && string.IsNullOrEmpty(cidade)) && (string.IsNullOrEmpty(estado) && string.IsNullOrEmpty(pontoReferencia)))) && ((string.IsNullOrEmpty(telefoneResidencial) && string.IsNullOrEmpty(telefoneCelular)) && (string.IsNullOrEmpty(telefoneResidencial2) && string.IsNullOrEmpty(telefoneCelular2))))
                        {
                            throw new ApplicationException("É necessário informar um ou mais campos para atualizar.");
                        }
                        DateTime? dtNascimento = null;
                        if (!string.IsNullOrEmpty(cpfFormatado))
                        {
                            throw new ApplicationException("Não é possível atualizar o CPF.");
                        }
                        cpfFormatado = null;
                        if (!clienteDAL.Listar(clienteId, usuarioSessao.SistemaID))
                        {
                            throw new ApplicationException("Cliente inexistente.");
                        }
                        if (!clienteDAL.EstaAtivo(clienteId, usuarioSessao.SistemaID))
                        {
                            throw new ApplicationException("Cliente inexistente.");
                        }
                        clienteDAL.Atualizar(new ClientePFDAO(clienteId, nome, cpfFormatado, string.IsNullOrEmpty(dataNascimento) ? dtNascimento : Convert.ToDateTime(txtDataNascimento.Text.Trim()), endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial2, telefoneCelular2, usuarioSessao.SistemaID, email, cep));
                    }
                    else if (tipoClienteId == UtilitarioBLL.TipoCliente.PessoaJuridica.GetHashCode())
                    {
                        string cnpjFormatado = string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCnpj.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                        string nomeFantasia = string.IsNullOrEmpty(txtNomeFantasia.Text.Trim().ToUpper()) ? null : txtNomeFantasia.Text.Trim().ToUpper();
                        string razaoSocial = string.IsNullOrEmpty(txtRazaoSocial.Text.Trim().ToUpper()) ? null : txtRazaoSocial.Text.Trim().ToUpper();

                        if ((((string.IsNullOrEmpty(nomeFantasia) && string.IsNullOrEmpty(razaoSocial)) && (string.IsNullOrEmpty(cnpjFormatado) && string.IsNullOrEmpty(endereco))) && ((string.IsNullOrEmpty(bairro) && string.IsNullOrEmpty(cidade)) && (string.IsNullOrEmpty(txtEstado.Text.Trim().ToUpper()) && string.IsNullOrEmpty(pontoReferencia)))) && ((string.IsNullOrEmpty(telefoneResidencial) && string.IsNullOrEmpty(telefoneCelular)) && (string.IsNullOrEmpty(telefoneResidencial2) && string.IsNullOrEmpty(telefoneCelular2))))
                        {
                            throw new ApplicationException("É necessário informar um ou mais campos para atualizar.");
                        }
                        if (!string.IsNullOrEmpty(cnpjFormatado))
                        {
                            throw new ApplicationException("Não é possível atualizar o CNPJ.");
                        }
                        cnpjFormatado = null;
                        if (!clienteDAL.Listar(clienteId, usuarioSessao.SistemaID))
                        {
                            throw new ApplicationException("Cliente inexistente.");
                        }
                        if (!clienteDAL.EstaAtivo(clienteId, usuarioSessao.SistemaID))
                        {
                            throw new ApplicationException("Cliente inexistente.");
                        }
                        clienteDAL.Atualizar(new ClientePJDAO(clienteId, cnpjFormatado, nomeFantasia, razaoSocial, endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial2, telefoneCelular2, usuarioSessao.SistemaID, email, cep));
                    }
                    LimparFormulario();
                    CarregarGridCliente();
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

        protected void imbCadastrar_Click(object sender, ImageClickEventArgs e)
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
                DAL.ClienteDAL clienteDAL = new DAL.ClienteDAL();

                int tipoClienteId = Convert.ToInt32(rblTipoCliente.SelectedValue);
                string endereco = string.IsNullOrEmpty(txtEndereco.Text.Trim().ToUpper()) ? null : txtEndereco.Text.Trim().ToUpper();
                string bairro = string.IsNullOrEmpty(txtBairro.Text.Trim().ToUpper()) ? null : txtBairro.Text.Trim().ToUpper();
                string cidade = string.IsNullOrEmpty(txtCidade.Text.Trim().ToUpper()) ? null : txtCidade.Text.Trim().ToUpper();
                string estado = string.IsNullOrEmpty(txtEstado.Text.Trim().ToUpper()) ? null : txtEstado.Text.Trim().ToUpper();
                string pontoReferencia = string.IsNullOrEmpty(txtPontoReferencia.Text.Trim().ToUpper()) ? null : txtPontoReferencia.Text.Trim().ToUpper();
                string telefoneResidencial = string.IsNullOrEmpty(txtTelefoneResidencial.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneResidencial.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                string telefoneResidencial2 = string.IsNullOrEmpty(txtTelefoneResidencial2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneResidencial2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                string telefoneCelular = string.IsNullOrEmpty(txtTelefoneCelular.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneCelular.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                string telefoneCelular2 = string.IsNullOrEmpty(txtTelefoneCelular2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneCelular2.Text.Trim().ToUpper().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                string nome = string.IsNullOrEmpty(txtNome.Text.Trim().ToUpper()) ? null : txtNome.Text.Trim().ToUpper();
                string cpfFormatado = string.IsNullOrEmpty(txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                string dataNascimento = txtDataNascimento.Text.Trim().ToUpper().Replace("_", "").Replace("/", "").Replace("-", "").Replace(".", "").Replace(",", "").Replace(@"\", "");
                string cnpjFormatado = string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCnpj.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                string nomeFantasia = string.IsNullOrEmpty(txtNomeFantasia.Text.Trim().ToUpper()) ? null : txtNomeFantasia.Text.Trim().ToUpper();
                string razaoSocial = string.IsNullOrEmpty(txtRazaoSocial.Text.Trim().ToUpper()) ? null : txtRazaoSocial.Text.Trim().ToUpper();

                string email = string.IsNullOrEmpty(txtEmail.Text.Trim().ToUpper()) ? null : txtEmail.Text.Trim().ToUpper();
                string cep = string.IsNullOrEmpty(txtCEP.Text.Trim().Replace("-", "")) ? null : txtCEP.Text.Trim().Replace("-", "");

                try
                {
                    if (tipoClienteId == UtilitarioBLL.TipoCliente.PessoaFisica.GetHashCode())
                    {
                        if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(cpfFormatado) || string.IsNullOrEmpty(dataNascimento) || string.IsNullOrEmpty(endereco) || string.IsNullOrEmpty(bairro) || string.IsNullOrEmpty(cidade) || string.IsNullOrEmpty(estado) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(cep) || string.IsNullOrEmpty(telefoneCelular))
                        {
                            throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                        }

                        if (!new BLL.UtilsBLL().ValidarCpf(cpfFormatado))
                        {
                            throw new ApplicationException("CPF inválido.");
                        }

                        int clienteId = clienteDAL.ListarByCpf(cpfFormatado, usuarioSessao.SistemaID);

                        if (clienteId <= 0)
                        {
                            clienteDAL.Inserir(new ClientePFDAO(nome, cpfFormatado, Convert.ToDateTime(txtDataNascimento.Text.Trim()), endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial2, telefoneCelular2, usuarioSessao.SistemaID, email, cep));
                        }
                        else
                        {
                            if (clienteDAL.EstaAtivoByCpf(cpfFormatado, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException("Cliente cadastrado.");
                            }
                            DateTime? dtNascimento = null;
                            clienteDAL.Atualizar(new ClientePFDAO(clienteId, nome, cpfFormatado, string.IsNullOrEmpty(dataNascimento) ? dtNascimento : Convert.ToDateTime(txtDataNascimento.Text.Trim()), endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial2, telefoneCelular2, usuarioSessao.SistemaID, email, cep));
                        }
                    }
                    else if (tipoClienteId == UtilitarioBLL.TipoCliente.PessoaJuridica.GetHashCode())
                    {
                        if (((string.IsNullOrEmpty(nomeFantasia) || string.IsNullOrEmpty(razaoSocial)) || (string.IsNullOrEmpty(cnpjFormatado) || string.IsNullOrEmpty(endereco))) || ((string.IsNullOrEmpty(bairro) || string.IsNullOrEmpty(cidade)) || string.IsNullOrEmpty(estado)))
                        {
                            throw new ApplicationException("É necessário informar todos os campos obrigatórios para cadastrar.");
                        }

                        if (!new BLL.UtilsBLL().ValidarCnpj(cnpjFormatado))
                        {
                            throw new ApplicationException("CNPJ inválido.");
                        }

                        int clienteId = clienteDAL.ListarByCnpj(cnpjFormatado, usuarioSessao.SistemaID);

                        if (clienteId <= 0)
                        {
                            clienteDAL.Inserir(new ClientePJDAO(cnpjFormatado, nomeFantasia, razaoSocial, endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial2, telefoneCelular2, usuarioSessao.SistemaID, email, cep));
                        }
                        else
                        {
                            if (clienteDAL.EstaAtivoByCnpj(cnpjFormatado, usuarioSessao.SistemaID))
                            {
                                throw new ApplicationException("Cliente cadastrado.");
                            }

                            clienteDAL.Atualizar(new ClientePJDAO(clienteId, cnpjFormatado, nomeFantasia, razaoSocial, endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial2, telefoneCelular2, usuarioSessao.SistemaID, email, cep));
                        }
                    }

                    LimparFormulario();
                    CarregarGridCliente();

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
                        ex.Data.Add("tipoClienteId", tipoClienteId);
                        ex.Data.Add("endereco", endereco);
                        ex.Data.Add("bairro", bairro);
                        ex.Data.Add("cidade", cidade);
                        ex.Data.Add("estado", estado);
                        ex.Data.Add("pontoReferencia", pontoReferencia);
                        ex.Data.Add("telefoneResidencial", telefoneResidencial);
                        ex.Data.Add("telefoneResidencial2", telefoneResidencial2);
                        ex.Data.Add("telefoneCelular", telefoneCelular);
                        ex.Data.Add("telefoneCelular2", telefoneCelular2);
                        ex.Data.Add("nome", nome);
                        ex.Data.Add("cpfFormatado", cpfFormatado);
                        ex.Data.Add("dataNascimento", dataNascimento);
                        ex.Data.Add("cnpjFormatado", cnpjFormatado);
                        ex.Data.Add("nomeFantasia", nomeFantasia);
                        ex.Data.Add("razaoSocial", razaoSocial);
                        ex.Data.Add("email", email);
                        ex.Data.Add("cep", cep);

                        UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message, ex);
                    }
                }
            }
        }

        protected void imbConsultar_Click(object sender, ImageClickEventArgs e)
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
                int tipoClienteId = Convert.ToInt32(rblTipoCliente.SelectedValue);
                string endereco = string.IsNullOrEmpty(txtEndereco.Text.Trim().ToUpper()) ? null : txtEndereco.Text.Trim().ToUpper();
                string bairro = string.IsNullOrEmpty(txtBairro.Text.Trim().ToUpper()) ? null : txtBairro.Text.Trim().ToUpper();
                string cidade = string.IsNullOrEmpty(txtCidade.Text.Trim().ToUpper()) ? null : txtCidade.Text.Trim().ToUpper();
                string estado = string.IsNullOrEmpty(txtEstado.Text.Trim().ToUpper()) ? null : txtEstado.Text.Trim().ToUpper();
                string pontoReferencia = string.IsNullOrEmpty(txtPontoReferencia.Text.Trim().ToUpper()) ? null : txtPontoReferencia.Text.Trim().ToUpper();
                string telefoneResidencial = string.IsNullOrEmpty(txtTelefoneResidencial.Text.Trim().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneResidencial.Text.Trim().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                string telefoneResidencial2 = string.IsNullOrEmpty(txtTelefoneResidencial2.Text.Trim().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneResidencial2.Text.Trim().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                string telefoneCelular = string.IsNullOrEmpty(txtTelefoneCelular.Text.Trim().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneCelular.Text.Trim().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                string telefoneCelular2 = string.IsNullOrEmpty(txtTelefoneCelular2.Text.Trim().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "")) ? null : txtTelefoneCelular2.Text.Trim().Replace("(", "").Replace(")", "").Replace("_", "").Replace("-", "");
                int? funcionarioId = null;
                string mesNascimento = (ddlMes.SelectedValue == "0") ? null : ddlMes.SelectedValue;
                string nome = string.IsNullOrEmpty(txtNome.Text.Trim().ToUpper()) ? null : txtNome.Text.Trim().ToUpper();
                string cpfFormatado = string.IsNullOrEmpty(txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCpf.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                string dataNascimento = txtDataNascimento.Text.Trim().Replace("_", "").Replace("/", "").Replace("-", "").Replace(".", "").Replace(",", "").Replace(@"\", "");
                string cnpjFormatado = string.IsNullOrEmpty(txtCnpj.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "")) ? null : txtCnpj.Text.Replace(".", "").Replace(",", "").Replace("/", "").Replace("-", "").Replace("_", "");
                string nomeFantasia = string.IsNullOrEmpty(txtNomeFantasia.Text.Trim().ToUpper()) ? null : txtNomeFantasia.Text.Trim().ToUpper();
                string razaoSocial = string.IsNullOrEmpty(txtRazaoSocial.Text.Trim().ToUpper()) ? null : txtRazaoSocial.Text.Trim().ToUpper();

                string email = string.IsNullOrEmpty(txtEmail.Text.Trim().ToUpper()) ? null : txtEmail.Text.Trim().ToUpper();
                string cep = string.IsNullOrEmpty(txtCEP.Text.Trim().Replace("-", "")) ? null : txtCEP.Text.Trim().Replace("-", "");

                try
                {
                    if (ddlFuncionario.SelectedValue != "0")
                    {
                        funcionarioId = Convert.ToInt32(ddlFuncionario.SelectedValue);
                    }

                    if (tipoClienteId == UtilitarioBLL.TipoCliente.PessoaFisica.GetHashCode())
                    {
                        DateTime? dtNascimento = null;

                        if (string.IsNullOrEmpty(nome) && string.IsNullOrEmpty(cpfFormatado) && string.IsNullOrEmpty(dataNascimento) && string.IsNullOrEmpty(endereco) && string.IsNullOrEmpty(bairro) && string.IsNullOrEmpty(cidade) && string.IsNullOrEmpty(estado) && string.IsNullOrEmpty(pontoReferencia) && string.IsNullOrEmpty(telefoneResidencial) && string.IsNullOrEmpty(telefoneCelular) && string.IsNullOrEmpty(telefoneResidencial2) && string.IsNullOrEmpty(telefoneCelular2) && funcionarioId == null && string.IsNullOrEmpty(mesNascimento))
                        {
                            throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                        }

                        if (!CarregarDadosPF(nome, cpfFormatado, string.IsNullOrEmpty(dataNascimento) ? dtNascimento : Convert.ToDateTime(txtDataNascimento.Text.Trim()), endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial2, telefoneCelular2, funcionarioId, mesNascimento, email, cep))
                        {
                            throw new ApplicationException("Cliente inexistente.");
                        }
                    }
                    else if (tipoClienteId == UtilitarioBLL.TipoCliente.PessoaJuridica.GetHashCode())
                    {
                        if (string.IsNullOrEmpty(nomeFantasia) && string.IsNullOrEmpty(razaoSocial) && string.IsNullOrEmpty(cnpjFormatado) && string.IsNullOrEmpty(endereco) && string.IsNullOrEmpty(bairro) && string.IsNullOrEmpty(cidade) && string.IsNullOrEmpty(estado) && string.IsNullOrEmpty(pontoReferencia) && string.IsNullOrEmpty(telefoneResidencial) && string.IsNullOrEmpty(telefoneCelular) && string.IsNullOrEmpty(telefoneResidencial2) && string.IsNullOrEmpty(telefoneCelular2) && funcionarioId == null)
                        {
                            throw new ApplicationException("É necessário informar um ou mais campos para consultar.");
                        }

                        if (!CarregarDadosPJ(cnpjFormatado, nomeFantasia, razaoSocial, endereco, bairro, cidade, estado, pontoReferencia, telefoneResidencial, telefoneCelular, telefoneResidencial, telefoneCelular2, funcionarioId, email, cep))
                        {
                            throw new ApplicationException("Cliente inexistente.");
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
                    ex.Data.Add("tipoClienteId", tipoClienteId);
                    ex.Data.Add("endereco", endereco);
                    ex.Data.Add("bairro", bairro);
                    ex.Data.Add("cidade", cidade);
                    ex.Data.Add("estado", estado);
                    ex.Data.Add("pontoReferencia", pontoReferencia);
                    ex.Data.Add("telefoneResidencial", telefoneResidencial);
                    ex.Data.Add("telefoneResidencial2", telefoneResidencial2);
                    ex.Data.Add("telefoneCelular", telefoneCelular);
                    ex.Data.Add("telefoneCelular2", telefoneCelular2);
                    ex.Data.Add("nome", nome);
                    ex.Data.Add("cpfFormatado", cpfFormatado);
                    ex.Data.Add("dataNascimento", dataNascimento);
                    ex.Data.Add("cnpjFormatado", cnpjFormatado);
                    ex.Data.Add("nomeFantasia", nomeFantasia);
                    ex.Data.Add("razaoSocial", razaoSocial);
                    ex.Data.Add("email", email);
                    ex.Data.Add("cep", cep);

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
        }

        protected void imbExcluir_Click(object sender, ImageClickEventArgs e)
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
                    DAL.ClienteDAL clienteDAL = new DAL.ClienteDAL();

                    if (string.IsNullOrEmpty(txtClienteID.Text.Trim().ToUpper()))
                        throw new ApplicationException("Informe o ClienteID para efetuar exclusão.");

                    int clienteId = Convert.ToInt32(txtClienteID.Text.Trim().ToUpper());
                    if (!clienteDAL.Listar(clienteId, usuarioSessao.SistemaID))
                        throw new ApplicationException("Cliente inexistente.");

                    if (!clienteDAL.EstaAtivo(clienteId, usuarioSessao.SistemaID))
                        throw new ApplicationException("Cliente inexistente.");

                    clienteDAL.Excluir(clienteId, usuarioSessao.SistemaID);
                    LimparFormulario();
                    CarregarGridCliente();
                }
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

        private void LimparFormulario()
        {
            txtClienteID.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtNomeFantasia.Text = string.Empty;
            txtRazaoSocial.Text = string.Empty;
            txtCpf.Text = string.Empty;
            txtCnpj.Text = string.Empty;
            txtDataNascimento.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtCidade.Text = string.Empty;
            txtEstado.Text = string.Empty;
            txtPontoReferencia.Text = string.Empty;
            txtTelefoneResidencial.Text = string.Empty;
            txtTelefoneCelular.Text = string.Empty;
            txtTelefoneResidencial2.Text = string.Empty;
            txtTelefoneCelular2.Text = string.Empty;
            ddlFuncionario.SelectedIndex = 0;
            ddlMes.SelectedIndex = 0;
            txtEmail.Text = string.Empty;
            txtCEP.Text = string.Empty;
        }

        protected void rblTipoCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                VisualizacaoPorTipoCliente(Convert.ToInt32(rblTipoCliente.SelectedValue));
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

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                string cep = new string(txtCEP.Text.Trim().Where(char.IsDigit).ToArray());

                if (string.IsNullOrEmpty(cep) || cep.Length != 8)
                {
                    throw new ApplicationException("Por favor, insira um CEP válido (8 números).");
                }

                //string url = $"https://viacep.com.br/ws/{cep}/json/";
                string url = $"https://brasilapi.com.br/api/cep/v2/{cep}";

                string json = ConsultarCEP(url);

                if (!string.IsNullOrEmpty(json))
                {
                    //var dadosCep = JsonConvert.DeserializeObject<Endereco>(json);
                    var dadosCep = JsonConvert.DeserializeObject<CepResponse>(json);

                    if (dadosCep != null)
                    {
                        txtEndereco.Text = dadosCep.Street;
                        txtBairro.Text = dadosCep.Neighborhood;
                        txtCidade.Text = dadosCep.City;
                        txtEstado.Text = dadosCep.State;
                        //txtPontoReferencia.Text = dadosCep.Service;
                    }
                    else
                    {
                        throw new ApplicationException("CEP não encontrado");
                    }

                    /*
                    if (dadosCep != null && string.IsNullOrEmpty(dadosCep.Erro))
                    {
                        txtEndereco.Text = dadosCep.Logradouro;
                        txtBairro.Text = dadosCep.Bairro;
                        txtCidade.Text = dadosCep.Localidade;
                        txtEstado.Text = dadosCep.Uf;
                        txtPontoReferencia.Text = dadosCep.Complemento;
                    }
                    else
                    {
                        throw new ApplicationException("CEP não encontrado");
                    }*/
                }
                else
                {
                    throw new ApplicationException("Erro ao consultar o CEP.");
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

        private string ConsultarCEP(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            /*
            using (WebClient client = new WebClient())
            {
                try
                {
                    return client.DownloadString(url);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro na consulta de CEP: {ex.Message}");
                }
            }*/

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Timeout = 30000;  // Timeout de 30 segundos
            request.ReadWriteTimeout = 30000;
            request.Method = "GET";
            request.ProtocolVersion = HttpVersion.Version11; // Definir versão HTTP
            request.ServicePoint.Expect100Continue = false;
            request.AllowAutoRedirect = true;
            //request.SecurityProtocol = SecurityProtocolType.Tls12;  // Forçar TLS 1.2

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    throw new Exception($"Erro na consulta. Código HTTP: {response.StatusCode}");
                }
            }
        }

        public class CepResponse
        {
            public string Cep { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Neighborhood { get; set; }
            public string Street { get; set; }
            public string Service { get; set; }
        }

        /*
        public class Endereco
        {
            [JsonProperty("cep")]
            public string Cep { get; set; }

            [JsonProperty("logradouro")]
            public string Logradouro { get; set; }

            [JsonProperty("complemento")]
            public string Complemento { get; set; }

            [JsonProperty("bairro")]
            public string Bairro { get; set; }

            [JsonProperty("localidade")]
            public string Localidade { get; set; }

            [JsonProperty("uf")]
            public string Uf { get; set; }

            [JsonProperty("ibge")]
            public string Ibge { get; set; }

            [JsonProperty("gia")]
            public string Gia { get; set; }

            [JsonProperty("ddd")]
            public string Ddd { get; set; }

            [JsonProperty("siafi")]
            public string Siafi { get; set; }

            [JsonProperty("erro")]
            public string Erro { get; set; }
        }
        */

        private void SetarBordaGridView()
        {
            gdvCliente.Attributes.Add(UtilitarioBLL.ATRIBUTO_BORDER_COLOR, UtilitarioBLL.BORDER_COLOR);
        }

        private void VisualizacaoPorTipoCliente(int tipoClienteId)
        {
            if (tipoClienteId == UtilitarioBLL.TipoCliente.PessoaFisica.GetHashCode())
            {
                lblNome_.Text = "Nome";
                txtNome.Visible = true;
                txtNomeFantasia.Visible = false;
                lblRazaoSocial_.Visible = false;
                txtRazaoSocial.Visible = false;
                lblCpfCnpj_.Text = "CPF";
                txtCpf.Visible = true;
                lblDataNascimento_.Visible = true;
                txtDataNascimento.Visible = true;
                txtCnpj.Visible = false;
            }
            else if (tipoClienteId == UtilitarioBLL.TipoCliente.PessoaJuridica.GetHashCode())
            {
                lblNome_.Text = "Nome Fantasia";
                txtNome.Visible = false;
                txtNomeFantasia.Visible = true;
                lblRazaoSocial_.Visible = true;
                txtRazaoSocial.Visible = true;
                lblCpfCnpj_.Text = "CNPJ";
                txtCpf.Visible = false;
                lblDataNascimento_.Visible = false;
                txtDataNascimento.Visible = false;
                txtCnpj.Visible = true;
            }
            LimparFormulario();
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
            lblTopo.Text = "CONSULTA - CLIENTE";

            if
            (
                usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()
                || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode()
                || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Logistica.GetHashCode()
                || usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Vendedor.GetHashCode()
            )
            {
                ckbClienteID.Visible = true;
                lblClienteID.Visible = true;
                txtClienteID.Visible = true;
                imbCadastrar.Visible = true;
            }
            else
            {
                ckbClienteID.Visible = false;
                lblClienteID.Visible = false;
                txtClienteID.Visible = false;
                imbCadastrar.Visible = true;

                if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode())
                {
                    imbCadastrar.Visible = false;
                }
            }

            VisualizacaoPorTipoCliente(Convert.ToInt32(rblTipoCliente.SelectedValue));
        }
    }
}