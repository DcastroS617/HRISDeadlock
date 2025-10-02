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
    public partial class TrainingPlanPrograms : Page
    {
        [Dependency]
        public ITrainingPlanProgramsBll<TrainingPlanProgramEntity> ObjTrainingPlanProgramsBll { get; set; }

        [Dependency]
        public IMasterProgramBll ObjIMasterProgramBll { get; set; }

        //session key for the results
        readonly string sessionKeyTrainingPlanProgramsResults = "TrainingPlanPrograms-TrainingPlanProgramsResults";
        readonly string sessionKeyMasterProgramsResults = "TrainingPlanPrograms-MasterProgramsResults";
        readonly string sessionKeyAssociatedMasterPrograms = "TrainingPlanPrograms-MasterProgramsAssociated";

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
                if (ObjTrainingPlanProgramsBll == null)
                {
                    ObjTrainingPlanProgramsBll = Application.GetContainer().Resolve<ITrainingPlanProgramsBll<TrainingPlanProgramEntity>>();
                }

                if (!IsPostBack)
                {
                    //fire the event
                    BtnSearch_ServerClick(sender, e);
                }

                chkSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chkSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));

                //activate the pager
                if (Session[sessionKeyTrainingPlanProgramsResults] != null)
                {
                    PageHelper<TrainingPlanProgramEntity> pageHelper = (PageHelper<TrainingPlanProgramEntity>)Session[sessionKeyTrainingPlanProgramsResults];
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
                string hdfTrainingPlanProgramGeographicDivisionCode = !string.IsNullOrWhiteSpace(hdfTrainingPlanProgramGeographicDivisionCodeEdit.Value) ?
                    Convert.ToString(hdfTrainingPlanProgramGeographicDivisionCodeEdit.Value) : "-1";

                string hdfTrainingPlanProgramCode = !string.IsNullOrWhiteSpace(hdfTrainingPlanProgramCodeEdit.Value) ?
                    Convert.ToString(hdfTrainingPlanProgramCodeEdit.Value) : "-1";

                string TrainingPlanProgramCode = txtTrainingPlanProgramCode.Text.Trim();
                string TrainingPlanProgramName = txtTrainingPlanProgramName.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;

                string lastModifiedUser = UserHelper.GetCurrentFullUserName;
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                if (hdfTrainingPlanProgramCode.Equals("-1"))
                {
                    Tuple<bool, TrainingPlanProgramEntity> addResult = ObjTrainingPlanProgramsBll.Add(
                        workingDivision.GeographicDivisionCode, 
                        TrainingPlanProgramCode, 
                        workingDivision.DivisionCode, 
                        TrainingPlanProgramName,
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
                        TrainingPlanProgramEntity previousTrainingPlanProgram = addResult.Item2;

                        if (previousTrainingPlanProgram.Deleted)
                        {
                            txtActivateDeletedTrainingPlanProgramCode.Text = previousTrainingPlanProgram.TrainingPlanProgramCode;
                            txtActivateDeletedTrainingPlanProgramName.Text = previousTrainingPlanProgram.TrainingPlanProgramName;

                            hdfActivateDeletedTrainingPlanProgramGeographicDivisionCode.Value = previousTrainingPlanProgram.GeographicDivisionCode;
                            hdfActivateDeletedTrainingPlanProgramCode.Value = previousTrainingPlanProgram.TrainingPlanProgramCode;

                            divActivateDeletedDialog.InnerHtml = previousTrainingPlanProgram.TrainingPlanProgramCode == TrainingPlanProgramCode ?
                               Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog")) :
                               Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialogName"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                        }

                        else
                        {
                            txtDuplicatedTrainingPlanProgramCode.Text = previousTrainingPlanProgram.TrainingPlanProgramCode;
                            txtDuplicatedTrainingPlanProgramName.Text = previousTrainingPlanProgram.TrainingPlanProgramName;

                            hdfDuplicatedTrainingPlanProgramGeographicDivisionCode.Value = previousTrainingPlanProgram.GeographicDivisionCode;
                            hdfDuplicatedTrainingPlanProgramCode.Value = previousTrainingPlanProgram.TrainingPlanProgramCode;

                            divDuplicatedDialogText.InnerHtml = previousTrainingPlanProgram.TrainingPlanProgramCode == TrainingPlanProgramCode ?
                               Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog")) :
                               Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialogName"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated(); ", true);
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    var result = ObjTrainingPlanProgramsBll.Edit(
                         hdfTrainingPlanProgramGeographicDivisionCode, 
                         hdfTrainingPlanProgramCode,
                         TrainingPlanProgramName,
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
                        TrainingPlanProgramEntity previousTrainingPlanProgram = result.Item2;
                        bool sameDivisionTrainers = false;

                        if (SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode.Equals(previousTrainingPlanProgram.DivisionCode))
                        {
                            sameDivisionTrainers = true;
                        }

                        if (previousTrainingPlanProgram.Deleted && sameDivisionTrainers)
                        {
                            txtActivateDeletedTrainingPlanProgramCode.Text = previousTrainingPlanProgram.TrainingPlanProgramCode;
                            txtActivateDeletedTrainingPlanProgramName.Text = previousTrainingPlanProgram.TrainingPlanProgramName;

                            hdfActivateDeletedTrainingPlanProgramGeographicDivisionCode.Value = previousTrainingPlanProgram.GeographicDivisionCode;
                            hdfActivateDeletedTrainingPlanProgramCode.Value = previousTrainingPlanProgram.TrainingPlanProgramCode;

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
                string TrainingPlanProgramGeographicDivisionGeographicDivisionCode = hdfActivateDeletedTrainingPlanProgramGeographicDivisionCode.Value;
                string TrainingPlanProgramCode = hdfActivateDeletedTrainingPlanProgramCode.Value;
                string TrainingPlanProgramName = txtTrainingPlanProgramName.Text.Trim();

                bool searchEnable = chkSearchEnabled.Checked;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    ObjTrainingPlanProgramsBll.Activate(TrainingPlanProgramGeographicDivisionGeographicDivisionCode, TrainingPlanProgramCode, lastModifiedUser);

                    RefreshTable();
                    hdfSelectedRowIndex.Value = "0";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                }

                //update and activate the deleted item
                else
                {
                    ObjTrainingPlanProgramsBll.Edit(
                        TrainingPlanProgramGeographicDivisionGeographicDivisionCode
                        , TrainingPlanProgramCode
                        , TrainingPlanProgramName
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
                    string selectedTrainingPlanProgramGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                    string selectedTrainingPlanProgramCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingPlanProgramCode"]);
                    DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                    TrainingPlanProgramEntity TrainingPlanProgram = ObjTrainingPlanProgramsBll.ListByCode(selectedTrainingPlanProgramGeographicDivisionCode, workingDivision.DivisionCode, selectedTrainingPlanProgramCode);

                    hdfTrainingPlanProgramGeographicDivisionCodeEdit.Value = selectedTrainingPlanProgramGeographicDivisionCode;
                    hdfTrainingPlanProgramCodeEdit.Value = selectedTrainingPlanProgramCode;

                    txtTrainingPlanProgramCode.Text = TrainingPlanProgram.TrainingPlanProgramCode;
                    txtTrainingPlanProgramCode.Enabled = false;
                    txtTrainingPlanProgramName.Text = TrainingPlanProgram.TrainingPlanProgramName;
                    chkSearchEnabled.Checked = TrainingPlanProgram.SearchEnabled;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);
                }

                else
                {
                    btnAdd.Disabled = false;
                    btnEdit.Disabled = false;
                    btnDelete.Disabled = false;
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

        #region Master Programs

        /// <summary>
        /// Handles the btnRefreshAssociatedMasterPrograms click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnRefreshMasterPrograms_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshAssociatedMasterPrograms();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppAssociatedMasterPrograms
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppAssociatedMasterPrograms
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnEditMasterProgram click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEditMasterPrograms_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    LoadAssociatedMasterPrograms();
                    List<MasterProgramEntity> associatedMasterPrograms = (List<MasterProgramEntity>)Session[sessionKeyAssociatedMasterPrograms];

                    rptAssociatedMasterPrograms.DataSource = associatedMasterPrograms;
                    rptAssociatedMasterPrograms.DataBind();
                    uppAssociatedMasterPrograms.Update();

                    txtSearchMasterPrograms.Text = string.Empty;
                    uppSearchBarMasterProgram.Update();

                    txtSearchMasterPrograms.Text = string.Empty;
                    rptMasterPrograms.DataSource = null;
                    rptMasterPrograms.DataBind();
                    uppSearchMasterPrograms.Update();

                    lblSearchMasterProgramsResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchMasterProgramsResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchMasterProgramsResultsCount")), 0, 0));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditMasterProgramsClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditMasterProgramsClickPostBack(); }}, 200);", true);
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
        /// Handles the txtSearchMasterPrograms text changed event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void TxtSearchMasterPrograms_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FilterSearchedMasterPrograms();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppSearchMasterPrograms
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppSearchMasterPrograms
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the btnDeleteAssociatedMasterProgram click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDeleteMasterProgram_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedTrainingPlanProgramGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                    string selectedTrainingPlanProgramCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingPlanProgramCode"]);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string masterProgramCode = ((TextBox)htmlButton.Parent.FindControl("hdfMasterProgramCode")).Text;
                    string masterProgramGeographicDivisionCode = ((TextBox)htmlButton.Parent.FindControl("hdfMasterProgramGeographicDivisionCode")).Text;

                    LoadAssociatedMasterPrograms();
                    List<MasterProgramEntity> associatedMasterPrograms = (List<MasterProgramEntity>)Session[sessionKeyAssociatedMasterPrograms];

                    List<MasterProgramEntity> removedMasterPrograms =
                        associatedMasterPrograms.Where(at =>
                            string.Equals(masterProgramCode, at.MasterProgramCode) &&
                            string.Equals(masterProgramGeographicDivisionCode, at.GeographicDivisionCode)
                    ).ToList();

                    if (removedMasterPrograms.Count >= 1)
                    {
                        ObjTrainingPlanProgramsBll.DeleteMasterProgramByTrainingPlanPrograms(
                             removedMasterPrograms[0],
                             new TrainingPlanProgramEntity(
                                 selectedTrainingPlanProgramGeographicDivisionCode,
                                 selectedTrainingPlanProgramCode,
                                 UserHelper.GetCurrentFullUserName)
                         );

                        associatedMasterPrograms.RemoveAll(at => string.Equals(masterProgramCode, at.MasterProgramCode));

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteMasterProgramClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnDeleteMasterProgramClickPostBack('{0}'); }}, 200);", btnDelete.ClientID), true);
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
        /// Handles the btnAddMasterProgram click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddMasterProgram_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedTrainingPlanProgramGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                    string selectedTrainingPlanProgramCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingPlanProgramCode"]);

                    HtmlButton htmlButton = (HtmlButton)sender;
                    string masterProgramCode = ((TextBox)htmlButton.Parent.FindControl("hdfAddMasterProgramCode")).Text;
                    string masterProgramGeographicDivisionCode = ((TextBox)htmlButton.Parent.FindControl("hdfAddMasterProgramGeographicDivisionCode")).Text;

                    LoadMasterPrograms();
                    DataTable masterPrograms = (DataTable)Session[sessionKeyMasterProgramsResults];

                    LoadAssociatedMasterPrograms();
                    List<MasterProgramEntity> associatedMasterPrograms = (List<MasterProgramEntity>)Session[sessionKeyAssociatedMasterPrograms];

                    List<MasterProgramEntity> addedMasterProgram =
                        masterPrograms.AsEnumerable().Where(t =>
                            string.Equals(masterProgramCode, t.Field<string>("MasterProgramCode")) &&
                            string.Equals(masterProgramGeographicDivisionCode, t.Field<string>("GeographicDivisionCode")))
                                .Select(t =>
                                    new MasterProgramEntity
                                    {
                                        GeographicDivisionCode = t.Field<string>("GeographicDivisionCode"),
                                        MasterProgramCode = t.Field<string>("MasterProgramCode"),
                                        MasterProgramName = t.Field<string>("MasterProgramName")
                                    }).ToList();

                    if (addedMasterProgram.Count >= 1)
                    {
                        ObjTrainingPlanProgramsBll.AddMasterProgramByTrainingPlanPrograms(
                            addedMasterProgram[0],
                            new TrainingPlanProgramEntity(
                                selectedTrainingPlanProgramGeographicDivisionCode,
                                selectedTrainingPlanProgramCode,
                                UserHelper.GetCurrentFullUserName)
                            );

                        associatedMasterPrograms.Add(addedMasterProgram[0]);

                        rptAssociatedMasterPrograms.DataSource = associatedMasterPrograms;
                        rptAssociatedMasterPrograms.DataBind();
                        uppAssociatedMasterPrograms.Update();

                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAddMasterProgramClickPostBack{0}", Guid.NewGuid()), string.Format("setTimeout(function () {{ ReturnFromBtnAddMasterProgramClickPostBack('{0}'); }}, 200);", btnAdd.ClientID), true);
                    }
                }

                else
                {
                    btnDelete.Disabled = false;
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
                    LoadAssociatedMasterPrograms();
                    List<MasterProgramEntity> associatedMasterPrograms = (List<MasterProgramEntity>)Session[sessionKeyAssociatedMasterPrograms];

                    if (associatedMasterPrograms.Count > 0)
                    {
                        MensajeriaHelper.MostrarMensaje(Page,
                            TipoMensaje.Advertencia,
                            GetLocalResourceObject("msgAssociatedMasterPrograms").ToString());

                        return;
                    }

                    string selectedTrainingPlanProgramGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                    string selectedTrainingPlanProgramCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingPlanProgramCode"]);
                    DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                    List<MasterProgramEntity> masterProgramsAssociated = ObjIMasterProgramBll.MasterProgramByTrainingPlanProgramsAssociated(selectedTrainingPlanProgramGeographicDivisionCode, workingDivision.DivisionCode, selectedTrainingPlanProgramCode);
                    if (masterProgramsAssociated.Count == 0)
                    {
                        ObjTrainingPlanProgramsBll.Delete(
                            selectedTrainingPlanProgramGeographicDivisionCode,
                            workingDivision.DivisionCode,
                            selectedTrainingPlanProgramCode,
                            UserHelper.GetCurrentFullUserName);

                        PageHelper<TrainingPlanProgramEntity> pageHelper = (PageHelper<TrainingPlanProgramEntity>)Session[sessionKeyTrainingPlanProgramsResults];

                        pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.TrainingPlanProgramCode == selectedTrainingPlanProgramCode));
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
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msjTrainingPlanProgramAsociatedMasterProgramsException").ToString());
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
            if (Session[sessionKeyTrainingPlanProgramsResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<TrainingPlanProgramEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Load empty data structure for MasterPrograms
        /// </summary>
        /// <returns>Empty data structure for MasterPrograms</returns>
        private DataTable LoadEmptyMasterPrograms()
        {
            DataTable MasterPrograms = new DataTable();
            MasterPrograms.Columns.Add("GeographicDivisionCode", typeof(string));
            MasterPrograms.Columns.Add("MasterProgramCode", typeof(string));
            MasterPrograms.Columns.Add("MasterProgramName", typeof(string));

            return MasterPrograms;
        }

        /// <summary>
        /// Load MasterPrograms from database
        /// </summary>        
        private void LoadMasterPrograms()
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                DataTable masterPrograms = LoadEmptyMasterPrograms();

                List<MasterProgramEntity> registeredMasterPrograms = ObjIMasterProgramBll.MasterProgramByTrainingPlanProgramsNotAssociated(
                    SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

                registeredMasterPrograms.ForEach(x => masterPrograms.Rows.Add(x.GeographicDivisionCode, x.MasterProgramCode, x.MasterProgramName));

                Session[sessionKeyMasterProgramsResults] = masterPrograms;
            }

            else
            {
                Session[sessionKeyMasterProgramsResults] = new List<MasterProgramEntity>();
            }
        }

        /// <summary>
        /// Load associated masterPrograms
        /// </summary>        
        private void LoadAssociatedMasterPrograms()
        {
            if (hdfSelectedRowIndex.Value != "-1")
            {
                string selectedTrainingPlanProgramGeographicDivisionCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["GeographicDivisionCode"]);
                string selectedTrainingPlanProgramCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["TrainingPlanProgramCode"]);
                DivisionByUserEntity workingDivision = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision);

                Session[sessionKeyAssociatedMasterPrograms] = ObjIMasterProgramBll.MasterProgramByTrainingPlanProgramsAssociated(selectedTrainingPlanProgramGeographicDivisionCode, workingDivision.DivisionCode, selectedTrainingPlanProgramCode);
            }

            else
            {
                Session[sessionKeyAssociatedMasterPrograms] = new List<TrainerEntity>();
            }
        }

        /// <summary>
        /// Refresh the associated masterPrograms tables by rebinding data
        /// </summary>
        private void RefreshAssociatedMasterPrograms()
        {
            if (Session[sessionKeyAssociatedMasterPrograms] != null)
            {
                rptAssociatedMasterPrograms.DataSource = Session[sessionKeyAssociatedMasterPrograms];
                rptAssociatedMasterPrograms.DataBind();
            }
        }

        /// <summary>
        /// Filter the searched MasterPrograms by search tokens
        /// </summary>
        private void FilterSearchedMasterPrograms()
        {
            LoadMasterPrograms();
            DataTable existentMasterPrograms = (DataTable)Session[sessionKeyMasterProgramsResults];
            DataTable masterPrograms = existentMasterPrograms.Copy();

            LoadAssociatedMasterPrograms();
            List<MasterProgramEntity> associatedMasterPrograms = (List<MasterProgramEntity>)Session[sessionKeyAssociatedMasterPrograms];

            associatedMasterPrograms.ForEach(at =>
            {
                masterPrograms.AsEnumerable().Where(t =>
                    string.Equals(at.MasterProgramCode, t.Field<string>("MasterProgramCode"))).ToList().ForEach(t => masterPrograms.Rows.Remove(t));
            });

            if (!string.IsNullOrWhiteSpace(txtSearchMasterPrograms.Text))
            {
                char[] charSeparators = new char[] { ',', ' ' };
                string[] terms = txtSearchMasterPrograms.Text.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                string filter = string.Empty;
                foreach (string term in terms)
                {
                    string subfilter = string.Format("(MasterProgramCode Like '%{0}%' OR MasterProgramName Like '%{0}%')", term);
                    filter = string.Format("{0} {1} {2} ", filter, string.IsNullOrWhiteSpace(filter) ? "" : " AND ", subfilter);
                }

                masterPrograms.DefaultView.RowFilter = filter;

                DataTable finalTopResults = LoadEmptyMasterPrograms();
                int minRowCount = Math.Min(10, masterPrograms.DefaultView.Count);

                for (int i = 0; i < minRowCount; i++)
                {
                    finalTopResults.ImportRow(masterPrograms.DefaultView[i].Row);
                }

                rptMasterPrograms.DataSource = finalTopResults;
                rptMasterPrograms.DataBind();

                lblSearchMasterProgramsResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchMasterProgramsResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchMasterProgramsResultsCount")), minRowCount, masterPrograms.DefaultView.Count));
            }

            else
            {
                masterPrograms.DefaultView.RowFilter = "1 = 0";
                rptMasterPrograms.DataSource = null;
                rptMasterPrograms.DataBind();

                lblSearchMasterProgramsResults.InnerHtml = string.Format("{0} {1}",
                    Convert.ToString(GetLocalResourceObject("lblSearchMasterProgramsResults")),
                    string.Format(Convert.ToString(GetLocalResourceObject("lblSearchMasterProgramsResultsCount")), 0, 0));
            }

            ScriptManager.RegisterStartupScript(uppSearchMasterPrograms, uppSearchMasterPrograms.GetType(), string.Format("ReturnFromSearchMasterProgramsPostBack{0}", Guid.NewGuid()), "ReturnFromSearchMasterProgramsPostBack()", true);
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<TrainingPlanProgramEntity> SearchResults(int page)
        {
            string TrainingPlanProgramCode = string.IsNullOrWhiteSpace(txtTrainingPlanProgramCodeFilter.Text) ? null : txtTrainingPlanProgramCodeFilter.Text.Trim();
            string TrainingPlanProgramName = string.IsNullOrWhiteSpace(txtTrainingPlanProgramNameFilter.Text) ? null : txtTrainingPlanProgramNameFilter.Text.Trim();

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<TrainingPlanProgramEntity> pageHelper = ObjTrainingPlanProgramsBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, 
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode, 
                TrainingPlanProgramCode, TrainingPlanProgramName, sortExpression, sortDirection, page);

            Session[sessionKeyTrainingPlanProgramsResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyTrainingPlanProgramsResults] != null)
            {
                PageHelper<TrainingPlanProgramEntity> pageHelper = (PageHelper<TrainingPlanProgramEntity>)Session[sessionKeyTrainingPlanProgramsResults];

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