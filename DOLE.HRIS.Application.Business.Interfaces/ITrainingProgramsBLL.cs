using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ITrainingProgramsBll<T> where T : TrainingProgramEntity
    {
        /// <summary>
        /// List the training Program by code
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingProgramCode">The training Program code</param>
        /// <param name="divisionCode">The division code</param>
        /// <returns>The training Program</returns>
        T ListByCode(string geographicDivisionCode, string trainingProgramCode, int divisionCode);

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
        /// <param name="divisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <returns>The training programs meeting the given filters</returns>
        List<T> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode);

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
        /// Add the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingProgramCode">The training Program code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="trainingProgramName">The training Program name</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        Tuple<bool, T> Add(string geographicDivisionCode, string trainingProgramCode, int divisionCode, string trainingProgramName, bool searchEnabled, string lastModifiedUser);

        /// <summary>
        /// Edit the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="trainingProgramCode">The training Program code</param>
        /// <param name="trainingProgramName">The training Program name</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="deleted">Deleted ?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        Tuple<bool, TrainingProgramEntity> Edit(string geographicDivisionCode, int divisionCode, string trainingProgramCode, string trainingProgramName, bool searchEnabled, bool deleted, string lastModifiedUser);

        /// <summary>
        /// Delete the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="trainingProgramCode">The training Program code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        void Delete(string geographicDivisionCode, int divisionCode, string trainingProgramCode, string lastModifiedUser);
        
        /// <summary>
        /// List the training Programs by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingProgramCode">The training Program Code</param>
        /// <param name="trainingProgramName">The training Program name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The training Programs meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string geographicDivisionCode, string trainingProgramCode, string trainingProgramName, string sortExpression, string sortDirection, int pageNumber);
        
        /// <summary>
        /// Activate the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingProgramCode">The training Program code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        void Activate(string geographicDivisionCode, string trainingProgramCode, string lastModifiedUser);
    }
}