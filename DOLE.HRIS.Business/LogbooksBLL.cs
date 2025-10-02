using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Configuration;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    public class LogbooksBll : ILogbooksBll
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ILogbooksDal LogbooksDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public LogbooksBll(ILogbooksDal objDal)
        {
            LogbooksDal = objDal;
        }

        /// <summary>
        /// List the logbooks by the given filters
        /// </summary>
        /// <param name="logbookEntity">Entity</param>
        /// <param name="trainingCenterCode">Training center code</param>
        /// <param name="user">User</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>   
        /// <returns>The logbooks meeting the given filters and page config</returns>
        public PageHelper<LogbookEntity> ListByFilters(LogbookEntity logbookEntity, string trainingCenterCode, string user, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<LogbookEntity> pageHelper = LogbooksDal.ListByFilters(logbookEntity
                    , trainingCenterCode
                    , user
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
        /// List the logbooks History by the given filters
        /// </summary>
        /// <param name="logbookEntity">Entity</param>
        /// <param name="trainingCenterCode">Training center code</param>
        /// <param name="user">User</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>   
        /// <returns>The logbooks meeting the given filters and page config</returns>
        public PageHelper<LogbookEntity> ListHistoryByFilters(LogbookEntity logbookEntity, string trainingCenterCode, string user, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<LogbookEntity> pageHelper = LogbooksDal.ListHistoryByFilters(logbookEntity, trainingCenterCode, user,
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
        /// List a logbook by its key
        /// </summary>
        /// <param name="logbookNumber">LogbookNumber</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The logbook and its participants</returns>
        public LogbookEntity ListByKey(int logbookNumber, string geographicDivisionCode, int? DivisionCode = null)
        {
            try
            {
                return LogbooksDal.ListByKey(logbookNumber, geographicDivisionCode, DivisionCode);
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
        /// List a logbook by its key
        /// </summary>
        /// <param name="logbookNumber">LogbookNumber</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The logbook and its participants</returns>
        public LogbookEntity ListHistoryByKey(int logbookNumber, string geographicDivisionCode, int? DivisionCode = null)
        {
            try
            {
                return LogbooksDal.ListHistoryByKey(logbookNumber, geographicDivisionCode, DivisionCode);
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
        /// List the draft logbooks
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The logbooks meeting the given filters and page config</returns>
        public List<LogbookEntity> ListByDraft(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return LogbooksDal.ListByDraft(divisionCode, geographicDivisionCode);
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
        /// List the logbooks by the validation filters, to verify does not match the course, start date, end date, classroom, instructor and students of another logbook
        /// </summary>
        /// <param name="entity">The logbook</param>
        /// <param name="participants">Participants</param>
        /// <returns>The logbook and its participants</returns>
        public LogbookEntity LogbooksListByValidationFilters(LogbookEntity entity, DataTable dtParticipants)
        {
            try
            {
                return LogbooksDal.LogbooksListByValidationFilters(entity, dtParticipants);
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
        /// Add or update the logbook
        /// </summary>
        /// <param name="entity">The logbook</param>
        /// <param name="participants">Participants</param>
        /// <returns>Logbook number</returns>
        public DbaEntity AddOrUpdate(LogbookEntity entity, DataTable participants)
        {
            try
            {                
                return LogbooksDal.AddOrUpdate(entity, participants);                
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
        /// <param name="logbookNumber">Logbook Number</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        public void Delete(int logbookNumber, string geographicDivisionCode, int divisionCode)
        {
            try
            {
                LogbooksDal.Delete(logbookNumber, geographicDivisionCode, divisionCode);
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
