using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Configuration;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class TypeTrainingBll : ITypeTrainingBll
    {
        private readonly ITypeTrainingDal TypeTrainingDal;

        public TypeTrainingBll(ITypeTrainingDal objDal)
        {
            TypeTrainingDal = objDal;
        }
       
        /// <summary>
        /// List the type training by the given filters
        /// </summary>
        /// <param name="typeTraining">The type training</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The type training meeting the given filters and page config</returns>
        public PageHelper<TypeTrainingEntity> TypeTrainingListByFilter(TypeTrainingEntity typeTraining, int DivisionCode, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                var pageHelper = TypeTrainingDal.TypeTrainingListByFilter(typeTraining
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
        /// List the type training by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="entity">The type training</param>
        public TypeTrainingEntity TypeTrainingByKey(TypeTrainingEntity typeTraining)
        {
            try
            {
                return TypeTrainingDal.TypeTrainingByKey(typeTraining);
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
        /// Add the type training
        /// </summary>
        /// <param name="entity">The type training</param>
        public TypeTrainingEntity TypeTrainingAdd(TypeTrainingEntity typeTraining)
        {
            try
            {
                return TypeTrainingDal.TypeTrainingAdd(typeTraining);
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
        /// Edit the type training
        /// </summary>
        /// <param name="entity">The type training</param>
        public TypeTrainingEntity TypeTrainingEdit(TypeTrainingEntity typeTraining)
        {
            try
            {
                return TypeTrainingDal.TypeTrainingEdit(typeTraining);
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
        /// List the type training by catalog 
        /// </summary>
        /// <returns></returns>
        public ListItem[] TypeTrainingListByCatalog()
        {
            try
            {
                return TypeTrainingDal.TypeTrainingListByCatalog();
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
