using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Infrastructure;

namespace HRISWeb.App_Start
{
    public sealed class SameSiteCookieManager : ICookieManager
    {
        private readonly ICookieManager _inner = new SystemWebCookieManager();

        public string GetRequestCookie(IOwinContext context, string key)
            => _inner.GetRequestCookie(context, key);

        public void AppendResponseCookie(IOwinContext context, string key, string value, CookieOptions options)
        {
            options.SameSite = SameSiteMode.None;   // <- critical
            options.Secure = true;
            _inner.AppendResponseCookie(context, key, value, options);
        }

        public void DeleteCookie(IOwinContext context, string key, CookieOptions options)
            => _inner.DeleteCookie(context, key, options);
    }
}