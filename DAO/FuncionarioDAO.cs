using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    public class FuncionarioDAO
    {
        // Fields
        private string _email;
        private int? _funcionarioId;
        private int? _lojaId;
        private string _nome;
        private int _sistemaId;
        private string _telefone;

        // Methods
        public FuncionarioDAO()
        {
        }

        public FuncionarioDAO(int funcionarioId, int sistemaId)
        {
            this.FuncionarioID = new int?(funcionarioId);
            this.SistemaID = sistemaId;
        }

        public FuncionarioDAO(int? funcionarioId, string nome, int? lojaId, string telefone, string email, int sistemaId)
        {
            this.FuncionarioID = funcionarioId;
            this.Nome = nome;
            this.LojaID = lojaId;
            this.Telefone = telefone;
            this.Email = email;
            this.SistemaID = sistemaId;
        }

        // Properties
        public string Email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
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

        public string Telefone
        {
            get
            {
                return this._telefone;
            }
            set
            {
                this._telefone = value;
            }
        }

    }
}
