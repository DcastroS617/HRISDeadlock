using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IDegreeFormationTypeBll<T> where T : DegreeFormationTypeEntity
    {
        /// <summary>
        /// List the Degree Formation type
        /// </summary>
        /// <returns>The Degree Formation type</returns>
        List<T> ListEnabled();
    }
}