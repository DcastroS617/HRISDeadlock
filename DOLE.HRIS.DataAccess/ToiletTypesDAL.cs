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
    public class ToiletTypesDal : IToiletTypesDal<ToiletTypeEntity>
    {
        /// <summary>
        /// List the Toilet Types enabled
        /// </summary>
        /// <returns>The Toilet Types</returns>
        public List<ToiletTypeEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.ToiletTypesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new ToiletTypeEntity
                {
                    ToiletTypeCode = r.Field<byte>("ToiletTypeCode"),
                    ToiletTypeDescriptionSpanish = r.Field<string>("ToiletTypeDescriptionSpanish"),
                    ToiletTypeDescriptionEnglish = r.Field<string>("ToiletTypeDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjToiletTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionToiletTypesList, ex);
                }
            }
        }
    }
}