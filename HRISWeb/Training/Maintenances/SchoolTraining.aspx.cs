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

namespace HRISWeb.Training.Maintenances
{
    public partial class SchoolTraining : Page
    {
        [Dependency]
        public ISchoolTrainingBll ObjSchoolTrainingBll { get; set; }

        [Dependency]
        public ICoursesBll<CourseEntity> ObjCoursesBll { get; set; }

        readonly string sessionKeySchoolTrainingResults = "Trainers-SchoolTrainingResults";
        readonly string sessionKeyCoursesResults = "SchoolsTrainings-CoursesResults";
        readonly string sessionKeyAssociatedCourses = "SchoolsTrainings-CoursesAssociated";

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
                    //fire the event
                    BtnSearch_ServerClick(sender, e);
                }

                //activate the pager
                if (Session[sessionKeySchoolTrainingResults] != null)
                {
                    PageHelper<SchoolTrainingEntity> pageHelper = (PageHelper<SchoolTrainingEntity>)Session[sessionKeySchoolTrainingResults];
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
                var SchoolTrainingId = hdfSchoolTrainingIdEdit.Value == "-1" || string.IsNullOrEmpty(hdfSchoolTrainingIdEdit.Value) ? (int?)null : int.Parse(hdfSchoolTrainingIdEdit.Value);

                var entity = new SchoolTrainingEntity
                {
                    SchoolTrainingId = SchoolTrainingId,
                    SchoolTrainingCode = SchoolTrainingCodeEdit.Text.Trim(),
                    SchoolTrainingName = SchoolTrainingNameEdit.Text.Trim(),
                    SearchEnabled = SearchEnabledEdit.Checked,
                    Deleted = false,
                    LastModifiedUser = UserHelper.GetCurrentFullUserName,
                    GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                    DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                };

                SchoolTrainingEntity result = null;
                if (SchoolTrainingId.HasValue)
                {
                    //Editar
                    result = ObjSchoolTrainingBll.SchoolTrainingEdit(entity);
                }
                else
                {
                    //Insertar
                    result = ObjSchoolTrainingBll.SchoolTrainingAdd(entity);
                }

                if (result.ErrorNumber == 0)
                {
                    hdfSelectedRowIndex.Value = "-1";
                    RefreshTable();

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnAcceptClickPostBack(); },200);", true);
                }

                else if (result.ErrorNumber != -1 && result.ErrorNumber != -2)
                {
                    Exception exception = new Exception(result.ErrorMessage);
                    throw exception;
                }

                else
                {
                    if (result.Deleted)
                    {
                        hdfActivateDeletedSchoolTrainingId.Value = result.SchoolTrainingId?.ToString();
                        txtActivateDeletedSchoolTrainingCode.Text = result.SchoolTrainingCode;
                        txtActivateDeletedSchoolTrainingName.Text = result.SchoolTrainingName;

                        var typetext = result.ErrorNumber == -1 ?
                           GetLocalResourceObject("SchoolTrainingCodeHeaderText").ToString() : GetLocalResourceObject("SchoolTrainingNameHeaderText").ToString();

                        divActivateDeletedDialog.InnerHtml = string.Format(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails").ToString(), typetext);

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "setTimeout(function () {ReturnFromBtnAcceptClickPostBackDeleted();},200); ", true);
                    }

                    else
                    {
                        txtDuplicatedSchoolTrainingCode.Text = result.SchoolTrainingCode;
                        txtDuplicatedTrainerCode.Text = result.SchoolTrainingName;

                        var typetext = result.ErrorNumber == -1 ?
                            GetLocalResourceObject("SchoolTrainingCodeHeaderText").ToString() : GetLocalResourceObject("SchoolTrainingNameHeaderText").ToString();

                        divDuplicatedDialogText.InnerHtml = string.Format(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails").ToString(), typetext);

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnAcceptClickPostBackDuplicated(); },200); ", true);
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
                    int selectedid = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["SchoolTrainingId"]);

                    var result = ObjSchoolTrainingBll.SchoolTrainingByKey(new SchoolTrainingEntity
                    {
                        SchoolTrainingId = selectedid

                    });

                    hdfSchoolTrainingIdEdit.Value = result.SchoolTrainingId?.ToString();
                    SchoolTrainingCodeEdit.Text = result.SchoolTrainingCode.Trim();
                    SchoolTrainingNameEdit.Text = result.SchoolTrainingName.Trim();
                    SearchEnabledEdit.Checked = result.SearchEnabled;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), " setTimeout(function () {  ReturnFromBtnEditClickPostBack(); },200);  ", true);
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
                    string selectedSchoolsTrainingCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["SchoolTrainingCode"]);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string courseCode = ((TextBox)htmlButton.Parent.FindControl("hdfAssociatedCourseCode")).Text;
                    string courseGeographicDivisionCode = ((TextBox)htmlButton.Parent.FindControl("hdfAssociatedCourseGeographicDivisionCode")).Text;

                    LoadAssociatedCourses(false);
                    List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCourses];

                    List<CourseEntity> removedCourses = associatedCourses.Where(at => 
                        string.Equals(courseGeographicDivisionCode, at.GeographicDivisionCode) && 
                        string.Equals(courseCode, at.CourseCode)).ToList();

                    if (removedCourses.Count >= 1)
                    {
                        ObjCoursesBll.DeleteCourseBySchoolsTraining(
                            removedCourses[0],
                            new SchoolTrainingEntity(selectedSchoolsTrainingCode, UserHelper.GetCurrentFullUserName)
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
                    string selectedSchoolsTrainingCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["SchoolTrainingCode"]);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string courseCode = ((TextBox)htmlButton.Parent.FindControl("hdfCourseCode")).Text;
                    string courseGeographicDivisionCode = ((TextBox)htmlButton.Parent.FindControl("hdfCourseGeographicDivisionCode")).Text;

                    LoadCourses();
                    DataTable courses = (DataTable)Session[sessionKeyCoursesResults];

                    LoadAssociatedCourses(false);
                    List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCourses];

                    List<CourseEntity> addedCourse = courses.AsEnumerable().Where(t =>
                            string.Equals(courseCode, t.Field<string>("CourseCode")) &&
                            string.Equals(courseGeographicDivisionCode, t.Field<string>("GeographicDivisionCode"))).Select(t => new CourseEntity
                            (
                                t.Field<string>("GeographicDivisionCode"), 
                                t.Field<string>("CourseCode"),
                                t.Field<int>("DivisionCode"),
                                t.Field<string>("CourseName"), 
                                UserHelper.GetCurrentFullUserName)
                            ).ToList();

                    if (addedCourse.Count >= 1)
                    {

                        DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                        string selectedCourseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                        List<SchoolTrainingEntity> associatedSchoolsTraining = ObjSchoolTrainingBll.ListByCourses(workingDivision.GeographicDivisionCode, workingDivision.DivisionCode, addedCourse[0].CourseCode, true);
                        if (associatedSchoolsTraining.Count >= 1)
                        {
                            string warningMessage = Convert.ToString(GetLocalResourceObject("msjSingleAssociationWarning"));
                            MensajeriaHelper.MostrarMensaje((Control)sender,
                              TipoMensaje.Advertencia,
                              warningMessage);
                            return;
                        }

                        ObjCoursesBll.AddCourseBySchoolsTraining(
                            addedCourse[0],
                            new SchoolTrainingEntity(selectedSchoolsTrainingCode, UserHelper.GetCurrentFullUserName)
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
        /// Handles the btnActivateDeletedAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">C
        protected void BtnActivateDeletedAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                var SchoolTrainingId = hdfActivateDeletedSchoolTrainingId.Value == "-1" || string.IsNullOrEmpty(hdfActivateDeletedSchoolTrainingId.Value) ? (int?)null : int.Parse(hdfActivateDeletedSchoolTrainingId.Value);

                if (SchoolTrainingId.HasValue)
                {
                    var updateEntity = new SchoolTrainingEntity();

                    if (chbActivateDeleted.Checked)
                    {
                        updateEntity = new SchoolTrainingEntity
                        {
                            SchoolTrainingId = SchoolTrainingId,
                            SearchEnabled = true,
                            Deleted = false,
                            LastModifiedUser = UserHelper.GetCurrentFullUserName
                        };
                    }

                    else
                    {
                        updateEntity = new SchoolTrainingEntity
                        {
                            SchoolTrainingId = SchoolTrainingId,
                            SchoolTrainingCode = SchoolTrainingCodeEdit.Text.Trim(),
                            SchoolTrainingName = SchoolTrainingNameEdit.Text.Trim(),
                            SearchEnabled = SearchEnabledEdit.Checked,
                            Deleted = false,
                            LastModifiedUser = UserHelper.GetCurrentFullUserName
                        };
                    }

                    var result = ObjSchoolTrainingBll.SchoolTrainingEdit(updateEntity);
                    if (result.ErrorNumber == 0)
                    {
                        hdfSelectedRowIndex.Value = "-1";
                        hdfActivateDeletedSchoolTrainingId.Value = "-1";
                        RefreshTable();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnAcceptActivateDeletedClickPostBack(); },200);  ", true);
                    }

                    else if (result.ErrorNumber != -1 && result.ErrorNumber != -2)
                    {
                        Exception exception = new Exception(result.ErrorMessage);
                        throw exception;
                    }

                    else
                    {
                        if (result.Deleted)
                        {
                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("lblTextActivateDeletedDialog")));
                        }

                        else
                        {
                            var typetext = result.ErrorNumber == -1 ? GetLocalResourceObject("SchoolTrainingCodeHeaderText").ToString() : GetLocalResourceObject("SchoolTrainingNameHeaderText").ToString();

                            var msgValidate = string.Format(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails").ToString(), typetext);

                            MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(msgValidate));
                        }
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msj000.Text")));
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
                    int selectedSchoolTrainingId = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["SchoolTrainingId"]);

                    ObjSchoolTrainingBll.SchoolTrainingEdit(
                        new SchoolTrainingEntity
                        {
                            SchoolTrainingId = selectedSchoolTrainingId,
                            SearchEnabled = false,
                            Deleted = true,
                            LastModifiedUser = UserHelper.GetCurrentFullUserName
                    });

                    PageHelper<SchoolTrainingEntity> pageHelper = (PageHelper<SchoolTrainingEntity>)Session[sessionKeySchoolTrainingResults];

                    pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.SchoolTrainingId == selectedSchoolTrainingId));
                    pageHelper.TotalResults--;

                    if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                    {
                        SearchResults(pageHelper.TotalPages - 1);
                        PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages - 1);
                    }

                    pageHelper.UpdateTotalPages();
                    RefreshTable();

                    hdfSelectedRowIndex.Value = "-1";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnDeleteClickPostBack(); },200);", true);
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
            if (Session[sessionKeySchoolTrainingResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<SchoolTrainingEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Private methods

        #region Course

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
            
            DataTable courses = LoadEmptyCourses();

            List<CourseEntity> registeredCourses = ObjCoursesBll.ListByDivision(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

            registeredCourses.ForEach(x => courses.Rows.Add(x.GeographicDivisionCode, x.CourseCode, x.CourseName, x.DivisionCode));

            Session[sessionKeyCoursesResults] = courses;
            
        }

        /// <summary>
        /// Load associated courses
        /// </summary>        
        private void LoadAssociatedCourses(bool forceRead)
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                string selectedGeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;
                int  selectedDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                string selectedSchoolsTrainingCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["SchoolTrainingCode"]);

                if (Session[sessionKeyAssociatedCourses] == null || forceRead)
                {
                    Session[sessionKeyAssociatedCourses] = ObjCoursesBll.ListBySchoolsTraining(selectedGeographicDivisionCode, selectedDivisionCode,  selectedSchoolsTrainingCode);
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
        private PageHelper<SchoolTrainingEntity> SearchResults(int page)
        {
            var Filter = new SchoolTrainingEntity
            {
                SchoolTrainingCode = string.IsNullOrWhiteSpace(SchoolTrainingCodeFilter.Text) ? null : SchoolTrainingCodeFilter.Text,
                SchoolTrainingName = string.IsNullOrWhiteSpace(SchoolTrainingNameFilter.Text) ? null : SchoolTrainingNameFilter.Text
            };

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<SchoolTrainingEntity> pageHelper = ObjSchoolTrainingBll.SchoolTrainingListByFilter(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                Filter,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                sortExpression, sortDirection, page);

            Session[sessionKeySchoolTrainingResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeySchoolTrainingResults] != null)
            {
                PageHelper<SchoolTrainingEntity> pageHelper = (PageHelper<SchoolTrainingEntity>)Session[sessionKeySchoolTrainingResults];

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