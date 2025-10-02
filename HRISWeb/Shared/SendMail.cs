using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Shared.Notification;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
namespace HRISWeb.Shared
{
    public static class SendMailUtil
    {
        private const string clientId = "ClientId";
        private const string clientSecret = "ClientSecret";
        private const string tenantID = "TenantID";
        private const string resourceID = "ResourceID";
        private const string aPISourceApplication = "APISourceApplication";
        private const string createdBy = "ApiCreatedBy";
        private const string urlNotificacionService = "UrlNotificacionService";

        /// <summary>
        /// Get token
        /// </summary>
        /// <returns></returns>
        private static string GetBearerToken(IGeneralParametersBll<GeneralParameterEntity> objGeneralParametersBll)
        {
            AuthenticationContext authContext = new AuthenticationContext(string.Format("{0}{1}", ConfigurationManager.AppSettings["UrlTokenEndpoint"], objGeneralParametersBll.ListByFilter(tenantID), true));
            ClientCredential credential = new ClientCredential(
                                                               objGeneralParametersBll.ListByFilter(clientId),
                                                               objGeneralParametersBll.ListByFilter(clientSecret)
                                                                );
            var task = authContext.AcquireTokenAsync(objGeneralParametersBll.ListByFilter(resourceID), credential);
            var result = task.GetAwaiter().GetResult();

            return result.AccessToken;
        }
        /// <summary>
        /// Send emial 
        /// </summary>
        /// <param name="input">Email request</param>
        /// <returns>Request notification Identifier</returns>
        public static int SendEmail(IGeneralParametersBll<GeneralParameterEntity> objGeneralParametersBll, CreateEmailQueuedDto input)
        {

            EmailDto mail = null;
            HttpClient httpClient = new HttpClient();
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings[urlNotificacionService] + "api/Email/Create"))
            {

                string token = GetBearerToken(objGeneralParametersBll);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer"
                    , token);

                var content = new MultipartFormDataContent
                {
                    { new StringContent(input.Subject), "Subject" },
                    { new StringContent(input.Body), "Body" },
                    { new StringContent(input.Recipients), "Recipients" },
                    { new StringContent(input.CCRecipients), "CCRecipients" },
                    { new StringContent(input.BCCRecipients), "BCCRecipients" },
                    { new StringContent(objGeneralParametersBll.ListByFilter(aPISourceApplication)), "SourceApplication" },
                    { new StringContent(objGeneralParametersBll.ListByFilter(createdBy)), "CreatedBy" },
                    { new StringContent(objGeneralParametersBll.ListByFilter(createdBy)), "LastUpdateUser" },
                };
                if (input.AttachmentFiles != null && input.AttachmentFiles.Count > 0)
                {
                    foreach (var file in input.AttachmentFiles)
                    {
                        content.Add(new StreamContent(file.InputStream), "AttachmentFiles", file.FileName);
                    }
                }

                request.Content = content;

                var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    string result = responseContent.ReadAsStringAsync().GetAwaiter().GetResult();
                    mail = (JsonConvert.DeserializeObject<ApiResult<EmailDto>>(result)).Result;
                }
            }
            return mail.EmailRequestID;
        }
    }
}