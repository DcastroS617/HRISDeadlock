using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class EmployeesBll : IEmployeesBll<EmployeeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IEmployeesDal<EmployeeEntity> EmployeesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public EmployeesBll(IEmployeesDal<EmployeeEntity> objDal)
        {
            EmployeesDal = objDal;            
        }

        /// <summary>
        /// Filter the employees by the geographic division, division and Employee code or ID
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="employeeCode">The employee code</param>
        /// <returns>A list of employees that meet the filters</returns>
        public List<EmployeeEntity> FilterByGeographicDivisionAndEmployeeCode(string geographicDivisionCode, int divisionCode, string employeeCode)
        {
            try
            {
                return EmployeesDal.FilterByGeographicDivisionAndEmployeeCode(new EmployeeEntity(geographicDivisionCode, divisionCode, employeeCode));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjEmployeesList, ex);
                }
            }
        }

        /// <summary>
        /// List the employees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>        
        /// <returns>The employees meeting the given filters</returns>
        public List<EmployeeEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return EmployeesDal.ListByDivision(divisionCode, geographicDivisionCode);                
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjEmployeesList, ex);
                }
            }
        }

        /// <summary>
        /// List the employees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>        
        /// <returns>The employees meeting the given filters</returns>
        public List<EmployeeEntity> ListByDivisionByDepartment(int divisionCode, string geographicDivisionCode, string departmentCode = null)
        {
            try
            {
                return EmployeesDal.ListByDivisionByDepartment(divisionCode, geographicDivisionCode, departmentCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjEmployeesList, ex);
                }
            }
        }

        /// <summary>
        /// List the inactive employees by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>        
        /// <returns>The inactive employees meeting the given filters</returns>
        public List<EmployeeEntity> ListByInactiveDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return EmployeesDal.ListByInactiveDivision(divisionCode, geographicDivisionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjEmployeesList, ex);
                }
            }
        }

        /// <summary>
        /// List the employees by struct by farm or nominal class
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>        
        /// <returns>The employees meeting the given filters</returns>
        public PageHelper<EmployeeEntity> ListByStruct(string geographicDivisionCode, int structBy, DataTable participants, DataTable costZones, DataTable costMiniZones, DataTable costFarms, DataTable companies, DataTable nominalClass, DataTable costCenters, string employee, int divisionCode, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                var pageHelper = EmployeesDal.ListByStruct(geographicDivisionCode
                    , structBy
                    , participants
                    , costZones
                    , costMiniZones
                    , costFarms
                    , companies
                    , nominalClass
                    , costCenters
                    , employee
                    , divisionCode
                    , sortExpression
                    , sortDirection
                    , pageNumber.Value
                    , null
                    , pageSizeValue);

                pageHelper.TotalPages = (pageHelper.TotalResults - 1) / pageHelper.PageSize + 1;

                return pageHelper;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the employee by its employee code and the geographic division
        /// </summary>
        /// <param name="employeeCode">The employee code</param>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <returns>The employee that meet the filters</returns>
        public EmployeeEntity ListByEmployeeCodeGeographicDivision(string employeeCode, string geographicDivisionCode)
        {
            try
            {
                return EmployeesDal.ListByEmployeeCodeGeographicDivision(new EmployeeEntity(employeeCode, geographicDivisionCode));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjEmployeesList, ex);
                }
            }
        }
       
        /// <summary>
        /// List the primary key of the employee that meet the email filter
        /// </summary>
        /// <param name="email">The employee email</param>
        /// <returns>The employee information</returns>
        public EmployeeEntity ListKeyByEmail(string email)
        {
            try
            {
                return EmployeesDal.ListKeyByEmail(email);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjEmployeesList, ex);
                }
            }
        }

        /// <summary>
        /// List the employee filtering by Active Directory Usser Account
        /// </summary>
        /// <param name="userAccount">The usser account</param>
        /// <returns>An employee</returns>
        public EmployeeEntity ListEmployeeByActiveDirectoryUserAccount(string userAccount, string email)
        {
            try
            {
                return EmployeesDal.ListEmployeeByActiveDirectoryUserAccount(userAccount, email);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjEmployeesList, ex);
                }
            }
        }
    }
}