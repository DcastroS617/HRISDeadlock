using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ISurveyAnswersDal<T> where T : SurveyAnswerEntity
    {
        /// <summary>
        /// List the survey answers
        /// </summary>
        /// <param name="entity">SurveyAnswer entity, the survey code</param>
        /// <returns>The survey answers</returns>
        List<T> ListBySurveyCode(T entity);

        /// <summary>
        /// Save(update or insert) the survey answers
        /// </summary>
        /// <param name="entity">The survey answers</param>
        void Save(SurveyAnswersTypeEntity entity);

        SurveySummaryEntity SurveySummaryGet(EmployeeEntity entity, string Lang, int SurveyVersion);
    }
}