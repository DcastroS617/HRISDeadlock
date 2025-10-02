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
    public class DegreeFormationTypeDal : IDegreeFormationTypeDal<DegreeFormationTypeEntity>
    {
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        public List<DegreeFormationTypeEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.DegreeFormationTypesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DegreeFormationTypeEntity
                {
                    DegreeFormationTypeCode = r.Field<byte>("DegreeFormationTypeCode"),
                    DegreeFormationTypeDescriptionSpanish = r.Field<string>("DegreeFormationTypeNameSpanish"),
                    DegreeFormationTypeDescriptionEnglish = r.Field<string>("DegreeFormationTypeNameEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjDegreeFormationTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionDegreeFormationTypesList, ex);
                }
            }

        }
    }
}