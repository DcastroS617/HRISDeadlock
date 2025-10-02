using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface IMasterProgramBll
    {
        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        PageHelper<MasterProgramEntity> MasterProgramByFilter(MasterProgramEntity entity, int Divisioncode, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        DbaEntity MasterProgramAdd(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        MasterProgramEntity MasterProgramById(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        DbaEntity MasterProgramEdit(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        DbaEntity MasterProgramDelete(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        DbaEntity MasterProgramIsExists(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        ListItem[] MasterProgramList(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        MasterProgramEntity MasterProgramValidationTypeSearch(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        DbaEntity MasterProgramRelationship(MasterProgramEntity entity, DataTable EmpleadosList, DataTable LaborList, DataTable PositionsList);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        Tuple<MasterProgramEntity, List<string>, List<int>, List<string>> MasterProgramRelationshipById(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        Tuple<int, List<EmployeeEntity>, List<PositionEntity>, List<LaborEntity>> MasterProgramRelatedSummary(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        List<EmployeeEntity> MasterProgramByEmployeeList(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        ListItem[] MasterProgramByEmployeesByPlacesOccupation(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        ListItem[] MasterProgramByLaborById(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        List<LaborEntity> MasterProgramByLaborByCode(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        List<PositionEntity> MasterProgramByPositions(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        ListItem[] MasterProgramByPositionsByPlacesOccupation(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        ListItem[] MatrixTargetList(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        MatrixMasterProgramResultEntity MasterProgramByCourseByFilter(MasterProgramEntity entity, DataTable ThematicAreas, DataTable Courses, DataTable Positions, DataTable Labors, DataTable Employees);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        List<CourseEntity> CoursesListByThematicArea(string GeographicDivisionCode, int DivisionCode, DataTable ThematicAreaCode);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        ListItem[] MasterProgramThematicAreasListByCoursesExists(MasterProgramEntity entity);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        DbaEntity MasterProgramByCourseAdd(DataTable Course);

        /// <summary>
        /// List the master Program assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        List<MasterProgramEntity> MasterProgramByTrainingPlanProgramsAssociated(string GeographicDivisionCode, int divisionCode, string trainingPlanProgramCode);

        /// <summary>
        /// List the master Program not assocated with a training programs key: GeographicDivisionCode
        /// </summary>
        List<MasterProgramEntity> MasterProgramByTrainingPlanProgramsNotAssociated(string GeographicDivisionCode);
    }
}