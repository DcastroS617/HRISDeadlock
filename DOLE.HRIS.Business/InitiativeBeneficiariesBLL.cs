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
    public class InitiativeBeneficiariesBLL : IInitiativeBeneficiariesBLL<InitiativeBeneficiaries>
    {
        private readonly IInitiativeBeneficiariesDAL<InitiativeBeneficiaries> InitiativesDal;

        /// <summary>
        /// Constructor for InitiativesBll.
        /// </summary>
        /// <param name="objDal">Data access object for Initiatives.</param>
        public InitiativeBeneficiariesBLL(IInitiativeBeneficiariesDAL<InitiativeBeneficiaries> objDal)
        {
            InitiativesDal = objDal;
        }

        /// <summary>
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Coordinator code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public PageHelper<InitiativeBeneficiaries> ListByFilters(
            int initiativeCode,
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

                PageHelper<InitiativeBeneficiaries> pageHelper = InitiativesDal.ListByFilters( initiativeCode,
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
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Coordinator code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public PageHelper<IndividualsDeprivations> IndividualsDeprivationsByFilters(
            int? initiativeCode,
            int?   poverty,
            string gender,
            string familyRelationship,
            int?   startAge,
            int?   endAge,
            int?   startSeniority,
            int?   endSeniority,
            decimal? startPovertyScore,
            decimal? endPovertyScore,
            string employeeCode,
            string employeeName,
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

                PageHelper<IndividualsDeprivations> pageHelper = InitiativesDal.IndividualsDeprivationsByFilters(initiativeCode,
                    poverty,
                    gender,
                    familyRelationship,
                    startAge,
                    endAge,
                    startSeniority,
                    endSeniority,
                    startPovertyScore,
                    endPovertyScore,
                    employeeCode,
                    employeeName,
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
        /// Save Initiatives.
        /// </summary>
        /// <param name="entity">Initiative Entity</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public DbaEntity InitiativeBeneficiariesSave(
            int? initiativeCode,
            int? poverty,
            string gender,
            string familyRelationship,
            int? startAge,
            int? endAge,
            int? startSeniority,
            int? endSeniority,
            decimal? startPovertyScore,
            decimal? endPovertyScore,
            string employeeCode,
            string employeeName,
            string lastModifiedUser
            )
        {
            try
            {
                return InitiativesDal.InitiativeBeneficiariesSave(initiativeCode,
                    poverty,
                    gender,
                    familyRelationship,
                    startAge,
                    endAge,
                    startSeniority,
                    endSeniority,
                    startPovertyScore,
                    endPovertyScore, 
                    employeeCode, 
                    employeeName,
                    lastModifiedUser);
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
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="employeeCode">Employee code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public PageHelper<IndividualsDeprivations> IndividualsDeprivationsByEmployee(
            string employeeCode,
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

                PageHelper<IndividualsDeprivations> pageHelper = InitiativesDal.IndividualsDeprivationsByEmployee(employeeCode,
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
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="employeeCode">Employee code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public PageHelper<IndividualsDeprivations> HouseholdDeprivationsByEmployee(
            string employeeCode,
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

                PageHelper<IndividualsDeprivations> pageHelper = InitiativesDal.HouseholdDeprivationsByEmployee(employeeCode,
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
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Coordinator code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public PageHelper<IndividualsDeprivations> DeprivationsByFilters(
            string employeeCode,
            int? divisionCode,
            int? companyCode,
            string costFarmId,
            int? indicatorCode,
            int? coordinatorCode,
            int? initiativeCode,
            int? poverty,
            string gender,
            string familyRelationship,
            int? startAge,
            int? endAge,
            int? startSeniority,
            int? endSeniority,
            decimal? startPovertyScore,
            decimal? endPovertyScore,
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

                PageHelper<IndividualsDeprivations> pageHelper = InitiativesDal.DeprivationsByFilters(
                    employeeCode,
                    divisionCode,
                    companyCode,
                    costFarmId,
                    indicatorCode,
                    coordinatorCode,
                    initiativeCode,
                    poverty,
                    gender,
                    familyRelationship,
                    startAge,
                    endAge,
                    startSeniority,
                    endSeniority,
                    startPovertyScore,
                    endPovertyScore,
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
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Coordinator code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        public PageHelper<IndividualsDeprivations> ClosedDeprivationsByFilters(
            string employeeCode,
            int? divisionCode,
            int? companyCode,
            string costFarmId,
            int? indicatorCode,
            int? coordinatorCode,
            int? initiativeCode,
            int? poverty,
            string gender,
            string familyRelationship,
            int? startAge,
            int? endAge,
            int? startSeniority,
            int? endSeniority,
            decimal? startPovertyScore,
            decimal? endPovertyScore,
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

                PageHelper<IndividualsDeprivations> pageHelper = InitiativesDal.ClosedDeprivationsByFilters(
                    employeeCode,
                    divisionCode,
                    companyCode,
                    costFarmId,
                    indicatorCode,
                    coordinatorCode,
                    initiativeCode,
                    poverty,
                    gender,
                    familyRelationship,
                    startAge,
                    endAge,
                    startSeniority,
                    endSeniority,
                    startPovertyScore,
                    endPovertyScore,
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
    }
}
