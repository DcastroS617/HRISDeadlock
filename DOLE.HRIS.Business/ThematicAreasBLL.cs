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
    public class ThematicAreasBll : IThematicAreasBll<ThematicAreaEntity>
    {
        /// <summary>
        /// Data access object
        /// </summary>
        private readonly IThematicAreasDal<ThematicAreaEntity> ThematicAreasDal;

        /// <summary>
        /// Creates an instance of the class
        /// </summary>
        /// <param name="objDal">Data access object</param>
        public ThematicAreasBll(IThematicAreasDal<ThematicAreaEntity> objDal)
        {
            ThematicAreasDal = objDal;
        }

        /// <summary>
        /// Activate the thematic area
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="divisionCode">The geographic division code</param>
        /// <param name="thematicAreaCode">The thematic area code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public void Activate(string geographicDivisionCode, int divisionCode, string thematicAreaCode, string lastModifiedUser)
        {
            try
            {
                ThematicAreaEntity Program = new ThematicAreaEntity(geographicDivisionCode, thematicAreaCode)
                {
                    LastModifiedUser = lastModifiedUser,
                    DivisionCode = divisionCode
                };

                ThematicAreasDal.Activate(Program);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjThematicAreasActivate, ex);
                }
            }
        }

        /// <summary>
        /// Add the thematic area
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="thematicAreaCode">The thematic area code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="thematicAreaName">The thematic area name</param> 
        /// <param name="placeLocation">The thematic area Location</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public Tuple<bool, ThematicAreaEntity> Add(string geographicDivisionCode, string thematicAreaCode, int divisionCode, string thematicAreaName, bool searchEnabled, string lastModifiedUser)
        {
            try
            {
                ThematicAreaEntity previousThematicArea = ThematicAreasDal.ListByCode(
                    new ThematicAreaEntity(geographicDivisionCode, thematicAreaCode, thematicAreaName) { 
                        DivisionCode = divisionCode 
                    });

                if (previousThematicArea == null)
                {
                    ThematicAreaEntity entity = new ThematicAreaEntity(geographicDivisionCode,
                        thematicAreaCode,
                        divisionCode,
                        thematicAreaName,
                        searchEnabled,
                        lastModifiedUser);

                    ThematicAreasDal.Add(entity);

                    return new Tuple<bool, ThematicAreaEntity>(true, entity);
                }

                else
                {
                    return new Tuple<bool, ThematicAreaEntity>(false, previousThematicArea);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjThematicAreasAdd, ex);
                }
            }
        }

        /// <summary>
        /// Delete the thematic area
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="thematicAreaCode">The thematic area code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public void Delete(string geographicDivisionCode, int divisionCode, string thematicAreaCode, string lastModifiedUser)
        {
            try
            {
                ThematicAreaEntity Program = new ThematicAreaEntity(geographicDivisionCode, thematicAreaCode)
                {
                    LastModifiedUser = lastModifiedUser
                };
                Program.DivisionCode = divisionCode;

                ThematicAreasDal.Delete(Program);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjThematicAreasDelete, ex);
                }
            }
        }

        /// <summary>
        /// Edit the thematic area
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="thematicAreaCode">The thematic area code</param>
        /// <param name="thematicAreaName">The thematic area Name</param> 
        /// <param name="placeLocation">The thematic area Location</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="deleted">Deleted ?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        public Tuple<bool, ThematicAreaEntity> Edit(string geographicDivisionCode, int divisionCode, string thematicAreaCode, string thematicAreaName, bool searchEnabled, bool deleted, string lastModifiedUser)
        {
            try
            {
                ThematicAreaEntity previousThematicArea = ThematicAreasDal.ListByCode(new ThematicAreaEntity(geographicDivisionCode, "", thematicAreaName) { DivisionCode = divisionCode });

                if (previousThematicArea == null || previousThematicArea?.ThematicAreaCode == thematicAreaCode || previousThematicArea?.Deleted == false)
                {
                    ThematicAreasDal.Edit(new ThematicAreaEntity(geographicDivisionCode, 
                        thematicAreaCode, 
                        thematicAreaName, 
                        searchEnabled, 
                        deleted, 
                        lastModifiedUser)
                        { DivisionCode = divisionCode });

                    return new Tuple<bool, ThematicAreaEntity>(true, null);
                }

                else
                {
                    return new Tuple<bool, ThematicAreaEntity>(false, previousThematicArea);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjThematicAreasEdit, ex);
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
                ThematicAreasDal.AddCourseByThematicArea(entity, thematicArea);
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
                ThematicAreasDal.DeleteCourseByThematicArea(entity, thematicArea);
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
        /// List the thematic areas by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The thematic areas meeting the given filters</returns>
        public List<ThematicAreaEntity> ListByDivision(int divisionCode, string geographicDivisionCode)
        {
            try
            {
                return ThematicAreasDal.ListByDivision(divisionCode, geographicDivisionCode);
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
        /// List the thematic areas by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="courseCode"Course code</param>
        /// <returns>The thematic areas meeting the given filters</returns>
        public List<ThematicAreaEntity> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode)
        {
            try
            {
                return ThematicAreasDal.ListByCourse(geographicDivisionCode,divisionCode ,courseCode);
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
        /// List the thematic area by code
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="thematicAreaCode">The thematic area code</param>
        /// <returns>The thematic area</returns>
        public ThematicAreaEntity ListByCode(string geographicDivisionCode, int DivisionCode, string thematicAreaCode)
        {
            try
            {
                return ThematicAreasDal.ListByCode(new ThematicAreaEntity(geographicDivisionCode, thematicAreaCode)
                {
                    DivisionCode = DivisionCode
                });
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjThematicAreasEdit, ex);
                }
            }
        }

        /// <summary>
        /// List the thematic areas by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="thematicAreaCode">The thematic area Code</param>
        /// <param name="thematicAreaName">The thematic area Name</param>
        /// <param name="placeLocation">The place location</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The thematic areas meeting the given filters and page config</returns>
        public PageHelper<ThematicAreaEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string thematicAreaCode, string thematicAreaName, string sortExpression, string sortDirection, int pageNumber)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<ThematicAreaEntity> pageHelper = ThematicAreasDal.ListByFilters(divisionCode
                    , geographicDivisionCode
                    , thematicAreaCode
                    , thematicAreaName
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
                    throw new BusinessException(msjThematicAreasListByFilters, ex);
                }
            }
        }

        /// <summary>
        /// Add the relation between the course and the thematic area
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        public List<ThematicAreaEntity> ThematicAreasByCourseAssociated(string GeographicDivisionCode, int DivisionCode, string CourseCode)
        {
            try
            {
                return ThematicAreasDal.ThematicAreasByCourseAssociated(GeographicDivisionCode, DivisionCode, CourseCode);
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
        public List<ThematicAreaEntity> ThematicAreasByCourseNotAssociated(string GeographicDivisionCode, int DivisionCode, string CourseCode)
        {
            try
            {
                return ThematicAreasDal.ThematicAreasByCourseNotAssociated(GeographicDivisionCode, DivisionCode, CourseCode);
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