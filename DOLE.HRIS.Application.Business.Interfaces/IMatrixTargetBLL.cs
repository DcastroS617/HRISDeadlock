using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IMatrixTargetBll
    {
        /// <summary>
        /// Retrieves a paginated list of Matrix Targets based on filter criteria.
        /// </summary>
        PageHelper<MatrixTargetEntity> MatrixTargetByFilter(MatrixTargetEntity entity, string lang, int divisionCode, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// Retrieves detailed Matrix Target data by its identifier.
        /// </summary>
        Tuple<MatrixTargetEntity, List<MatrixTargetByDivisionsEntity>, List<MatrixTargetByCompaniesEntity>, List<MatrixTargetByCostZonesEntity>, List<MatrixTargetByCostMiniZonesEntity>, List<MatrixTargetByCostFarmsEntity>, List<MatrixTargetByNominalClassEntity>> MatrixTargetById(MatrixTargetEntity entity);

        /// <summary>
        /// Adds a new Matrix Target record along with associated cost structure data.
        /// </summary>
        DbaEntity MatrixTargetAdd(MatrixTargetEntity entity, DataTable divisions, DataTable costZones, DataTable costMiniZones, DataTable costFarms, DataTable companies, DataTable nominalClass);

        /// <summary>
        /// Updates an existing Matrix Target record and its associated cost structure data.
        /// </summary>
        DbaEntity MatrixTargetEdit(MatrixTargetEntity entity, DataTable divisions, DataTable costZones, DataTable costMiniZones, DataTable costFarms, DataTable companies, DataTable nominalClass);

        /// <summary>
        /// Deactivates a Matrix Target record.
        /// </summary>
        DbaEntity MatrixTargetDeactivate(MatrixTargetEntity entity);

        /// <summary>
        /// Checks if the user has permission to access Matrix Target data at the regional level.
        /// </summary>
        DbaEntity MatrixTargetRegionalPermit(MatrixTargetEntity entity, int UserCode);

        /// <summary>
        /// Retrieves a list of enabled cost zones for the specified divisions.
        /// </summary>
        List<MatrixTargetByCostZonesEntity> CostZonesListEnableByDivisions(DataTable divisions);

        /// <summary>
        /// Retrieves a list of enabled cost mini-zones for the specified division.
        /// </summary>
        List<MatrixTargetByCostMiniZonesEntity> CostMiniZonesListEnableByDivision(string geographicDivisionCode, int divisionCode);

        /// <summary>
        /// Retrieves a list of enabled cost mini-zones for multiple divisions.
        /// </summary>
        List<MatrixTargetByCostMiniZonesEntity> CostMiniZonesListEnableByDivisions(string geographicDivisionCode, DataTable divisions);

        /// <summary>
        /// Retrieves a list of enabled cost farms for the specified division.
        /// </summary>
        List<MatrixTargetByCostFarmsEntity> CostFarmsListEnableByDivision(string geographicDivisionCode);

        /// <summary>
        /// Retrieves a list of enabled cost farms for multiple divisions.
        /// </summary>
        List<MatrixTargetByCostFarmsEntity> CostFarmsListEnableByDivisions(string geographicDivisionCode, DataTable divisions);

        /// <summary>
        /// Retrieves a list of enabled companies by division.
        /// </summary>
        List<MatrixTargetByCompaniesEntity> CompaniesListEnableByDivision(DataTable divisions);

        /// <summary>
        /// Retrieves a list of enabled nominal classes for the specified company.
        /// </summary>
        List<MatrixTargetByNominalClassEntity> NominalClassListEnabledByCompanie(string geographicDivisionCode);

        /// <summary>
        /// Retrieves a list of enabled nominal classes for multiple companies.
        /// </summary>
        List<MatrixTargetByNominalClassEntity> NominalClassListEnabledByCompanies(DataTable divisions);

        /// <summary>
        /// Retrieves a list of cost centers based on the specified geographic structure.
        /// </summary>
        List<MatrixTargetByCostCentresEntity> CostCentersListByStruct(string geographicDivisionCode);
    }
}