using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace HRISWeb.Training
{
    public class LogbookFilesController : ApiController
    {
        public const string Octet = "application/octet-stream";
        public const string Pdf = "application/pdf";
        public const string attachment = "attachment";

        private const string reportServerUrl = "ReportServerUrl";
        private const string reportServerFolder = "ReportServerFolder";

        public IGeneralParametersBll<GeneralParameterEntity> ObjGeneralParametersBll { get; set; }


        [HttpGet]
        public async Task<HttpResponseMessage> DownloadFileLogbook(int id)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                ILogbooksFilesBll objLogbooksFilesBll = new LogbooksFilesBll(new LogbooksFilesDal());

                var Result = objLogbooksFilesBll.LogbookFilesByIdFile(new DOLE.HRIS.Shared.Entity.LogbooksFileEntity
                {
                    LogbooksFileId = id
                });

                if (Result.File.Any())
                {
                    var typeFile = "";
                    switch (Result.FileExtension)
                    {
                        case "pdf":
                            typeFile = Pdf;
                            break;
                        default:
                            typeFile = Octet;
                            break;
                    }

                    var File = Result.File;

                    response.Content = new ByteArrayContent(File);
                    await response.Content.LoadIntoBufferAsync(File.LongCount());

                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(typeFile);
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(attachment)
                    {
                        FileName = $"{Result.FileName}.{Result.FileExtension}"
                    };

                    return response;
                }

                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> DownloadPrintLogbook(string id)
        {
            WebClient client = null;
            try
            {
                if (ObjGeneralParametersBll == null)
                {
                    ObjGeneralParametersBll = new GeneralParametersBll(new GeneralParametersDal());
                }

                string[] arrayParams = id.Split('|');
                string userAccount = arrayParams[2].Replace("$", @"\");

                string serverUrl = ObjGeneralParametersBll.ListByFilter(reportServerUrl);
                string folder = ObjGeneralParametersBll.ListByFilter(reportServerFolder);
                string urlReport = serverUrl + $"{folder}%2fTraining%20-%20LogbookParticipationSignatures&GeographicDivisionCode={arrayParams[0]}&LogbookNumber={arrayParams[1]}&LogbookType={arrayParams[2]}&UserAccount={userAccount}&rs:Format=PDF";

                client = new WebClient
                {
                    UseDefaultCredentials = true
                };

                var Result = client.DownloadData(urlReport);

                if (Result.Any())
                {
                    var typeFile = "";
                    switch ("pdf")
                    {
                        case "pdf":
                            typeFile = Pdf;
                            break;
                    }

                    var File = Result;

                    HttpResponseMessage response = new HttpResponseMessage
                    {
                        Content = new ByteArrayContent(File)
                    };
                    await response.Content.LoadIntoBufferAsync(File.LongCount());

                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(typeFile);
                    response.StatusCode = HttpStatusCode.OK;

                    return response;
                }

                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }
        }
    }
}