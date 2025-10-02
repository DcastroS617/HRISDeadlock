using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IPositionsDal<T> where T : PositionEntity
    {
        /// <summary>
        /// List the positions enabled
        /// </summary>
        /// <returns>The positions</returns>
        List<T> ListEnabled();

        /// <summary>
        /// List the Positions by course: CourseCode
        /// </summary>                
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The positions meeting the given filters</returns>
        List<T> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode);
    }
}