using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Exceptions.Messages;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class GeneralConfigurationsDal : IGeneralConfigurationsDal
    {
        /// <summary>
        /// List the general configurations by code
        /// </summary>
        /// <param name="configurationCode">The general configuration to retrieve</param>
        /// <returns>The general configuration</returns>
        public GeneralConfigurationEntity ListByCode(HrisEnum.GeneralConfigurations configurationCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.GeneralConfigurationsListByCode", new SqlParameter[] {
                    new SqlParameter("@GeneralConfigurationCode", (short)configurationCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new GeneralConfigurationEntity
                {
                    GeneralConfigurationCode = r.Field<short>("GeneralConfigurationCode"),
                    GeneralConfigurationName = r.Field<string>("GeneralConfigurationName"),
                    GeneralConfigurationDescription = r.Field<string>("GeneralConfigurationDescription"),
                    GeneralConfigurationValue = r.Field<string>("GeneralConfigurationValue"),
                    IsPublic = r.Field<bool>("IsPublic"),
                }).FirstOrDefault();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msgGeneralConfigurations), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgExceptionGeneralConfigurationList, ex);
                }
            }
        }
    }
}