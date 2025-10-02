using DOLE.HRIS.Entity;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ICoursesDal<T> where T : CourseEntity
    {
        /// <summary>
        /// List the courses by the given filters
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Code</param>
        /// <param name="courseName">Description</param>
        /// <param name="courseAcronym">Acroonym</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="sortDirection">Sort Direction</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageSizeParameterModuleCode">Page size parameter module code</param>
        /// <param name="pageSizeParameterName">Page size parameter name</param>
        /// <returns>The courses meeting the given filters and page config</returns>
        PageHelper<CourseEntity> ListByFilters(int divisionCode, string geographicDivisionCode, string courseCode, string courseName,string courseState, string sortExpression, string sortDirection, int pageNumber, int? pageSize, int pageSizeValue, string Lang = "ES");

        /// <summary>
        /// List the courses by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        List<T> ListByDivision(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the course by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <param name="divisionCode">Division code</param>
        /// <returns>The course</returns>
        T ListByKey(string geographicDivisionCode, string courseCode, int divisionCode);

        /// <summary>
        /// Add the course
        /// </summary>
        /// <param name="entity">The course</param>
        void Add(T entity);

        /// <summary>
        /// Edit the course
        /// </summary>
        /// <param name="entity">The course</param>
        void Edit(T entity);

        /// <summary>
        /// Delete the course
        /// </summary>
        /// <param name="entity">The course</param>
        void Delete(T entity);

        /// <summary>
        /// Activate the course
        /// </summary>
        /// <param name="entity">The course</param>
        void Activate(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Lang"></param>
        /// <returns></returns>
        List<ClassificationCourseEntity> ClassificationCourseListGet(string Lang);

        /// <summary>
        /// cOURSE PARAMETERS  MAX COUNT WORK
        /// </summary>
        /// <param name="@PiCoursesParametersId">rOW /param>
        /// <param name="@PcGeographicDivisionCode">Geographic division code</param>
        /// <param name="@PiDivisionCode">dIVISION CODE</param>
        /// <param name="trainingModuleCode">Training module code</param>
        /// <returns> cOURSE PARAMETERS  MAX COUNT WORK</returns>
        /// 
        short MaxAmountWorkCourses();

        /// <summary>
        /// List the course by key: GeographicDivisionCode and Code
        /// </summary>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="courseCode">Course code</param>
        /// <param name="divisionCode">Division code</param>
        /// <returns>The course</returns>
        List<T> ListByKeyExport(string geographicDivisionCode, int divisionCode);

        /// <summary>
        /// List the courses by training program: GeographicDivisionCode and TrainingProgramCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="trainingProgramCode">trainingProgramCode</param>
        /// <returns>The courses meeting the given filters</returns>
        List<CourseEntity> ListByTrainingProgram(string geographicDivisionCode, int divisionCode, string trainingProgramCode);

        /// <summary>
        /// Add the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainingProgram">the trainingProgram</param>
        void AddCourseByTrainingProgram(CourseEntity entity, TrainingProgramEntity trainingProgram);

        /// <summary>
        /// Delete the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainingProgram">the trainingProgram</param>
        void DeleteCourseByTrainingProgram(CourseEntity entity, TrainingProgramEntity trainingProgram);

        /// <summary>
        /// List the courses by school training: schoolTrainingCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic Division Code</param>
        /// <param name="DivisionCode">Division Code</param>
        /// <param name="schoolTrainingCode">schoolTrainingCode</param>
        /// <returns>The courses meeting the given filters</returns>
        List<CourseEntity> ListBySchoolsTraining(string geographicDivisionCode, int DivisionCode, string schoolTrainingCode);

        /// <summary>
        /// Add the relation between the course and the schools Training
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="schoolsTraining">the schoolsTraining</param>
        void AddCourseBySchoolsTraining(CourseEntity entity, SchoolTrainingEntity schoolsTraining);

        /// <summary>
        /// Delete the relation between the course and the schools Training
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="schoolsTraining">the schoolsTraining</param>
        void DeleteCourseBySchoolsTraining(CourseEntity entity, SchoolTrainingEntity schoolsTraining);

        /// <summary>
        /// List the course associated with a trainer key: GeographicDivisionCode and trainerCode 
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">division code</param>
        /// <param name="trainerCode">Trainer code</param>
        /// <returns>The trainers meeting the given filters</returns>
        List<CourseEntity> ListByCoursesByTrainerAssociated(string geographicDivisionCode, int divisionCode, string trainerCode);

        /// <summary>
        /// List the course not associated with a trainer key: GeographicDivisionCode 
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">division code</param>
        /// <returns>The trainers meeting the given filters</returns>
        List<CourseEntity> ListByCourseByTrainersNotAssociated(string geographicDivisionCode, int divisionCode, string trainerCode);

        /// <summary>
        /// Add the relation between the course and the trainer
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainer">the trainer</param>
        void AddTrainerByCourse(T entity, TrainerEntity trainer);

        /// <summary>
        /// Delete the relation between the course and the trainer
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="trainer">the trainer</param>
        void DeleteTrainerByCourse(T entity, TrainerEntity trainer);

        /// <summary>
        /// List the courses by payment rate: GeographicDivisionCode and PaymentRateCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="paymentRateCode">paymentRateCode</param>
        /// <returns>The courses meeting the given filters</returns>
        List<CourseEntity> ListByPaymentRate(string geographicDivisionCode, int divisionCode, int paymentRateCode);

        /// <summary>
        /// Delete the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="paymentRate">the paymentRate</param>
        void DeleteCourseByPaymentRate(CourseEntity entity, PaymentRateEntity paymentRate);

        /// <summary>
        /// Add the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="paymentRate">the paymentRate</param>
        void AddCourseByPaymentRate(CourseEntity entity, PaymentRateEntity paymentRate);

        /// <summary>
        /// List the courses by position: GeographicDivisionCode and PositionCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="positionCode">positionCode</param>
        /// <returns>The courses meeting the given filters</returns>
        List<CourseEntity> ListByPosition(string geographicDivisionCode, string positionCode);

        /// <summary>
        /// Delete the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="position">the position</param>
        void DeleteCourseByPosition(CourseEntity entity, PositionEntity position);

        /// <summary>
        /// Add the relation between the course and the training program
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="position">the position</param>
        void AddCourseByPosition(CourseEntity entity, PositionEntity position);

        /// <summary>
        /// List the courses not associated with a themtic area by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        List<T> ListByDivisionNotThematicAreaAssociated(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the courses by thematic area: GeographicDivisionCode and ThematicAreaCode
        /// </summary>        
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="divisionCode">Division code</param>
        /// <param name="thematicAreaCode">thematicAreaCode</param>
        /// <returns>The courses meeting the given filters</returns>
        List<CourseEntity> ListByThematicArea(string geographicDivisionCode, int divisionCode, string thematicAreaCode);

        /// <summary>
        /// Add the relation between the course and the thematic area
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        void AddCourseByThematicArea(CourseEntity entity, ThematicAreaEntity thematicArea);

        /// <summary>
        /// Delete the relation between the course and the thematic area
        /// </summary>
        /// <param name="entity">The course</param>
        /// <param name="thematicArea">the thematicArea</param>
        void DeleteCourseByThematicArea(CourseEntity entity, ThematicAreaEntity thematicArea);

        /// <summary>
        /// List the Courses that meet the filters and is related to a logbook
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <param name="user">User</param>
        /// <param name="trainingModuleCode">Training module code</param>
        /// <returns>The trainers meeting the given filters</returns>
        List<T> ListByLogbook(int divisionCode, string geographicDivisionCode, string user);

        /// <summary>
        /// List the courses used in logbooks by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        List<T> ListByDivisionUsedByLogbooks(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// List the courses used in logbooks history by division key: Division and GeographicDivisionCode
        /// </summary>
        /// <param name="divisionCode">Division code</param>
        /// <param name="geographicDivisionCode">Geographic division code</param>
        /// <returns>The courses meeting the given filters</returns>
        List<T> ListByDivisionUsedByLogbooksHistory(int divisionCode, string geographicDivisionCode);

        /// <summary>
        /// Validates a course by thematic area using the provided entity parameters.
        /// </summary>
        /// <param name="entity">The course entity containing DivisionCode and CourseCode.</param>
        /// <returns>
        /// Returns an integer result from the stored procedure indicating the validation status.
        /// </returns>
        /// <exception cref="DataAccessException">
        /// Thrown when an SQL error occurs during the database operation or for other exceptions during data access.
        /// </exception>
        int ValidateCourseByThematicArea(CourseEntity entity);
    }
}