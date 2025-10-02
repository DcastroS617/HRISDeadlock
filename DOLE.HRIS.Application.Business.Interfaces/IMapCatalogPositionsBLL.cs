using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IMapCatalogPositionsBll
    {
        ListItem[] CategoryFTEListEnabled();
        
        ListItem[] CompaniesListEnable(string Geografia);
        
        DbaEntity MapCatalogPositionsAdd(MapCatalogPositionsEntity entity, DataTable Companies, DataTable NominalClass);
       
        Tuple<MapCatalogPositionsEntity, List<MapCatalogPositionsByCompaniesEntity>, List<MapCatalogPositionsByNominalClassEntity>, MapCatalogPositionsByPaymentRatesEntity> MapCatalogPositionsById(MapCatalogPositionsEntity entity);
       
        DbaEntity MapCatalogPositionsDesactiveteOrActive(MapCatalogPositionsEntity entity);
        
        DbaEntity MapCatalogPositionsEdit(MapCatalogPositionsEntity entity, DataTable Companies, DataTable NominalClass);
        
        PageHelper<MapCatalogPositionsEntity> MapCatalogPositionsListByFilter(MapCatalogPositionsEntity entity,int Divisioncode, string sortExpression, string sortDirection, int? pageNumber);
        
        ListItem[] PaymentRatesListByGeographicDivision(string Geografia);
        
        ListItem[] NominalClassListEnabled(string Geografia);
    }
}