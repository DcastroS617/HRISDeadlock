using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class AbsenteeismCausesLocalDal : IAbsenteeismCausesLocalDal<AbsenteeismCauseLocalEntity>
    {
        /// <summary>
        /// List all the absenteeism cause local
        /// </summary>
        /// <returns></returns>
        public List<AbsenteeismCauseLocalEntity> ListAll()
        {
            try
            {
                var ds = Dal.QueryDataSet("Absenteeism.CausesLocalList");

                var result = ds.Tables[0].AsEnumerable().Select(r => new AbsenteeismCauseLocalEntity
                {
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    AbsenteeismCausesCode = r.Field<string>("AbsenteeismCausesCode"),
                    AbsenteeismCausesDescription = r.Field<string>("AbsenteeismCausesDescription")
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjAbsenteeismCausesLocal), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjAbsenteeismCausesLocalList, ex);
                }
            }
        }
    }
}
