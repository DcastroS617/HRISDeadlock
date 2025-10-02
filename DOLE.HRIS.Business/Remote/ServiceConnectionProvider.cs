using DOLE.HRIS.Exceptions;
using Flurl.Http;
using Flurl.Http.Configuration;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
namespace DOLE.HRIS.Application.Business.Remote
{

    /// <summary>
    /// Provider for service connection
    /// </summary>
    public static class ServiceConnectionProvider
    {
        private const string serviceConnectionAppKeyPrefix = "ServiceConnectionForDivision";

        /// <summary>
        /// Finds a service connection
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <returns>The service connection url</returns>
        public static string FindServiceConnection(int divisionCode)
        {
            string serviceConnection = string.Format("{0}{1}", serviceConnectionAppKeyPrefix, divisionCode);

            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[serviceConnection]))
            {
                throw new NotImplementedException(string.Format("There is no service connection configure for division {0}", divisionCode));
            }

            return ConfigurationManager.AppSettings[serviceConnection];
        }

        /// <summary>
        /// Logs the services error (using the ServiceException that already logs)
        /// </summary>
        /// <param name="ex"></param>
        private static void LogErrorAsync(Exception ex)
        {
            ServiceException serviceException = new ServiceException(ex.Message, ex);
            throw serviceException;
        }

        /// <summary>
        /// Hadles the services errores
        /// </summary>
        /// <param name="call"></param>
        private static void HandleFlurlErrorAsync(HttpCall call)
        {
            LogErrorAsync(call.Exception);
            call.ExceptionHandled = true;
        }

        /// <summary>
        /// Initialize the flurl services settings
        /// </summary>
        public static void Initialize()
        {
            FlurlHttp.Configure(settings =>
            {
                settings.OnError = HandleFlurlErrorAsync;
            }
            );

            ConfigurationManager.AppSettings.AllKeys
                .Where(appKey => appKey.StartsWith(serviceConnectionAppKeyPrefix))
                .ToList()
                .ForEach(serviceConnection =>
                {
                    FlurlHttp.ConfigureClient(ConfigurationManager.AppSettings[serviceConnection], cli => cli
                        .Configure(settings =>
                        {
                            // keeps logging & error handling out of SimpleCastClient
                            settings.BeforeCall = call => Console.WriteLine($"Calling {call.Request.RequestUri}");
                            settings.OnError = call => HandleFlurlErrorAsync(call);
                            settings.AfterCall = call => Console.WriteLine($"End Call {call.Request.RequestUri}");
                        })

                        // adds default headers to send with every call
                        .WithHeaders(new
                        {
                            Accept = "application/json",
                        })

                        .WithBasicAuth(string.Format("{0}\\{1}", ConfigurationManager.AppSettings["DominioUsuarioServiceConnection"], ConfigurationManager.AppSettings["UsuarioConsultaServiceConnection"]), ConfigurationManager.AppSettings["ClaveConsultaServiceConnection"])
                        .Settings.HttpClientFactory = new WindowsAuthClientFactory()
                    );
                }
            );
        }
    }

    /// <summary>
    /// Default credentials auth factory
    /// </summary>
    public class WindowsAuthClientFactory : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientHandler { UseDefaultCredentials = true };
        }
    }
}
