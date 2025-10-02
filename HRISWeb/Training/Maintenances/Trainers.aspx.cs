using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Unity.Attributes;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.Training
{
    public partial class Trainers : Page
    {
        [Dependency]
        public ITrainersBll<TrainerEntity> ObjTrainersBll { get; set; }

        [Dependency]
        public ICoursesBll<CourseEntity> ObjCourseBll { get; set; }

        [Dependency]
        public IEmployeesBll<EmployeeEntity> ObjEmployeesBll { get; set; }

        //session key for the results
        readonly string sessionKeyTrainersResults = "Trainers-TrainersResults";

        //session key for the results
        readonly string sessionKeyEmployeesResults = "Trainers-EmployeesResults";

        //session key for the courses associated by Trainers
        readonly string sessionKeyAssociatedCoursesTraining = "Trainers-AssociatedCoursesTraining";
        readonly string sessionKeyCoursesResults = "Trainers-CoursesResults";

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
                    Session[sessionKeyEmployeesResults] = null;

                    LoadEmployees();

                    cboTrainerTypeFilter.Enabled = true;
                    cboTrainerTypeFilter.DataValueField = "Key";
                    cboTrainerTypeFilter.DataTextField = "Value";
                    cboTrainerTypeFilter.DataSource = LoadtrainerTypes();
                    cboTrainerTypeFilter.DataBind();
                    cboTrainerTypeFilter.SelectedIndex = 0;

                    cboTrainerType.Enabled = true;
                    cboTrainerType.DataValueField = "Key";
                    cboTrainerType.DataTextField = "Value";
                    cboTrainerType.DataSource = LoadtrainerTypes();
                    cboTrainerType.DataBind();
                    cboTrainerType.SelectedIndex = 0;

                    rptEmployees.DataSource = LoadEmptyEmployees();
                    rptEmployees.DataBind();

                    //fire the event
                    BtnSearch_ServerClick(sender, e);
                }

                txtInternalEmployeeTrainerCode.Attributes.Add("readonly", "readonly");
                txtInternalEmployeeTrainerName.Attributes.Add("readonly", "readonly");

                lblSearchEmployeesResults.InnerHtml = Convert.ToString(GetLocalResourceObject("lblSearchEmployeesResults"));

                //activate the pager
                if (Session[sessionKeyTrainersResults] != null)
                {
                    PageHelper<TrainerEntity> pageHelper = (PageHelper<TrainerEntity>)Session[sessionKeyTrainersResults];
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
                    MensajeriaHelper.MostrarMensaje(Page,
                        TipoMensaje.Error,
                        newEx.Message);
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
                TrainerType trainerType = HrisEnum.ParseEnumByName<TrainerType>(
                     !string.IsNullOrWhiteSpace(hdfTrainerTypeEdit.Value) ? hdfTrainerTypeEdit.Value : cboTrainerType.SelectedValue);

                string trainerCode = null;
                if (!string.IsNullOrWhiteSpace(hdfTrainerCodeEdit.Value))
                {
                    trainerCode = hdfTrainerCodeEdit.Value;
                }

                if (TrainerType.EC.Equals(trainerType) || TrainerType.EP.Equals(trainerType) || TrainerType.GA.Equals(trainerType))
                {
                    trainerCode = txtExternalsTrainerCode.Text.Trim();
                }

                if (TrainerType.E.Equals(trainerType))
                {
                    trainerCode = txtInternalEmployeeTrainerCode.Text.Trim();
                }

                TrainerEntity trainer = null;
                if (TrainerType.EC.Equals(trainerType) || TrainerType.EP.Equals(trainerType) || TrainerType.GA.Equals(trainerType))
                {
                    trainer = new TrainerEntity(
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                        trainerType,
                        trainerCode,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                        txtExternalsTrainerName.Text.Trim(),
                        txtExternalsTrainerTelephone.Text.Trim(),
                        chbExternalsSearchEnabled.Checked,
                        false,
                        UserHelper.GetCurrentFullUserName,
                        DateTime.Now);
                }

                else if (TrainerType.E.Equals(trainerType))
                {
                    trainer = new TrainerEntity(
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                        trainerType,
                        trainerCode,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                        txtInternalEmployeeTrainerName.Text.Trim(),
                        txtInternalEmployeeTrainerTelephone.Text.Trim(),
                        chbInternalEmployeeSearchEnabled.Checked,
                        false,
                        UserHelper.GetCurrentFullUserName,
                        DateTime.Now);
                }

                PageHelper<TrainerEntity> pageHelper = (PageHelper<TrainerEntity>)Session[sessionKeyTrainersResults];

                if (string.IsNullOrWhiteSpace(hdfTrainerCodeEdit.Value))
                {
                    Tuple<bool, TrainerEntity> addResult = ObjTrainersBll.Add(trainer);
                    if (addResult.Item1)
                    {
                        if (pageHelper != null)
                        {
                            SearchResults(pageHelper.CurrentPage);
                            DisplayResults();
                        }

                        hdfSelectedRowIndex.Value = "-1";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFrombtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFrombtnAcceptClickPostBack();", true);
                    }

                    else if (!addResult.Item1)
                    {
                        TrainerEntity previousTrainer = addResult.Item2;
                        bool sameDivisionTrainers = true;

                        if (previousTrainer.Deleted && sameDivisionTrainers)
                        {
                            txtActivateDeletedTrainerType.Text = GetLocatizatedDescription(previousTrainer.TrainerType);
                            txtActivateDeletedTrainerCode.Text = previousTrainer.TrainerCode;
                            txtActivateDeletedTrainerName.Text = previousTrainer.TrainerName;

                            hdfActivateDeletedTrainerType.Value = previousTrainer.TrainerType.ToString();
                            hdfActivateDeletedTrainerCode.Value = previousTrainer.TrainerCode;

                            divActivateDeletedDialog.InnerHtml =
                                sameDivisionTrainers ?
                                    Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog")) :
                                    Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDeleted();", true);
                        }

                        else
                        {
                            txtDuplicatedTrainerType.Text = GetLocatizatedDescription(previousTrainer.TrainerType);
                            txtDuplicatedTrainerCode.Text = previousTrainer.TrainerCode;
                            txtDuplicatedTrainerName.Text = previousTrainer.TrainerName;
                            pnlDuplicatedDialogDataDetail.Visible = sameDivisionTrainers;

                            divDuplicatedDialogText.InnerHtml =
                                sameDivisionTrainers ?
                                    Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog")) :
                                    Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    ObjTrainersBll.Edit(trainer);

                    if (pageHelper != null)
                    {
                        pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.TrainerType == trainer.TrainerType && x.TrainerCode == trainer.TrainerCode));

                        pageHelper.ResultList.Insert(Convert.ToInt32(hdfSelectedRowIndex.Value), trainer);
                        DisplayResults();
                    }

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFrombtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFrombtnAcceptClickPostBack();", true);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFrombtnAcceptClickPostBackError{0}", Guid.NewGuid()), "ShowModalPanelForTrainerType(); ", true);

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
                string trainerCode =
                    !string.IsNullOrWhiteSpace(hdfActivateDeletedTrainerCode.Value) ?
                        hdfActivateDeletedTrainerCode.Value :
                        null;

                TrainerType? trainerType =
                    !string.IsNullOrWhiteSpace(hdfActivateDeletedTrainerType.Value) ?
                        HrisEnum.ParseEnumByName<TrainerType>(hdfActivateDeletedTrainerType.Value) :
                        (TrainerType?)null;

                if (!string.IsNullOrWhiteSpace(trainerCode) && trainerType.HasValue)
                {
                    TrainerEntity trainer = null;

                    if (TrainerType.EC.Equals(trainerType) || TrainerType.EP.Equals(trainerType) || TrainerType.GA.Equals(trainerType))
                    {
                        trainer = new TrainerEntity(
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                            trainerType.Value,
                            trainerCode,
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                            txtExternalsTrainerName.Text,
                            txtExternalsTrainerTelephone.Text,
                            chbExternalsSearchEnabled.Checked,
                            true,
                            UserHelper.GetCurrentFullUserName,
                            DateTime.Now);
                    }

                    else if (TrainerType.E.Equals(trainerType))
                    {
                        trainer = new TrainerEntity(
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                            trainerType.Value,
                            trainerCode,
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                            txtInternalEmployeeTrainerName.Text,
                            txtInternalEmployeeTrainerTelephone.Text,
                            chbInternalEmployeeSearchEnabled.Checked,
                            true,
                            UserHelper.GetCurrentFullUserName,
                            DateTime.Now);
                    }

                    PageHelper<TrainerEntity> pageHelper = (PageHelper<TrainerEntity>)Session[sessionKeyTrainersResults];

                    //activate the deleted item
                    if (chbActivateDeleted.Checked)
                    {
                        ObjTrainersBll.Activate(trainer);

                        if (pageHelper != null)
                        {
                            SearchResults(pageHelper.CurrentPage);
                            DisplayResults();
                        }

                        hdfSelectedRowIndex.Value = "-1";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                    }

                    //update and activate the deleted item
                    else
                    {
                        if (trainer != null && pageHelper != null)
                        {
                            trainer.Deleted = false;

                            ObjTrainersBll.Edit(trainer);

                            string position = hdfSelectedRowIndex.Value;

                            SearchResults(pageHelper.CurrentPage);
                            DisplayResults();

                            hdfSelectedRowIndex.Value = position;
                        }

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , Convert.ToString(GetLocalResourceObject("msj000.Text")));
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
        /// Handles the btnAdd click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                txtSearchEmployees.Enabled = true;
                txtSearchEmployees.Attributes.Remove("readonly");
                cboTrainerType.Enabled = true;
                cboTrainerType.Attributes.Remove("readonly");
                txtExternalsTrainerCode.Enabled = true;


                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAddClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAddClickPostBack(); }}, 200);", true);
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
                    string selectedTrainerCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainerCode"]);
                    string selectedTrainerTypeValue = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainerType"]);
                    TrainerType selectedTrainerType = HrisEnum.ParseEnumByName<TrainerType>(selectedTrainerTypeValue);

                    TrainerEntity trainer = ObjTrainersBll.ListByKey(
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                        selectedTrainerType, selectedTrainerCode);

                    hdfTrainerCodeEdit.Value = trainer.TrainerCode;
                    hdfTrainerTypeEdit.Value = trainer.TrainerType.ToString();

                    cboTrainerType.SelectedValue = trainer.TrainerType.ToString();
                    cboTrainerType.Enabled = false;
                    txtSearchEmployees.Enabled = false;
                    txtSearchEmployees.Attributes.Add("readonly", "readonly");

                    txtExternalsTrainerCode.Text = trainer.TrainerCode;
                    txtExternalsTrainerCode.Enabled = false;
                    txtExternalsTrainerName.Text = trainer.TrainerName;
                    txtExternalsTrainerTelephone.Text = trainer.Telephone;
                    chbExternalsSearchEnabled.Checked = trainer.SearchEnabled;

                    txtInternalEmployeeTrainerCode.Text = trainer.TrainerCode;
                    txtInternalEmployeeTrainerCode.Enabled = false;
                    txtInternalEmployeeTrainerName.Text = trainer.TrainerName;
                    txtInternalEmployeeTrainerTelephone.Text = trainer.Telephone;
                    chbInternalEmployeeSearchEnabled.Checked = trainer.SearchEnabled;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);
                }

                else
                {
                    btnAdd.Disabled = false;
                    btnEdit.Disabled = false;
                    btnDelete.Disabled = false;
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

        #region Employees

        /// <summary>
        /// Handles the cboTrainerType selected index changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void CboTrainerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hdfTrainerTypeEdit.Value = cboTrainerType.SelectedValue;
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppSearchEmployees
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppSearchEmployees
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the txtSearchEmployees text changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void TxtSearchEmployees_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FilterSearchedEmployees();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppSearchEmployees
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppSearchEmployees
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        #endregion

        #region Courses

        /// <summary>
        /// Handles the btnRefreshAssociatedCourses click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRefreshAssociatedTrainingCourses_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshAssociatedCourses();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppAssociatedTrainingCourses
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppAssociatedTrainingCourses
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnCoursesByTrainers click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCoursesByTrainers_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    LoadAssociatedCoursesTraining(true);
                    List<CourseEntity> associatedCoursesTraining = (List<CourseEntity>)Session[sessionKeyAssociatedCoursesTraining];

                    rptAssociatedTrainingCourses.DataSource = associatedCoursesTraining;
                    rptAssociatedTrainingCourses.DataBind();
                    uppAssociatedTrainingCourses.Update();

                    txtSearchCourses.Text = string.Empty;
                    uppSearchBarCourse.Update();

                    txtSearchCourses.Text = string.Empty;
                    rptCourses.DataSource = null;
                    rptCourses.DataBind();
                    uppSearchCourses.Update();

                    lblSearchCoursesResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchCoursesResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchCoursesResultsCount")), 0, 0));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnCoursesClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnCoursesClickPostBack(); }}, 200);", true);
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
        /// Handles the txtSearchCourses text changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void TxtSearchCourses_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FilterSearchedCourses();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppSearchCourses, TipoMensaje.Error, ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppSearchCourses, TipoMensaje.Error, newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnDeleteAssociatedCourse click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDeleteAssociatedCourse_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedTrainerCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    string selectedTrainerType = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainerType"]);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string courseCode = ((TextBox)htmlButton.Parent.FindControl("hdfAssociatedCourseCode")).Text;

                    LoadAssociatedCoursesTraining(false);
                    List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCoursesTraining];

                    List<CourseEntity> removedCourses =
                        associatedCourses.Where(at =>
                            string.Equals(courseCode, at.CourseCode)).ToList();

                    if (removedCourses.Count >= 1)
                    {
                        ObjCourseBll.DeleteTrainerByCourse(
                            removedCourses[0],
                            new TrainerEntity(
                                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                                HrisEnum.ParseEnumByName<TrainerType>(selectedTrainerType),
                                selectedTrainerCode,
                                UserHelper.GetCurrentFullUserName)
                        );

                        associatedCourses.RemoveAll(at => string.Equals(courseCode, at.CourseCode));

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteCourseClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnDeleteCourseClickPostBack('{0}'); }}, 200);", btnDelete.ClientID), true);
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
        /// Handles the btnAddCourse click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddCourse_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedTrainerCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    string selectedTrainerType = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainerType"]);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string courseCode = ((TextBox)htmlButton.Parent.FindControl("hdfCourseCode")).Text;

                    LoadCourses();
                    DataTable courses = (DataTable)Session[sessionKeyCoursesResults];

                    LoadAssociatedCoursesTraining(false);
                    List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCoursesTraining];

                    List<CourseEntity> addedCourse =
                        courses.AsEnumerable().Where(t =>
                            string.Equals(courseCode, t.Field<string>("CourseCode"))).Select(t =>
                                new CourseEntity(
                                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                                    t.Field<string>("CourseCode"),
                                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                                    t.Field<string>("CourseName"),
                                    UserHelper.GetCurrentFullUserName
                                    )).ToList();

                    if (addedCourse.Count >= 1)
                    {
                        ObjCourseBll.AddTrainerByCourse(
                            addedCourse[0],
                            new TrainerEntity(
                                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                                HrisEnum.ParseEnumByName<TrainerType>(selectedTrainerType),
                                selectedTrainerCode,
                                UserHelper.GetCurrentFullUserName)

                            );

                        associatedCourses.Add(addedCourse[0]);

                        hdfModeAddTraining.Value = bool.TrueString;

                        rptAssociatedTrainingCourses.DataSource = associatedCourses;
                        rptAssociatedTrainingCourses.DataBind();
                        FilterSearchedCourses();

                        uppAssociatedTrainingCourses.Update();
                        uppSearchCourses.Update();
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAddCourseClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnAddCourseClickPostBack('{0}'); }}, 200);", btnAdd.ClientID), true);
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
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("UQ_CoursesByThematicAreas_CourseCode"))
                    {
                        MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msjThematicAreaRelatedCourseException").ToString());
                    }

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
        /// Handles the RptAssociatedTrainingCourses ItemDataBound event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void RptAssociatedTrainingCourses_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item || e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            Control divDeleteControlsCourses = e.Item.FindControl("divDeleteControlsCourses") as Control;

            if (divDeleteControlsCourses != null)
            {
                HtmlButton btnDeleteAssociatedCourse = divDeleteControlsCourses.FindControl("btnDeleteAssociatedCourse") as HtmlButton;
                HiddenField hdfAssociatedCourseSearchEnabled = divDeleteControlsCourses.FindControl("hdfAssociatedCourseSearchEnabled") as HiddenField;

                btnDeleteAssociatedCourse.Visible = bool.Parse(hdfAssociatedCourseSearchEnabled.Value);
                btnDeleteAssociatedCourse.Disabled = bool.Parse(hdfAssociatedCourseSearchEnabled.Value);

                var disabled = bool.Parse(hdfAssociatedCourseSearchEnabled.Value) ? bool.FalseString : bool.TrueString;

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("DisableBtnAdd{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ DisableBtnAdd('{0}', '{1}'); }}, 200);", btnDeleteAssociatedCourse.ClientID, disabled), true);
            }
        }

        /// <summary>
        /// Handles the RptCourses ItemDataBound event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void RptCourses_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item || e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            Control divAddControlsCourses = e.Item.FindControl("divAddControlsCourses") as Control;

            if (divAddControlsCourses != null)
            {
                HtmlButton btnAddCourse = divAddControlsCourses.FindControl("btnAddCourse") as HtmlButton;
                HiddenField hdfCourseSearchEnabled = divAddControlsCourses.FindControl("hdfCourseSearchEnabled") as HiddenField;

                btnAddCourse.Visible = bool.Parse(hdfCourseSearchEnabled.Value);
                btnAddCourse.Disabled = bool.Parse(hdfCourseSearchEnabled.Value);

                var disabled = bool.Parse(hdfCourseSearchEnabled.Value) ? bool.FalseString : bool.TrueString;

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("DisableBtnAdd{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ DisableBtnAdd('{0}', '{1}'); }}, 200);", btnAddCourse.ClientID, disabled), true);
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
                    LoadAssociatedCoursesTraining(true);
                    List<CourseEntity> associatedCoursesTraining = (List<CourseEntity>)Session[sessionKeyAssociatedCoursesTraining];

                    if (associatedCoursesTraining.Count > 0)
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Advertencia,
                            GetLocalResourceObject("msgAssociatedCourses").ToString());

                        return;
                    }

                    string selectedTrainerCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainerCode"]);
                    string selectedTrainerTypeValue = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainerType"]);
                    TrainerType selectedTrainerType = HrisEnum.ParseEnumByName<TrainerType>(selectedTrainerTypeValue);

                    ObjTrainersBll.Delete(
                        new TrainerEntity(
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                            selectedTrainerType,
                            selectedTrainerCode,
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                            null,
                            null,
                            true,
                            true,
                            UserHelper.GetCurrentFullUserName,
                            DateTime.Now));

                    PageHelper<TrainerEntity> pageHelper = (PageHelper<TrainerEntity>)Session[sessionKeyTrainersResults];

                    pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.TrainerCode == selectedTrainerCode));
                    pageHelper.TotalResults--;

                    if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                    {
                        SearchResults(pageHelper.TotalPages - 1);
                        PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages - 1);
                    }

                    pageHelper.UpdateTotalPages();
                    RefreshTable();

                    hdfSelectedRowIndex.Value = "-1";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page , TipoMensaje.Error , GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(Page , TipoMensaje.Error, ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error , newEx.Message);
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
                hdfTrainerCodeFilter.Value = txtTrainerCodeFilter.Text;
                hdfTrainerNameFilter.Value = txtTrainerNameFilter.Text;
                hdfTrainerTypeValueFilter.Value = GetTrainerTypeFilterSelectedValue();
                hdfTrainerTypeTextFilter.Value = GetTrainerTypeFilterSelectedText();

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
            if (Session[sessionKeyTrainersResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<TrainerEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Load empty data structure for employees
        /// </summary>
        /// <returns>Empty data structure for employees</returns>
        private DataTable LoadEmptyEmployees()
        {
            DataTable employees = new DataTable();
            employees.Columns.Add("EmployeeCode", typeof(string));
            employees.Columns.Add("EmployeeName", typeof(string));

            return employees;
        }

        /// <summary>
        /// Load employees from database
        /// </summary>        
        private void LoadEmployees()
        {
            if (Session[sessionKeyEmployeesResults] == null)
            {
                DataTable employees = LoadEmptyEmployees();

                List<EmployeeEntity> registeredEmployees = ObjEmployeesBll.ListByDivision(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                    , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredEmployees.ForEach(x => employees.Rows.Add(x.EmployeeCode, x.EmployeeName));

                Session[sessionKeyEmployeesResults] = employees;
            }
        }

        /// <summary>
        /// Filter the searched employees by search tokens
        /// </summary>
        private void FilterSearchedEmployees()
        {
            LoadEmployees();
            DataTable employees = (DataTable)Session[sessionKeyEmployeesResults];

            if (!string.IsNullOrWhiteSpace(txtSearchEmployees.Text))
            {
                char[] charSeparators = new char[] { ',', ' ' };
                string[] terms = txtSearchEmployees.Text.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                string filter = string.Empty;
                foreach (string term in terms)
                {
                    string subfilter = string.Format("(EmployeeCode Like '%{0}%' OR EmployeeName Like '%{0}%')", term);
                    filter = string.Format("{0} {1} {2} ", filter, string.IsNullOrWhiteSpace(filter) ? "" : " AND ", subfilter);
                }

                employees.DefaultView.RowFilter = filter;

                DataTable finalTopResults = LoadEmptyEmployees();
                int minRowCount = Math.Min(10, employees.DefaultView.Count);

                for (int i = 0; i < minRowCount; i++)
                {
                    finalTopResults.ImportRow(employees.DefaultView[i].Row);
                }

                rptEmployees.DataSource = finalTopResults;
                rptEmployees.DataBind();

                lblSearchEmployeesResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchEmployeesResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchEmployeesResultsCount")), minRowCount, employees.DefaultView.Count));

                if (finalTopResults.Rows.Count == 0)
                {
                    MensajeriaHelper.MostrarMensaje(Page
                    , TipoMensaje.Validacion
                    , GetLocalResourceObject("msgValidacionNoRegistrosFiltrados").ToString());
                }
            }

            else
            {
                employees.DefaultView.RowFilter = "1 = 0";
                rptEmployees.DataSource = null;
                rptEmployees.DataBind();

                lblSearchEmployeesResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchEmployeesResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchEmployeesResultsCount")), 0, 0));
            }

            Session[sessionKeyEmployeesResults] = employees;
            ScriptManager.RegisterStartupScript(uppSearchEmployees, uppSearchEmployees.GetType(), string.Format("ReturnFromSearchEmployeesPostBack{0}", Guid.NewGuid()), "ReturnFromSearchEmployeesPostBack()", true);
        }

        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for Courses</returns>
        private DataTable LoadEmptyCourses()
        {
            DataTable Courses = new DataTable();
            Courses.Columns.Add("CourseCode", typeof(string));
            Courses.Columns.Add("DivisionCode", typeof(int));
            Courses.Columns.Add("CourseName", typeof(string));
            Courses.Columns.Add("SearchEnabled", typeof(bool));

            return Courses;
        }

        /// <summary>
        /// Load Courses from database
        /// </summary>        
        private void LoadCourses(bool RefreshData = false)
        {
            if (Session[sessionKeyCoursesResults] == null || RefreshData)
            {
                DataTable courses = LoadEmptyCourses();

                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                string selectedTrainerCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                List<CourseEntity> registeredCourses = ObjCourseBll.ListByCourseByTrainersNotAssociated(workingDivision.GeographicDivisionCode, workingDivision.DivisionCode, selectedTrainerCode);

                registeredCourses.ForEach(x => courses.Rows.Add(x.CourseCode,x.DivisionCode , x.CourseName, x.SearchEnabled));

                Session[sessionKeyCoursesResults] = courses;
            }
        }

        /// <summary>
        /// Load associated training programs
        /// </summary>        
        private void LoadAssociatedCoursesTraining(bool forceRead)
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                string selectedTrainerCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                if (Session[sessionKeyAssociatedCoursesTraining] == null || forceRead)
                {
                    Session[sessionKeyAssociatedCoursesTraining] = ObjCourseBll.ListByCoursesByTrainersAssociated(workingDivision.GeographicDivisionCode, workingDivision.DivisionCode, selectedTrainerCode);
                }
            }

            else
            {
                Session[sessionKeyAssociatedCoursesTraining] = new List<TrainingProgramEntity>();
            }
        }

        /// <summary>
        /// Filter the searched Courses by search tokens
        /// </summary>
        private void FilterSearchedCourses()
        {
            LoadCourses(true);
            DataTable existentCourses = (DataTable)Session[sessionKeyCoursesResults];
            DataTable courses = existentCourses.Copy();

            LoadAssociatedCoursesTraining(false);
            List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCoursesTraining];

            associatedCourses.ForEach(at =>
            {
                courses.AsEnumerable().Where(t =>
                    string.Equals(at.CourseCode, t.Field<string>("CourseCode"))).ToList().ForEach(t => courses.Rows.Remove(t));
            });

            if (!string.IsNullOrWhiteSpace(txtSearchCourses.Text))
            {
                char[] charSeparators = new char[] { ',', ' ' };
                string[] terms = txtSearchCourses.Text.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                string filter = string.Empty;
                foreach (string term in terms)
                {
                    string subfilter = string.Format("(CourseCode Like '%{0}%' OR CourseName Like '%{0}%')", term);
                    filter = string.Format("{0} {1} {2} ", filter, string.IsNullOrWhiteSpace(filter) ? "" : " AND ", subfilter);
                }

                courses.DefaultView.RowFilter = filter;

                DataTable finalTopResults = LoadEmptyCourses();

                int minRowCount = Math.Min(10, courses.DefaultView.Count);
                for (int i = 0; i < minRowCount; i++)
                {
                    finalTopResults.ImportRow(courses.DefaultView[i].Row);
                }

                rptCourses.DataSource = finalTopResults;
                rptCourses.DataBind();

                lblSearchCoursesResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchCoursesResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchCoursesResultsCount")), minRowCount, courses.DefaultView.Count));
            }

            else
            {
                courses.DefaultView.RowFilter = "1 = 0";
                rptCourses.DataSource = null;
                rptCourses.DataBind();

                lblSearchCoursesResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchCoursesResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchCoursesResultsCount")), 0, 0));
            }

            ScriptManager.RegisterStartupScript(uppSearchCourses, uppSearchCourses.GetType(), string.Format("ReturnFromSearchCoursesPostBack{0}", Guid.NewGuid()), "ReturnFromSearchCoursesPostBack()", true);
        }

        /// <summary>
        /// Refresh the associated courses tables by rebinding data
        /// </summary>
        private void RefreshAssociatedCourses()
        {
            if (Session[sessionKeyAssociatedCoursesTraining] != null)
            {
                rptAssociatedTrainingCourses.DataSource = Session[sessionKeyAssociatedCoursesTraining];
                rptAssociatedTrainingCourses.DataBind();
            }
        }

        /// <summary>
        /// Load the trainer types as options for combobox
        /// </summary>
        /// <returns>The trainer types as options for combobox</returns>
        private IDictionary<string, string> LoadtrainerTypes()
        {
            Dictionary<string, string> trainerTypesOptions = new Dictionary<string, string>
            {
                { "-1", "" }
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
        /// Returns the selected trainer type
        /// </summary>
        /// <returns>The selected trainer type value</returns>
        private string GetTrainerTypeFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboTrainerTypeFilter.SelectedValue)
                && !"-1".Equals(cboTrainerTypeFilter.SelectedValue))
            {
                selected = cboTrainerTypeFilter.SelectedValue;
            }
            return selected;
        }

        /// <summary>
        /// Returns the selected trainer type
        /// </summary>
        /// <returns>The selected trainer type text</returns>
        private string GetTrainerTypeFilterSelectedText()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboTrainerTypeFilter.SelectedValue)
                && !"-1".Equals(cboTrainerTypeFilter.SelectedValue))
            {
                selected = cboTrainerTypeFilter.SelectedItem.Text;
            }
            return selected;
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<TrainerEntity> SearchResults(int page)
        {
            string trainerCode = string.IsNullOrWhiteSpace(hdfTrainerCodeFilter.Value) ? null : hdfTrainerCodeFilter.Value;
            string trainerName = string.IsNullOrWhiteSpace(hdfTrainerNameFilter.Value) ? null : hdfTrainerNameFilter.Value;
            string trainerType = string.IsNullOrWhiteSpace(hdfTrainerTypeValueFilter.Value) ? null : hdfTrainerTypeValueFilter.Value;

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<TrainerEntity> pageHelper = ObjTrainersBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, 
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, 
                trainerCode, trainerName, trainerType, 
                sortExpression, sortDirection, page);

            Session[sessionKeyTrainersResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyTrainersResults] != null)
            {
                PageHelper<TrainerEntity> pageHelper = (PageHelper<TrainerEntity>)Session[sessionKeyTrainersResults];

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

    }
}