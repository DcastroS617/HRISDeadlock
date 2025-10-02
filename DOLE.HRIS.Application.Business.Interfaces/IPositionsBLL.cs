using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IPositionsBll<T> where T : PositionEntity
    {
        /// <summary>
        /// List the Positions enabled
        /// </summary>
        /// <returns>The Positions</returns>
        List<T> ListEnabled();

        /// <summary>
        /// List the positions by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The positions meeting the given filters</returns>
        List<T> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode);
    }
}