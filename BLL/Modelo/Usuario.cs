using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Modelo
{
    [Serializable()]
    public class Usuario
    {
        // Fields
        private int lojaId;
        private string nomeLoja;
        private int sistemaId;
        private int statusSistemaId;
        private string tipoUsuario;
        private int tipoUsuarioId;

        // Methods
        public Usuario(int tipoUsuarioId, string tipoUsuario, int sistemaId, int statusSistemaId)
        {
            this.TipoUsuarioID = tipoUsuarioId;
            this.TipoUsuario = tipoUsuario;
            this.SistemaID = sistemaId;
            this.statusSistemaId = statusSistemaId;
        }

        public Usuario(int tipoUsuarioId, string tipoUsuario, int sistemaId, int statusSistemaId, int lojaId, string nomeLoja)
        {
            this.TipoUsuarioID = tipoUsuarioId;
            this.TipoUsuario = tipoUsuario;
            this.SistemaID = sistemaId;
            this.statusSistemaId = statusSistemaId;
            this.LojaID = lojaId;
            this.NomeLoja = nomeLoja;
        }

        public Usuario(object sessao)
        {
            if (sessao == null)
            {
                //Utilitario.Sair();
                
                //if (System.Web.HttpContext.Current.Request.Url.Segments.Length == 3)
                //{
                //    System.Web.HttpContext.Current.Response.Redirect("../Default.aspx", false);
                //}
                //else
                //{
                //    System.Web.HttpContext.Current.Response.Redirect("Default.aspx", false);
                //}
                
                return;
            }

            if (((Usuario)sessao).LojaID > 0)
            {
                this.LojaID = ((Usuario)sessao).LojaID;
            }
            
            if (!string.IsNullOrEmpty(((Usuario)sessao).NomeLoja))
            {
                this.NomeLoja = ((Usuario)sessao).NomeLoja;
            }

            this.SistemaID = ((Usuario)sessao).SistemaID;
            this.StatusSistemaID = ((Usuario)sessao).StatusSistemaID;
            this.TipoUsuario = ((Usuario)sessao).TipoUsuario;
            this.tipoUsuarioId = ((Usuario)sessao).tipoUsuarioId;
        }
        
        // Properties
        public int LojaID
        {
            get
            {
                return this.lojaId;
            }
            set
            {
                this.lojaId = value;
            }
        }

        public string NomeLoja
        {
            get
            {
                return this.nomeLoja;
            }
            set
            {
                this.nomeLoja = value;
            }
        }

        public int SistemaID
        {
            get
            {
                return this.sistemaId;
            }
            set
            {
                this.sistemaId = value;
            }
        }

        public int StatusSistemaID
        {
            get
            {
                return this.statusSistemaId;
            }
            set
            {
                this.statusSistemaId = value;
            }
        }

        public string TipoUsuario
        {
            get
            {
                return this.tipoUsuario;
            }
            set
            {
                this.tipoUsuario = value;
            }
        }

        public int TipoUsuarioID
        {
            get
            {
                return this.tipoUsuarioId;
            }
            set
            {
                this.tipoUsuarioId = value;
            }
        }
    }
}
