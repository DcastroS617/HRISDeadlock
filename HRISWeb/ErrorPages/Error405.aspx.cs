using HRISWeb.Shared;
using System;
using System.Globalization;
using System.Threading;

namespace HRIS.ErrorPages
{
    public partial class Error405 : System.Web.UI.Page
    {
        /// <summary>
        /// Sets the culture information
        /// </summary>
        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            if (Session[Constants.cCulture] != null)
            {
                CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
        }
    }
}