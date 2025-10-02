using DOLE.HRIS.Exceptions;
using System;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web;
using Unity;
using Unity.Web;
using DOLE.HRIS.Application.Business.Interfaces;

using static System.String;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace HRISWeb.Shared
{
    /// <summary>
    /// Manejador generico para descargar de archivos
    /// </summary>
    public class FileDownloadHandler : IHttpHandler
    {
        #region Private members
        private IAbsenteeismFileBll<FileEntity> objAbsenteeismFileBll = null;
        #endregion

        /// <summary>
        /// Handle download File
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.Clear();
                DownloadAbsenteeismFile(context, Convert.ToInt32(context.Request.QueryString["idAbsenteeismFile"]));
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="idFile">File Id</param>
        private void DownloadAbsenteeismFile(HttpContext context, int idFile)
        {
            try
            {
                objAbsenteeismFileBll = context.Application.GetContainer().Resolve<IAbsenteeismFileBll<FileEntity>>();
                List<FileEntity> listFile = objAbsenteeismFileBll.GetFileByKey(idFile);
                
                string fullFileName = Convert.ToString(listFile[0].FileName + "." + listFile[0].FileExtension);
                byte[] file = (byte[])listFile[0].File;
                DownloadFile(context, fullFileName, file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileFullName"></param>
        /// <param name="file"></param>
        public static void DownloadFile(HttpContext context, string fileFullName, byte[] file)
        {   
            context.Response.Clear();
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileFullName);
            context.Response.OutputStream.Write(file, 0, file.Length);
            context.Response.Flush();
            context.Response.Close();
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