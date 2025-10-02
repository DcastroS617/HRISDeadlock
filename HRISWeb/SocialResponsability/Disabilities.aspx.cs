using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using Unity.Web;
using static System.String;

namespace HRISWeb.SocialResponsability
{
    public partial class Disabilities : System.Web.UI.Page
    {
        private List<FamiliarEntity> familyMembers;
        private List<SurveyAnswerEntity> employeeSavedAnswers = null;

        [Dependency]
        protected IFamilyRelationshipsBll<FamilyRelationshipEntity> objFamilyRelationshipsBll { get; set; }

        [Dependency]
        protected IDisabilityTypesBll<DisabilityTypeEntity> objDisabilityTypesBll { get; set; }

        [Dependency]
        protected IOtherDiseasesBll<OtherDiseaseEntity> objOtherDiseasesBll { get; set; }

        [Dependency]
        protected ISurveysBll<SurveyEntity> objSurveysBll { get; set; }

        [Dependency]
        protected ISurveyAnswersBll<SurveyAnswerEntity> objSurveyAnswersBll { get; set; }

        [Dependency]
        protected ISurveyQuestionsBll<SurveyQuestionEntity> objSurveyQuestionsBll { get; set; }

        private const string surveyVersionName = "SurveyVersion";

        private int SurveyVersion;
        private int numberOfMenFamily;
        private int numberOfWomenFamily;

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
                cboNumberFamilyMembersWithDisabilities.Items.Add(new ListItem(Empty, "-1"));

                cboNumberFamilyMembersWithDisabilities.Items.AddRange(Enumerable.Range(1, GetTotalPeopleLivingWithEmployee()).Select(n => new ListItem(n.ToString(), n.ToString())).ToArray());
                cboNumberFamilyMembersWithDisabilities.Enabled = false;
                //
                LoadSurveyAnswers();
                //
                rptDiseaseByFamilyMembers.DataSource = GetOtherDiseases();
                rptDiseaseByFamilyMembers.DataBind();
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
            Response.Redirect("FamilyWelfare.aspx", false);
        }

        /// <summary>
        /// Handles the lbtnNext click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            SaveSurveyAnswers(false);
            Response.Redirect("DiseasesFrequency.aspx", false);
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
        /// Handles the cboNumberFamilyMembersWithDisabilities select index change event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void cboNumberFamilyMembersWithDisabilities_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboNumberFamilyMembersWithDisabilities.Enabled = true;
            FillFamiliarsWithDisabilities();

