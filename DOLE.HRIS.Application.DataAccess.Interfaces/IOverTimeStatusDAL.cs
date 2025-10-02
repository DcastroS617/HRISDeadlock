using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess
{
    public interface IOverTimeStatusDAL<T> where T : OverTimeStatusEntity
    {
        /// <summary>
        /// List the OverTimeStatus
        /// </summary>        
        /// <returns>The OverTimeStatus List</returns>
        PageHelper<T> GetOverTimeStatusList(int overTimeStatusCode, string overTimeStatusName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
        
        /// <summary>
        /// Get OverTimeStatus Record By overTimeStatusCode
        /// </summary>        
        /// <param name="overTimeStatusCode">OverTimeStatusCode</param> 
        T OverTimeStatusByOverTimeStatusCode(int overTimeStatusCode);

        /// <summary>
        /// Save the OverTimeStatus
        /// </summary>
        /// <param name="overTimeStatus">OverTimeStatus</param> 
        bool AddOverTimeStatus(OverTimeStatusEntity overTimeStatus);

        /// <summary>
        /// Update the OverTimeStatus
        /// </summary>
        /// <param name="overTimeStatus">OverTimeStatus</param>  
        bool UpdateOverTimeStatus(OverTimeStatusEntity overTimeStatus);

        /// <summary>
        /// Delete an OverTimeStatus
        /// </summary>
        /// <param name="overTimeStatusCode">OverTimeStatusCode</param>
        bool DeleteOverTimeStatus(int overTimeStatusCode);

        /// <summary>
        /// Get OverTimeStatus Record 
        /// </summary>        
        /// <returns>The OverTimeStatus List</returns>
        List<T> GetOverTimeStatusList();
    }
}
