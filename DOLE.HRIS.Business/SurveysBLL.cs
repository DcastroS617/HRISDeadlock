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
    public class SurveysBll : ISurveysBll<SurveyEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly ISurveysDal<SurveyEntity> SurveysDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public SurveysBll(ISurveysDal<SurveyEntity> objDal)
        {
            SurveysDal = objDal;
        }

        /// <summary>
        /// Add a new survey "header" for an employee
        /// </summary>
        /// <param name="entity">The survey header entity for the empoyee</param>
        /// <returns>The survey code</returns>
        public long Add(SurveyEntity entity)
        {
            try
            {
                return SurveysDal.Add(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ExceptionSurveyAdd, ex);
                }
            }
        }
       
        /// <summary>
        /// Edit a survey "header" for an employee
        /// </summary>
        /// <param name="entity">The survey header for the employee</param>
        public void Edit(SurveyEntity entity)
        {
            try
            {
                SurveysDal.Edit(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ExceptionSurveyEdit, ex);
                }
            }
        }
       
        /// <summary>
        /// List the last survey for the employee
        /// </summary>
        /// <param name="employeeCode">The empoyee code</param>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <returns>The last survey for the employee</returns>
        public SurveyEntity ListLastByEmployeeCodeGeographicDivision(string employeeCode, string geographicDivisionCode, int surveyVersion)
        {
            try
            {
                return SurveysDal.ListLastByEmployeeCodeGeographicDivision(new SurveyEntity(employeeCode, geographicDivisionCode) { SurveyVersion = surveyVersion });
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionSurveyList, ex);
                }
            }
        }
        
        /// <summary>
        /// Save the current state for the employee survey
        /// </summary>
        /// <param name="surveyCode">The survey code</param>
        /// <param name="surveyStateCode">The survey state code</param>
        /// <param name="surveyCurrentPageCode">The survey current page code</param>
        /// <param name="surveyCompletedBy">The survey completed by</param>
        /// <param name="surveyEndDateTime">The survey end date and time</param>
        /// <param name="lastModifiedUser">The last modified user</param>
        public void SaveCurrentState(long surveyCode, byte surveyStateCode, byte surveyCurrentPageCode, string surveyCompletedBy, DateTime? surveyEndDateTime, string lastModifiedUser, int surveyVersion)
        {
            try
            {
                SurveysDal.SaveCurrentState(new SurveyEntity(surveyCode, 
                    surveyStateCode, 
                    surveyCurrentPageCode, 
                    surveyCompletedBy, 
                    surveyEndDateTime, 
                    lastModifiedUser){ SurveyVersion = surveyVersion});
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ExceptionSurveyEditState, ex);
                }
            }
        }
       
        /// <summary>
        /// List the completed and pending sinchronization surveys by page
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        /// <param name="pageNumber">THe page number</param>
        /// <returns>The surveys</returns>
        public PageHelper<SurveyEntity> ListPendingSynchronizationByPage(int divisionCode, int pageNumber)
        {
            try
            {
                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<SurveyEntity>  response = SurveysDal.ListPendingSynchronizationByPage(divisionCode, 
                    pageNumber, pageSizeValue);

                response.TotalPages = (response.TotalResults - 1) / response.PageSize + 1;

                return response;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionSurveyList, ex);
                }
            }
        }
        
        /// <summary>
        /// Add or update a survey "header" for an employee
        /// </summary>
        /// <param name="entity">The survey</param>
        public void Save(SurveyEntity entity)
        {
            try
            {
                SurveysDal.Save(entity);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ExceptionSurveyAdd, ex);
                }
            }
        }
       
        /// <summary>
        /// List the completed and pending sinchronization surveys
        /// </summary>
        /// <returns>The pending synchronization surveys</returns>
        public List<SurveyEntity> ListPendingSynchronization(long? SurveyCode)
        {
            try
            {
                return SurveysDal.ListPendingSynchronization(SurveyCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionSurveyList, ex);
                }
            }
        }
        
        /// <summary>
        /// Sets the socio economic card as synchronized
        /// </summary>
        /// <param name="surveyCode">The survey code</param>
        public void SetSynchronized(long surveyCode)
        {            
            try
            {
                SurveysDal.SetSynchronized(surveyCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ExceptionSurveyEdit, ex);
                }
            }
        }

        /// <summary>
        /// Method to search an inactive employee
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="GeographicDivisionCode"></param>
        /// <returns>an INT value who indicates is inactive or active</returns>
        public int SurveyEmployeeInactive(string EmployeeCode, string GeographicDivisionCode)
        {
            try
            {
                return SurveysDal.SurveyEmployeeInactive(EmployeeCode,GeographicDivisionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ExceptionSurveyEdit, ex);
                }
            }
        }

        /// <summary>
        /// Method to  export surveys by division
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns>a list of diision entitys</returns>
        public List<DivisionEntity> RptCboSurveyExportDivision(int UserCode)
        {
            try
            {
                return SurveysDal.RptCboSurveyExportDivision(UserCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionSurveyList, ex);
                }
            }
        }

        /// <summary>
        /// Method to  export surveys by company
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="UserCode"></param>
        /// <returns>A list of companies</returns>
        public List<Companies> RptCboSurveyExportCompany(int? divisionCode, int UserCode)
        {
            try
            {
                return SurveysDal.RptCboSurveyExportCompany(divisionCode, UserCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjExceptionSurveyList, ex);
                }
            }
        }

        /// <summary>
        /// Method to search and employee in the survey scope
        /// </summary>
        /// <param name="EmployeeCode"></param>
        /// <param name="GeographicDivisionCode"></param>
        /// <param name="divisionCode"></param>
        /// <returns> The current campaign whith the employee filter</returns>
        public SurveyEmployeeCampaign SurveyScopeEmployee(string EmployeeCode, string GeographicDivisionCode, int? divisionCode)
        {
            try
            {
                return SurveysDal.SurveyScopeEmployee(EmployeeCode, GeographicDivisionCode, divisionCode);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(ExceptionSurveyEdit, ex);
                }
            }
        }

    }
}