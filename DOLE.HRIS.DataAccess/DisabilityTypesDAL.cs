using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Exceptions.Messages;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace DOLE.HRIS.Application.DataAccess
{
    public class DisabilityTypesDal : IDisabilityTypesDal<DisabilityTypeEntity>
    {
        /// <summary>
        /// List the Disability types enabled
        /// </summary>
        /// <returns>The Disability types</returns>
        public List<DisabilityTypeEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.DisabilityTypesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DisabilityTypeEntity
                {
                    DisabilityTypeCode = r.Field<byte>("DisabilityTypeCode"),
                    DisabilityTypeDescriptionSpanish = r.Field<string>("DisabilityTypeDescriptionSpanish"),
                    DisabilityTypeDescriptionEnglish = r.Field<string>("DisabilityTypeDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjDisabilityTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionDisabilityTypesList, ex);
                }
            }
        }
    }
}