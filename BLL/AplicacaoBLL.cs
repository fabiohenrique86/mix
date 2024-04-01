using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Modelo;
using System.Web;

namespace BLL
{
    public static class AplicacaoBLL
    {
        // Properties
        public static Empresa Empresa
        {
            get
            {
                return (HttpContext.Current.Session["Empresa"] as Empresa);
            }
            set
            {
                Empresa empresa = HttpContext.Current.Session["Empresa"] as Empresa;
                if (value == null)
                {
                    empresa = null;
                    HttpContext.Current.Session["Empresa"] = null;
                }
                else if ((empresa == null) || (empresa.Identificacao != value.Identificacao))
                {
                    empresa = new Empresa(value.Identificacao);
                    HttpContext.Current.Session["Empresa"] = empresa;
                }
            }
        }

        //public static Usuario Usuario
        //{
        //    get
        //    {
        //        return (HttpContext.Current.Session["Usuario"] as Usuario);
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["Usuario"] = value;
        //    }
        //}
    }
}
