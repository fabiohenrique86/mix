using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    [Serializable()]
    public class Transferencia
    {
        // Fields
        private DateTime _dataTransferencia;
        private List<Produto> _listaProduto;
        private int _lojaDeId;
        private int _lojaParaId;
        private int _sistemaId;
        private int _transferenciaId;

        // Methods
        public Transferencia()
        {
        }

        public Transferencia(int transferenciaId, int lojaDeId, int lojaParaId, List<Produto> listaProduto, DateTime dataTransferencia, int sistemaId)
        {
            this.TransferenciaID = transferenciaId;
            this.LojaDeID = lojaDeId;
            this.LojaParaID = lojaParaId;
            this.ListaProduto = listaProduto;
            this.DataTransferencia = dataTransferencia;
            this.SistemaID = sistemaId;
        }

        // Properties
        public DateTime DataTransferencia
        {
            get
            {
                return this._dataTransferencia;
            }
            set
            {
                this._dataTransferencia = value;
            }
        }

        public List<Produto> ListaProduto
        {
            get
            {
                return this._listaProduto;
            }
            set
            {
                this._listaProduto = value;
            }
        }

        public int LojaDeID
        {
            get
            {
                return this._lojaDeId;
            }
            set
            {
                this._lojaDeId = value;
            }
        }

        public int LojaParaID
        {
            get
            {
                return this._lojaParaId;
            }
            set
            {
                this._lojaParaId = value;
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

        public int TransferenciaID
        {
            get
            {
                return this._transferenciaId;
            }
            set
            {
                this._transferenciaId = value;
            }
        }

        public bool Valida { get; set; }
    }
}
