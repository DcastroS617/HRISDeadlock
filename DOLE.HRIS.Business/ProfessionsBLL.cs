using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class ProfessionsBll : IProfessionsBll<ProfessionEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IProfessionsDal<ProfessionEntity> ProfessionsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public ProfessionsBll(IProfessionsDal<ProfessionEntity> objDal)
        {
            ProfessionsDal = objDal;
        }

        /// <summary>
        /// List the Professions enabled
        /// </summary>
        /// <returns>The Professions</returns>
        public List<ProfessionEntity> ListEnabled()
        {
            try
            {
                return  ProfessionsDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgProfessionsList, ex);
                }
            }
        }

        /// <summary>
        /// Add the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        /// <returns>Tuple: En the first item a bool: true if Profession successfully added. False otherwise
        /// Second item: the Profession added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, ProfessionEntity> Add(ProfessionEntity entity)
        {
            try
            {
                ProfessionEntity previousEntity = ProfessionsDal.ListByNames(entity.ProfessionNameSpanish, entity.ProfessionNameEnglish);

                if (previousEntity == null)
                {
                    short professionCode = ProfessionsDal.Add(entity);
                    entity.ProfessionCode = professionCode;

                    return new Tuple<bool, ProfessionEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, ProfessionEntity>(false, previousEntity);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgProfessionsAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>       
        public Tuple<bool, ProfessionEntity> Edit(ProfessionEntity entity)
        {
            try
            {
                ProfessionEntity previousEntity = ProfessionsDal.ListByNames(entity.ProfessionNameSpanish, entity.ProfessionNameEnglish);

                if (previousEntity == null || previousEntity?.ProfessionCode== entity.ProfessionCode || previousEntity?.Deleted==false)
                {
                    ProfessionsDal.Edit(entity);

                    return new Tuple<bool, ProfessionEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, ProfessionEntity>(false, previousEntity);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgProfessionsEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        public void Delete(ProfessionEntity entity)
        {
            try
            {
                ProfessionsDal.Delete(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgProfessionsDelete, ex);
                }
            }
        }

        /// <summary>
        /// Activate the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        public void Activate(ProfessionEntity entity)
        {
            try
            {
                ProfessionsDal.Activate(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgProfessionsActivate, ex);
                }
            }
        }

        /// <summary>
        /// List the Profession By key
        /// </summary>
        /// <param name="professionCode">The profession code</param>
        /// <returns>The Household Contribution Range </returns>
        public ProfessionEntity ListByKey(short professionCode)
        {
            try
            {
                return ProfessionsDal.ListByKey(professionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgProfessionsListByKey, ex);
                }
            }
        }

        /// <summary>
        /// List the Profession by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="professionNameSpanish">The profession name spanish</param>
        /// <param name="professionNameEnglish">The profession name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The Profession meeting the given filters and page config</returns>
        public PageHelper<ProfessionEntity> ListByFilters(int divisionCode, string professionNameSpanish, string professionNameEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<ProfessionEntity> pageHelper = ProfessionsDal.ListByFilters(divisionCode
                    , professionNameSpanish
                    , professionNameEnglish
                    , sortExpression
                    , sortDirection
                    , pageNumber
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
                    throw new BusinessException(msgProfessionsList, ex);
                }
            }
        }

        /// <summary>
        /// List the Profession By the spanish o english name
        /// </summary>
        /// <param name="professionNameSpanish">The profession name spanish</param>
        /// <param name="professionNameEnglish">The profession name english</param>
        /// <returns>The Profession </returns>
        public ProfessionEntity ListByNames(string professionNameSpanish, string professionNameEnglish)
        {
            try
            {
                return ProfessionsDal.ListByNames(professionNameSpanish, professionNameEnglish);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgProfessionsListByKey, ex);
                }
            }
        }
    }
}