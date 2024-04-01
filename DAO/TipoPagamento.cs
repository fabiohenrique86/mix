using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    public class TipoPagamento
    {
        // Fields
        private string _descricao;
        private int _parcelaId;
        private int _sistemaId;
        private int _tipoPagamentoId;
        private decimal _valor;

        // Methods
        public TipoPagamento()
        {
        }

        public TipoPagamento(string descricao, int sistemaId)
        {
            this.Descricao = descricao;
            this.SistemaID = sistemaId;
        }

        public TipoPagamento(int tipoPagamentoId, int parcelaId, decimal valor, int sistemaId)
        {
            this.TipoPagamentoID = tipoPagamentoId;
            this.ParcelaID = parcelaId;
            this.Valor = valor;
            this.SistemaID = sistemaId;
        }

        // Properties
        public string Descricao
        {
            get
            {
                return this._descricao;
            }
            set
            {
                this._descricao = value;
            }
        }

        public int ParcelaID
        {
            get
            {
                return this._parcelaId;
            }
            set
            {
                this._parcelaId = value;
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

        public int TipoPagamentoID
        {
            get
            {
                return this._tipoPagamentoId;
            }
            set
            {
                this._tipoPagamentoId = value;
            }
        }

        public decimal Valor
        {
            get
            {
                return this._valor;
            }
            set
            {
                this._valor = value;
            }
        }

    }
}
