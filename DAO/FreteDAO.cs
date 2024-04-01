using System;

namespace DAO
{
    public class FreteDAO
    {
        public string PedidoID { get; set; }
        public int LojaOrigemID { get; set; }
        public string DscLojaOrigem { get; set; }
        public int FuncionarioID { get; set; }
        public string NmFuncionario { get; set; }
        public decimal ValorFrete { get; set; }
        public string CV { get; set; }
        public string NmCidade { get; set; }
        public string NomeCarreteiro { get; set; }
        public DateTime DtReserva { get; set; }
        public DateTime DtReservaInicial { get; set; }
        public DateTime DtReservaFinal { get; set; }
        public DateTime DtEntrega { get; set; }
        public DateTime DtEntregaInicial { get; set; }
        public DateTime DtEntregaFinal { get; set; }
        public int StEntregaID { get; set; }
        public string DscEntrega { get; set; }
        public int SistemaID { get; set; }
    }
}
