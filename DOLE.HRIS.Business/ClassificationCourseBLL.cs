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
    public class ClassificationCourseBll : IClassificationCourseBll
    {
        private readonly IClassificationCourseDal ClassificacitonCourseDal;

        public ClassificationCourseBll(IClassificationCourseDal obj)
        {
            ClassificacitonCourseDal = obj;
        }

        public PageHelper<ClassificationCourseEntity> ClassificationCourseByFilter(ClassificationCourseEntity entity, int Divisioncode, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                var pageHelper = ClassificacitonCourseDal.ClassificationCourseByFilter(entity
                    , ""
                    , Divisioncode
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

        public DbaEntity ClassificationCourseAdd(ClassificationCourseEntity entity)
        {
            try
            {
                return ClassificacitonCourseDal.ClassificationCourseAdd(entity);
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

        public ClassificationCourseEntity ClassificationCourseById(ClassificationCourseEntity entity)
        {
            try
            {
                return ClassificacitonCourseDal.ClassificationCourseById(entity);
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

        public DbaEntity ClassificationCourseDesactivate(ClassificationCourseEntity entity)
        {
            try
            {
                return ClassificacitonCourseDal.ClassificationCourseDesactivate(entity);
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

        public DbaEntity ClassificationCourseEdit(ClassificationCourseEntity entity)
        {
            try
            {
                return ClassificacitonCourseDal.ClassificationCourseEdit(entity);
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
