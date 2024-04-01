using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Compilation;
using System.Web.UI;

namespace Site
{
    public class CustomRouteHandler : IRouteHandler
    {
        // Fields
        private string virtualPath;

        // Methods
        public CustomRouteHandler(string virtualPath)
        {
            this.virtualPath = virtualPath;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string empresa = requestContext.RouteData.GetRequiredString("empresa");
            string pagina = requestContext.RouteData.GetRequiredString("pagina");
            string virtualPath = string.Format(this.virtualPath, pagina);
            requestContext.HttpContext.Items.Add("empresa", empresa);
            return (BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(Page)) as IHttpHandler);
        }

    }
}