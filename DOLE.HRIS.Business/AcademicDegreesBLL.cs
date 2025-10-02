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
    public class AcademicDegreesBll : IAcademicDegreesBll<AcademicDegreeEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IAcademicDegreesDal<AcademicDegreeEntity> AcademicDegreesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public AcademicDegreesBll(IAcademicDegreesDal<AcademicDegreeEntity> objDal)
        {
            AcademicDegreesDal = objDal;
        }

        /// <summary>
        /// List the Academic degrees enabled
        /// </summary>
        /// <returns>The Academic degrees</returns>
        public List<AcademicDegreeEntity> ListEnabled()
        {
            try
            {
                return AcademicDegreesDal.ListEnabled();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionAcademicDegreesList, ex);
                }
            }
        }

        /// <summary>
        /// Add the academicDegree
        /// </summary>
        /// <param name="entity">The academicDegree</param>
        /// <returns>Tuple: En the first item a bool: true if academicDegree successfully added. False otherwise
        /// Second item: the academicDegree added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, AcademicDegreeEntity> Add(AcademicDegreeEntity entity)
        {
            try
            {
                AcademicDegreeEntity previousEntity = AcademicDegreesDal.ListByNames(entity.AcademicDegreeDescriptionSpanish, entity.AcademicDegreeDescriptionEnglish);

                if (previousEntity == null)
                {
                    byte academicDegreeCode = AcademicDegreesDal.Add(entity);
                    entity.AcademicDegreeCode = academicDegreeCode;

                    return new Tuple<bool, AcademicDegreeEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, AcademicDegreeEntity>(false, previousEntity);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgAcademicDegreeAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit the academicDegree
        /// </summary>
        /// <param name="entity">The academicDegree</param>       
        public Tuple<bool, AcademicDegreeEntity> Edit(AcademicDegreeEntity entity)
        {
            try
            {
                AcademicDegreeEntity previousEntity = AcademicDegreesDal.ListByNames(entity.AcademicDegreeDescriptionSpanish, entity.AcademicDegreeDescriptionEnglish);

                if (previousEntity == null || previousEntity?.AcademicDegreeCode == entity.AcademicDegreeCode || previousEntity?.Deleted == false )
                {
                    AcademicDegreesDal.Edit(entity);

                    return new Tuple<bool, AcademicDegreeEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, AcademicDegreeEntity>(false, previousEntity);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgAcademicDegreeEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete the academicDegree
        /// </summary>
        /// <param name="entity">The academicDegree</param>
        public void Delete(AcademicDegreeEntity entity)
        {
            try
            {
                AcademicDegreesDal.Delete(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgAcademicDegreeDelete, ex);
                }
            }
        }

        /// <summary>
        /// Activate the academicDegree
        /// </summary>
        /// <param name="entity">The academicDegree</param>
        public void Activate(AcademicDegreeEntity entity)
        {
            try
            {
                AcademicDegreesDal.Activate(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgAcademicDegreeActivate, ex);
                }
            }
        }

        /// <summary>
        /// List the academicDegree By key
        /// </summary>
        /// <param name="academicDegreeCode">The academicDegree code</param>
        /// <returns>The Household Contribution Range </returns>
        public AcademicDegreeEntity ListByKey(short academicDegreeCode)
        {
            try
            {
                return AcademicDegreesDal.ListByKey(academicDegreeCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgAcademicDegreeListByKey, ex);
                }
            }
        }

        /// <summary>
        /// List the academicDegree by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="AcademicDegreeDescriptionSpanish">The academicDegree name spanish</param>
        /// <param name="AcademicDegreeDescriptionEnglish">The academicDegree name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The academicDegree meeting the given filters and page config</returns>
        public PageHelper<AcademicDegreeEntity> ListByFilters(int divisionCode, string AcademicDegreeDescriptionSpanish, string AcademicDegreeDescriptionEnglish, string sortExpression, string sortDirection, int pageNumber)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<AcademicDegreeEntity> pageHelper = AcademicDegreesDal.ListByFilters(divisionCode
                    , AcademicDegreeDescriptionSpanish
                    , AcademicDegreeDescriptionEnglish
                    , sortExpression
                    , sortDirection
                    , pageNumber
                    , pageSizeValue);

                pageHelper.TotalPages = (pageHelper.TotalResults - 1) / pageHelper.PageSize + 1;

                return pageHelper;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgAcademicDegreeList, ex);
                }
            }
        }

        /// <summary>
        /// List the academicDegree By the spanish o english name
        /// </summary>
        /// <param name="AcademicDegreeDescriptionSpanish">The academicDegree name spanish</param>
        /// <param name="AcademicDegreeDescriptionEnglish">The academicDegree name english</param>
        /// <returns>The academicDegree </returns>
        public AcademicDegreeEntity ListByNames(string AcademicDegreeDescriptionSpanish, string AcademicDegreeDescriptionEnglish)
        {
            try
            {
                return AcademicDegreesDal.ListByNames(AcademicDegreeDescriptionSpanish, AcademicDegreeDescriptionEnglish);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msgAcademicDegreeListByKey, ex);
                }
            }
        }
    }
}