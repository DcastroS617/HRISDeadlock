using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Exceptions.Messages.Messages;
using static DOLE.HRIS.Exceptions.SqlExceptionAnalyzer;

namespace DOLE.HRIS.Application.DataAccess
{
    public class EmployeeByLaborDal : IEmployeeByLaborDal
    {
        /// <summary>
        /// List the employe by labor by the given filters
        /// </summary>
        /// <param name="entity">The employe by labor</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        public List<EmployeeByLaborEntity> EmployeeByLaborWithFilter(EmployeeByLaborEntity entity, string SortExpression, string SortDirection)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.EmployeeByLaborWithFilter", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@EmployeeCode",entity.EmployeeCode),
                    new SqlParameter("@EmployeeName",entity.EmployeeName),
                    new SqlParameter("@CompanyCode",entity.CompanyCode),
                    new SqlParameter("@PayrollClassCode",entity.PayrollClassCode),
                    new SqlParameter("@CostsCenterCode",entity.CostsCenterCode),
                    new SqlParameter("@PositionCode",entity.PositionCode),
                    new SqlParameter("@RecruitmentDATE",entity.RecruitmentDATE),
                    new SqlParameter("@SortExpression",SortExpression),
                    new SqlParameter("@SortDirection",SortDirection),
                }, 360);

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeByLaborEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    DivisionCode = r.Field<int>("DivisionCode"),
                    DivisionName = r.Field<string>("DivisionName"),
                    EmployeeName = r.Field<string>("EmployeeName"),

                    CompanyCode = r.Field<string>("CompanyCode"),
                    PayrollClassCode = r.Field<string>("PayrollClassCode"),

                    PositionCode = r.Field<string>("PositionCode"),
                    PositionName = r.Field<string>("PositionName"),

                    CostsCenterCode = r.Field<string>("CostsCenterCode"),
                    CostsCenterName = r.Field<string>("CostCenterName"),

                    LaborId1 = r.Field<int?>("LaborId1"),
                    Labor1 = new LaborEntity { LaborCode = r.Field<string>("LaborCode1"), LaborName = r.Field<string>("LaborName1") },

                    LaborId2 = r.Field<int?>("LaborId2"),
                    Labor2 = new LaborEntity { LaborCode = r.Field<string>("LaborCode2"), LaborName = r.Field<string>("LaborName2") },

                    LaborId3 = r.Field<int?>("LaborId3"),
                    Labor3 = new LaborEntity { LaborCode = r.Field<string>("LaborCode3"), LaborName = r.Field<string>("LaborName3") },
                }).ToList();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// Add the employee by labor
        /// </summary>
        /// <param name="entity">The employee by labor</param>
        /// <param name="EMPLOYEES">The employee</param>
        public DbaEntity EmployeeByLaborAdd(EmployeeByLaborEntity entity, DataTable EMPLOYEES)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Dole.EmployeeByLaborAdd", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@EmployeeList", EMPLOYEES),
                    new SqlParameter("@Level", entity.Level),
                    new SqlParameter("@LaborId1", entity.LaborId1),
                    new SqlParameter("@LaborId2", entity.LaborId2),
                    new SqlParameter("@LaborId3", entity.LaborId3),
                });

                return new DbaEntity { ErrorNumber = ds.Item1, ErrorMessage = ds.Item2 };
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// Delete the employee by labor
        /// </summary>
        /// <param name="entity">The employee by labor</param>
        public DbaEntity EmployeeByLaborDelete(EmployeeByLaborEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Dole.EmployeeByLaborDelete", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@EmployeeCode",entity.EmployeeCode),
                });

                return new DbaEntity { ErrorNumber = ds.Item1, ErrorMessage = ds.Item2 };
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// List the list item by key: DivisionCode
        /// </summary>
        /// <param name="DivisionCode"></param>
        /// <returns></returns>
        public ListItem[] CompaniesListEnableByDivision(int DivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.CompaniesListEnableByDivision", new SqlParameter[] {
                    new SqlParameter("@DivisionCode", DivisionCode)
                }).Tables[0];
                
                var result = ds.AsEnumerable().Select(r => new ListItem()
                {
                    Value = r.Field<int>("CompanyID").ToString(),
                    Text = r.Field<string>("CompanyName").ToString(),
                }).ToArray();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// List the list item by key: GeographicDivisionID and CompanyID
        /// </summary>
        /// <param name="GeographicDivisionID"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        public ListItem[] NominalClassListEnableByDivision(string GeographicDivisionID, int? CompanyID)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.NominalClassListEnableByCompany", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionID",GeographicDivisionID),
                    new SqlParameter("@CompanyID",CompanyID)
                });
                
                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem()
                {
                    Value = r.Field<string>("NominalClassId").ToString(),
                    Text = r.Field<string>("NominalClassName").ToString(),
                }).ToArray();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// List the list item by key: GeographicDivisionCode, CompanyID and PayrollClassCode
        /// </summary>
        /// <param name="GeographicDivisionCode"></param>
        /// <param name="CompanyID"></param>
        /// <param name="PayrollClassCode"></param>
        /// <returns></returns>
        public ListItem[] CostCenterListEnableByDivision(string GeographicDivisionCode, int? CompanyID, string PayrollClassCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.CostCenterListEnableByDivision", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",GeographicDivisionCode),
                    new SqlParameter("@CompanyID",CompanyID),
                    new SqlParameter("@PayrollClassCode",PayrollClassCode),
                });
                
                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem()
                {
                    Value = r.Field<string>("CostCenterID").ToString(),
                    Text = r.Field<string>("CostCenterName").ToString(),
                }).ToArray();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }

        /// <summary>
        /// List the list item by key: CompanyCode and PayrollClassCode
        /// </summary>
        /// <param name="CompanyCode"></param>
        /// <param name="PayrollClassCode"></param>
        /// <returns></returns>
        public ListItem[] PositionsListEnabled(int? CompanyCode, string PayrollClassCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Dole.PositionsListByCompaniePayrollClass", new SqlParameter[] {
                    new SqlParameter("@CompanyCode",CompanyCode),
                    new SqlParameter("@PayrollClassCode",PayrollClassCode),
                });
                             
                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem()
                {
                    Value = r.Field<string>("PositionCode").ToString(),
                    Text = r.Field<string>("PositionCode").ToString() + " - " + r.Field<string>("PositionName").ToString(),
                }).ToArray();

                return result;
            }

            catch (SqlException sqlex)
            {
                throw new DataAccessException(AnalyzeException(sqlex, msjGeneralParameters), sqlex);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException) throw;
                else
                {
                    throw new DataAccessException(msjGeneralParametersEditParameterByDivisionAndModule, ex);
                }
            }
        }
    }
}
