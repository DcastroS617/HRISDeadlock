using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Configuration;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class AbsenteeismCauseCategoriesBll : IAbsenteeismCauseCategoriesBll<AbsenteeismCauseCategoryEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IAbsenteeismCauseCategoriesDal<AbsenteeismCauseCategoryEntity> AbsenteeismCauseCategoriesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public AbsenteeismCauseCategoriesBll(IAbsenteeismCauseCategoriesDal<AbsenteeismCauseCategoryEntity> objDal)
        {
            AbsenteeismCauseCategoriesDal = objDal;
        }

        /// <summary>
        /// List the absenteeism Cause categories by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>        
        /// <param name="absenteeismCauseCategoryCode">Code</param>
        /// <param name="absenteeismCauseCategoryName">Name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>        
        /// <returns>The absenteeismCauses meeting the given filters and page config</returns>
        public PageHelper<AbsenteeismCauseCategoryEntity> ListByFilters(int divisionCode, string causeCategoryCode, string causeCategoryName, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<AbsenteeismCauseCategoryEntity> pageHelper = AbsenteeismCauseCategoriesDal.ListByFilters(divisionCode
                    , causeCategoryCode
                    , causeCategoryName
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

        /// <summary>
        /// List the absenteeism Cause Category by key: Code
        /// </summary>        
        /// <param name="causeCategoryCode">Absenteeism Cause Category code</param>
        /// <returns>The absenteeismCause Category</returns>
        public AbsenteeismCauseCategoryEntity ListByKey(string causeCategoryCode)
        {
            try
            {
                return AbsenteeismCauseCategoriesDal.ListByKey(causeCategoryCode);
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
        /// Add the absenteeism Cause Category
        /// </summary>
        /// <param name="entity">The absenteeism Cause Category</param>
        /// <returns>Tuple: En the first item a bool: true if absenteeismCause successfully added. False otherwise
        /// Second item: the absenteeism Cause Category added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, AbsenteeismCauseCategoryEntity> Add(AbsenteeismCauseCategoryEntity entity)
        {
            try
            {
                AbsenteeismCauseCategoryEntity previousAbsenteeismCauseCategory = AbsenteeismCauseCategoriesDal.ListByKeyOrName(entity.CauseCategoryCode, entity.CauseCategoryName);

                if (previousAbsenteeismCauseCategory == null)
                {
                    AbsenteeismCauseCategoriesDal.Add(entity);
                    return new Tuple<bool, AbsenteeismCauseCategoryEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, AbsenteeismCauseCategoryEntity>(false, previousAbsenteeismCauseCategory);
                }
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
        /// Edit the absenteeism Cause Category
        /// </summary>
        /// <param name="entity">The absenteeism Cause Category</param>       
        public void Edit(AbsenteeismCauseCategoryEntity entity)
        {
            try
            {
                AbsenteeismCauseCategoriesDal.Edit(entity);
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
        /// Delete the absenteeism Cause Category
        /// </summary>
        /// <param name="entity">The absenteeism Cause Category</param>
        public void Delete(AbsenteeismCauseCategoryEntity entity)
        {
            try
            {
                AbsenteeismCauseCategoriesDal.Delete(entity);
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
        /// Activate the absenteeism Cause Category
        /// </summary>
        /// <param name="entity">The absenteeism Cause Category</param>
        public void Activate(AbsenteeismCauseCategoryEntity entity)
        {
            try
            {
                AbsenteeismCauseCategoriesDal.Activate(entity);
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