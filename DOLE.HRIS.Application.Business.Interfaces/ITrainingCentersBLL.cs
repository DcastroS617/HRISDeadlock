using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ITrainingCentersBll<T> where T : TrainingCenterEntity
    {
        /// <summary>
        /// List the classrooms by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training centers meeting the given filter</returns>
        List<T> ListByDivision(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the training centers used in logbooks by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training centers meeting the given filter</returns>
        List<T> ListByDivisionUsedByLogbooks(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the training centers used in logbooks history by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training centers meeting the given filter</returns>
        List<T> ListByDivisionUsedByLogbooksHistory(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the training center by code
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingCenterCode">The training center code</param>
        /// <returns>The training center</returns>
        T ListByCode(string geographicDivisionCode, int divisionCode, string trainingCenterCode, string trainingCenterDes=null);

        /// <summary>
        /// Add the training center
        /// </summary>
        /// <param name="entity">The training center entity</param>
        TrainingCenterEntity Add(TrainingCenterEntity entity);

        /// <summary>
        /// Edit the training center
        /// </summary>
        /// <param name="entity">The training center entity</param>
        TrainingCenterEntity Edit(TrainingCenterEntity entity);

        /// <summary>
        /// Delete the training center
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingCenterCode">The training center code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        void Delete(string geographicDivisionCode, string trainingCenterCode, string lastModifiedUser);
       
        /// <summary>
        /// List the training Centers by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingCenterCode">The training center Code</param>
        /// <param name="trainingCenterDescription">The training center Description</param>
        /// <param name="placeLocation">The place location</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The training Centers meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string geographicDivisionCode, string trainingCenterCode, string trainingCenterDescription, string placeLocation, string sortExpression, string sortDirection, int pageNumber);
        
        /// <summary>
        /// Activate the training center
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingCenterCode">The training center code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        void Activate(string geographicDivisionCode, string trainingCenterCode, string lastModifiedUser);
 
        /// <summary>
        /// List the training center by description
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingCenterDescription">The training center description</param>
        /// <returns>The training center</returns>
        T ListByDescription(string geographicDivisionCode, string trainingCenterDescription);

        /// <summary>
        /// Method that validates if there is a training center with that description.
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingCenterCode">The training center code</param>
        /// <param name="trainingCenterDescription">The training center Description</param> 
        Tuple<bool, TrainingCenterEntity> ValidatedDescription(string geographicDivisionCode, string trainingCenterCode, string trainingCenterDescription, int DivisionCode, PlaceLocation placeLocation);
        
        /// <summary>
        /// List the Training Centers that meet the filters and is related to a logbook
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="user">User</param>
        /// <returns>The training centers meeting the given filter</returns>
        List<T> ListByLogbook(int divisionCode, string geographicDivisionCode, string user);

        /// <summary>
        /// List the division filter cb
        /// </summary>
        /// <param name="divisionCode"></param>
        /// <param name="geographicDivisionCode"></param>
        /// <returns></returns>
        List<TrainingCenterEntity> ListByDivisionFilterCB(int divisionCode, string geographicDivisionCode);
    }
}