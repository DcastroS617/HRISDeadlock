using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    public class CycleTrainingBll : ICycleTrainingBll
    {
        private readonly ICycleTrainingDal CycleTrainingDal;

        public CycleTrainingBll(ICycleTrainingDal objDal)
        {
            CycleTrainingDal = objDal;
        }

        /// <summary>
        /// List the cycle training by the given filters
        /// </summary>
        /// <param name="CycleTraining">The cycle training</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The cycle training meeting the given filters and page config</returns>
        public PageHelper<CycleTrainingEntity> CycleTrainingListByFilter(CycleTrainingEntity CycleTraining, int DivisionCode, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                var pageHelper = CycleTrainingDal.CycleTrainingListByFilter(CycleTraining
                    , DivisionCode
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
        /// List the cycle training by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="entity">The cycle training</param>
        public CycleTrainingEntity CycleTrainingByKey(CycleTrainingEntity CycleTraining)
        {
            try
            {
                return CycleTrainingDal.CycleTrainingByKey(CycleTraining);
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
        /// Add the cycle training
        /// </summary>
        /// <param name="entity">The cycle training</param>
        public CycleTrainingEntity CycleTrainingAdd(CycleTrainingEntity CycleTraining)
        {
            try
            {
                return CycleTrainingDal.CycleTrainingAdd(CycleTraining);
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
        /// Edit the cycle training
        /// </summary>
        /// <param name="entity">The cycle training</param>
        public CycleTrainingEntity CycleTrainingEdit(CycleTrainingEntity CycleTraining)
        {
            try
            {
                return CycleTrainingDal.CycleTrainingEdit(CycleTraining);
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
        /// List the cycle training by catalog
        /// </summary>
        /// <returns></returns>
        public ListItem[] CycleTrainingListByCatalog(CycleTrainingEntity CycleTraining)
        {
            try
            {
                return CycleTrainingDal.CycleTrainingListByCatalog(CycleTraining);
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
        /// List the cycle tranning by division key: Division an d GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CycleTrainingEntity> CycleTrainingListByByMasterProgramByCourse(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return CycleTrainingDal.CycleTrainingListByByMasterProgramByCourse(divisionCode, geographicDivisionCode);
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
