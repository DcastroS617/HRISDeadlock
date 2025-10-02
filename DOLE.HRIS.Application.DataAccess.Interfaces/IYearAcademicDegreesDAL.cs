using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IYearAcademicDegreesDAL<T> where T : YearAcademicDegreesEntity
    {
        /// <summary>
        /// List the year academic degrees enabled
        /// </summary>
        /// <returns>The year academic degrees</returns>
        List<T> ListEnabled();
        /// <summary>
        /// List the year academic degrees enabled
        /// </summary>
        /// <returns>The year academic degrees</returns>
        List<T> ListAll();

        /// <summary>
        /// Add the academic degrees
        /// </summary>
        /// <param name="entity">The academic degrees</param>
        byte Add(T entity);


        /// <summary>
        /// Delete the academic degrees
        /// </summary>
        /// <param name="entity">The academic degrees</param>
        void Delete(T entity);
    }
}
