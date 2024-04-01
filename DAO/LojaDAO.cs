using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    public class LojaDAO
    {
        /// <summary>
        /// 
        /// </summary>
        public int LojaID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NomeFantasia { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CNPJ { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SistemaID { get; set; }

        public string RazaoSocial { get; set; }
        public string Telefone { get; set; }
        public double Cota { get; set; }
    }
}
