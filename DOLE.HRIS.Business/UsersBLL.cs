using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class UsersBll : IUsersBll<UserEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IUsersDal<UserEntity> UsersDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public UsersBll(IUsersDal<UserEntity> objDal)
        {
            UsersDal = objDal;
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="activeDirectoryUserAccount">The user active directory account</param>
        /// <param name="userName">The user name</param>
        /// <param name="emailAddress">The user email address</param>
        /// <param name="isActive">The user is active or not</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        public void Add(string activeDirectoryUserAccount, string userName, string emailAddress, bool isActive, string lastModifiedUser)
        {
            try
            {
                UsersDal.Add(new UserEntity(activeDirectoryUserAccount, userName, emailAddress, isActive, lastModifiedUser));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionUsersAdd, ex);
                }
            }
        }

        /// <summary>
        /// Count the total registered users
        /// </summary>
        /// <returns>Total users</returns>
        public int Count()
        {
            try
            {
                return UsersDal.Count();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionUsersCount, ex);
                }
            }
        }

        /// <summary>
        /// Update the user information
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="userName">The user name</param>
        /// <param name="emailAddress">The user email address</param>
        /// <param name="isActive">The user is active or not</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        public void Edit(short userCode, string userName, string emailAddress, bool isActive, string lastModifiedUser)
        {
            try
            {
                UsersDal.Edit(new UserEntity(userCode, userName, emailAddress, isActive, lastModifiedUser));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionUsersEdit, ex);
                }
            }
        }

        /// <summary>
        /// List all the active users
        /// </summary>
        /// <returns>A list of users</returns>
        public List<UserEntity> ListActive()
        {
            try
            {
                return UsersDal.ListActive();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionUsersListUsers, ex);
                }
            }
        }

        /// <summary>
        /// List the user information by the active directory account
        /// </summary>
        /// <param name="emailAddress">The user active directory account</param>
        /// <returns>The user</returns>
        public UserEntity ListByActiveDirectoryAccount(string emailAddress)
        {
            try
            {
                return UsersDal.ListByActiveDirectoryAccount(new UserEntity(emailAddress));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionUsersListByActiveDirectoryAccount, ex);
                }
            }
        }

        /// <summary>
        /// List the user by the given filters and page config
        /// </summary>
        /// <param name="divisionCode">THe division code</param>
        /// <param name="userName">The user name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The users</returns>
        public PageHelper<UserEntity> ListByFilters(int divisionCode, string userName, string sortExpression, string sortDirection, int pageNumber)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<UserEntity> response = UsersDal.ListByFilters(divisionCode
                    , userName
                    ,sortExpression
                    , sortDirection
                    , pageNumber
                    , pageSizeValue);

                response.TotalPages = (response.TotalResults - 1) / response.PageSize + 1;

                return response;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionUsersListUsers, ex);
                }
            }
        }

        /// <summary>
        /// List the user for a grid page
        /// </summary>
        /// <param name="pageNumber">The grid page number</param>
        /// <returns>The users</returns>
        public List<UserEntity> ListByPage(int pageNumber)
        {
            try
            {
                return UsersDal.ListByPage(pageNumber);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionUsersListUsers, ex);
                }
            }
        }

        /// <summary>
        /// List the user information by the user code
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <returns>The user</returns>
        public UserEntity ListByUserCode(short userCode)
        {
            try
            {
                return UsersDal.ListByUserCode(new UserEntity(userCode));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionUsersListByUserCode, ex);
                }
            }
        }

        /// <summary>
        /// List the user information by the user code
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <returns>The user</returns>
        public DbaEntity DeleteUser(UserEntity entity)
        {
            try
            {
                return UsersDal.DeleteUser(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionUsersListByUserCode, ex);
                }
            }
        }
       
        /// <summary>
        /// Method to list all Uer by division conde
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <returns> a list of user</returns>
        public List<UserEntity> ListByDivisionCode(int divisionCode)
        {
            List<UserEntity> response = new List<UserEntity>();
            try
            {
                response = UsersDal.ListByDivisionCode(divisionCode);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionUsersListByUserCode, ex);
                }
            }
            return response;
        }
    }
}