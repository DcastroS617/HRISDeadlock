using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IUsersDal<T> where T: UserEntity
    {
        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="entity">The user information</param>
        void Add(T entity);

        /// <summary>
        /// Count the total registered users
        /// </summary>
        /// <returns>Total users</returns>
        int Count();

        /// <summary>
        /// Update the user information
        /// </summary>
        /// <param name="entity">The user information</param>
        void Edit(T entity);

        /// <summary>
        /// List the user information by the active directory account
        /// </summary>
        /// <param name="entity">The entity to filter, active directory account</param>
        /// <returns>The user</returns>
        T ListByActiveDirectoryAccount(T entity);

        /// <summary>
        /// List the user for a grid page
        /// </summary>
        /// <param name="pageNumber">The grid page number</param>
        /// <returns>The users</returns>
        List<T> ListByPage(int pageNumber);

        /// <summary>
        /// List the user information by the user code
        /// </summary>
        /// <param name="entity">The entity to filter, the user code</param>
        /// <returns>The user</returns>
        T ListByUserCode(T entity);

        /// <summary>
        /// List the user by the given filters and page config
        /// </summary>
        /// <param name="divisionCode">THe division code</param>
        /// <param name="userName">The user name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The users</returns>
        PageHelper<T> ListByFilters(int divisionCode, string userName, string sortExpression, string sortDirection, int pageNumber, int? pageSize);
        
        /// <summary>
        /// List all the active users
        /// </summary>
        /// <returns>A list of users</returns>
        List<T> ListActive();

        /// <summary>
        /// Delete users
        /// </summary>
        /// <returns>return msg error and msg</returns>
        DbaEntity DeleteUser(UserEntity entity);
        List<T> ListByDivisionCode(int divisionCode);
    }
}