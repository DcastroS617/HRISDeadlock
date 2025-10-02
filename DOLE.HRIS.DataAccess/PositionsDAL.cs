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
    public class PositionsDal : IPositionsDal<PositionEntity>
    {
        /// <summary>
        /// List the Positions enabled
        /// </summary>
        /// <returns>The Positions</returns>
        public List<PositionEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PositionsListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new PositionEntity
                {
                    PositionCode = r.Field<string>("PositionCode"),
                    PositionName = r.Field<string>("PositionName"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjPaymentRates), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionPaymentRatesList, ex);
                }
            }
        }

        /// <summary>
        /// List the positions by course: CourseCode
        /// </summary>                
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The positions meeting the given filters</returns>
        public List<PositionEntity> ListByCourse(string geographicDivisionCode,int divisionCode, string courseCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PositionsListByCourse", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@DivisionCode",divisionCode),
                    new SqlParameter("@CourseCode",courseCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PositionEntity
                {
                    PositionCode = r.Field<string>("PositionCode"),
                    PositionName = r.Field<string>("PositionName"),
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(SqlExceptionAnalyzer.AnalyzeException(sqlex, Messages.msjPaymentRates), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(Messages.msjExceptionPaymentRatesList, ex);
                }
            }
        }
    }
}