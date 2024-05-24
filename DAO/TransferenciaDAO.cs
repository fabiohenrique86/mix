using System;
using System.Collections.Generic;

namespace DAO
{
    [Serializable()]
    public class TransferenciaDAO
    {
        public TransferenciaDAO()
        {
        }

        public TransferenciaDAO(int transferenciaId, int lojaDeId, int lojaParaId, List<ProdutoDAO> listaProduto, DateTime dataTransferencia, int sistemaId)
        {
            this.TransferenciaID = transferenciaId;
            this.LojaDeID = lojaDeId;
            this.LojaParaID = lojaParaId;
            this.ListaProduto = listaProduto;
            this.DataTransferencia = dataTransferencia;
            this.SistemaID = sistemaId;
        }

        public DateTime DataTransferencia { get; set; }
        public string DataTransferenciaString { get; set; }
        public List<ProdutoDAO> ListaProduto { get; set; }
        public int LojaDeID { get; set; }
        public string LojaDeNome { get; set; }
        public int LojaParaID { get; set; }
        public string LojaParaNome { get; set; }
        public int SistemaID { get; set; }
        public int TransferenciaID { get; set; }
        public bool Valida { get; set; }
    }
}
