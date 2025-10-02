using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class MapCatalogPositionsBll : IMapCatalogPositionsBll
    {
        private readonly IMapCatalogPositionsDal MapCatalogPositionsDal;

        public MapCatalogPositionsBll(IMapCatalogPositionsDal obj)
        {
            MapCatalogPositionsDal = obj;
        }

        public PageHelper<MapCatalogPositionsEntity> MapCatalogPositionsListByFilter(MapCatalogPositionsEntity entity, int Divisioncode, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                var pageHelper = MapCatalogPositionsDal.MapCatalogPositionsListByFilter(entity
                    , Divisioncode 
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

        public Tuple<MapCatalogPositionsEntity, List<MapCatalogPositionsByCompaniesEntity>, List<MapCatalogPositionsByNominalClassEntity>, MapCatalogPositionsByPaymentRatesEntity> MapCatalogPositionsById(MapCatalogPositionsEntity entity)
        {
            try
            {
                return MapCatalogPositionsDal.MapCatalogPositionsById(entity);
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

        public DbaEntity MapCatalogPositionsAdd(MapCatalogPositionsEntity entity, DataTable Companies, DataTable NominalClass)
        {
            try
            {
                return MapCatalogPositionsDal.MapCatalogPositionsAdd(entity, Companies, NominalClass);
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

        public DbaEntity MapCatalogPositionsEdit(MapCatalogPositionsEntity entity, DataTable Companies, DataTable NominalClass)
        {
            try
            {
                return MapCatalogPositionsDal.MapCatalogPositionsEdit(entity, Companies, NominalClass);
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

        public DbaEntity MapCatalogPositionsDesactiveteOrActive(MapCatalogPositionsEntity entity)
        {
            try
            {
                return MapCatalogPositionsDal.MapCatalogPositionsDesactiveteOrActive(entity);
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

        public ListItem[] CompaniesListEnable(string Geografia)
        {
            try
            {
                return MapCatalogPositionsDal.CompaniesListEnable(Geografia);
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

        public ListItem[] NominalClassListEnabled(string Geografia)
        {
            try
            {
                return MapCatalogPositionsDal.NominalClassListEnabled(Geografia);
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

        public ListItem[] CategoryFTEListEnabled()
        {
            try
            {
                return MapCatalogPositionsDal.CategoryFTEListEnabled();
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

        public ListItem[] PaymentRatesListByGeographicDivision(string Geografia)
        {
            try
            {
                return MapCatalogPositionsDal.PaymentRatesListByGeographicDivision(Geografia);
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
