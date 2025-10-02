using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.DataAccess.Interfaces
{
    public interface ISurveyQuestionsDal<T> where T : SurveyQuestionEntity
    {
        /// <summary>
        /// List all the survey questions
        /// </summary>
        /// <returns>The survey questions</returns>
        List<T> ListAll(int surveyVersion);
    }
}