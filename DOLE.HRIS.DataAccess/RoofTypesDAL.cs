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
    public class RoofTypesDal : IRoofTypesDal<RoofTypeEntity>
    {
        /// <summary>
        /// List the Roof Types enabled
        /// </summary>
        /// <returns>The Roof Types</returns>
        public List<RoofTypeEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.RoofTypesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new RoofTypeEntity
                {
                    RoofTypeCode = r.Field<byte>("RoofTypeCode"),
                    RoofTypeDescriptionSpanish = r.Field<string>("RoofTypeDescriptionSpanish"),
                    RoofTypeDescriptionEnglish = r.Field<string>("RoofTypeDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjRoofTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionRoofTypesList, ex);
                }
            }
        }
    }
}