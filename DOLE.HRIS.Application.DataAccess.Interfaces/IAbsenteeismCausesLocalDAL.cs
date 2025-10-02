using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IAbsenteeismCausesLocalDal<T> where T: AbsenteeismCauseLocalEntity
    {
        /// <summary>
        /// Get all Asenteeism Local Causes
        /// </summary>
        /// <returns></returns>
        List<T> ListAll();
    }
}
