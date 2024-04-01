using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    public class UsuarioDAO
    {
        // Fields
        public int LojaID { get; set; }
        public string NomeLoja { get; set; }
        public int SistemaID { get; set; }
        public int StatusSistemaID { get; set; }
        public string TipoUsuario { get; set; }
        public int tipoUsuarioID { get; set; }
        
        public UsuarioDAO(object sessao)
        {
            this.LojaID = ((UsuarioDAO)sessao).LojaID;
            this.NomeLoja = ((UsuarioDAO)sessao).NomeLoja;
            this.SistemaID = ((UsuarioDAO)sessao).SistemaID;
            this.StatusSistemaID = ((UsuarioDAO)sessao).StatusSistemaID;
            this.TipoUsuario = ((UsuarioDAO)sessao).TipoUsuario;
            this.tipoUsuarioID = ((UsuarioDAO)sessao).tipoUsuarioID;
        }

    }
}
