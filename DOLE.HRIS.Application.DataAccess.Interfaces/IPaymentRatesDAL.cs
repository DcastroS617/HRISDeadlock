using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IPaymentRatesDal<T> where T : PaymentRateEntity
    {
        /// <summary>
        /// List the Payment rates enabled
        /// </summary>
        /// <returns>The Payment rates</returns>
        List<T> ListEnabled();

        /// <summary>
        /// List the Payment rates by geographic division
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The Payment rates meeting the given filter</returns>
        List<T> ListByGeographicDivision(string geographicDivisionCode);

        /// <summary>
        /// List the payment rates by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division Code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The payment rates meeting the given filters</returns>
        List<PaymentRateEntity> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode);
    }
}