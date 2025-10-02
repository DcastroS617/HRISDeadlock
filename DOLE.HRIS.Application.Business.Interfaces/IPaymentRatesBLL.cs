using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IPaymentRatesBll<T> where T : PaymentRateEntity
    {
        /// <summary>
        /// List the Payment Rates enabled
        /// </summary>
        /// <returns>The Payment Rates</returns>
        List<T> ListEnabled();

        /// <summary>
        /// List the Payment rates by geographic division
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The Payment rates meeting the given filter</returns>
        List<PaymentRateEntity> ListByGeographicDivision(string geographicDivisionCode);

        /// <summary>
        /// List the payment rates by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The payment rates meeting the given filters</returns>
        List<T> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode);
    }
}