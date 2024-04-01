using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    [Serializable()]
    public class NotaFiscalXML
    {
        public System.IO.Stream InputStream { get; set; }
        public string ContentType { get; set; }
    }

    [Serializable()]
    public class NotaFiscal
    {
        // Fields
        private DateTime _dataNotaFiscal;
        private int _lojaId;
        private int _notaFiscalId;
        private int _pedidoMaeId;
        private long _produtoId;
        private short _quantidade;
        private int _sistemaId;
        public List<NotaFiscalXML> NotaFiscalXML = new List<NotaFiscalXML>();
        public DAO.Produto Produto { get; set; }
        public string NomeLoja { get; set; }

        // Methods
        public NotaFiscal()
        {

        }

        public NotaFiscal(int notaFiscalId, int pedidoMaeId, int lojaId, long produtoId, short quantidade, DateTime dataNotaFiscal, int sistemaId)
        {
            this.NotaFiscalID = notaFiscalId;
            this.PedidoMaeID = pedidoMaeId;
            this.LojaID = lojaId;
            this.ProdutoID = produtoId;
            this.Quantidade = quantidade;
            this.DataNotaFiscal = dataNotaFiscal;
            this.SistemaID = sistemaId;
        }

        // Properties
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

        public int PedidoMaeID
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
