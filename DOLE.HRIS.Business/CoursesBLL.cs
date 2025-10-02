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
    public class CoursesBll : ICoursesBll<CourseEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ICoursesDal<CourseEntity> CoursesDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public CoursesBll(ICoursesDal<CourseEntity> objDal)
        {
            CoursesDal = objDal;
        }

        /// <summary>
        /// List the courses by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Code</param>
        /// <param name="courseName">Description</param>
        /// <param name="courseAcronym">Acroonym</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The courses meeting the given filters and page config</returns>
        public PageHelper<CourseEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string courseCode, string courseName, string courseState,string sortExpression, string sortDirection, int? pageNumber, string Lang = "ES")
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<CourseEntity> pageHelper = CoursesDal.ListByFilters(divisionCode, geographicDivisionCode, courseCode, courseName, courseState,
                    sortExpression, sortDirection, 
                    pageNumber.Value, null, pageSizeValue,
                    Lang);
               
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
        /// List the courses by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return CoursesDal.ListByDivision(divisionCode, geographicDivisionCode);
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
        /// List the course by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The course</returns>
        public CourseEntity ListByKey(string geographicDivisionCode, string courseCode, int divisionCode)
        {
            try
            {
                return CoursesDal.ListByKey(geographicDivisionCode, courseCode, divisionCode);
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
        /// Add the course
        /// </summary>
        /// <param name="entity">The course</param>
        /// <returns>Tuple: En the first item a bool: true if course successfully added. False otherwise
        /// Second item: the course added if true was return in first item. Existing class by code if false.</returns>
        public Tuple<bool, CourseEntity> Add(CourseEntity entity)
        {
            try
            {
                CourseEntity previousCourse = CoursesDal.ListByKey(entity.GeographicDivisionCode, entity.CourseCode, entity.DivisionCode);

                if (previousCourse == null)
                {
                    CoursesDal.Add(entity);
                    return new Tuple<bool, CourseEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, CourseEntity>(false, previousCourse);
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
        /// Edit the course
        /// </summary>
        /// <param name="entity">The course</param>       
        public Tuple<bool, CourseEntity> Edit(CourseEntity entity)
        {
            try
            {
                CourseEntity previousCourse = CoursesDal.ListByKey(entity.GeographicDivisionCode, entity.CourseCode, entity.DivisionCode);

                if (previousCourse == null || previousCourse?.CourseCode == entity.CourseCode || previousCourse?.Deleted == false)
                {
                    CoursesDal.Edit(entity);
                    return new Tuple<bool, CourseEntity>(true, previousCourse);
                }

                else
                {
                    return new Tuple<bool, CourseEntity>(false, previousCourse);
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
        /// Delete the course
        /// </summary>
        /// <param name="entity">The course</param>
        public void Delete(CourseEntity entity)
        {
            try
            {
                CoursesDal.Delete(entity);
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
        /// Activate the course
        /// </summary>
        /// <param name="entity">The course</param>
        public void Activate(CourseEntity entity)
        {
            try
            {
                CoursesDal.Activate(entity);
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
        /// 
        /// </summary>
        /// <param name="Lang"></param>
        /// <returns></returns>
        public List<ClassificationCourseEntity> ClassificationCourseListGet(string Lang)
        {
            try
            {
                return CoursesDal.ClassificationCourseListGet(Lang);
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
        /// cOURSE PARAMETERS  MAX COUNT WORK
        /// </summary>
        /// <param name="@PiCoursesParametersId">rOW /param>
        /// <param name="@PcGeographicDivisionCode">Geographic division code</param>
        /// <param name="@PiDivisionCode">dIVISION CODE</param>
        /// <param name="trainingModuleCode">Training module code</param>
        /// <returns> cOURSE PARAMETERS  MAX COUNT WORK</returns>
        /// 
        public short MaxAmountWorkCourses()
        {
            try
            {
                return CoursesDal.MaxAmountWorkCourses();
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
        /// List the course by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The course</returns>
        public List<CourseEntity> ListByKeyExport(string geographicDivisionCode, int divisionCode)
        {
            try
            {
                return CoursesDal.ListByKeyExport(geographicDivisionCode, divisionCode);
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
        /// List the courses by training program: GeographicDivisionCode and TrainingProgramCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="trainingProgramCode">trainingProgramCode</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByTrainingProgram(string geographicDivisionCode, int divisionCode, string trainingProgramCode)
        {
            try
            {
                return CoursesDal.ListByTrainingProgram(geographicDivisionCode, divisionCode ,trainingProgramCode);
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
        /// Add the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainingProgram">the trainingProgram</param>
        public void AddCourseByTrainingProgram(CourseEntity entity, TrainingProgramEntity trainingProgram)
        {
            try
            {
                CoursesDal.AddCourseByTrainingProgram(entity, trainingProgram);
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
        /// Delete the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainingProgram">the trainingProgram</param>
        public void DeleteCourseByTrainingProgram(CourseEntity entity, TrainingProgramEntity trainingProgram)
        {
            try
            {
                CoursesDal.DeleteCourseByTrainingProgram(entity, trainingProgram);
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
        /// List the courses by school training: schoolTrainingCode
        /// </summary>        
        /// <param name="schoolTrainingCode">schoolTrainingCode</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListBySchoolsTraining(string geographicDivisionCode, int DivisionCode, string schoolTrainingCode)
        {
            try
            {
                return CoursesDal.ListBySchoolsTraining(geographicDivisionCode, DivisionCode, schoolTrainingCode);
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
        /// Add the relation between the course and the school training
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="schoolsTraining">the schoolsTraining</param>
        public void AddCourseBySchoolsTraining(CourseEntity entity, SchoolTrainingEntity schoolsTraining)
        {
            try
            {
                CoursesDal.AddCourseBySchoolsTraining(entity, schoolsTraining);
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
        /// Delete the relation between the course and the school training
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="schoolsTraining">the schoolsTraining</param>
        public void DeleteCourseBySchoolsTraining(CourseEntity entity, SchoolTrainingEntity schoolsTraining)
        {
            try
            {
                CoursesDal.DeleteCourseBySchoolsTraining(entity, schoolsTraining);
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
        /// List the course associated with a trainer key: GeographicDivisionCode and trainerCode 
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">division code</param>
        /// <param name="trainerCode">Trainer code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<CourseEntity> ListByCoursesByTrainersAssociated(string geographicDivisionCode, int divisionCode, string trainerCode)
        {
            try
            {
                return CoursesDal.ListByCoursesByTrainerAssociated(geographicDivisionCode, divisionCode, trainerCode);
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
        /// List the course not associated with a trainer key: GeographicDivisionCode 
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">division code</param>
        /// <param name="trainerCode">Trainer code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<CourseEntity> ListByCourseByTrainersNotAssociated(string geographicDivisionCode, int divisionCode, string trainerCode)
        {
            try
            {
                return CoursesDal.ListByCourseByTrainersNotAssociated(geographicDivisionCode, divisionCode, trainerCode);
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
        /// Add the relation between the course and the trainer
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainer">the trainer</param>
        public void AddTrainerByCourse(CourseEntity entity, TrainerEntity trainer)
        {
            try
            {
                CoursesDal.AddTrainerByCourse(entity, trainer);
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
        /// Delete the relation between the course and the trainer
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainer">the trainer</param>
        public void DeleteTrainerByCourse(CourseEntity entity, TrainerEntity trainer)
        {
            try
            {
                CoursesDal.DeleteTrainerByCourse(entity, trainer);
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
        /// List the courses by training program: GeographicDivisionCode and PaymentRateCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="paymentRateCode">paymentRateCode</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByPaymentRate(string geographicDivisionCode, int divisionCode, int paymentRateCode)
        {
            try
            {
                return CoursesDal.ListByPaymentRate(geographicDivisionCode, divisionCode, paymentRateCode);
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
        /// Add the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="paymentRate">the paymentRate</param>
        public void AddCourseByPaymentRate(CourseEntity entity, PaymentRateEntity paymentRate)
        {
            try
            {
                CoursesDal.AddCourseByPaymentRate(entity, paymentRate);
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
        /// Delete the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="paymentRate">the paymentRate</param>
        public void DeleteCourseByPaymentRate(CourseEntity entity, PaymentRateEntity paymentRate)
        {
            try
            {
                CoursesDal.DeleteCourseByPaymentRate(entity, paymentRate);
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
        /// List the courses by position: GeographicDivisionCode and PositionCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="positionCode">positionCode</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByPosition(string geographicDivisionCode, string positionCode)
        {
            try
            {
                return CoursesDal.ListByPosition(geographicDivisionCode, positionCode);
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
        /// Add the relation between the course and the position
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="position">the position</param>
        public void AddCourseByPosition(CourseEntity entity, PositionEntity position)
        {
            try
            {
                CoursesDal.AddCourseByPosition(entity, position);
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
        /// Delete the relation between the course and the position
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="position">the position</param>
        public void DeleteCourseByPosition(CourseEntity entity, PositionEntity position)
        {
            try
            {
                CoursesDal.DeleteCourseByPosition(entity, position);
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
        /// List the courses by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByDivisionNotThematicAreaAssociated(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return CoursesDal.ListByDivisionNotThematicAreaAssociated(divisionCode, geographicDivisionCode);
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
        /// List the courses by thematic area: GeographicDivisionCode and ThematicAreaCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="thematicAreaCode">thematicAreaCode</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByThematicArea(string geographicDivisionCode, int divisionCode, string thematicAreaCode)
        {
            try
            {
                return CoursesDal.ListByThematicArea(geographicDivisionCode, divisionCode, thematicAreaCode);
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
        /// Add the relation between the course and the thematic area
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        public void AddCourseByThematicArea(CourseEntity entity, ThematicAreaEntity thematicArea)
        {
            try
            {
                CoursesDal.AddCourseByThematicArea(entity, thematicArea);
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
        /// Validates a course by thematic area using the provided entity parameters.
        /// </summary>
        /// <param name="entity">The course entity containing DivisionCode and CourseCode.</param>
        /// <returns>
        /// Returns an integer result from the stored procedure indicating the validation status.
        /// </returns>
        /// <exception cref="DataAccessException">
        /// Thrown when an SQL error occurs during the database operation or for other exceptions during data access.
        /// </exception>
        public int ValidateCourseByThematicArea(CourseEntity entity)
        {
            try
            {
               return CoursesDal.ValidateCourseByThematicArea(entity);
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
        /// Delete the relation between the course and the thematic area
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        public void DeleteCourseByThematicArea(CourseEntity entity, ThematicAreaEntity thematicArea)
        {
            try
            {
                CoursesDal.DeleteCourseByThematicArea(entity, thematicArea);
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
        /// List the courses used in logbooks by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByDivisionUsedByLogbooks(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return CoursesDal.ListByDivisionUsedByLogbooks(divisionCode, geographicDivisionCode);
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
        /// List the courses used in logbooks history by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        public List<CourseEntity> ListByDivisionUsedByLogbooksHistory(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return CoursesDal.ListByDivisionUsedByLogbooksHistory(divisionCode, geographicDivisionCode);
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
        /// List the Courses that meet the filters and is related to a logbook
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="user">User</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<CourseEntity> ListByLogbook(int divisionCode, string geographicDivisionCode, string user)
        {
            try
            {
                return CoursesDal.ListByLogbook(divisionCode, geographicDivisionCode, user);
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
