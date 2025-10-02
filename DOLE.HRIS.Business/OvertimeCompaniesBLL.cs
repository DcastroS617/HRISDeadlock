using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    public class OvertimeCompaniesBLL : IOvertimeCompaniesBLL<OvertimeCompaniesEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private IOvertimeCompaniesDAL<OvertimeCompaniesEntity> overtimeCompaniesDAL;

        // <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="overtimeCompaniesDAL">Data access object</param>
        public OvertimeCompaniesBLL(IOvertimeCompaniesDAL<OvertimeCompaniesEntity> overtimeCompaniesDAL)
        {
            this.overtimeCompaniesDAL = overtimeCompaniesDAL;
        }

        /// <summary>
        /// Save the Over Time Companies
        /// </summary>
        /// <param name="overtimeCompanies">Overtime Companies</param> 
        public bool AddOvertimeCompanies(OvertimeCompaniesEntity overtimeCompanies)
        {
            try
            {
                return overtimeCompaniesDAL.AddOvertimeCompanies(overtimeCompanies);
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
        /// Delete a Overtime Companies
        /// </summary>
        /// <param name="overtimeCompanieCode">Overtime Companie Code</param>
        public bool DeleteOvertimeCompanies(int overtimeCompanieCode)
        {
            try
            {
                return overtimeCompaniesDAL.DeleteOvertimeCompanies(overtimeCompanieCode);
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
        /// Get List the Overtime Companies
        /// /// </summary> 
        /// <param name="overtimeCompanieCode">overtime Companie Code</param>
        /// <param name="overtimeCompanieName">overtime Companie Name</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="sortDirection">sort Direction</param>
        /// <param name="pageNumber">page Number</param>
        /// <param name="pageSize">page Size</param>
        /// <returns>The OvertimeCompaniesEntity List</returns>
        public PageHelper<OvertimeCompaniesEntity> GetOvertimeCompaniesList(int overtimeCompanieCode, string overtimeCompanieName, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            PageHelper<OvertimeCompaniesEntity> response ;
            try
            {
                response = overtimeCompaniesDAL.GetOvertimeCompaniesList(
                    overtimeCompanieCode,
                    overtimeCompanieName,
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
        /// Get OvertimeCompanie By Overtime Companie Code
        /// </summary>        
        /// <param name="overtimeCompanieCode">Overtime Companie Code</param> 
        /// <returns>OvertimeCompaniesEntity</returns>
        public OvertimeCompaniesEntity OvertimeCompaniesByOvertimeCompanieCode(int overtimeCompanieCode)
        {
            try
            {
                return overtimeCompaniesDAL.OvertimeCompaniesByOvertimeCompanieCode(overtimeCompanieCode);
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
        /// Update the Overtime Companies
        /// </summary>
        /// <param name="overtimeCompanies">Overtime Companies Entity</param>
        public bool UpdateOvertimeCompanies(OvertimeCompaniesEntity overtimeCompanies)
        {
            try
            {
                return overtimeCompaniesDAL.UpdateOvertimeCompanies(overtimeCompanies);
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
        /// Get list the Overtime Companie
        /// </summary>
        /// <returns>The OvertimeCompaniesEntity List</returns>
        public List<OvertimeCompaniesEntity> OvertimeCompanieList()
        {
            try
            {
                return overtimeCompaniesDAL.OvertimeCompanieList();
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
