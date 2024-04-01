using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    public class MotivoOcorrenciaDAO
    {
        public int MotivoOcorrenciaID { get; set; }
        public string Descricao { get; set; }
        public int SistemaID { get; set; }
    }

    public enum EMotivoOcorrencia
    {
        CANCELAMENTO_DE_COMPRA = 1,
        BAIXA_DA_CONSIGNACAO = 2,
        PRODUTO_DA_REPOSIÇAO_DANIFICADO = 3,
        TROCA_DE_MERCADORIA = 4,
        FALTA_DE_MERCADORIA = 5
    }
}
