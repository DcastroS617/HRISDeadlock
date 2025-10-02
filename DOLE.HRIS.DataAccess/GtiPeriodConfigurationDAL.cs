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
    public class GtiPeriodConfigurationDAL : IGtiPeriodConfigurationDAL
    {
        /// <summary>
        /// Adds a new Period Campaign Configuration or returns an error if it already exists.
        /// </summary>
        /// <param name="configuration">The configuration entity to add.</param>
        /// <returns>The resulting configuration entity with error information, if any.</returns>
        public CreatePeriodCampaignConfigurationEntity AddOrUpdateConfiguration(CreatePeriodCampaignConfigurationEntity configuration)
        {
            var parameterIdsString = configuration.GetParameterIdsAsString();

            var ds = Dal.QueryDataSet("GTI.sp_CreatePeriodCampaignConfiguration", new SqlParameter[] {
                    new SqlParameter("@PeriodCampaignId", configuration.PeriodCampaignId),
                    new SqlParameter("@gti", configuration.Gti),
                    new SqlParameter("@hmt", configuration.Hmt),
                    new SqlParameter("@hle", configuration.Hle),
                    new SqlParameter("@dui", configuration.Dui),
                    new SqlParameter("@qke", configuration.Qke),
                    new SqlParameter("@sct", configuration.Sct),
                    new SqlParameter("@gti_precheck", configuration.GtiPrecheck),
                    new SqlParameter("@hmt_precheck", configuration.HmtPrecheck),
                    new SqlParameter("@hle_precheck", configuration.HlePrecheck),
                    new SqlParameter("@dui_precheck", configuration.DuiPrecheck),
                    new SqlParameter("@qke_precheck", configuration.QkePrecheck),
                    new SqlParameter("@sct_precheck", configuration.SctPrecheck),
                    new SqlParameter("@ParameterIds", parameterIdsString == "" ? DBNull.Value : (object)parameterIdsString)
                });

            var result = ds.Tables[0].AsEnumerable().Select(r => new CreatePeriodCampaignConfigurationEntity
            {
                PeriodCampaignId = r.Field<int>("PeriodCampaignId"),
                ErrorNumber = r.Field<int>("MsgCode"),
                ErrorMessage = r.Field<string>("MsgError")
            }).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Marks a Period Campaign Configuration as deleted.
        /// </summary>
        /// <param name="configurationId">The ID of the configuration to delete.</param>
        public void DeleteConfiguration(int configurationId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a Period Campaign Configuration by its ID.
        /// </summary>
        /// <param name="configurationId">The ID of the configuration to retrieve.</param>
        /// <returns>The configuration entity if found; otherwise, null.</returns>
        public CreatePeriodCampaignConfigurationEntity ListConfigurationByKey(int configurationId)
        {
            var ds = Dal.QueryDataSet("GTI.GetPeriodCampaignConfigurationByKey", new SqlParameter[] {
        new SqlParameter("@PeriodCampaignConfigurationId", configurationId)
    });

            var result = ds.Tables[0].AsEnumerable().Select(r => new CreatePeriodCampaignConfigurationEntity
            {
                PeriodCampaignId = r.Field<int>("PeriodCampaignId"),
                Gti = r.Field<bool>("gti"),
                Hmt = r.Field<bool>("hmt"),
                Hle = r.Field<bool>("hle"),
                Dui = r.Field<bool>("dui"),
                Qke = r.Field<bool>("qke"),
                Sct = r.Field<bool>("sct"),
                GtiPrecheck = r.Field<bool>("gti_precheck"),
                HmtPrecheck = r.Field<bool>("hmt_precheck"),
                HlePrecheck = r.Field<bool>("hle_precheck"),
                DuiPrecheck = r.Field<bool>("dui_precheck"),
                QkePrecheck = r.Field<bool>("qke_precheck"),
                SctPrecheck = r.Field<bool>("sct_precheck"),
                // Populate ParameterIds if necessary
            }).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Lists all active Period Campaign Configurations.
        /// </summary>
        /// <returns>A list of all active configurations.</returns>
        public List<CreatePeriodCampaignConfigurationEntity> ListAllConfigurations()
        {
            var ds = Dal.QueryDataSet("GTI.ListAllPeriodCampaignConfigurations");

            var result = ds.Tables[0].AsEnumerable().Select(r => new CreatePeriodCampaignConfigurationEntity
            {
                PeriodCampaignId = r.Field<int>("PeriodCampaignId"),
                Gti = r.Field<bool>("gti"),
                Hmt = r.Field<bool>("hmt"),
                Hle = r.Field<bool>("hle"),
                Dui = r.Field<bool>("dui"),
                Qke = r.Field<bool>("qke"),
                Sct = r.Field<bool>("sct"),
                GtiPrecheck = r.Field<bool>("gti_precheck"),
                HmtPrecheck = r.Field<bool>("hmt_precheck"),
                HlePrecheck = r.Field<bool>("hle_precheck"),
                DuiPrecheck = r.Field<bool>("dui_precheck"),
                QkePrecheck = r.Field<bool>("qke_precheck"),
                SctPrecheck = r.Field<bool>("sct_precheck"),
                // Populate ParameterIds if necessary
            }).ToList();

            return result;
        }

        /// <summary>
        /// Lists all parameters associated with a given configuration.
        /// </summary>
        /// <param name="configurationId">The ID of the configuration.</param>
        /// <returns>A list of parameter IDs associated with the configuration.</returns>
        public List<int> ListParametersByConfigurationId(int configurationId)
        {
            var ds = Dal.QueryDataSet("GTI.ListParametersByConfigurationId", new SqlParameter[] {
                    new SqlParameter("@PeriodCampaignConfigurationId", configurationId)
                });

            var result = ds.Tables[0].AsEnumerable().Select(r => r.Field<int>("PeriodParameterDivisionCurrencyId")).ToList();

            return result;
        }

        /// <summary>
        /// Adds or updates a period configuration in the database.
        /// </summary>
        /// <param name="periodConfigurationEntity">The entity to be added or updated</param>
        /// <returns>Returns the updated or added PeriodConfigurationRequestEntity</returns>
        public PeriodConfigurationEntity ConfigurationAddorUpdate(PeriodConfigurationEntity periodConfigurationEntity)
        {
            // Crear DataTable para la lista de reportes
            DataTable reportTable = new DataTable();
            reportTable.Columns.Add("ReportId", typeof(int));
            reportTable.Columns.Add("IsEnabled", typeof(bool));

            foreach (var report in periodConfigurationEntity.ConfigurationReports)
            {
                reportTable.Rows.Add(report.ReportId, report.IsEnabled);
            }

            // Crear DataTable para la lista de parámetros
            DataTable parameterTable = new DataTable();
            parameterTable.Columns.Add("ReportId", typeof(int));
            parameterTable.Columns.Add("PeriodParameterDivisionCurrencyId", typeof(int));
            parameterTable.Columns.Add("RequiresPreApproval", typeof(bool));
            parameterTable.Columns.Add("ProcessResponsible", typeof(string));
            parameterTable.Columns.Add("ExchangeRate", typeof(decimal));

            // Iterar sobre los reportes y sus parámetros
            foreach (var report in periodConfigurationEntity.ConfigurationReports)
            {
                foreach (var param in report.ConfigurationParameters)
                {
                    parameterTable.Rows.Add(
                        report.ReportId, // Usamos el ReportId del reporte actual
                        param.PeriodParameterDivisionCurrencyId,
                        param.RequiresPreApproval,
                        param.ProcessResponsible,
                        (object)param.ExchangeRate ?? DBNull.Value
                    );
                }
            }

            // Configurar los parámetros para el procedimiento almacenado
            var parameters = new List<SqlParameter>{
                                                        new SqlParameter("@PeriodCampaignId", periodConfigurationEntity.PeriodCampaignId),
                                                        new SqlParameter("@ConfigurationState", periodConfigurationEntity.ConfigurationState),
                                                        new SqlParameter("@ReportList", SqlDbType.Structured)
                                                            {
                                                                TypeName = "GTI.ReportType",
                                                                Value = reportTable
                                                            },
                                                        new SqlParameter("@ParameterList", SqlDbType.Structured)
                                                            {
                                                                TypeName = "GTI.ConfigurationParameterType",
                                                                Value = parameterTable
                                                            }
                                                    };

            // Ejecutar el procedimiento almacenado y obtener el DataSet
            var ds = Dal.QueryDataSet("GTI.ConfigurationAddorUpdate", parameters.ToArray());

            // Mapear el resultado al PeriodConfigurationEntity
            var result = ds.Tables[0].AsEnumerable().Select(r => new PeriodConfigurationEntity
            {
                ConfigurationId = r.Field<int>("ConfigurationId"),
                ErrorNumber = r.Field<int>("MsgCode"),
                ErrorMessage = r.Field<string>("MsgError")
            }).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Gets a Period Campaign Configuration by its ID.
        /// </summary>
        /// <param name="configurationId">The ID of the configuration to retrieve.</param>
        /// <returns>The configuration entity if found; otherwise, null.</returns>
        public PeriodConfigurationEntity GetDraftConfigurationByPeriod(int periodCampaignId)
        {
            // Assuming that ConfigurationState = 1 represents "Draft"
            int draftState = 1;
            return GetConfigurationByPeriodAndState(periodCampaignId, draftState);
        }

        /// <summary>
        /// Retrieves a configuration by period and state.
        /// </summary>
        /// <param name="periodCampaignId">The ID of the period campaign.</param>
        /// <param name="configurationState">The state of the configuration.</param>
        /// <returns>The configuration entity matching the criteria, or null if not found.</returns>
        public PeriodConfigurationEntity GetConfigurationByPeriodAndState(int periodCampaignId, int configurationState)
        {
            // Configurar los parámetros para el procedimiento almacenado
            var parameters = new SqlParameter[]
            {
        new SqlParameter("@PeriodCampaignId", periodCampaignId),
        new SqlParameter("@ConfigurationState", configurationState)
            };

            // Ejecutar el procedimiento almacenado y obtener el DataSet
            var ds = Dal.QueryDataSet("GTI.GetConfigurationByPeriodAndState", parameters);

            // Verificar si el DataSet tiene tablas y filas
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Mapear la configuración desde la primera tabla
                var configRow = ds.Tables[0].Rows[0];

                var configuration = new PeriodConfigurationEntity
                {
                    ConfigurationId = Convert.ToInt32(configRow["ConfigurationId"]),
                    PeriodCampaignId = Convert.ToInt32(configRow["PeriodCampaignId"]),
                    ConfigurationState = Convert.ToInt32(configRow["ConfigurationState"]),
                    ConfigurationReports = new List<PeriodConfigurationReportEntity>()
                };

                // Mapear los reportes desde la segunda tabla
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    var reportsTable = ds.Tables[1];

                    var reports = reportsTable.AsEnumerable().Select(r => new PeriodConfigurationReportEntity
                    {
                        ConfigurationReportId = r.Field<int>("ConfigurationReportId"),
                        ConfigurationId = r.Field<int>("ConfigurationId"),
                        ReportId = r.Field<int>("ReportId"),
                        IsEnabled = r.Field<bool>("IsEnabled"),
                        ConfigurationParameters = new List<PeriodConfigurationParameterEntity>()
                    }).ToList();

                    configuration.ConfigurationReports = reports;
                }

                // Mapear los parámetros desde la tercera tabla
                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    var parametersTable = ds.Tables[2];

                    var parametersList = parametersTable.AsEnumerable().Select(r => new PeriodConfigurationParameterEntity
                    {
                        PeriodConfigurationParameterId = r.Field<int>("ConfigurationParameterId"),
                        ConfigurationReportId = r.Field<int>("ConfigurationReportId"),
                        PeriodParameterDivisionCurrencyId = r.Field<int>("PeriodParameterDivisionCurrencyId"),
                        RequiresPreApproval = r.Field<bool>("RequiresPreApproval"),
                        ProcessResponsible = r.Field<string>("ProcessResponsible"),
                        ExchangeRate = r.Field<decimal?>("ExchangeRate")
                    }).ToList();

                    // Asociar los parámetros con los reportes correspondientes
                    foreach (var report in configuration.ConfigurationReports)
                    {
                        report.ConfigurationParameters = parametersList
                            .Where(p => p.ConfigurationReportId == report.ConfigurationReportId)
                            .ToList();
                    }
                }

                return configuration;
            }
            else
            {
                // Si no hay datos, retornar null
                return null;
            }
        }

    }
}
