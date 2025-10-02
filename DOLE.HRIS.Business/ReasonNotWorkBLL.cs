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
    public class ReasonNotWorkBll : IReasonNotWorkBLL<ReasonNotWorkEntity>
    {

        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IReasonNotWorkDal<ReasonNotWorkEntity> reasonNotWorkDal;

        public ReasonNotWorkBll(IReasonNotWorkDal<ReasonNotWorkEntity> objDal)
        {
            this.reasonNotWorkDal = objDal;
        }

        /// <summary>
        /// List the Reason Nots Work enabled
        /// </summary>
        /// <returns>The Reason Not Works</returns>
        public List<ReasonNotWorkEntity> ListEnabled()
        {
            try
            {
                return reasonNotWorkDal.ListEnabled();
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
        /// Add the ReasonNotWork
        /// </summary>
        /// <param name="entity">The ReasonNotWork</param>
        /// <returns>Tuple: En the first item a bool: true if ReasonNotWork successfully added. False otherwise
        /// Second item: the ReasonNotWork added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, ReasonNotWorkEntity> Add(ReasonNotWorkEntity entity)
        {
            try
            {
                ReasonNotWorkEntity previousEntity = reasonNotWorkDal.ListByNames(entity.ReasonNotWorkDescriptionSpanish, entity.ReasonNotWorkDescriptionEnglish);

                if (previousEntity == null)
                {
                    short principalReasonNotWorkCode = reasonNotWorkDal.Add(entity);
                    entity.ReasonNotWorkCode = (byte)principalReasonNotWorkCode;

                    return new Tuple<bool, ReasonNotWorkEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, ReasonNotWorkEntity>(false, previousEntity);
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
        /// Edit the ReasonNotWork
        /// </summary>
        /// <param name="entity">The ReasonNotWork</param>       
        public Tuple<bool, ReasonNotWorkEntity> Edit(ReasonNotWorkEntity entity)
        {
            try
            {
                ReasonNotWorkEntity previousEntity = reasonNotWorkDal.ListByNames(entity.ReasonNotWorkDescriptionSpanish, entity.ReasonNotWorkDescriptionEnglish);

                if (previousEntity == null || previousEntity?.ReasonNotWorkCode == entity.ReasonNotWorkCode || previousEntity?.Deleted == false)
                {
                    reasonNotWorkDal.Edit(entity);

                    return new Tuple<bool, ReasonNotWorkEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, ReasonNotWorkEntity>(false, previousEntity);
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
        /// Delete the ReasonNotWork
        /// </summary>
        /// <param name="entity">The ReasonNotWork</param>
        public void Delete(ReasonNotWorkEntity entity)
        {
            try
            {
                reasonNotWorkDal.Delete(entity);
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
        /// Activate the ReasonNotWork
        /// </summary>
        /// <param name="entity">The ReasonNotWork</param>
        public void Activate(ReasonNotWorkEntity entity)
        {
            try
            {
                reasonNotWorkDal.Activate(entity);
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
        /// List the ReasonNotWork By key
        /// </summary>
        /// <param name="ReasonNotWorkCode">The ReasonNotWork code</param>
        /// <returns>The Household Contribution Range </returns>
        public ReasonNotWorkEntity ListByKey(short ReasonNotWorkCode)
        {
            try
            {
                return reasonNotWorkDal.ListByKey(ReasonNotWorkCode);
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
        /// List the ReasonNotWork by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="ReasonNotWorkDescriptionSpanish">The ReasonNotWork name spanish</param>
        /// <param name="ReasonNotWorkDescriptionEnglish">The ReasonNotWork name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The ReasonNotWork meeting the given filters and page config</returns>
        public PageHelper<ReasonNotWorkEntity> ListByFilters(int divisionCode, string ReasonNotWorkDescriptionSpanish, string ReasonNotWorkDescriptionEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<ReasonNotWorkEntity> pageHelper = reasonNotWorkDal.ListByFilters(divisionCode
                    , ReasonNotWorkDescriptionSpanish
                    , ReasonNotWorkDescriptionEnglish
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
        /// List the ReasonNotWork By the spanish o english name
        /// </summary>
        /// <param name="ReasonNotWorkDescriptionSpanish">The ReasonNotWork name spanish</param>
        /// <param name="ReasonNotWorkDescriptionEnglish">The ReasonNotWork name english</param>
        /// <returns>The ReasonNotWork </returns>
        public ReasonNotWorkEntity ListByNames(string ReasonNotWorkDescriptionSpanish, string ReasonNotWorkDescriptionEnglish)
        {
            try
            {
                return reasonNotWorkDal.ListByNames(ReasonNotWorkDescriptionSpanish, ReasonNotWorkDescriptionEnglish);
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
