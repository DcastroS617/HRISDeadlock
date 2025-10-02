using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    /// <summary>
    /// Business Logic class to communicate with the corresponding data access class(DivisionsDal)
    /// </summary>
    public class DivisionsBll : IDivisionsBll<DivisionEntity>
    {        
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IDivisionsDal<DivisionEntity> DivisionsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public DivisionsBll(IDivisionsDal<DivisionEntity> objDal)
        {
            DivisionsDal = objDal;
        }

        /// <summary>
        /// List the divisions defined
        /// </summary>
        /// <returns>A list of division entity</returns>
        public List<DivisionEntity> ListAll()
        {
            try
            {
                return DivisionsDal.ListAll();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(Shared.ErrorMessages.msjBusinessLogicExceptionListDivisionsAll, ex);
                }
            }
        }

        /// <summary>
        /// List divisions defined
        /// </summary>
        /// <returns>A list of Division entity</returns>
        public List<KeyValuePair<DivisionEntity, string>> ListAllWithGeographicDivision()
        {
            try
            {
                return DivisionsDal.ListAllWithGeographicDivision();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(Shared.ErrorMessages.msjBusinessLogicExceptionListDivisionsAll, ex);
                }
            }
        }

        /// <summary>
        /// List divisions defined by Active employess
        /// </summary>
        /// <returns>A list of Division entity</returns>
        public List<DivisionByActiveEmployeesEntity> ListAllDivisionByActiveEmployee()
        {
            try
            {
                return DivisionsDal.ListAllDivisionByActiveEmployee();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(Shared.ErrorMessages.msjBusinessLogicExceptionListDivisionsAll, ex);
                }
            }
        }

    }
}
