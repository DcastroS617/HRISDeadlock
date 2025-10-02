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
    public class GarbageDisposalTypesDal : IGarbageDisposalTypesDal<GarbageDisposalTypeEntity>
    {
        /// <summary>
        /// List the Garbage Disposal Types enabled
        /// </summary>
        /// <returns>The Garbage Disposal Types</returns>
        public List<GarbageDisposalTypeEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.GarbageDisposalTypesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new GarbageDisposalTypeEntity
                {
                    GarbageDisposalTypeCode = r.Field<byte>("GarbageDisposalTypeCode"),
                    GarbageDisposalTypeDescriptionSpanish = r.Field<string>("GarbageDisposalTypeDescriptionSpanish"),
                    GarbageDisposalTypeDescriptionEnglish = r.Field<string>("GarbageDisposalTypeDescriptionEnglish"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjGarbageDisposalTypes), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionGarbageDisposalTypesList, ex);
                }
            }
        }
    }
}