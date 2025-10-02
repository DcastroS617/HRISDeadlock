using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class PaymentRatesBll : IPaymentRatesBll<PaymentRateEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IPaymentRatesDal<PaymentRateEntity> PaymentRatesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public PaymentRatesBll(IPaymentRatesDal<PaymentRateEntity> objDal)
        {
            PaymentRatesDal = objDal;
        }

        /// <summary>
        /// List the Payment Rates enabled
        /// </summary>
        /// <returns>The Payment Rates</returns>
        public List<PaymentRateEntity> ListEnabled()
        {
            try
            {
                return PaymentRatesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionPaymentRatesList, ex);
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
                return PaymentRatesDal.ListByGeographicDivision(geographicDivisionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionPaymentRatesList, ex);
                }
            }
        }

        /// <summary>
        /// List the payment rates by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The payment rates meeting the given filters</returns>
        public List<PaymentRateEntity> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode)
        {
            try
            {
                return PaymentRatesDal.ListByCourse(geographicDivisionCode,divisionCode, courseCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionPaymentRatesList, ex);
                }
            }
        }
    }
}