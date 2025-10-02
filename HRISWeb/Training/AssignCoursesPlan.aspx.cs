using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Help;
using HRISWeb.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Services;
using System.Web.UI.WebControls;
using Unity.Attributes;

namespace HRISWeb.Training
{
    public partial class AssignCoursesPlan : System.Web.UI.Page
    {
        [Dependency]
        public IMasterProgramBll ObjIMasterProgramBll { get; set; }

        public ListItem[] ThematicAreasCodeFilterList { get; set; }

        public static ListItem[] PositionByMasterProgramList { get; set; }

        #region Events

        /// <summary>
        /// Sets the culture information
        /// </summary>
        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            if (Session[Constants.cCulture] != null)
            {
                CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
        }

        #endregion

        #region WebMethod

        /// <summary>
        /// Retrieves a list of master programs as ListItem objects.
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static ListItem[] ListMasterPrograms()
        {
            try
            {
                var DivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                var GeographicDivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

                var master = new MasterProgramEntity()
                {
                    DivisionCode = DivisionCodeGlobal,
                    GeographicDivisionCode = GeographicDivisionCodeGlobal
                };

                var page = new AssignCoursesPlan
                {
                    ObjIMasterProgramBll = new MasterProgramBll(new MasterProgramDal())
                };

                ListItem[] elementDefault = new ListItem[] { new ListItem { Value = "", Text = "" } };
                ListItem[] masterProgramList = page.ObjIMasterProgramBll.MasterProgramList(master);

                if (masterProgramList == null)
                {
                    masterProgramList = elementDefault;
                }
                else
                {
                    Array.Copy(elementDefault, masterProgramList, 0);
                }

                return masterProgramList;
            }

            catch (Exception)
            {
                return new ListItem[] { new ListItem { Value = "", Text = "" } };
            }
        }
        
        /// <summary>
        /// Retrieves a list of thematic areas associated with the current user's master programs based on their division and geographic codes.
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static ListItem[] ListThematicAreas()
        {
            try
            {
                var DivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                var GeographicDivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

                var master = new MasterProgramEntity()
                {
                    DivisionCode = DivisionCodeGlobal,
                    GeographicDivisionCode = GeographicDivisionCodeGlobal
                };

                var page = new AssignCoursesPlan
                {
                    ObjIMasterProgramBll = new MasterProgramBll(new MasterProgramDal())
                };

                return page.ObjIMasterProgramBll.MasterProgramThematicAreasListByCoursesExists(master);
            }

            catch (Exception)
            {
                return new ListItem[] { new ListItem { Value = "", Text = "" } };
            }
        } 

        /// <summary>
        /// Retrieves a list of courses based on the specified thematic area codes and the user's geographic division code.
        /// <param name="ThematicAreasCode">List of Thematic areas code sperated by comma</param>
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static List<CourseEntity> ListCoursesByMasterProgram(string ThematicAreasCode)
        {
            try
            {
                var GeographicDivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                var DivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                var page = new AssignCoursesPlan
                {
                    ObjIMasterProgramBll = new MasterProgramBll(new MasterProgramDal())
                };

                DataTable ThematicAreas = ThematicAreasCode.Split(',').Select(r => new TypeTableMultipleIdDto
                {
                    Code = r,
                    KeyValue1 = GeographicDivisionCodeGlobal
                }).ToList().ToDataTableGet();

                return page.ObjIMasterProgramBll.CoursesListByThematicArea(GeographicDivisionCodeGlobal,DivisionCodeGlobal, ThematicAreas);
            }

            catch (Exception)
            {
                return new List<CourseEntity>();
            }
        }

        /// <summary>
        /// Retrieves a list of labor entities associated with a specified master program ID. Used in planning and assigning labor resources in master programs.
        /// <param name="MasterProgramId">A specified master program ID</param>
        /// </summary>
        [WebMethod(EnableSession = true)]
        public static List<LaborEntity> ListLaborByMasterProgram(int? MasterProgramId)
        {
            try
            {
                var page = new AssignCoursesPlan
                {
                    ObjIMasterProgramBll = new MasterProgramBll(new MasterProgramDal())
                };

                return page.ObjIMasterProgramBll.MasterProgramByLaborByCode(new MasterProgramEntity() { MasterProgramId = MasterProgramId });
            }

            catch (Exception)
            {
                return new List<LaborEntity>();
            }
        }
       
