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
    public partial class ThematicAreas : Page
    {
        [Dependency]
        public IThematicAreasBll<ThematicAreaEntity> ObjThematicAreasBll { get; set; }

        [Dependency]
        public ICoursesBll<CourseEntity> ObjCoursesBll { get; set; }

        //session key for the results
        readonly string sessionKeyThematicAreasResults = "ThematicAreas-ThematicAreasResults";
        readonly string sessionKeyCoursesResults = "ThematicAreas-CoursesResults";
        readonly string sessionKeyAssociatedCourses = "ThematicAreas-CoursesAssociated";


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
                if (ObjThematicAreasBll == null)
                {
                    ObjThematicAreasBll = Application.GetContainer().Resolve<IThematicAreasBll<ThematicAreaEntity>>();
                }

                if (!IsPostBack)
                {
                    //fire the event
                    BtnSearch_ServerClick(sender, e);
                }

                chkSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chkSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));

                //activate the pager
                if (Session[sessionKeyThematicAreasResults] != null)
                {
                    PageHelper<ThematicAreaEntity> pageHelper = (PageHelper<ThematicAreaEntity>)Session[sessionKeyThematicAreasResults];
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
                string hiddenThematicAreaCode = !string.IsNullOrWhiteSpace(hdfThematicAreaCodeEdit.Value) ?
                    Convert.ToString(hdfThematicAreaCodeEdit.Value) : "-1";

                string thematicAreaCode = txtThematicAreaCode.Text.Trim();
                string thematicAreaName = txtThematicAreaName.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                if (hiddenThematicAreaCode.Equals("-1"))
                {
                    Tuple<bool, ThematicAreaEntity> addResult = ObjThematicAreasBll.Add(workingDivision.GeographicDivisionCode, 
                        thematicAreaCode, 
                        workingDivision.DivisionCode, 
                        thematicAreaName, 
                        searchEnable, 
                        lastModifiedUser);

                    if (addResult.Item1)
                    {
                        RefreshTable();

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else if (!addResult.Item1)
                    {
                        ThematicAreaEntity previousThematicArea = addResult.Item2;
                        if (previousThematicArea.Deleted)
                        {
                            txtActivateDeletedThematicAreaCode.Text = previousThematicArea.ThematicAreaCode;
                            txtActivateDeletedThematicAreaName.Text = previousThematicArea.ThematicAreaName;

                            divActivateDeletedDialog.InnerHtml = thematicAreaCode.Equals(previousThematicArea.ThematicAreaCode) ?
                               GetLocalResourceObject("lblTextActivateDeletedDialog").ToString() : GetLocalResourceObject("lblTextActivateDeletedDialogName").ToString();


                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                        }

                        else
                        {
                            txtDuplicatedThematicAreaCode.Text = previousThematicArea.ThematicAreaCode;
                            txtDuplicatedThematicAreaName.Text = previousThematicArea.ThematicAreaName;

                            divActivateDeletedDialog.InnerHtml = thematicAreaCode.Equals(previousThematicArea.ThematicAreaCode) ?
                               GetLocalResourceObject("lblTextActivateDeletedDialog").ToString() : GetLocalResourceObject("lblTextActivateDeletedDialogName").ToString();

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    var result = ObjThematicAreasBll.Edit(workingDivision.GeographicDivisionCode, 
                        workingDivision.DivisionCode, 
                        thematicAreaCode, 
                        thematicAreaName, 
                        searchEnable, 
                        false, 
                        lastModifiedUser);

                    if (result.Item1)
                    {
                        RefreshTable();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBack();", true);
                    }

                    else
                    {
                        ThematicAreaEntity previousThematicArea = result.Item2;
                        if (previousThematicArea.Deleted)
                        {
                            txtActivateDeletedThematicAreaCode.Text = previousThematicArea.ThematicAreaCode;
                            txtActivateDeletedThematicAreaName.Text = previousThematicArea.ThematicAreaName;
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
                txtThematicAreaCode.ReadOnly = false;
                txtThematicAreaCode.Enabled = true;
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
                string thematicAreaName = txtThematicAreaName.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    string thematicAreaCodeDELETE = txtActivateDeletedThematicAreaCode.Text.Trim();
                    ObjThematicAreasBll.Activate(workingDivision.GeographicDivisionCode, workingDivision.DivisionCode, thematicAreaCodeDELETE, lastModifiedUser);

                    RefreshTable();
                    hdfSelectedRowIndex.Value = "0";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                }

                //update and activate the deleted item
                else
                {
                    string thematicAreaCodeDELETE = txtActivateDeletedThematicAreaCode.Text.Trim();
                    ObjThematicAreasBll.Edit(workingDivision.GeographicDivisionCode
                        , workingDivision.DivisionCode
                        , thematicAreaCodeDELETE
                        , thematicAreaName
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
                    DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                    string selectedThematicAreaCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    ThematicAreaEntity thematicArea = ObjThematicAreasBll.ListByCode(workingDivision.GeographicDivisionCode
                        , workingDivision.DivisionCode
                        , selectedThematicAreaCode);

                    hdfThematicAreaCodeEdit.Value = selectedThematicAreaCode;
                    txtThematicAreaCode.Text = thematicArea.ThematicAreaCode;
                    txtThematicAreaCode.Enabled = false;
                    txtThematicAreaName.Text = thematicArea.ThematicAreaName;
                    chkSearchEnabled.Checked = thematicArea.SearchEnabled;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); $('.btnsdisabled').prop('disabled',true); }}, 200);", true);
                }

                else
                {
                    btnAdd.Disabled = false;
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
                }

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }

            catch (Exception ex)
            {
                btnAdd.Disabled = false;
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
                    string selectedThematicAreaCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string courseCode = ((TextBox)htmlButton.Parent.FindControl("hdfAssociatedCourseCode")).Text;

                    LoadAssociatedCourses(false);
                    List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCourses];

                    List<CourseEntity> removedCourses =
                        associatedCourses.Where(at => string.Equals(courseCode, at.CourseCode)).ToList();

                    if (removedCourses.Count >= 1)
                    {
                        var thematicArea = new ThematicAreaEntity(
                                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                                selectedThematicAreaCode,
                                UserHelper.GetCurrentFullUserName);
                        thematicArea.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        ObjCoursesBll.DeleteCourseByThematicArea(
                            removedCourses[0],
                           thematicArea
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
                    string selectedThematicAreaCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string courseCode = ((TextBox)htmlButton.Parent.FindControl("hdfCourseCode")).Text;

                    LoadCourses();
                    DataTable courses = (DataTable)Session[sessionKeyCoursesResults];

                    LoadAssociatedCourses(false);
                    List<CourseEntity> associatedCourses = (List<CourseEntity>)Session[sessionKeyAssociatedCourses];

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
                        int flat = ObjCoursesBll.ValidateCourseByThematicArea(addedCourse[0]);
                        if (flat == 0)
                        {
                            var thematicArea = new ThematicAreaEntity(
                                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                                    selectedThematicAreaCode,
                                    UserHelper.GetCurrentFullUserName);
                            thematicArea.DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                            ObjCoursesBll.AddCourseByThematicArea(
                                addedCourse[0],
                                thematicArea
                                );

                            associatedCourses.Add(addedCourse[0]);

                            rptAssociatedCourses.DataSource = associatedCourses;
                            rptAssociatedCourses.DataBind();
                            FilterSearchedCourses();

                            uppAssociatedCourses.Update();
                            uppSearchCourses.Update();
                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAddCourseClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnAddCourseClickPostBack('{0}'); }}, 200);", btnAdd.ClientID), true);
                        }
                        else 
                        {
                            string messege = "";
                            switch (flat) 
                            {
                                case 1:
                                    messege = GetLocalResourceObject("msgAssociatedCourse").ToString();
                                    break;
                            }
                            MensajeriaHelper.MostrarMensaje((Control)sender,
                          TipoMensaje.Advertencia,
                          messege);
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
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("UQ_CoursesByThematicAreas_CourseCode"))
                    {
                        MensajeriaHelper.MostrarMensaje((Control)sender, 
                            TipoMensaje.Error, 
                            GetLocalResourceObject("msjThematicAreaRelatedCourseException").ToString());
                    }

                    MensajeriaHelper.MostrarMensaje((Control)sender, 
                        TipoMensaje.Error, 
                        ex.Message);
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
                            GetLocalResourceObject("msgAssociatedCourse").ToString());

                        return;
                    }

                    string selectedThematicAreaCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    ObjThematicAreasBll.Delete(
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                            selectedThematicAreaCode,
                            UserHelper.GetCurrentFullUserName);

                    PageHelper<ThematicAreaEntity> pageHelper = (PageHelper<ThematicAreaEntity>)Session[sessionKeyThematicAreasResults];

                    pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.ThematicAreaCode == selectedThematicAreaCode));
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
            if (Session[sessionKeyThematicAreasResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<ThematicAreaEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Private methods

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
        /// Load associated courses
        /// </summary>        
        private void LoadAssociatedCourses(bool forceRead)
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                string selectedThematicAreaCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                if (Session[sessionKeyAssociatedCourses] == null || forceRead)
                {
                    Session[sessionKeyAssociatedCourses] = ObjCoursesBll.ListByThematicArea(
                        workingDivision.GeographicDivisionCode, 
                        workingDivision.DivisionCode,
                        selectedThematicAreaCode);
                }
            }

            else
            {
                Session[sessionKeyAssociatedCourses] = new List<TrainerEntity>();
            }
        }

        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for Courses</returns>
        private DataTable LoadEmptyCourses()
        {
            DataTable Courses = new DataTable();
            Courses.Columns.Add("CourseCode", typeof(string));
            Courses.Columns.Add("CourseName", typeof(string));

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

                List<CourseEntity> registeredCourses = ObjCoursesBll.ListByDivisionNotThematicAreaAssociated(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                    , SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredCourses.ForEach(x => courses.Rows.Add(x.CourseCode, x.CourseName));

                Session[sessionKeyCoursesResults] = courses;
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

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<ThematicAreaEntity> SearchResults(int page)
        {
            string ThematicAreaCode = string.IsNullOrWhiteSpace(txtThematicAreaCodeFilter.Text) ? null : txtThematicAreaCodeFilter.Text.Trim();
            string ThematicAreaName = string.IsNullOrWhiteSpace(txtThematicAreaNameFilter.Text) ? null : txtThematicAreaNameFilter.Text.Trim();

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<ThematicAreaEntity> pageHelper = ObjThematicAreasBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, 
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, 
                ThematicAreaCode, ThematicAreaName, sortExpression, sortDirection, page);

            Session[sessionKeyThematicAreasResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyThematicAreasResults] != null)
            {
                PageHelper<ThematicAreaEntity> pageHelper = (PageHelper<ThematicAreaEntity>)Session[sessionKeyThematicAreasResults];

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