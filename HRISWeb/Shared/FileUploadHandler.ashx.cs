using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace HRISWeb.Shared
{
    /// <summary>
    /// Summary description for FileUploadHandler
    /// </summary>
    public class FileUploadHandler : IHttpHandler, IRequiresSessionState
    {

        /// <summary>
        /// Handle files upload
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                List<FileEntity> archivos = new List<FileEntity>();
                context.Session["ArchivosTemporales"] = null;
                
                //archivos = (List<AbsenteeismFile>)context.Session["ArchivosTemporales"];

                if (context.Request.Files.Count > 0)
                {
                    HttpFileCollection files = context.Request.Files;

                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        //string documentType = context.Request.Form["documentType"];
                        string company = context.Request.Form["company"];
                        int adamCase = Convert.ToInt32(context.Request.Form["adamCase"]);

                        //string[] componenteNombreArchivo = file.FileName.Split('.');
                        int lastIndex = file.FileName.LastIndexOf('.');
                        string[] componenteNombreArchivo = new string[2] { file.FileName.Substring(0, lastIndex), file.FileName.Substring(lastIndex + 1) };
                        string[] rutaNombreArchivo = componenteNombreArchivo[componenteNombreArchivo.Length - 2].Split('\\');
                        string nombreArchivo = rutaNombreArchivo[rutaNombreArchivo.Length - 1].TrimEnd('.');
                        nombreArchivo = string.IsNullOrWhiteSpace(nombreArchivo) ? "ArchivoSinNombre" : nombreArchivo;
                        string extensionArchivo = componenteNombreArchivo[componenteNombreArchivo.Length - 1];
                        byte[] contenidoArchivo = new byte[file.InputStream.Length];
                        file.InputStream.Read(contenidoArchivo, 0, (int)file.InputStream.Length);

                        archivos.Add(new FileEntity(company, adamCase
                            , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                            , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                            , string.Empty, contenidoArchivo, nombreArchivo, extensionArchivo, UserHelper.GetCurrentFullUserName));
                        
                    }
                }

                context.Session["ArchivosTemporales"] = archivos;

                //context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "application/json";
                context.Response.Write("{}");
            }
            catch (Exception)
            {
                //context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                context.Response.Write("Ha sucedido un problema para administrar el archivo temporalmente para el ausentismo, por favor intente de nuevo la acción o bien contacte al administrador.");
            }
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