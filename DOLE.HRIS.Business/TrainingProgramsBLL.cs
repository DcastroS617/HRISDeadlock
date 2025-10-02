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
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class TrainingProgramsBll : ITrainingProgramsBll<TrainingProgramEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ITrainingProgramsDal<TrainingProgramEntity> TrainingProgramsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public TrainingProgramsBll(ITrainingProgramsDal<TrainingProgramEntity> objDal)
        {
            TrainingProgramsDal = objDal;
        }

        /// <summary>
        /// Activate the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingProgramCode">The training Program code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public void Activate(string geographicDivisionCode, string trainingProgramCode, string lastModifiedUser)
        {
            try
            {
                TrainingProgramEntity Program = new TrainingProgramEntity(geographicDivisionCode, trainingProgramCode)
                {
                    LastModifiedUser = lastModifiedUser
                };

                TrainingProgramsDal.Activate(Program);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingProgramsActivate, ex);
                }
            }
        }

        /// <summary>
        /// Add the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingProgramCode">The training Program code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="trainingProgramName">The training Program name</param> 
        /// <param name="placeLocation">The training Program Location</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public Tuple<bool, TrainingProgramEntity> Add(string geographicDivisionCode, string trainingProgramCode, int divisionCode, string trainingProgramName, bool searchEnabled, string lastModifiedUser)
        {
            try
            {
                TrainingProgramEntity previousTrainingProgram = TrainingProgramsDal.ListByCode(new TrainingProgramEntity(geographicDivisionCode, trainingProgramCode, divisionCode, trainingProgramName));

                if (previousTrainingProgram == null)
                {
                    TrainingProgramEntity entity = new TrainingProgramEntity(geographicDivisionCode, 
                        trainingProgramCode, 
                        divisionCode, 
                        trainingProgramName, 
                        searchEnabled, 
                        lastModifiedUser);

                    TrainingProgramsDal.Add(entity);

                    return new Tuple<bool, TrainingProgramEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, TrainingProgramEntity>(false, previousTrainingProgram);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingProgramsAdd, ex);
                }
            }
        }

        /// <summary>
        /// Delete the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingProgramCode">The training Program code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public void Delete(string geographicDivisionCode, int divisionCode, string trainingProgramCode, string lastModifiedUser)
        {
            try
            {
                TrainingProgramEntity Program = new TrainingProgramEntity(geographicDivisionCode, trainingProgramCode, divisionCode, null)
                {
                    LastModifiedUser = lastModifiedUser
                };

                TrainingProgramsDal.Delete(Program);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingProgramsDelete, ex);
                }
            }
        }

        /// <summary>
        /// Edit the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="trainingProgramCode">The training Program code</param>
        /// <param name="trainingProgramName">The training Program Name</param> 
        /// <param name="placeLocation">The training Program Location</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="deleted">Deleted ?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public Tuple<bool, TrainingProgramEntity> Edit(string geographicDivisionCode, int divisionCode, string trainingProgramCode, string trainingProgramName, bool searchEnabled, bool deleted, string lastModifiedUser)
        {
            try
            {
                TrainingProgramEntity previousTrainingProgram = TrainingProgramsDal.ListByCode(new TrainingProgramEntity(geographicDivisionCode, "", trainingProgramName));

                if (previousTrainingProgram == null || previousTrainingProgram?.TrainingProgramCode == trainingProgramCode || previousTrainingProgram?.Deleted == false)
                {
                    TrainingProgramsDal.Edit(new TrainingProgramEntity(geographicDivisionCode, 
                        trainingProgramCode,
                        divisionCode,
                        trainingProgramName, 
                        searchEnabled, 
                        deleted, 
                        lastModifiedUser,
                        DateTime.Now));

                    return new Tuple<bool, TrainingProgramEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, TrainingProgramEntity>(false, previousTrainingProgram);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingProgramsEdit, ex);
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
                TrainingProgramsDal.AddCourseByTrainingProgram(entity, trainingProgram);
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
                TrainingProgramsDal.DeleteCourseByTrainingProgram(entity, trainingProgram);
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
        /// List the training programs key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training programs meeting the given filters</returns>
        public ListItem[] TrainingProgramsList(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return TrainingProgramsDal.TrainingProgramsList(divisionCode, geographicDivisionCode);
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
        /// List the training programs by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training programs meeting the given filters</returns>
        public List<TrainingProgramEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return TrainingProgramsDal.ListByDivision(divisionCode, geographicDivisionCode);
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
        /// List the training programs by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="courseCode"Course code</param>
        /// <returns>The training programs meeting the given filters</returns>
        public List<TrainingProgramEntity> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode)
        {
            try
            {
                return TrainingProgramsDal.ListByCourse(geographicDivisionCode, divisionCode,courseCode);
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
        /// List the training Program by code
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingProgramCode">The training Program code</param>
        /// <param name="divisionCode">The division code</param>
        /// <returns>The training Program</returns>
        public TrainingProgramEntity ListByCode(string geographicDivisionCode, string trainingProgramCode, int divisionCode)
        {
            try
            {
                return TrainingProgramsDal.ListByCode(new TrainingProgramEntity(geographicDivisionCode, trainingProgramCode, divisionCode, null));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingProgramsEdit, ex);
                }
            }
        }

        /// <summary>
        /// List the training Programs by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingProgramCode">The training Program Code</param>
        /// <param name="trainingProgramName">The training Program Name</param>
        /// <param name="placeLocation">The place location</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The training Programs meeting the given filters and page config</returns>
        public PageHelper<TrainingProgramEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string trainingProgramCode, string trainingProgramName, string sortExpression, string sortDirection, int pageNumber)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<TrainingProgramEntity> pageHelper = TrainingProgramsDal.ListByFilters(divisionCode
                    , geographicDivisionCode
                    , trainingProgramCode
                    , trainingProgramName
                    ,  sortExpression
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
                    throw new BusinessException(msjTrainingProgramsListByFilters, ex);
                }
            }
        }
    }
}