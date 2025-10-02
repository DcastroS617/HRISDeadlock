using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Unity;
using Unity.Web;
using Unity.Attributes;
using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.DataAccess;
using System.Web.Http.Cors;

namespace HRISWeb.SocialResponsability
{
   
    [AllowAnonymous]
    public class SurveysController : ApiController
    {
        private ISurveysBll<SurveyEntity> objSurveysBll = null;
        private ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll = null;

        //public SurveysController([Dependency]ISurveysBll<Survey> objSurveys, [Dependency]ISurveyAnswersBll<SurveyAnswer> objSurveyAnswers)
        //{
        //    objSurveysBll = objSurveys;
        //    objSurveyAnswersBll = objSurveyAnswers;
        //}

     
        [HttpGet]
      
        [AllowAnonymous]
        public HttpResponseMessage Echo(string echo)
        {
            try
            {
                // do you want to test how is the application managing failures? uncomment the following lines...
                // int v1 = 0;
                // int v2 = 10 / v1;

                // generate a response
                HttpResponseMessage response = Request.CreateResponse(
                    HttpStatusCode.OK
                    , string.Format("Echoing: '{0}'.", echo));

                return response;
            }
            catch (Exception)
            {                
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Process failed.");
            }
        }

      
        [HttpPost]
       
        [AllowAnonymous]
        public HttpResponseMessage Save([FromBody]SurveyEntity survey)
        {
            HttpResponseMessage response;
            if (survey != null && survey.SurveyAnswers != null)
            {
                try
                {
                    objSurveysBll = new SurveysBll(new SurveysDal());
                    objSurveyAnswersBll = new SurveyAnswersBll(new SurveyAnswersDal());

                    survey.PendingSynchronization = false;

                    objSurveysBll.Save(survey);
                    objSurveyAnswersBll.Save(survey.SurveyAnswers);

                    response = new HttpResponseMessage(HttpStatusCode.OK);
                }
                catch (Exception)
                {
                    response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            //return Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}