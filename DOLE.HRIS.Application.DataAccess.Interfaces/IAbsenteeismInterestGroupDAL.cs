using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IAbsenteeismInterestGroupDal<T> where T: AbsenteeismInterestGroupEntity
    {
        /// <summary>
        /// Get all Absenteeism Interest Group
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
