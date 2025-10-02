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
    public class PrincipalProfesionBll : IPrincipalProfesionBLL<PrincipalProfesionEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IPrincipalProfesionDal<PrincipalProfesionEntity> principalProfesionDal;

        public PrincipalProfesionBll(IPrincipalProfesionDal<PrincipalProfesionEntity> objDal) {
            this.principalProfesionDal = objDal;
        }
        public List<PrincipalProfesionEntity> ListEnabled()
        {
            try
            {
                return principalProfesionDal.ListEnabled();
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
        /// Add the Profession
        /// </summary>
        /// <param name="entity">The Profession</param>
        /// <returns>Tuple: En the first item a bool: true if Profession successfully added. False otherwise
        /// Second item: the Profession added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, PrincipalProfesionEntity> Add(PrincipalProfesionEntity entity)
        {
            try
            {
                PrincipalProfesionEntity previousEntity = principalProfesionDal.ListByNames(entity.PrincipalProfesionDescriptionSpanish, entity.PrincipalProfesionDescriptionEnglish);

                if (previousEntity == null)
                {
                    short principalPrincipalProfesionCode = principalProfesionDal.Add(entity);
                    entity.PrincipalProfesionCode = (byte) principalPrincipalProfesionCode;

                    return new Tuple<bool, PrincipalProfesionEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, PrincipalProfesionEntity>(false, previousEntity);
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
        public Tuple<bool, PrincipalProfesionEntity> Edit(PrincipalProfesionEntity entity)
        {
            try
            {
                PrincipalProfesionEntity previousEntity = principalProfesionDal.ListByNames(entity.PrincipalProfesionDescriptionSpanish, entity.PrincipalProfesionDescriptionEnglish);

                if (previousEntity == null || previousEntity?.PrincipalProfesionCode == entity.PrincipalProfesionCode || previousEntity?.Deleted == false)
                {
                    principalProfesionDal.Edit(entity);

                    return new Tuple<bool, PrincipalProfesionEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, PrincipalProfesionEntity>(false, previousEntity);
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
        public void Delete(PrincipalProfesionEntity entity)
        {
            try
            {
                principalProfesionDal.Delete(entity);
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
        public void Activate(PrincipalProfesionEntity entity)
        {
            try
            {
                principalProfesionDal.Activate(entity);
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
        /// <param name="PrincipalProfesionCode">The profession code</param>
        /// <returns>The Household Contribution Range </returns>
        public PrincipalProfesionEntity ListByKey(short PrincipalProfesionCode)
        {
            try
            {
                return principalProfesionDal.ListByKey(PrincipalProfesionCode);
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
        /// <param name="PrincipalProfesionDescriptionSpanish">The profession name spanish</param>
        /// <param name="PrincipalProfesionDescriptionEnglish">The profession name english</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The Profession meeting the given filters and page config</returns>
        public PageHelper<PrincipalProfesionEntity> ListByFilters(int divisionCode, string PrincipalProfesionDescriptionSpanish, string PrincipalProfesionDescriptionEnglish, string sortExpression, string sortDirection, int pageNumber, int? pageSize)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<PrincipalProfesionEntity> pageHelper = principalProfesionDal.ListByFilters(divisionCode
                    , PrincipalProfesionDescriptionSpanish
                    , PrincipalProfesionDescriptionEnglish
                    , sortExpression
                    , sortDirection
                    , pageNumber
                    , null
                    , 0);

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
        /// <param name="PrincipalProfesionDescriptionSpanish">The profession name spanish</param>
        /// <param name="PrincipalProfesionDescriptionEnglish">The profession name english</param>
        /// <returns>The Profession </returns>
        public PrincipalProfesionEntity ListByNames(string PrincipalProfesionDescriptionSpanish, string PrincipalProfesionDescriptionEnglish)
        {
            try
            {
                return principalProfesionDal.ListByNames(PrincipalProfesionDescriptionSpanish, PrincipalProfesionDescriptionEnglish);
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
