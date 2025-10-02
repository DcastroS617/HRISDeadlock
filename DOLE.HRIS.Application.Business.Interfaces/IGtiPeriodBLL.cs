using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IGtiPeriodBLL
    {
        /// <summary>
        /// List the GTI period campaign by the given filters
        /// </summary>
        /// <param name="periodCampaignEntity">Entity</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>   
        /// <param name="pageSize">Page size</param>
        /// <returns>The period meeting the given filters and page config</returns>
        PageHelper<PeriodCampaignEntity> ListGtiPeriodByFilters(PeriodCampaignEntity periodCampaignEntity, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// List a logbook by its key
        /// </summary>
        /// <param name="PeriodCampaignId">LogbookNumber</param>
        /// <returns>The GTI period Campign</returns>
        PeriodCampaignEntity ListByKey(int PeriodCampaignId);

        /// <summary>
        /// Add or update the  GTI period Campign
        /// </summary>
        /// <param name="entity">The logbook</param>
        /// <returns>Gti Period number</returns>
        PeriodCampaignEntity AddOrUpdate(PeriodCampaignEntity periodCampaignEntity);

        ListItem[] MasterParameterList();

        /// <summary>
        /// Delete the GTI period Campign
        /// </summary>
        /// <param name="entity">The logbook</param>
        void Delete(PeriodCampaignEntity periodCampaignEntity);

        /// <summary>
        /// List the Quarter Period enabled
        /// </summary>
        /// <returns>The Quarter Periods</returns>
        List<QuarterPeriodEntity> ListQuarterPeriodActive();

        /// <summary>
        /// List the Quarter Year enabled
        /// </summary>
        /// <returns>The Quarter Years</returns>
        List<QuarterYearEntity> ListQuarterYearActive();
    }
}
