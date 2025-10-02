using System;
using System.Configuration;
using System.Linq;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Exceptions.Messages.Messages;

namespace DOLE.HRIS.Application.Business
{
    /// <summary>
    /// Business logic layer for Initiative Goals.
    /// </summary>
    public class InitiativeGoalsBll : IInitiativeGoalsBLL
    {
        private readonly IInitiativeGoalsDAL InitiativeGoalsDal;

        /// <summary>
        /// Constructor for InitiativeGoalsBll.
        /// </summary>
        /// <param name="objDal">Data access object for Initiative Goals.</param>
        public InitiativeGoalsBll(IInitiativeGoalsDAL objDal)
        {
            InitiativeGoalsDal = objDal;
        }

        /// <summary>
        /// Lists initiative goals based on provided filters.
        /// </summary>
        /// <param name="initiativeCode">Initiative code.</param>
        /// <param name="sortExpression">Sort expression.</param>
        /// <param name="sortDirection">Sort direction.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <returns>PageHelper containing filtered initiative goals.</returns>
        public PageHelper<InitiativeGoalEntity> ListByFilters(
            long initiativeCode,
            string sortExpression,
            string sortDirection,
            int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                PageHelper<InitiativeGoalEntity> pageHelper = InitiativeGoalsDal.ListByFilters(
                    initiativeCode, sortExpression, sortDirection, pageNumber.Value, null, pageSizeValue);

                pageHelper.TotalPages = (pageHelper.TotalResults - 1) / pageHelper.PageSize + 1;

                return pageHelper;
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                else
                {
                    throw new BusinessException(msjGeneralParametersList, ex);
                }
            }
        }
    }
}

