using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IUserCodeBLL<T> where T : UserCodeEntity
    {
        /// <summary>
        /// List the UserCode
        /// </summary>        
        /// <returns>UserCode</returns>
        PageHelper<T> GetUserCodeList(int userCodeID, int codeRoleApproverID, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
        
        /// <summary>
        /// Get UserCode Record By UserCodeID
        /// </summary>        
        /// <param name="userCodeID">UserCodeID</param> 
        T UserCodeByUserCodeID(int userCodeID);

        /// <summary>
        /// Save the UserCode
        /// </summary>
        /// <param name="userCode">UserCode</param> 
        bool AddUserCode(UserCodeEntity userCode);

        /// <summary>
        /// Update the UserCode
        /// </summary>
        /// <param name="userCode">UserCode</param>  
        bool UpdateUserCode(UserCodeEntity userCode);

        /// <summary>
        /// Delete an UserCode
        /// </summary>
        /// <param name="userCodeID">UserCodeID</param>
        bool DeleteUserCode(int userCodeID);
    }
}
