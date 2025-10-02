using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using HRISWeb.Training.Maintenances;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using Unity.Web;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.Training
{
    public partial class Courses : Page
    {
        [Dependency]
        public ICoursesBll<CourseEntity> ObjCoursesBll { get; set; }

        [Dependency]
        public ITrainersBll<TrainerEntity> ObjTrainersBll { get; set; }

        [Dependency]
        public IPaymentRatesBll<PaymentRateEntity> ObjPaymentRatesBll { get; set; }

        [Dependency]
        public IPositionsBll<PositionEntity> ObjPositionsBll { get; set; }

        [Dependency]
        public ITrainingProgramsBll<TrainingProgramEntity> ObjTrainingProgramsBll { get; set; }

        [Dependency]
        public IThematicAreasBll<ThematicAreaEntity> ObjThematicAreasBll { get; set; }

        [Dependency]
        public ITypeTrainingBll ObjTypeTrainingBll { get; set; }

        [Dependency]
        public ISchoolTrainingBll ObjSchoolTrainingBll { get; set; }

        //session key for the results
        readonly string sessionKeyCoursesResults = "Courses-CoursesResults";
        readonly string sessionKeyTrainersAjustResults = "Courses-CoursesTrainersAjustResults";
        readonly string sessionKeyAssociatedTrainers = "Courses-CoursesTrainersAssociated";
        readonly string sessionKeyTrainersResults = "Courses-CoursesTrainersResults";
        readonly string sessionKeyAssociatedThematicAreas = "Courses-CoursesThematicAreasAssociated";
        readonly string sessionKeyThematicAreasResults = "Courses-CoursesThematicAreasResults";
        readonly string sessionKeyAssociatedPositions = "Courses-CoursesPositionsAssociated";
        readonly string sessionKeyPositionsResults = "Courses-CoursesPositionsResults";
        readonly string sessionKeyAssociatedTrainingPrograms = "Courses-CoursesTrainingProgramsAssociated";
        readonly string sessionKeyTrainingProgramsResults = "Courses-CoursesTrainingProgramsResults";
        readonly string sessionKeyAssociatedSchoolTraining = "Courses-CoursesSchoolTrainingAssociated";
        readonly string sessionKeySchoolTrainingResults = "Courses-CoursesSchoolTrainingResults";

        #region Events

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
            try
            {
                if (!IsPostBack)
                {
                    Session[sessionKeyTrainersResults] = null;
                    Session[sessionKeyTrainersAjustResults] = null;
                    Session[sessionKeyThematicAreasResults] = null;
                    Session[sessionKeyPositionsResults] = null;
                    Session[sessionKeyTrainingProgramsResults] = null;

                    TypeTrainingLoadDll();

                    cboTrainerType.Enabled = true;
                    cboTrainerType.DataValueField = "Key";
                    cboTrainerType.DataTextField = "Value";
                    cboTrainerType.DataSource = LoadtrainerTypes();
                    cboTrainerType.DataBind();
                    cboTrainerType.SelectedIndex = 0;

                    cboCourseState.Items.Add(new ListItem() { Value = "", Text = "" });
                    cboCourseState.Items.Add(new ListItem() { Value = "1", Text = GetLocalResourceObject("Enable").ToString() });
                    cboCourseState.Items.Add(new ListItem() { Value = "0", Text = GetLocalResourceObject("Disable").ToString() });

                    //fire the event
                    BtnSearch_ServerClick(sender, e);

                    hdfDivisionCode.Value = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode.ToString();
                    hdfGeographicDivisionCode.Value = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                }

                chbSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chbSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));

                //activate the pager
                if (Session[sessionKeyCoursesResults] != null)
                {
                    PageHelper<CourseEntity> pageHelper = (PageHelper<CourseEntity>)Session[sessionKeyCoursesResults];
                    PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string courseCode = !string.IsNullOrWhiteSpace(hdfCourseCodeEdit.Value) ? hdfCourseCodeEdit.Value : txtCourseCode.Text.Trim();

                CourseEntity course = new CourseEntity(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    courseCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    txtCourseName.Text.Trim(),
                    Convert.ToDecimal(txtCourseCostByParticipant.Text.Trim().Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)),
                    Convert.ToDecimal(txtCourseDuration.Text.Trim().Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)),
                    chbSearchEnabled.Checked,
                    false,
                    UserHelper.GetCurrentFullUserName,
                    DateTime.Now)
                {
                    TypeTrainingId = int.Parse(txtTypeTrainingEdit.Value),
                    ForMatrix = chbForMatrix.Checked,
                    NoteRequired = chbNoteRequired.Checked,
                    CyclesRefreshment = chbCyclesRefreshment.Checked,
                    MaxDaysTrain = txtMaxDaysTrainEdit.Text.ToInt32Null(),
                    DaysRenewCourse = txtDaysRenewCourseEdit.Text.ToInt32Null(),
                    ExternalCourse = chbExternalCourse.Checked
                };

                PageHelper<CourseEntity> pageHelper = (PageHelper<CourseEntity>)Session[sessionKeyCoursesResults];

                if (string.IsNullOrWhiteSpace(hdfCourseCodeEdit.Value))
                {
                    Tuple<bool, CourseEntity> addResult = ObjCoursesBll.Add(course);
                    if (addResult.Item1)
                    {
                        if (pageHelper != null)
                        {
                            SearchResults(pageHelper.CurrentPage);
                            DisplayResults();
                        }

                        hdfSelectedRowIndex.Value = "-1";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack();", true);
                    }

                    else if (!addResult.Item1)
                    {
                        CourseEntity previousCourse = addResult.Item2;

                        if (previousCourse.Deleted)
                        {
                            txtActivateDeletedCourseCode.Text = previousCourse.CourseCode;
                            txtActivateDeletedCourseName.Text = previousCourse.CourseName;

                            hdfActivateDeletedCourseCode.Value = previousCourse.CourseCode;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDeleted();", true);
                        }

                        else
                        {
                            txtDuplicatedCourseCode.Text = previousCourse.CourseCode;
                            txtDuplicatedCourseName.Text = previousCourse.CourseName;

                            pnlDuplicatedDialogDataDetail.Visible = true;

                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    var result = ObjCoursesBll.Edit(course);
                    if (result.Item1)
                    {
                        if (pageHelper != null)
                        {
                            SearchResults(pageHelper.CurrentPage);
                            DisplayResults();
                        }

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack();", true);
                    }

                    else
                    {
                        var previousCourse = result.Item2;

                        txtDuplicatedCourseCode.Text = previousCourse.CourseCode;
                        txtDuplicatedCourseName.Text = previousCourse.CourseName;
                        pnlDuplicatedDialogDataDetail.Visible = false;

                        divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDuplicated(); ", true);
                    }
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                     || ex is BusinessException
                     || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnActivateDeletedAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">C
        protected void BtnActivateDeletedAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string courseCode = !string.IsNullOrWhiteSpace(hdfActivateDeletedCourseCode.Value) ? hdfActivateDeletedCourseCode.Value : null;

                CourseEntity course = new CourseEntity(
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                        courseCode,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                        txtCourseName.Text.Trim(),
                        Convert.ToDecimal(txtCourseCostByParticipant.Text.Trim().Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)),
                        Convert.ToDecimal(txtCourseDuration.Text.Trim().Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)),
                        chbSearchEnabled.Checked,
                        true,
                        UserHelper.GetCurrentFullUserName,
                        DateTime.Now)
                {
                    TypeTrainingId = int.Parse(txtTypeTrainingEdit.Value),
                    ForMatrix = chbForMatrix.Checked,
                    NoteRequired = chbNoteRequired.Checked,
                    CyclesRefreshment = chbCyclesRefreshment.Checked,
                    MaxDaysTrain = txtMaxDaysTrainEdit.Text.ToInt32Null(),
                    DaysRenewCourse = txtDaysRenewCourseEdit.Text.ToInt32Null(),
                    ExternalCourse = chbExternalCourse.Checked
                };

                PageHelper<CourseEntity> pageHelper = (PageHelper<CourseEntity>)Session[sessionKeyCoursesResults];

                //activate the deleted item
                if (chbActivateDeleted.Checked)
                {
                    ObjCoursesBll.Activate(course);

                    if (pageHelper != null)
                    {
                        SearchResults(pageHelper.CurrentPage);
                        DisplayResults();
                    }

                    hdfSelectedRowIndex.Value = "0";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                }

                //update and activate the deleted item
                else
                {
                    course.Deleted = false;

                    var result = ObjCoursesBll.Edit(course);
                    if (result.Item1)
                    {

                        if (pageHelper != null)
                        {
                            string position = hdfSelectedRowIndex.Value;

                            SearchResults(pageHelper.CurrentPage);
                            DisplayResults();

                            hdfSelectedRowIndex.Value = position;
                        }

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                    }

                    else
                    {
                        if (result.Item2.Deleted)
                        {
                            MensajeriaHelper.MostrarMensaje(Page,
                                TipoMensaje.Error,
                                Convert.ToString(GetLocalResourceObject("lblTextActivateDeletedDialog")));
                        }

                        else
                        {
                            MensajeriaHelper.MostrarMensaje(Page,
                                TipoMensaje.Error,
                                Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialogNoDetailsError")));
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    CourseEntity course = ObjCoursesBll.ListByKey(
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                        selectedCourseCode,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

                    hdfCourseCodeEdit.Value = course.CourseCode;
                    txtCourseCode.Text = course.CourseCode;
                    txtCourseCode.Enabled = false;
                    txtCourseName.Text = course.CourseName;
                    txtCourseCostByParticipant.Text = Convert.ToString(course.CourseCostByParticipant).Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
                    txtCourseDuration.Text = Convert.ToString(course.CourseDuration).Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
                    chbSearchEnabled.Checked = course.SearchEnabled;

                    txtTypeTrainingEdit.Value = course.TypeTrainingId?.ToString() ?? "";
                    chbForMatrix.Checked = course.ForMatrix;
                    chbNoteRequired.Checked = course.NoteRequired;
                    chbCyclesRefreshment.Checked = course.CyclesRefreshment;
                    txtMaxDaysTrainEdit.Text = course.MaxDaysTrain?.ToString();
                    txtDaysRenewCourseEdit.Text = course.DaysRenewCourse?.ToString();
                    chbExternalCourse.Checked = course.ExternalCourse;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        #region Training Programs

        /// <summary>
        /// Handles the btnRefreshAssociatedTrainingPrograms click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRefreshAssociatedTrainingPrograms_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshAssociatedTrainingPrograms();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppAssociatedTrainingPrograms
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppAssociatedTrainingPrograms
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnEditTrainingProgram click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEditTrainingPrograms_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    LoadAssociatedTrainingPrograms(true);
                    List<TrainingProgramEntity> associatedTrainingPrograms = (List<TrainingProgramEntity>)Session[sessionKeyAssociatedTrainingPrograms];

                    rptAssociatedTrainingPrograms.DataSource = associatedTrainingPrograms;
                    rptAssociatedTrainingPrograms.DataBind();
                    uppAssociatedTrainingPrograms.Update();

                    txtSearchTrainingPrograms.Text = string.Empty;
                    uppSearchBarTrainingProgram.Update();

                    txtSearchTrainingPrograms.Text = string.Empty;
                    rptTrainingPrograms.DataSource = null;
                    rptTrainingPrograms.DataBind();
                    uppSearchTrainingPrograms.Update();

                    lblSearchTrainingProgramsResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchTrainingProgramsResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchTrainingProgramsResultsCount")), 0, 0));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditTrainingProgramsClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditTrainingProgramsClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                }

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
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
        /// Handles the txtSearchTrainingPrograms text changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void TxtSearchTrainingPrograms_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FilterSearchedTrainingPrograms();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppSearchTrainingPrograms
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppSearchTrainingPrograms
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnDeleteAssociatedTrainingProgram click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDeleteAssociatedTrainingProgram_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string trainingProgramCode = ((TextBox)htmlButton.Parent.FindControl("hdfAssociatedTrainingProgramCode")).Text;

                    LoadAssociatedTrainingPrograms(false);
                    List<TrainingProgramEntity> associatedTrainingPrograms = (List<TrainingProgramEntity>)Session[sessionKeyAssociatedTrainingPrograms];

                    List<TrainingProgramEntity> removedTrainingPrograms = associatedTrainingPrograms.Where(at => string.Equals(trainingProgramCode, at.TrainingProgramCode)).ToList();

                    if (removedTrainingPrograms.Count >= 1)
                    {
                        ObjCoursesBll.DeleteCourseByTrainingProgram(
                            new CourseEntity(
                                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                                selectedCourseCode,
                                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                                UserHelper.GetCurrentFullUserName),
                            removedTrainingPrograms[0]
                        );

                        associatedTrainingPrograms.RemoveAll(at => string.Equals(trainingProgramCode, at.TrainingProgramCode));

                        rptAssociatedTrainingPrograms.DataSource = associatedTrainingPrograms;
                        rptAssociatedTrainingPrograms.DataBind();

                        FilterSearchedTrainingPrograms();

                        uppAssociatedTrainingPrograms.Update();
                        uppSearchTrainingPrograms.Update();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteTrainingProgramClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnDeleteTrainingProgramClickPostBack('{0}'); }}, 200);", btnDelete.ClientID), true);
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnAddTrainingProgram click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddTrainingProgram_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string trainingProgramCode = ((TextBox)htmlButton.Parent.FindControl("hdfTrainingProgramCode")).Text;

                    LoadTrainingPrograms();
                    DataTable trainingPrograms = (DataTable)Session[sessionKeyTrainingProgramsResults];

                    LoadAssociatedTrainingPrograms(false);
                    List<TrainingProgramEntity> associatedTrainingPrograms = (List<TrainingProgramEntity>)Session[sessionKeyAssociatedTrainingPrograms];

                    if (associatedTrainingPrograms.Count == 0 || associatedTrainingPrograms == null)
                    {
                        List<TrainingProgramEntity> addedTrainingProgram = trainingPrograms.AsEnumerable().Where(t => string.Equals(trainingProgramCode, t.Field<string>("TrainingProgramCode"))).Select(t => new TrainingProgramEntity
                         (
                             SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                             t.Field<string>("TrainingProgramCode"),
                             SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                             t.Field<string>("TrainingProgramName"))
                         ).ToList();

                        if (addedTrainingProgram.Count >= 1)
                        {
                            CourseEntity curso = new CourseEntity(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, selectedCourseCode, UserHelper.GetCurrentFullUserName);
                            curso.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                            ObjCoursesBll.AddCourseByTrainingProgram(
                                curso,
                                addedTrainingProgram[0]
                            );

                            associatedTrainingPrograms.Add(addedTrainingProgram[0]);

                            rptAssociatedTrainingPrograms.DataSource = associatedTrainingPrograms;
                            rptAssociatedTrainingPrograms.DataBind();

                            FilterSearchedTrainingPrograms();

                            uppAssociatedTrainingPrograms.Update();
                            uppSearchTrainingPrograms.Update();

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAddTrainingProgramClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnAddTrainingProgramClickPostBack('{0}'); }}, 200);", btnAdd.ClientID), true);
                        }
                        else
                        {
                            MensajeriaHelper.MostrarMensaje((Control)sender
                              , TipoMensaje.Error
                              , GetLocalResourceObject("msgInvalidSelection").ToString());
                        }
                    }
                    else
                    {
                        MensajeriaHelper.MostrarMensaje((Control)sender
                           , TipoMensaje.Error
                           , GetLocalResourceObject("msgAlertPurpose").ToString());
                    }

                }

            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        #endregion

        #region Trainers

        /// <summary>
        /// Handles the btnRefreshAssociatedTrainers click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRefreshAssociatedTrainers_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshAssociatedTrainers();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppAssociatedTrainers
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppAssociatedTrainers
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEditTrainers_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    LoadAssociatedTrainers(true);
                    List<TrainerEntity> associatedTrainers = (List<TrainerEntity>)Session[sessionKeyAssociatedTrainers];

                    rptAssociatedTrainers.DataSource = associatedTrainers;
                    rptAssociatedTrainers.DataBind();
                    uppAssociatedTrainers.Update();

                    txtSearchTrainers.Text = string.Empty;
                    uppSearchBarTrainer.Update();
                    txtSearchTrainers.Text = string.Empty;

                    AjustTrainers();
                    DataTable trainers = (DataTable)Session[sessionKeyTrainersAjustResults];

                    rptTrainers.DataSource = trainers;
                    rptTrainers.DataBind();
                    uppSearchTrainers.Update();

                    cboTrainerType.SelectedValue = "-1";

                    int minRowCount = Math.Min(1, trainers.DefaultView.Count);

                    lblSearchTrainersResults.InnerHtml = string.Format("{0} {1}",
                        Convert.ToString(GetLocalResourceObject("lblSearchTrainersResults")),
                        string.Format(Convert.ToString(GetLocalResourceObject("lblSearchTrainersResultsCount")), minRowCount, trainers.DefaultView.Count));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditTrainersClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditTrainersClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                }

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
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
        /// Handles the cboTrainerType text changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CboTrainerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FilterSearchedTrainers();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppSearchTrainers
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppSearchTrainers
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the txtSearchTrainers text changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void TxtSearchTrainers_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FilterSearchedTrainers();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppSearchTrainers
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppSearchTrainers
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnDeleteAssociatedTrainer click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDeleteAssociatedTrainer_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string trainerCode = ((TextBox)htmlButton.Parent.FindControl("hdfAssociatedTrainerCode")).Text;
                    string trainerType = ((TextBox)htmlButton.Parent.FindControl("hdfAssociatedTrainerType")).Text;

                    LoadAssociatedTrainers(false);
                    List<TrainerEntity> associatedTrainers = (List<TrainerEntity>)Session[sessionKeyAssociatedTrainers];

                    List<TrainerEntity> removedTrainers = associatedTrainers.Where(at =>
                        string.Equals(trainerType, Convert.ToString(at.TrainerType)) &&
                        string.Equals(trainerCode, at.TrainerCode)).ToList();

                    if (removedTrainers.Count >= 1)
                    {
                        var course = new CourseEntity(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, selectedCourseCode, UserHelper.GetCurrentFullUserName);
                        course.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        ObjCoursesBll.DeleteTrainerByCourse(
                           course,
                            removedTrainers[0]
                        );

                        associatedTrainers.RemoveAll(at => string.Equals(trainerType, Convert.ToString(at.TrainerType)) && string.Equals(trainerCode, at.TrainerCode));

                        rptAssociatedTrainers.DataSource = associatedTrainers;
                        rptAssociatedTrainers.DataBind();

                        FilterSearchedTrainers();

                        uppAssociatedTrainers.Update();
                        uppSearchTrainers.Update();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteTrainerClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnDeleteTrainerClickPostBack('{0}'); }}, 200);", btnDelete.ClientID), true);
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnAddTrainer click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddTrainer_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string trainerCode = ((TextBox)htmlButton.Parent.FindControl("hdfTrainerCode")).Text;
                    string trainerType = ((TextBox)htmlButton.Parent.FindControl("hdfTrainerType")).Text;

                    LoadTrainers();
                    DataTable trainers = (DataTable)Session[sessionKeyTrainersResults];

                    LoadAssociatedTrainers(false);
                    List<TrainerEntity> associatedTrainers = (List<TrainerEntity>)Session[sessionKeyAssociatedTrainers];

                    List<TrainerEntity> addedTrainer = trainers.AsEnumerable().Where(t =>
                        string.Equals(trainerType, t.Field<string>("TrainerType").ToString()) &&
                        string.Equals(trainerCode, t.Field<string>("TrainerCode"))).Select(t => new TrainerEntity
                        (
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                            HrisEnum.ParseEnumByName<TrainerType>(t.Field<string>("TrainerType").ToString()),
                            t.Field<string>("TrainerCode"),
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                            t.Field<string>("TrainerName"))
                        ).ToList();

                    if (addedTrainer.Count >= 1)
                    {
                        var course = new CourseEntity(
                                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode
                                , selectedCourseCode
                                , UserHelper.GetCurrentFullUserName);
                        course.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                        ObjCoursesBll.AddTrainerByCourse(
                          course ,
                          addedTrainer[0]
                        );

                        associatedTrainers.Add(addedTrainer[0]);

                        rptAssociatedTrainers.DataSource = associatedTrainers;
                        rptAssociatedTrainers.DataBind();

                        FilterSearchedTrainers();

                        uppAssociatedTrainers.Update();
                        uppSearchTrainers.Update();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAddTrainerClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnAddTrainerClickPostBack('{0}'); }}, 200);", btnAdd.ClientID), true);
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender,
                        TipoMensaje.Error,
                        ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender,
                        TipoMensaje.Error,
                        newEx.Message);
                }
            }
        }

        #endregion

        #region Thematic Areas

        /// <summary>
        /// Handles the btnRefreshAssociatedThematicAreas click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRefreshAssociatedThematicAreas_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshAssociatedThematicAreas();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppAssociatedThematicAreas
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppAssociatedThematicAreas
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEditThematicAreas_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    LoadAssociatedThematicAreas(true);
                    List<ThematicAreaEntity> associatedAreasThematic = (List<ThematicAreaEntity>)Session[sessionKeyAssociatedThematicAreas];

                    rptAssociatedThematicAreas.DataSource = associatedAreasThematic;
                    rptAssociatedThematicAreas.DataBind();
                    uppAssociatedThematicAreas.Update();

                    txtSearchThematicAreas.Text = string.Empty;
                    uppSearchBarThematicArea.Update();

                    txtSearchThematicAreas.Text = string.Empty;
                    rptThematicAreas.DataSource = null;
                    rptThematicAreas.DataBind();
                    uppSearchThematicAreas.Update();
                    Session[sessionKeyThematicAreasResults] = null;
                    lblSearchThematicAreasResults.InnerHtml = string.Format("{0} {1}",

                    Convert.ToString(GetLocalResourceObject("lblSearchThematicAreasResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchThematicAreasResultsCount")), 0, 0));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditThematicAreasClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditThematicAreasClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                }

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
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
        /// Handles the txtSearchThematicAreas text changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void TxtSearchThematicAreas_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FilterSearchedThematicAreas();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppSearchThematicAreas
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppSearchThematicAreas
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnDeleteAssociatedThematicArea click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDeleteAssociatedThematicArea_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string ThematicAreaCode = Convert.ToString(((TextBox)htmlButton.Parent.FindControl("hdfAssociatedThematicAreaCode")).Text);

                    LoadAssociatedThematicAreas(false);
                    List<ThematicAreaEntity> associatedThematicAreas = (List<ThematicAreaEntity>)Session[sessionKeyAssociatedThematicAreas];

                    List<ThematicAreaEntity> removedThematicAreas = associatedThematicAreas.Where(at => ThematicAreaCode == at.ThematicAreaCode).ToList();

                    if (removedThematicAreas.Count >= 1)
                    {
                        var course = new CourseEntity(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, selectedCourseCode, UserHelper.GetCurrentFullUserName);
                        course.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        ObjThematicAreasBll.DeleteCourseByThematicArea(
                            course,
                            removedThematicAreas[0]
                        );

                        associatedThematicAreas.RemoveAll(at => ThematicAreaCode == at.ThematicAreaCode);

                        rptAssociatedThematicAreas.DataSource = associatedThematicAreas;
                        rptAssociatedThematicAreas.DataBind();

                        FilterSearchedThematicAreas();

                        uppAssociatedThematicAreas.Update();
                        uppSearchThematicAreas.Update();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteThematicAreaClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnDeleteThematicAreaClickPostBack('{0}'); }}, 200);", btnDelete.ClientID), true);
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnAddThematicArea click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddThematicArea_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string ThematicAreaCode = Convert.ToString(((TextBox)htmlButton.Parent.FindControl("hdfThematicAreaCode")).Text);

                    LoadThematicAreas();
                    DataTable thematicAreas = (DataTable)Session[sessionKeyThematicAreasResults];

                    LoadAssociatedThematicAreas(false);
                    List<ThematicAreaEntity> associatedThematicAreas = (List<ThematicAreaEntity>)Session[sessionKeyAssociatedThematicAreas];
                    if (associatedThematicAreas.Count >= 1)
                    {
                        string errorMessage = Convert.ToString(GetLocalResourceObject("msjSingleTematicAssociationWarning"));
                        MensajeriaHelper.MostrarMensaje((Control)sender,
                          TipoMensaje.Error,
                          errorMessage);
                        return;
                    }

                    List<ThematicAreaEntity> addedThematicArea = thematicAreas.AsEnumerable().Where(t => ThematicAreaCode == t.Field<string>("ThematicAreaCode")).Select(t => new ThematicAreaEntity()
                    {
                        GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                        ThematicAreaCode = t.Field<string>("ThematicAreaCode"),
                        ThematicAreaName = t.Field<string>("ThematicAreaName")
                    }).ToList();

                    if (addedThematicArea.Count >= 1)
                    {
                        var thematicArea = new CourseEntity(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, selectedCourseCode, UserHelper.GetCurrentFullUserName);
                        thematicArea.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        ObjThematicAreasBll.AddCourseByThematicArea(
                           thematicArea,
                            addedThematicArea[0]
                        );

                        associatedThematicAreas.Add(addedThematicArea[0]);

                        rptAssociatedThematicAreas.DataSource = associatedThematicAreas;
                        rptAssociatedThematicAreas.DataBind();

                        FilterSearchedThematicAreas();

                        uppAssociatedThematicAreas.Update();
                        uppSearchThematicAreas.Update();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAddThematicAreaClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnAddThematicAreaClickPostBack('{0}'); }}, 200);", btnAdd.ClientID), true);
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender,
                        TipoMensaje.Error,
                        GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender,
                        TipoMensaje.Error,
                        ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender,
                        TipoMensaje.Error,
                        newEx.Message);
                }
            }
        }

        #endregion

        #region Positions

        /// <summary>
        /// Handles the btnRefreshAssociatedPositions click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRefreshAssociatedPositions_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshAssociatedPositions();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppAssociatedPositions
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppAssociatedPositions
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEditPositions_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    LoadAssociatedPositions(true);
                    List<PositionEntity> associatedPositions = (List<PositionEntity>)Session[sessionKeyAssociatedPositions];

                    rptAssociatedPositions.DataSource = associatedPositions;
                    rptAssociatedPositions.DataBind();
                    uppAssociatedPositions.Update();

                    txtSearchPositions.Text = string.Empty;
                    uppSearchBarPosition.Update();

                    txtSearchPositions.Text = string.Empty;
                    rptPositions.DataSource = null;
                    rptPositions.DataBind();
                    uppSearchPositions.Update();

                    lblSearchPositionsResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchPositionsResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchPositionsResultsCount")), 0, 0));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditPositionsClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditPositionsClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                }

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
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
        /// Handles the txtSearchPositions text changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void TxtSearchPositions_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FilterSearchedPositions();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppSearchPositions
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppSearchPositions
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnDeleteAssociatedPosition click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDeleteAssociatedPosition_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string positionCode = ((TextBox)htmlButton.Parent.FindControl("hdfAssociatedPositionCode")).Text;

                    LoadAssociatedPositions(false);
                    List<PositionEntity> associatedPositions = (List<PositionEntity>)Session[sessionKeyAssociatedPositions];

                    List<PositionEntity> removedPositions = associatedPositions.Where(at => string.Equals(positionCode, at.PositionCode)).ToList();

                    if (removedPositions.Count >= 1)
                    {
                        var course = new CourseEntity(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, selectedCourseCode, UserHelper.GetCurrentFullUserName);
                        course.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        ObjCoursesBll.DeleteCourseByPosition(
                           course,
                            removedPositions[0]
                        );

                        associatedPositions.RemoveAll(at => positionCode == at.PositionCode);

                        rptAssociatedPositions.DataSource = associatedPositions;
                        rptAssociatedPositions.DataBind();

                        FilterSearchedPositions();

                        uppAssociatedPositions.Update();
                        uppSearchPositions.Update();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeletePositionClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnDeletePositionClickPostBack('{0}'); }}, 200);", btnDelete.ClientID), true);
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnAddPosition click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddPosition_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string positionCode = ((TextBox)htmlButton.Parent.FindControl("hdfPositionCode")).Text;

                    LoadPositions();
                    DataTable positions = (DataTable)Session[sessionKeyPositionsResults];

                    LoadAssociatedPositions(false);
                    List<PositionEntity> associatedPositions = (List<PositionEntity>)Session[sessionKeyAssociatedPositions];

                    List<PositionEntity> addedPosition = positions.AsEnumerable().Where(t => string.Equals(positionCode, t.Field<string>("PositionCode"))).Select(t => new PositionEntity
                    (
                        t.Field<string>("PositionCode"),
                        t.Field<string>("PositionName"))
                    ).ToList();

                    if (addedPosition.Count >= 1)
                    {
                        if (associatedPositions.Count == 0)
                        {
                            var course = new CourseEntity(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, selectedCourseCode, UserHelper.GetCurrentFullUserName);
                            course.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                            ObjCoursesBll.AddCourseByPosition(
                                course ,
                                addedPosition[0]
                            );

                            associatedPositions.Add(addedPosition[0]);

                            rptAssociatedPositions.DataSource = associatedPositions;
                            rptAssociatedPositions.DataBind();

                            FilterSearchedPositions();

                            uppAssociatedPositions.Update();
                            uppSearchPositions.Update();

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAddPositionClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnAddPositionClickPostBack('{0}'); }}, 200);", btnAdd.ClientID), true);
                        }

                        else
                        {
                            MensajeriaHelper.MostrarMensaje((Control)sender,
                                TipoMensaje.Informacion,
                                GetLocalResourceObject("MsgPosicionesMaximasAsiganadas").ToString());
                        }
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        #endregion

        #region School Tranning

        /// <summary>
        /// Handles the btnRefreshAssociatedSchoolsTraining click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRefreshAssociatedSchoolsTraining_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshAssociatedSchoolsTraining();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppAssociatedSchoolsTraining
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppAssociatedSchoolsTraining
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnEditSchoolTraining click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEditSchoolsTraining_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    LoadAssociatedSchoolsTraining(true);
                    List<SchoolTrainingEntity> associatedSchoolsTraining = (List<SchoolTrainingEntity>)Session[sessionKeyAssociatedSchoolTraining];

                    rptAssociatedSchoolsTraining.DataSource = associatedSchoolsTraining;
                    rptAssociatedSchoolsTraining.DataBind();
                    uppAssociatedSchoolsTraining.Update();

                    txtSearchSchoolsTraining.Text = string.Empty;
                    uppSearchBarSchoolTraining.Update();

                    txtSearchSchoolsTraining.Text = string.Empty;
                    rptSchoolsTraining.DataSource = null;
                    rptSchoolsTraining.DataBind();
                    uppSearchSchoolsTraining.Update();

                    lblSearchSchoolsTrainingResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchSchoolsTrainingResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchSchoolsTrainingResultsCount")), 0, 0));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditSchoolsTrainingClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditSchoolsTrainingClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                }

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
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
        /// Handles the txtSearchSchoolsTraining text changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void TxtSearchSchoolsTraining_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FilterSearchedSchoolsTraining();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppSearchSchoolsTraining
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppSearchSchoolsTraining
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnDeleteAssociatedSchoolTraining click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDeleteAssociatedSchoolTraining_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string SchoolTrainingCode = ((TextBox)htmlButton.Parent.FindControl("hdfAssociatedSchoolTrainingCode")).Text;

                    LoadAssociatedSchoolsTraining(false);
                    List<SchoolTrainingEntity> associatedSchoolsTraining = (List<SchoolTrainingEntity>)Session[sessionKeyAssociatedSchoolTraining];

                    List<SchoolTrainingEntity> removedSchoolsTraining = associatedSchoolsTraining.Where(at => string.Equals(SchoolTrainingCode, at.SchoolTrainingCode)).ToList();

                    if (removedSchoolsTraining.Count >= 1)
                    {
                        ObjSchoolTrainingBll.DeleteSchoolsTrainingByCourse(
                            removedSchoolsTraining[0],
                            new CourseEntity(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, 
                                             selectedCourseCode,
                                             SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                                             null,
                                             UserHelper.GetCurrentFullUserName
                                             )
                        );

                        associatedSchoolsTraining.RemoveAll(at => string.Equals(SchoolTrainingCode, at.SchoolTrainingCode));

                        rptAssociatedSchoolsTraining.DataSource = associatedSchoolsTraining;
                        rptAssociatedSchoolsTraining.DataBind();

                        FilterSearchedSchoolsTraining();

                        uppAssociatedSchoolsTraining.Update();
                        uppSearchSchoolsTraining.Update();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteSchoolTrainingClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnDeleteSchoolsTrainingClickPostBack('{0}'); }}, 200);", btnDelete.ClientID), true);
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnAddSchoolTraining click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddSchoolTraining_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string SchoolTrainingCode = ((TextBox)htmlButton.Parent.FindControl("hdfSchoolTrainingCode")).Text;

                    LoadSchoolsTraining();
                    DataTable SchoolsTraining = (DataTable)Session[sessionKeySchoolTrainingResults];

                    LoadAssociatedSchoolsTraining(false);
                    List<SchoolTrainingEntity> associatedSchoolsTraining = (List<SchoolTrainingEntity>)Session[sessionKeyAssociatedSchoolTraining];
                    if (associatedSchoolsTraining.Count >= 1)
                    {
                        string warningMessage = Convert.ToString(GetLocalResourceObject("msjSingleAssociationWarning"));
                        MensajeriaHelper.MostrarMensaje((Control)sender,
                          TipoMensaje.Advertencia,
                          warningMessage);
                        return;
                    }

                    List<SchoolTrainingEntity> addedSchoolTraining = SchoolsTraining.AsEnumerable().Where(t => string.Equals(SchoolTrainingCode, t.Field<string>("SchoolTrainingCode"))).Select(t => new SchoolTrainingEntity
                    (
                        t.Field<string>("SchoolTrainingCode"),
                        t.Field<string>("SchoolTrainingName"))
                    ).ToList();

                    if (addedSchoolTraining.Count >= 1)
                    {
                        var course = new CourseEntity(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, selectedCourseCode, UserHelper.GetCurrentFullUserName);
                        course.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                        ObjSchoolTrainingBll.AddSchoolsTrainingByCourse(
                            addedSchoolTraining[0],
                            course
                        );

                        associatedSchoolsTraining.Add(addedSchoolTraining[0]);

                        rptAssociatedSchoolsTraining.DataSource = associatedSchoolsTraining;
                        rptAssociatedSchoolsTraining.DataBind();

                        FilterSearchedSchoolsTraining();

                        uppAssociatedSchoolsTraining.Update();
                        uppSearchSchoolsTraining.Update();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAddSchoolTrainingClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnAddSchoolTrainingClickPostBack('{0}'); }}, 200);", btnAdd.ClientID), true);
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        #endregion

        /// <summary>
        /// Handles the btnDelete click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    LoadAssociatedTrainingPrograms(true);
                    List<TrainingProgramEntity> associatedTrainingPrograms = (List<TrainingProgramEntity>)Session[sessionKeyAssociatedTrainingPrograms];

                    if (associatedTrainingPrograms.Count > 0)
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Advertencia,
                            GetLocalResourceObject("msgAssociatedTrainingPrograms").ToString());

                        return;
                    }

                    LoadAssociatedTrainers(true);
                    List<TrainerEntity> associatedTrainers = (List<TrainerEntity>)Session[sessionKeyAssociatedTrainers];

                    if (associatedTrainers.Count > 0)
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Advertencia,
                            GetLocalResourceObject("msgAssociatedTrainers").ToString());

                        return;
                    }

                    LoadAssociatedThematicAreas(true);
                    List<ThematicAreaEntity> associatedAreasThematic = (List<ThematicAreaEntity>)Session[sessionKeyAssociatedThematicAreas];

                    if (associatedAreasThematic.Count > 0)
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Advertencia,
                            GetLocalResourceObject("msgAssociatedThematicAreas").ToString());

                        return;
                    }

                    string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    ObjCoursesBll.Delete(
                        new CourseEntity(
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                            selectedCourseCode,
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                            null,
                            UserHelper.GetCurrentFullUserName));

                    PageHelper<CourseEntity> pageHelper = (PageHelper<CourseEntity>)Session[sessionKeyCoursesResults];

                    pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.CourseCode == selectedCourseCode));
                    pageHelper.TotalResults--;

                    if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                    {
                        SearchResults(pageHelper.TotalPages - 1);
                    }

                    pageHelper.UpdateTotalPages();
                    RefreshTable();

                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Error,
                        GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                hdfCourseCodeFilter.Value = txtCourseCodeFilter.Text;
                hdfCourseNameFilter.Value = txtCourseNameFilter.Text;
                hdfCourseStateFilter.Value = cboCourseState.Value;

                SearchResults(1);

                CommonFunctions.ResetSortDirection(Page.ClientID, grvList.ClientID);

                DisplayResults();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void CboCourseStateChanged(object sender, EventArgs e)
        {
            try
            {
                hdfCourseCodeFilter.Value = txtCourseCodeFilter.Text;
                hdfCourseNameFilter.Value = txtCourseNameFilter.Text;
                hdfCourseStateFilter.Value = cboCourseState.Value;

                hdfDivisionCode.Value = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode.ToString();
                hdfGeographicDivisionCode.Value = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the BtnExport_ServerClick click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnExport_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<CourseEntity> courses = ObjCoursesBll.ListByKeyExport(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

                var jsonExport = courses.AsEnumerable().Select(c => new
                {
                    Codigo = c.CourseCode,
                    Nombre = c.CourseName,
                    TipoFormacion = c.TypeTrainingName,
                    Costo = c.CourseCostByParticipant,
                    Duracion = c.CourseDuration,
                    AreaTematica = c.ThematicAreaName,
                    ProgramaEntrenamiento = c.TrainingProgramName,
                    EscuelaEntrenamientoCodigo = c.SchoolTrainingCode,
                    EscuelaEntrenamientoNombre = c.SchoolTrainingName,
                    Matrix = c.ForMatrix ? GetLocalResourceObject("Yes") : GetLocalResourceObject("NO"),
                    TieneCiclosRefrescamiento = c.CyclesRefreshment ? GetLocalResourceObject("Yes") : GetLocalResourceObject("NO"),
                    NoteRequired = c.NoteRequired ? GetLocalResourceObject("Yes") : GetLocalResourceObject("NO"),
                    IsAnnual = c.NoteRequired ? GetLocalResourceObject("Yes") : GetLocalResourceObject("NO"),
                    Externo = c.ExternalCourse ? GetLocalResourceObject("Yes") : GetLocalResourceObject("NO"),
                    Estado = c.State ? GetLocalResourceObject("Yes") : GetLocalResourceObject("NO"),
                });

                var jsonserializado = new JavaScriptSerializer().Serialize(jsonExport);

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnExportClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnExportClickPostBack(" + jsonserializado + "); }}, 500);", true);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the blstPager click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BlstPager_Click(object sender, BulletedListEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(((BulletedList)sender).Items[e.Index].Value))
                {
                    int page = Convert.ToInt32(((BulletedList)sender).Items[e.Index].Value);
                    PagerUtil.SetActivePage(blstPager, page);

                    SearchResults(page);
                    DisplayResults();
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the grvList pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvList_PreRender(object sender, EventArgs e)
        {
            if ((grvList.ShowHeader && grvList.Rows.Count > 0) || (grvList.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                grvList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvList.ShowFooter && grvList.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                grvList.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[sessionKeyCoursesResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<CourseEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Load the trainer types as options for combobox
        /// </summary>
        /// <returns>The trainer types as options for combobox</returns>
        private IDictionary<string, string> LoadtrainerTypes()
        {
            Dictionary<string, string> trainerTypesOptions = new Dictionary<string, string>
            {
                { "-1", Convert.ToString(GetLocalResourceObject("lblTrainerTypeOption")) }
            };

            IDictionary<string, string> trainerTypes = GetAllValuesAndLocalizatedDescriptions<TrainerType>();
            foreach (KeyValuePair<string, string> type in trainerTypes)
            {
                trainerTypesOptions.Add(type.Key, type.Value);
            }

            return trainerTypesOptions;
        }

        /// <summary>
        /// Get the localizated string of trainer type 
        /// </summary>
        /// <param name="enumerationValuee">Enumeration value</param>
        /// <returns>Localizated name</returns>
        public string GetTrainerTypeLocalizatedDescription(string enumerationValue)
        {
            return GetLocatizatedDescription(HrisEnum.ParseEnumByName<TrainerType>(enumerationValue));
        }

        /// <summary>
        /// Load data of type training
        /// </summary>
        private void TypeTrainingLoadDll()
        {
            var itemsDll = ObjTypeTrainingBll.TypeTrainingListByCatalog();

            txtTypeTrainingEdit.Items.AddRange(itemsDll);
        }

        /// <summary>
        /// Ajust trainers from database
        /// </summary>
        private void AjustTrainers()
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                LoadTrainers();
                DataTable existentTrainers = (DataTable)Session[sessionKeyTrainersResults];
                DataTable trainers = existentTrainers.Copy();

                LoadAssociatedTrainers(false);
                List<TrainerEntity> associatedTrainers = (List<TrainerEntity>)Session[sessionKeyAssociatedTrainers];

                associatedTrainers.ForEach(at =>
                {
                    trainers.AsEnumerable().Where(t =>
                        string.Equals(Convert.ToString(at.TrainerType), t.Field<string>("TrainerType").ToString()) &&
                        string.Equals(at.TrainerCode, t.Field<string>("TrainerCode"))).ToList().ForEach(t => trainers.Rows.Remove(t));
                });

                Session[sessionKeyTrainersAjustResults] = trainers;
            }

            else
            {
                Session[sessionKeyTrainersAjustResults] = new DataTable();
            }
        }

        /// <summary>
        /// Create the filter for the searched training
        /// </summary>
        /// <returns></returns>
        private string CreateFilterWithTrainerTypeSearchedTraining()
        {
            char[] charSeparators = new char[] { ',', ' ' };
            string[] terms = txtSearchTrainers.Text.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

            string filter = string.Empty;
            foreach (string term in terms)
            {
                string subfilter = string.Format(" (TrainerCode Like '%{0}%' OR TrainerName Like '%{0}%') AND (TrainerType = '{1}') ", term, cboTrainerType.SelectedItem.Value);
                filter = string.Format("{0} {1} {2} ", filter, string.IsNullOrWhiteSpace(filter) ? "" : " AND ", subfilter);
            }

            return filter;
        }

        /// <summary>
        /// Create the filter for the searched training
        /// </summary>
        /// <returns></returns>
        private string CreateFilterWithoutTrainerTypeSearchedTraining()
        {
            char[] charSeparators = new char[] { ',', ' ' };
            string[] terms = txtSearchTrainers.Text.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

            string filter = string.Empty;
            foreach (string term in terms)
            {
                string subfilter = string.Format(" (TrainerCode Like '%{0}%' OR TrainerName Like '%{0}%') ", term);
                filter = string.Format("{0} {1} {2} ", filter, string.IsNullOrWhiteSpace(filter) ? "" : " AND ", subfilter);
            }

            return filter;
        }

        #region Trainers

        /// <summary>
        /// Load empty data structure for Trainers
        /// </summary>
        /// <returns>Empty data structure for Trainers</returns>
        private DataTable LoadEmptyTrainers()
        {
            DataTable Trainers = new DataTable();
            Trainers.Columns.Add("TrainerCode", typeof(string));
            Trainers.Columns.Add("TrainerType", typeof(string));
            Trainers.Columns.Add("TrainerName", typeof(string));

            return Trainers;
        }

        /// <summary>
        /// Load Trainers from database
        /// </summary>        
        private void LoadTrainers()
        {
            if (Session[sessionKeyTrainersResults] == null)
            {
                DataTable Trainers = LoadEmptyTrainers();

                List<TrainerEntity> registeredTrainers = ObjTrainersBll.ListByDivision(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredTrainers.ForEach(x => Trainers.Rows.Add(x.TrainerCode, x.TrainerType, x.TrainerName));

                Session[sessionKeyTrainersResults] = Trainers;
            }
        }

        /// <summary>
        /// Load associated trainers
        /// </summary>        
        private void LoadAssociatedTrainers(bool forceRead)
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                if (Session[sessionKeyAssociatedTrainers] == null || forceRead)
                {
                    Session[sessionKeyAssociatedTrainers] = ObjTrainersBll.ListByCourse(workingDivision.GeographicDivisionCode,workingDivision.DivisionCode, selectedCourseCode, forceRead ? forceRead : (bool?)null);
                }
            }

            else
            {
                Session[sessionKeyAssociatedTrainers] = new List<TrainerEntity>();
            }
        }

        /// <summary>
        /// Load associated trainers overload
        /// </summary>
        private void LoadAssociatedTrainers(bool forceRead, string selectedCourseCode)
        {
            if (selectedCourseCode != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                if (Session[sessionKeyAssociatedTrainers] == null || forceRead)
                {
                    ObjTrainersBll = HttpContext.Current.Application.GetContainer().Resolve<ITrainersBll<TrainerEntity>>();
                    HttpContext.Current.Session[sessionKeyAssociatedTrainers] = ObjTrainersBll.ListByCourse(workingDivision.GeographicDivisionCode, workingDivision.DivisionCode,selectedCourseCode);
                }
            }

            else
            {
                Session[sessionKeyAssociatedTrainers] = new List<TrainerEntity>();
            }
        }

        /// <summary>
        /// Refresh the associated trainers tables by rebinding data
        /// </summary>
        private void RefreshAssociatedTrainers()
        {
            if (Session[sessionKeyAssociatedTrainers] != null)
            {
                rptAssociatedTrainers.DataSource = Session[sessionKeyAssociatedTrainers];
                rptAssociatedTrainers.DataBind();
            }
        }

        /// <summary>
        /// Filter the searched Trainers by search tokens
        /// </summary>
        private void FilterSearchedTrainers()
        {
            AjustTrainers();
            DataTable trainers = (DataTable)Session[sessionKeyTrainersAjustResults];

            int minRowCount;

            if (!string.IsNullOrWhiteSpace(txtSearchTrainers.Text))
            {
                if (!cboTrainerType.SelectedItem.Value.Equals("-1"))
                {
                    trainers.DefaultView.RowFilter = CreateFilterWithTrainerTypeSearchedTraining();
                }
                else
                {
                    trainers.DefaultView.RowFilter = CreateFilterWithoutTrainerTypeSearchedTraining();
                }

                DataTable finalTopResults = LoadEmptyTrainers();
                minRowCount = Math.Min(1, trainers.DefaultView.Count);

                foreach (DataRowView row in trainers.DefaultView)
                {
                    finalTopResults.ImportRow(row.Row);
                }

                rptTrainers.DataSource = finalTopResults;
                rptTrainers.DataBind();
                uppSearchTrainers.Update();
            }

            else
            {
                if (!cboTrainerType.SelectedItem.Value.Equals("-1"))
                {
                    trainers.DefaultView.RowFilter = string.Format(" (TrainerType = '{0}') ", cboTrainerType.SelectedItem.Value);
                }

                txtSearchTrainers.Text = string.Empty;

                minRowCount = Math.Min(1, trainers.DefaultView.Count);

                rptTrainers.DataSource = trainers;
                rptTrainers.DataBind();
                uppSearchTrainers.Update();
            }

            lblSearchTrainersResults.InnerHtml = string.Format("{0} {1}",
                Convert.ToString(GetLocalResourceObject("lblSearchTrainersResults")),
                string.Format(Convert.ToString(GetLocalResourceObject("lblSearchTrainersResultsCount")), minRowCount, trainers.DefaultView.Count));

            ScriptManager.RegisterStartupScript(uppSearchTrainers, uppSearchTrainers.GetType(), string.Format("ReturnFromSearchTrainersPostBack{0}", Guid.NewGuid()), "ReturnFromSearchTrainersPostBack()", true);
        }

        #endregion

        #region Training Programs

        /// <summary>
        /// Load empty data structure for TrainingPrograms
        /// </summary>
        /// <returns>Empty data structure for TrainingPrograms</returns>
        private DataTable LoadEmptyTrainingPrograms()
        {
            DataTable TrainingPrograms = new DataTable();
            TrainingPrograms.Columns.Add("TrainingProgramCode", typeof(string));
            TrainingPrograms.Columns.Add("TrainingProgramName", typeof(string));

            return TrainingPrograms;
        }

        /// <summary>
        /// Load TrainingPrograms from database
        /// </summary>        
        private void LoadTrainingPrograms()
        {
            if (Session[sessionKeyTrainingProgramsResults] == null)
            {
                DataTable trainingPrograms = LoadEmptyTrainingPrograms();

                List<TrainingProgramEntity> registeredTrainingPrograms = ObjTrainingProgramsBll.ListByDivision(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredTrainingPrograms.ForEach(x => trainingPrograms.Rows.Add(x.TrainingProgramCode, x.TrainingProgramName));

                Session[sessionKeyTrainingProgramsResults] = trainingPrograms;
            }
        }

        /// <summary>
        /// Load associated training programs
        /// </summary>        
        private void LoadAssociatedTrainingPrograms(bool forceRead)
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                if (Session[sessionKeyAssociatedTrainingPrograms] == null || forceRead)
                {
                    Session[sessionKeyAssociatedTrainingPrograms] = ObjTrainingProgramsBll.ListByCourse(workingDivision.GeographicDivisionCode, workingDivision.DivisionCode,selectedCourseCode);
                }
            }

            else
            {
                Session[sessionKeyAssociatedTrainingPrograms] = new List<TrainingProgramEntity>();
            }
        }

        /// <summary>
        /// Load associated training programs overload
        /// </summary>
        private void LoadAssociatedTrainingPrograms(bool forceRead, string selectedCourseCode)
        {
            if (selectedCourseCode != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                if (Session[sessionKeyAssociatedTrainingPrograms] == null || forceRead)
                {
                    ObjTrainingProgramsBll = HttpContext.Current.Application.GetContainer().Resolve<ITrainingProgramsBll<TrainingProgramEntity>>();
                    HttpContext.Current.Session[sessionKeyAssociatedTrainingPrograms] = ObjTrainingProgramsBll.ListByCourse(workingDivision.GeographicDivisionCode, workingDivision.DivisionCode,selectedCourseCode);
                }
            }

            else
            {
                Session[sessionKeyAssociatedTrainingPrograms] = new List<TrainingProgramEntity>();
            }
        }

        /// <summary>
        /// Refresh the associated training programs tables by rebinding data
        /// </summary>
        private void RefreshAssociatedTrainingPrograms()
        {
            if (Session[sessionKeyAssociatedTrainingPrograms] != null)
            {
                rptAssociatedTrainingPrograms.DataSource = Session[sessionKeyAssociatedTrainingPrograms];
                rptAssociatedTrainingPrograms.DataBind();
            }
        }

        /// <summary>
        /// Filter the searched TrainingPrograms by search tokens
        /// </summary>
        private void FilterSearchedTrainingPrograms()
        {
            LoadTrainingPrograms();
            DataTable existentTrainingPrograms = (DataTable)Session[sessionKeyTrainingProgramsResults];
            DataTable trainingPrograms = existentTrainingPrograms.Copy();

            LoadAssociatedTrainingPrograms(false);
            List<TrainingProgramEntity> associatedTrainingPrograms = (List<TrainingProgramEntity>)Session[sessionKeyAssociatedTrainingPrograms];

            associatedTrainingPrograms.ForEach(at =>
            {
                trainingPrograms.AsEnumerable().Where(t =>
                    string.Equals(at.TrainingProgramCode, t.Field<string>("TrainingProgramCode"))).ToList().ForEach(t => trainingPrograms.Rows.Remove(t));
            });

            if (!string.IsNullOrWhiteSpace(txtSearchTrainingPrograms.Text))
            {
                char[] charSeparators = new char[] { ',', ' ' };
                string[] terms = txtSearchTrainingPrograms.Text.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                string filter = string.Empty;
                foreach (string term in terms)
                {
                    string subfilter = string.Format("(TrainingProgramCode Like '%{0}%' OR TrainingProgramName Like '%{0}%')", term);
                    filter = string.Format("{0} {1} {2} ", filter, string.IsNullOrWhiteSpace(filter) ? "" : " AND ", subfilter);
                }

                trainingPrograms.DefaultView.RowFilter = filter;
                DataTable finalTopResults = LoadEmptyTrainingPrograms();

                int minRowCount = Math.Min(10, trainingPrograms.DefaultView.Count);
                for (int i = 0; i < minRowCount; i++)
                {
                    finalTopResults.ImportRow(trainingPrograms.DefaultView[i].Row);
                }

                rptTrainingPrograms.DataSource = finalTopResults;
                rptTrainingPrograms.DataBind();

                lblSearchTrainingProgramsResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchTrainingProgramsResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchTrainingProgramsResultsCount")), minRowCount, trainingPrograms.DefaultView.Count));
            }

            else
            {
                trainingPrograms.DefaultView.RowFilter = "1 = 0";
                rptTrainingPrograms.DataSource = null;
                rptTrainingPrograms.DataBind();

                lblSearchTrainingProgramsResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchTrainingProgramsResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchTrainingProgramsResultsCount")), 0, 0));
            }

            ScriptManager.RegisterStartupScript(uppSearchTrainingPrograms, uppSearchTrainingPrograms.GetType(), string.Format("ReturnFromSearchTrainingProgramsPostBack{0}", Guid.NewGuid()), "ReturnFromSearchTrainingProgramsPostBack()", true);
        }

        #endregion

        #region Thematic Areas

        /// <summary>
        /// Load empty data structure for ThematicAreas
        /// </summary>
        /// <returns>Empty data structure for ThematicAreas</returns>
        private DataTable LoadEmptyThematicAreas()
        {
            DataTable ThematicAreas = new DataTable();
            ThematicAreas.Columns.Add("ThematicAreaCode", typeof(string));
            ThematicAreas.Columns.Add("ThematicAreaName", typeof(string));

            return ThematicAreas;
        }

        /// <summary>
        /// Load ThematicAreas from database
        /// </summary>        
        private void LoadThematicAreas()
        {
            if (Session[sessionKeyThematicAreasResults] == null)
            {
                DataTable thematicAreas = LoadEmptyThematicAreas();
                string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                List<ThematicAreaEntity> registeredThematicAreas = ObjThematicAreasBll.ThematicAreasByCourseNotAssociated(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, 
                    selectedCourseCode);

                registeredThematicAreas.ForEach(x => thematicAreas.Rows.Add(x.ThematicAreaCode, x.ThematicAreaName));

                Session[sessionKeyThematicAreasResults] = thematicAreas;
            }
        }

        /// <summary>
        /// Load associated thematicAreas
        /// </summary>        
        private void LoadAssociatedThematicAreas(bool forceRead)
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                if (Session[sessionKeyAssociatedThematicAreas] == null || forceRead)
                {
                    Session[sessionKeyAssociatedThematicAreas] = ObjThematicAreasBll.ThematicAreasByCourseAssociated(workingDivision.GeographicDivisionCode, workingDivision.DivisionCode ,selectedCourseCode);
                }
            }

            else
            {
                Session[sessionKeyAssociatedThematicAreas] = new List<ThematicAreaEntity>();
            }
        }

        /// <summary>
        /// Load associated thematicAreas overload
        /// </summary>
        private void LoadAssociatedThematicAreas(bool forceRead, string selectedCourseCode)
        {
            if (selectedCourseCode != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                if (Session[sessionKeyAssociatedThematicAreas] == null || forceRead)
                {
                    ObjThematicAreasBll = HttpContext.Current.Application.GetContainer().Resolve<IThematicAreasBll<ThematicAreaEntity>>();

                    HttpContext.Current.Session[sessionKeyAssociatedThematicAreas] = ObjThematicAreasBll.ListByCourse(workingDivision.GeographicDivisionCode ,workingDivision.DivisionCode ,selectedCourseCode);
                }
            }

            else
            {
                Session[sessionKeyAssociatedThematicAreas] = new List<ThematicAreaEntity>();
            }
        }

        /// <summary>
        /// Refresh the associated thematicAreas tables by rebinding data
        /// </summary>
        private void RefreshAssociatedThematicAreas()
        {
            if (Session[sessionKeyAssociatedThematicAreas] != null)
            {
                rptAssociatedThematicAreas.DataSource = Session[sessionKeyAssociatedThematicAreas];
                rptAssociatedThematicAreas.DataBind();
            }
        }

        /// <summary>
        /// Filter the searched ThematicAreas by search tokens
        /// </summary>
        private void FilterSearchedThematicAreas()
        {
            LoadThematicAreas();
            DataTable existentThematicAreas = (DataTable)Session[sessionKeyThematicAreasResults];
            DataTable thematicAreas = existentThematicAreas.Copy();

            LoadAssociatedThematicAreas(false);
            List<ThematicAreaEntity> associatedThematicAreas = (List<ThematicAreaEntity>)Session[sessionKeyAssociatedThematicAreas];

            associatedThematicAreas.ForEach(at =>
            {
                thematicAreas.AsEnumerable().Where(t => at.ThematicAreaCode == t.Field<string>("ThematicAreaCode")).ToList().ForEach(t => thematicAreas.Rows.Remove(t));
            });

            if (!string.IsNullOrWhiteSpace(txtSearchThematicAreas.Text))
            {
                char[] charSeparators = new char[] { ',', ' ' };
                string[] terms = txtSearchThematicAreas.Text.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                string filter = string.Empty;
                foreach (string term in terms)
                {
                    string subfilter = string.Format("(Convert([ThematicAreaCode], System.String) Like '%{0}%' OR ThematicAreaName Like '%{0}%')", term);
                    filter = string.Format("{0} {1} {2} ", filter, string.IsNullOrWhiteSpace(filter) ? "" : " AND ", subfilter);
                }

                thematicAreas.DefaultView.RowFilter = filter;

                DataTable finalTopResults = LoadEmptyThematicAreas();
                int minRowCount = Math.Min(10, thematicAreas.DefaultView.Count);

                for (int i = 0; i < minRowCount; i++)
                {
                    finalTopResults.ImportRow(thematicAreas.DefaultView[i].Row);
                }

                rptThematicAreas.DataSource = finalTopResults;
                rptThematicAreas.DataBind();

                lblSearchThematicAreasResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchThematicAreasResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchThematicAreasResultsCount")), minRowCount, thematicAreas.DefaultView.Count));
            }

            else
            {
                thematicAreas.DefaultView.RowFilter = "1 = 0";
                rptThematicAreas.DataSource = null;
                rptThematicAreas.DataBind();

                lblSearchThematicAreasResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchThematicAreasResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchThematicAreasResultsCount")), 0, 0));
            }

            ScriptManager.RegisterStartupScript(uppSearchThematicAreas, uppSearchThematicAreas.GetType(), string.Format("ReturnFromSearchThematicAreasPostBack{0}", Guid.NewGuid()), "ReturnFromSearchThematicAreasPostBack()", true);
        }

        #endregion

        #region Positions

        /// <summary>
        /// Load empty data structure for Positions
        /// </summary>
        /// <returns>Empty data structure for Positions</returns>
        private DataTable LoadEmptyPositions()
        {
            DataTable Positions = new DataTable();
            Positions.Columns.Add("PositionCode", typeof(string));
            Positions.Columns.Add("PositionName", typeof(string));

            return Positions;
        }

        /// <summary>
        /// Load Positions from database
        /// </summary>        
        private void LoadPositions()
        {
            if (Session[sessionKeyPositionsResults] == null)
            {
                DataTable positions = LoadEmptyPositions();

                List<PositionEntity> registeredPositions = ObjPositionsBll.ListEnabled();

                registeredPositions.ForEach(x => positions.Rows.Add(x.PositionCode, x.PositionName));

                Session[sessionKeyPositionsResults] = positions;
            }
        }

        /// <summary>
        /// Load associated positions
        /// </summary>        
        private void LoadAssociatedPositions(bool forceRead)
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                if (Session[sessionKeyAssociatedPositions] == null || forceRead)
                {
                    Session[sessionKeyAssociatedPositions] = ObjPositionsBll.ListByCourse(workingDivision.GeographicDivisionCode,workingDivision.DivisionCode, selectedCourseCode);
                }
            }

            else
            {
                Session[sessionKeyAssociatedPositions] = new List<PositionEntity>();
            }
        }

        /// <summary>
        /// Load associated positions overload
        /// </summary>
        private void LoadAssociatedPositions(bool forceRead, string selectedCourseCode)
        {
            if (selectedCourseCode != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                if (Session[sessionKeyAssociatedPositions] == null || forceRead)
                {
                    ObjPositionsBll = HttpContext.Current.Application.GetContainer().Resolve<IPositionsBll<PositionEntity>>();
                    HttpContext.Current.Session[sessionKeyAssociatedPositions] = ObjPositionsBll.ListByCourse(workingDivision.GeographicDivisionCode,workingDivision.DivisionCode, selectedCourseCode);
                }
            }

            else
            {
                Session[sessionKeyAssociatedPositions] = new List<PositionEntity>();
            }
        }

        /// <summary>
        /// Refresh the associated positions tables by rebinding data
        /// </summary>
        private void RefreshAssociatedPositions()
        {
            if (Session[sessionKeyAssociatedPositions] != null)
            {
                rptAssociatedPositions.DataSource = Session[sessionKeyAssociatedPositions];
                rptAssociatedPositions.DataBind();
            }
        }

        /// <summary>
        /// Filter the searched Positions by search tokens
        /// </summary>
        private void FilterSearchedPositions()
        {
            LoadPositions();
            DataTable existentPositions = (DataTable)Session[sessionKeyPositionsResults];
            DataTable positions = existentPositions.Copy();

            LoadAssociatedPositions(false);
            List<PositionEntity> associatedPositions = (List<PositionEntity>)Session[sessionKeyAssociatedPositions];

            associatedPositions.ForEach(at =>
            {
                positions.AsEnumerable().Where(t => string.Equals(at.PositionCode, t.Field<string>("PositionCode"))).ToList().ForEach(t => positions.Rows.Remove(t));
            });

            if (!string.IsNullOrWhiteSpace(txtSearchPositions.Text))
            {
                char[] charSeparators = new char[] { ',', ' ' };
                string[] terms = txtSearchPositions.Text.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                string filter = string.Empty;
                foreach (string term in terms)
                {
                    string subfilter = string.Format("(PositionCode Like '%{0}%' OR PositionName Like '%{0}%')", term);
                    filter = string.Format("{0} {1} {2} ", filter, string.IsNullOrWhiteSpace(filter) ? "" : " AND ", subfilter);
                }

                positions.DefaultView.RowFilter = filter;

                DataTable finalTopResults = LoadEmptyPositions();
                int minRowCount = Math.Min(10, positions.DefaultView.Count);
                for (int i = 0; i < minRowCount; i++)
                {
                    finalTopResults.ImportRow(positions.DefaultView[i].Row);
                }

                rptPositions.DataSource = finalTopResults;
                rptPositions.DataBind();

                lblSearchPositionsResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchPositionsResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchPositionsResultsCount")), minRowCount, positions.DefaultView.Count));
            }

            else
            {
                positions.DefaultView.RowFilter = "1 = 0";
                rptPositions.DataSource = null;
                rptPositions.DataBind();

                lblSearchPositionsResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchPositionsResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchPositionsResultsCount")), 0, 0));
            }

            ScriptManager.RegisterStartupScript(uppSearchPositions, uppSearchPositions.GetType(), string.Format("ReturnFromSearchPositionsPostBack{0}", Guid.NewGuid()), "ReturnFromSearchPositionsPostBack()", true);
        }

        #endregion

        #region Schools Trainning

        /// <summary>
        /// Load empty data structure for SchoolsTraining
        /// </summary>
        /// <returns>Empty data structure for SchoolsTraining</returns>
        private DataTable LoadEmptySchoolsTraining()
        {
            DataTable SchoolsTraining = new DataTable();
            SchoolsTraining.Columns.Add("SchoolTrainingCode", typeof(string));
            SchoolsTraining.Columns.Add("SchoolTrainingName", typeof(string));

            return SchoolsTraining;
        }

        /// <summary>
        /// Load SchoolsTraining from database
        /// </summary>        
        private void LoadSchoolsTraining()
        {
            
            DataTable SchoolsTraining = LoadEmptySchoolsTraining();

            var entity = new SchoolTrainingEntity
            {
                GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
            };

            List<SchoolTrainingEntity> registeredSchoolsTraining = ObjSchoolTrainingBll.ListByDivision(entity);

            registeredSchoolsTraining.ForEach(x => SchoolsTraining.Rows.Add(x.SchoolTrainingCode, x.SchoolTrainingName));

            Session[sessionKeySchoolTrainingResults] = SchoolsTraining;
            
        }

        /// <summary>
        /// Load associated SchoolsTraining
        /// </summary>        
        private void LoadAssociatedSchoolsTraining(bool forceRead)
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                if (Session[sessionKeyAssociatedSchoolTraining] == null || forceRead)
                {
                    Session[sessionKeyAssociatedSchoolTraining] = ObjSchoolTrainingBll.ListByCourses(workingDivision.GeographicDivisionCode,workingDivision.DivisionCode ,selectedCourseCode, forceRead ? forceRead : (bool?)null);
                }
            }

            else
            {
                Session[sessionKeyAssociatedSchoolTraining] = new List<TrainerEntity>();
            }
        }

        /// <summary>
        /// Refresh the associated SchoolsTraining tables by rebinding data
        /// </summary>
        private void RefreshAssociatedSchoolsTraining()
        {
            if (Session[sessionKeyAssociatedSchoolTraining] != null)
            {
                rptAssociatedSchoolsTraining.DataSource = Session[sessionKeyAssociatedSchoolTraining];
                rptAssociatedSchoolsTraining.DataBind();
            }
        }

        /// <summary>
        /// Filter the searched SchoolsTraining by search tokens
        /// </summary>
        private void FilterSearchedSchoolsTraining()
        {
            LoadSchoolsTraining();
            DataTable existentSchoolsTraining = (DataTable)Session[sessionKeySchoolTrainingResults];
            DataTable SchoolsTraining = existentSchoolsTraining.Copy();

            LoadAssociatedSchoolsTraining(false);
            List<SchoolTrainingEntity> associatedSchoolsTraining = (List<SchoolTrainingEntity>)Session[sessionKeyAssociatedSchoolTraining];

            associatedSchoolsTraining.ForEach(at =>
            {
                SchoolsTraining.AsEnumerable().Where(t =>
                    string.Equals(at.SchoolTrainingCode, t.Field<string>("SchoolTrainingCode"))).ToList().ForEach(t => SchoolsTraining.Rows.Remove(t));
            });

            if (!string.IsNullOrWhiteSpace(txtSearchSchoolsTraining.Text))
            {
                char[] charSeparators = new char[] { ',', ' ' };
                string[] terms = txtSearchSchoolsTraining.Text.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                string filter = string.Empty;
                foreach (string term in terms)
                {
                    string subfilter = string.Format("(SchoolTrainingCode Like '%{0}%' OR SchoolTrainingName Like '%{0}%')", term);
                    filter = string.Format("{0} {1} {2} ", filter, string.IsNullOrWhiteSpace(filter) ? "" : " AND ", subfilter);
                }

                SchoolsTraining.DefaultView.RowFilter = filter;

                DataTable finalTopResults = LoadEmptySchoolsTraining();
                int minRowCount = Math.Min(10, SchoolsTraining.DefaultView.Count);

                for (int i = 0; i < minRowCount; i++)
                {
                    finalTopResults.ImportRow(SchoolsTraining.DefaultView[i].Row);
                }

                rptSchoolsTraining.DataSource = finalTopResults;
                rptSchoolsTraining.DataBind();

                lblSearchSchoolsTrainingResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchSchoolsTrainingResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchSchoolsTrainingResultsCount")), minRowCount, SchoolsTraining.DefaultView.Count));
            }

            else
            {
                SchoolsTraining.DefaultView.RowFilter = "1 = 0";
                rptSchoolsTraining.DataSource = null;
                rptSchoolsTraining.DataBind();

                lblSearchSchoolsTrainingResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchSchoolsTrainingResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchSchoolsTrainingResultsCount")), 0, 0));
            }

            ScriptManager.RegisterStartupScript(uppSearchSchoolsTraining, uppSearchSchoolsTraining.GetType(), string.Format("ReturnFromSearchSchoolsTrainingPostBack{0}", Guid.NewGuid()), "ReturnFromSearchSchoolsTrainingPostBack()", true);
        }

        #endregion

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<CourseEntity> SearchResults(int page)
        {
            string courseCode = string.IsNullOrWhiteSpace(hdfCourseCodeFilter.Value) ? null : hdfCourseCodeFilter.Value;
            string courseName = string.IsNullOrWhiteSpace(hdfCourseNameFilter.Value) ? null : hdfCourseNameFilter.Value;
            string courseState = string.IsNullOrWhiteSpace(hdfCourseStateFilter.Value) ? null : hdfCourseStateFilter.Value;

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<CourseEntity> pageHelper = ObjCoursesBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                courseCode, courseName, courseState,
                sortExpression, sortDirection, page, GetLocalResourceObject("Lang").ToString());

            Session[sessionKeyCoursesResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyCoursesResults] != null)
            {
                PageHelper<CourseEntity> pageHelper = (PageHelper<CourseEntity>)Session[sessionKeyCoursesResults];

                grvList.DataSource = pageHelper.ResultList;
                grvList.DataBind();

                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                {
                    PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                }

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));

                htmlResultsSubtitle.InnerHtml = string.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
            }

            else
            {
                htmlResultsSubtitle.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
            }

            hdfSelectedRowIndex.Value = "-1";
        }

        private void RefreshTable()
        {
            SearchResults(PagerUtil.GetActivePage(blstPager));
            DisplayResults();
        }

        #endregion

        #region WebMethod

        [WebMethod(EnableSession = true)]
        public static bool AsociatedRecordsCourses(string CodCourses)
        {
            var vm = new Courses();

            vm.LoadAssociatedPositions(true, CodCourses);
            List<PositionEntity> associatedPositions = (List<PositionEntity>)HttpContext.Current.Session[vm.sessionKeyAssociatedPositions];

            vm.LoadAssociatedTrainingPrograms(true, CodCourses);
            List<TrainingProgramEntity> associatedTrainingPrograms = (List<TrainingProgramEntity>)HttpContext.Current.Session[vm.sessionKeyAssociatedTrainingPrograms];

            vm.LoadAssociatedTrainers(true, CodCourses);
            List<TrainerEntity> associatedTrainers = (List<TrainerEntity>)HttpContext.Current.Session[vm.sessionKeyAssociatedTrainers];

            vm.LoadAssociatedThematicAreas(true, CodCourses);
            List<ThematicAreaEntity> associatedThematicAreas = (List<ThematicAreaEntity>)HttpContext.Current.Session[vm.sessionKeyAssociatedThematicAreas];

            if (associatedPositions.Any() || associatedTrainingPrograms.Any() || associatedTrainers.Any() || associatedThematicAreas.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}