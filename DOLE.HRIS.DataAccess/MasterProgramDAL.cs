using DOLE.HRIS.Application.DataAccess.Interfaces;
using DOLE.HRIS.Entity;
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
    public class MasterProgramDal : IMasterProgramDal
    {
        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public PageHelper<MasterProgramEntity> MasterProgramByFilter(MasterProgramEntity entity, string Lang, int Divisioncode, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramByFilter", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",Divisioncode),
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@MasterProgramCode",entity.MasterProgramCode),
                    new SqlParameter("@MasterProgramName",entity.MasterProgramName),
                    new SqlParameter("@MatrixTargetId",entity.MatrixTargetId),
                    new SqlParameter("@SortExpression",sortExpression),
                    new SqlParameter("@SortDirection",sortDirection),
                    new SqlParameter("@PageNumber",pageNumber),
                    new SqlParameter("@PageSize",pageSize),
                    new SqlParameter("@PageSizeValue", pageSizeValue)
                });

                var Page = ds.Tables[0].AsEnumerable().Select(r => new
                {
                    TotalResults = r.Field<int>("TotalResults"),
                    PageSize = r.Field<int>("PageSize")
                }).FirstOrDefault();


                var result = ds.Tables[1].AsEnumerable().Select(r => new MasterProgramEntity
                {
                    MasterProgramId = r.Field<long?>("MasterProgramId"),
                    MasterProgramCode = r.Field<string>("MasterProgramCode"),
                    MasterProgramName = r.Field<string>("MasterProgramName"),
                    MatrixTargetName = r.Field<string>("MatrixTargetName"),
                    IsExpiration = r.Field<bool>("IsExpiration"),
                    CycleTrainingId = r.Field<int?>("CycleTrainingId"),
                }).ToList();

                return new PageHelper<MasterProgramEntity>(result, Page.TotalResults, pageNumber, Page.PageSize);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public DbaEntity MasterProgramAdd(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.MasterProgramAdd", new SqlParameter[] {
                    new SqlParameter("@MasterProgramIdExisted", entity.MasterProgramIdExisted),
                    new SqlParameter("@MasterProgramCode", entity.MasterProgramCode),
                    new SqlParameter("@MasterProgramName", entity.MasterProgramName),
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@MatrixTargetId", entity.MatrixTargetId),
                    new SqlParameter("@TrainingPlanProgramCode", entity.TrainingPlanProgramCode),
                    new SqlParameter("@IsExpiration", entity.IsExpiration),
                    new SqlParameter("@ApplyRuleRecruitmentDate", entity.ApplyRuleRecruitmentDate),
                    new SqlParameter("@FromDate", entity.FromDate),
                    new SqlParameter("@ToDate", entity.ToDate),
                    new SqlParameter("@CycleTrainingId", entity.CycleTrainingId),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public MasterProgramEntity MasterProgramById(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramById", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MasterProgramEntity
                {
                    MasterProgramId = r.Field<long?>("MasterProgramId"),
                    MasterProgramIdExisted = r.Field<int>("MasterProgramIdExisted"),
                    MasterProgramCode = r.Field<string>("MasterProgramCode"),
                    MasterProgramName = r.Field<string>("MasterProgramName"),
                    MatrixTargetId = r.Field<int?>("MatrixTargetId"),
                    TrainingPlanProgramCode = r.Field<string>("TrainingPlanProgramCode"),
                    IsExpiration = r.Field<bool>("IsExpiration"),
                    ApplyRuleRecruitmentDate = r.Field<bool>("ApplyRuleRecruitmentDate"),
                    FromDate = r.Field<DateTime?>("FromDate"),
                    ToDate = r.Field<DateTime?>("ToDate"),
                    CycleTrainingId = r.Field<int?>("CycleTrainingId"),
                    Relatedby = r.Field<int?>("Relatedby"),
                    SearchEnabled = r.Field<bool>("SearchEnabled"),
                }).FirstOrDefault();

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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public DbaEntity MasterProgramEdit(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.MasterProgramEdit", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId", entity.MasterProgramId),
                    new SqlParameter("@MasterProgramCode", entity.MasterProgramCode),
                    new SqlParameter("@MasterProgramName", entity.MasterProgramName),
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@MatrixTargetId", entity.MatrixTargetId),
                    new SqlParameter("@TrainingPlanProgramCode", entity.TrainingPlanProgramCode),
                    new SqlParameter("@IsExpiration", entity.IsExpiration),
                    new SqlParameter("@ApplyRuleRecruitmentDate", entity.ApplyRuleRecruitmentDate),
                    new SqlParameter("@FromDate", entity.FromDate),
                    new SqlParameter("@ToDate", entity.ToDate),
                    new SqlParameter("@CycleTrainingId", entity.CycleTrainingId),
                    new SqlParameter("@SearchEnabled", entity.SearchEnabled),
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public DbaEntity MasterProgramDelete(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.MasterProgramDelete", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public DbaEntity MasterProgramIsExists(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.MasterProgramIsExists", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                    new SqlParameter("@MasterProgramCode", entity.MasterProgramCode),
                    new SqlParameter("@MasterProgramName", entity.MasterProgramName)
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public ListItem[] MasterProgramList(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramList", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode", entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode", entity.DivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<long>("MasterProgramId").ToString(),
                    Text = r.Field<string>("MasterProgramName"),
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
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public MasterProgramEntity MasterProgramValidationTypeSearch(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramValidationTypeSearch", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId)
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MasterProgramEntity
                {
                    Relatedby = r.Field<int>("Relatedby"),
                }).FirstOrDefault();

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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public DbaEntity MasterProgramRelationship(MasterProgramEntity entity, DataTable EmpleadosList, DataTable LaborList, DataTable PositionsList)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.MasterProgramRelationship", new SqlParameter[] {
                    new SqlParameter("@Relatedby",entity.Relatedby),
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                    new SqlParameter("@EmployeesList",EmpleadosList),
                    new SqlParameter("@LaborList",LaborList),
                    new SqlParameter("@PositionsList",PositionsList),
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public Tuple<MasterProgramEntity, List<string>, List<int>, List<string>> MasterProgramRelationshipById(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramRelationshipById", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MasterProgramEntity
                {
                    Relatedby = r.Field<int?>("Relatedby"),
                }).FirstOrDefault();

                var result2 = ds.Tables[1].AsEnumerable().Select(r => r.Field<string>("EmployeeCode")).ToList();
                var result3 = ds.Tables[2].AsEnumerable().Select(r => r.Field<int>("LaborId")).ToList();
                var result4 = ds.Tables[3].AsEnumerable().Select(r => r.Field<string>("PositionCode")).ToList();

                return new Tuple<MasterProgramEntity, List<string>, List<int>, List<string>>(result, result2, result3, result4);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public Tuple<int, List<EmployeeEntity>, List<PositionEntity>, List<LaborEntity>> MasterProgramRelatedSummary(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramRelatedSummary", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                });

                var relatedBy = ds.Tables[0].AsEnumerable().Select(r => r.Field<int?>("Relatedby")).FirstOrDefault() ?? 0;

                var empleados = ds.Tables[1].AsEnumerable().Select(r => new EmployeeEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                }).ToList();

                var Position = ds.Tables[2].AsEnumerable().Select(r => new PositionEntity
                {
                    PositionCode = r.Field<string>("PositionCode"),
                    PositionName = r.Field<string>("PositionName"),
                }).ToList();

                var Labor = ds.Tables[3].AsEnumerable().Select(r => new LaborEntity
                {
                    LaborCode = r.Field<string>("LaborCode"),
                    LaborName = r.Field<string>("LaborName"),
                }).ToList();

                return new Tuple<int, List<EmployeeEntity>, List<PositionEntity>, List<LaborEntity>>(relatedBy, empleados, Position, Labor);
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public List<EmployeeEntity> MasterProgramByEmployeeList(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramByEmployees", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new EmployeeEntity
                {
                    EmployeeCode = r.Field<string>("EmployeeCode").ToString(),
                    EmployeeName = r.Field<string>("EmployeeName"),
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public ListItem[] MasterProgramByEmployeesByPlacesOccupation(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramByEmployeesByPlacesOccupation", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<string>("EmployeeCode"),
                    Text = r.Field<string>("EmployeeName"),
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
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public ListItem[] MasterProgramByLaborById(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramByLaborById", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId", entity.MasterProgramId),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<int>("LaborId").ToString(),
                    Text = r.Field<string>("LaborName"),
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
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public List<LaborEntity> MasterProgramByLaborByCode(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramByLaborByCode", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new LaborEntity
                {
                    LaborCode = r.Field<string>("LaborCode").ToString(),
                    LaborName = r.Field<string>("LaborName"),
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public List<PositionEntity> MasterProgramByPositions(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramByPositions", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new PositionEntity
                {
                    PositionCode = r.Field<string>("PositionCode").ToString(),
                    PositionName = r.Field<string>("PositionName"),
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public ListItem[] MasterProgramByPositionsByPlacesOccupation(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramByPositionsByPlacesOccupation", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<string>("PositionCode"),
                    Text = r.Field<string>("PositionName"),
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
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public ListItem[] MatrixTargetList(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MatrixTargetList", new SqlParameter[] {
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<int>("MatrixTargetId").ToString(),
                    Text = r.Field<string>("MatrixTargetName"),
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
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public MatrixMasterProgramResultEntity MasterProgramByCourseByFilter(MasterProgramEntity entity, DataTable ThematicAreas, DataTable Courses, DataTable Positions, DataTable Labors, DataTable Employees)
        {
            try
            {
                MatrixMasterProgramResultEntity matrixMasterProgramResult = new MatrixMasterProgramResultEntity();

                var ds = Dal.QueryDataSet("Training.MasterProgramByCourseByFilter", new SqlParameter[] {
                    new SqlParameter("@MasterProgramId",entity.MasterProgramId),
                    new SqlParameter("@ThematicAreasList", ThematicAreas),
                    new SqlParameter("@CoursesList", Courses),
                    new SqlParameter("@PositionList", Positions),
                    new SqlParameter("@LaborList", Labors),
                    new SqlParameter("@EmployeeList", Employees),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new MasterProgramByCourseEntity
                {
                    MasterProgramId = r.Field<long?>("MasterProgramId"),
                    CompanyID = null,
                    NominalClassId = null,
                    PositionType = r.Field<int?>("PositionType"),
                    PositionCode = r.Field<string>("PositionCode"),
                    PositionName = r.Field<string>("PositionName"),
                    LaborId = r.Field<int?>("LaborId"),
                    LaborCode = r.Field<string>("LaborCode"),
                    LaborName = r.Field<string>("LaborName"),
                    EmployeeCode = r.Field<string>("EmployeeCode"),
                    EmployeeName = r.Field<string>("EmployeeName"),
                    MasterProgramName = r.Field<string>("MasterProgramName"),
                    IsExpiration = r.Field<bool>("IsExpiration"),
                    MatrixTargetName = r.Field<string>("MatrixTargetName"),
                    ThematicAreaName = r.Field<string>("ThematicAreaName"),
                    CourseCode = r.Field<string>("CourseCode"),
                    CourseName = r.Field<string>("CourseName"),
                    RelateBy = r.Field<int>("RelateBy"),
                    IsChecked = r.Field<bool>("IsChecked"),
                    IsCheckedAll = r.Field<bool>("IsCheckedAll"),
                }).ToList();

                if (result.Count > 0)
                {
                    MasterProgramByCourseEntity masterProgram = result.FirstOrDefault();

                    if (masterProgram.RelateBy == 1)
                    {
                        List<LookupMasterProgramEntity> matrix = result.ToLookup(R => new
                        {
                            R.MasterProgramId,
                            R.PositionCode,
                            R.PositionName,
                            R.LaborId
                        }).Select(R => new LookupMasterProgramEntity
                        {
                            Key = $"{R.Key.PositionCode}{R.Key.LaborId}",
                            Entity = R.FirstOrDefault(),
                            Courses = R.OrderBy(r => r.CourseName).ToList()
                        }).ToList();

                        List<MasterProgramByCourseEntity> courseEntities = result.GroupBy(r => r.CourseCode).Select(r => r.FirstOrDefault()).OrderBy(r => r.CourseName).ToList();

                        matrixMasterProgramResult = new MatrixMasterProgramResultEntity
                        {
                            Plan = masterProgram,
                            Matrix = matrix,
                            Colums = courseEntities
                        };
                    }

                    if (masterProgram.RelateBy == 2)
                    {
                        List<LookupMasterProgramEntity> matrix = result.ToLookup(R => new
                        {
                            R.MasterProgramId,
                            R.EmployeeCode
                        }).Select(R => new LookupMasterProgramEntity
                        {
                            Key = $"{R.Key.EmployeeCode}",
                            Entity = R.FirstOrDefault(),
                            Courses = R.OrderBy(r => r.CourseName).ToList()
                        }).ToList();

                        List<MasterProgramByCourseEntity> courseEntities = result.GroupBy(r => r.CourseCode).Select(r => r.FirstOrDefault()).OrderBy(r => r.CourseName).ToList();

                        matrixMasterProgramResult = new MatrixMasterProgramResultEntity
                        {
                            Plan = masterProgram,
                            Matrix = matrix,
                            Colums = courseEntities
                        };
                    }
                }

                return matrixMasterProgramResult;
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public List<CourseEntity> CoursesListByThematicArea(string GeographicDivisionCode, int DivisionCode ,DataTable ThematicAreaCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.CoursesListByThematicAreaEnabledMatrix", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",DivisionCode),
                    new SqlParameter("@ThematicAreaCodes",ThematicAreaCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new CourseEntity()
                {
                    CourseCode = r.Field<string>("CourseCode").ToString(),
                    CourseName = r.Field<string>("CourseCode").ToString() + " - " + r.Field<string>("CourseName"),
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public ListItem[] MasterProgramThematicAreasListByCoursesExists(MasterProgramEntity entity)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramByThematicAreasListByCoursesExists", new SqlParameter[] {
                    new SqlParameter("@GeographicDivisionCode",entity.GeographicDivisionCode),
                    new SqlParameter("@DivisionCode",entity.DivisionCode),
                });

                var result = ds.Tables[0].AsEnumerable().Select(r => new ListItem
                {
                    Value = r.Field<string>("ThematicAreaCode").ToString(),
                    Text = r.Field<string>("ThematicAreaName"),
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
                    throw new DataAccessException(msjGeneralParametersList, ex);
                }
            }
        }

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>     
        public DbaEntity MasterProgramByCourseAdd(DataTable Course)
        {
            try
            {
                var ds = Dal.TransactionScalarTuple("Training.MasterProgramByCourseAdd", new SqlParameter[] {
                    new SqlParameter("@Courses", Course),
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
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>        
        public List<MasterProgramEntity> MasterProgramByTrainingPlanProgramsAssociated(string GeographicDivisionCode, int divisionCode, string trainingPlanProgramCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramByTrainingPlanProgramsAssociated", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionCode", GeographicDivisionCode),
                     new SqlParameter("@DivisionCode", divisionCode),
                     new SqlParameter("@TrainingPlanProgramCode", trainingPlanProgramCode),
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new MasterProgramEntity
                {
                    MasterProgramCode = r.Field<string>("MasterProgramCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    MasterProgramName = r.Field<string>("MasterProgramName")
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
        /// List the master Program not assocated with a training programs key: GeographicDivisionCode
        /// </summary>        
        public List<MasterProgramEntity> MasterProgramByTrainingPlanProgramsNotAssociated(string GeographicDivisionCode)
        {
            try
            {
                var ds = Dal.QueryDataSet("Training.MasterProgramByTrainingPlanProgramsNotAssociated", new SqlParameter[] {
                     new SqlParameter("@GeographicDivisionCode", GeographicDivisionCode),
                }).Tables[0];

                var result = ds.AsEnumerable().Select(r => new MasterProgramEntity
                {
                    MasterProgramCode = r.Field<string>("MasterProgramCode"),
                    GeographicDivisionCode = r.Field<string>("GeographicDivisionCode"),
                    MasterProgramName = r.Field<string>("MasterProgramName")
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
    }
}
