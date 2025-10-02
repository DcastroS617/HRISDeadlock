using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ITrainingPlanProgramsDal<T> where T : TrainingPlanProgramEntity
    {
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
        /// List the training Plan Programs by the given filters
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
        /// <returns>The training Plan Programs meeting the given filters and page config</returns>
        PageHelper<T> ListByFilters(int divisionCode, string geographicDivisionCode, string trainingPlanProgramCode, string trainingPlanProgramName, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue);

        /// <summary>
        /// List the training program by code
        /// </summary>
        /// <param name="entity">The training program code</param>
        /// <returns>The training program</returns>
        T ListByCode(T entity);

        /// <summary>
        /// List the training Plan Programs key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The training Plan Programs meeting the given filters</returns>
        ListItem[] TrainingPlanProgramsList(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// Activate the training program
        /// </summary>
        /// <param name="entity">The training program</param>
        void Activate(T entity);

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