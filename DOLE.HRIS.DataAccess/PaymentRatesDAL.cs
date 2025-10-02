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
    public class PaymentRatesDal : IPaymentRatesDal<PaymentRateEntity>
    {
        /// <summary>
        /// List the Payment Rates enabled
        /// </summary>
        /// <returns>The Payment Rates</returns>
        public List<PaymentRateEntity> ListEnabled()
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PaymentRatesListEnabled");

                var result = ds.Tables[0].AsEnumerable().Select(r => new PaymentRateEntity
                {
                    PaymentRateCode = r.Field<int>("PaymentRateCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ShortDescription = r.Field<string>("ShortDescription"),
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
        /// List the Payment rates by geographic division
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The Payment rates meeting the given filter</returns>
        public List<PaymentRateEntity> ListByGeographicDivision(string geographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PaymentRatesListByGeographicDivision", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PaymentRateEntity
                {
                    PaymentRateCode = r.Field<int>("PaymentRateCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ShortDescription = r.Field<string>("ShortDescription"),
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
        /// List the payment rates by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The payment rates meeting the given filters</returns>
        public List<PaymentRateEntity> ListByCourse(string geographicDivisionCode,int divisionCode ,string courseCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PaymentRatesListByCourse", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",geographicDivisionCode),
                    new SqlParameter("@DivisionCode",geographicDivisionCode),
                    new SqlParameter("@CourseCode",courseCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PaymentRateEntity
                {
                    PaymentRateCode = r.Field<int>("PaymentRateCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    ShortDescription = r.Field<string>("ShortDescription"),
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