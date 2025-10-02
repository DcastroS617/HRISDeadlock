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
    public class TrainersBll : ITrainersBll<TrainerEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ITrainersDal<TrainerEntity> TrainersDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public TrainersBll(ITrainersDal<TrainerEntity> objDal)
        {
            TrainersDal = objDal;            
        }

        /// <summary>
        /// List the trainers by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainerCode">Code</param>
        /// <param name="trainerName">Description</param>
        /// <param name="trainerType">Training center code</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The trainers meeting the given filters and page config</returns>
        public PageHelper<TrainerEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string trainerCode, string trainerName, string trainerType, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<TrainerEntity> pageHelper = TrainersDal.ListByFilters(divisionCode, geographicDivisionCode
                    , trainerCode
                    , trainerName
                    , trainerType
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
        /// List the trainers by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<TrainerEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return TrainersDal.ListByDivision(divisionCode, geographicDivisionCode);
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
        /// List the trainers used in logbooks by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<TrainerEntity> ListByDivisionUsedByLogbooks(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return TrainersDal.ListByDivisionUsedByLogbooks(divisionCode, geographicDivisionCode);
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
        /// List the trainers used in logbooks history by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<TrainerEntity> ListByDivisionUsedByLogbooksHistory(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return TrainersDal.ListByDivisionUsedByLogbooksHistory(divisionCode, geographicDivisionCode);
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
        /// List the trainers by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="courseCode"Course code</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<TrainerEntity> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode, bool? isForce = null)
        {
            try
            {
                return TrainersDal.ListByCourse( geographicDivisionCode,  divisionCode,  courseCode,isForce);
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
        /// List the trainer by key: GeographicDivisionCode, Type and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainerType">Trainer type</param>
        /// <param name="trainerCode">Trainer code</param>
        /// <returns>The trainer</returns>
        public TrainerEntity ListByKey(string geographicDivisionCode, int divisionCode, TrainerType? trainerType, string trainerCode)
        {
            try
            {
                return TrainersDal.ListByKey(geographicDivisionCode, divisionCode, trainerType, trainerCode);
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
        /// Add the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>
        /// <returns>Tuple: En the first item a bool: true if trainer successfully added. False otherwise
        /// Second item: the trainer added if true was return in first item. Existing class by type and code if false.</returns>
        public Tuple<bool, TrainerEntity> Add(TrainerEntity entity)
        {
            try
            {
                TrainerEntity previousTrainer = TrainersDal.ListByKey(entity.GeographicDivisionCode, entity.DivisionCode, entity.TrainerType,  entity.TrainerCode);
                if (previousTrainer == null)
                {
                    TrainersDal.Add(entity);
                    return new Tuple<bool, TrainerEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, TrainerEntity>(false, previousTrainer);
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
        /// Edit the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>       
        public void Edit(TrainerEntity entity)
        {
            try
            {
                TrainersDal.Edit(entity);
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
        /// Delete the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>
        public void Delete(TrainerEntity entity)
        {
            try
            {
                TrainersDal.Delete(entity);
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
        /// Activate the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>
        public void Activate(TrainerEntity entity)
        {
            try
            {
                TrainersDal.Activate(entity);
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
        /// List the trainers that meet the filters and is related to a logbook
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="user">User</param>
        /// <returns>The trainers meeting the given filters</returns>
        public List<TrainerEntity> ListByLogbook(int divisionCode, string geographicDivisionCode, string user)
        {
            try
            {
                return TrainersDal.ListByLogbook(divisionCode, geographicDivisionCode, user);
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
