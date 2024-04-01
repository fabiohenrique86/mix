using System;
using System.Collections.Generic;

namespace DAO
{
    public class OcorrenciaDAO
    {
        public int OcorrenciaID { get; set; }
        public int NotaFiscalID { get; set; }
        public int LojaID { get; set; }
        public int? NumeroTroca { get; set; }
        public string NomeFantasia { get; set; }
        public DateTime DataOcorrencia { get; set; }
        public DateTime DataOcorrenciaInicial { get; set; }
        public DateTime DataOcorrenciaFinal { get; set; }
        public int MotivoOcorrenciaID { get; set; }
        public string MotivoOcorrencia { get; set; }
        public string NomeMotorista { get; set; }
        public string PlacaCaminhao { get; set; }
        public string Observacao { get; set; }
        public int StatusOcorrenciaID { get; set; }
        public int SistemaID { get; set; }
        public List<ProdutoDAO> ProdutoDAO = new List<ProdutoDAO>();
    }

    public enum EStatusOcorrencia
    {
        Pendente = 1,
        Resolvida = 2
    }
}
