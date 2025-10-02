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
    public class TrainingCentersBll : ITrainingCentersBll<TrainingCenterEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ITrainingCentersDal<TrainingCenterEntity> TrainingCentersDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public TrainingCentersBll(ITrainingCentersDal<TrainingCenterEntity> objDal)
        {
            TrainingCentersDal = objDal;            
        }

        /// <summary>
        /// Activate the training center
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingCenterCode">The training center code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public void Activate(string geographicDivisionCode, string trainingCenterCode, string lastModifiedUser)
        {
            try
            {
                TrainingCenterEntity center = new TrainingCenterEntity(geographicDivisionCode, trainingCenterCode)
                {
                    LastModifiedUser = lastModifiedUser
                };

                TrainingCentersDal.Activate(center);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersActivate, ex);
                }
            }
        }

        /// <summary>
        /// Add the training center
        /// </summary>
        /// <param name="entity">The training center entity</param>
        public TrainingCenterEntity Add(TrainingCenterEntity entity)
        {
            try
            {
                return TrainingCentersDal.Add(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersAdd, ex);
                }
            }
        }
        
        /// <summary>
        /// Delete the training center
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingCenterCode">The training center code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public void Delete(string geographicDivisionCode, string trainingCenterCode, string lastModifiedUser)
        {
            try
            {
                TrainingCenterEntity center = new TrainingCenterEntity(geographicDivisionCode, trainingCenterCode)
                {
                    LastModifiedUser = lastModifiedUser
                };

                TrainingCentersDal.Delete(center);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersDelete, ex);
                }
            }
        }

        /// <summary>
        /// Edit the training center
        /// </summary>
        /// <param name="entity">The training center entity</param>
        public TrainingCenterEntity Edit(TrainingCenterEntity entity)
        {
            try
            {
                return TrainingCentersDal.Edit(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersEdit, ex);
                }
            }
        }
       
        /// <summary>
        /// List the training center by code
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingCenterCode">The training center code</param>
        /// <returns>The training center</returns>
        public TrainingCenterEntity ListByCode(string geographicDivisionCode,int divisionCode,string trainingCenterCode,string trainingCenterDes=null)
        {
            try
            {
                return TrainingCentersDal.ListByCode(new TrainingCenterEntity(geographicDivisionCode, trainingCenterCode) {
                    DivisionCode=divisionCode,TrainingCenterDescription=trainingCenterDes
                });
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersEdit, ex);
                }
            }
        }

        /// <summary>
        /// List the classrooms by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training centers meeting the given filter</returns>
        public List<TrainingCenterEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {            
            try
            {   
                return TrainingCentersDal.ListByDivision(divisionCode, geographicDivisionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersListByDivision, ex);
                }
            }            
        }

        /// <summary>
        /// List the division filter cb
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="geographicDivisionCode"></param>
        /// <returns></returns>s
        public List<TrainingCenterEntity> ListByDivisionFilterCB(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return TrainingCentersDal.ListByDivisionFilterCB(divisionCode, geographicDivisionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        /// List the training centers used in logbooks by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training centers meeting the given filter</returns>
        public List<TrainingCenterEntity> ListByDivisionUsedByLogbooks(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return TrainingCentersDal.ListByDivisionUsedByLogbooks(divisionCode, geographicDivisionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        /// List the training centers used in logbooks history by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training centers meeting the given filter</returns>
        public List<TrainingCenterEntity> ListByDivisionUsedByLogbooksHistory(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return TrainingCentersDal.ListByDivisionUsedByLogbooksHistory(divisionCode, geographicDivisionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }

        /// <summary>
        /// List the training Centers by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingCenterCode">The training center Code</param>
        /// <param name="trainingCenterDescription">The training center Description</param>
        /// <param name="placeLocation">The place location</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The training Centers meeting the given filters and page config</returns>
        public PageHelper<TrainingCenterEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string trainingCenterCode, string trainingCenterDescription, string placeLocation, string sortExpression, string sortDirection, int pageNumber)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<TrainingCenterEntity> pageHelper = TrainingCentersDal.ListByFilters(divisionCode, geographicDivisionCode, trainingCenterCode, trainingCenterDescription, placeLocation, 
                    sortExpression, sortDirection, pageNumber, pageSizeValue);

                pageHelper.TotalPages = (pageHelper.TotalResults - 1) / pageHelper.PageSize + 1;

                return pageHelper;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersListByFilters, ex);
                }
            }
        }

        /// <summary>
        /// List the training center by description
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingCenterDescription">The training center description</param>
        /// <returns>The training center</returns>
        public TrainingCenterEntity ListByDescription(string geographicDivisionCode, string trainingCenterDescription)
        {
            try
            {
                return TrainingCentersDal.ListByDescription(new TrainingCenterEntity(geographicDivisionCode, trainingCenterDescription));
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersEdit, ex);
                }
            }
        }

        /// <summary>
        /// Method that validates if there is a training center with that description.
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingCenterCode">The training center code</param>
        /// <param name="trainingCenterDescription">The training center Description</param> 
        public Tuple<bool, TrainingCenterEntity> ValidatedDescription(string geographicDivisionCode, string trainingCenterCode, string trainingCenterDescription, int DivisionCode, PlaceLocation placeLocation)
        {
            try
            {
                TrainingCenterEntity previousTrainingCenter = TrainingCentersDal.ListByDescription(
                    new TrainingCenterEntity(geographicDivisionCode, trainingCenterCode, trainingCenterDescription) { 
                        PlaceLocation= placeLocation,DivisionCode= DivisionCode 
                    });

                if (previousTrainingCenter == null)
                {
                    return new Tuple<bool, TrainingCenterEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, TrainingCenterEntity>(false, previousTrainingCenter);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersAdd, ex);
                }
            }
        }

        /// <summary>
        /// List the Training Centers that meet the filters and is related to a logbook
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="user">User</param>
        /// <returns>The training centers meeting the given filter</returns>
        public List<TrainingCenterEntity> ListByLogbook(int divisionCode, string geographicDivisionCode, string user)
        {
            try
            {
                return TrainingCentersDal.ListByLogbook(divisionCode, geographicDivisionCode, user);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjTrainingCentersListByDivision, ex);
                }
            }
        }
    }
}