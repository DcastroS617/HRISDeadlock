using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Application.Business.Remote;
using DOLE.HRIS.Application.DataAccess;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.App_Start;
using HRISWeb.Shared;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.UI;
using Unity;
using Unity.Web;
using static System.String;

namespace HRISWeb
{
    /// <summary>
    /// Class for global events of the application
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        private const string webAppIsLocal = "WebAppIsLocal";
        private const string preloadSocialResponsibilityCatalogs = "PreloadSocialResponsibilityCatalogs";
    
        /// <summary>
        /// Application Start
        /// </summary>
        /// <param name="sender">refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">contains the event data</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            GlobalConfiguration.Configure(WebApiConfig.Register);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            IUnityContainer container = Application.GetContainer();
            UnityConfig.RegisterTypes(container);

            Application["UnityContainer"] = container;

            //Initialize the service connection provider
            if (ConfigurationManager.AppSettings[webAppIsLocal].ToString().Equals("0"))
                ServiceConnectionProvider.Initialize();

            /* Load the Social Responsability Catalogs */
            if (LoadSocialResponsabilityCatalogs())
            {
                //
                IMaritalStatusBll<MaritalStatusEntity> objMaritalStatusBll = Application.GetContainer().Resolve<IMaritalStatusBll<MaritalStatusEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogMaritalStatus] = objMaritalStatusBll.ListEnabled();
                //
                IFamilyRelationshipsBll<FamilyRelationshipEntity> objFamilyRelationshipsBll = Application.GetContainer().Resolve<IFamilyRelationshipsBll<FamilyRelationshipEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogFamilyRelationships] = objFamilyRelationshipsBll.ListEnabled();
                //
                IAcademicDegreesBll<AcademicDegreeEntity> objAcademicDegreesBll = Application.GetContainer().Resolve<IAcademicDegreesBll<AcademicDegreeEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogAcademicDegrees] = objAcademicDegreesBll.ListEnabled();
                //
                ILanguagesBll<LanguageEntity> objLanguagesBll = Application.GetContainer().Resolve<ILanguagesBll<LanguageEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogLanguages] = objLanguagesBll.ListEnabled();
                //
                IDiseaseCarePlacesBll<DiseaseCarePlaceEntity> objDiseaseCarePlacesBll = Application.GetContainer().Resolve<IDiseaseCarePlacesBll<DiseaseCarePlaceEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDiseaseCarePlaces] = objDiseaseCarePlacesBll.ListEnabled();
                //
                IDisabilityTypesBll<DisabilityTypeEntity> objDisabilityTypesBll = Application.GetContainer().Resolve<IDisabilityTypesBll<DisabilityTypeEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDisabilityTypes] = objDisabilityTypesBll.ListEnabled();
                //
                IProfessionsBll<ProfessionEntity> objProfessionsBll = Application.GetContainer().Resolve<IProfessionsBll<ProfessionEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogProfessions] = objProfessionsBll.ListEnabled();
                //
                IDiseasesBll<DiseaseEntity> objDiseasesBll = Application.GetContainer().Resolve<IDiseasesBll<DiseaseEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDiseases] = objDiseasesBll.ListEnabled();
                //
                IOtherDiseasesBll<OtherDiseaseEntity> objOtherDiseasesBll = Application.GetContainer().Resolve<IOtherDiseasesBll<OtherDiseaseEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogOtherDiseases] = objOtherDiseasesBll.ListEnabled();
                //
                IDiseaseFrequenciesBll<DiseaseFrequencyEntity> objDiseaseFrequenciesBll = Application.GetContainer().Resolve<IDiseaseFrequenciesBll<DiseaseFrequencyEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDiseaseFrequencies] = objDiseaseFrequenciesBll.ListEnabled();
                //
                ITransportsBll<TransportEntity> objTransportsBll = Application.GetContainer().Resolve<ITransportsBll<TransportEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogTransports] = objTransportsBll.ListEnabled();
                //
                IHousingTypesBll<HousingTypeEntity> objHousingTypesBll = Application.GetContainer().Resolve<IHousingTypesBll<HousingTypeEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingTypes] = objHousingTypesBll.ListEnabled();
                //
                IHousingTenuresBll<HousingTenureEntity> objHousingTenuresBll = Application.GetContainer().Resolve<IHousingTenuresBll<HousingTenureEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingTenures] = objHousingTenuresBll.ListEnabled();
                //
                IHousingAcquisitionWaysBll<HousingAcquisitionWayEntity> objHousingAcquisitionWaysBll = Application.GetContainer().Resolve<IHousingAcquisitionWaysBll<HousingAcquisitionWayEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingAcquisitionWays] = objHousingAcquisitionWaysBll.ListEnabled();
                //
                IHousingDistributionsBll<HousingDistributionEntity> objHousingDistributionsBll = Application.GetContainer().Resolve<IHousingDistributionsBll<HousingDistributionEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingDistributions] = objHousingDistributionsBll.ListEnabled();
                //
                IFloorTypesBll<FloorTypeEntity> objFloorTypesBll = Application.GetContainer().Resolve<IFloorTypesBll<FloorTypeEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogFloorTypes] = objFloorTypesBll.ListEnabled();
                //
                IWallTypesBll<WallTypeEntity> objWallTypesBll = Application.GetContainer().Resolve<IWallTypesBll<WallTypeEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogWallTypes] = objWallTypesBll.ListEnabled();
                //
                IRoofTypesBll<RoofTypeEntity> objRoofTypesBll = Application.GetContainer().Resolve<IRoofTypesBll<RoofTypeEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogRoofTypes] = objRoofTypesBll.ListEnabled();
                //
                IBasicServicesBll<BasicServiceEntity> objBasicServicesBll = Application.GetContainer().Resolve<IBasicServicesBll<BasicServiceEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogBasicServices] = objBasicServicesBll.ListEnabled();
                //
                ISectorsBll<SectorEntity> objSectorsBll = Application.GetContainer().Resolve<ISectorsBll<SectorEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogSectors] = objSectorsBll.ListEnabled();
                //
                IServicesAvailabilityBll<ServicesAvailabilityEntity> objServicesAvailabilityBll = Application.GetContainer().Resolve<IServicesAvailabilityBll<ServicesAvailabilityEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogServicesAvailability] = objServicesAvailabilityBll.ListEnabled();
                //
                IOtherServicesBll<OtherServiceEntity> objOtherServicesBll = Application.GetContainer().Resolve<IOtherServicesBll<OtherServiceEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogOtherServices] = objOtherServicesBll.ListEnabled();
                //
                IGarbageDisposalTypesBll<GarbageDisposalTypeEntity> objGarbageDisposalTypesBll = Application.GetContainer().Resolve<IGarbageDisposalTypesBll<GarbageDisposalTypeEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogGarbageDisposalTypes] = objGarbageDisposalTypesBll.ListEnabled();
                //
                IWaterSuppliesBll<WaterSupplyEntity> objWaterSuppliesBll = Application.GetContainer().Resolve<IWaterSuppliesBll<WaterSupplyEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogWaterSupplies] = objWaterSuppliesBll.ListEnabled();
                //
                ICookEnergyTypesBll<CookEnergyTypeEntity> objCookEnergyTypesBll = Application.GetContainer().Resolve<ICookEnergyTypesBll<CookEnergyTypeEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogCookEnergyTypes] = objCookEnergyTypesBll.ListEnabled();
                //
                IToiletTypesBll<ToiletTypeEntity> objToiletTypesBll = Application.GetContainer().Resolve<IToiletTypesBll<ToiletTypeEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogToiletTypes] = objToiletTypesBll.ListEnabled();
                //
                IHouseholdContributionRangesByDivisionsBll<HouseHoldContributionRangeByDivisionEntity> objHouseholdContributionRangesByDivisionsBll = Application.GetContainer().Resolve<IHouseholdContributionRangesByDivisionsBll<HouseHoldContributionRangeByDivisionEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHouseholdContributionRangesByDivisions] = objHouseholdContributionRangesByDivisionsBll.ListEnabled();
                //
                IPoliticalDivisionsLabelsBll<PoliticalDivisionLabelEntity> objPoliticalDivisionsLabelsBll = Application.GetContainer().Resolve<IPoliticalDivisionsLabelsBll<PoliticalDivisionLabelEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogPoliticalDivisionsLabels] = objPoliticalDivisionsLabelsBll.ListEnabled();
                //
                ICurrenciesBll<CurrencyEntity> objCurrenciesBll = Application.GetContainer().Resolve<ICurrenciesBll<CurrencyEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogCurrencies] = objCurrenciesBll.ListEnabled();
                //
                ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll = Application.GetContainer().Resolve<ISurveyQuestionsBll<SurveyQuestionEntity>>();
                Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilitySurveyQuestions] = objSurveyQuestionsBll.ListAll(Convert.ToInt32(ConfigurationManager.AppSettings["SurveyVersion"]));
            }
        }

        /// <summary>
        /// Session start
        /// </summary>
        /// <param name="sender">refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">contains the event data</param>
        protected void Session_Start(object sender, EventArgs e)
        {
            if (Session.IsNewSession)
            {
                if (!SessionManager.DoesUserLoggedIn || !SessionManager.DoesKeyExist(SessionKey.WorkingDivision))
                {                    
                    Response.Redirect("~/Default.aspx", false);
                }
            }            
        }

        /// <summary>
        /// Handle the application pre request handler event
        /// </summary>
        /// <param name="sender">refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">contains the event data</param>
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            IHttpHandler handler = HttpContext.Current.Handler as System.Web.UI.Page;
            Page page = HttpContext.Current.Handler as System.Web.UI.Page;

            if (handler != null)
            {
                var container = Application.GetContainer();

                var userClaims = HttpContext.Current.User as ClaimsPrincipal;
                List<string> roles = UserHelper.GetUserRoles(userClaims);
                bool allowedCasbin = CasbinBll.CheckAccess(UserHelper.GetCurrentFullUserName, "read", page.AppRelativeVirtualPath, roles);

                if (container != null && allowedCasbin)
                {
                    container.BuildUp(handler.GetType(), handler);
                }
            }

            if (Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState)
            {                
                if (Session.IsNewSession || !SessionManager.DoesUserLoggedIn)
                {                    
                    if (!Context.Request.Url.AbsoluteUri.ToLower().Contains("/default.aspx"))
                    {                        
                        Context.Response.Redirect(Format("~/default.aspx?ReturnUrl={0}", Context.Request.Url.PathAndQuery));
                    }
                }
            }
        }

        /// <summary>
        /// Application begin request
        /// </summary>
        /// <param name="sender">refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">contains the event data</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Application authentixation request
        /// </summary>
        /// <param name="sender">refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">contains the event data</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Application post authentication request
        /// </summary>
        /// <param name="sender">refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">contains the event data</param>
        /*protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            /*var path = HttpContext.Current.Request.Path;

            bool isStatic = path.Equals("/signin-oidc", StringComparison.OrdinalIgnoreCase)
             || path.Equals("/signout-callback-oidc", StringComparison.OrdinalIgnoreCase) 
             || path.EndsWith(".axd") || path.StartsWith("/content")
             || path.StartsWith("/scripts") || path.StartsWith("/images")
             || path.StartsWith("/errorpages");

            if (isStatic)
                return;

            if (!HttpContext.Current.User.Identity.IsAuthenticated
                && !isStatic 
                && HttpContext.Current.Request.HttpMethod == "GET")
            {
                //Response.Redirect("~/default.aspx");
                HttpContext.Current.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties
                    {
                        RedirectUri = HttpContext.Current.Request.RawUrl
                    },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType
                );

                HttpContext.Current.Response.StatusCode = 401;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

            
        }*/


        /// <summary>
        /// Application error
        /// </summary>
        /// <param name="sender">refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">contains the event data</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                // Get the error details
                HttpException lastErrorWrapper = Server.GetLastError() as HttpException;

                if (lastErrorWrapper != null && lastErrorWrapper.InnerException != null)
                {
                    Exception lastError = lastErrorWrapper.InnerException;

                    CustomException customException = new CustomException();
                    customException.CreateLogException(String.Format("{0}. Request object:{1}", lastError.Message, Request.RawUrl), lastError);
                }

                Server.ClearError();

                if (lastErrorWrapper != null && lastErrorWrapper.GetHttpCode() == 403)
                {
                    HttpContext.Current.RewritePath("~/ErrorPages/Error403.aspx");
                }

                if (lastErrorWrapper != null && lastErrorWrapper.GetHttpCode() == 404)
                {
                    HttpContext.Current.RewritePath("~/ErrorPages/Error404.aspx");
                }

                else
                {
                    HttpContext.Current.RewritePath("~/ErrorPages/Error405.aspx");
                }
            }
            finally
            {
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnGlobalAsax{0}", Guid.NewGuid()), "setTimeout(function () {{ ActivateModalProgress(false); }}, 200);", true);
            }
        }

        /// <summary>
        /// Session end
        /// </summary>
        /// <param name="sender">refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">contains the event data</param>
        protected void Session_End(object sender, EventArgs e)
        {            
        }

        /// <summary>
        /// Application end
        /// </summary>
        /// <param name="sender">refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">contains the event data</param>
        protected void Application_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Preload the social responsability catalogs on the application start
        /// </summary>
        /// <returns>True if load the catalogs, false otherwise</returns>
        private bool LoadSocialResponsabilityCatalogs()
        {
            return ConfigurationManager.AppSettings[preloadSocialResponsibilityCatalogs].ToString() != null && ConfigurationManager.AppSettings[preloadSocialResponsibilityCatalogs].ToString().Equals("1") ;
        }
    }
}