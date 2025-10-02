using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Exceptions.Messages;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.DataAccess
{
    public class DaysWeekDal : IDaysWeekDal<DaysWeekEntity>
    {
        public List<DaysWeekEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("HRIS.DaysWeekListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new DaysWeekEntity
                {
                    DaysWeekCode = r.Field<byte>("DaysWeekCode"),
                    DaysWeekDescriptionSpanish = r.Field<string>("DaysWeekDescriptionSpanish"),
                    DaysWeekDescriptionEnglish = r.Field<string>("DaysWeekDescriptionEnglish")
                }).ToList();
                return result;
            }
            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msgDaysWeek), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msgDaysWeek, ex);
                }
            }
        }
    }
}
