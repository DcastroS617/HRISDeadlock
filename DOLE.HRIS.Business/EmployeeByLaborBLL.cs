using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class EmployeeByLaborBll : IEmployeeByLaborBll
    {
        private readonly IEmployeeByLaborDal EmployeeByLaborDal;

        public EmployeeByLaborBll(IEmployeeByLaborDal obj)
        {
            EmployeeByLaborDal = obj;
        }

        /// <summary>
        /// Search employees by filter
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="SortExpression"></param>
        /// <param name="SortDirection"></param>
        /// <returns> A list of employee who match the criteria filter</returns>
        public List<EmployeeByLaborEntity> EmployeeByLaborWithFilter(EmployeeByLaborEntity entity, string SortExpression, string SortDirection)
        {
            try
            {
                return EmployeeByLaborDal.EmployeeByLaborWithFilter(entity, SortExpression, SortDirection);
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
        /// Method to add a employee by labor
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="EMPLOYEES"></param>
        /// <returns>DbaEntity with the id of insert or a message with the error</returns>
        public DbaEntity EmployeeByLaborAdd(EmployeeByLaborEntity entity, DataTable EMPLOYEES)
        {
            try
            {
                return EmployeeByLaborDal.EmployeeByLaborAdd(entity, EMPLOYEES);
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
        /// Method to delete an employe by labor
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>DbaEntity with the id of insert or a message with the error</returns>
        public DbaEntity EmployeeByLaborDelete(EmployeeByLaborEntity entity)
        {
            try
            {
                return EmployeeByLaborDal.EmployeeByLaborDelete(entity);
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
        /// Method to list companies by enabled divisions
        /// </summary>
        /// <param name="DivisionCode"></param>
        /// <returns>A list of enabled companies by division</returns>
        public ListItem[] CompaniesListEnableByDivision(int DivisionCode)
        {
            try
            {
                return EmployeeByLaborDal.CompaniesListEnableByDivision(DivisionCode);
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
        /// Method to list nominal class by division and company
        /// </summary>
        /// <param name="GeographicDivisionID"></param>
        /// <param name="CompanyID"></param>
        /// <returns>a list of enable nominal class by division and companu</returns>
        public ListItem[] NominalClassListEnableByDivision(string GeographicDivisionID, int? CompanyID)
        {
            try
            {
                return EmployeeByLaborDal.NominalClassListEnableByDivision(GeographicDivisionID, CompanyID);
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
        /// Method to list costcenter by division,companies and nominal class code
        /// </summary>
        /// <param name="GeographicDivisionCode"></param>
        /// <param name="CompanyID"></param>
        /// <param name="PayrollClassCode"></param>
        /// <returns>A list of enabled cost center by division and nominal class code,company</returns>
        public ListItem[] CostCenterListEnableByDivision(string GeographicDivisionCode, int? CompanyID, string PayrollClassCode)
        {
            try
            {
                return EmployeeByLaborDal.CostCenterListEnableByDivision(GeographicDivisionCode, CompanyID, PayrollClassCode);
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
        /// Method to list enable positions
        /// </summary>
        /// <param name="CompanyCode"></param>
        /// <param name="PayrollClassCode"></param>
        /// <returns>A list of enabled positions </returns>
        public ListItem[] PositionsListEnabled(int? CompanyCode, string PayrollClassCode)
        {
            try
            {
                return EmployeeByLaborDal.PositionsListEnabled(CompanyCode, PayrollClassCode);
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
    }
}
