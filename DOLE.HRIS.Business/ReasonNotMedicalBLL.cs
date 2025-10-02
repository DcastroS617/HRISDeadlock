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
    public class ReasonNotMedicalBll : IReasonNotMedicalBLL<ReasonNotMedicalEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IReasonNotMedicalDal<ReasonNotMedicalEntity> reasonNotMedicalDal;

        public ReasonNotMedicalBll(IReasonNotMedicalDal<ReasonNotMedicalEntity> objDal)
        {
            this.reasonNotMedicalDal = objDal;
        }
        /// <summary>
        /// List the Reason Not Medical enabled
        /// </summary>
        /// <returns>The Reason Not Medical</returns>
        public List<ReasonNotMedicalEntity> ListEnabled()
        {
            try
            {
                return reasonNotMedicalDal.ListEnabled();
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
        /// Add the ReasonNotMedical
        /// </summary>
        /// <param name="entity">The ReasonNotMedical</param>
        /// <returns>Tuple: En the first item a bool: true if ReasonNotMedical successfully added. False otherwise
        /// Second item: the ReasonNotMedical added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, ReasonNotMedicalEntity> Add(ReasonNotMedicalEntity entity)
        {
            try
            {
                ReasonNotMedicalEntity previousEntity = reasonNotMedicalDal.ListByNames(entity.ReasonNotMedicalDescriptionSpanish, entity.ReasonNotMedicalDescriptionEnglish);

                if (previousEntity == null)
                {
                    short principalReasonNotMedicalCode = reasonNotMedicalDal.Add(entity);
                    entity.ReasonNotMedicalCode = (byte)principalReasonNotMedicalCode;

                    return new Tuple<bool, ReasonNotMedicalEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, ReasonNotMedicalEntity>(false, previousEntity);
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
        /// Edit the ReasonNotMedical
        /// </summary>
        /// <param name="entity">The ReasonNotMedical</param>       
        public Tuple<bool, ReasonNotMedicalEntity> Edit(ReasonNotMedicalEntity entity)
        {
            try
            {
                ReasonNotMedicalEntity previousEntity = reasonNotMedicalDal.ListByNames(entity.ReasonNotMedicalDescriptionSpanish, entity.ReasonNotMedicalDescriptionEnglish);

                if (previousEntity == null || previousEntity?.ReasonNotMedicalCode == entity.ReasonNotMedicalCode || previousEntity?.Deleted == false)
                {
                    reasonNotMedicalDal.Edit(entity);

                    return new Tuple<bool, ReasonNotMedicalEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, ReasonNotMedicalEntity>(false, previousEntity);
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
        /// Delete the ReasonNotMedical
        /// </summary>
        /// <param name="entity">The ReasonNotMedical</param>
        public void Delete(ReasonNotMedicalEntity entity)
        {
            try
            {
                reasonNotMedicalDal.Delete(entity);
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
        /// Activate the ReasonNotMedical
        /// </summary>
        /// <param name="entity">The ReasonNotMedical</param>
        public void Activate(ReasonNotMedicalEntity entity)
        {
            try
            {
                reasonNotMedicalDal.Activate(entity);
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
        /// List the ReasonNotMedical By key
        /// </summary>
        /// <param name="ReasonNotMedicalCode">The ReasonNotMedical code</param>
        /// <returns>The Household Contribution Range </returns>
        public ReasonNotMedicalEntity ListByKey(short ReasonNotMedicalCode)
        {
            try
            {
                return reasonNotMedicalDal.ListByKey(ReasonNotMedicalCode);
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
        /// List the ReasonNotMedical by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="ReasonNotMedicalDescriptionSpanish">The ReasonNotMedical name spanish</param>
        /// <param name="ReasonNotMedicalDescriptionEnglish">The ReasonNotMedical name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The ReasonNotMedical meeting the given filters and page config</returns>
        public PageHelper<ReasonNotMedicalEntity> ListByFilters(int divisionCode, string ReasonNotMedicalDescriptionSpanish, string ReasonNotMedicalDescriptionEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<ReasonNotMedicalEntity> pageHelper = reasonNotMedicalDal.ListByFilters(divisionCode
                    , ReasonNotMedicalDescriptionSpanish
                    , ReasonNotMedicalDescriptionEnglish
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
        /// List the ReasonNotMedical By the spanish o english name
        /// </summary>
        /// <param name="ReasonNotMedicalDescriptionSpanish">The ReasonNotMedical name spanish</param>
        /// <param name="ReasonNotMedicalDescriptionEnglish">The ReasonNotMedical name english</param>
        /// <returns>The ReasonNotMedical </returns>
        public ReasonNotMedicalEntity ListByNames(string ReasonNotMedicalDescriptionSpanish, string ReasonNotMedicalDescriptionEnglish)
        {
            try
            {
                return reasonNotMedicalDal.ListByNames(ReasonNotMedicalDescriptionSpanish, ReasonNotMedicalDescriptionEnglish);
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
