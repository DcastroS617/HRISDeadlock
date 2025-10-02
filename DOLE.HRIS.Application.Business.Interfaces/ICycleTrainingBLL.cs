using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ICycleTrainingBll
    {
        /// <summary>
        /// List the cycle training by the given filters
        /// </summary>
        /// <param name="CycleTraining">The cycle training</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>The cycle training meeting the given filters and page config</returns>
        PageHelper<CycleTrainingEntity> CycleTrainingListByFilter(CycleTrainingEntity CycleTraining, int DivisionCode, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// List the cycle training by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="entity">The cycle training</param>
        CycleTrainingEntity CycleTrainingByKey(CycleTrainingEntity CycleTraining);

        /// <summary>
        /// Add the cycle training
        /// </summary>
        /// <param name="entity">The cycle training</param>
        CycleTrainingEntity CycleTrainingAdd(CycleTrainingEntity CycleTraining);

        /// <summary>
        /// Edit the cycle training
        /// </summary>
        /// <param name="entity">The cycle training</param>
        CycleTrainingEntity CycleTrainingEdit(CycleTrainingEntity CycleTraining);

        /// <summary>
        /// List the cycle training by catalog
        /// </summary>
        /// <returns></returns>
        ListItem[] CycleTrainingListByCatalog(CycleTrainingEntity CycleTraining);

        /// <summary>
        /// List the cycle tranning by division key: Division an d GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        List<CycleTrainingEntity> CycleTrainingListByByMasterProgramByCourse(int divisionCode, string geographicDivisionCode);
    }
}
