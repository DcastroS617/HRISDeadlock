using System;
using System.Configuration;
using System.Linq;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Shared.Entity;
using DOLE.HRIS.Exceptions;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using DOLE.HRIS.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business
{
    public class DeprivationManagementBLL : IDeprivationManagementBLL<DeprivationManagementEntity>
    {
        private readonly IDeprivationManagementDAL<DeprivationManagementEntity> _deprivationManagementDal;

        /// <summary>
        /// Constructor for DeprivationManagementBLL.
        /// </summary>
        /// <param name="objDal">Data access object for DeprivationManagement.</param>
        public DeprivationManagementBLL(IDeprivationManagementDAL<DeprivationManagementEntity> objDal)
        {
            _deprivationManagementDal = objDal;
        }

        /// <summary>
        /// List Deprivation Management records by filters.
        /// </summary>
        public PageHelper<DeprivationManagementEntity> ListByFilters(
            int? deprivationCode,
            string sortExpression,
            string sortDirection,
            int pageNumber)
        {
            try
            {
                if (pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                int pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
                PageHelper<DeprivationManagementEntity> pageHelper = _deprivationManagementDal.ListByFilters(
                    deprivationCode,
                    sortExpression,
                    sortDirection,
                    pageNumber,
                    null,
                    pageSizeValue);

                pageHelper.TotalPages = (pageHelper.TotalResults - 1) / pageHelper.PageSize + 1;
                return pageHelper;
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                throw new BusinessException(msjGeneralParametersList, ex);
            }
        }

        /// <summary>
        /// List Deprivation Management record by key.
        /// </summary>
        public DeprivationManagementEntity ListByKey(int deprivationManagementId)
        {
            try
            {
                return _deprivationManagementDal.ListByKey(deprivationManagementId);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                throw new BusinessException(msjGeneralParametersList, ex);
            }
        }


        /// <summary>
        /// Save a Deprivation Management record.
        /// </summary>
        public DbaEntity Save(DeprivationManagementEntity entity)
        {
            try
            {
                return _deprivationManagementDal.Save(entity);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                throw new BusinessException(msjGeneralParametersList, ex);
            }
        }

        /// <summary>
        /// Deactivate a Deprivation Management record.
        /// </summary>
        public DbaEntity Deactivate(DeprivationManagementEntity entity)
        {
            try
            {
                return _deprivationManagementDal.Deactivate(entity);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                throw new BusinessException(msjGeneralParametersList, ex);
            }
        }

        /// <summary>
        /// Close deprivation management.
        /// </summary>
        public DbaEntity CloseDeprivation(DeprivationManagementEntity entity)
        {
            try
            {
                return _deprivationManagementDal.CloseDeprivation(entity);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException) throw;
                throw new BusinessException(msjGeneralParametersList, ex);
            }
        }
    }
}
