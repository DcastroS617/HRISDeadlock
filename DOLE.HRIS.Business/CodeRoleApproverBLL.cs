using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    public class CodeRoleApproverBLL : ICodeRoleApproverBLL<CodeRoleApproverEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private ICodeRoleApproverDAL<CodeRoleApproverEntity> codeRoleApproverDAL;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="codeRoleApproverDAL">Data access object</param>
        public CodeRoleApproverBLL(ICodeRoleApproverDAL<CodeRoleApproverEntity> codeRoleApproverDAL)
        {
            this.codeRoleApproverDAL = codeRoleApproverDAL;
        }

        /// <summary>
        /// Get list the Code Role Approver
        /// </summary>     
        /// <param name="codeRoleApproverID">code Role Approver ID</param>
        /// <param name="roleApprover">role Approver</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The CodeRoleApproverEntity List</returns>
        public PageHelper<CodeRoleApproverEntity> GetCodeRoleApproverList(string geographicDivisionCode, int divisionCode, int codeRoleApproverID, string roleApprover, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<CodeRoleApproverEntity> response = new PageHelper<CodeRoleApproverEntity>();
            try
            {
                response = codeRoleApproverDAL.GetCodeRoleApproverList(
                    geographicDivisionCode
                    , divisionCode
                    , codeRoleApproverID
                    , roleApprover
                    , sortExpression
                    , sortDirection
                    , pageNumber
                    , pageSize);
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
        /// Get Code Role Approver By Code Role Approver ID
        /// </summary>        
        /// <param name="codeRoleApproverID">Code Role Approver ID</param>
        /// <returns>CodeRoleApproverEntity</returns>
        public CodeRoleApproverEntity GetCodeRoleApproverByCodeRoleApproverID(int codeRoleApproverID)
        {
            try
            {
                return codeRoleApproverDAL.GetCodeRoleApproverByCodeRoleApproverID(codeRoleApproverID);
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
        /// Save the Code Role Approver
        /// </summary>
        /// <param name="codeRoleApproverEntity">code Role Approver Entity</param> 
        public Tuple<bool,int> AddCodeRoleApprover(CodeRoleApproverEntity codeRoleApprover)
        {
            try
            {
                CodeRoleApproverEntity codeRolePrevius = codeRoleApproverDAL.ValidateByName(codeRoleApprover.RoleApprover);
                if (codeRolePrevius == null)
                {
                    return new Tuple<bool, int>(codeRoleApproverDAL.AddCodeRoleApprover(codeRoleApprover), 1);
                }
                else 
                {
                    return new Tuple<bool, int>(false, codeRolePrevius.CodeRoleApproverID);
                }
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
        /// Update the Code Role Approver
        /// </summary>
        /// <param name="codeRoleApproverEntity">code Role Approver Entity</param>  
        public bool UpdateCodeRoleApprover(CodeRoleApproverEntity codeRoleApprover)
        {
            try
            {
                return codeRoleApproverDAL.UpdateCodeRoleApprover(codeRoleApprover);
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
        /// Delete a Code Role Approver
        /// </summary>
        /// <param name="codeRoleApproverID">Code Role Approver ID</param>
        public bool DeleteCodeRoleApprover(int codeRoleApproverID)
        {
            try
            {
                return codeRoleApproverDAL.DeleteCodeRoleApprover(codeRoleApproverID);
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
        /// Get Code Role Approver For Dropdown
        /// </summary>        
        /// <returns>The CodeRoleApproverEntity list</returns>
        public List<CodeRoleApproverEntity> GetCodeRoleApproverListForDropdown()
        {
            try
            {
                return codeRoleApproverDAL.GetCodeRoleApproverListForDropdown();
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
