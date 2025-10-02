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
    public class PoliticalDivisionsBll : IPoliticalDivisionsBll<PoliticalDivisionEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IPoliticalDivisionsDal<PoliticalDivisionEntity> PoliticalDivisionsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public PoliticalDivisionsBll(IPoliticalDivisionsDal<PoliticalDivisionEntity> objDal)
        {
            PoliticalDivisionsDal = objDal;
        }

        /// <summary>
        /// List the Political division enabled by country and by level(Parent political division ID)
        /// </summary>
        /// <param name="countryID">The country ID</param>
        /// <param name="parentPoliticalDivisionID">The parent political division ID</param>
        /// <returns>The political division</returns>
        public List<PoliticalDivisionEntity> ListEnabledByCountryByParentPoliticalDivision(string countryID, int? parentPoliticalDivisionID)
        {
            try
            {
                return PoliticalDivisionsDal.ListEnabledByCountryByParentPoliticalDivision(new PoliticalDivisionEntity(countryID, parentPoliticalDivisionID));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionPoliticalDivisionsList, ex);
                }
            }
        }

        /// <summary>
        /// List the Nationalities
        /// </summary>
        /// <returns>The Nationalities</returns>
        public PageHelper<NationalityEntity> ListNationalities (NationalityEntity nationalityEntity, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                var pageHelper = PoliticalDivisionsDal.ListNationalities(nationalityEntity
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
                    throw new BusinessException(msjExceptionPoliticalDivisionsList, ex);
                }
            }
        }

        /// <summary>
        /// List the Political division  by country 
        /// </summary>
        /// <param name="parentPoliticalDivisionID">The parent political division ID</param>
        /// <returns>The political division</returns>
        public List<PoliticalDivisionEntity> ListByCountryByParentPoliticalDivision(PoliticalDivisionEntity entity)
        {
            try
            {
                return PoliticalDivisionsDal.ListByCountryByParentPoliticalDivision(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionPoliticalDivisionsList, ex);
                }
            }
        }

        /// <summary>
        /// Add a Political division  by country 
        /// </summary>
        /// <param name="entity">The PoliticalDivisionEntity</param>
        /// <returns>The political division entity</returns>
        public PoliticalDivisionEntity Add(PoliticalDivisionEntity entity)
        {
            try
            {
                return PoliticalDivisionsDal.Add(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionPoliticalDivisionsList, ex);
                }
            }
        }

        /// <summary>
        /// Edit a Political division  by country que dicho
        /// </summary>
        /// <param name="entity">The PoliticalDivisionEntity</param>
        /// <returns>The political division entity</returns>
        public PoliticalDivisionEntity Edit(PoliticalDivisionEntity entity)
        {
            try
            {
                return PoliticalDivisionsDal.Edit(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionPoliticalDivisionsList, ex);
                }
            }
        }


        /// <summary>
        /// List a Political division  by politicalDivisionID 
        /// </summary>
        /// <param name="entity">The PoliticalDivisionEntity</param>
        /// <returns>The political division entity</returns>
        public PoliticalDivisionEntity ListByPoliticalDivision(int politicalDivisionID)
        {
            try
            {
                return PoliticalDivisionsDal.ListByPoliticalDivision(politicalDivisionID);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionPoliticalDivisionsList, ex);
                }
            }
        }
    }
}