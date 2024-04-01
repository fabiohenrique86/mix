using System;
using System.Collections.Generic;

namespace DAO
{
    [Serializable()]
    public class NotaFiscalXML
    {
        public System.IO.Stream InputStream { get; set; }
        public string ContentType { get; set; }
        public string XML { get; set; }
    }

    [Serializable()]
    public class NotaFiscalDAO
    {
        private DateTime _dataNotaFiscal;
        private int _lojaId;
        private int _notaFiscalId;
        private string _pedidoMaeId;
        private long _produtoId;
        private short _quantidade;
        private int _sistemaId;
        public List<NotaFiscalXML> NotasFiscaisXML = new List<NotaFiscalXML>();
        public ProdutoDAO Produto { get; set; }
        public string NomeLoja { get; set; }
        public int Estoque { get; set; }
        
        public NotaFiscalDAO()
        {

        }

        public NotaFiscalDAO(int notaFiscalId, string pedidoMaeId, int lojaId, long produtoId, short quantidade, DateTime dataNotaFiscal, int sistemaId)
        {
            NotaFiscalID = notaFiscalId;
            PedidoMaeID = pedidoMaeId;
            LojaID = lojaId;
            ProdutoID = produtoId;
            Quantidade = quantidade;
            DataNotaFiscal = dataNotaFiscal;
            SistemaID = sistemaId;
        }
        
        public DateTime DataNotaFiscal
        {
            get
            {
                return this._dataNotaFiscal;
            }
            set
            {
                this._dataNotaFiscal = value;
            }
        }

        public int LojaID
        {
            get
            {
                return this._lojaId;
            }
            set
            {
                this._lojaId = value;
            }
        }

        public int NotaFiscalID
        {
            get
            {
                return this._notaFiscalId;
            }
            set
            {
                this._notaFiscalId = value;
            }
        }

        public string PedidoMaeID
        {
            get
            {
                return this._pedidoMaeId;
            }
            set
            {
                this._pedidoMaeId = value;
            }
        }

        public long ProdutoID
        {
            get
            {
                return this._produtoId;
            }
            set
            {
                this._produtoId = value;
            }
        }

        public short Quantidade
        {
            get
            {
                return this._quantidade;
            }
            set
            {
                this._quantidade = value;
            }
        }

        public int SistemaID
        {
            get
            {
                return this._sistemaId;
            }
            set
            {
                this._sistemaId = value;
            }
        }

    }
}
