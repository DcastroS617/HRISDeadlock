using System;
using System.Collections.Generic;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;


namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IIndicatorsDAL<T> where T : IndicatorEntity
    {
        /// <summary>
        /// List all Indicators
        /// </summary>
        /// <returns>List of Indicator Entities</returns>
        List<IndicatorEntity> ListAll();
    }
}
