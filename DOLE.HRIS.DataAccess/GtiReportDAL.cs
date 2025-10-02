using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;
using static DOLE.HRIS.Shared.Entity.HrisEnum;
using System.Configuration;

namespace DOLE.HRIS.Application.DataAccess
{
    public class GtiReportDAL : IGtiReportDAL
    {
        /// <summary>
        /// Retrieves the Report ID associated with a given report name.
        /// </summary>
        /// <param name="reportName">The name of the report to search for.</param>
        /// <returns>The Report ID if found; otherwise, returns 0.</returns>
        public int GetReportIdByName(string reportName)
        {
            int reportId = 0;

            // Configurar los parámetros para el procedimiento almacenado o consulta
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@ReportName", reportName)
            };

            // Ejecutar la consulta y obtener el DataSet
            var ds = Dal.QueryDataSet("GTI.GetReportIdByName", parameters);

            // Verificar si el DataSet tiene tablas y filas
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Obtener el ReportId desde la primera fila
                var row = ds.Tables[0].Rows[0];
                reportId = Convert.ToInt32(row["ReportId"]);
            }

            return reportId;
        }
    }
}
