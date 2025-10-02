using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace DOLE.HRIS.Application.Business
{
    /// <summary>
    /// Business logic class for handling operations related to matrix targets.
    /// </summary>
    public class MatrixTargetBll : IMatrixTargetBll
    {
        /// <summary>
        /// Data access layer interface for matrix target operations.
        /// </summary>
        private readonly IMatrixTargetDal MatrixTargetDal;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixTargetBll"/> class with the specified data access layer.
        /// </summary>
        /// <param name="obj">The data access layer object implementing <see cref="IMatrixTargetDal"/>.</param>
        public MatrixTargetBll(IMatrixTargetDal obj)
        {
            MatrixTargetDal = obj;
        }

        /// <summary>
        /// Retrieves a paginated list of Matrix Targets based on filter criteria.
        /// </summary>
        /// <param name="entity">Filter parameters for the Matrix Target.</param>
        /// <param name="lang">Language code for localization.</param>
        /// <param name="divisionCode">Division code for filtering.</param>
        /// <param name="sortExpression">Column to sort by.</param>
        /// <param name="sortDirection">Sort direction (ASC/DESC).</param>
        /// <param name="pageNumber">Current page number.</param>
        /// <returns>A paginated list of <see cref="MatrixTargetEntity"/> records.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public PageHelper<MatrixTargetEntity> MatrixTargetByFilter(MatrixTargetEntity entity, string lang, int divisionCode, string sortExpression, string sortDirection, int? pageNumber)
        {
            try
            {
                if (!pageNumber.HasValue || pageNumber <= 0)
                {
                    pageNumber = 1;
                }

                var pageSizeValue = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

                var pageHelper = MatrixTargetDal.MatrixTargetByFilter(entity
                    , lang
                    , divisionCode
                    , sortExpression
                    , sortDirection
                    , pageNumber.Value
                    , null
                    , pageSizeValue);

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

        /// <summary>
        /// Retrieves detailed Matrix Target data by its identifier.
        /// </summary>
        /// <param name="entity">Matrix Target entity containing the ID to search for.</param>
        /// <returns>A tuple containing the Matrix Target entity and related data entities.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public Tuple<MatrixTargetEntity, List<MatrixTargetByDivisionsEntity>, List<MatrixTargetByCompaniesEntity>, List<MatrixTargetByCostZonesEntity>, List<MatrixTargetByCostMiniZonesEntity>, List<MatrixTargetByCostFarmsEntity>, List<MatrixTargetByNominalClassEntity>> MatrixTargetById(MatrixTargetEntity entity)
        {
            try
            {
                return MatrixTargetDal.MatrixTargetById(entity);
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

        /// <summary>
        /// Checks if the user has permission to access Matrix Target data at the regional level.
        /// </summary>
        /// <param name="entity">Matrix Target entity to verify.</param>
        /// <param name="UserCode">User identifier.</param>
        /// <returns>A <see cref="DbaEntity"/> indicating permission status.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public DbaEntity MatrixTargetRegionalPermit(MatrixTargetEntity entity, int UserCode)
        {
            try
            {
                return MatrixTargetDal.MatrixTargetRegionalPermit(entity, UserCode);
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

        /// <summary>
        /// Adds a new Matrix Target record along with associated cost structure data.
        /// </summary>
        /// <param name="entity">Main Matrix Target entity to add.</param>
        /// <param name="divisions">Associated divisions data.</param>
        /// <param name="costZones">Associated cost zones data.</param>
        /// <param name="costMiniZones">Associated cost mini-zones data.</param>
        /// <param name="costFarms">Associated cost farms data.</param>
        /// <param name="companies">Associated companies data.</param>
        /// <param name="nominalClass">Associated nominal classes data.</param>
        /// <returns>A <see cref="DbaEntity"/> with operation results.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public DbaEntity MatrixTargetAdd(MatrixTargetEntity entity, DataTable divisions, DataTable costZones, DataTable costMiniZones, DataTable costFarms, DataTable companies, DataTable nominalClass)
        {
            try
            {
                return MatrixTargetDal.MatrixTargetAdd(entity, divisions, costZones, costMiniZones, costFarms, companies, nominalClass);
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

        /// <summary>
        /// Updates an existing Matrix Target record and its associated cost structure data.
        /// </summary>
        /// <param name="entity">Matrix Target entity to update.</param>
        /// <param name="divisions">Associated divisions data.</param>
        /// <param name="costZones">Associated cost zones data.</param>
        /// <param name="costMiniZones">Associated cost mini-zones data.</param>
        /// <param name="costFarms">Associated cost farms data.</param>
        /// <param name="companies">Associated companies data.</param>
        /// <param name="nominalClass">Associated nominal classes data.</param>
        /// <returns>A <see cref="DbaEntity"/> with operation results.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public DbaEntity MatrixTargetEdit(MatrixTargetEntity entity, DataTable divisions, DataTable costZones, DataTable costMiniZones, DataTable costFarms, DataTable companies, DataTable nominalClass)
        {
            try
            {
                return MatrixTargetDal.MatrixTargetEdit(entity, divisions, costZones, costMiniZones, costFarms, companies, nominalClass);
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

        /// <summary>
        /// Deactivates a Matrix Target record.
        /// </summary>
        /// <param name="entity">Matrix Target entity to deactivate.</param>
        /// <returns>A <see cref="DbaEntity"/> with operation results.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public DbaEntity MatrixTargetDeactivate(MatrixTargetEntity entity)
        {
            try
            {
                return MatrixTargetDal.MatrixTargetDeactivate(entity);
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

        /// <summary>
        /// Retrieves a list of enabled cost zones for the specified divisions.
        /// </summary>
        /// <param name="divisions">DataTable containing divisions.</param>
        /// <returns>List of enabled <see cref="MatrixTargetByCostZonesEntity"/>.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public List<MatrixTargetByCostZonesEntity> CostZonesListEnableByDivisions(DataTable divisions)
        {
            try
            {
                return MatrixTargetDal.CostZonesListEnableByDivisions(divisions);
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

        /// <summary>
        /// Retrieves a list of enabled cost mini-zones for the specified division.
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code.</param>
        /// <param name="divisionCode">Division code.</param>
        /// <returns>List of enabled <see cref="MatrixTargetByCostMiniZonesEntity"/>.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public List<MatrixTargetByCostMiniZonesEntity> CostMiniZonesListEnableByDivision(string geographicDivisionCode, int divisionCode)
        {
            try
            {
                return MatrixTargetDal.CostMiniZonesListEnableByDivision(geographicDivisionCode, divisionCode);
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

        /// <summary>
        /// Retrieves a list of enabled cost mini-zones for multiple divisions.
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code.</param>
        /// <param name="divisions">DataTable containing divisions.</param>
        /// <returns>List of enabled <see cref="MatrixTargetByCostMiniZonesEntity"/>.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public List<MatrixTargetByCostMiniZonesEntity> CostMiniZonesListEnableByDivisions(string geographicDivisionCode, DataTable divisions)
        {
            try
            {
                return MatrixTargetDal.CostMiniZonesListEnableByDivisions(geographicDivisionCode, divisions);
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

        /// <summary>
        /// Retrieves a list of enabled cost farms for the specified division.
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code.</param>
        /// <returns>List of enabled <see cref="MatrixTargetByCostFarmsEntity"/>.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public List<MatrixTargetByCostFarmsEntity> CostFarmsListEnableByDivision(string geographicDivisionCode)
        {
            try
            {
                return MatrixTargetDal.CostFarmsListEnableByDivision(geographicDivisionCode);
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

        /// <summary>
        /// Retrieves a list of enabled cost farms for multiple divisions.
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code.</param>
        /// <param name="divisions">DataTable containing divisions.</param>
        /// <returns>List of enabled <see cref="MatrixTargetByCostFarmsEntity"/>.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public List<MatrixTargetByCostFarmsEntity> CostFarmsListEnableByDivisions(string geographicDivisionCode, DataTable divisions)
        {
            try
            {
                return MatrixTargetDal.CostFarmsListEnableByDivisions(geographicDivisionCode, divisions);
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

        /// <summary>
        /// Retrieves a list of enabled companies by division.
        /// </summary>
        /// <param name="divisions">DataTable containing divisions.</param>
        /// <returns>List of enabled <see cref="MatrixTargetByCompaniesEntity"/>.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public List<MatrixTargetByCompaniesEntity> CompaniesListEnableByDivision(DataTable divisions)
        {
            try
            {
                return MatrixTargetDal.CompaniesListEnableByDivision(divisions);
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

        /// <summary>
        /// Retrieves a list of enabled nominal classes for the specified company.
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code.</param>
        /// <returns>List of enabled <see cref="MatrixTargetByNominalClassEntity"/>.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public List<MatrixTargetByNominalClassEntity> NominalClassListEnabledByCompanie(string geographicDivisionCode)
        {
            try
            {
                return MatrixTargetDal.NominalClassListEnabledByCompanie(geographicDivisionCode);
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

        /// <summary>
        /// Retrieves a list of enabled nominal classes for multiple companies.
        /// </summary>
        /// <param name="divisions">DataTable containing company divisions.</param>
        /// <returns>List of enabled <see cref="MatrixTargetByNominalClassEntity"/>.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public List<MatrixTargetByNominalClassEntity> NominalClassListEnabledByCompanies(DataTable divisions)
        {
            try
            {
                return MatrixTargetDal.NominalClassListEnabledByCompanies(divisions);
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

        /// <summary>
        /// Retrieves a list of cost centers based on the specified geographic structure.
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code.</param>
        /// <returns>List of <see cref="MatrixTargetByCostCentresEntity"/>.</returns>
        /// <exception cref="BusinessException">Thrown when a processing error occurs.</exception>
        public List<MatrixTargetByCostCentresEntity> CostCentersListByStruct(string geographicDivisionCode)
        {
            try
            {
                return MatrixTargetDal.CostCentersListByStruct(geographicDivisionCode);
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