        /// <summary>
        /// Retrieves a list of employees associated with a specific master program ID. This information is used for assigning employees to tasks within master programs.
        /// </summary>
        /// <param name="MasterProgramId">The master program ID used to fetch associated employees.</param>
        [WebMethod(EnableSession = true)]
        public static List<EmployeeEntity> ListEmployeeByMasterProgram(int? MasterProgramId)
        {
            try
            {
                var page = new AssignCoursesPlan
                {
                    ObjIMasterProgramBll = new MasterProgramBll(new MasterProgramDal())
                };

                return page.ObjIMasterProgramBll.MasterProgramByEmployeeList(new MasterProgramEntity() { MasterProgramId = MasterProgramId });
            }

            catch (Exception)
            {
                return new List<EmployeeEntity>();
            }
        }

        /// <summary>
        /// Retrieves a list of positions associated with a specified master program ID. This is used to manage position assignments within the scope of master programs.
        /// </summary>
        /// <param name="MasterProgramId">The master program ID used to fetch associated positions.</param>
        [WebMethod(EnableSession = true)]
        public static List<PositionEntity> ListPositionsByMasterProgram(int? MasterProgramId)
        {
            try
            {
                var page = new AssignCoursesPlan
                {
                    ObjIMasterProgramBll = new MasterProgramBll(new MasterProgramDal())
                };

                return page.ObjIMasterProgramBll.MasterProgramByPositions(new MasterProgramEntity() { MasterProgramId = MasterProgramId });
            }

            catch (Exception)
            {
                return new List<PositionEntity>();
            }
        }

        /// <summary>
        /// Validates the type of a master program by its ID. This method is used to confirm the classification type of the program for proper handling in the user interface.
        /// </summary>
        /// <param name="MasterProgramId">The master program ID to be validated for its type.</param>
        [WebMethod(EnableSession = true)]
        public static MasterProgramEntity MasterProgramValidationTypeSearch(int? MasterProgramId)
        {
            try
            {
                var page = new AssignCoursesPlan
                {
                    ObjIMasterProgramBll = new MasterProgramBll(new MasterProgramDal())
                };

                return page.ObjIMasterProgramBll.MasterProgramValidationTypeSearch(new MasterProgramEntity() { MasterProgramId = MasterProgramId });
            }

            catch (Exception)
            {
                return new MasterProgramEntity { };
            }
        }

