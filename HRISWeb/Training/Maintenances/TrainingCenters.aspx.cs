using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using Unity.Web;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.Training.Maintenances
{
    public partial class TrainingCenters : Page
    {
        [Dependency]
        public ITrainingCentersBll<TrainingCenterEntity> ObjTrainingCentersBll { get; set; }

        [Dependency]
        public IClassroomsBll<ClassroomEntity> ObjClassroomsBll { get; set; }

        //session key for the results
        readonly string sessionKeyTrainingCentersResults = "TrainerCenters-TrainerCentersResults";
        readonly string sessionKeyAssociatedClassrooms = "TrainerCenters-TrainerCentersClassroomsAssociated";

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
                if (ObjTrainingCentersBll == null)
                {
                    ObjTrainingCentersBll = Application.GetContainer().Resolve<ITrainingCentersBll<TrainingCenterEntity>>();
                }

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
                    LoadPlaceLocations();

                    //fire the event
                    BtnSearch_ServerClick(sender, e);
                }

                chkSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chkSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));

                //activate the pager
                if (Session[sessionKeyTrainingCentersResults] != null)
                {
                    PageHelper<TrainingCenterEntity> pageHelper = (PageHelper<TrainingCenterEntity>)Session[sessionKeyTrainingCentersResults];
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
                var trainingCenterId = hdfTrainingCenterCodeEdit.Value == "-1" || string.IsNullOrEmpty(hdfTrainingCenterCodeEdit.Value) ? (int?)null : int.Parse(hdfTrainingCenterCodeEdit.Value);

                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                var entity = new TrainingCenterEntity
                {
                    TrainingCenterId = trainingCenterId,
                    GeographicDivisionCode = workingDivision.GeographicDivisionCode,
                    DivisionCode = workingDivision.DivisionCode,
                    TrainingCenterCode = txtTrainingCenterCode.Text.Trim(),
                    TrainingCenterDescription = txtTrainingCenterDescription.Text.Trim(),
                    PlaceLocation = HrisEnum.ParseEnumByDescription<PlaceLocation>(Convert.ToString(cboPlaceLocation.SelectedValue)),
                    SearchEnabled = chkSearchEnabled.Checked,
                    Deleted = false,
                    LastModifiedUser = UserHelper.GetCurrentFullUserName
                };

                TrainingCenterEntity result = null;
                if (trainingCenterId.HasValue)
                {
                    //Editar
                    result = ObjTrainingCentersBll.Edit(entity);
                }
                else
                {
                    //Insertar
                    result = ObjTrainingCentersBll.Add(entity);
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
                        hdfActivateDeletedTrainingCentersId.Value = result.TrainingCenterId?.ToString();
                        txtActivateDeletedTrainingCenterCode.Text = result.TrainingCenterCode;
                        txtActivateDeletedTrainingCenterDescription.Text = result.TrainingCenterDescription;

                        divActivateDeletedDialog.InnerHtml = result.ErrorNumber == -1 ?
                            Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog")) :
                            Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialogName"));

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDeleted();", true);
                    }

                    else
                    {
                        txtDuplicatedCode.Text = result.TrainingCenterCode;
                        txtDuplicatedDescription.Text = result.TrainingCenterDescription;

                        hdfDuplicateType.Value = "-1";
                        pnlDuplicatedDialogDataDetail.Visible = true;

                        divDuplicatedDialogText.InnerHtml = result.ErrorNumber == -1 ?
                                Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog")) :
                                Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialogName"));

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDuplicated();", true);
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
                btnAccept.Disabled = false;
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
                var TrainingCentersId = hdfActivateDeletedTrainingCentersId.Value == "-1" || string.IsNullOrEmpty(hdfActivateDeletedTrainingCentersId.Value) ? (int?)null : int.Parse(hdfActivateDeletedTrainingCentersId.Value);

                if (TrainingCentersId.HasValue)
                {
                    DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                    var updateEntity = new TrainingCenterEntity();

                    if (chkActivateDeleted.Checked)
                    {
                        updateEntity = new TrainingCenterEntity
                        {
                            TrainingCenterId = TrainingCentersId,
                            GeographicDivisionCode = workingDivision.GeographicDivisionCode,
                            TrainingCenterCode = txtActivateDeletedTrainingCenterCode.Text.Trim(),
                            TrainingCenterDescription = txtTrainingCenterDescription.Text.Trim(),
                            DivisionCode = workingDivision.DivisionCode,
                            SearchEnabled = true,
                            Deleted = false,
                            LastModifiedUser = UserHelper.GetCurrentFullUserName
                        };
                    }

                    else
                    {
                        updateEntity = new TrainingCenterEntity
                        {
                            TrainingCenterId = TrainingCentersId,
                            GeographicDivisionCode = workingDivision.GeographicDivisionCode,
                            DivisionCode = workingDivision.DivisionCode,
                            TrainingCenterCode = txtActivateDeletedTrainingCenterCode.Text.Trim(),
                            TrainingCenterDescription = txtTrainingCenterDescription.Text.Trim(),
                            PlaceLocation = HrisEnum.ParseEnumByDescription<PlaceLocation>(Convert.ToString(cboPlaceLocation.SelectedValue)),
                            SearchEnabled = chkSearchEnabled.Checked,
                            Deleted = false,
                            LastModifiedUser = UserHelper.GetCurrentFullUserName
                        };
                    }

                    var result = ObjTrainingCentersBll.Edit(updateEntity);
                    if (result.ErrorNumber == 0)
                    {
                        hdfSelectedRowIndex.Value = "-1";
                        RefreshTable();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
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
                            txtDuplicatedCode.Text = result.TrainingCenterCode;
                            txtDuplicatedDescription.Text = result.TrainingCenterDescription;

                            hdfDuplicateType.Value = "-1";
                            pnlDuplicatedDialogDataDetail.Visible = result.ErrorNumber == -1;

                            divDuplicatedDialogText.InnerHtml = result.ErrorNumber == -1 ?
                                    Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog")) :
                                    Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialogName"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDuplicated();", true);
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
                    string selectedTrainingCenterCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingCenterCode"]);

                    TrainingCenterEntity trainingCenter = ObjTrainingCentersBll.ListByCode(workingDivision.GeographicDivisionCode,
                        workingDivision.DivisionCode,
                        selectedTrainingCenterCode);

                    hdfTrainingCenterCodeEdit.Value = trainingCenter.TrainingCenterId.ToString();
                    txtTrainingCenterCode.Text = trainingCenter.TrainingCenterCode;
                    txtTrainingCenterCode.Enabled = false;
                    txtTrainingCenterDescription.Text = trainingCenter.TrainingCenterDescription;
                    cboPlaceLocation.SelectedValue = GetDescription(trainingCenter.PlaceLocation);
                    chkSearchEnabled.Checked = trainingCenter.SearchEnabled;
                    btnAccept.Disabled = false;

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
                btnAdd.Disabled = false;
                btnEdit.Disabled = false;
                btnDelete.Disabled = false;
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

        #region Classrooms

        /// <summary>
        /// Handles the btnRefreshAssociatedClassrooms click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnRefreshAssociatedClassrooms_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshAssociatedClassrooms();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppAssociatedClassrooms
                      , TipoMensaje.Error
                      , ex.Message);
                }
                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppAssociatedClassrooms
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnEditClassrooms click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEditClassrooms_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    LoadAssociatedClassrooms(true);
                    List<ClassroomEntity> associatedClassrooms = (List<ClassroomEntity>)Session[sessionKeyAssociatedClassrooms];
                    PageHelper<TrainingCenterEntity> pageHelper = (PageHelper<TrainingCenterEntity>)Session[sessionKeyTrainingCentersResults];

                    if (!pageHelper.ResultList[Convert.ToInt32(hdfSelectedRowIndex.Value)].SearchEnabled)
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInactivedCenter").ToString());
                    }

                    else
                    {
                        rptAssociatedClassrooms.DataSource = associatedClassrooms;
                        rptAssociatedClassrooms.DataBind();
                        uppAssociatedClassrooms.Update();

                        uppSearchClassrooms.Update();

                        ucClassroom.LoadTrainingCenters();

                        ucClassroom.cboTrainingCenter.Enabled = false;

                        string selectedTrainingCenterCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingCenterCode"]);
                        SetSelectedText(ucClassroom.cboTrainingCenter, selectedTrainingCenterCode);

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClassroomsClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClassroomsClickPostBack(); }}, 200);", true);
                    }
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
        /// Handles the btnAddClassroom click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnAddClassroom_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    ClassroomEntity classroom = ucClassroom.GetClassroom();

                    if (ucClassroom.IsValidClassroom(sender))
                    {
                        return;
                    }

                    LoadAssociatedClassrooms(false);
                    List<ClassroomEntity> associatedClassrooms = (List<ClassroomEntity>)Session[sessionKeyAssociatedClassrooms];

                    Tuple<bool, ClassroomEntity> addResult = ObjClassroomsBll.Add(classroom);
                    if (addResult.Item1)
                    {
                        associatedClassrooms.Add(classroom);

                        rptAssociatedClassrooms.DataSource = associatedClassrooms;
                        rptAssociatedClassrooms.DataBind();

                        uppAssociatedClassrooms.Update();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAddClassroomClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnAddClassroomClickPostBack('{0}'); }}, 200);", btnAdd.ClientID), true);
                    }

                    else if (!addResult.Item1)
                    {
                        ClassroomEntity previousClassroom = addResult.Item2;
                        bool sameDivisionClassrooms = false;

                        if (SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode.Equals(previousClassroom.DivisionCode))
                        {
                            sameDivisionClassrooms = true;
                        }

                        if (sameDivisionClassrooms)
                        {
                            txtDuplicatedCode.Text = previousClassroom.ClassroomCode;
                            txtDuplicatedDescription.Text = previousClassroom.ClassroomDescription;
                            hdfDuplicateType.Value = "1";
                            pnlDuplicatedDialogDataDetail.Visible = sameDivisionClassrooms;

                            var duplicate = classroom.ClassroomCode == previousClassroom.ClassroomCode
                                ? "lblTextDuplicatedDialog" : "lblTextDuplicatedDialogName";

                            divDuplicatedDialogText.InnerHtml = GetLocalResourceObject(duplicate).ToString();

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDuplicated();", true);
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
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("UQ_ClassroomsByThematicAreas_ClassroomCode"))
                    {
                        MensajeriaHelper.MostrarMensaje((Control)sender
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msjThematicAreaRelatedClassroomException").ToString());
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
                    LoadAssociatedClassrooms(true);
                    List<ClassroomEntity> associatedClassrooms = (List<ClassroomEntity>)Session[sessionKeyAssociatedClassrooms];

                    if (associatedClassrooms.Count > 0)
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Advertencia,
                            GetLocalResourceObject("msgAssociatedClassrooms").ToString());

                        return;
                    }

                    string selectedTrainingCenterCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingCenterCode"]);

                    List<ClassroomEntity> listClassroomsAsociated = ObjClassroomsBll.ListByTrainingCenter(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, selectedTrainingCenterCode, SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode);
                    if (listClassroomsAsociated.Count == 0)
                    {
                        ObjTrainingCentersBll.Delete(
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                            selectedTrainingCenterCode,
                            UserHelper.GetCurrentFullUserName);

                        PageHelper<TrainingCenterEntity> pageHelper = (PageHelper<TrainingCenterEntity>)Session[sessionKeyTrainingCentersResults];

                        pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.TrainingCenterCode == selectedTrainingCenterCode));
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
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msjTrainingCenterAsociatedClassroomsException").ToString());
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
            SearchResults(1);

            CommonFunctions.ResetSortDirection(Page.ClientID, grvList.ClientID);

            DisplayResults();
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
            if ((grvList.ShowHeader && grvList.Rows.Count > 0)
                || (grvList.ShowHeaderWhenEmpty))
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
            if (Session[sessionKeyTrainingCentersResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<TrainingCenterEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Set selected text for dropDownList
        /// </summary>
        /// <param name="dropDownList"></param>
        /// <param name="selectedText"></param>
        /// <returns></returns>
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
        /// Refresh the associated classrooms tables by rebinding data
        /// </summary>
        private void RefreshAssociatedClassrooms()
        {
            if (Session[sessionKeyAssociatedClassrooms] != null)
            {
                rptAssociatedClassrooms.DataSource = Session[sessionKeyAssociatedClassrooms];
                rptAssociatedClassrooms.DataBind();
            }
        }

        /// <summary>
        /// Load associated training programs
        /// </summary>        
        private void LoadAssociatedClassrooms(bool forceRead)
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);
                string selectedTrainingCenterCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingCenterCode"]);

                if (Session[sessionKeyAssociatedClassrooms] == null || forceRead)
                {
                    Session[sessionKeyAssociatedClassrooms] = ObjClassroomsBll.ListByTrainingCenter(workingDivision.GeographicDivisionCode, selectedTrainingCenterCode, workingDivision.DivisionCode);
                }
            }
            else
            {
                Session[sessionKeyAssociatedClassrooms] = new List<TrainingProgramEntity>();
            }
        }

        /// <summary>
        /// Load the place locations controls
        /// </summary>
        private void LoadPlaceLocations()
        {
            Dictionary<string, string> placeLocationOptions = new Dictionary<string, string>
            {
                { "-1", "" }
            };

            IDictionary<string, string> placeLocations = GetAllValuesAndLocalizatedDescriptions<PlaceLocation>();
            foreach (KeyValuePair<string, string> type in placeLocations)
            {
                placeLocationOptions.Add(type.Key, type.Value);
            }

            cboPlaceLocationFilter.DataValueField = "Key";
            cboPlaceLocationFilter.DataTextField = "Value";
            cboPlaceLocationFilter.DataSource = placeLocationOptions;
            cboPlaceLocationFilter.DataBind();
            cboPlaceLocationFilter.SelectedIndex = 0;

            cboPlaceLocation.DataValueField = "Key";
            cboPlaceLocation.DataTextField = "Value";
            cboPlaceLocation.DataSource = placeLocationOptions;
            cboPlaceLocation.DataBind();
            cboPlaceLocation.SelectedIndex = 0;
        }

        /// <summary>
        /// Get the localizated string of place location 
        /// </summary>
        /// <param name="enumerationValuee">Enumeration value</param>
        /// <returns>Localizated name</returns>
        public string GetPlaceLocationLocalizatedDescription(PlaceLocation enumerationValue)
        {
            return GetLocatizatedDescription(enumerationValue);
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<TrainingCenterEntity> SearchResults(int page)
        {
            string TrainingCenterCode = string.IsNullOrWhiteSpace(txtTrainingCenterCodeFilter.Text) ? null : txtTrainingCenterCodeFilter.Text.Trim();
            string TrainingCenterDescription = string.IsNullOrWhiteSpace(txtTrainingCenterDescriptionFilter.Text) ? null : txtTrainingCenterDescriptionFilter.Text.Trim();
            string placeLocation = cboPlaceLocationFilter.SelectedValue != "-1" ? cboPlaceLocationFilter.SelectedValue.Trim() : null;

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<TrainingCenterEntity> pageHelper = ObjTrainingCentersBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                TrainingCenterCode, TrainingCenterDescription, placeLocation,
                sortExpression, sortDirection, page);

            Session[sessionKeyTrainingCentersResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyTrainingCentersResults] != null)
            {
                PageHelper<TrainingCenterEntity> pageHelper = (PageHelper<TrainingCenterEntity>)Session[sessionKeyTrainingCentersResults];

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