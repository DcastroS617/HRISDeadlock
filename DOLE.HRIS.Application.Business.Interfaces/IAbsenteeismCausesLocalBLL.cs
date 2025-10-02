using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IAbsenteeismCausesLocalBll<T> where T : AbsenteeismCauseLocalEntity
    {
        /// <summary>
        /// List all Absenteeism Local Causes
        /// </summary>
        /// <returns></returns>
        List<T> ListAll();
    }
}
