using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IGeneralParametersBll<T> where T : GeneralParameterEntity
    {
        /// <summary>
        /// List a parameterValue by unique criteria
        /// </summary>
        /// <param name="ParameterName">Unique Criteria</param>
        string ListByFilter(string ParameterName);
    }
}