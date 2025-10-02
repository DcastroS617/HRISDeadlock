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
    public partial class Classrooms : Page
    {
        [Dependency]
        public IClassroomsBll<ClassroomEntity> ObjClassroomsBll { get; set; }

        [Dependency]
        public ITrainingCentersBll<TrainingCenterEntity> ObjTrainingCentersBll { get; set; }


        //session key for the results
        readonly string sessionKeyClassroomsResults = "Classrooms-ClassroomsResults";

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
        /// Handles the init of the page
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                ucClassroom.ObjTrainingCentersBll = ObjTrainingCentersBll;
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
                    LoadTrainingCentersFilter();

                    ucClassroom.LoadTrainingCenters();

                    //fire the event
                    BtnSearch_ServerClick(sender, e);
                }

                //activate the pager
                if (Session[sessionKeyClassroomsResults] != null)
                {
                    PageHelper<ClassroomEntity> pageHelper = (PageHelper<ClassroomEntity>)Session[sessionKeyClassroomsResults];
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
                ClassroomEntity classroom = ucClassroom.GetClassroom();

                if (ucClassroom.IsValidClassroom(sender))
                {
                    return;
                }

                PageHelper<ClassroomEntity> pageHelper = (PageHelper<ClassroomEntity>)Session[sessionKeyClassroomsResults];

                if (string.IsNullOrWhiteSpace(ucClassroom.hdfClassroomCodeEdit.Value))
                {
                    Tuple<bool, ClassroomEntity> addResult = ObjClassroomsBll.Add(classroom);
                    if (addResult.Item1)
                    {
                        if (pageHelper != null)
                        {
                            SearchResults(pageHelper.CurrentPage);
                            DisplayResults();
                        }

                        LoadTrainingCentersFilter();

                        ucClassroom.LoadTrainingCenters();

                        hdfSelectedRowIndex.Value = "-1";

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else if (!addResult.Item1)
                    {
                        ClassroomEntity previousClassroom = addResult.Item2;
                        bool sameDivisionClassrooms = false;

                        if (SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode.Equals(previousClassroom.DivisionCode))
                        {
                            sameDivisionClassrooms = true;
                        }

                        if (previousClassroom.Deleted && sameDivisionClassrooms)
                        {
                            txtActivateDeletedClassroomCode.Text = previousClassroom.ClassroomCode;
                            txtActivateDeletedClassroomDescription.Text = previousClassroom.ClassroomDescription;

                            hdfActivateDeletedClassroomCode.Value = previousClassroom.ClassroomCode;
                            hdfActivateDeletedClassroomGeographicDivisionCode.Value = previousClassroom.GeographicDivisionCode;
                            
                            divActivateDeletedDialog.InnerHtml = classroom.ClassroomCode == previousClassroom.ClassroomCode ?
                                                            GetLocalResourceObject("lblTextDuplicatedDialog").ToString() : GetLocalResourceObject("lblTextDuplicatedDialogName").ToString();
                          
                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                        }

                        else
                        {
                            txtDuplicatedClassroomCode.Text = previousClassroom.ClassroomCode;
                            txtDuplicatedClassroomDescription.Text = previousClassroom.ClassroomDescription;
                            pnlDuplicatedDialogDataDetail.Visible = sameDivisionClassrooms;

                            divDuplicatedDialogText.InnerHtml = classroom.ClassroomCode == previousClassroom.ClassroomCode ?
                                GetLocalResourceObject("lblTextDuplicatedDialog").ToString() : GetLocalResourceObject("lblTextDuplicatedDialogName").ToString();

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    var result = ObjClassroomsBll.Edit(classroom);
                    if (result.Item1)
                    {
                        if (pageHelper != null)
                        {
                            pageHelper.ResultList.Remove(
                                pageHelper.ResultList.Find(x => x.ClassroomCode == classroom.ClassroomCode)
                            );

                            pageHelper.ResultList.Insert(Convert.ToInt32(hdfSelectedRowIndex.Value), classroom);
                            DisplayResults();
                        }

                        LoadTrainingCentersFilter();

                        ucClassroom.LoadTrainingCenters();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else
                    {
                        var divisionsome = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;
                        ClassroomEntity previousClassroom = result.Item2;

                        if (previousClassroom.Deleted && divisionsome == previousClassroom.DivisionCode)
                        {
                            txtActivateDeletedClassroomCode.Text = previousClassroom.ClassroomCode;
                            txtActivateDeletedClassroomDescription.Text = previousClassroom.ClassroomDescription;

                            hdfActivateDeletedClassroomCode.Value = previousClassroom.ClassroomCode;
                            hdfActivateDeletedClassroomGeographicDivisionCode.Value = previousClassroom.GeographicDivisionCode;

                            divActivateDeletedDialog.InnerHtml = ucClassroom.ClassroomCode() == previousClassroom.ClassroomCode ?
                                                           Convert.ToString(GetLocalResourceObject("lblTextActivateDeletedDialog")) : GetLocalResourceObject("lblTextActivateDeletedDialogDescripcion").ToString();
                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted(); ", true);

                        }
                    }
                }

                LoadTrainingCentersFilter();
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
                ucClassroom.hdfClassroomCodeEdit.Value =
                       !string.IsNullOrWhiteSpace(hdfActivateDeletedClassroomCode.Value) ?
                           hdfActivateDeletedClassroomCode.Value :
                           null;

                ucClassroom.hdfClassroomGeographicDivisionCodeEdit.Value =
                    !string.IsNullOrWhiteSpace(hdfActivateDeletedClassroomGeographicDivisionCode.Value) ?
                        hdfActivateDeletedClassroomGeographicDivisionCode.Value :
                        null;

                ClassroomEntity classroom = ucClassroom.GetClassroom();

                PageHelper<ClassroomEntity> pageHelper = (PageHelper<ClassroomEntity>)Session[sessionKeyClassroomsResults];

                //activate the deleted item
                if (chbActivateDeleted.Checked)
                {
                    ObjClassroomsBll.Activate(classroom);

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
                    classroom.Deleted = false;

                    ObjClassroomsBll.Edit(classroom);

                    if (pageHelper != null)
                    {
                        string position = hdfSelectedRowIndex.Value;

                        SearchResults(pageHelper.CurrentPage);
                        DisplayResults();

                        hdfSelectedRowIndex.Value = position;
                    }

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                }

                LoadTrainingCentersFilter();
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
                    string selectedClassroomCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["ClassroomCode"]);
                    string selectedClassroomGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                    int divisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                    ClassroomEntity classroom = ObjClassroomsBll.ListByKey(
                        selectedClassroomGeographicDivisionCode,
                        selectedClassroomCode, divisionCode);

                    ucClassroom.cboTrainingCenter.SelectedValue = classroom.TrainingCenter.TrainingCenterCode;
                    ucClassroom.hdfClassroomCodeEdit.Value = classroom.ClassroomCode;
                    ucClassroom.hdfClassroomGeographicDivisionCodeEdit.Value = classroom.GeographicDivisionCode;
                    ucClassroom.txtClassroomCode.Text = classroom.ClassroomCode;
                    ucClassroom.txtClassroomCode.Enabled = false;
                    ucClassroom.txtClassroomDescription.Text = classroom.ClassroomDescription;

                    ucClassroom.txtCapacity.Text = Convert.ToString(classroom.Capacity);
                    ucClassroom.txtComments.Text = classroom.Comments;
                    ucClassroom.chbSearchEnabled.Checked = classroom.SearchEnabled;

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
                btnAdd.Disabled = false;
                btnEdit.Disabled = false;
                btnDelete.Disabled = false;

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
                    string selectedClassroomCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["ClassroomCode"]);
                    string selectedClassroomGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                    int DivisionCode = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

                    ObjClassroomsBll.Delete(
                        new ClassroomEntity(
                            selectedClassroomGeographicDivisionCode,
                            selectedClassroomCode,
                            UserHelper.GetCurrentFullUserName)
                        { DivisionCode = DivisionCode });

                    PageHelper<ClassroomEntity> pageHelper = (PageHelper<ClassroomEntity>)Session[sessionKeyClassroomsResults];

                    pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.ClassroomCode == selectedClassroomCode));
                    pageHelper.TotalResults--;

                    if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                    {
                        SearchResults(pageHelper.TotalPages - 1);
                    }

                    pageHelper.UpdateTotalPages();
                    RefreshTable();

                    hdfSelectedRowIndex.Value = "-1";
                    LoadTrainingCentersFilter();

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error , GetLocalResourceObject("msgInvalidSelection").ToString());
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
                    MensajeriaHelper.MostrarMensaje(Page , TipoMensaje.Error , newEx.Message);
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
                hdfMinCapacityFilter.Value = txtMinCapacityFilter.Text.Trim();
                hdfMaxCapacityFilter.Value = txtMaxCapacityFilter.Text.Trim();
                hdfClassroomCodeFilter.Value = txtClassroomCodeFilter.Text.Trim();
                hdfClassroomDescriptionFilter.Value = txtClassroomDescriptionFilter.Text.Trim();
                hdfTrainingCenterValueFilter.Value = GetTrainingCenterFilterSelectedValue();
                hdfTrainingCenterTextFilter.Value = GetTrainingCenterFilterSelectedText();

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
            if (Session[sessionKeyClassroomsResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<ClassroomEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Private methods

        public bool SetSelectedText(DropDownList dropDownList, string selectedText)
        {
            dropDownList.ClearSelection();
            ListItem selectedListItem = dropDownList.Items.FindByValue(selectedText);
            if (selectedListItem != null)
            {
                selectedListItem.Selected = true;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Returns the selected training center id
        /// </summary>
        /// <returns>The selected training center id</returns>
        private string GetTrainingCenterFilterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboTrainingCenterFilter.SelectedValue))
            {
                selected = cboTrainingCenterFilter.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Returns the selected training center text
        /// </summary>
        /// <returns>The selected training center text</returns>
        private string GetTrainingCenterFilterSelectedText()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboTrainingCenterFilter.SelectedValue))
            {
                selected = ucClassroom.cboTrainingCenter.SelectedItem.Text;
            }

            return selected;
        }

        private void LoadTrainingCentersFilter()
        {
            List<TrainingCenterEntity> trainingCenters = ObjTrainingCentersBll.ListByDivisionFilterCB(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

            IDictionary<string, string> placeLocations = GetAllValuesAndLocalizatedDescriptions<PlaceLocation>();

            trainingCenters = trainingCenters.Select(R =>
            {
                R.TrainingCenterDescription = $"{R.TrainingCenterDescription} - {placeLocations[R.PlaceLocation.ToString()]}";
                return R;
            }).ToList();

            trainingCenters.Insert(0, new TrainingCenterEntity("", "", ""));

            cboTrainingCenterFilter.Enabled = true;
            cboTrainingCenterFilter.DataValueField = "TrainingCenterCode";
            cboTrainingCenterFilter.DataTextField = "TrainingCenterDescription";
            cboTrainingCenterFilter.DataSource = trainingCenters;
            cboTrainingCenterFilter.DataBind();
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<ClassroomEntity> SearchResults(int page)
        {
            int? minCapacity = int.TryParse(hdfMinCapacityFilter.Value, out int tempMinVal) ? tempMinVal : (int?)null;
            int? maxCapacity = int.TryParse(hdfMaxCapacityFilter.Value, out int tempMaxVal) ? tempMaxVal : (int?)null;

            string classroomCode = string.IsNullOrWhiteSpace(hdfClassroomCodeFilter.Value) ? null : hdfClassroomCodeFilter.Value;
            string classroomDescription = string.IsNullOrWhiteSpace(hdfClassroomDescriptionFilter.Value) ? null : hdfClassroomDescriptionFilter.Value;
            string trainingCenterCode = hdfTrainingCenterValueFilter.Value;

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<ClassroomEntity> pageHelper = ObjClassroomsBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                classroomCode, classroomDescription, trainingCenterCode, minCapacity, maxCapacity,
                sortExpression, sortDirection, page);

            Session[sessionKeyClassroomsResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyClassroomsResults] != null)
            {
                PageHelper<ClassroomEntity> pageHelper = (PageHelper<ClassroomEntity>)Session[sessionKeyClassroomsResults];

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