using System;
using System.Configuration;
using System.Linq;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using DOLE.HRIS.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    public class DeprivationsBLL : IDeprivationsBLL<DeprivationEntity>
    {
        private readonly IDeprivationsDAL<DeprivationEntity> DeprivationsDal;

        /// <summary>
        /// Constructor for InitiativesBll.
        /// </summary>
        /// <param name="objDal">Data access object for Initiatives.</param>
        public DeprivationsBLL(IDeprivationsDAL<DeprivationEntity> objDal)
        {
            DeprivationsDal = objDal;
        }

        /// <summary>
        /// List Individuals by Filters
        /// </summary>
        /// <param name="indicatorCode">Indicator Code</param>
        /// <param name="divisionCode">Division Name</param>
        /// <param name="companyCode">Company Name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <returns>List of Individual Entities</returns>
        public PageHelper<DeprivationEntity> ListByFilters(
            int? indicatorCode,
            string divisionCode,
            string companyCode,
            string sortExpression,
            string sortDirection,
            int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<DeprivationEntity> pageHelper = DeprivationsDal.ListByFilters(
                    indicatorCode,
                    divisionCode, companyCode,
                    sortExpression, sortDirection, pageNumber.Value, null, pageSizeValue);

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
        /// List Surveys by Filters
        /// </summary>
        /// <param name="employeeCode">Employee Code</param>
        /// <returns>List of Survey Entities</returns>
        public PageHelper<HouseholdDeprivationEntity> ListByEmployee(
            string employeeCode)
        {
            try
            {

                PageHelper<HouseholdDeprivationEntity> pageHelper = DeprivationsDal.ListByEmployee(
                    employeeCode);

                pageHelper.TotalPages = 1;

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
    }
}
