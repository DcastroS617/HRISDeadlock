using Microsoft.Reporting.WebForms;
using System.Net;

namespace HRISWeb.Shared
{
    /// <summary>
    /// CustomReportCredentials to implement the credentials parameters to send to report server
    /// </summary>
    public class CustomReportCredentials : IReportServerCredentials
    {
        private readonly string userName;
        private readonly string password;
        private readonly string domain;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <param name="domain">Domain</param>
        public CustomReportCredentials(string userName, string password, string domain)
        {
            this.userName = userName;
            this.password = password;
            this.domain = domain;
        }

        /// <summary>
        /// ImpersonationUser
        /// </summary>
        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }

        /// <summary>
        /// NetworkCredentials
        /// </summary>
        public ICredentials NetworkCredentials
        {
            get { return new NetworkCredential(userName, password, domain); }
        }

        /// <summary>
        /// GetFormsCredentials
        /// </summary>
        /// <param name="authCookie">Cookie</param>
        /// <param name="user">User</param>
        /// <param name="password">Password</param>
        /// <param name="authority">Authority</param>
        /// <returns></returns>
        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}