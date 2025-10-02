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
    public class TrainingPlanProgramsBll : ITrainingPlanProgramsBll<TrainingPlanProgramEntity>
    {        
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ITrainingPlanProgramsDal<TrainingPlanProgramEntity> TrainingPlanProgramsDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public TrainingPlanProgramsBll(ITrainingPlanProgramsDal<TrainingPlanProgramEntity> objDal)
        {
            TrainingPlanProgramsDal = objDal;
        }

        /// <summary>
        /// Add the training plan program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingPlanProgramCode">The training Plan Program code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="trainingPlanProgramName">The training Plan Program name</param> 
        /// <param name="placeLocation">The training Plan Program Location</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public Tuple<bool, TrainingPlanProgramEntity> Add(string geographicDivisionCode, string trainingPlanProgramCode, int divisionCode, string trainingPlanProgramName, bool searchEnabled, string lastModifiedUser)
        {
            try
            {
                TrainingPlanProgramEntity previousTrainingPlanProgram = TrainingPlanProgramsDal.ListByCode(new TrainingPlanProgramEntity(geographicDivisionCode, trainingPlanProgramCode, divisionCode, trainingPlanProgramName, true, null));

                if (previousTrainingPlanProgram == null)
                {
                    TrainingPlanProgramEntity entity = new TrainingPlanProgramEntity(geographicDivisionCode, 
                        trainingPlanProgramCode, 
                        divisionCode, 
                        trainingPlanProgramName, 
                        searchEnabled, 
                        lastModifiedUser);

                    TrainingPlanProgramsDal.Add(entity);

                    return new Tuple<bool, TrainingPlanProgramEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, TrainingPlanProgramEntity>(false, previousTrainingPlanProgram);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingPlanProgramsAdd, ex);
                }
            }
        }

        /// <summary>
        /// Edit the training Plan Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingPlanProgramCode">The training Plan Program code</param>
        /// <param name="trainingPlanProgramName">The training Plan Program Name</param> 
        /// <param name="placeLocation">The training Plan Program Location</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="deleted">Deleted ?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public Tuple<bool, TrainingPlanProgramEntity> Edit(string geographicDivisionCode, string trainingPlanProgramCode, string trainingPlanProgramName, bool searchEnabled, bool deleted, string lastModifiedUser)
        {
            try
            {
                TrainingPlanProgramEntity previousTrainingPlanProgram = TrainingPlanProgramsDal.ListByCode(new TrainingPlanProgramEntity(geographicDivisionCode, "", trainingPlanProgramName));

                if (previousTrainingPlanProgram == null || previousTrainingPlanProgram?.TrainingPlanProgramCode == trainingPlanProgramCode || previousTrainingPlanProgram?.Deleted == false)
                {
                    TrainingPlanProgramsDal.Edit(new TrainingPlanProgramEntity(geographicDivisionCode, 
                        trainingPlanProgramCode, 
                        trainingPlanProgramName, 
                        searchEnabled, 
                        deleted, 
                        lastModifiedUser));

                    return new Tuple<bool, TrainingPlanProgramEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, TrainingPlanProgramEntity>(false, previousTrainingPlanProgram);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingPlanProgramsEdit, ex);
                }
            }
        }

        /// <summary>
        /// Delete the training Plan Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="divisionCode">The geographic division code</param>
        /// <param name="trainingPlanProgramCode">The training Plan Program code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public void Delete(string geographicDivisionCode, int divisionCode, string trainingPlanProgramCode, string lastModifiedUser)
        {
            try
            {
                TrainingPlanProgramEntity Program = new TrainingPlanProgramEntity(geographicDivisionCode, trainingPlanProgramCode, divisionCode, null)
                {
                    LastModifiedUser = lastModifiedUser
                };

                TrainingPlanProgramsDal.Delete(Program);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingPlanProgramsDelete, ex);
                }
            }
        }

        /// <summary>
        /// List the training Plan Programs by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingPlanProgramCode">The training Plan Program Code</param>
        /// <param name="trainingPlanProgramName">The training Plan Program Name</param>
        /// <param name="placeLocation">The place location</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The training Plan Programs meeting the given filters and page config</returns>
        public PageHelper<TrainingPlanProgramEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string trainingPlanProgramCode, string trainingPlanProgramName, string sortExpression, string sortDirection, int pageNumber)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<TrainingPlanProgramEntity> pageHelper = TrainingPlanProgramsDal.ListByFilters(divisionCode
                    , geographicDivisionCode
                    , trainingPlanProgramCode
                    , trainingPlanProgramName
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
                    throw new BusinessException(msjTrainingPlanProgramsListByFilters, ex);
                }
            }
        }

        /// <summary>
        /// List the training Plan Program by code
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="trainingPlanProgramCode">The training Plan Program code</param>
        /// <returns>The training Plan Program</returns>
        public TrainingPlanProgramEntity ListByCode(string geographicDivisionCode, int divisionCode, string trainingPlanProgramCode)
        {
            try
            {
                return TrainingPlanProgramsDal.ListByCode(new TrainingPlanProgramEntity(geographicDivisionCode, trainingPlanProgramCode, divisionCode, null));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingPlanProgramsEdit, ex);
                }
            }
        }

        /// <summary>
        /// List the training Plan Programs key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training Plan Programs meeting the given filters</returns>
        public ListItem[] TrainingPlanProgramsList(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return TrainingPlanProgramsDal.TrainingPlanProgramsList(divisionCode, geographicDivisionCode);
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
        /// Activate the training Plan Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingPlanProgramCode">The training Plan Program code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public void Activate(string geographicDivisionCode, string trainingPlanProgramCode, string lastModifiedUser)
        {
            try
            {
                TrainingPlanProgramEntity Program = new TrainingPlanProgramEntity(geographicDivisionCode, trainingPlanProgramCode)
                {
                    LastModifiedUser = lastModifiedUser
                };

                TrainingPlanProgramsDal.Activate(Program);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingPlanProgramsActivate, ex);
                }
            }
        }

        /// <summary>
        /// Add the relation between the master program and the training Plan Programs
        /// </summary>
        /// <param name="entity">The master program</param>
        /// <param name="trainingPlanProgram">the trainingPlanProgram</param>
        public void AddMasterProgramByTrainingPlanPrograms(MasterProgramEntity entity, TrainingPlanProgramEntity trainingPlanProgram)
        {
            try
            {
                TrainingPlanProgramsDal.AddMasterProgramByTrainingPlanPrograms(entity, trainingPlanProgram);
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
        /// Delete the relation between the master program and the training Plan Programs
        /// </summary>
        /// <param name="entity">The master program</param>
        /// <param name="trainingPlanProgram">the trainingPlanProgram</param>
        public void DeleteMasterProgramByTrainingPlanPrograms(MasterProgramEntity entity, TrainingPlanProgramEntity trainingPlanProgram)
        {
            try
            {
                TrainingPlanProgramsDal.DeleteMasterProgramByTrainingPlanPrograms(entity, trainingPlanProgram);
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