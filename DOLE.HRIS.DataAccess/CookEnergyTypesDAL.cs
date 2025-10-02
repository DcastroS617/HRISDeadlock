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
    public class CookEnergyTypesDal : ICookEnergyTypesDal<CookEnergyTypeEntity>
    {
        /// <summary>
        /// List the Cook Energy Types enabled
        /// </summary>
        /// <returns>The Cook Energy Types</returns>
        public List<CookEnergyTypeEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.CookEnergyTypesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new CookEnergyTypeEntity
                {
                    CookEnergyTypeCode = r.Field<byte>("CookEnergyTypeCode"),
                    CookEnergyTypeDescriptionSpanish = r.Field<string>("CookEnergyTypeDescriptionSpanish"),
                    CookEnergyTypeDescriptionEnglish = r.Field<string>("CookEnergyTypeDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjCookEnergyTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionCookEnergyTypesList, ex);
                }
            }
        }
    }
}