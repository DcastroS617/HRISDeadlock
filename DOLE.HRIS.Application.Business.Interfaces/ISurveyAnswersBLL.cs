using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ISurveyAnswersBll<T> where T : SurveyAnswerEntity
    {
        /// <summary>
        /// List the survey answers
        /// </summary>
        /// <param name="surveyCode">The survey code</param>
        /// <returns>The survey answers</returns>
        List<T> ListBySurveyCode(long surveyCode, int SurveyVersion);

        /// <summary>
        /// Save(update or insert) the survey answers
        /// </summary>
        /// <param name="entity">The survey answers</param>
        void Save(List<T> entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="Lang"></param>
        /// <returns></returns>
        SurveySummaryEntity SurveySummaryGet(EmployeeEntity entity, string Lang);
    }
}