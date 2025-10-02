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
    public class ReasonNotAttentionBll : IReasonNotAttentionBLL<ReasonNotAttentionEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IReasonNotAttentionDal<ReasonNotAttentionEntity> reasonNotAttentionDal;

        public ReasonNotAttentionBll(IReasonNotAttentionDal<ReasonNotAttentionEntity> objDal)
        {
            this.reasonNotAttentionDal = objDal;
        }
        /// <summary>
        /// List the Reason Not Attention enabled
        /// </summary>
        /// <returns>The Reason Not Attention</returns>
        public List<ReasonNotAttentionEntity> ListEnabled()
        {
            try
            {
                return reasonNotAttentionDal.ListEnabled();
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
        /// Add the ReasonNotAttention
        /// </summary>
        /// <param name="entity">The ReasonNotAttention</param>
        /// <returns>Tuple: En the first item a bool: true if ReasonNotAttention successfully added. False otherwise
        /// Second item: the ReasonNotAttention added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, ReasonNotAttentionEntity> Add(ReasonNotAttentionEntity entity)
        {
            try
            {
                ReasonNotAttentionEntity previousEntity = reasonNotAttentionDal.ListByNames(entity.ReasonNotAttentionDescriptionSpanish, entity.ReasonNotAttentionDescriptionEnglish);

                if (previousEntity == null)
                {
                    short principalReasonNotAttentionCode = reasonNotAttentionDal.Add(entity);
                    entity.ReasonNotAttentionCode = (byte)principalReasonNotAttentionCode;

                    return new Tuple<bool, ReasonNotAttentionEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, ReasonNotAttentionEntity>(false, previousEntity);
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
        /// Edit the ReasonNotAttention
        /// </summary>
        /// <param name="entity">The ReasonNotAttention</param>       
        public Tuple<bool, ReasonNotAttentionEntity> Edit(ReasonNotAttentionEntity entity)
        {
            try
            {
                ReasonNotAttentionEntity previousEntity = reasonNotAttentionDal.ListByNames(entity.ReasonNotAttentionDescriptionSpanish, entity.ReasonNotAttentionDescriptionEnglish);

                if (previousEntity == null || previousEntity?.ReasonNotAttentionCode == entity.ReasonNotAttentionCode || previousEntity?.Deleted == false)
                {
                    reasonNotAttentionDal.Edit(entity);

                    return new Tuple<bool, ReasonNotAttentionEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, ReasonNotAttentionEntity>(false, previousEntity);
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
        /// Delete the ReasonNotAttention
        /// </summary>
        /// <param name="entity">The ReasonNotAttention</param>
        public void Delete(ReasonNotAttentionEntity entity)
        {
            try
            {
                reasonNotAttentionDal.Delete(entity);
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
        /// Activate the ReasonNotAttention
        /// </summary>
        /// <param name="entity">The ReasonNotAttention</param>
        public void Activate(ReasonNotAttentionEntity entity)
        {
            try
            {
                reasonNotAttentionDal.Activate(entity);
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
        /// List the ReasonNotAttention By key
        /// </summary>
        /// <param name="ReasonNotAttentionCode">The ReasonNotAttention code</param>
        /// <returns>The Household Contribution Range </returns>
        public ReasonNotAttentionEntity ListByKey(short ReasonNotAttentionCode)
        {
            try
            {
                return reasonNotAttentionDal.ListByKey(ReasonNotAttentionCode);
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
        /// List the ReasonNotAttention by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="ReasonNotAttentionDescriptionSpanish">The ReasonNotAttention name spanish</param>
        /// <param name="ReasonNotAttentionDescriptionEnglish">The ReasonNotAttention name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The ReasonNotAttention meeting the given filters and page config</returns>
        public PageHelper<ReasonNotAttentionEntity> ListByFilters(int divisionCode, string ReasonNotAttentionDescriptionSpanish, string ReasonNotAttentionDescriptionEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<ReasonNotAttentionEntity> pageHelper = reasonNotAttentionDal.ListByFilters(divisionCode
                    , ReasonNotAttentionDescriptionSpanish
                    , ReasonNotAttentionDescriptionEnglish
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
        /// List the ReasonNotAttention By the spanish o english name
        /// </summary>
        /// <param name="ReasonNotAttentionDescriptionSpanish">The ReasonNotAttention name spanish</param>
        /// <param name="ReasonNotAttentionDescriptionEnglish">The ReasonNotAttention name english</param>
        /// <returns>The ReasonNotAttention </returns>
        public ReasonNotAttentionEntity ListByNames(string ReasonNotAttentionDescriptionSpanish, string ReasonNotAttentionDescriptionEnglish)
        {
            try
            {
                return reasonNotAttentionDal.ListByNames(ReasonNotAttentionDescriptionSpanish, ReasonNotAttentionDescriptionEnglish);
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
