using System;
using System.Web;
using System.Web.Routing;

namespace Site
{
    public class Global : HttpApplication
    {
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //try
            //{
            //    Email.Enviar("MiX", "contato@sistemamix.com.br", Server.GetLastError().GetBaseException().StackTrace, "Application_Error");
            //}
            //catch (Exception ex)
            //{

            //}
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Add("Padrao", new Route("{empresa}/{pagina}", new CustomRouteHandler("~/{0}")));
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }
    }
}