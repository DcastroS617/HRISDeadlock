using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ITrainingPlanProgramsBll<T> where T : TrainingPlanProgramEntity
    {
        /// <summary>
        /// Add the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingPlanProgramCode">The training Program code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="trainingPlanProgramName">The training Program name</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        Tuple<bool, T> Add(string geographicDivisionCode, string trainingPlanProgramCode, int divisionCode, string trainingPlanProgramName, bool searchEnabled, string lastModifiedUser);

        /// <summary>
        /// Edit the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingPlanProgramCode">The training Program code</param>
        /// <param name="trainingPlanProgramName">The training Program name</param>
        /// <param name="searchEnabled">Search enabled?</param>
        /// <param name="deleted">Deleted ?</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        Tuple<bool, TrainingPlanProgramEntity> Edit(string geographicDivisionCode, string trainingPlanProgramCode, string trainingPlanProgramName, bool searchEnabled, bool deleted, string lastModifiedUser);

        /// <summary>
        /// Delete the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="trainingPlanProgramCode">The training Program code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        void Delete(string geographicDivisionCode, int divisionCode, string trainingPlanProgramCode, string lastModifiedUser);

        /// <summary>
        /// List the training Programs by the given filters
        /// </summary>
        /// <param name="divisionCode">The Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="trainingPlanProgramCode">The training Program Code</param>
        /// <param name="trainingPlanProgramName">The training Program name</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>The training Programs meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string geographicDivisionCode, string trainingPlanProgramCode, string trainingPlanProgramName, string sortExpression, string sortDirection, int pageNumber);

        /// <summary>
        /// List the training Program by code
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="divisionCode">The division code</param>
        /// <param name="trainingPlanProgramCode">The training Program code</param>
        /// <returns>The training Program</returns>
        T ListByCode(string geographicDivisionCode, int divisionCode, string trainingPlanProgramCode);

        /// <summary>
        /// List the training Plan Programs key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training Plan Programs meeting the given filters</returns>
        ListItem[] TrainingPlanProgramsList(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// Activate the training Program
        /// </summary>
        /// <param name="geographicDivisionCode">The geographic division code</param>
        /// <param name="trainingPlanProgramCode">The training Program code</param>
        /// <param name="lastModifiedUser">Last modified user</param>
        void Activate(string geographicDivisionCode, string trainingPlanProgramCode, string lastModifiedUser);

        /// <summary>
        /// Add the relation between the master program and the training Plan Programs
        /// </summary>
        /// <param name="entity">The master program</param>
        /// <param name="trainingPlanProgram">the trainingPlanProgram</param>
        void AddMasterProgramByTrainingPlanPrograms(MasterProgramEntity entity, TrainingPlanProgramEntity trainingPlanProgram);

        /// <summary>
        /// Delete the relation between the master program and the training Plan Programs
        /// </summary>
        /// <param name="entity">The master program</param>
        /// <param name="trainingPlanProgram">the trainingPlanProgram</param>
        void DeleteMasterProgramByTrainingPlanPrograms(MasterProgramEntity entity, TrainingPlanProgramEntity trainingPlanProgram);
    }
}