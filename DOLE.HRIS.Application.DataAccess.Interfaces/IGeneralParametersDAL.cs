using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IGeneralParametersDal<T> where T : GeneralParameterEntity
    {
        /// <summary>
        /// List a parameterValue by unique criteria
        /// </summary>
        /// <param name="ParameterName">Unique Criteria</param>
        string ListByFilter(string ParameterName);
    }
}