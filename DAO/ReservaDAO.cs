using System;
using System.Collections.Generic;

namespace DAO
{
    public class ReservaDAO
    {
        public int? ClienteID;
        public string ClienteNome;
        public string Cnpj;
        public string Cpf;
        public DateTime? DataEntrega;
        public DateTime? DataReserva;
        public int? FuncionarioID;
        public string FuncionarioNome;
        public List<ProdutoDAO> ListaProduto;
        public int? LojaOrigemID { get; set; }
        public string LojaOrigemNomeFantasia { get; set; }
        public string Observacao;
        public string ReservaID;
        public bool SemDataEntrega;
        public int SistemaID;
        public int StatusID;
        public string DataReservaFormatada { get; set; }
        public string DataEntregaFormatada { get; set; }
        public decimal ValorFrete { get; set; }
        public long CV { get; set; }
        public string NomeCarreteiro { get; set; }
        public int LojaSaidaID { get; set; }
        public ClienteDAO Cliente { get; set; }
    }
}
