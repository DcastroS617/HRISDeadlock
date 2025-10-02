using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ISchoolTrainingBll
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geographicDivisionCode"></param>
        /// <param name="SchoolTraining"></param>
        /// <param name="DivisionCode"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        PageHelper<SchoolTrainingEntity> SchoolTrainingListByFilter(string geographicDivisionCode, SchoolTrainingEntity SchoolTraining, int DivisionCode, string sortExpression, string sortDirection, int? pageNumber);

        /// <summary>
        /// List the schools Training
        /// </summary>
        /// <param name="SchoolTraining"></param>
        /// <returns>The schools Training meeting the given filters</returns>
        List<SchoolTrainingEntity> ListByDivision(SchoolTrainingEntity SchoolTraining);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SchoolTraining"></param>
        /// <returns></returns>
        SchoolTrainingEntity SchoolTrainingByKey(SchoolTrainingEntity SchoolTraining);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SchoolTraining"></param>
        /// <returns></returns>
        SchoolTrainingEntity SchoolTrainingAdd(SchoolTrainingEntity SchoolTraining);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SchoolTraining"></param>
        /// <returns></returns>
        SchoolTrainingEntity SchoolTrainingEdit(SchoolTrainingEntity SchoolTraining);

        /// <summary>
        /// List the school training by course: courseCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="courseCode">courseCode</param>
        /// <param name="isforce">Is force</param>
        /// <returns>The courses meeting the given filters</returns>
        List<SchoolTrainingEntity> ListByCourses(string geographicDivisionCode, int divisionCode, string schoolTrainingCode, bool? isForce = null);

        /// <summary>
        /// Add the relation between the course and the schools Training
        /// </summary>
        /// <param name="entity">The schoolsTraining</param>
        /// <param name="course">the course</param>
        void AddSchoolsTrainingByCourse(SchoolTrainingEntity entity, CourseEntity course);

        /// <summary>
        /// Delete the relation between the course and the schools Training
        /// </summary>
        /// <param name="entity">The schoolsTraining</param>
        /// <param name="course">the course</param>
        void DeleteSchoolsTrainingByCourse(SchoolTrainingEntity entity, CourseEntity course);
    }
}
