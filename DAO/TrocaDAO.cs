using System;
using System.Collections.Generic;

namespace DAO
{
    public class TrocaDAO
    {
        public TrocaDAO()
        {
            TrocaProdutoDAO = new List<TrocaProdutoDAO>();
        }

        public int TrocaID { get; set; }
        public DateTime DataTroca { get; set; }
        public int SistemaID { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataExclusao { get; set; }
        public DateTime DataEntrega { get; set; }
        public string Observacao { get; set; }
        public string Status { get; set; }
        public string Svt { get; set; }

        public int LojaID { get; set; }
        public string NomeFantasia { get; set; }

        public int ClienteID { get; set; }
        public string NomeCliente { get; set; }
        public string Cidade { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }        
        public string PontoReferencia { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }

        public string TelefoneResidencial { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneResidencial2 { get; set; }
        public string TelefoneCelular2 { get; set; }

        public string Produto { get; set; }
        public short Quantidade { get; set; }

        public List<TrocaProdutoDAO> TrocaProdutoDAO { get; set; }
    }
}
