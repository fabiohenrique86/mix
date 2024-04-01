using System;
using System.Collections.Generic;

namespace DAO
{
    public class PedidoDAO
    {
        // Fields
        private int? _clienteId;
        private string _cnpj;
        private string _cpf;
        private DateTime? _dataPedido;
        private int? _funcionarioId;
        private List<ProdutoDAO> _listaProduto;
        //private List<TipoPagamentoDAO> _listaTipoPagamento;
        private int? _lojaId;
        private int? _lojaOrigemId;
        private string _observacao;
        private string _pedidoId;
        private int _sistemaId;

        // Methods
        public PedidoDAO()
        {
        }

        public PedidoDAO(string pedidoId, int? lojaId, int? lojaOrigemId, int? funcionarioId, string cpf, string cnpj, DateTime? dataPedido, int sistemaId)
        {
            this.PedidoID = pedidoId;
            this.LojaID = lojaId;
            this.LojaOrigemID = lojaOrigemId;
            this.FuncionarioID = funcionarioId;
            this.Cpf = cpf;
            this.Cnpj = cnpj;
            this.DataPedido = dataPedido;
            this.SistemaID = sistemaId;
        }

        public PedidoDAO(string pedidoId, int lojaId, int lojaOrigemId, int funcionarioId, int? clienteId, string observacao, DateTime dataPedido, int sistemaId, List<ProdutoDAO> listaProduto/*, List<TipoPagamentoDAO> listaTipoPagamento*/)
        {
            this.PedidoID = pedidoId;
            this.LojaID = new int?(lojaId);
            this.LojaOrigemID = new int?(lojaOrigemId);
            this.FuncionarioID = new int?(funcionarioId);
            this.ClienteID = clienteId;
            this.Observacao = observacao;
            this.SistemaID = sistemaId;
            this.ListaProduto = listaProduto;
            //this.ListaTipoPagamento = listaTipoPagamento;
            this.DataPedido = new DateTime?(dataPedido);
        }

        // Properties
        public int? ClienteID
        {
            get
            {
                return this._clienteId;
            }
            set
            {
                this._clienteId = value;
            }
        }

        public string Cnpj
        {
            get
            {
                return this._cnpj;
            }
            set
            {
                this._cnpj = value;
            }
        }

        public string Cpf
        {
            get
            {
                return this._cpf;
            }
            set
            {
                this._cpf = value;
            }
        }

        public DateTime? DataPedido
        {
            get
            {
                return this._dataPedido;
            }
            set
            {
                this._dataPedido = value;
            }
        }

        public int? FuncionarioID
        {
            get
            {
                return this._funcionarioId;
            }
            set
            {
                this._funcionarioId = value;
            }
        }

        public List<ProdutoDAO> ListaProduto
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

        //public List<TipoPagamentoDAO> ListaTipoPagamento
        //{
        //    get
        //    {
        //        return this._listaTipoPagamento;
        //    }
        //    set
        //    {
        //        this._listaTipoPagamento = value;
        //    }
        //}

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

        public int? LojaOrigemID
        {
            get
            {
                return this._lojaOrigemId;
            }
            set
            {
                this._lojaOrigemId = value;
            }
        }

        public string Observacao
        {
            get
            {
                return this._observacao;
            }
            set
            {
                this._observacao = value;
            }
        }

        public string PedidoID
        {
            get
            {
                return this._pedidoId;
            }
            set
            {
                this._pedidoId = value;
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

        // Nested Types
        private enum StatusImprimido
        {
            Não,
            Sim
        }
    }
}
