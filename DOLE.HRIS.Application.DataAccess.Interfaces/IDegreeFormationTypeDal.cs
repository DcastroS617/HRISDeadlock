using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;


namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IDegreeFormationTypeDal<T> where T : DegreeFormationTypeEntity
    {
        /// <summary>
        /// List the academic degrees enabled
        /// </summary>
        /// <returns>The academic degrees</returns>
        List<T> ListEnabled();
    }
}
