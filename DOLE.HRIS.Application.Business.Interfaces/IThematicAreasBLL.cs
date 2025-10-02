using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IThematicAreasBll<T> where T : ThematicAreaEntity
    {        
        /// <summary>
        /// List the thematic area by code
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="thematicAreaCode">The thematic area code</param>
        /// <returns>The thematic area</returns>
        T ListByCode(string geographicDivisionCode, int DivisionCode, string thematicAreaCode);

        /// <summary>
        /// List the thematic areas by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The thematic areas meeting the given filters</returns>
        List<T> ListByDivision(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the thematic areas by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The thematic areas meeting the given filters</returns>
        List<T> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode);

        /// <summary>
        /// Add the relation between the course and the thematic area
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        void AddCourseByThematicArea(CourseEntity entity, ThematicAreaEntity thematicArea);

        /// <summary>
        /// Delete the relation between the course and the thematic area
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        void DeleteCourseByThematicArea(CourseEntity entity, ThematicAreaEntity thematicArea);

        /// <summary>
        /// Add the thematic area
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="thematicAreaCode">The thematic area code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="thematicAreaName">The thematic area name</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        Tuple<bool, T> Add(string geographicDivisionCode, string thematicAreaCode, int divisionCode, string thematicAreaName, bool searchEnabled, string lastModifiedUser);
       
        /// <summary>
        /// Edit the thematic area
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="thematicAreaCode">The thematic area code</param>
        /// <param name="thematicAreaName">The thematic area name</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="deleted">Deleted ?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        Tuple<bool, ThematicAreaEntity> Edit(string geographicDivisionCode,int divisionCode, string thematicAreaCode, string thematicAreaName, bool searchEnabled, bool deleted, string lastModifiedUser);
        
        /// <summary>
        /// Delete the thematic area
        /// </summary>     
        /// <param name="thematicAreaCode">The thematic area code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        void Delete(string geographicDivisionCode, int divisionCode, string thematicAreaCode, string lastModifiedUser);
        
        /// <summary>
        /// List the thematic areas by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="thematicAreaCode">The thematic area Code</param>
        /// <param name="thematicAreaName">The thematic area name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The thematic areas meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string geographicDivisionCode, string thematicAreaCode, string thematicAreaName, string sortExpression, string sortDirection, int pageNumber);

        /// <summary>
        /// Activate the thematic area
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="thematicAreaCode">The thematic area code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        void Activate(string geographicDivisionCode, int divisionCode, string thematicAreaCode, string lastModifiedUser);

        List<ThematicAreaEntity> ThematicAreasByCourseAssociated(string GeographicDivisionCode, int DivisionCode, string CourseCode);
        
        List<ThematicAreaEntity> ThematicAreasByCourseNotAssociated(string GeographicDivisionCode, int DivisionCode, string CourseCode);
    }
}