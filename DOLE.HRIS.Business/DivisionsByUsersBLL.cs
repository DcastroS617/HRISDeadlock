using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class DivisionsByUsersBll : IDivisionsByUsersBll<DivisionByUserEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IDivisionsByUsersDal<DivisionByUserEntity> DivisionsByUsersDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public DivisionsByUsersBll(IDivisionsByUsersDal<DivisionByUserEntity> objDal)
        {
            DivisionsByUsersDal = objDal;
        }

        /// <summary>
        /// Add a division for an user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        public void Add(short userCode, int divisionCode, string lastModifiedUser)
        {
            try
            {
                DivisionsByUsersDal.Add(new DivisionByUserEntity(userCode, divisionCode, lastModifiedUser));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionDivisionsByUsersAdd, ex);
                }
            }
        }

        /// <summary>
        /// Delete a division for an user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <param name="divisionCode">The division code</param>
        public void Delete(short userCode, int divisionCode)
        {
            try
            {
                DivisionsByUsersDal.Delete(new DivisionByUserEntity(userCode, divisionCode));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionDivisionsByUsersDelete, ex);
                }
            }
        }
       
        /// <summary>
        /// List de division for an user
        /// </summary>
        /// <param name="userCode">The user code</param>
        /// <returns>The divisions</returns>
        public List<DivisionByUserEntity> ListByUser(short userCode)
        {
            try
            {
                return DivisionsByUsersDal.ListByUser(new DivisionByUserEntity(userCode));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionDivisionsByUserListByUserCode, ex);
                }
            }
        }
    }
}