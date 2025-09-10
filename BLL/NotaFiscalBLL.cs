using DAL;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Xml;

namespace BLL
{
    public class NotaFiscalBLL
    {
        private NotaFiscalDAL notaFiscalDAL;
        private ProdutoBLL produtoBLL;
        private ProdutoDAL produtoDAL;

        public NotaFiscalBLL()
        {
            notaFiscalDAL = new NotaFiscalDAL();
            produtoBLL = new ProdutoBLL();
            produtoDAL = new ProdutoDAL();
        }

        /// <summary>
        /// Valida os campos obrigatórios
        /// </summary>
        /// <param name="notaFiscalDAO"></param>
        private void ValidarImportar(NotaFiscalDAO notaFiscalDao)
        {
            if (notaFiscalDao.NotasFiscaisXML == null || notaFiscalDao.NotasFiscaisXML.Count() <= 0)
            {
                throw new ApplicationException("Arquivo não informado.");
            }

            if (notaFiscalDao.NotasFiscaisXML.Count(x => x.ContentType.Equals("application/octet-stream")) > 0)
            {
                throw new ApplicationException("Arquivo não informado.");
            }

            if (notaFiscalDao.NotasFiscaisXML.Count(x => !x.ContentType.Equals("text/xml")) > 0)
            {
                throw new ApplicationException("Algum dos arquivos informados não possui extensão XML.");
            }
        }

        /// <summary>
        /// Valida os campos obrigatórios
        /// </summary>
        /// <param name="notaFiscalDAO"></param>
        private void ValidarIncluir(NotaFiscalDAO notaFiscalDAO)
        {
            if (notaFiscalDAO.NotaFiscalID <= 0)
            {
                throw new ApplicationException("NotaFiscalID não informado.");
            }

            if (notaFiscalDAO.LojaID <= 0)
            {
                throw new ApplicationException("LojaID não informado.");
            }

            if (string.IsNullOrEmpty(notaFiscalDAO.PedidoMaeID))
            {
                throw new ApplicationException("PedidoMaeID não informado.");
            }

            if (notaFiscalDAO.DataNotaFiscal == DateTime.MinValue)
            {
                throw new ApplicationException("DataNotaFiscal não informado.");
            }

            if (notaFiscalDAO.SistemaID <= 0)
            {
                throw new ApplicationException("SistemaID não informado.");
            }

            if (notaFiscalDAO.Produto == null)
            {
                throw new ApplicationException("Produto não informado.");
            }
        }

