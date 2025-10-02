using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class MasterProgramBll : IMasterProgramBll
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IMasterProgramDal MasterProgramDal;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public MasterProgramBll(IMasterProgramDal obj)
        {
            MasterProgramDal = obj;
        }

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public PageHelper<MasterProgramEntity> MasterProgramByFilter(MasterProgramEntity entity, int Divisioncode, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                var pageHelper = MasterProgramDal.MasterProgramByFilter(entity
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

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public DbaEntity MasterProgramAdd(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramAdd(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public MasterProgramEntity MasterProgramById(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramById(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public DbaEntity MasterProgramEdit(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramEdit(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public DbaEntity MasterProgramDelete(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramDelete(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public DbaEntity MasterProgramIsExists(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramIsExists(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public ListItem[] MasterProgramList(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramList(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public MasterProgramEntity MasterProgramValidationTypeSearch(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramValidationTypeSearch(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public DbaEntity MasterProgramRelationship(MasterProgramEntity entity, DataTable EmpleadosList, DataTable LaborList, DataTable PositionsList)
        {
            try
            {
                return MasterProgramDal.MasterProgramRelationship(entity, EmpleadosList, LaborList, PositionsList);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public Tuple<MasterProgramEntity, List<string>, List<int>, List<string>> MasterProgramRelationshipById(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramRelationshipById(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public Tuple<int, List<EmployeeEntity>, List<PositionEntity>, List<LaborEntity>> MasterProgramRelatedSummary(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramRelatedSummary(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public List<EmployeeEntity> MasterProgramByEmployeeList(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramByEmployeeList(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public ListItem[] MasterProgramByEmployeesByPlacesOccupation(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramByEmployeesByPlacesOccupation(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public ListItem[] MasterProgramByLaborById(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramByLaborById(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public List<LaborEntity> MasterProgramByLaborByCode(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramByLaborByCode(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public List<PositionEntity> MasterProgramByPositions(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramByPositions(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public ListItem[] MasterProgramByPositionsByPlacesOccupation(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MasterProgramByPositionsByPlacesOccupation(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public ListItem[] MatrixTargetList(MasterProgramEntity entity)
        {
            try
            {
                return MasterProgramDal.MatrixTargetList(entity);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public MatrixMasterProgramResultEntity MasterProgramByCourseByFilter(MasterProgramEntity entity, DataTable ThematicAreas, DataTable Courses, DataTable Positions, DataTable Labors, DataTable Employees)
        {
            try
            {
                return MasterProgramDal.MasterProgramByCourseByFilter(entity,ThematicAreas,Courses, Positions, Labors, Employees);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public List<CourseEntity> CoursesListByThematicArea(string GeographicDivisionCode, int DivisionCode, DataTable ThematicAreaCode)
        {
            try
            {
                return MasterProgramDal.CoursesListByThematicArea(GeographicDivisionCode, DivisionCode, ThematicAreaCode);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public ListItem[] MasterProgramThematicAreasListByCoursesExists(MasterProgramEntity entity)
        {
            try
            {
                ListItem[] masterProgramThematicAreasList = MasterProgramDal.MasterProgramThematicAreasListByCoursesExists(entity);

                if (masterProgramThematicAreasList == null)
                {
                    masterProgramThematicAreasList = new ListItem[] { new ListItem { Value = "", Text = "" } };
                }

                return masterProgramThematicAreasList;
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        public DbaEntity MasterProgramByCourseAdd(DataTable Course)
        {
            try
            {
                return MasterProgramDal.MasterProgramByCourseAdd(Course);
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
        /// List the Master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">division code</param>
        /// <param name="trainingProgramCode">trainingProgramCode</param>
        /// <returns>The training programs meeting the given filters</returns>
        public List<MasterProgramEntity> MasterProgramByTrainingPlanProgramsAssociated(string GeographicDivisionCode, int divisionCode, string trainingPlanProgramCode)
        {
            try
            {
                return MasterProgramDal.MasterProgramByTrainingPlanProgramsAssociated(GeographicDivisionCode, divisionCode, trainingPlanProgramCode);
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
        ///  List the Master Program not assocated with a training programs key: GeographicDivisionCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingProgramCode">trainingProgramCode</param>
        /// <returns>The training programs meeting the given filters</returns>
        public List<MasterProgramEntity> MasterProgramByTrainingPlanProgramsNotAssociated(string GeographicDivisionCode)
        {
            try
            {
                return MasterProgramDal.MasterProgramByTrainingPlanProgramsNotAssociated(GeographicDivisionCode);
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
