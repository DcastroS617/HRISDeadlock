using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HRISWeb.Shared;
using System.Threading;
using System.Globalization;
using DOLE.HRIS.Exceptions;
using Unity;
using Unity.Web;
using Unity.Attributes;
using static System.String;
using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Shared.Entity;
using System.Configuration;

namespace HRISWeb.SocialResponsability
{
    public partial class HousingDistribution : System.Web.UI.Page
    {
        [Dependency]
        protected IHousingDistributionsBll<DOLE.HRIS.Shared.Entity.HousingDistributionEntity> objHousingDistributionsBll { get; set; }
        
        [Dependency]
        protected IFloorTypesBll<FloorTypeEntity> objFloorTypesBll { get; set; }
     
        [Dependency]
        protected IWallTypesBll<WallTypeEntity> objWallTypesBll { get; set; }
       
        [Dependency]
        protected IRoofTypesBll<RoofTypeEntity> objRoofTypesBll { get; set; }
        
        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }
      
        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }
       
        [Dependency]
        protected ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll { get; set; }
    
        private const string surveyVersionName = "SurveyVersion";

        private int SurveyVersion;

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
        /// <summary>
        /// Handles the load of the page
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SurveyVersion = Convert.ToInt32(ConfigurationManager.AppSettings[surveyVersionName].ToString());
            if (!IsPostBack)
            {
                LoadHousingDistributions();

                cboBedroomsQuantity.Items.Add(new ListItem(Empty, "-1"));
                cboBedroomsQuantity.Items.AddRange(Enumerable.Range(0, 10).Select(b => new ListItem(b.ToString(), b.ToString())).ToArray());
                                
                cboBathroomsInternalQuantity.Items.AddRange(new ListItem[11] { new ListItem(Empty, "-1"), new ListItem("0"), new ListItem("1")
                    , new ListItem("1.5"), new ListItem("2"), new ListItem("2.5"), new ListItem("3"), new ListItem("3.5")
                    , new ListItem("4"), new ListItem("4.5"), new ListItem("5")});

                cboBathroomsExternalQuantity.Items.AddRange(new ListItem[11] { new ListItem(Empty, "-1"), new ListItem("0"), new ListItem("1")
                    , new ListItem("1.5"), new ListItem("2"), new ListItem("2.5"), new ListItem("3"), new ListItem("3.5")
                    , new ListItem("4"), new ListItem("4.5"), new ListItem("5")});
                LoadFloorTypes();
                LoadWallTypes();
                LoadRoofTypes();
                //
                LoadSurveyAnswers();
                //
                ValidateWorkingDivisionVsEmployeeDivision();
            }
        }
        /// <summary>
        /// Handles the lbtnBack click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("HousingInformation.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("BasicServices.aspx", false);
        }
        /// <summary>
        /// Handles the lbtnSaveAsDraft click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnSaveAsDraft_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(true);
        }

        /// <summary>
        /// Gets the enabled Housing distributions
        /// </summary>
        private void LoadHousingDistributions()
        {
            List<DOLE.HRIS.Shared.Entity.HousingDistributionEntity> housingDistributionsBDList = new List<DOLE.HRIS.Shared.Entity.HousingDistributionEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingDistributions] != null)
                {
                    housingDistributionsBDList = (List<DOLE.HRIS.Shared.Entity.HousingDistributionEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogHousingDistributions];
                }
                else
                {
                    objHousingDistributionsBll = objHousingDistributionsBll ?? Application.GetContainer().Resolve<IHousingDistributionsBll<DOLE.HRIS.Shared.Entity.HousingDistributionEntity>>();
                    housingDistributionsBDList = objHousingDistributionsBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                cboHousingDistribution.Items.AddRange(housingDistributionsBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.HousingDistributionDescriptionSpanish : f.HousingDistributionDescriptionEnglish)
                    .Select(d => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? d.HousingDistributionDescriptionSpanish : d.HousingDistributionDescriptionEnglish
                        , d.HousingDistributionCode.ToString())).ToArray());
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, newEx.Message);
                }
            }
        }
        /// <summary>
        /// Gets the enabled Floor Types
        /// </summary>
        private void LoadFloorTypes()
        {
            List<FloorTypeEntity> floorTypesBDList = new List<FloorTypeEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogFloorTypes] != null)
                {
                    floorTypesBDList = (List<FloorTypeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogFloorTypes];
                }
                else
                {
                    objFloorTypesBll = objFloorTypesBll ?? Application.GetContainer().Resolve<IFloorTypesBll<FloorTypeEntity>>();
                    floorTypesBDList = objFloorTypesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();
                
                cboFloor.Items.Add(new ListItem(Empty, "-1"));
                cboFloor.Items.AddRange(floorTypesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.FloorTypeDescriptionSpanish : f.FloorTypeDescriptionEnglish)
                    .Select(d => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? d.FloorTypeDescriptionSpanish : d.FloorTypeDescriptionEnglish
                        , d.FloorTypeCode.ToString())).ToArray());
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, newEx.Message);
                }
            }
        }
        /// <summary>
        /// Gets the enabled Wall Types
        /// </summary>
        private void LoadWallTypes()
        {
            List<WallTypeEntity> wallTypesBDList = new List<WallTypeEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogWallTypes] != null)
                {
                    wallTypesBDList = (List<WallTypeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogWallTypes];
                }
                else
                {
                    objWallTypesBll = objWallTypesBll ?? Application.GetContainer().Resolve<IWallTypesBll<WallTypeEntity>>();
                    wallTypesBDList = objWallTypesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                cboWalls.Items.Add(new ListItem(Empty, "-1"));
                cboWalls.Items.AddRange(wallTypesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.WallTypeDescriptionSpanish : f.WallTypeDescriptionEnglish)
                    .Select(d => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? d.WallTypeDescriptionSpanish : d.WallTypeDescriptionEnglish
                        , d.WallTypeCode.ToString())).ToArray());
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, newEx.Message);
                }
            }
        }
        /// <summary>
        /// Gets the enabled Roof Types
        /// </summary>
        private void LoadRoofTypes()
        {
            List<RoofTypeEntity> roofTypesBDList = new List<RoofTypeEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogRoofTypes] != null)
                {
                    roofTypesBDList = (List<RoofTypeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogRoofTypes];
                }
                else
                {
                    objRoofTypesBll = objRoofTypesBll ?? Application.GetContainer().Resolve<IRoofTypesBll<RoofTypeEntity>>();
                    roofTypesBDList = objRoofTypesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();
                cboHouseRoof.Items.Add(new ListItem(Empty, "-1"));
                cboHouseRoof.Items.AddRange(roofTypesBDList.OrderBy(f => currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.RoofTypeDescriptionSpanish : f.RoofTypeDescriptionEnglish)
                    .Select(d => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? d.RoofTypeDescriptionSpanish : d.RoofTypeDescriptionEnglish
                        , d.RoofTypeCode.ToString())).ToArray());
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, newEx.Message);
                }
            }
        }
        /// <summary>
        /// Gets the current culture selected by the user
        /// </summary>
        /// <returns>The current cultture</returns>
        private CultureInfo GetCurrentCulture()
        {
            if (Session[Constants.cCulture] != null)
            {
                return new CultureInfo(Convert.ToString(Session[Constants.cCulture]));
            }
            return new CultureInfo(Constants.cCultureEsCR);
        }
        /// <summary>
        /// Validates that the working division is the same of the current employee survey
        /// </summary>
        private void ValidateWorkingDivisionVsEmployeeDivision()
        {
            if (Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard] != null)
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> currentSurvey = (Tuple<SurveyEntity, List<SurveyAnswerEntity>>)Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard];

                if (!SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                    .Equals(currentSurvey.Item1.DivisionCode))
                {
                    Session.Remove(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard);
                    Session.Remove(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogPoliticalDivisions);
                    Response.Redirect("StartSurvey.aspx", false);
                }
            }
            else
            {
                Response.Redirect("StartSurvey.aspx", false);
            }
        }
        /// <summary>
        /// Gets the survey questions
        /// </summary>
        /// <returns>The survey questions</returns>
        private List<SurveyQuestionEntity> GetSurveyQuestions()
        {
            List<SurveyQuestionEntity> surveyQuestions = null;

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilitySurveyQuestions] != null)
                {
                    surveyQuestions = (List<SurveyQuestionEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilitySurveyQuestions];
                }
                else
                {
                    objSurveyQuestionsBll = objSurveyQuestionsBll ?? Application.GetContainer().Resolve<ISurveyQuestionsBll<SurveyQuestionEntity>>();
                    surveyQuestions = objSurveyQuestionsBll.ListAll(Convert.ToInt32(ConfigurationManager.AppSettings["SurveyVersion"]));
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, newEx.Message);
                }
            }
            return surveyQuestions;
        }
        /// <summary>
        /// Gets the survey header and answers values
        /// </summary>
        /// <returns>The survey values</returns>
        private Tuple<SurveyEntity, List<SurveyAnswerEntity>> GetSurveyAnswers()
        {
            Tuple<SurveyEntity, List<SurveyAnswerEntity>> currentSurvey = null;
            try
            {
                if (Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard] != null)
                {
                    currentSurvey = (Tuple<SurveyEntity, List<SurveyAnswerEntity>>)Session[HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard];
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, newEx.Message);
                }
            }
            return currentSurvey;
        }
        /// <summary>
        /// Load the survey answers for the current page
        /// </summary>
        private void LoadSurveyAnswers()
        {
            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    cboHousingDistribution.SelectedIndex = -1;
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("39"));
                    hdfSelectedHousingDistributions.Value = answer != null ? answer.AnswerValue : Empty;
                    //
                    cboBedroomsQuantity.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("39.a"));
                    ListItem liBedroomsQuantity = answer != null ? cboBedroomsQuantity.Items.FindByValue(answer.AnswerValue) : null;
                    if (liBedroomsQuantity != null)
                    {
                        liBedroomsQuantity.Selected = true;
                    }
                    //
                    cboBathroomsInternalQuantity.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("39.b"));
                    ListItem liBathroomsQuantity = answer != null ? cboBathroomsInternalQuantity.Items.FindByValue(answer.AnswerValue) : null;
                    if (liBathroomsQuantity != null)
                    {
                        liBathroomsQuantity.Selected = true;
                    }
                    //
                    cboBathroomsExternalQuantity.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("39.c"));
                    ListItem liBathroomsExternalQuantity = answer != null ? cboBathroomsExternalQuantity.Items.FindByValue(answer.AnswerValue) : null;
                    if (liBathroomsExternalQuantity != null)
                    {
                        liBathroomsExternalQuantity.Selected = true;
                    }
                    //
                    cboFloor.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("40.1"));
                    ListItem liFloor = answer != null ? cboFloor.Items.FindByValue(answer.AnswerValue) : null;
                    if (liFloor != null)
                    {
                        liFloor.Selected = true;
                    }
                    //
                    cboWalls.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("40.2"));
                    ListItem liWalls = answer != null ? cboWalls.Items.FindByValue(answer.AnswerValue) : null;
                    if (liWalls != null)
                    {
                        liWalls.Selected = true;
                    }                    
                    //
                    cboHouseRoof.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("40.3"));
                    ListItem liHouseRoof = answer != null ? cboHouseRoof.Items.FindByValue(answer.AnswerValue) : null;
                    if (liHouseRoof != null)
                    {
                        liHouseRoof.Selected = true;
                    }                  
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("40.4"));
                    chkCeiling.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, newEx.Message);
                }
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this
                    , this.GetType()
                    , "ReturnLoadSurveyAnswers" + Guid.NewGuid().ToString()
                    , "setTimeout(function () { RestoreSelectedHousingDistributions(); }, 200);", true);
            }
        }
        /// <summary>
        /// Save the survey answers of the employee
        /// </summary>
        /// <param name="saveAsDraft">Indicates if the survey must saved as draft</param>
        private void SaveSurveyAnswers(bool saveAsDraft)
        {
            try
            {
                // get answers from screen
                string housingDistributions = hdfSelectedHousingDistributions.Value;
                string bedrooms = cboBedroomsQuantity.SelectedValue;
                string bathrooms = cboBathroomsInternalQuantity.SelectedValue;
                string externalBathrooms = cboBathroomsExternalQuantity.SelectedValue;
                string floorType = cboFloor.SelectedValue;                             
                string wallType = cboWalls.SelectedValue;
                string houseRoof = cboHouseRoof.SelectedValue;
                bool hasCeiling = chkCeiling.Checked;                                
                //
                List<Tuple<string, byte, string,int>> employeeAnswers = new List<Tuple<string, byte, string, int>>();
                if (!IsNullOrWhiteSpace(housingDistributions))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int >("39", 1, housingDistributions,6));
                }
                if (!IsNullOrWhiteSpace(bedrooms) && !bedrooms.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int >("39.a", 1, bedrooms, 6));
                }
                if (!IsNullOrWhiteSpace(bathrooms) && !bathrooms.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int >("39.b", 1, bathrooms, 6));
                }
                if (!IsNullOrWhiteSpace(externalBathrooms) && !externalBathrooms.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("39.c", 1, externalBathrooms, 6));
                }
                //
                if (!IsNullOrWhiteSpace(floorType) && !floorType.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int >("40.1", 1, floorType, 6));
                }
                //
                if (!IsNullOrWhiteSpace(wallType) && !wallType.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int >("40.2", 1, wallType, 6));
                }               
                //
                if (!IsNullOrWhiteSpace(houseRoof) && !houseRoof.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int >("40.3", 1, houseRoof, 6));
                }
                //
                employeeAnswers.Add(new Tuple<string, byte, string, int >("40.4", 1, hasCeiling.ToString(), 6));
                
                // Insert or edit answers values
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                List<SurveyQuestionEntity> surveyQuestions = GetSurveyQuestions();
                string currentUser = UserHelper.GetCurrentFullUserName;
                if (surveyAnswers.Item2 != null)
                {
                    surveyAnswers.Item2.ForEach(a => a.LastModifiedUser = currentUser);
                }

                surveyAnswers.Item2.RemoveAll(a => a.QuestionID.Contains("40.1") || a.QuestionID.Contains("40.2") || a.QuestionID.Contains("40.3")
                || a.QuestionID.Contains("39.") );

                foreach (var item in employeeAnswers)
                {
                    var answer = surveyAnswers.Item2.Find(v => v.QuestionID.Equals(item.Item1) && v.AnswerItem.Equals(item.Item2));
                    if (answer != null)
                    {
                        answer.AnswerValue = item.Item3;
                        answer.LastModifiedUser = currentUser;
                        answer.SurveyVersion = this.SurveyVersion;
                        answer.IdSurveyModule = 6;
                    }
                    else
                    {
                        SurveyQuestionEntity question = surveyQuestions.Find(q => q.QuestionID.Equals(item.Item1));
                        surveyAnswers.Item2.Add(new SurveyAnswerEntity(surveyAnswers.Item1.SurveyCode
                            , question != null ? question.QuestionCode : 0
                            , item.Item1
                            , item.Item2
                            , item.Item3
                            , currentUser
                            ,item.Item4) { SurveyVersion = this.SurveyVersion });
                    }
                }// FOREACH                
                //
                // Save current state for the page                
                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(HrisEnum.SurveyStates.Draft);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.HousingDistribution);
                surveyAnswers.Item1.SurveyCompletedBy = currentUser;
                surveyAnswers.Item1.LastModifiedUser = currentUser;
                //
                // Save in data base
                if (saveAsDraft)
                {
                    objSurveysBll = objSurveysBll ?? Application.GetContainer().Resolve<ISurveysBll<SurveyEntity>>();
                    objSurveysBll.SaveCurrentState(surveyAnswers.Item1.SurveyCode
                        , surveyAnswers.Item1.SurveyStateCode
                        , surveyAnswers.Item1.SurveyCurrentPageCode
                        , surveyAnswers.Item1.SurveyCompletedBy
                        , null
                        , surveyAnswers.Item1.LastModifiedUser
                        , this.SurveyVersion);

                    objSurveyAnswersBll = objSurveyAnswersBll ?? Application.GetContainer().Resolve<ISurveyAnswersBll<SurveyAnswerEntity>>();
                    objSurveyAnswersBll.Save(surveyAnswers.Item2);

                    MensajeriaHelper.MostrarMensaje(upnMain, TipoMensaje.Informacion, GetLocalResourceObject("msjSurveySavedAsDraft").ToString());

                    ScriptManager.RegisterStartupScript(this
                        , this.GetType()
                        , Format("ReturnFromProcessSaveAsDraftResponse{0}", Guid.NewGuid().ToString())
                        , "ProcessSaveAsDraftResponse();", true);
                }
                //
                // Save in session
                Session.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityEmployeeSocioEconomicCard, surveyAnswers);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, newEx.Message);
                }
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this
                    , this.GetType()
                    , String.Format("ReturnLoadSurveyAnswers{0}", Guid.NewGuid().ToString())
                    , "setTimeout(function () { RestoreSelectedHousingDistributions(); }, 200);", true);
            }
        }
    }
}