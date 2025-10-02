using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDivisionsBll<T> where T : DivisionEntity
    {
        /// <summary>
        /// List divisions
        /// </summary>
        /// <returns>A list of Division entity</returns>
        List<T> ListAll();

        /// <summary>
        /// List divisions defined by Geographic division
        /// </summary>
        /// <returns>A list of Division entity</returns>
        List<KeyValuePair<T, string>> ListAllWithGeographicDivision();

        /// <summary>
        /// List divisions defined by Active employess
        /// </summary>
        /// <returns>A list of Division entity</returns>
        List<DivisionByActiveEmployeesEntity> ListAllDivisionByActiveEmployee();
    }
}
