using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    public class ClientePJ : Cliente
    {
        // Fields
        private string _cnpj;
        private string _nomeFantasia;
        private string _razaoSocial;

        // Methods
        public ClientePJ(string cnpj, string nomeFantasia, string razaoSocial, string endereco, string bairro, string cidade, string estado, string pontoReferencia, string telefoneResidencial, string telefoneCelular, string telefoneResidencial2, string telefoneCelular2, int sistemaId)
        {
            this.Cnpj = cnpj;
            this.NomeFantasia = nomeFantasia;
            this.RazaoSocial = razaoSocial;
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

        public ClientePJ(int clienteId, string cnpj, string nomeFantasia, string razaoSocial, string endereco, string bairro, string cidade, string estado, string pontoReferencia, string telefoneResidencial, string telefoneCelular, string telefoneResidencial2, string telefoneCelular2, int sistemaId)
        {
            base.ClienteID = clienteId;
            this.Cnpj = cnpj;
            this.NomeFantasia = nomeFantasia;
            this.RazaoSocial = razaoSocial;
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

        public ClientePJ(string cnpj, string nomeFantasia, string razaoSocial, string endereco, string bairro, string cidade, string estado, string pontoReferencia, string telefoneResidencial, string telefoneCelular, string telefoneResidencial2, string telefoneCelular2, int sistemaId, int? funcionarioId)
        {
            this.Cnpj = cnpj;
            this.NomeFantasia = nomeFantasia;
            this.RazaoSocial = razaoSocial;
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
        }

        // Properties
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

        public string NomeFantasia
        {
            get
            {
                return this._nomeFantasia;
            }
            set
            {
                this._nomeFantasia = value;
            }
        }

        public string RazaoSocial
        {
            get
            {
                return this._razaoSocial;
            }
            set
            {
                this._razaoSocial = value;
            }
        }

    }
}
