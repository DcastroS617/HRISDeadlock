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
    public class ClassroomsBll : IClassroomsBll<ClassroomEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IClassroomsDal<ClassroomEntity> ClassroomsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public ClassroomsBll(IClassroomsDal<ClassroomEntity> objDal)
        {
            ClassroomsDal = objDal;            
        }

        /// <summary>
        /// List the classrooms by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="classroomCode">Code</param>
        /// <param name="classroomDescription">Description</param>
        /// <param name="trainingCenterCode">Training center code</param>
        /// <param name="minCapacity">Min capacity</param>
        /// <param name="maxCapacity">Max capacity</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>        
        /// <returns>The classrooms meeting the given filters and page config</returns>
        public PageHelper<ClassroomEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string classroomCode, string classroomDescription, string trainingCenterCode, int? minCapacity, int? maxCapacity, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<ClassroomEntity> pageHelper = ClassroomsDal.ListByFilters(divisionCode
                    , geographicDivisionCode
                    , classroomCode
                    , classroomDescription
                    , trainingCenterCode
                    , minCapacity
                    , maxCapacity
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
        /// List the classroom by division key: Division an d GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The classrooms meeting the given filters</returns>
        public List<ClassroomEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return ClassroomsDal.ListByDivision(divisionCode, geographicDivisionCode);
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
        /// List the classroom by training center: GeographicDivisionCode and TraningCenterCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingCenterCode">Trianing center code</param>
        /// <returns>The classrooms meeting the given filters</returns>
        public List<ClassroomEntity> ListByTrainingCenter(string geographicDivisionCode, string trainingCenterCode,int divisionCode)
        {
            try
            {
                return ClassroomsDal.ListByTrainingCenter(geographicDivisionCode, trainingCenterCode,divisionCode);
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
        /// List the classroom by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="classroomCode">Classroom code</param>
        /// <returns>The classroom</returns>
        public ClassroomEntity ListByKey(string geographicDivisionCode, string classroomCode, int DivisionCode)
        {
            try
            {                
                return ClassroomsDal.ListByKey(geographicDivisionCode, classroomCode,DivisionCode);
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
        /// Add the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>
        /// <returns>Tuple: En the first item a bool: true if classroom successfully added. False otherwise
        /// Second item: the classroom added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, ClassroomEntity> Add(ClassroomEntity entity)
        {
            try
            {
                var division =  entity.DivisionCode;

                ClassroomEntity previousClassroom = ClassroomsDal.ListByKey(entity.GeographicDivisionCode, entity.ClassroomCode, division, entity.ClassroomDescription);

                if (previousClassroom == null)
                {
                    ClassroomsDal.Add(entity);
                    return new Tuple<bool, ClassroomEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, ClassroomEntity>(false, previousClassroom);
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
        /// Edit the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>       
        public Tuple<bool, ClassroomEntity> Edit(ClassroomEntity entity)
        {
            try
            {
                ClassroomEntity previousClassroom = ClassroomsDal.ListByKey(entity.GeographicDivisionCode, null, entity.DivisionCode, entity.ClassroomDescription);
                
                if (previousClassroom == null || previousClassroom?.ClassroomCode==entity.ClassroomCode || previousClassroom?.Deleted==false)
                {
                    ClassroomsDal.Edit(entity);
                    return new Tuple<bool, ClassroomEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, ClassroomEntity>(false, previousClassroom);
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
        /// Delete the associated classroowm
        /// </summary>
        /// <param name="entity">The classroom</param>
        public void DeleteAssociatedClassroom(ClassroomEntity entity)
        {
            try
            {
                ClassroomsDal.DeleteAssociatedClassroom(entity);
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
        /// Delete the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>
        public void Delete(ClassroomEntity entity)
        {
            try
            {
                ClassroomsDal.Delete(entity);
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
        /// Activate the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>
        public void Activate(ClassroomEntity entity)
        {
            try
            {
                ClassroomsDal.Activate(entity);
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
