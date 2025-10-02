using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface IClassroomsDal<T> where T : ClassroomEntity
    {
        /// <summary>
        /// List the classrooms by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="classroomCode">Code</param>
        /// <param name="classroomDescription">Description</param>
        /// <param name="trainingCenterCode">Training center code</param>
        /// <param name="minCapacity">Min capacity</param>
        /// <param name="maxCapacity">Max capacity</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The classrooms meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string geographicDivisionCode, string classroomCode, string classroomDescription, string trainingCenterCode, int? minCapacity, int? maxCapacity, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// List the classroom by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The classrooms meeting the given filters</returns>
        List<T> ListByDivision(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the classroom by training center: GeographicDivisionCode and TraningCenterCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingCenterCode">Trianing center code</param>
        /// <returns>The classrooms meeting the given filters</returns>
        List<T> ListByTrainingCenter(string geographicDivisionCode, string trainingCenterCode, int divisionCode);

        /// <summary>
        /// List the classroom by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="classroomCode">Classroom code</param>
        /// <returns>The classroom</returns>
        T ListByKey(string geographicDivisionCode, string classroomCode, int DivisionCode, string ClassroomDescription = null);

        /// <summary>
        /// Add the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>
        void Add(T entity);

        /// <summary>
        /// Edit the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>
        void Delete(T entity);

        /// <summary>
        /// Delete the associated classroowm
        /// </summary>
        /// <param name="entity">The classroom</param>
        void DeleteAssociatedClassroom(T entity);

        /// <summary>
        /// Activate the classroom
        /// </summary>
        /// <param name="entity">The classroom</param>
        void Activate(T entity);
    }
}