        /// <summary>
        /// Filters and returns a matrix of master programs based on a complex set of criteria including thematic areas, courses, positions, labors, and employees.
        /// </summary>
        /// <param name="MasterProgramId">Master program ID to use in filtering.</param>
        /// <param name="ThematicAreasCode">Comma-separated thematic area codes for filtering.</param>
        /// <param name="CoursesCodes">Comma-separated course codes for filtering.</param>
        /// <param name="PositionCodes">Comma-separated position codes for filtering.</param>
        /// <param name="LaborCodes">Comma-separated labor codes for filtering.</param>
        /// <param name="EmployeeCodes">Comma-separated employee codes for filtering.</param>
        [WebMethod(EnableSession = true)]
        public static MatrixMasterProgramResultEntity FilterMatrix(int MasterProgramId, string ThematicAreasCode, string CoursesCodes, string PositionCodes, string LaborCodes, string EmployeeCodes)
        {
            try
            {
                var GeographicDivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

                var page = new AssignCoursesPlan
                {
                    ObjIMasterProgramBll = new MasterProgramBll(new MasterProgramDal())
                };

                var master = new MasterProgramEntity() { MasterProgramId = MasterProgramId };

                DataTable ThematicAreas = ThematicAreasCode.Split(',').Select(r => new TypeTableMultipleIdDto
                {
                    Code = r,
                    KeyValue1 = GeographicDivisionCodeGlobal
                }).ToList().ToDataTableGet();

                DataTable courses = CoursesCodes.Split(',').Select(r => new TypeTableMultipleIdDto
                {
                    Code = r,
                    KeyValue1 = GeographicDivisionCodeGlobal
                }).ToList().ToDataTableGet();

                #region PUESTOS

                DataTable positions = new DataTable();
                if (PositionCodes != "" && PositionCodes != null)
                {
                    positions = PositionCodes.Split(',').Select(r => new TypeTableMultipleIdDto
                    {
                        Code = r,
                    }).ToList().ToDataTableGet();
                }
                else
                {
                    positions.Columns.Add("Id");
                    positions.Columns.Add("code");
                    positions.Columns.Add("KeyValue1");
                    positions.Columns.Add("Selected");
                }

                #endregion

                #region LABORES

                DataTable labors = new DataTable();
                if (LaborCodes != "" && LaborCodes != null)
                {
                    labors = LaborCodes.Split(',').Select(r => new TypeTableMultipleIdDto
                    {
                        Code = r,
                    }).ToList().ToDataTableGet();
                }

                else
                {
                    labors.Columns.Add("Id");
                    labors.Columns.Add("code");
                    labors.Columns.Add("KeyValue1");
                    labors.Columns.Add("Selected");
                }

                #endregion

                #region EMPLEADOS

                DataTable employees = new DataTable();
                if (EmployeeCodes != "" && EmployeeCodes != null)
                {
                    employees = EmployeeCodes.Split(',').Select(r => new TypeTableMultipleIdDto
                    {
                        Code = r
                    }).ToList().ToDataTableGet();
                }

                else
                {
                    employees.Columns.Add("Id");
                    employees.Columns.Add("code");
                    employees.Columns.Add("KeyValue1");
                    employees.Columns.Add("Selected");
                }

                #endregion

                return page.ObjIMasterProgramBll.MasterProgramByCourseByFilter(master, ThematicAreas, courses, positions, labors, employees);
            }

            catch (Exception ex)
            {
                return new MatrixMasterProgramResultEntity { ErrorNumber = ex.HResult, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Saves the matrix of master programs with related course details to the database. 
        /// </summary>
        /// <param name="matrix">A list of lookup master program entities containing course details to be updated and saved.</param>
        /// <param name="courses">A list of courses, detailing their current state, used to update the matrix.</param>
        [WebMethod(EnableSession = true)]
        public static DbaEntity SaveMatrix(List<LookupMasterProgramEntity> matrix, List<CourseEntity> courses)
        {
            try
            {
                var DivisionCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                var GeographicCodeGlobal = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

                var page = new AssignCoursesPlan
                {
                    ObjIMasterProgramBll = new MasterProgramBll(new MasterProgramDal())
                };

                var masterProgramByCourses = new List<MasterProgramByCourseEntity>();
                matrix.ForEach(r => masterProgramByCourses.AddRange(r.Courses));

                foreach (var masterProgram in masterProgramByCourses)
                {
                    var cursesState = courses.Where(a => a.CourseCode == masterProgram.CourseCode).FirstOrDefault();

                    if (courses.Any(a => a.CourseCode == masterProgram.CourseCode))
                        masterProgram.IsChecked = cursesState.State;
                }

                var CoursesType = masterProgramByCourses.Select(r => new TypeMasterProgramByCourseEntity
                {
                    RelateBy = r.RelateBy,
                    GeographicDivisionCode = GeographicCodeGlobal,
                    DivisionCode = DivisionCodeGlobal,
                    MasterProgramId = r.MasterProgramId,
                    LaborId = r.LaborId,
                    PositionCode = r.PositionCode,
                    EmployeeCode = r.EmployeeCode,
                    CourseCode = r.CourseCode,
                    IsChecked = r.IsChecked
                }).ToList().ToDataTableGet();

                var result = page.ObjIMasterProgramBll.MasterProgramByCourseAdd(CoursesType);
                return result;
            }

            catch (Exception ex)
            {
                return new MatrixMasterProgramResultEntity { ErrorNumber = ex.HResult, ErrorMessage = ex.Message };
            }
        }

        #endregion
    }
}