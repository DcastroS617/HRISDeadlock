using System.Web;
using System.Web.SessionState;

namespace HRISWeb
{
    /// <summary>
    /// Summary description for KeepSessionAlive
    /// </summary>
    public class KeepSessionAlive : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(context.Session.SessionID);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}