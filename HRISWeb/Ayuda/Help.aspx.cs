using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRISWeb.Ayuda
{
    public partial class Help : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureOptions();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfigureOptions()
        {
            
            if (String.IsNullOrEmpty(Request.Params["Screem"]))
            {

            }
            else
            {

            }
            ScriptManager.RegisterStartupScript(uppFrames
                , uppFrames.GetType()
                , "Alert"
                , String.Format("OpenSection('{0}', '{1}')", "sd80", "sd81")
                , true);
        }
    }
}