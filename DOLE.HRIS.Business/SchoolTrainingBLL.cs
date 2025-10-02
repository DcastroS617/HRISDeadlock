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
    public class SchoolTrainingBll : ISchoolTrainingBll
    {
        private readonly ISchoolTrainingDAL SchoolTrainingDal;

        public SchoolTrainingBll(ISchoolTrainingDAL objDal)
        {
            SchoolTrainingDal = objDal;
        }

        /// <summary>
        /// List the cycle training by the given filters
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic Division Code</param>
        /// <param name="SchoolTraining">The cycle training</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The cycle training meeting the given filters and page config</returns>
        public PageHelper<SchoolTrainingEntity> SchoolTrainingListByFilter(string geographicDivisionCode, SchoolTrainingEntity SchoolTraining, int DivisionCode, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                var pageHelper = SchoolTrainingDal.SchoolTrainingListByFilter(geographicDivisionCode, SchoolTraining, DivisionCode,
                    sortExpression, sortDirection,
                    pageNumber.Value, null, pageSizeValue);

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
        /// List the schools Training
        /// </summary>
        /// <param name="entity">The cycle training</param>
        /// <returns>The schools Training meeting the given filters</returns>
        public List<SchoolTrainingEntity> ListByDivision(SchoolTrainingEntity SchoolTraining)
        {
            try
            {
                return SchoolTrainingDal.ListByDivision(SchoolTraining);
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
        public SchoolTrainingEntity SchoolTrainingByKey(SchoolTrainingEntity SchoolTraining)
        {
            try
            {
                return SchoolTrainingDal.SchoolTrainingByKey(SchoolTraining);
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
        public SchoolTrainingEntity SchoolTrainingAdd(SchoolTrainingEntity SchoolTraining)
        {
            try
            {
                return SchoolTrainingDal.SchoolTrainingAdd(SchoolTraining);
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
        public SchoolTrainingEntity SchoolTrainingEdit(SchoolTrainingEntity SchoolTraining)
        {
            try
            {
                return SchoolTrainingDal.SchoolTrainingEdit(SchoolTraining);
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
        /// List the school training by course: courseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="courseCode">courseCode</param>
        /// <param name="isforce">Is force</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<SchoolTrainingEntity> ListByCourses(string geographicDivisionCode, int divisionCode ,string schoolTrainingCode, bool? isForce=null)
        {
            try
            {
                return SchoolTrainingDal.ListByCourses(geographicDivisionCode,divisionCode, schoolTrainingCode,isForce);
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
        /// Add the relation between the course and the schools Training
        /// </summary>
        /// <param name="entity">The schoolsTraining</param>
        /// <param name="course">the course</param>
        public void AddSchoolsTrainingByCourse(SchoolTrainingEntity entity, CourseEntity course)
        {
            try
            {
                SchoolTrainingDal.AddSchoolsTrainingByCourse(entity, course);
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
        /// Delete the relation between the course and the schools Training
        /// </summary>
        /// <param name="entity">The schoolsTraining</param>
        /// <param name="course">the course</param>
        public void DeleteSchoolsTrainingByCourse(SchoolTrainingEntity entity, CourseEntity course)
        {
            try
            {
                SchoolTrainingDal.DeleteSchoolsTrainingByCourse(entity, course);
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
