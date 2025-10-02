using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class GtiPeriodParameterDivisionCurrencyBLL : IGtiPeriodParameterDivisionCurrencyBLL
    {
        /// <summary>
        /// Data access object for interacting with the data layer.
        /// </summary>
        private readonly IGtiPeriodParameterDivisionCurrencyDAL objGtiPeriodParameterDivisionCurrencyDal;

        /// <summary>
        /// Constructor to create an instance of the GtiPeriodParameterDivisionCurrencyBLL class.
        /// </summary>
        /// <param name="gtiPeriodParamaeterDivisionCurrencyDal">Data access object to interact with the database</param>
        public GtiPeriodParameterDivisionCurrencyBLL(IGtiPeriodParameterDivisionCurrencyDAL gtiPeriodParamaeterDivisionCurrencyDal)
        {
            objGtiPeriodParameterDivisionCurrencyDal = gtiPeriodParamaeterDivisionCurrencyDal;
        }

        /// <summary>
        /// Adds or updates a period parameter division currency entity.
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity to add or update</param>
        /// <returns>Returns the added or updated entity</returns>
        public PeriodParameterDivisionCurrencyEntity AddOrUpdate(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity)
        {
            return objGtiPeriodParameterDivisionCurrencyDal.AddOrUpdate(periodParameterDivisionCurrencyEntity);
        }

        /// <summary>
        /// Deletes a period parameter division currency entity.
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity to be deleted</param>
        public void Delete(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a period parameter division currency entity by its ID.
        /// </summary>
        /// <param name="PeriodParameterDivisionCurrencyId">The ID of the period parameter division currency entity</param>
        /// <returns>Returns the entity matching the given ID</returns>
        public PeriodParameterDivisionCurrencyEntity ListByKey(int PeriodParameterDivisionCurrencyId)
        {
            try
            {
                return objGtiPeriodParameterDivisionCurrencyDal.ListByKey(PeriodParameterDivisionCurrencyId);
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
        /// Lists all active currencies.
        /// </summary>
        /// <returns>Returns a list of active currencies</returns>
        public List<CurrencyEntity> ListCurrenciesActive()
        {
            try
            {
                return objGtiPeriodParameterDivisionCurrencyDal.ListCurrenciesActive();
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
        /// Lists all enabled nominal classes.
        /// </summary>
        /// <returns>Returns a list of enabled nominal classes</returns>
        public List<NominalClassEntity> ListNominalClassEnable()
        {
            try
            {
                return objGtiPeriodParameterDivisionCurrencyDal.ListNominalClassEnable();
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
        /// Lists enabled nominal classes filtered by geographic division.
        /// </summary>
        /// <param name="GeographicDivisionCode">The code of the geographic division</param>
        /// <returns>Returns a list of enabled nominal classes by division</returns>
        public List<NominalClassEntity> ListNominalClassEnableByDivision(string GeographicDivisionCode)
        {
            try
            {
                return objGtiPeriodParameterDivisionCurrencyDal.ListNominalClassEnableByDivision(GeographicDivisionCode);
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
        /// Lists all geographic divisions by their division codes.
        /// </summary>
        /// <returns>Returns a list of geographic divisions by division codes</returns>
        public List<DivisionEntity> ListGeographicDivisionsByDivisions()
        {
            try
            {
                return objGtiPeriodParameterDivisionCurrencyDal.ListGeographicDivisionsByDivisions();
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
        /// Lists all name of geographic divisions by their division codes.
        /// </summary>
        /// <returns>Returns a list of geographic divisions by division codes</returns>
        public List<PeriodParameterDivisionCurrencyEntity> ListNameGeographicDivisionsByDivisions()
        {
            try
            {
                return objGtiPeriodParameterDivisionCurrencyDal.ListNameGeographicDivisionsByDivisions();
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
        /// Lists period parameter division currency entities by the given filters with sorting and pagination.
        /// </summary>
        /// <param name="periodParameterDivisionCurrencyEntity">The entity containing filter values</param>
        /// <param name="sortExpression">The field by which to sort</param>
        /// <param name="sortDirection">The direction of sorting (ASC/DESC)</param>
        /// <param name="pageNumber">The page number for pagination</param>
        /// <returns>Returns a PageHelper object containing the filtered results and pagination info</returns>
        public PageHelper<PeriodParameterDivisionCurrencyEntity> ListGtiPeriodParameterDivisionCurrencyByFilters(PeriodParameterDivisionCurrencyEntity periodParameterDivisionCurrencyEntity, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                int pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<PeriodParameterDivisionCurrencyEntity> pageHelper = 
                    objGtiPeriodParameterDivisionCurrencyDal.ListGtiPeriodParameterDivisionCurrencyByFilters(periodParameterDivisionCurrencyEntity
                    , sortExpression
                    , sortDirection
                    , pageNumber.Value
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
    }
}
