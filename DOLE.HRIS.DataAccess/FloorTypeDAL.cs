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
    public class FloorTypesDal : IFloorTypesDal<FloorTypeEntity>
    {
        /// <summary>
        /// List the FloorTypes enabled
        /// </summary>
        /// <returns>The FloorTypes</returns>
        public List<FloorTypeEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.FloorTypesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new FloorTypeEntity
                {
                    FloorTypeCode = r.Field<byte>("FloorTypeCode"),
                    FloorTypeDescriptionSpanish = r.Field<string>("FloorTypeDescriptionSpanish"),
                    FloorTypeDescriptionEnglish = r.Field<string>("FloorTypeDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjFloorTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionFloorTypesList, ex);
                }
            }
        }
    }
}