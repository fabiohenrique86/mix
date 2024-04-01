using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Configuration;
using BLL;
using DAO;
using Dropbox.Api;
using System.Threading.Tasks;
using System.IO;
using DAL;

namespace Site
{
    public partial class NotaFiscalAutomatica : Page
    {
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
                        CarregarDropDownListLoja();
                        VisualizarFormulario();
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

        private void CarregarDropDownListLoja()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            if ((Session["dsDropDownListLoja"] == null) || (Session["bdLoja"] != null))
            {
                Session["dsDropDownListLoja"] = new LojaDAL().ListarDropDownList(usuarioSessao.LojaID, usuarioSessao.SistemaID);
                Session["bdLoja"] = null;
            }
        }

        private void VisualizarFormulario()
        {
            BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

            //if (
            //    (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Administrador.GetHashCode()) ||
            //    (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Estoquista.GetHashCode())
            //    )
            //{
            //    ddlLoja.Enabled = true;
            //    imbCadastrar.Visible = true;
            //}
            //else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Conferentista.GetHashCode())
            //{
            //    ddlLoja.Enabled = true;
            //    imbCadastrar.Visible = false;
            //}
            //else if (usuarioSessao.TipoUsuarioID == UtilitarioBLL.TipoUsuario.Gerente.GetHashCode())
            //{
            //    ddlLoja.Enabled = false;
            //    ddlLoja.SelectedValue = usuarioSessao.LojaID.ToString();
            //    imbCadastrar.Visible = true;
            //}
        }

        static async Task ImportarDropbox(DropboxClient dbx, int sistemaId)
        {
            List<string> mensagens = new List<string>();
            string mensagem = string.Empty;
            NotaFiscalBLL notaFiscalBLL = new NotaFiscalBLL();
            NotaFiscalDAO notaFiscalDAO = new NotaFiscalDAO();

            notaFiscalDAO.Estoque = 0;
            notaFiscalDAO.SistemaID = sistemaId;

            // lista a pasta "principal"
            var pastas = await dbx.Files.ListFolderAsync(string.Empty);

            // lista os arquivos ".xml" por ordem decrescente de data de modificação
            var arquivos = pastas.Entries.Where(x => x.IsFile && x.Name.Contains(".xml")).OrderByDescending(x => x.AsFile.ClientModified).Take(10);

            // adiciona cada arquivo no sistema mix
            foreach (var item in arquivos)
            {
                try
                {
                    using (var response = await dbx.Files.DownloadAsync(item.PathLower))
                    {
                        var xml = await response.GetContentAsStringAsync();

                        NotaFiscalXML notaFiscalXML = new NotaFiscalXML()
                        {
                            ContentType = "text/xml",
                            InputStream = null,
                            XML = xml
                        };

                        notaFiscalDAO.NotasFiscaisXML.Clear();
                        notaFiscalDAO.NotasFiscaisXML.Add(notaFiscalXML);

                        // importa o xml no sistema mix
                        var msgs = notaFiscalBLL.Importar(notaFiscalDAO, false, true);

                        // adiciona a msg de erro a lista de msgs
                        mensagens.Add(msgs.FirstOrDefault());

                        // se não deu erro, move o arquivo para pasta de "lidos"
                        if (msgs == null || msgs.Count() <= 0)
                        {
                            await dbx.Files.MoveAsync(item.PathLower, "/lidos/" + item.Name);
                            mensagens.Add(string.Format("Arquivo {0} importado com sucesso.", item.Name));
                        }
                    }
                }
                catch (Exception ex)
                {
                    mensagens.Add(ex.Message);
                }
            }

            mensagem = string.Join(@"\r\n", mensagens.ToArray());

            if (mensagens != null && mensagens.Count() > 0)
            {
                throw new ApplicationException(mensagem);
            }
        }

        protected void imbImportarXML_Click(object sender, ImageClickEventArgs e)
        {
            string mensagem = string.Empty;

            try
            {
                var access_token = ConfigurationManager.AppSettings["DropBoxAccessToken"].ToString();
                var dropboxClient = new DropboxClient(access_token);
                var sistemaId = new BLL.Modelo.Usuario(Session["Usuario"]).SistemaID;

                ImportarDropbox(dropboxClient, sistemaId).Wait();
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ImportarXML", "alert('" + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ImportarXML", "alert('" + ex.InnerException.Message + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ImportarXML", "alert('Ocorreu um erro ao tentar o arquivo XML. O arquivo não foi importado.');", true);
                }
            }
        }

        protected void imbImportarColeta_Click(object sender, ImageClickEventArgs e)
        {
            List<dynamic> produto = new List<dynamic>();

            try
            {
                NotaFiscalBLL notaFiscalBLL = new NotaFiscalBLL();
                List<string> arquivoColeta = null;
                List<NotaFiscalDAO> notasFiscaisDAO = new List<NotaFiscalDAO>();

                var files = Request.Files;

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[0];
                    if (file.ContentLength <= 0)
                    {
                        throw new ApplicationException("Informe o arquivo de coleta");
                    }
                }

                if (string.IsNullOrEmpty(txtNotaFiscalID.Text))
                {
                    throw new ApplicationException("Informe as Notas Fiscais");
                }

                var notas = txtNotaFiscalID.Text.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (var nota in notas)
                {
                    notasFiscaisDAO.Add(new NotaFiscalDAO() { NotaFiscalID = Convert.ToInt32(nota) });
                }

                using (StreamReader sr = new StreamReader(files[0].InputStream))
                {
                    var a = sr.ReadToEnd();
                    // armazena o arquivo de coleta no hidden field da tela
                    hdfArquivoColeta.Value = a;
                    arquivoColeta = a.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();
                }

                foreach (var linhaArquivoColeta in arquivoColeta)
                {
                    var campos = linhaArquivoColeta.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    //var dataColeta = campos[0];
                    //var codigoProduto = campos[1];
                    //var descricaoProduto = campos[2];
                    //var medidaProduto = campos[3];
                    //var quantidadeTotalProduto = campos[4];
                    //var quantidadeRealProduto = campos[5];

                    ProdutoDAO produtoDAO = new ProdutoDAO();

                    produtoDAO.ProdutoID = Convert.ToInt64(campos[1]);
                    produtoDAO.Quantidade = Convert.ToInt16(campos[5]);
                    produtoDAO.SistemaID = new BLL.Modelo.Usuario(Session["Usuario"]).SistemaID;

                    var mensagem = string.Empty;
                    var quantidadeDiferente = 0;
                    var sobMedida = 0;
                    var geraOcorrencia = false;

                    notaFiscalBLL.ImportarArquivoColeta(produtoDAO, notasFiscaisDAO, out mensagem, out quantidadeDiferente, out sobMedida, out geraOcorrencia);

                    if (!string.IsNullOrEmpty(mensagem))
                    {
                        produto.Add(new { ProdutoID = produtoDAO.ProdutoID, Quantidade = quantidadeDiferente, Mensagem = mensagem, SobMedida = sobMedida });
                    }
                }

                if (produto != null && produto.Count > 0)
                {
                    mpeNotaFiscal.Show();
                    System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string sJSON = oSerializer.Serialize(produto);
                    ScriptManager.RegisterStartupScript(this, GetType(), "ImportarXML", "modalOcorrencia(" + sJSON + ");", true);
                }
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ImportarXML", "alert('" + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ImportarXML", "alert('" + ex.InnerException.Message + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ImportarXML", "alert('Ocorreu um erro ao tentar o arquivo XML. O arquivo não foi importado.');", true);
                }
            }
        }
    }
}