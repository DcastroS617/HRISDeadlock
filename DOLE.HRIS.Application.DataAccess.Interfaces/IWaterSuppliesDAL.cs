using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IWaterSuppliesDal<T> where T : WaterSupplyEntity
    {
        /// <summary>
        /// List the Water Supplies enabled
        /// </summary>
        /// <returns>The Water Supplies</returns>
        List<T> ListEnabled();
    }
}