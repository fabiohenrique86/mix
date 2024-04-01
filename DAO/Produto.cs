using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    [Serializable()]
    public class Produto
    {
        private short? _comissaoFranqueado;
        private short? _comissaoFuncionario;
        private string _descricao;
        private int? _linhaId;
        private int? _lojaId;
        private string _medida;
        private int? _medidaId;
        private decimal _preco;
        private long _produtoId;
        private short? _quantidade;
        private int _sistemaId;

        public Produto()
        {
        }

        public Produto(long produtoId, short quantidade, int sistemaId)
        {
            this.ProdutoID = produtoId;
            this.Quantidade = new short?(quantidade);
            this.SistemaID = sistemaId;
        }

        public Produto(long produtoId, int lojaId, int sistemaId)
        {
            this.ProdutoID = produtoId;
            this.LojaID = new int?(lojaId);
            this.SistemaID = sistemaId;
        }

        public Produto(long produtoId, short quantidade, string medida, decimal preco, int sistemaId)
        {
            this.ProdutoID = produtoId;
            this.Quantidade = new short?(quantidade);
            this.Medida = medida;
            this.Preco = preco;
            this.SistemaID = sistemaId;
        }

        public Produto(long produtoId, int lojaId, int linhaId, short comissaoFuncionario, short comissaoFranqueado, string descricao, int medidaId, short quantidade, int sistemaId)
        {
            this.ProdutoID = produtoId;
            this.LojaID = new int?(lojaId);
            this.LinhaID = new int?(linhaId);
            this.ComissaoFuncionario = new short?(comissaoFuncionario);
            this.ComissaoFranqueado = new short?(comissaoFranqueado);
            this.Descricao = descricao;
            this.MedidaID = new int?(medidaId);
            this.Quantidade = new short?(quantidade);
            this.SistemaID = sistemaId;
        }

        // Properties
        public short? ComissaoFranqueado
        {
            get
            {
                return this._comissaoFranqueado;
            }
            set
            {
                this._comissaoFranqueado = value;
            }
        }

        public short? ComissaoFuncionario
        {
            get
            {
                return this._comissaoFuncionario;
            }
            set
            {
                this._comissaoFuncionario = value;
            }
        }

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

        public int? LinhaID
        {
            get
            {
                return this._linhaId;
            }
            set
            {
                this._linhaId = value;
            }
        }

        public int? LojaID
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

        public string Medida
        {
            get
            {
                return this._medida;
            }
            set
            {
                this._medida = value;
            }
        }

        public int? MedidaID
        {
            get
            {
                return this._medidaId;
            }
            set
            {
                this._medidaId = value;
            }
        }

        public decimal Preco
        {
            get
            {
                return this._preco;
            }
            set
            {
                this._preco = value;
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

        public short? Quantidade
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

        public int TransferenciaID { get; set; }
        public int LojaDeID { get; set; }
        public int LojaParaID { get; set; }
    }
}
