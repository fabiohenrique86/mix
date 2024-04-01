using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    public class ClienteDAO
    {
        // Fields
        private bool _ativo;
        private string _bairro;
        private string _cidade;
        private int _clienteId;
        private string _endereco;
        private string _estado;
        private int? _funcionarioId;
        private string _pontoReferencia;
        private int _sistemaId;
        private string _telefoneCelular;
        private string _telefoneCelular2;
        private string _telefoneResidencial;
        private string _telefoneResidencial2;
        private string _email;
        private string _cep;

        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Referencia { get; set; }
        public string Email { get; set; }
        public string Cep { get; set; }

        // Properties
        public bool Ativo
        {
            get
            {
                return this._ativo;
            }
            set
            {
                this._ativo = value;
            }
        }

        public string Bairro
        {
            get
            {
                return this._bairro;
            }
            set
            {
                this._bairro = value;
            }
        }

        public string Cidade
        {
            get
            {
                return this._cidade;
            }
            set
            {
                this._cidade = value;
            }
        }

        public int ClienteID
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

        public string Endereco
        {
            get
            {
                return this._endereco;
            }
            set
            {
                this._endereco = value;
            }
        }

        public string Estado
        {
            get
            {
                return this._estado;
            }
            set
            {
                this._estado = value;
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

        public string PontoReferencia
        {
            get
            {
                return this._pontoReferencia;
            }
            set
            {
                this._pontoReferencia = value;
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

        public string TelefoneCelular
        {
            get
            {
                return this._telefoneCelular;
            }
            set
            {
                this._telefoneCelular = value;
            }
        }

        public string TelefoneCelular2
        {
            get
            {
                return this._telefoneCelular2;
            }
            set
            {
                this._telefoneCelular2 = value;
            }
        }

        public string TelefoneResidencial
        {
            get
            {
                return this._telefoneResidencial;
            }
            set
            {
                this._telefoneResidencial = value;
            }
        }

        public string TelefoneResidencial2
        {
            get
            {
                return this._telefoneResidencial2;
            }
            set
            {
                this._telefoneResidencial2 = value;
            }
        }
    }
}
