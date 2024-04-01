using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    public class Pedido
    {
        // Fields
        private int? _clienteId;
        private string _cnpj;
        private string _cpf;
        private DateTime? _dataPedido;
        private int? _funcionarioId;
        private List<Produto> _listaProduto;
        private List<TipoPagamento> _listaTipoPagamento;
        private int? _lojaId;
        private int? _lojaOrigemId;
        private string _observacao;
        private int? _pedidoId;
        private int _sistemaId;

        // Methods
        public Pedido()
        {
        }

        public Pedido(int? pedidoId, int? lojaId, int? lojaOrigemId, int? funcionarioId, string cpf, string cnpj, DateTime? dataPedido, int sistemaId)
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

        public Pedido(int pedidoId, int lojaId, int lojaOrigemId, int funcionarioId, int? clienteId, string observacao, DateTime dataPedido, int sistemaId, List<Produto> listaProduto, List<TipoPagamento> listaTipoPagamento)
        {
            this.PedidoID = new int?(pedidoId);
            this.LojaID = new int?(lojaId);
            this.LojaOrigemID = new int?(lojaOrigemId);
            this.FuncionarioID = new int?(funcionarioId);
            this.ClienteID = clienteId;
            this.Observacao = observacao;
            this.SistemaID = sistemaId;
            this.ListaProduto = listaProduto;
            this.ListaTipoPagamento = listaTipoPagamento;
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

        public List<TipoPagamento> ListaTipoPagamento
        {
            get
            {
                return this._listaTipoPagamento;
            }
            set
            {
                this._listaTipoPagamento = value;
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

        public int? PedidoID
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
