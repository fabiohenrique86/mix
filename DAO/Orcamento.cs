using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    public class Orcamento
    {
        public int OrcamentoID { get; set; }
        public int LojaID { get; set; }
        public int FuncionarioID { get; set; }
        public int StatusID { get; set; }
        public DateTime DataOrcamento { get; set; }
        public string Observacao { get; set; }
        public int SistemaID { get; set; }
        public List<Produto> ListaProduto = new List<Produto>();
    }
}
