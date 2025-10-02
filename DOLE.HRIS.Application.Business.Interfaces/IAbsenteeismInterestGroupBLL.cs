using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IAbsenteeismInterestGroupBll<T> where T : AbsenteeismInterestGroupEntity
    {
        /// <summary>
        /// List all Absenteeism Interest Group
        /// </summary>
        /// <returns></returns>
        List<T> ListAll();

        /// <summary>
        /// Update InterestGroups in HRIS getting data from ADAM
        /// </summary>
        /// <returns></returns>
        void UpdateInterestGroupFromADAM(DataTable datatable, string divisionCode);
    }
}
