using System;
using System.Collections.Generic;

namespace DAO
{
    public class Reserva
    {
        // Fields
        private int? _clienteId;
        private string _clienteNome;
        private string _cnpj;
        private string _cpf;
        private DateTime? _dataEntrega;
        private DateTime? _dataReserva;
        private int? _funcionarioId;
        private string _funcionarioNome;
        private List<Produto> _listaProduto;
        private List<TipoPagamento> _listaTipoPagamento;
        private int? _lojaId;
        private int? _lojaOrigemId;
        private string _lojaOrigemNomeFantasia;
        private string _lojaDestinoNomeFantasia;
        private string _observacao;
        private int? _reservaId;
        private bool _semDataEntrega;
        private int _sistemaId;
        private int _statusId;
        public string DataReservaFormatada { get; set; }
        public string DataEntregaFormatada { get; set; }
        public decimal ValorFrete { get; set; }
        public long CV { get; set; }
        public string NomeCarreteiro { get; set; }

        public Cliente Cliente { get; set; }

        // Methods
        public Reserva()
        {
        }

        public Reserva(int? reservaId, int? lojaId, int? lojaOrigemId, int? funcionarioId, string cpf, string cnpj, DateTime? dataReserva, DateTime? dataEntrega, bool semDataEntrega, int sistemaId)
        {
            this.ReservaID = reservaId;
            this.LojaID = lojaId;
            this.LojaOrigemID = lojaOrigemId;
            this.FuncionarioID = funcionarioId;
            this.Cpf = cpf;
            this.Cnpj = cnpj;
            this.SistemaID = sistemaId;
            this.DataReserva = dataReserva;
            this.DataEntrega = dataEntrega;
            this.SemDataEntrega = semDataEntrega;
        }

        public Reserva(int reservaId, int lojaId, int lojaOrigemId, int funcionarioId, int? clienteId, string observacao, DateTime dataReserva, DateTime? dataEntrega, int sistemaId, int statusId, List<Produto> listaProduto, List<TipoPagamento> listaTipoPagamento)
        {
            this.ReservaID = new int?(reservaId);
            this.LojaID = new int?(lojaId);
            this.LojaOrigemID = new int?(lojaOrigemId);
            this.FuncionarioID = new int?(funcionarioId);
            this.ClienteID = clienteId;
            this.Observacao = observacao;
            this.SistemaID = sistemaId;
            this.ListaProduto = listaProduto;
            this.ListaTipoPagamento = listaTipoPagamento;
            this.DataReserva = new DateTime?(dataReserva);
            this.DataEntrega = dataEntrega;
            this.StatusID = statusId;
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

        public string ClienteNome
        {
            get
            {
                return this._clienteNome;
            }
            set
            {
                this._clienteNome = value;
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

        public DateTime? DataEntrega
        {
            get
            {
                return this._dataEntrega;
            }
            set
            {
                this._dataEntrega = value;
            }
        }

        public DateTime? DataReserva
        {
            get
            {
                return this._dataReserva;
            }
            set
            {
                this._dataReserva = value;
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

        public string FuncionarioNome
        {
            get
            {
                return this._funcionarioNome;
            }
            set
            {
                this._funcionarioNome = value;
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

        public string LojaOrigemNomeFantasia
        {
            get
            {
                return this._lojaOrigemNomeFantasia;
            }
            set
            {
                this._lojaOrigemNomeFantasia = value;
            }
        }

        public string LojaDestinoNomeFantasia
        {
            get
            {
                return this._lojaDestinoNomeFantasia;
            }
            set
            {
                this._lojaDestinoNomeFantasia = value;
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

        public int? ReservaID
        {
            get
            {
                return this._reservaId;
            }
            set
            {
                this._reservaId = value;
            }
        }

        public bool SemDataEntrega
        {
            get
            {
                return this._semDataEntrega;
            }
            set
            {
                this._semDataEntrega = value;
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

        public int StatusID
        {
            get
            {
                return this._statusId;
            }
            set
            {
                this._statusId = value;
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
