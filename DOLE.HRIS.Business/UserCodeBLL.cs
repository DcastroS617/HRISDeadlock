using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System;

namespace DOLE.HRIS.Application.Business
{
    public class UserCodeBLL : IUserCodeBLL<UserCodeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IUserCodeDAL<UserCodeEntity> userCodeDAL;

        // <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="userCodeDAL">Data access object</param>
        public UserCodeBLL(IUserCodeDAL<UserCodeEntity> userCodeDAL)
        {
            this.userCodeDAL = userCodeDAL;
        }

        /// <summary>
        /// Save the UserCode
        /// </summary>
        /// <param name="userCode">User Code Entity</param> 
        public bool AddUserCode(UserCodeEntity userCode)
        {
            try
            {
                return userCodeDAL.AddUserCode(userCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Delete an UserCode
        /// </summary>
        /// <param name="userCodeID">User Code ID</param>
        public bool DeleteUserCode(int userCodeID)
        {
            try
            {
                return userCodeDAL.DeleteUserCode(userCodeID);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// List the UserCode
        /// </summary>        
        /// <param name="userCodeID">user Code ID</param>
        /// <param name="codeRoleApproverID">code Role Approver ID</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <return>The WorkingDayTypesEntity List</return>
        public PageHelper<UserCodeEntity> GetUserCodeList(int userCodeID, int codeRoleApproverID, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<UserCodeEntity> response = new PageHelper<UserCodeEntity>();
            try
            {
                response = userCodeDAL.GetUserCodeList(
                    userCodeID,
                    codeRoleApproverID,
                    sortExpression,
                    sortDirection,
                    pageNumber,
                    null);
                response.TotalPages = (response.TotalResults - 1) / response.PageSize + 1;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
            return response;
        }

        /// <summary>
        /// Save the UserCode
        /// </summary>
        /// <param name="UserCodeEntity">User Code Entity</param> 
        public bool UpdateUserCode(UserCodeEntity userCode)
        {
            try
            {
                return userCodeDAL.UpdateUserCode(userCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Get UserCode Record By UserCodeID
        /// </summary>        
        /// <param name="userCodeID">UserCodeID</param>
        public UserCodeEntity UserCodeByUserCodeID(int userCodeID)
        {
            try
            {
                return userCodeDAL.UserCodeByUserCodeID(userCodeID);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }
    }
}
