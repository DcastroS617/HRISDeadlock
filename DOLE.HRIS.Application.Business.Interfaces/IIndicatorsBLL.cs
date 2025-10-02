using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IIndicatorsBll<T> where T : IndicatorEntity
    {
        /// <summary>
        /// Lists all indicators.
        /// </summary>
        /// <returns>List of all indicators.</returns>
        List<IndicatorEntity> ListAll();
    }
}
