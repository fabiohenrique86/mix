using System;
using System.Collections.Generic;

namespace DAO
{
    public class CargaDAO
    {
        public int CargaID { get; set; }
        public string NumeroCarga { get; set; }
        public DateTime DataCadastro { get; set; }
        public int SistemaID { get; set; }

        public List<NotaFiscalDAO> NotaFiscalDao = new List<NotaFiscalDAO>();
    }
}
