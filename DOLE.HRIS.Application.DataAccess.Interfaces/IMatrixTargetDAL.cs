using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IMatrixTargetDal
    {
        PageHelper<MatrixTargetEntity> MatrixTargetByFilter(MatrixTargetEntity entity, string lang, int divisionCode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        Tuple<MatrixTargetEntity, List<MatrixTargetByDivisionsEntity>, List<MatrixTargetByCompaniesEntity>, List<MatrixTargetByCostZonesEntity>, List<MatrixTargetByCostMiniZonesEntity>, List<MatrixTargetByCostFarmsEntity>, List<MatrixTargetByNominalClassEntity>> MatrixTargetById(MatrixTargetEntity entity);

        DbaEntity MatrixTargetAdd(MatrixTargetEntity entity, DataTable divisions, DataTable costZones, DataTable costMiniZones, DataTable costFarms, DataTable companies, DataTable nominalClass);

        DbaEntity MatrixTargetEdit(MatrixTargetEntity entity, DataTable divisions, DataTable costZones, DataTable costMiniZones, DataTable costFarms, DataTable companies, DataTable nominalClass);

        DbaEntity MatrixTargetDeactivate(MatrixTargetEntity entity);

        DbaEntity MatrixTargetRegionalPermit(MatrixTargetEntity entity, int UserCode);

        List<MatrixTargetByCostZonesEntity> CostZonesListEnableByDivisions(DataTable divisions);

        List<MatrixTargetByCostMiniZonesEntity> CostMiniZonesListEnableByDivision(string geographicDivisionCode, int divisionCode);

        List<MatrixTargetByCostMiniZonesEntity> CostMiniZonesListEnableByDivisions(string geographicDivisionCode, DataTable divisions);

        List<MatrixTargetByCostFarmsEntity> CostFarmsListEnableByDivision(string geographicDivisionCode);

        List<MatrixTargetByCostFarmsEntity> CostFarmsListEnableByDivisions(string geographicDivisionCode, DataTable divisions);

        List<MatrixTargetByCompaniesEntity> CompaniesListEnableByDivision(DataTable divisions);

        List<MatrixTargetByNominalClassEntity> NominalClassListEnabledByCompanie(string geographicDivisionCode);

        List<MatrixTargetByNominalClassEntity> NominalClassListEnabledByCompanies(DataTable divisions);

        List<MatrixTargetByCostCentresEntity> CostCentersListByStruct(string geographicDivisionCode);
    }
}