        public void ValidarObter(NotaFiscalDAO notaFiscalDAO)
        {
            if (notaFiscalDAO.NotaFiscalID <= 0 && string.IsNullOrEmpty(notaFiscalDAO.PedidoMaeID))
            {
                throw new ApplicationException("Informe o campo NotaFiscalID ou PedidoMaeID para pesquisar.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notaFiscalDAO"></param>
        /// <returns></returns>
        public bool Obter(NotaFiscalDAO notaFiscalDAO)
        {
            bool retorno = false;
            NotaFiscalDAL notaFiscalDAL = new NotaFiscalDAL();
            retorno = notaFiscalDAL.Listar(notaFiscalDAO);
            return retorno;
        }

        public void ImportarArquivoColeta(ProdutoDAO produtoDAO, List<NotaFiscalDAO> notasFiscaisDAO, out string mensagem, out int quantidadeDiferente, out int sobMedida, out bool geraOcorrencia)
        {
            mensagem = string.Empty;
            quantidadeDiferente = 0;
            sobMedida = 0;
            geraOcorrencia = false;
            var produtoEncontrado = false;            

            try
            {
                ValidarImportarArquivoColeta(produtoDAO, notasFiscaisDAO);

                var quantidadeNFe = 0;

                // loop nas notas fiscais
                foreach (var notaFiscalDAO in notasFiscaisDAO)
                {
                    // procura o produto do arquivo de coleta nas notas fiscais
                    var rows = notaFiscalDAL.ListarProduto(notaFiscalDAO.NotaFiscalID, produtoDAO.SistemaID, produtoDAO.ProdutoID).Tables[0].Rows;

                    if (rows == null || rows.Count <= 0) { continue; }
                                        
                    // loop em cada produto
                    foreach (System.Data.DataRow item in rows) { quantidadeNFe += Convert.ToInt32(item["Quantidade"]); }

                    produtoEncontrado = true;
                }

                if (produtoEncontrado)
                {
                    if (produtoDAO.Quantidade < quantidadeNFe)
                    {
                        mensagem = string.Format("Produto: {0} | Arquivo de coleta: {1} | NFes: {2}", produtoDAO.ProdutoID, produtoDAO.Quantidade, quantidadeNFe);
                        quantidadeDiferente = produtoDAO.Quantidade.GetValueOrDefault() - quantidadeNFe;
                        geraOcorrencia = true;
                    }                    
                }
                else
                {
                    sobMedida = 1;
                    mensagem = string.Format("Produto {0} não encontrado nas NFes informadas", produtoDAO.ProdutoID);
                    geraOcorrencia = false;
                }
            }
            catch (ApplicationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ValidarImportarArquivoColeta(ProdutoDAO produtoDAO, List<NotaFiscalDAO> notaFiscalDAO)
        {
            if (produtoDAO == null)
            {
                throw new ApplicationException("Produto não encontrado no arquivo de coleta.");
            }

            if (produtoDAO.ProdutoID <= 0)
            {
                throw new ApplicationException("Código do produto não encontrado no arquivo de coleta.");
            }

            if (notaFiscalDAO == null || notaFiscalDAO.Count() <= 0)
            {
                throw new ApplicationException("Nota Fiscal não informada");
            }
        }

        /// <summary>
        /// Importa nota fiscal via XML
        /// </summary>
        /// <param name="notaFiscal"></param>
        public List<string> Importar(NotaFiscalDAO notaFiscalDAO, bool validarPedidoMae, bool importarParaDeposito)
        {
            var retorno = new List<string>();
            //var pedidoMaeBLL = new PedidoMaeBLL();
            var lojaBLL = new LojaBLL();

            ValidarImportar(notaFiscalDAO);

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (var notaFiscalXML in notaFiscalDAO.NotasFiscaisXML)
                    {
                        var xml = new XmlDocument();
                        XmlElement root = null;

                        if (notaFiscalXML.InputStream != null)
                        {
                            xml.Load(notaFiscalXML.InputStream);
                        }
                        else
                        {
                            xml.LoadXml(notaFiscalXML.XML);
                        }

                        root = xml.DocumentElement;

                        // Retira o namespace do xml, pois com o ns não é possível ler os filhos
                        xml.DocumentElement.InnerXml = root.InnerXml.Replace("http://www.portalfiscal.inf.br/nfe", "");

                        // Obtém o campo notaFiscalId
                        string notaFiscalId = root.SelectSingleNode("NFe/infNFe/ide/nNF").InnerText;
                        if (string.IsNullOrEmpty(notaFiscalId))
                        {
                            retorno.Add("Nota Fiscal não possui campo de código da nota fiscal.");
                            continue;
                        }

                        // Obtém o campo infAdFisco para obter o campo pedidoMaeId
                        string infAdFisco = root.SelectSingleNode("NFe/infNFe/infAdic/infAdFisco").InnerText;   // <infAdFisco> Pedido: 567554
                        if (string.IsNullOrEmpty(infAdFisco))
                        {
                            retorno.Add(string.Format("Nota Fiscal {0} não possui pedido mãe.", notaFiscalId));
                            continue;
                        }
                        string[] infAdFiscoList = infAdFisco.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        int index = Array.IndexOf(infAdFiscoList, "Pedido:");
                        var pedidoMae = infAdFiscoList.GetValue(index + 1);
                        if (pedidoMae == null)
                        {
                            retorno.Add(string.Format("Nota Fiscal {0} não possui pedido mãe.", notaFiscalId));
                            continue;
                        }

                        string pedidoMaeId = pedidoMae.ToString();
                        if (string.IsNullOrEmpty(pedidoMaeId))
                        {
                            retorno.Add(string.Format("Nota Fiscal {0} não possui pedido mãe.", notaFiscalId));
                            continue;
                        }
                        notaFiscalDAO.PedidoMaeID = pedidoMaeId;
                        
                        //if (validarPedidoMae)
                        //{
                        //    var pedidoMaeDao = pedidoMaeBLL.Listar(new PedidoMaeDAO() { PedidoMaeID = notaFiscalDAO.PedidoMaeID, SistemaID = notaFiscalDAO.SistemaID }).FirstOrDefault();

                        //    if (pedidoMaeDao == null)
                        //    {
                        //        retorno.Add(string.Format("Nota Fiscal {0} não possui nenhum pedido mãe cadastrado.", notaFiscalId));
                        //        continue;
                        //    }
                        //}

                        // Obtém o campo lista de produtos
                        var listaProdutos = root.SelectNodes("NFe/infNFe/det");
                        if (listaProdutos.Count == 0)
                        {
                            retorno.Add(string.Format("Nota Fiscal {0} não possui produtos.", notaFiscalId));
                            continue;
                        }

                        notaFiscalDAO.NotaFiscalID = Convert.ToInt32(notaFiscalId);    // Número da nota fiscal

                        // Verifica se a nota fiscal já foi cadastrada
                        bool notaFiscalIdCheck = Obter(notaFiscalDAO);
                        if (notaFiscalIdCheck)
                        {
                            retorno.Add(string.Format("Nota Fiscal {0} já cadastrada.", notaFiscalId));
                            continue;
                        }

                        // Verifica se o pedido mãe já foi cadastrado
                        bool pedidoMaeIdCheck = Obter(notaFiscalDAO);
                        if (pedidoMaeIdCheck)
                        {
                            retorno.Add(string.Format("Nota Fiscal {0} com pedido mãe {1} já foi cadastrado.", notaFiscalId, pedidoMaeId));
                            continue;
                        }

                        string cnpj = string.Empty;
                        if (importarParaDeposito)
                        {
                            cnpj = "00000000000000"; // CNPJ do deposito -> cadastrar sempre para o deposito
                        }
                        else
                        {
                            cnpj = root.SelectSingleNode("NFe/infNFe/dest/CNPJ").InnerText; // Obtém o campo CNPJ da nf
                        }

                        // Verifica se a loja existe no mix                        
                        var lojaDAO = lojaBLL.Obter(new LojaDAO() { CNPJ = cnpj, SistemaID = notaFiscalDAO.SistemaID });
                        if (lojaDAO == null)
                        {
                            retorno.Add(string.Format("Loja com CNPJ {0} não cadastrada da Nota Fiscal {1}.", cnpj, notaFiscalId));
                            continue;
                        }

                        notaFiscalDAO.LojaID = lojaDAO.LojaID;  // Seta o id da loja cadastrada no sistema mix
                        notaFiscalDAO.NomeLoja = lojaDAO.Nome;  // Seta o nome do loja cadastrada no sistema mix                
                        notaFiscalDAO.DataNotaFiscal = DateTime.Now.Date;

                        foreach (XmlNode xmlNode in listaProdutos)
                        {
                            // Obtém o campo produtoId
                            string produtoId = xmlNode.SelectSingleNode("prod/cProd").InnerText;

                            // Obtém o campo descricao
                            string descricao = xmlNode.SelectSingleNode("prod/xProd").InnerText;

                            if (string.IsNullOrEmpty(produtoId))
                            {
                                retorno.Add(string.Format("Nota Fiscal {0} não possui produto.", notaFiscalId));
                                continue;
                            }

                            // Obtém o campo quantidade
                            string quantidade = xmlNode.SelectSingleNode("prod/qCom").InnerText;

                            if (string.IsNullOrEmpty(quantidade))
                            {
                                retorno.Add(string.Format("Nota Fiscal {0} não possui quantidade do produto {1}.", notaFiscalId, produtoId));
                                continue;
                            }

                            notaFiscalDAO.Produto = new ProdutoDAO()
                            {
                                ProdutoID = Convert.ToInt64(produtoId),
                                Descricao = descricao,
                                Quantidade = Convert.ToInt16(Math.Floor(Convert.ToDecimal(quantidade, new System.Globalization.CultureInfo("en-US").NumberFormat))),
                                LojaID = lojaDAO.LojaID,
                                SistemaID = notaFiscalDAO.SistemaID
                            };
                            
                            if (!produtoBLL.ExisteNaLoja(notaFiscalDAO.Produto))
                            {
                                retorno.Add(string.Format(string.Format("Produto {0} da Nota Fiscal {1} não está cadastrado na loja {2}.", notaFiscalDAO.Produto.ProdutoID, notaFiscalDAO.NotaFiscalID, notaFiscalDAO.NomeLoja)));
                                continue;
                            }

                            Incluir(notaFiscalDAO);
                        }
                    }

                    scope.Complete();
                }

                return retorno;
            }
            catch (ApplicationException ex)
            {
                retorno.Add(ex.Message);
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inclui um registro na tabela NotaFiscal
        /// </summary>
        /// <param name="notaFiscal"></param>
        public void Incluir(NotaFiscalDAO notaFiscalDAO)
        {
            ValidarIncluir(notaFiscalDAO);

            var notaFiscalDAL = new NotaFiscalDAL();
            //ProdutoBLL produtoBLL = new ProdutoBLL();

            //bool produtoExisteNaLoja = produtoBLL.ExisteNaLoja(notaFiscalDAO.Produto);

            //if (!produtoExisteNaLoja)
            //{
            //    throw new ApplicationException(string.Format("Produto {0} da Nota Fiscal {1} não está cadastrado na loja {2}.", notaFiscalDAO.Produto.ProdutoID, notaFiscalDAO.NotaFiscalID, notaFiscalDAO.NomeLoja));
            //}

            notaFiscalDAL.Inserir(notaFiscalDAO);
        }

        public List<string> ImportarArquivoCarga(List<NotaFiscalDAO> notasFiscaisDao)
        {
            var listaRetorno = new List<string>();

            ValidarImportarArquivoCarga(notasFiscaisDao);

            foreach (var item in notasFiscaisDao)
            {
                try
                {
                    notaFiscalDAL.Inserir(item);
                    listaRetorno.Add("Produto " + item.Produto.ProdutoID + " da nota fiscal " + item.NotaFiscalID  + " atualizado com sucesso");
                }
                catch (Exception ex)
                {
                    listaRetorno.Add("Produto " + item.Produto.ProdutoID + " da nota fiscal " + item.NotaFiscalID + " não foi atualizado");
                }
            }

            return listaRetorno;
        }

        private void ValidarImportarArquivoCarga(List<NotaFiscalDAO> notasFiscaisDao)
        {
            if (notasFiscaisDao == null || !notasFiscaisDao.Any())
                throw new ApplicationException("Nota Fiscal é obrigatório");

            if (notasFiscaisDao.Any(x => x.DataNotaFiscal == DateTime.MinValue))
                throw new ApplicationException("Data da Nota Fiscal não encontrada no arquivo de carga.");

            if (notasFiscaisDao.Any(x => x.DataNotaFiscal == DateTime.MinValue))
                throw new ApplicationException("Data da Nota Fiscal não encontrada no arquivo de carga.");

            if (notasFiscaisDao.Any(x => x.Produto.ProdutoID <= 0))
                throw new ApplicationException("Produto não encontrado no arquivo de carga.");

            if (notasFiscaisDao.Any(x => x.Produto.Quantidade < 0))
                throw new ApplicationException("Quantidade Recebida do Produto não pode ser negativa no arquivo de carga.");

            if (notasFiscaisDao.Any(x => x.NotaFiscalID <= 0))
                throw new ApplicationException("Nota Fiscal não encontrada no arquivo de carga.");

            // verifica se algum produto da lista não está cadastrado
            foreach (var item in notasFiscaisDao)
            {
                var produtoExiste = produtoDAL.ExisteNaLoja(item.Produto.ProdutoID, string.Empty, item.SistemaID);
                
                if (!produtoExiste)
                    throw new ApplicationException($"Produto '{item.Produto.ProdutoID}' não está cadastrado");
            }
        }
    }
}
