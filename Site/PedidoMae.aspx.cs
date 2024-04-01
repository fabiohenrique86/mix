using BLL;
using DAO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site
{
    public partial class PedidoMae : Page
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

        protected void imbCadastrar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                PedidoMaeDAO pedidoMaeDao = new PedidoMaeDAO();
                PedidoMaeBLL pedidoMaeBLL = new PedidoMaeBLL();

                string pedidoMaeID = TxtPedidoMaeID.Text.Trim();

                pedidoMaeDao.PedidoMaeID = pedidoMaeID;
                pedidoMaeDao.DataCadastro = DateTime.Now;
                pedidoMaeDao.SistemaID = usuarioSessao.SistemaID;
                pedidoMaeDao.Origem = "M";

                for (int i = 0; i < Request.Form.AllKeys.Count(); i++)
                {
                    if (Request.Form["txtProdutoID_" + i] != null && Request.Form["txtQuantidade_" + i] != null)
                    {
                        long produtoId;
                        short quantidade;
                        string sobMedida = string.Empty;

                        long.TryParse(Request.Form.GetValues("txtProdutoID_" + i).FirstOrDefault(), out produtoId);
                        short.TryParse(Request.Form.GetValues("txtQuantidade_" + i).FirstOrDefault(), out quantidade);
                        sobMedida = Request.Form.GetValues("txtSobMedida_" + i).FirstOrDefault();

                        if (produtoId <= 0)
                        {
                            throw new ApplicationException(string.Format("Informe o produto {0}.", produtoId));
                        }

                        if (quantidade <= 0)
                        {
                            throw new ApplicationException(string.Format("Informe a quantidade do produto {0}.", produtoId));
                        }

                        pedidoMaeDao.PedidoMaeProdutoDAO.Add(new PedidoMaeProdutoDAO()
                        {
                            PedidoMaeID = pedidoMaeDao.PedidoMaeID,
                            ProdutoID = produtoId,
                            Quantidade = quantidade,
                            Medida = sobMedida,
                            SistemaID = usuarioSessao.SistemaID
                        });
                    }
                }

                var existe = pedidoMaeBLL.ListarByPedido(new PedidoMaeDAO() { PedidoMaeID = pedidoMaeDao.PedidoMaeID, SistemaID = pedidoMaeDao.SistemaID }).FirstOrDefault();

                if (existe != null)
                {
                    throw new ApplicationException(string.Format("PedidoMaeID {0} já cadastrado em {1}", pedidoMaeDao.PedidoMaeID, existe.DataCadastro.ToString("dd/MM/yyyy")));
                }

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    pedidoMaeBLL.Inserir(pedidoMaeDao, true);
                    scope.Complete();
                }

                rptPedidoMae.DataSource = pedidoMaeBLL.Listar(new PedidoMaeDAO() { SistemaID = usuarioSessao.SistemaID });
                rptPedidoMae.DataBind();

                LimparFormulario();
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
        }

        protected void imbConsultar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                var pedidoMaeDao = new PedidoMaeDAO();

                string pedidoMaeID = TxtPedidoMaeID.Text.Trim();

                pedidoMaeDao.PedidoMaeID = pedidoMaeID;
                pedidoMaeDao.SistemaID = usuarioSessao.SistemaID;

                DateTime dtCadastro;
                DateTime.TryParse(txtDataCadastro.Text, out dtCadastro);
                pedidoMaeDao.DataCadastro = dtCadastro.Date;

                pedidoMaeDao.Lincado = true;

                var pedidoMaeBLL = new PedidoMaeBLL();

                if (string.IsNullOrEmpty(pedidoMaeDao.PedidoMaeID) && pedidoMaeDao.DataCadastro == DateTime.MinValue)
                {
                    throw new ApplicationException("É necessário informar algum campo para pesquisar");
                }

                var pedidosMaeID = pedidoMaeBLL.Listar(pedidoMaeDao);

                if (pedidosMaeID == null || pedidosMaeID.Count <= 0)
                {
                    throw new ApplicationException("Não existem registros a serem exibidos com os parâmetros de pesquisa");
                }

                rptPedidoMae.DataSource = pedidosMaeID;
                rptPedidoMae.DataBind();
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
        }

        protected void imbExcluir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string pedidoMaeID = TxtPedidoMaeID.Text.Trim();

                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);
                PedidoMaeBLL pedidoMaeBLL = new PedidoMaeBLL();
                PedidoMaeDAO pedidoMaeDao = new PedidoMaeDAO();

                pedidoMaeDao.PedidoMaeID = pedidoMaeID;
                pedidoMaeDao.SistemaID = usuarioSessao.SistemaID;

                pedidoMaeBLL.Excluir(pedidoMaeDao);

                var pedidosMaeID = pedidoMaeBLL.Listar(new PedidoMaeDAO() { SistemaID = usuarioSessao.SistemaID });

                rptPedidoMae.DataSource = pedidosMaeID;
                rptPedidoMae.DataBind();

                LimparFormulario();
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
        }

        protected void imbImportar_Click(object sender, ImageClickEventArgs e)
        {
            var options = new ChromeOptions();
            options.AddArgument("disable-infobars");
            var driver = new ChromeDriver(options);

            try
            {
                BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                var produtoBLL = new ProdutoBLL();
                var pedidoMaeProdutoBLL = new PedidoMaeProdutoBLL();
                var pedidoMaeTrocaBLL = new PedidoMaeTrocaBLL();
                var pedidoMaeBLL = new PedidoMaeBLL();
                var pedidoMaeFilhoBLL = new PedidoMaeFilhoBLL();

                var IsPedidoMae = false;

                var pedidosMaeProdutoDAO = new List<PedidoMaeProdutoDAO>();
                var pedidosMaeTrocaDAO = new List<PedidoMaeTrocaDAO>();
                var pedidosMaeDAO = new List<PedidoMaeDAO>();

                var listaErros = new List<string>();

                // vai para o site
                driver.Navigate().GoToUrl(@"http://ortobom:orto2020@extranet.ortobom.com.br/src/login.php");

                // preenche os campos da pagina de login
                driver.FindElement(By.Name("login_username")).SendKeys(ConfigurationManager.AppSettings["LoginWebmail"]);
                driver.FindElement(By.Name("secretkey")).SendKeys(ConfigurationManager.AppSettings["SenhaWebmail"]);

                // clica no botão "login"
                driver.FindElement(By.Name("button")).Click();

                // navega até a pagina principal email
                driver.Navigate().GoToUrl("http://ortobom:orto2020@extranet.ortobom.com.br/src/right_main.php");

                // obtém o primeiro e-mail que tenha link com o "PASSED_ID" e o título "PEDIDO"
                var link_email = driver.FindElements(By.XPath("html/body/table/tbody/tr/td/table/tbody/tr/td/a")).FirstOrDefault(x => x.GetAttribute("href").Contains("passed_id") && x.Text.Contains("PEDIDO"));

                // verifica se existe algum e-mail válido
                if (link_email == null)
                {
                    return;
                }

                // navega até a página interna email
                driver.Navigate().GoToUrl(link_email.GetAttribute("href"));

                while (true)
                {
                    var guid = Guid.NewGuid().ToString();

                    // pega o text entre a tag <pre>
                    var corpoMensagem = Regex.Match(driver.PageSource, @"<pre>(.|\n)*?<\/pre>").Value.Replace("<pre>", string.Empty).Replace("</pre>", string.Empty);

                    // separa o conteúdo das linhas por "\n"
                    var linhasMensagem = Regex.Matches(corpoMensagem, ".+");

                    // varre a mensagem procurando os dados do produto e pedido mãe
                    for (int i = 7; i < linhasMensagem.Count; i++)
                    {
                        var l = linhasMensagem[i].Value;

                        // passa para o próxima se encontrar somente a quebra de linha "\r"
                        if (l == "\r")
                        {
                            continue;
                        }

                        // produto
                        try
                        {
                            var pedidoMaeProdutoDAO = new PedidoMaeProdutoDAO();

                            //pedidoMaeProdutoDAO.PedidoMaeID = Guid.NewGuid().ToString();
                            pedidoMaeProdutoDAO.ProdutoID = Convert.ToInt64(l.Substring(0, 20).Trim());
                            pedidoMaeProdutoDAO.Quantidade = Convert.ToInt16(l.Substring(20, 2).Trim());
                            pedidoMaeProdutoDAO.SistemaID = usuarioSessao.SistemaID;
                            pedidoMaeProdutoDAO.EmailID = guid;

                            pedidosMaeProdutoDAO.Add(pedidoMaeProdutoDAO);
                        }
                        catch (Exception ex)
                        {

                        }

                        // pedido troca
                        try
                        {
                            // verifica se é realmente uma troca
                            if (!l.Contains("TROCA"))
                            {
                                throw new Exception();
                            }

                            var pedidoMaeId = Convert.ToInt32(l.Substring(0, 11).Trim()).ToString();
                            PedidoMaeTrocaDAO pedidoMaeTrocaDAO = null;
                            var adicionar = true;

                            // verifica se já existe na lista de pedido mae troca o pedidoMaeId lido
                            if (pedidosMaeTrocaDAO.FirstOrDefault(x => x.PedidoMaeID == pedidoMaeId) == null)
                            {
                                pedidoMaeTrocaDAO = new PedidoMaeTrocaDAO()
                                {
                                    PedidoMaeID = pedidoMaeId,
                                    SistemaID = usuarioSessao.SistemaID
                                };
                            }
                            else
                            {
                                pedidoMaeTrocaDAO = pedidosMaeTrocaDAO.FirstOrDefault(x => x.PedidoMaeID == pedidoMaeId);
                                adicionar = false;
                            }

                            var pedidoMaeTrocaProdutoDao = new PedidoMaeTrocaProdutoDAO();
                            var descricao = l.Substring(17, 28).Trim();
                            var medida = l.Substring(45, 21).Trim();
                            var quantidade = Convert.ToInt16(l.Substring(11, 6).Trim());

                            // consulta o produto por descrição para obter o ProdutoID
                            var produtoDAO = produtoBLL.Listar(descricao, medida, usuarioSessao.SistemaID).FirstOrDefault();
                            if (produtoDAO == null)
                            {
                                throw new ApplicationException("Produto [" + descricao + "] de medida [" + medida + "] não encontrado no MiX");
                            }

                            pedidoMaeTrocaProdutoDao.ProdutoID = produtoDAO.ProdutoID;
                            pedidoMaeTrocaProdutoDao.Quantidade = quantidade;
                            pedidoMaeTrocaProdutoDao.SistemaID = usuarioSessao.SistemaID;

                            pedidoMaeTrocaDAO.PedidoMaeTrocaProdutosDAO.Add(pedidoMaeTrocaProdutoDao);

                            // verifica se deve adicionar um novo pedido mae troca
                            if (adicionar)
                            {
                                pedidosMaeTrocaDAO.Add(pedidoMaeTrocaDAO);
                            }
                        }
                        catch (ApplicationException ex)
                        {
                            listaErros.Add(ex.Message);
                        }
                        catch (Exception ex)
                        {

                        }

                        // pedido mãe
                        do
                        {
                            try
                            {
                                var pedidoMaeDAO = new PedidoMaeDAO()
                                {
                                    PedidoMaeID = Convert.ToInt64(l.Trim()).ToString(),
                                    DataCadastro = DateTime.Now,
                                    Origem = "E",
                                    SistemaID = usuarioSessao.SistemaID,
                                    EmailID = guid
                                };

                                i++;
                                l = linhasMensagem[i].Value;

                                var pedidoFilhos = l.Replace(".", "").Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                                // pedidos filho
                                foreach (var pedidoID in pedidoFilhos)
                                {
                                    pedidoMaeDAO.PedidoMaeFilhoDAO.Add(new PedidoMaeFilhoDAO()
                                    {
                                        PedidoMaeID = pedidoMaeDAO.PedidoMaeID,
                                        PedidoID = pedidoID.Trim(),
                                        SistemaID = usuarioSessao.SistemaID
                                    });
                                }

                                pedidosMaeDAO.Add(pedidoMaeDAO);

                                i++;
                                l = linhasMensagem[i].Value;

                                IsPedidoMae = true;
                            }
                            catch (Exception ex)
                            {
                                IsPedidoMae = false;
                            }
                        }
                        while (IsPedidoMae);
                    }

                    // verifica se é fim da caixa de e-mail
                    if (driver.FindElements(By.TagName("img")).Where(x => x.GetAttribute("src").Contains("forward_grey")).FirstOrDefault() != null)
                    {
                        break;
                    }

                    // avança para o próximo e-mail
                    driver.FindElements(By.TagName("img")).Where(x => x.GetAttribute("src").Contains("forward")).FirstOrDefault().Click();
                }

                if (listaErros != null && listaErros.Count() > 0)
                {
                    var erro = string.Empty;
                    foreach (var item in listaErros) { erro += item + "\r\n"; }
                    UtilitarioBLL.ExibirMensagemAjax(Page, "Importação não realizada. Ocorreram os erros abaixo: \r\n\r\n" + erro);
                    return;
                }

                // retira da lista os pedidos já lidos
                foreach (var item in pedidosMaeTrocaDAO.ToList())
                {
                    var existe = pedidoMaeTrocaBLL.Listar(item).FirstOrDefault();
                    if (existe != null)
                    {
                        pedidosMaeTrocaDAO.Remove(item);
                    }
                }

                // retira da lista os pedidos já lidos
                foreach (var pm in pedidosMaeDAO.ToList())
                {
                    var existePedidoMae = pedidoMaeBLL.ListarByPedido(pm).FirstOrDefault();
                    if (existePedidoMae != null)
                    {
                        pedidosMaeDAO.Remove(pm);
                    }
                         
                    foreach (var pmf in pm.PedidoMaeFilhoDAO.ToList())
                    {
                        var existePedidoMaeFilho = pedidoMaeFilhoBLL.Listar(pmf).FirstOrDefault();
                        if (existePedidoMaeFilho != null)
                        {
                            pm.PedidoMaeFilhoDAO.Remove(pmf);
                        }
                    }
                }

                // COMEÇO DA TRANSAÇÃO COM O BANCO DE DADOS
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 10, 0)))
                {
                    //// PedidoMaeProduto
                    //if (pedidosMaeProdutoDAO != null && pedidosMaeProdutoDAO.Count() > 0)
                    //{
                    //    foreach (var item in pedidosMaeProdutoDAO)
                    //    {
                    //        try
                    //        {
                    //            pedidoMaeProdutoBLL.Inserir(item);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            listaErros.Add(string.Format("Produto {0} não está cadastrado no MiX", item.ProdutoID));
                    //        }
                    //    }
                    //}

                    if (listaErros != null && listaErros.Count() > 0)
                    {
                        var erro = string.Empty;
                        foreach (var item in listaErros.Take(25)) { erro += item + "\r\n"; }
                        UtilitarioBLL.ExibirMensagemAjax(Page, "Importação não realizada. Ocorreram os erros abaixo: \r\n\r\n" + erro);
                        return;
                    }

                    // PedidoMaeTroca
                    if (pedidosMaeTrocaDAO != null && pedidosMaeTrocaDAO.Count() > 0)
                    {
                        foreach (var item in pedidosMaeTrocaDAO)
                        {
                            //var existe = pedidoMaeTrocaBLL.Listar(item).FirstOrDefault();
                            //if (existe == null)
                            //{
                            pedidoMaeTrocaBLL.Inserir(item);
                            //}
                        }
                    }

                    // PedidoMae
                    if (pedidosMaeDAO != null && pedidosMaeDAO.Count() > 0)
                    {
                        foreach (var pm in pedidosMaeDAO)
                        {
                            // PedidoMaeProduto
                            if (pedidosMaeProdutoDAO != null && pedidosMaeProdutoDAO.Count() > 0)
                            {
                                foreach (var pmp in pedidosMaeProdutoDAO.Where(x => x.EmailID == pm.EmailID))
                                {
                                    try
                                    {
                                        pmp.PedidoMaeID = pm.PedidoMaeID;
                                        pedidoMaeProdutoBLL.Inserir(pmp);
                                    }
                                    catch (Exception ex)
                                    {
                                        listaErros.Add(string.Format("Produto {0} não está cadastrado no MiX", pmp.ProdutoID));
                                    }
                                }
                            }

                            //var existePedidoMae = pedidoMaeBLL.Listar(item).FirstOrDefault();
                            //if (existePedidoMae == null)
                            //{
                            pedidoMaeBLL.Inserir(pm, false);
                            //}

                            // PedidoMaeFilho                            
                            foreach (var pmf in pm.PedidoMaeFilhoDAO)
                            {
                                //var existePedidoMaeFilho = pedidoMaeFilhoBLL.Listar(item2).FirstOrDefault();
                                //if (existePedidoMaeFilho == null)
                                //{
                                pedidoMaeFilhoBLL.Inserir(pmf);
                                //}
                            }
                        }
                    }

                    scope.Complete();
                }

                UtilitarioBLL.ExibirMensagemAjax(Page, "Foram importados com sucesso:"
                                                        + "\r\n\r\n PedidosMae: " + pedidosMaeDAO.Count()
                                                        + "\r\n PedidosMaeFilho: " + pedidosMaeDAO.Sum(x => x.PedidoMaeFilhoDAO.Count())
                                                        + "\r\n PedidosMaeProduto: " + pedidosMaeProdutoDAO.Count());
            }
            catch (ApplicationException ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            catch (Exception ex)
            {
                UtilitarioBLL.ExibirMensagemAjax(Page, ex.Message);
            }
            finally
            {
                driver.Quit();
            }
        }

        private void LimparFormulario()
        {
            TxtPedidoMaeID.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this, GetType(), "LimparGrid", "document.getElementById('ctl00_ContentPlaceHolder1_hdfProdutoValores').value = ''; clearGrid();", true);
        }

        protected void rptPedidoMae_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
                {
                    BLL.Modelo.Usuario usuarioSessao = new BLL.Modelo.Usuario(Session["Usuario"]);

                    string pedidoMaeId = ((Label)e.Item.FindControl("lblPedidoMaeID")).Text;

                    GridView gdvPedidoMaeProdutoAux = (GridView)e.Item.FindControl("gdvPedidoMaeProduto");

                    PedidoMaeProdutoDAO pedidoMaeProdutoDao = new PedidoMaeProdutoDAO();
                    PedidoMaeProdutoBLL pedidoMaeProdutoBLL = new PedidoMaeProdutoBLL();

                    pedidoMaeProdutoDao.PedidoMaeID = pedidoMaeId;
                    pedidoMaeProdutoDao.SistemaID = usuarioSessao.SistemaID;

                    var pedidos = pedidoMaeProdutoBLL.Listar(pedidoMaeProdutoDao);

                    gdvPedidoMaeProdutoAux.DataSource = pedidos;
                    gdvPedidoMaeProdutoAux.DataBind();
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
    }
}