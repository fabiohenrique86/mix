using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    public class ClientePF : Cliente
    {
        // Fields
        private string _cpf;
        private DateTime? _dataNascimento;
        private string _mesNascimento;
        private string _nome;

        // Methods
        public ClientePF(string nome, string cpf, DateTime dataNascimento, string endereco, string bairro, string cidade, string estado, string pontoReferencia, string telefoneResidencial, string telefoneCelular, string telefoneResidencial2, string telefoneCelular2, int sistemaId)
        {
            this.Nome = nome;
            this.Cpf = cpf;
            this.DataNascimento = new DateTime?(dataNascimento);
            base.Endereco = endereco;
            base.Bairro = bairro;
            base.Cidade = cidade;
            base.Estado = estado;
            base.PontoReferencia = pontoReferencia;
            base.TelefoneResidencial = telefoneResidencial;
            base.TelefoneCelular = telefoneCelular;
            base.TelefoneResidencial2 = telefoneResidencial2;
            base.TelefoneCelular2 = telefoneCelular2;
            base.SistemaID = sistemaId;
        }

        public ClientePF(int clienteId, string nome, string cpf, DateTime? dataNascimento, string endereco, string bairro, string cidade, string estado, string pontoReferencia, string telefoneResidencial, string telefoneCelular, string telefoneResidencial2, string telefoneCelular2, int sistemaId)
        {
            base.ClienteID = clienteId;
            this.Nome = nome;
            this.Cpf = cpf;
            this.DataNascimento = dataNascimento;
            base.Endereco = endereco;
            base.Bairro = bairro;
            base.Cidade = cidade;
            base.Estado = estado;
            base.PontoReferencia = pontoReferencia;
            base.TelefoneResidencial = telefoneResidencial;
            base.TelefoneCelular = telefoneCelular;
            base.TelefoneResidencial2 = telefoneResidencial2;
            base.TelefoneCelular2 = telefoneCelular2;
            base.SistemaID = sistemaId;
        }

        public ClientePF(string nome, string cpf, DateTime? dataNascimento, string endereco, string bairro, string cidade, string estado, string pontoReferencia, string telefoneResidencial, string telefoneCelular, string telefoneResidencial2, string telefoneCelular2, int sistemaId, int? funcionarioId, string mesNascimento)
        {
            this.Nome = nome;
            this.Cpf = cpf;
            this.DataNascimento = dataNascimento;
            base.Endereco = endereco;
            base.Bairro = bairro;
            base.Cidade = cidade;
            base.Estado = estado;
            base.PontoReferencia = pontoReferencia;
            base.TelefoneResidencial = telefoneResidencial;
            base.TelefoneCelular = telefoneCelular;
            base.TelefoneResidencial2 = telefoneResidencial2;
            base.TelefoneCelular2 = telefoneCelular2;
            base.SistemaID = sistemaId;
            base.FuncionarioID = funcionarioId;
            this.MesNascimento = mesNascimento;
        }

        // Properties
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

        public DateTime? DataNascimento
        {
            get
            {
                return this._dataNascimento;
            }
            set
            {
                this._dataNascimento = value;
            }
        }

        public string MesNascimento
        {
            get
            {
                return this._mesNascimento;
            }
            set
            {
                this._mesNascimento = value;
            }
        }

        public string Nome
        {
            get
            {
                return this._nome;
            }
            set
            {
                this._nome = value;
            }
        }

    }
}
