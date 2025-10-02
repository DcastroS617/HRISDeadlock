using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;

namespace DOLE.HRIS.Application.Business.Interfaces
{
    public interface ISurveyQuestionsBll<T> where T : SurveyQuestionEntity
    {
        /// <summary>
        /// List all the survey questions
        /// </summary>
        /// <returns>The survey questions</returns>
        List<T> ListAll(int surveyVersion);
    }
}