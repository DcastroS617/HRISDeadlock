using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Unity.Web
{
    /// <summary>
    ///	HttpModule that maintains a Unity container per request for dependency resolution.
    /// </summary>
    public class UnityHttpModule : IHttpModule
    {
        HttpApplication mContext;

        public void Init(HttpApplication context)
        {
            mContext = context;
            mContext.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
        }

        public void Dispose() { }

        private void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            IHttpHandler currentHandler = HttpContext.Current.Handler;
            HttpContext.Current.Application.GetContainer().BuildUp(currentHandler.GetType(), currentHandler);

            // User Controls are ready to be built up after page initialization is complete
            if (HttpContext.Current.Handler is Page currentPage)
            {
                currentPage.InitComplete += OnPageInitComplete;
            }
        }

        // Build up each control in the page's control tree
        private void OnPageInitComplete(object sender, EventArgs e)
        {
            var currentPage = (Page)sender;
            IUnityContainer container = HttpContext.Current.Application.GetContainer();

            foreach (Control c in GetControlTree(currentPage))
            {
                container.BuildUp(c.GetType(), c);
            }

            mContext.PreRequestHandlerExecute -= OnPreRequestHandlerExecute;
        }

        // Get the controls in the page's control tree excluding the page itself
        private IEnumerable<Control> GetControlTree(Control root)
        {
            foreach (Control child in root.Controls)
            {
                yield return child;

                foreach (Control c in GetControlTree(child))
                {
                    yield return c;
                }
            }
        }
    }
}