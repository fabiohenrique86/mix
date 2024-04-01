using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site.administrador
{
    public partial class BaseAdm : System.Web.UI.MasterPage
    {
        protected void lkbSair_Click(object sender, EventArgs e)
        {
            base.Session.Abandon();
            base.Response.Redirect("../Default.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Session["Owner"] != null)
            {
                this.menuadm.Visible = true;
            }
            else
            {
                this.menuadm.Visible = false;
            }
        }

    }
}