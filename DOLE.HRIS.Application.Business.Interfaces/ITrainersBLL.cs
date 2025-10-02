using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ITrainersBll<T> where T : TrainerEntity
    {
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
        PageHelper<TrainerEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string trainerCode, string trainerName, string trainerType, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// List the trainer by key: GeographicDivisionCode, Type and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="trainerType">Trainer type</param>
        /// <param name="trainerCode">Trainer code</param>
        /// <returns>The trainer</returns>
        T ListByKey(string geographicDivisionCode, int divisionCode, TrainerType? trainerType,  string trainerCode);

        /// <summary>
        /// List the trainers by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The trainers meeting the given filters</returns>
        List<T> ListByDivision(int divisionCode, string geographicDivisionCode);
        
        /// <summary>
        /// List the trainers used in logbooks by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The trainers meeting the given filters</returns>
        List<T> ListByDivisionUsedByLogbooks(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the trainers used in logbooks history by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The trainers meeting the given filters</returns>
        List<T> ListByDivisionUsedByLogbooksHistory(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the trainers by course: GeographicDivisionCode and CourseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode"Course code</param>
        /// <returns>The trainers meeting the given filters</returns>
        List<T> ListByCourse(string geographicDivisionCode, int divisionCode, string courseCode, bool? isForce = null);

        /// <summary>
        /// Add the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>
        /// <returns>Tuple: En the first item a bool: true if trainer successfully added. False otherwise
        /// Second item: the trainer added if true was return in first item. Existing class by code if false.</returns>
        Tuple<bool, TrainerEntity> Add(TrainerEntity entity);

        /// <summary>
        /// Edit the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>        
        void Edit(T entity);

        /// <summary>
        /// Delete the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the trainer
        /// </summary>
        /// <param name="entity">The trainer</param>
        void Activate(T entity);

        /// <summary>
        /// List the trainers that meet the filters and is related to a logbook
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="user">User</param>
        /// <returns>The trainers meeting the given filters</returns>
        List<T> ListByLogbook(int divisionCode, string geographicDivisionCode, string user);
    }
}