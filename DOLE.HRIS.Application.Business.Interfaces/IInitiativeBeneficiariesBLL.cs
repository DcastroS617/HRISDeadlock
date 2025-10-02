using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IInitiativeBeneficiariesBLL<T> where T : InitiativeBeneficiaries
    {
        /// <summary>
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Coordinator code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        PageHelper<InitiativeBeneficiaries> ListByFilters(
            int initiativeCode,
            string sortExpression,
            string sortDirection,
            int? pageNumber);

        /// <summary>
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Coordinator code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
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
            int? pageNumber);

        /// <summary>
        /// Save Initiatives.
        /// </summary>
        /// <param name="entity">Initiative Entity</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
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
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="employeeCode">Employee code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        PageHelper<IndividualsDeprivations> IndividualsDeprivationsByEmployee(
            string employeeCode,
            string sortExpression,
            string sortDirection,
            int? pageNumber);

        /// <summary>
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="employeeCode">Employee code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
        PageHelper<IndividualsDeprivations> HouseholdDeprivationsByEmployee(
            string employeeCode,
            string sortExpression,
            string sortDirection,
            int? pageNumber);

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
            int? pageNumber);

        /// <summary>
        /// Lists initiatives based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Coordinator code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiatives.</returns>
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
            int? pageNumber);
    }
}
