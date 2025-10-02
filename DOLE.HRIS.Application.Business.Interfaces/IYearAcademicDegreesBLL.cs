using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IYearAcademicDegreesBLL<T> where T : YearAcademicDegreesEntity
    {
        /// <summary>
        /// List the YearAcademicDegrees
        /// </summary>
        /// <returns>The YearAcademicDegrees</returns>
        List<T> ListEnabled();

        /// <summary>
        /// List ALL the YearAcademicDegrees
        /// </summary>
        /// <returns>The YearAcademicDegrees</returns>
        List<T> ListAll();

        /// <summary>
        /// Add the YearAcademicDegrees
        /// </summary>
        /// <param name="entity">The Principal Profession</param>
        Tuple<bool, T> Add(T entity);

        /// <summary>
        /// Delete the YearAcademicDegrees
        /// </summary>
        /// <param name="entity">The YearAcademicDegrees</param>
        void Delete(T entity);
    }
}