            ScriptManager.RegisterStartupScript(this
                , this.GetType()
                , "SetChronicDiseaseTextBoxConfiguration" + Guid.NewGuid().ToString()
                , "SetChronicDiseaseTextBoxConfiguration();", true);
        }

        /// <summary>
        /// Handles the rptMembersWithDisabilities item data bound event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void rptMembersWithDisabilities_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SurveyAnswerEntity answer = null;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                List<SurveyAnswerEntity> answers = GetsLocalSurveyAnswer();

                byte memberId = 0;
                Label lblMemberId = e.Item.FindControl("lblFamilyMemberId") as Label;
                memberId = lblMemberId != null ? Convert.ToByte(lblMemberId.Text) : (byte)0;

                DropDownList cboRelationshipItem = e.Item.FindControl("cboFamilyRelationship") as DropDownList;
                if (cboRelationshipItem != null)
                {
                    cboRelationshipItem.Items.AddRange(GetsFamilyRelationships().ToArray());
                    answer = answers.Find(a => a.QuestionID.Equals("26.1.a") && a.AnswerItem.Equals(memberId));
                    ListItem liRelationship = answer != null ? cboRelationshipItem.Items.FindByValue(answer.AnswerValue) : null;
                    if (liRelationship != null)
                    {
                        liRelationship.Selected = true;
                    }
                }
                //
                DropDownList cboMemberDisabilyTypeItem = e.Item.FindControl("cboMemberDisabilityType") as DropDownList;
                if (cboMemberDisabilyTypeItem != null)
                {
                    cboMemberDisabilyTypeItem.Items.AddRange(GetsDisabilityTypes().ToArray());
                    answer = answers.Find(a => a.QuestionID.Equals("26.1.b") && a.AnswerItem.Equals(memberId));
                    ListItem liDisability = answer != null ? cboMemberDisabilyTypeItem.Items.FindByValue(answer.AnswerValue) : null;
                    if (liDisability != null)
                    {
                        liDisability.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the chkFamilyDisability cheched change event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void chkFamilyDisability_CheckedChanged(object sender, EventArgs e)
        {
            cboNumberFamilyMembersWithDisabilities.SelectedIndex = -1;
            cboNumberFamilyMembersWithDisabilities.Enabled = chkFamilyDisability.Checked;

            if (!chkFamilyDisability.Checked)
            {
                rptMembersWithDisabilities.DataSource = new List<FamiliarEntity>();
                rptMembersWithDisabilities.DataBind();
            }

            ScriptManager.RegisterStartupScript(this
                , this.GetType()
                , "SetChronicDiseaseTextBoxConfiguration" + Guid.NewGuid().ToString()
                , "SetChronicDiseaseTextBoxConfiguration();", true);
        }

        /// <summary>
        /// Handles the rptDiseaseByFamilyMembers item data bound event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void rptDiseaseByFamilyMembers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                List<SurveyAnswerEntity> answers = GetsLocalSurveyAnswer();
                if (answers != null)
                {
                    SurveyAnswerEntity answer = null;
                    byte answerItem = 0;

                    Label lblDisease = e.Item.FindControl("lblDiseaseCode") as Label;
                    answer = lblDisease != null ? answers.Find(a => a.QuestionID.Equals("27.a") && a.AnswerValue.Equals(lblDisease.Text.Trim())) : null;
                    answerItem = answer != null ? answer.AnswerItem : (byte)0;

                    DropDownList cboMenNumberItem = e.Item.FindControl("cboMenNumber") as DropDownList;
                    if (cboMenNumberItem != null)
                    {
                        cboMenNumberItem.Items.Add(new ListItem("0", "0"));
                        cboMenNumberItem.Items.AddRange(Enumerable.Range(1, numberOfMenFamily).Select(n => new ListItem(n.ToString(), n.ToString())).ToArray());
                        answer = answers.Find(a => a.QuestionID.Equals("27.b") && a.AnswerItem.Equals(answerItem));
                        ListItem liNumberMen = answer != null ? cboMenNumberItem.Items.FindByValue(answer.AnswerValue) : null;
                        if (liNumberMen != null)
                        {
                            liNumberMen.Selected = true;
                        }
                    }
                    //
                    DropDownList cboWomenNumberItem = e.Item.FindControl("cboWomenNumber") as DropDownList;
                    if (cboWomenNumberItem != null)
                    {
                        cboWomenNumberItem.Items.Add(new ListItem("0", "0"));
                        cboWomenNumberItem.Items.AddRange(Enumerable.Range(1, numberOfWomenFamily).Select(n => new ListItem(n.ToString(), n.ToString())).ToArray());
                        answer = answers.Find(a => a.QuestionID.Equals("27.c") && a.AnswerItem.Equals(answerItem));
                        ListItem liNumberWomen = answer != null ? cboWomenNumberItem.Items.FindByValue(answer.AnswerValue) : null;
                        if (liNumberWomen != null)
                        {
                            liNumberWomen.Selected = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the total people living with the employee according to the response value of the questions 20 and 21
        /// </summary>
        /// <returns>The total people living with the employee</returns>
        private int GetTotalPeopleLivingWithEmployee()
        {
            int numberOfMen = 0, numberOfWomen = 0;

            try
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();

                if (surveyAnswers != null && surveyAnswers.Item2.Any())
                {
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("14.a"));
                    numberOfMen += answer != null ? Convert.ToInt32(answer.AnswerValue) : 0;
                    //
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("14.b"));
                    numberOfWomen += answer != null ? Convert.ToInt32(answer.AnswerValue) : 0;

                    numberOfMenFamily = numberOfMen;
                    numberOfWomenFamily = numberOfWomen;

                    if (surveyAnswers.Item1.Gender.StartsWith("M"))
                    {
                        numberOfMenFamily += 1;
                    }

                    else
                    {
                        numberOfWomenFamily += 1;
                    }
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

            return numberOfMen + numberOfWomen;
        }

        /// <summary>
        /// Fill the family relationships
        /// </summary>
        /// <returns>A list of family relationships</returns>
        private List<ListItem> GetsFamilyRelationships()
        {
            List<ListItem> familyRelationship = new List<ListItem>();
            try
            {
                List<FamilyRelationshipEntity> familyRelationshipsBDList = new List<FamilyRelationshipEntity>();
                int totalPeopleLivingWithEmployee = GetTotalPeopleLivingWithEmployee()+1;
                int contador = 1;
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                SurveyAnswerEntity answer = null;

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogFamilyRelationships] != null)
                {
                    familyRelationshipsBDList = (List<FamilyRelationshipEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogFamilyRelationships];
                }

                else
                {
                    objFamilyRelationshipsBll = objFamilyRelationshipsBll ?? Application.GetContainer().Resolve<IFamilyRelationshipsBll<FamilyRelationshipEntity>>();
                    familyRelationshipsBDList = objFamilyRelationshipsBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();
                familyRelationship.Add(new ListItem(Empty, "-1"));

                while (contador < totalPeopleLivingWithEmployee) {
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("20.1.a") && a.AnswerItem.Equals(Convert.ToByte(contador)));
                    familyRelationship.AddRange(familyRelationshipsBDList.Where(x => x.FamilyRelationshipCode == Convert.ToByte(answer.AnswerValue.Trim())).
                        Select(n => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR) ? n.FamilyRelationshipDescriptionSpanish : n.FamilyRelationshipDescriptionEnglish, n.FamilyRelationshipCode.ToString())));
                    contador++;
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

            return familyRelationship;
        }

        /// <summary>
        /// Gets the Disability Types
        /// </summary>
        /// <returns>A list with the disability types</returns>
        private List<ListItem> GetsDisabilityTypes()
        {
            List<ListItem> disabilyTypes = new List<ListItem>();
            try
            {
                List<DisabilityTypeEntity> disabilityTypesBDList = new List<DisabilityTypeEntity>();

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDisabilityTypes] != null)
                {
                    disabilityTypesBDList = (List<DisabilityTypeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDisabilityTypes];
                }

                else
                {
                    objDisabilityTypesBll = objDisabilityTypesBll ?? Application.GetContainer().Resolve<IDisabilityTypesBll<DisabilityTypeEntity>>();
                    disabilityTypesBDList = objDisabilityTypesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                disabilyTypes.Add(new ListItem(Empty, "-1"));
                disabilyTypes.AddRange(disabilityTypesBDList.OrderBy(o => o.DisabilityTypeCode)
                    .Select(f => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? f.DisabilityTypeDescriptionSpanish : f.DisabilityTypeDescriptionEnglish
                    , f.DisabilityTypeCode.ToString())));
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

            return disabilyTypes;
        }

        /// <summary>
        /// Fills the repeater with the number of family members with disabilities
        /// </summary>
        private void FillFamiliarsWithDisabilities()
        {
            familyMembers = new List<FamiliarEntity>();
            ListItem liFamilyNumber = cboNumberFamilyMembersWithDisabilities.SelectedItem;

            if (liFamilyNumber != null && !IsNullOrWhiteSpace(liFamilyNumber.Value) && !liFamilyNumber.Value.Equals("-1"))
            {
                int familyMembersWithDisabilities = Convert.ToInt32(cboNumberFamilyMembersWithDisabilities.SelectedValue);
                familyMembers.AddRange(Enumerable.Range(1, familyMembersWithDisabilities).Select(n => new FamiliarEntity(n)));
            }

            rptMembersWithDisabilities.DataSource = familyMembers;
            rptMembersWithDisabilities.DataBind();
        }

        /// <summary>
        /// Gets the enabled other diseases
        /// </summary>
        private List<OtherDiseaseEntity> GetOtherDiseases()
        {
            List<OtherDiseaseEntity> otherDiseasesBDList = new List<OtherDiseaseEntity>();

            try
            {
                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogOtherDiseases] != null)
                {
                    otherDiseasesBDList = (List<OtherDiseaseEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogOtherDiseases];
                }
                else
                {
                    objOtherDiseasesBll = objOtherDiseasesBll ?? Application.GetContainer().Resolve<IOtherDiseasesBll<OtherDiseaseEntity>>();
                    otherDiseasesBDList = objOtherDiseasesBll.ListEnabled();
                }

                CultureInfo currentCulture = GetCurrentCulture();

                otherDiseasesBDList.ForEach(d => d.CurrentCulture = currentCulture.Name);
                otherDiseasesBDList = currentCulture.Name.Equals(Constants.cCultureEsCR) ? otherDiseasesBDList.OrderBy(f => f.OtherDiseaseDescriptionSpanish).ToList() : otherDiseasesBDList.OrderBy(f => f.OtherDiseaseDescriptionEnglish).ToList();
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

            return otherDiseasesBDList;
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

                if (!SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode.Equals(currentSurvey.Item1.DivisionCode))
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
        /// Gets the employee answers
        /// </summary>
        /// <returns>The employee answers</returns>
        private List<SurveyAnswerEntity> GetsLocalSurveyAnswer()
        {
            if (employeeSavedAnswers == null)
            {
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> currentSurvey = GetSurveyAnswers();
                employeeSavedAnswers = currentSurvey != null ? currentSurvey.Item2 : null;
            }

            return employeeSavedAnswers;
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
                    SurveyAnswerEntity answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("26.a"));
                    chkFamilyDisability.Checked = answer != null ? Convert.ToBoolean(answer.AnswerValue) : false;
                    if (chkFamilyDisability.Checked)
                    {
                        cboNumberFamilyMembersWithDisabilities.Attributes.Remove("disabled");
                        cboNumberFamilyMembersWithDisabilities.Enabled = true;
                    }
                    
                    //
                    cboNumberFamilyMembersWithDisabilities.SelectedIndex = -1;
                    answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("26.b"));
                    
                    ListItem liNumberFamilyMembersWithDisabilities = answer != null ? cboNumberFamilyMembersWithDisabilities.Items.FindByValue(answer.AnswerValue.Trim()) : null;
                    if (liNumberFamilyMembersWithDisabilities != null)
                    {
                        liNumberFamilyMembersWithDisabilities.Selected = true;
                    }

                    //
                    employeeSavedAnswers = surveyAnswers.Item2;
                    FillFamiliarsWithDisabilities();
                    //

                    //answer = surveyAnswers.Item2.Find(a => a.QuestionID.Equals("27.d"));
                    //txtChronicDisease.Text = answer != null ? answer.AnswerValue : Empty;
                    //if (answer != null)
                    //{
                    //    txtChronicDisease.Attributes.Remove("disabled");
                    //}
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

            //finally
            //{
            //    ScriptManager.RegisterStartupScript(this
            //        , this.GetType()
            //        , "SetChronicDiseaseTextBoxConfiguration" + Guid.NewGuid().ToString()
            //        , "SetChronicDiseaseTextBoxConfiguration();", true);
            //}
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
                bool hasFamilyMembersWithDisabilities = chkFamilyDisability.Checked;
                string numberFamilyMembersWithDisabilities = cboNumberFamilyMembersWithDisabilities.SelectedValue;
                //string chronicDisease = txtChronicDisease.Text.Trim();
                
                //
                List<Tuple<string, byte, string, int>> employeeAnswers = new List<Tuple<string, byte, string, int>>();
                employeeAnswers.Add(new Tuple<string, byte, string, int>("26.a", 1, hasFamilyMembersWithDisabilities.ToString(), 4));
                if (!IsNullOrWhiteSpace(numberFamilyMembersWithDisabilities) && !numberFamilyMembersWithDisabilities.Equals("-1"))
                {
                    employeeAnswers.Add(new Tuple<string, byte, string, int>("26.b", 1, numberFamilyMembersWithDisabilities, 4));
                    cboNumberFamilyMembersWithDisabilities.Attributes.Remove("disabled");
                }
                
                //
                if (hasFamilyMembersWithDisabilities)
                {
                    foreach (RepeaterItem item in rptMembersWithDisabilities.Items)
                    {
                        if (item.ItemType.Equals(ListItemType.Item) || item.ItemType.Equals(ListItemType.AlternatingItem))
                        {
                            byte familiarId = 0;

                            Label lblFamiliarId = item.FindControl("lblFamilyMemberId") as Label;
                            DropDownList cboRelationship = item.FindControl("cboFamilyRelationship") as DropDownList;
                            DropDownList cboDisabilityType = item.FindControl("cboMemberDisabilityType") as DropDownList;

                            if (lblFamiliarId != null && cboRelationship != null && cboDisabilityType != null)
                            {
                                familiarId = Convert.ToByte(lblFamiliarId.Text);
                                string relationship = cboRelationship.SelectedValue;
                                string disability = cboDisabilityType.SelectedValue;

                                if (!IsNullOrWhiteSpace(relationship) && !relationship.Equals("-1"))
                                {
                                    employeeAnswers.Add(new Tuple<string, byte, string, int>("26.1.a", familiarId, relationship, 4));
                                }

                                if (!IsNullOrWhiteSpace(disability) && !disability.Equals("-1"))
                                {
                                    employeeAnswers.Add(new Tuple<string, byte, string, int>("26.1.b", familiarId, disability, 4));
                                }
                            }
                        }
                    }
                }

                //
                byte diseaseOrder = 0;
                foreach (RepeaterItem item in rptDiseaseByFamilyMembers.Items)
                {
                    if (item.ItemType.Equals(ListItemType.Item) || item.ItemType.Equals(ListItemType.AlternatingItem))
                    {
                        Label lblOtherDiseaseCode = item.FindControl("lblDiseaseCode") as Label;
                        DropDownList cboMen = item.FindControl("cboMenNumber") as DropDownList;
                        DropDownList cboWomen = item.FindControl("cboWomenNumber") as DropDownList;

                        if (lblOtherDiseaseCode != null && cboMen != null && cboWomen != null)
                        {
                            diseaseOrder++;
                            employeeAnswers.Add(new Tuple<string, byte, string, int>("27.a", diseaseOrder, lblOtherDiseaseCode.Text.Trim(), 4));
                            string numberOfMen = cboMen.SelectedValue;
                            string numberOfWomen = cboWomen.SelectedValue;

                            if (!IsNullOrWhiteSpace(numberOfMen) && !numberOfMen.Equals("-1"))
                            {
                                employeeAnswers.Add(new Tuple<string, byte, string, int>("27.b", diseaseOrder, numberOfMen, 4));
                            }

                            if (!IsNullOrWhiteSpace(numberOfWomen) && !numberOfWomen.Equals("-1"))
                            {
                                employeeAnswers.Add(new Tuple<string, byte, string, int>("27.c", diseaseOrder, numberOfWomen, 4));
                            }
                        }
                    }
                }

                //if (!IsNullOrWhiteSpace(chronicDisease))
                //{
                //    employeeAnswers.Add(new Tuple<string, byte, string, int>("27.d", 1, chronicDisease, 4));
                //    txtChronicDisease.Attributes.Remove("disabled");
                //}
                //else
                //{
                //    employeeAnswers.Add(new Tuple<string, byte, string, int>("27.d", 1, "", 4));
                //}
                
                // Insert or edit answers values
                Tuple<SurveyEntity, List<SurveyAnswerEntity>> surveyAnswers = GetSurveyAnswers();
                
                List<SurveyQuestionEntity> surveyQuestions = GetSurveyQuestions();
                string currentUser = UserHelper.GetCurrentFullUserName;
                if (surveyAnswers.Item2 != null)
                {
                    surveyAnswers.Item2.ForEach(a => a.LastModifiedUser = currentUser);
                }

                if (!hasFamilyMembersWithDisabilities)
                {
                    surveyAnswers.Item2.RemoveAll(a => a.QuestionID.Contains("26.b"));
                }

                surveyAnswers.Item2.RemoveAll(a => a.QuestionID.Contains("26.1"));
                surveyAnswers.Item2.RemoveAll(a => a.QuestionID.Contains("27."));

                foreach (var item in employeeAnswers)
                {
                    var answer = surveyAnswers.Item2.Find(v => v.QuestionID.Equals(item.Item1) && v.AnswerItem.Equals(item.Item2));
                    if (answer != null)
                    {
                        answer.AnswerValue = item.Item3;
                        answer.LastModifiedUser = currentUser;
                        answer.SurveyVersion = this.SurveyVersion;
                        answer.IdSurveyModule = 4;
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
                            , item.Item4)
                        { SurveyVersion = this.SurveyVersion });
                    }
                }                
                
                // Save current state for the page                
                surveyAnswers.Item1.SurveyStateCode = Convert.ToByte(HrisEnum.SurveyStates.Draft);
                surveyAnswers.Item1.SurveyCurrentPageCode = Convert.ToByte(HrisEnum.SurveyPages.Disabilities);
                surveyAnswers.Item1.SurveyCompletedBy = currentUser;
                surveyAnswers.Item1.LastModifiedUser = currentUser;
                
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
                        , "ReturnFromProcessSaveAsDraftResponse" + Guid.NewGuid().ToString()
                        , "ProcessSaveAsDraftResponse();", true);
                }
                
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
                    , "SetChronicDiseaseTextBoxConfiguration" + Guid.NewGuid().ToString()
                    , "SetChronicDiseaseTextBoxConfiguration();", true);
            }
        }
    }
}