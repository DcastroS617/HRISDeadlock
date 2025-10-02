using DOLE.HRIS.Application.Business;
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
using System.Web.UI.WebControls;
using Unity.Attributes;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.Training
{
    public partial class TrainingLogbooks : Page
    {
        [Dependency]
        public IClassroomsBll<ClassroomEntity> ObjClassroomsBll { get; set; }

        [Dependency]
        public LogbooksFilesBll ObjLogbooksFilesBll { get; set; }

        [Dependency]
        public ITrainingCentersBll<TrainingCenterEntity> ObjTrainingCentersBll { get; set; }

        [Dependency]
        public ICoursesBll<CourseEntity> ObjCoursesBll { get; set; }

        [Dependency]
        public ITrainersBll<TrainerEntity> ObjTrainersBll { get; set; }
        
        [Dependency]
        public ILogbooksBll ObjLogbooksBll { get; set; }

        [Dependency]
        protected IGeneralConfigurationsBll ObjGeneralConfigurationsBll { get; set; }

        [Dependency]
        public IAdminUsersByModulesBll<AdminUserByModuleEntity> ObjAdminUsersByModulesBll { get; set; }

        //session key for the results
        readonly string sessionKeyLogbooksResults = "TrainingLogbooks-LogbooksResults";
        readonly string sessionKeyCoursesResults = "TrainingLogbooks-CoursesResults";
        readonly string sessionKeyTrainersResults = "TrainingLogbooks-TrainersResults";
        readonly string sessionKeyTrainingCentersResults = "Logbooks-TrainingCentersResults";
        readonly string sessionKeyTypeLogbookResults = "TrainingLogbooks-LogbookNumberResults";
        readonly string sessionKeyLogbookNumberResults = "TrainingLogbooks-LogbookTypeResults";
        readonly string sessionKeyStatusLogbookResults = "TrainingLogbooks-LogbookStatusResults";
        readonly string sessionKeyStatusLogbookDelete = "TrainingLogbooks-LogbookStatusDelete";

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
                    Session[sessionKeyCoursesResults] = null;
                    Session[sessionKeyTrainersResults] = null;
                    Session[sessionKeyTrainingCentersResults] = null;
                    Session[sessionKeyTypeLogbookResults] = Request.QueryString.Get("type");

                    ApplyRulesBasedTheTypeLogbook();

                    cboStatus.Items.Clear();
                    cboStatus.Items.Add(new ListItem(string.Empty, "-1"));
                    cboStatus.Items.Add(new ListItem(GetLogbookStatusLocalizatedDescription(LogbookStatus.Draft), "false"));
                    cboStatus.Items.Add(new ListItem(GetLogbookStatusLocalizatedDescription(LogbookStatus.Closed), "true"));

                    cboExistFilesFilter.Items.Clear();
                    cboExistFilesFilter.Items.Add(new ListItem(string.Empty, "-1"));
                    cboExistFilesFilter.Items.Add(new ListItem(GetLocalResourceObject("Si").ToString(), "true"));
                    cboExistFilesFilter.Items.Add(new ListItem(GetLocalResourceObject("No").ToString(), "false"));

                    UserEntity currentUser = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation);
                    string[] userAccount = currentUser.ActiveDirectoryUserAccount.Split('\\');
                    hdfCurrentUser.Value = userAccount[0] + "$" + userAccount[1];

                    //fire the event
                    BtnSearch_ServerClick(sender, e);

                    if (Session[sessionKeyStatusLogbookDelete] != null)
                    {
                        if (Session[sessionKeyStatusLogbookDelete].Equals("true"))
                        {
                            Session[sessionKeyStatusLogbookDelete] = "false";
                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);
                        }
                    }
                }

                //activate the pager
                if (Session[sessionKeyLogbooksResults] != null)
                {
                    PageHelper<LogbookEntity> pageHelper = (PageHelper<LogbookEntity>)Session[sessionKeyLogbooksResults];
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
        /// Handles the btnAdd click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Session[sessionKeyLogbookNumberResults] = string.Empty;
                Session[sessionKeyStatusLogbookResults] = "New";

                Session[sessionKeyStatusLogbookDelete] = string.Empty;
                Session[sessionKeyStatusLogbookDelete] = "false";
                System.Web.HttpContext.Current.Session["loogbook"] = string.Empty;
                Response.Redirect("TrainingLogbookRegister.aspx");
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
                    string[] status = { "Cerrada", "Closed" };

                    Session[sessionKeyLogbookNumberResults] = grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["LogbookNumber"].ToString();
                    Session[sessionKeyStatusLogbookResults] =
                        status.Contains(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["Status"].ToString()) ? "Closed" : "Draft";

                    Session[sessionKeyStatusLogbookDelete] = string.Empty;
                    Session[sessionKeyStatusLogbookDelete] = "false";

                    Response.Redirect("TrainingLogbookRegister.aspx");
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

        #region Files

        /// <summary>
        /// Handles the btnFileButton click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDownloadFile_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdfSelectedFileIndex.Value))
                {
                    var splitvalues = hdfSelectedFileValues.Value.Split(',').ToList();
                    txtFileLogbookNumber.Text = hdfSelectedFileIndex.Value;
                    txtFileTrainer.Text = splitvalues.ElementAtOrDefault(0);
                    txtFileCourse.Text = splitvalues.ElementAtOrDefault(1);
                    txFileStatus.Text = splitvalues.ElementAtOrDefault(2);

                    Session["ArchivosTemporales"] = null;

                    rptFiles.DataSource = LoadLogbookFilesList(txtFileLogbookNumber.Text);
                    rptFiles.DataBind();
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
        /// Handles the btnAddFile click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddFile_ServerClick(object sender, EventArgs e)
        {
            try
            {
                var description = LogbookFileDescripcion.Text.Trim();
                if (string.IsNullOrEmpty(description))
                {

                    MensajeriaHelper.MostrarMensaje(btnAddFile,
                        TipoMensaje.Error,
                        Convert.ToString(GetLocalResourceObject("msjlogbookDescription").ToString()));

                    return;
                }

                if (Session["ArchivosTemporales"] == null)
                {
                    MensajeriaHelper.MostrarMensaje(btnAddFile,
                        TipoMensaje.Error,
                        Convert.ToString(GetLocalResourceObject("msjFileValidation").ToString()));

                    return;
                }

                FileEntity archivos = ((List<FileEntity>)Session["ArchivosTemporales"]).FirstOrDefault();
                if (string.IsNullOrEmpty(archivos?.FileName))
                {
                    MensajeriaHelper.MostrarMensaje(btnAddFile,
                        TipoMensaje.Error,
                        Convert.ToString(GetLocalResourceObject("msjFileValidation").ToString()));

                    return;
                }

                GeneralConfigurationEntity uploadSize = ObjGeneralConfigurationsBll.ListByCode(GeneralConfigurations.UploadFileSize);

                var Filesize = (archivos.File.Length / 1024f) / 1024;
                var size = uploadSize?.GeneralConfigurationValue ?? "25";

                if (Filesize < Convert.ToDouble(size))
                {
                    var obj = new LogbooksFileEntity
                    {
                        GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                        LogbookNumber = int.Parse(txtFileLogbookNumber.Text),
                        Description = description,
                        File = archivos.File,
                        FileName = archivos.FileName,
                        FileExtension = archivos.FileExtension,
                        DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                    };

                    var result = ObjLogbooksFilesBll.LogbookFilesAdd(obj);

                    if (result.ErrorNumber != 0) throw new PresentationException(result.ErrorMessage);

                    Session["ArchivosTemporales"] = null;

                    rptFiles.DataSource = LoadLogbookFilesList(txtFileLogbookNumber.Text);
                    rptFiles.DataBind();

                    RefreshTable();
                    LogbookFileDescripcion.Text = "";
                }

                else
                {
                    if (uploadSize != null)
                    {
                        MensajeriaHelper.MostrarMensaje(btnAddFile,
                            TipoMensaje.Error,
                            Convert.ToString(string.Format(GetLocalResourceObject("msjFileSize").ToString() + " <br>", uploadSize.GeneralConfigurationValue)));
                    }

                    return;
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
        /// Handles the btnDeleteFile click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDeleteFile_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedFileUsing.Value != "-1")
                {
                    var result = ObjLogbooksFilesBll.LogbookFileDelete(new LogbooksFileEntity { LogbooksFileId = Convert.ToInt32(hdfSelectedFileUsing.Value) });

                    if (result.ErrorNumber != 0) throw new PresentationException(result.ErrorMessage + " <br>");

                    Session["ArchivosTemporales"] = null;

                    rptFiles.DataSource = LoadLogbookFilesList(txtFileLogbookNumber.Text);
                    rptFiles.DataBind();

                    hdfSelectedFileUsing.Value = "-1";

                    RefreshTable();
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

        #endregion

        /// <summary>
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                hdfLogbookNumberFilter.Value = txtLogbookNumberFilter.Text;
                hdfStartDateFilter.Value = dtpStartDateFilter.Text;
                hdfCourseValueFilter.Value = GetCourseFilterSelectedValue();
                hdfTrainerValueFilter.Value = GetTrainerFilterSelectedValue();
                hdfEndDateFilter.Value = dtpEndDateFilter.Text;
                hdfTrainingCenterValueFilter.Value = GetTrainingCenterFilterSelectedValue();
                hdfExistFilesValueFilter.Value = GetExistFilesFilterSelectedValue();
                hdfStatusValueFilter.Value = GetStatusFilterSelectedValue();

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
                    int selectedLogbookNumber = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["LogbookNumber"]);
                    string createdBy = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["CreatedBy"]);
                    bool isClosed = Convert.ToBoolean(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["IsClosed"]);

                    if (!UserHelper.GetCurrentFullUserName.Equals(createdBy, StringComparison.InvariantCultureIgnoreCase) &&
                        !ObjAdminUsersByModulesBll.IsUserAdmin(UserHelper.GetCurrentFullUserName, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, GeneralParameters.cTrainingModuleCode))
                    {
                        MensajeriaHelper.MostrarMensaje(Page, 
                            TipoMensaje.Validacion,
                            Convert.ToString(GetLocalResourceObject("msjLogbookNotDeletableAccessDenied")));
                        
                        return;
                    }

                    if (isClosed &&
                        !ObjAdminUsersByModulesBll.IsUserAdmin(UserHelper.GetCurrentFullUserName, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, GeneralParameters.cTrainingModuleCode))
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Validacion, 
                            Convert.ToString(GetLocalResourceObject("msjLogbookNotDeletableClosed")));
                       
                        return;
                    }

                    List<LogbooksFileEntity> logbooksFiles = LoadLogbookFilesList(selectedLogbookNumber.ToString());
                    if (logbooksFiles.Count > 0)
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Validacion,
                            Convert.ToString(GetLocalResourceObject("msjLogbookNotDeletableFile")));

                        return;
                    }

                    ObjLogbooksBll.Delete(
                        selectedLogbookNumber,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);

                    PageHelper<LogbookEntity> pageHelper = (PageHelper<LogbookEntity>)Session[sessionKeyLogbooksResults];

                    pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.LogbookNumber == selectedLogbookNumber));
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
            if (Session[sessionKeyLogbooksResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<LogbookEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Returns the selected course id
        /// </summary>
        /// <returns>The selected course id</returns>
        private string GetCourseFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboCourseFilter.SelectedValue) && !"-1".Equals(cboCourseFilter.SelectedValue))
            {
                selected = cboCourseFilter.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Returns the selected trainer id
        /// </summary>
        /// <returns>The selected trainer id</returns>
        private string GetTrainerFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboTrainerFilter.SelectedValue) && !"-1".Equals(cboTrainerFilter.SelectedValue))
            {
                selected = cboTrainerFilter.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Returns the selected training center id
        /// </summary>
        /// <returns>The selected training center id</returns>
        private string GetTrainingCenterFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboTrainingCenterFilter.SelectedValue) && !"-1".Equals(cboTrainingCenterFilter.SelectedValue))
            {
                selected = cboTrainingCenterFilter.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Returns the selected exist file id
        /// </summary>
        /// <returns>The selected status id</returns>
        private string GetExistFilesFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboExistFilesFilter.SelectedValue) && !"-1".Equals(cboExistFilesFilter.SelectedValue))
            {
                selected = cboExistFilesFilter.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Returns the selected status id
        /// </summary>
        /// <returns>The selected status id</returns>
        private string GetStatusFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboStatus.SelectedValue) && !"-1".Equals(cboStatus.SelectedValue))
            {
                selected = cboStatus.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Load empty data structure for Courses
        /// </summary>
        /// <returns>Empty data structure for courses</returns>
        private DataTable LoadEmptyCourses()
        {
            DataTable courses = new DataTable();
            courses.Columns.Add("CourseCode", typeof(string));
            courses.Columns.Add("CourseName", typeof(string));
            courses.Columns.Add("CourseDuration", typeof(decimal));

            return courses;
        }

        /// <summary>
        /// Load courses from database
        /// </summary>        
        private DataTable LoadCoursesUsedByLogbooks()
        {
            DataTable courses = (DataTable)Session[sessionKeyCoursesResults];

            if (Session[sessionKeyCoursesResults] == null)
            {
                courses = LoadEmptyCourses();

                List<CourseEntity> registeredCourses = ObjCoursesBll.ListByDivisionUsedByLogbooks(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, 
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredCourses.ForEach(x => courses.Rows.Add(
                    x.CourseCode, x.CourseName, x.CourseDuration));

                DataRow defaultRow = courses.NewRow();
                defaultRow.SetField("CourseCode", "-1");
                defaultRow.SetField("CourseName", string.Empty);
                defaultRow.SetField<decimal>("CourseDuration", 0);
                courses.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyCoursesResults] = courses;
            }

            return courses;
        }

        /// <summary>
        /// Load courses history from database
        /// </summary>        
        private DataTable LoadCoursesUsedByLogbooksHistory()
        {
            DataTable courses = (DataTable)Session[sessionKeyCoursesResults];

            if (Session[sessionKeyCoursesResults] == null)
            {
                courses = LoadEmptyCourses();

                List<CourseEntity> registeredCourses = ObjCoursesBll.ListByDivisionUsedByLogbooksHistory(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredCourses.ForEach(x => courses.Rows.Add(
                    x.CourseCode, x.CourseName, x.CourseDuration));

                DataRow defaultRow = courses.NewRow();
                defaultRow.SetField("CourseCode", "-1");
                defaultRow.SetField("CourseName", string.Empty);
                defaultRow.SetField<decimal>("CourseDuration", 0);
                courses.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyCoursesResults] = courses;
            }

            return courses;
        }

        /// <summary>
        /// Load empty data structure for Trainers
        /// </summary>
        /// <returns>Empty data structure for Trainers</returns>
        private DataTable LoadEmptyTrainers()
        {
            DataTable trainers = new DataTable();
            trainers.Columns.Add("TrainerPK", typeof(string));
            trainers.Columns.Add("TrainerDisplayName", typeof(string));
            trainers.Columns.Add("TrainerCode", typeof(string));
            trainers.Columns.Add("TrainerType", typeof(string));
            trainers.Columns.Add("TrainerName", typeof(string));

            return trainers;
        }

        /// <summary>
        /// Load Trainers from database
        /// </summary>        
        private DataTable LoadTrainersUsedByLogbooks()
        {
            DataTable trainers = (DataTable)Session[sessionKeyTrainersResults];

            if (Session[sessionKeyTrainersResults] == null)
            {
                trainers = LoadEmptyTrainers();

                List<TrainerEntity> registeredTrainers = ObjTrainersBll.ListByDivisionUsedByLogbooks(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, 
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredTrainers.ForEach(x => trainers.Rows.Add(string.Format("{0},{1}", x.TrainerType, x.TrainerCode), string.Format("{0}-{1}", x.TrainerCode, x.TrainerName), x.TrainerCode, x.TrainerType, x.TrainerName));

                DataRow defaultRow = trainers.NewRow();
                defaultRow.SetField("TrainerPK", "-1");
                defaultRow.SetField("TrainerDisplayName", string.Empty);
                defaultRow.SetField("TrainerCode", string.Empty);
                defaultRow.SetField("TrainerType", string.Empty);
                defaultRow.SetField("TrainerName", string.Empty);
                trainers.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyTrainersResults] = trainers;
            }

            return trainers;
        }

        /// <summary>
        /// Load Trainers history from database
        /// </summary>        
        private DataTable LoadTrainersUsedByLogbooksHistory()
        {
            DataTable trainers = (DataTable)Session[sessionKeyTrainersResults];

            if (Session[sessionKeyTrainersResults] == null)
            {
                trainers = LoadEmptyTrainers();

                List<TrainerEntity> registeredTrainers = ObjTrainersBll.ListByDivisionUsedByLogbooksHistory(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredTrainers.ForEach(x => trainers.Rows.Add(string.Format("{0},{1}", x.TrainerType, x.TrainerCode), string.Format("{0}-{1}", x.TrainerCode, x.TrainerName), x.TrainerCode, x.TrainerType, x.TrainerName));

                DataRow defaultRow = trainers.NewRow();
                defaultRow.SetField("TrainerPK", "-1");
                defaultRow.SetField("TrainerDisplayName", string.Empty);
                defaultRow.SetField("TrainerCode", string.Empty);
                defaultRow.SetField("TrainerType", string.Empty);
                defaultRow.SetField("TrainerName", string.Empty);
                trainers.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyTrainersResults] = trainers;
            }

            return trainers;
        }

        /// <summary>
        /// Load empty data structure for training centers
        /// </summary>
        /// <returns>Empty data structure for Training centers</returns>
        private DataTable LoadEmptyTrainingCenters()
        {
            DataTable trainingCenters = new DataTable();
            trainingCenters.Columns.Add("TrainingCenterCode", typeof(string));
            trainingCenters.Columns.Add("TrainingCenterDescription", typeof(string));

            return trainingCenters;
        }

        /// <summary>
        /// Load training centers from database
        /// </summary>        
        private DataTable LoadTrainingCentersUsedByLogbooks()
        {
            DataTable trainingCenters = (DataTable)Session[sessionKeyTrainingCentersResults];

            if (Session[sessionKeyTrainingCentersResults] == null)
            {
                trainingCenters = LoadEmptyTrainingCenters();

                List<TrainingCenterEntity> registeredTrainingCenters = ObjTrainingCentersBll.ListByDivisionUsedByLogbooks(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, 
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredTrainingCenters.ForEach(x => trainingCenters.Rows.Add(
                    x.TrainingCenterCode, x.TrainingCenterDescription));

                DataRow defaultRow = trainingCenters.NewRow();
                defaultRow.SetField("TrainingCenterCode", "-1");
                defaultRow.SetField("TrainingCenterDescription", string.Empty);
                trainingCenters.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyTrainingCentersResults] = trainingCenters;
            }

            return trainingCenters;
        }

        /// <summary>
        /// Load training centers history from database
        /// </summary>        
        private DataTable LoadTrainingCentersUsedByLogbooksHistory()
        {
            DataTable trainingCenters = (DataTable)Session[sessionKeyTrainingCentersResults];

            if (Session[sessionKeyTrainingCentersResults] == null)
            {
                trainingCenters = LoadEmptyTrainingCenters();

                List<TrainingCenterEntity> registeredTrainingCenters = ObjTrainingCentersBll.ListByDivisionUsedByLogbooksHistory(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredTrainingCenters.ForEach(x => trainingCenters.Rows.Add(x.TrainingCenterCode, x.TrainingCenterDescription));

                DataRow defaultRow = trainingCenters.NewRow();
                defaultRow.SetField("TrainingCenterCode", "-1");
                defaultRow.SetField("TrainingCenterDescription", string.Empty);
                trainingCenters.Rows.InsertAt(defaultRow, 0);

                Session[sessionKeyTrainingCentersResults] = trainingCenters;
            }

            return trainingCenters;
        }

        /// <summary>
        /// Load logbooks from database
        /// </summary>
        private void LoadFilterByLogBooks()
        {
            DataTable trainers = LoadTrainersUsedByLogbooks();
            cboTrainerFilter.Enabled = true;
            cboTrainerFilter.DataValueField = "TrainerPK";
            cboTrainerFilter.DataTextField = "TrainerDisplayName";
            cboTrainerFilter.DataSource = trainers;
            cboTrainerFilter.DataBind();

            DataTable courses = LoadCoursesUsedByLogbooks();
            cboCourseFilter.Enabled = true;
            cboCourseFilter.DataValueField = "CourseCode";
            cboCourseFilter.DataTextField = "CourseName";
            cboCourseFilter.DataSource = courses;
            cboCourseFilter.DataBind();

            DataTable trainingCenters = LoadTrainingCentersUsedByLogbooks();
            cboTrainingCenterFilter.Enabled = true;
            cboTrainingCenterFilter.DataValueField = "TrainingCenterCode";
            cboTrainingCenterFilter.DataTextField = "TrainingCenterDescription";
            cboTrainingCenterFilter.DataSource = trainingCenters;
            cboTrainingCenterFilter.DataBind();
        }

        /// <summary>
        /// Load logbooks history from database
        /// </summary>
        private void LoadFilterByLogBooksHistory()
        {
            DataTable trainers = LoadTrainersUsedByLogbooksHistory();
            cboTrainerFilter.Enabled = true;
            cboTrainerFilter.DataValueField = "TrainerPK";
            cboTrainerFilter.DataTextField = "TrainerDisplayName";
            cboTrainerFilter.DataSource = trainers;
            cboTrainerFilter.DataBind();

            DataTable courses = LoadCoursesUsedByLogbooksHistory();
            cboCourseFilter.Enabled = true;
            cboCourseFilter.DataValueField = "CourseCode";
            cboCourseFilter.DataTextField = "CourseName";
            cboCourseFilter.DataSource = courses;
            cboCourseFilter.DataBind();

            DataTable trainingCenters = LoadTrainingCentersUsedByLogbooksHistory();
            cboTrainingCenterFilter.Enabled = true;
            cboTrainingCenterFilter.DataValueField = "TrainingCenterCode";
            cboTrainingCenterFilter.DataTextField = "TrainingCenterDescription";
            cboTrainingCenterFilter.DataSource = trainingCenters;
            cboTrainingCenterFilter.DataBind();
        }

        /// <summary>
        /// Load logbooks files from database
        /// </summary>
        /// <returns></returns>
        private List<LogbooksFileEntity> LoadLogbookFilesList(string selectedLogbookNumber)
        {
            List<LogbooksFileEntity> logbookFiles = ObjLogbooksFilesBll.LogbookFilesListByKey(new LogbooksFileEntity
            {
                GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                LogbookNumber = int.Parse(selectedLogbookNumber),
                DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
            });

            return logbookFiles;
        }

        /// <summary>
        /// Apply rules based the type Logbook
        /// </summary>
        private void ApplyRulesBasedTheTypeLogbook()
        {
            if (Session[sessionKeyTypeLogbookResults] != null)
            {
                if (Session[sessionKeyTypeLogbookResults].Equals("N"))
                {
                    grvList.Columns[7].Visible = true;
                    LoadFilterByLogBooks();
                }

                else if (Session[sessionKeyTypeLogbookResults].Equals("H"))
                {
                    btnAdd.Disabled = true;
                    btnAdd.Attributes.Add("disabled", "disabled");
                    btnAdd.Visible = false;

                    btnDelete.Disabled = true;
                    btnDelete.Attributes.Add("disabled", "disabled");
                    btnDelete.Visible = false;

                    grvList.Columns[7].Visible = false;
                    LoadFilterByLogBooksHistory();
                }
            }
        }

        /// <summary>
        /// Get the localizated string of trainer type 
        /// </summary>
        /// <param name="enumerationValuee">Enumeration value</param>
        /// <returns>Localizated name</returns>
        public string GetLogbookStatusLocalizatedDescription(string enumerationValue)
        {
            return GetLogbookStatusLocalizatedDescription(HrisEnum.ParseEnumByName<LogbookStatus>(enumerationValue));
        }

        /// <summary>
        /// Get the localizated string of trainer type 
        /// </summary>
        /// <param name="enumerationValuee">Enumeration value</param>
        /// <returns>Localizated name</returns>
        public string GetLogbookStatusLocalizatedDescription(LogbookStatus enumerationValue)
        {
            return GetLocatizatedDescription(enumerationValue);
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<LogbookEntity> SearchResults(int page)
        {
            LogbookEntity logbookEntity = new LogbookEntity()
            {
                DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                GeographicDivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                LogbookNumber = string.IsNullOrWhiteSpace(hdfLogbookNumberFilter.Value) ? (int?)null : int.Parse(hdfLogbookNumberFilter.Value),
                StartDateTime = string.IsNullOrWhiteSpace(hdfStartDateFilter.Value) ? (DateTime?)null : DateTime.ParseExact(hdfStartDateFilter.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None),
                EndDate = string.IsNullOrWhiteSpace(hdfEndDateFilter.Value) ? (DateTime?)null : DateTime.ParseExact(hdfEndDateFilter.Value, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None),
                CourseCode = string.IsNullOrWhiteSpace(hdfCourseValueFilter.Value) ? null : hdfCourseValueFilter.Value,
                IsClosed = string.IsNullOrWhiteSpace(hdfStatusValueFilter.Value) ? (bool?)null : Convert.ToBoolean(hdfStatusValueFilter.Value),
                ExistFiles = string.IsNullOrWhiteSpace(hdfExistFilesValueFilter.Value) ? (bool?)null : Convert.ToBoolean(hdfExistFilesValueFilter.Value),
            };

            if (!string.IsNullOrWhiteSpace(hdfTrainerValueFilter.Value))
            {
                string[] trainerTokens = hdfTrainerValueFilter.Value.Split(new char[] { ',' }, 2, StringSplitOptions.RemoveEmptyEntries);
                logbookEntity.TrainerType = HrisEnum.ParseEnumByName<TrainerType>(trainerTokens[0]);
                logbookEntity.TrainerCode = trainerTokens[1];
            }

            string trainingCenterCode = string.IsNullOrWhiteSpace(hdfTrainingCenterValueFilter.Value) ? null : hdfTrainingCenterValueFilter.Value;

            PageHelper<LogbookEntity> pageHelper = null;
            if (Session[sessionKeyTypeLogbookResults] != null)
            {
                var logbookType = Session[sessionKeyTypeLogbookResults] as string;
                if (logbookType.Equals("N"))
                {
                    pageHelper = ObjLogbooksBll.ListByFilters(
                        logbookEntity, trainingCenterCode,
                        UserHelper.GetCurrentFullUserName,
                        CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID),
                        CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID),
                        page);
                }

                else if (logbookType.Equals("H"))
                {
                    pageHelper = ObjLogbooksBll.ListHistoryByFilters(
                        logbookEntity, trainingCenterCode,
                        UserHelper.GetCurrentFullUserName,
                        CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID),
                        CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID),
                        page);
                }
            }

            if (pageHelper != null)
            {
                foreach (LogbookEntity entity in pageHelper.ResultList)
                {
                    entity.CourseCodeName = string.Format("{0} - {1}", entity.CourseCode, entity.CourseName);
                    entity.Status = entity.IsClosed.Value ? GetLogbookStatusLocalizatedDescription(LogbookStatus.Closed) : GetLogbookStatusLocalizatedDescription(LogbookStatus.Draft);
                    entity.StartDateTimeFormated = Convert.ToDateTime(entity.StartDateTime).ToString("MM/dd/yyyy");
                }
            }

            Session[sessionKeyLogbooksResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyLogbooksResults] != null)
            {
                PageHelper<LogbookEntity> pageHelper = (PageHelper<LogbookEntity>)Session[sessionKeyLogbooksResults];

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