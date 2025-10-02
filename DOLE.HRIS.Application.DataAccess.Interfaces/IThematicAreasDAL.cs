using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IThematicAreasDal<T> where T : ThematicAreaEntity
    {                        
        /// <summary>
        /// Activate the training program
        /// </summary>
        /// <param name="entity">The training program</param>
        void Activate(T entity);

        /// <summary>
        /// Add the training program
        /// </summary>
        /// <param name="entity">The training program</param>
        void Add(T entity);

        /// <summary>
        /// Edit the training program
        /// </summary>
        /// <param name="entity">The training program</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the training program
        /// </summary>
        /// <param name="entity">The training program</param>
        void Delete(T entity);

        /// <summary>
        /// List the training program by code
        /// </summary>
        /// <param name="entity">The training program code</param>
        /// <returns>The training program</returns>
        T ListByCode(T entity);

        /// <summary>
        /// Add the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        void AddCourseByThematicArea(CourseEntity entity, ThematicAreaEntity thematicArea);

        /// <summary>
        /// Delete the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        void DeleteCourseByThematicArea(CourseEntity entity, ThematicAreaEntity thematicArea);

        /// <summary>
        /// List the training programs by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training programs meeting the given filters</returns>
        List<T> ListByDivision(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the training programs by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The training programs meeting the given filters</returns>
        List<T> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode);

        /// <summary>
        /// List the training programs by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingprogramCode">The training program Code</param>
        /// <param name="trainingprogramName">The training program name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The training programs meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string geographicDivisionCode, string thematicAreaCode, string thematicAreaName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GeographicDivisionCode"></param>
        /// <param name="CourseCode"></param>
        /// <returns></returns>
        List<ThematicAreaEntity> ThematicAreasByCourseAssociated(string GeographicDivisionCode, int DivisionCode, string CourseCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GeographicDivisionCode"></param>
        /// <param name="DivisionCode"></param>
        /// <param name="CourseCode"></param>
        /// <returns></returns>
        List<ThematicAreaEntity> ThematicAreasByCourseNotAssociated(string GeographicDivisionCode, int DivisionCode, string CourseCode);
    }
}