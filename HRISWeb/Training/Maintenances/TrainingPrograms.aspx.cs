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
using Unity;
using Unity.Attributes;
using Unity.Web;

namespace HRISWeb.Training.Maintenances
{
    public partial class TrainingPrograms : Page
    {
        [Dependency]
        public ITrainingProgramsBll<TrainingProgramEntity> ObjTrainingProgramsBll { get; set; }

        [Dependency]
        public ICoursesBll<CourseEntity> ObjCoursesBll { get; set; }

        //session key for the results
        readonly string sessionKeyTrainingProgramsResults = "TrainingPrograms-TrainingProgramsResults";
        readonly string sessionKeyCoursesResults = "TrainingPrograms-CoursesResults";
        readonly string sessionKeyAssociatedCourses = "TrainingPrograms-CoursesAssociated";

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
                if (ObjTrainingProgramsBll == null)
                {
                    ObjTrainingProgramsBll = Application.GetContainer().Resolve<ITrainingProgramsBll<TrainingProgramEntity>>();
                }

                if (!IsPostBack)
                {
                    //fire the event
                    BtnSearch_ServerClick(sender, e);
                }

                chkSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chkSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));

                //activate the pager
                if (Session[sessionKeyTrainingProgramsResults] != null)
                {
                    PageHelper<TrainingProgramEntity> pageHelper = (PageHelper<TrainingProgramEntity>)Session[sessionKeyTrainingProgramsResults];
                    PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
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
                string hdfTrainingProgramGeographicDivisionCode = !string.IsNullOrWhiteSpace(hdfTrainingProgramGeographicDivisionCodeEdit.Value) ?
                    Convert.ToString(hdfTrainingProgramGeographicDivisionCodeEdit.Value) : "-1";

                string hdfTrainingProgramCode = !string.IsNullOrWhiteSpace(hdfTrainingProgramCodeEdit.Value) ?
                    Convert.ToString(hdfTrainingProgramCodeEdit.Value) : "-1";

                string trainingProgramCode = txtTrainingProgramCode.Text.Trim();
                string trainingProgramName = txtTrainingProgramName.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;

                string lastModifiedUser = UserHelper.GetCurrentFullUserName;
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                if (hdfTrainingProgramCode.Equals("-1"))
                {
                    Tuple<bool, TrainingProgramEntity> addResult = ObjTrainingProgramsBll.Add(
                        workingDivision.GeographicDivisionCode,
                        trainingProgramCode,
                        workingDivision.DivisionCode,
                        trainingProgramName,
                        searchEnable,
                        lastModifiedUser);

                    if (addResult.Item1)
                    {
                        RefreshTable();

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "$('.btnAccept').prop('disabled',true); ReturnFromBtnAcceptClickPostBack();", true);
                    }

                    else if (!addResult.Item1)
                    {
                        TrainingProgramEntity previousTrainingProgram = addResult.Item2;

                        if (previousTrainingProgram.Deleted)
                        {
                            txtActivateDeletedTrainingProgramCode.Text = previousTrainingProgram.TrainingProgramCode;
                            txtActivateDeletedTrainingProgramName.Text = previousTrainingProgram.TrainingProgramName;

                            hdfActivateDeletedTrainingProgramGeographicDivisionCode.Value = previousTrainingProgram.GeographicDivisionCode;
                            hdfActivateDeletedTrainingProgramCode.Value = previousTrainingProgram.TrainingProgramCode;

                            divActivateDeletedDialog.InnerHtml = previousTrainingProgram.TrainingProgramCode == trainingProgramCode ?
                                GetLocalResourceObject("lblTextDuplicatedDialog").ToString() : GetLocalResourceObject("lblTextDuplicatedDialogName").ToString();

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                        }

                        else
                        {
                            txtDuplicatedTrainingProgramCode.Text = previousTrainingProgram.TrainingProgramCode;
                            txtDuplicatedTrainingProgramName.Text = previousTrainingProgram.TrainingProgramName;

                            divDuplicatedDialogText.InnerHtml = previousTrainingProgram.TrainingProgramCode == trainingProgramCode ?
                                GetLocalResourceObject("lblTextDuplicatedDialog").ToString() : GetLocalResourceObject("lblTextDuplicatedDialogName").ToString();

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated(); ", true);
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    var result = ObjTrainingProgramsBll.Edit(
                         hdfTrainingProgramGeographicDivisionCode,
                         workingDivision.DivisionCode,
                         hdfTrainingProgramCode,
                         trainingProgramName,
                         searchEnable,
                         false,
                         lastModifiedUser);

                    if (result.Item1)
                    {
                        RefreshTable();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "$('.btnAccept').prop('disabled',true); ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else
                    {
                        TrainingProgramEntity previousTrainingProgram = result.Item2;
                        bool sameDivisionTrainers = false;

                        if (SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode.Equals(previousTrainingProgram.DivisionCode))
                        {
                            sameDivisionTrainers = true;
                        }

                        if (previousTrainingProgram.Deleted && sameDivisionTrainers)
                        {
                            txtActivateDeletedTrainingProgramCode.Text = previousTrainingProgram.TrainingProgramCode;
                            txtActivateDeletedTrainingProgramName.Text = previousTrainingProgram.TrainingProgramName;

                            hdfActivateDeletedTrainingProgramGeographicDivisionCode.Value = previousTrainingProgram.GeographicDivisionCode;
                            hdfActivateDeletedTrainingProgramCode.Value = previousTrainingProgram.TrainingProgramCode;

                            divActivateDeletedDialog.InnerHtml = previousTrainingProgram.TrainingProgramCode == trainingProgramCode ?
                                GetLocalResourceObject("lblTextDuplicatedDialog").ToString() : GetLocalResourceObject("lblTextDuplicatedDialogName").ToString();

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
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
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
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
                string trainingProgramGeographicDivisionGeographicDivisionCode = hdfActivateDeletedTrainingProgramGeographicDivisionCode.Value;
                string trainingProgramCode = hdfActivateDeletedTrainingProgramCode.Value;
                string trainingProgramName = txtTrainingProgramName.Text.Trim();

                bool searchEnable = chkSearchEnabled.Checked;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    ObjTrainingProgramsBll.Activate(trainingProgramGeographicDivisionGeographicDivisionCode, trainingProgramCode, lastModifiedUser);

                    RefreshTable();
                    hdfSelectedRowIndex.Value = "0";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                }

                //update and activate the deleted item
                else
                {
                    ObjTrainingProgramsBll.Edit(
                        trainingProgramGeographicDivisionGeographicDivisionCode
                        , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                        , trainingProgramCode
                        , trainingProgramName
                        , searchEnable
                        , false, lastModifiedUser);

                    RefreshTable();
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                    hdfSelectedRowIndex.Value = "-1";
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
                    string selectedTrainingProgramGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                    string selectedTrainingProgramCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingProgramCode"]);
                    int DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                    TrainingProgramEntity trainingProgram = ObjTrainingProgramsBll.ListByCode(selectedTrainingProgramGeographicDivisionCode, selectedTrainingProgramCode, DivisionCode);

                    hdfTrainingProgramGeographicDivisionCodeEdit.Value = selectedTrainingProgramGeographicDivisionCode;
                    hdfTrainingProgramCodeEdit.Value = selectedTrainingProgramCode;

                    txtTrainingProgramCode.Text = trainingProgram.TrainingProgramCode;
                    txtTrainingProgramName.Text = trainingProgram.TrainingProgramName;
                    chkSearchEnabled.Checked = trainingProgram.SearchEnabled;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);
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

        #region Courses

        /// <summary>
        /// Handles the btnRefreshAssociatedCourses click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRefreshAssociatedCourses_Click(object sender, EventArgs e)
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
                    MensajeriaHelper.MostrarMensaje(uppAssociatedCourses
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppAssociatedCourses
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnEditCourse click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEditCourses_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    LoadAssociatedCourses(true);
                    List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCourses];

                    rptAssociatedCourses.DataSource = associatedCourses;
                    rptAssociatedCourses.DataBind();
                    uppAssociatedCourses.Update();

                    txtSearchCourses.Text = string.Empty;
                    uppSearchBarCourse.Update();

                    txtSearchCourses.Text = string.Empty;
                    rptCourses.DataSource = null;
                    rptCourses.DataBind();
                    uppSearchCourses.Update();

                    lblSearchCoursesResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchCoursesResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchCoursesResultsCount")), 0, 0));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditCoursesClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditCoursesClickPostBack(); }}, 200);", true);
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
                    MensajeriaHelper.MostrarMensaje(uppSearchCourses
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppSearchCourses
                      , TipoMensaje.Error
                      , newEx.Message);
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
                    string selectedTrainingProgramGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                    string selectedTrainingProgramCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingProgramCode"]);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string courseCode = ((TextBox)htmlButton.Parent.FindControl("hdfAssociatedCourseCode")).Text;
                    string courseGeographicDivisionCode = ((TextBox)htmlButton.Parent.FindControl("hdfAssociatedCourseGeographicDivisionCode")).Text;

                    LoadAssociatedCourses(false);
                    List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCourses];

                    List<CourseEntity> removedCourses =
                        associatedCourses.Where(at =>
                            string.Equals(courseGeographicDivisionCode, at.GeographicDivisionCode) &&
                            string.Equals(courseCode, at.CourseCode)).ToList();

                    if (removedCourses.Count >= 1)
                    {
                        ObjCoursesBll.DeleteCourseByTrainingProgram(
                            removedCourses[0],
                            new TrainingProgramEntity(
                                selectedTrainingProgramGeographicDivisionCode,
                                selectedTrainingProgramCode,
                                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
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
                    string selectedTrainingProgramGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                    string selectedTrainingProgramCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingProgramCode"]);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string courseCode = ((TextBox)htmlButton.Parent.FindControl("hdfCourseCode")).Text;
                    string courseGeographicDivisionCode = ((TextBox)htmlButton.Parent.FindControl("hdfCourseGeographicDivisionCode")).Text;
                    int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                    LoadCourses();
                    DataTable courses = (DataTable)Session[sessionKeyCoursesResults];

                    LoadAssociatedCourses(false);
                    List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCourses];

                    List<CourseEntity> addedCourse =
                        courses.AsEnumerable().Where(t =>
                            string.Equals(courseCode, t.Field<string>("CourseCode"))
                           // && string.Equals(courseGeographicDivisionCode, t.Field<string>("GeographicDivisionCode"))
                           && int.Equals(divisionCode, t.Field<int>("DivisionCode"))
                            )
                                .Select(t =>
                                    new CourseEntity(
                                        t.Field<string>("GeographicDivisionCode"),
                                        t.Field<string>("CourseCode"),
                                        t.Field<int>("DivisionCode"),
                                        t.Field<string>("CourseName"),
                                        UserHelper.GetCurrentFullUserName
                                        )).ToList();

                    if (addedCourse.Count >= 1)
                    {
                        ObjCoursesBll.AddCourseByTrainingProgram(
                            addedCourse[0],
                            new TrainingProgramEntity(
                                selectedTrainingProgramGeographicDivisionCode,
                                selectedTrainingProgramCode,
                                UserHelper.GetCurrentFullUserName)
                            );

                        associatedCourses.Add(addedCourse[0]);

                        rptAssociatedCourses.DataSource = associatedCourses;
                        rptAssociatedCourses.DataBind();
                        FilterSearchedCourses();

                        uppAssociatedCourses.Update();
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
                    LoadAssociatedCourses(true);
                    List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCourses];

                    if (associatedCourses.Count > 0)
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Advertencia,
                            GetLocalResourceObject("msgAssociatedCourses").ToString());

                        return;
                    }

                    string selectedTrainingProgramGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                    string selectedTrainingProgramCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingProgramCode"]);

                    List<CourseEntity> coursesAssociated = ObjCoursesBll.ListByTrainingProgram
                        (
                        selectedTrainingProgramGeographicDivisionCode,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                        selectedTrainingProgramCode);
                    if (coursesAssociated.Count == 0)
                    {
                        ObjTrainingProgramsBll.Delete(
                            selectedTrainingProgramGeographicDivisionCode,
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                            selectedTrainingProgramCode,
                            UserHelper.GetCurrentFullUserName);

                        PageHelper<TrainingProgramEntity> pageHelper = (PageHelper<TrainingProgramEntity>)Session[sessionKeyTrainingProgramsResults];

                        pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.TrainingProgramCode == selectedTrainingProgramCode));
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
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msjTrainingProgramAsociatedCoursesException").ToString());
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException || ex is PresentationException)
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
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
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
        /// Handles the grvList pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvList_PreRender(object sender, EventArgs e)
        {
            if ((grvList.ShowHeader && grvList.Rows.Count > 0) || (grvList.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvList.ShowFooter && grvList.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
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
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

            PageHelper<TrainingProgramEntity> pageHelper = SearchResults(1);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults();
        }

        #endregion

        #region Private methods

        #region Courses

        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for Courses</returns>
        private DataTable LoadEmptyCourses()
        {
            DataTable Courses = new DataTable();
            Courses.Columns.Add("GeographicDivisionCode", typeof(string));
            Courses.Columns.Add("CourseCode", typeof(string));
            Courses.Columns.Add("CourseName", typeof(string));
            Courses.Columns.Add("DivisionCode", typeof(int));

            return Courses;
        }

        /// <summary>
        /// Load Courses from database
        /// </summary>        
        private void LoadCourses()
        {
            if (Session[sessionKeyCoursesResults] == null)
            {
                DataTable courses = LoadEmptyCourses();

                List<CourseEntity> registeredCourses = ObjCoursesBll.ListByDivision(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                    , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredCourses.ForEach(x => courses.Rows.Add(x.GeographicDivisionCode, x.CourseCode, x.CourseName,x.DivisionCode));

                Session[sessionKeyCoursesResults] = courses;
            }
        }

        /// <summary>
        /// Load associated courses
        /// </summary>        
        private void LoadAssociatedCourses(bool forceRead)
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                string selectedTrainingProgramGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                string selectedTrainingProgramCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingProgramCode"]);

                if (Session[sessionKeyAssociatedCourses] == null || forceRead)
                {
                    Session[sessionKeyAssociatedCourses] = ObjCoursesBll.ListByTrainingProgram(
                        selectedTrainingProgramGeographicDivisionCode,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                        selectedTrainingProgramCode);
                }
            }

            else
            {
                Session[sessionKeyAssociatedCourses] = new List<TrainerEntity>();
            }
        }

        /// <summary>
        /// Refresh the associated courses tables by rebinding data
        /// </summary>
        private void RefreshAssociatedCourses()
        {
            if (Session[sessionKeyAssociatedCourses] != null)
            {
                rptAssociatedCourses.DataSource = Session[sessionKeyAssociatedCourses];
                rptAssociatedCourses.DataBind();
            }
        }

        /// <summary>
        /// Filter the searched Courses by search tokens
        /// </summary>
        private void FilterSearchedCourses()
        {
            LoadCourses();
            DataTable existentCourses = (DataTable)Session[sessionKeyCoursesResults];
            DataTable courses = existentCourses.Copy();

            LoadAssociatedCourses(false);
            List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCourses];

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

        #endregion

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<TrainingProgramEntity> SearchResults(int page)
        {
            string TrainingProgramCode = string.IsNullOrWhiteSpace(txtTrainingProgramCodeFilter.Text) ? null : txtTrainingProgramCodeFilter.Text.Trim();
            string TrainingProgramName = string.IsNullOrWhiteSpace(txtTrainingProgramNameFilter.Text) ? null : txtTrainingProgramNameFilter.Text.Trim();

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<TrainingProgramEntity> pageHelper = ObjTrainingProgramsBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                TrainingProgramCode, TrainingProgramName, sortExpression, sortDirection, page);

            Session[sessionKeyTrainingProgramsResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyTrainingProgramsResults] != null)
            {
                PageHelper<TrainingProgramEntity> pageHelper = (PageHelper<TrainingProgramEntity>)Session[sessionKeyTrainingProgramsResults];

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