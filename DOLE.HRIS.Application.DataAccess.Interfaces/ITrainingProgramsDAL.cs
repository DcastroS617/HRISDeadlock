using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ITrainingProgramsDal<T> where T : TrainingProgramEntity
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
        /// <param name="trainingProgram">the trainingProgram</param>
        void AddCourseByTrainingProgram(CourseEntity entity, TrainingProgramEntity trainingProgram);

        /// <summary>
        /// Delete the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainingProgram">the trainingProgram</param>
        void DeleteCourseByTrainingProgram(CourseEntity entity, TrainingProgramEntity trainingProgram);

        /// <summary>
        /// List the training programs key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training programs meeting the given filters</returns>
        ListItem[] TrainingProgramsList(int divisionCode, string geographicDivisionCode);

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
        PageHelper<T> ListByFilters(int divisionCode, string geographicDivisionCode, string trainingProgramCode, string trainingProgramName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);
    }
}