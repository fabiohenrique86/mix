using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Transactions;

namespace BLL
{
    public class NotaFiscal
    {
        /// <summary>
        /// Valida os campos obrigatórios
        /// </summary>
        /// <param name="notaFiscalDAO"></param>
        private void ValidarImportar(DAO.NotaFiscal notaFiscal)
        {
            if (notaFiscal.NotaFiscalXML == null || notaFiscal.NotaFiscalXML.Count() == 0)
            {
                throw new ApplicationException("Arquivo não informado.");
            }

            if (notaFiscal.NotaFiscalXML.Where(x => x.ContentType.Equals("application/octet-stream")).Count() > 0)
            {
                throw new ApplicationException("Arquivo não informado.");
            }

            if (notaFiscal.NotaFiscalXML.Where(x => !x.ContentType.Equals("text/xml")).Count() > 0)
            {
                throw new ApplicationException("Algum dos arquivos informados não possui extensão XML.");
            }
        }

        /// <summary>
        /// Valida os campos obrigatórios
        /// </summary>
        /// <param name="notaFiscalDAO"></param>
        private void ValidarIncluir(DAO.NotaFiscal notaFiscalDAO)
        {
            if (notaFiscalDAO.NotaFiscalID <= 0)
            {
                throw new ApplicationException("NotaFiscalID não informado.");
            }

            if (notaFiscalDAO.LojaID <= 0)
            {
                throw new ApplicationException("LojaID não informado.");
            }

            if (notaFiscalDAO.PedidoMaeID <= 0)
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

        public void ValidarObter(DAO.NotaFiscal notaFiscalDAO)
        {
            if (notaFiscalDAO.NotaFiscalID <= 0 && notaFiscalDAO.PedidoMaeID <= 0)
            {
                throw new ApplicationException("Informe o campo NotaFiscalID ou PedidoMaeID para pesquisar.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notaFiscalDAO"></param>
        /// <returns></returns>
        public bool Obter(DAO.NotaFiscal notaFiscalDAO)
        {
            bool retorno = false;

            DAL.NotaFiscal notaFiscalDAL = new DAL.NotaFiscal();

            retorno = notaFiscalDAL.Listar(notaFiscalDAO.NotaFiscalID, notaFiscalDAO.PedidoMaeID, notaFiscalDAO.SistemaID);

            return retorno;
        }

        /// <summary>
        /// Importa nota fiscal via XML
        /// </summary>
        /// <param name="notaFiscal"></param>
        public void Importar(DAO.NotaFiscal notaFiscalDAO)
        {
            this.ValidarImportar(notaFiscalDAO);

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (var notaFiscalXML in notaFiscalDAO.NotaFiscalXML)
                    {
                        XmlDocument xml = new XmlDocument();

                        xml.Load(notaFiscalXML.InputStream);

                        // Cria um namespace para poder ler os filhos do xml
                        //var manager = new XmlNamespaceManager(xml.NameTable);
                        //manager.AddNamespace("ns", "http://www.portalfiscal.inf.br/nfe");
                        //var nodes = xml.DocumentElement.SelectNodes("/ns:nfeProc/ns:NFe/ns:infNFe", manager);

                        XmlElement root = xml.DocumentElement;

                        // Retira o namespace do xml, pois com o ns não é possível ler os filhos
                        xml.DocumentElement.InnerXml = root.InnerXml.Replace("http://www.portalfiscal.inf.br/nfe", "");

                        // Obtém o campo notaFiscalId
                        string notaFiscalId = root.SelectSingleNode("NFe/infNFe/ide/nNF").InnerText;

                        if (string.IsNullOrEmpty(notaFiscalId))
                        {
                            throw new ApplicationException("Nota Fiscal não possui campo de código da nota fiscal.");
                        }

                        // Obtém o campo infAdFisco para obter o campo pedidoMaeId
                        string infAdFisco = root.SelectSingleNode("NFe/infNFe/infAdic/infAdFisco").InnerText;   // <infAdFisco> Pedido: 567554
                        if (string.IsNullOrEmpty(infAdFisco))
                        {
                            throw new ApplicationException("Nota Fiscal não possui campo de código pedido mãe.");
                        }
                        string[] infAdFiscoList = infAdFisco.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        int index = Array.IndexOf(infAdFiscoList, "Pedido:");
                        var pedidoMae = infAdFiscoList.GetValue(index + 1);
                        if (pedidoMae == null)
                        {
                            throw new ApplicationException("Nota Fiscal não possui campo de código pedido mãe.");
                        }
                        string pedidoMaeId = pedidoMae.ToString();
                        if (string.IsNullOrEmpty(pedidoMaeId))
                        {
                            throw new ApplicationException("Nota Fiscal não possui campo de código pedido mãe.");
                        }
                        notaFiscalDAO.PedidoMaeID = Convert.ToInt32(pedidoMaeId);

                        // Obtém o campo lista de produtos
                        var listaProdutos = root.SelectNodes("NFe/infNFe/det");

                        if (listaProdutos.Count == 0)
                        {
                            throw new ApplicationException("Nota Fiscal não possui campo de produtos.");
                        }

                        notaFiscalDAO.NotaFiscalID = Convert.ToInt32(notaFiscalId);    // Número da nota fiscal

                        // Verifica se a nota fiscal já foi cadastrada
                        bool notaFiscalIdCheck = this.Obter(notaFiscalDAO);

                        if (notaFiscalIdCheck)
                        {
                            throw new ApplicationException(string.Format("Nota Fiscal {0} já cadastrada.", notaFiscalId));
                        }

                        // Verifica se o pedido mãe já foi cadastrado
                        bool pedidoMaeIdCheck = this.Obter(notaFiscalDAO);

                        if (pedidoMaeIdCheck)
                        {
                            throw new ApplicationException(string.Format("Pedido Mãe {0} já cadastrado.", pedidoMaeId));
                        }

                        string cnpj = string.Empty;

                        // 2 = Paulo
                        if (notaFiscalDAO.SistemaID == 2)
                        {
                            cnpj = "00000000000000"; // CNPJ do deposito -> cadastrar sempre para o deposito
                        }
                        else
                        {
                            cnpj = root.SelectSingleNode("NFe/infNFe/dest/CNPJ").InnerText; // Obtém o campo CNPJ da nf
                        }

                        BLL.Loja lojaBLL = new BLL.Loja();

                        // Verifica se a loja existe no mix
                        DAO.Loja lojaDAO = lojaBLL.Obter(new DAO.Loja() { CNPJ = cnpj, SistemaID = notaFiscalDAO.SistemaID });

                        if (lojaDAO == null)
                        {
                            throw new ApplicationException(string.Format("Loja com CNPJ {0} não cadastrada.", cnpj));
                        }

                        notaFiscalDAO.LojaID = lojaDAO.LojaID;                         // Seta o id da loja cadastrada no sistema mix
                        notaFiscalDAO.NomeLoja = lojaDAO.Nome;                         // Seta o nome do loja cadastrada no sistema mix                
                        notaFiscalDAO.DataNotaFiscal = DateTime.Now.Date;

                        foreach (XmlNode xmlNode in listaProdutos)
                        {
                            // Obtém o campo produtoId
                            string produtoId = xmlNode.SelectSingleNode("prod/cProd").InnerText;

                            // Obtém o campo descricao
                            string descricao = xmlNode.SelectSingleNode("prod/xProd").InnerText;

                            if (string.IsNullOrEmpty(produtoId))
                            {
                                throw new ApplicationException("Nota Fiscal não possui produto.");
                            }

                            // Obtém o campo quantidade
                            string quantidade = xmlNode.SelectSingleNode("prod/qCom").InnerText;

                            if (string.IsNullOrEmpty(quantidade))
                            {
                                throw new ApplicationException(string.Format("Nota Fiscal não possui quantidade do produto {0}.", produtoId));
                            }

                            notaFiscalDAO.Produto = new DAO.Produto()
                            {
                                ProdutoID = Convert.ToInt64(produtoId),
                                Descricao = descricao,
                                Quantidade = Convert.ToInt16(Math.Floor(Convert.ToDecimal(quantidade, new System.Globalization.CultureInfo("en-US").NumberFormat))),
                                LojaID = lojaDAO.LojaID,
                                SistemaID = notaFiscalDAO.SistemaID
                            };

                            this.Incluir(notaFiscalDAO);
                        }
                    }

                    scope.Complete();
                }
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
        public void Incluir(DAO.NotaFiscal notaFiscalDAO)
        {
            this.ValidarIncluir(notaFiscalDAO);

            DAL.NotaFiscal notaFiscalDAL = new DAL.NotaFiscal();
            BLL.Produto produtoBLL = new BLL.Produto();

            bool produtoExisteNaLoja = produtoBLL.ExisteNaLoja(notaFiscalDAO.Produto);

            if (!produtoExisteNaLoja)
            {
                throw new ApplicationException(string.Format("Produto {0} não está cadastrado na loja {1}.", notaFiscalDAO.Produto.ProdutoID, notaFiscalDAO.NomeLoja));
            }

            notaFiscalDAL.Inserir(notaFiscalDAO);
        }
    }
}
