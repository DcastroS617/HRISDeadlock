using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IUsersBll<T> where T : UserEntity
    {
        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="activeDirectoryUserAccount">The user active directory account</param>
        /// <param name="userName">The user name</param>
        /// <param name="emailAddress">The user email address</param>
        /// <param name="isActive">The user is active or not</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        void Add(string activeDirectoryUserAccount, string userName, string emailAddress, bool isActive, string lastModifiedUser);
       
        /// <summary>
        /// Count the total registered users
        /// </summary>
        /// <returns>Total users</returns>
        int Count();

        /// <summary>
        /// Update the user information
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="userName">The user name</param>
        /// <param name="emailAddress">The user email address</param>
        /// <param name="isActive">The user is active or not</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        void Edit(short userCode, string userName, string emailAddress, bool isActive, string lastModifiedUser);

        /// <summary>
        /// List the user information by the active directory account
        /// </summary>
        /// <param name="emailAddress">The user email address in its current active directory account</param>
        /// <returns>The user</returns>
        T ListByActiveDirectoryAccount(string emailAddress);

        /// <summary>
        /// List the user for a grid page
        /// </summary>
        /// <param name="pageNumber">The grid page number</param>
        /// <returns>The users</returns>
        List<T> ListByPage(int pageNumber);

        /// <summary>
        /// List the user information by the user code
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <returns>The user</returns>
        T ListByUserCode(short userCode);

        /// <summary>
        /// List the user by the given filters and page config
        /// </summary>
        /// <param name="divisionCode">THe division code</param>
        /// <param name="userName">The user name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The users</returns>
        PageHelper<T> ListByFilters(int divisionCode, string userName, string sortExpression, string sortDirection, int pageNumber);
       
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
        List<UserEntity> ListByDivisionCode(int divisionCode);
    }
}