using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IWaterSuppliesBll<T> where T : WaterSupplyEntity
    {
        /// <summary>
        /// List the Water Supplies enabled
        /// </summary>
        /// <returns>The Water Supplies</returns>
        List<T> ListEnabled();
    }
}