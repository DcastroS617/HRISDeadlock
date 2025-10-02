using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Infrastructure; 
using Microsoft.Owin.Host.SystemWeb;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using HRISWeb.App_Start;

[assembly: OwinStartup(typeof(HRISWeb.Startup))]
namespace HRISWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var cookieManager = new SameSiteCookieManager();
            // Set the default authentication type
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            // Cookie middleware must come first
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/default.aspx"),
                CookieSecure = CookieSecureOption.Always,
                CookieHttpOnly = true,
                CookieSameSite = Microsoft.Owin.SameSiteMode.None,
                CookieManager = cookieManager
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["ClientId"],
                Authority = ConfigurationManager.AppSettings["Authority"],
                RedirectUri = ConfigurationManager.AppSettings["RedirectUri"],
                PostLogoutRedirectUri = ConfigurationManager.AppSettings["PostLogoutRedirectUri"],

                ResponseType = OpenIdConnectResponseType.IdToken,
                ResponseMode = OpenIdConnectResponseMode.FormPost,

                CallbackPath = new PathString("/signin-oidc"),

                Scope = "openid profile email",

                SignInAsAuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                RequireHttpsMetadata = true,

                CookieManager = cookieManager,

                TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    NameClaimType = "name",               // or preferred_username
                    RoleClaimType = ClaimTypes.Role
                },

                ProtocolValidator = new OpenIdConnectProtocolValidator
                {
                    RequireNonce = true,
                    RequireState = true
                },

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    RedirectToIdentityProvider = ctx =>
                    {
                        var baseUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}";
                        ctx.ProtocolMessage.RedirectUri = $"{baseUrl}/signin-oidc";
                        ctx.ProtocolMessage.PostLogoutRedirectUri = $"{baseUrl}/signout-callback-oidc";
                        // Log what we send
                        System.Diagnostics.Trace.WriteLine(
                            $"OIDC->AuthZ request: redirectUri={ctx.ProtocolMessage.RedirectUri} " +
                            $"response_mode={ctx.ProtocolMessage.ResponseMode} state(len)={(ctx.ProtocolMessage.State ?? "").Length}");
                        return Task.FromResult(0);
                    },
                    SecurityTokenValidated = context =>
                    {
                        // This is key: manually sign in to create a cookie
                        var identity = context.AuthenticationTicket.Identity;
                        context.OwinContext.Authentication.SignIn(identity);
                        return Task.CompletedTask;
                    },
                    AuthenticationFailed = context =>
                    {
                        System.Diagnostics.Trace.WriteLine(
                            $"OIDC AuthFailed: {context.Exception}\n");

                        /*context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.Write("Unauthorized");
                        return Task.FromResult(0);*/

                        context.HandleResponse();
                        context.Response.Redirect("ErrorPages/Error401.aspx");
                        return Task.FromResult(0);
                    }
                }
            });

            app.UseStageMarker(PipelineStage.Authenticate);
        }
    }
}