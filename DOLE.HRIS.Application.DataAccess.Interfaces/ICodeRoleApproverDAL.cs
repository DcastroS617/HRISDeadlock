using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess
{
    public interface ICodeRoleApproverDAL<T> where T : CodeRoleApproverEntity
    {
        /// <summary>
        /// List the CodeRole Approver
        /// </summary>        
        /// <returns>The Code RoleApprover List</returns>
        PageHelper<T> GetCodeRoleApproverList(string geographicDivisionCode, int divisionCode, int codeRoleApproverID, string roleApprover, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
        
        /// <summary>
        /// Get Code Role Approver By Code RoleApprover ID
        /// </summary>        
        /// <param name="codeRoleApproverID">CodeRoleApproverID</param> 
        T GetCodeRoleApproverByCodeRoleApproverID(int codeRoleApproverID);

        /// <summary>
        /// Valite if name exist
        /// </summary>        
        /// <param name="roleAprovver">roleAprovver</param> 
       T ValidateByName(string roleAprovver);

        /// <summary>
        /// Save the CodeRoleApprover
        /// </summary>
        /// <param name="codeRoleApprover">CodeRoleApprover</param>    
        bool AddCodeRoleApprover(CodeRoleApproverEntity codeRoleApprover);

        /// <summary>
        /// Update the CodeRoleApprover
        /// </summary>
        /// <param name="codeRoleApprover">CodeRoleApprover</param>   
        bool UpdateCodeRoleApprover(CodeRoleApproverEntity codeRoleApprover);

        /// <summary>
        /// Delete a Code Role Approver
        /// </summary>
        /// <param name="codeRoleApproverID">CodeRoleApproverID</param>
        bool DeleteCodeRoleApprover(int codeRoleApproverID);

        /// <summary>
        /// List the CodeRole Approver
        /// </summary>        
        /// <returns>The Code RoleApprover List</returns>
        List<T> GetCodeRoleApproverListForDropdown();
    }
}
