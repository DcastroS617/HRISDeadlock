using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IInitiativeBeneficiariesDAL<T> where T : InitiativeBeneficiaries
    {
        /// <summary>
        /// List Initiatives by Filters
        /// </summary>
        /// <param name="coordinatorCode">Coordinator Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        PageHelper<InitiativeBeneficiaries> ListByFilters(
            int? coordinatorCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue);

        /// <summary>
        /// List Initiatives by Filters
        /// </summary>
        /// <param name="initiativeCode">Coordinator Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        PageHelper<IndividualsDeprivations> IndividualsDeprivationsByFilters(
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
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue);

        /// <summary>
        /// Save the Initiative
        /// </summary>
        /// <param name="entity">Initiative</param>
        DbaEntity InitiativeBeneficiariesSave(
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
        );

        /// <summary>
        /// List Initiatives by employee
        /// </summary>
        /// <param name="employeeCode">Employee Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        PageHelper<IndividualsDeprivations> IndividualsDeprivationsByEmployee(
            string employeeCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue);

        /// <summary>
        /// List Initiatives by employee
        /// </summary>
        /// <param name="employeeCode">Employee Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        PageHelper<IndividualsDeprivations> HouseholdDeprivationsByEmployee(
            string employeeCode,
            string sortExpression,
            string sortDirection,
            int pageNumber,
            int? pageSize,
            int pageSizeValue);

        PageHelper<IndividualsDeprivations> DeprivationsByFilters(
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
            int pageNumber,
            int? pageSize,
            int pageSizeValue);

        /// <summary>
        /// List Initiatives by Filters
        /// </summary>
        /// <param name="initiativeCode">Coordinator Code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageSizeValue">Page Size Value</param>
        /// <returns>List of Initiative Entities</returns>
        PageHelper<IndividualsDeprivations> ClosedDeprivationsByFilters(
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
            int pageNumber,
            int? pageSize,
            int pageSizeValue);

    }   
}
