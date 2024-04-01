using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Modelo
{
    [Serializable()]
    public class Empresa
    {
        // Methods
        public Empresa(string identificacao)
        {
            this.Identificacao = identificacao;
        }

        // Properties
        public string Identificacao { get; set; }
    }
}
