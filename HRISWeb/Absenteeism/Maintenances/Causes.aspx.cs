using HRISWeb.Shared;
using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using System;
using System.Web.UI;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using DOLE.HRIS.Application.Business.Interfaces;
using Unity.Attributes;
using DOLE.HRIS.Shared.Entity;
using System.Linq;
using ClosedXML.Excel;
using System.IO;
using static System.String;
using System.Text.RegularExpressions;

namespace HRISWeb.Training
{
    public partial class Causes : System.Web.UI.Page
    {
        [Dependency]
        public IAbsenteeismCausesBll<AbsenteeismCauseEntity> objCausesBll { get; set; }

        [Dependency]
        public IAbsenteeismCausesLocalBll<AbsenteeismCauseLocalEntity> objCausesLocalBll { get; set; }

        [Dependency]
        public IAbsenteeismInterestGroupBll<AbsenteeismInterestGroupEntity> objInterestGroupBll { get; set; }
        
        [Dependency]
        public IDivisionsBll<DivisionEntity> objDivisionsBll { get; set; }

        /// <summary>
        /// Session to store all causes local
        /// </summary>
        readonly string sessionKeyCausesLocal = "CausesLocal";

        //session key for the results
        readonly string sessionKeyCausesResults = "Causes-CausesResults";
        readonly string sessionKeyInterestGroup = "InterestGroups";

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
                List<AbsenteeismCauseCategoryEntity> causeCategories = null;
                if (!IsPostBack)
                {
                    LoadUserDivisions();
                    if (Session[sessionKeyCausesLocal] == null)
                    {
                        Session[sessionKeyCausesLocal] = objCausesLocalBll.ListAll();
                    }

                    if (Session[sessionKeyInterestGroup] == null)
                    {
                        Session[sessionKeyInterestGroup] = objInterestGroupBll.ListAll();
                    }

                    causeCategories = LoadCauseCategories();

                    cboCauseCategoryFilter.Enabled = true;
                    cboCauseCategoryFilter.DataValueField = "CauseCategoryCode";
                    cboCauseCategoryFilter.DataTextField = "CauseCategoryName";
                    cboCauseCategoryFilter.DataSource = causeCategories;
                    cboCauseCategoryFilter.DataBind();

                    cboCauseCategory.Enabled = true;
                    cboCauseCategory.DataValueField = "CauseCategoryCode";
                    cboCauseCategory.DataTextField = "CauseCategoryName";
                    cboCauseCategory.DataSource = causeCategories;
                    cboCauseCategory.DataBind();

                    SetCausesByDivisionsAsNew();



                    //fire the event
                    btnSearch_ServerClick(sender, e);
                }

                chbSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chbSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));
                
                //activate the pager
                if (Session[sessionKeyCausesResults] != null)
                {
                    PageHelper<AbsenteeismCauseEntity> pageHelper = (PageHelper<AbsenteeismCauseEntity>)Session[sessionKeyCausesResults];
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
        /// Set the causes by divisions repeater as new item
        /// </summary>
        private void SetCausesByDivisionsAsNew()
        {
            List<KeyValuePair<DivisionEntity, string>> divisions = objDivisionsBll.ListAllWithGeographicDivision();
            DataTable causesByDivisions = GetCausesByDivisionsStructure();
            //List<AbsenteeismCauseLocal> listCausesLocal = (List<AbsenteeismCauseLocal>)Session[sessionKeyCausesLocal];
            //List<AbsenteeismInterestGroup> listInterestGroup = (List<AbsenteeismInterestGroup>)Session[sessionKeyInterestGroup];
            foreach (KeyValuePair<DivisionEntity, string> d in divisions)
            {
                causesByDivisions.Rows.Add(false, d.Value, d.Key.DivisionCode, d.Key.DivisionName, "", false,false,false, string.Empty);
            }
            
            rptCauseByDivision.DataSource = causesByDivisions;
            rptCauseByDivision.DataBind();
        }

        /// <summary>
        /// Get the causes by divisions data table structure
        /// </summary>
        /// <returns>The causes by divisions data table structure</returns>
        private static DataTable GetCausesByDivisionsStructure()
        {
            DataTable causesByDivisions = new DataTable();
            causesByDivisions.Columns.Add("Enabled", typeof(bool));
            causesByDivisions.Columns.Add("GeographicDivision", typeof(string));
            causesByDivisions.Columns.Add("Division", typeof(string));
            causesByDivisions.Columns.Add("DivisionName", typeof(string));
            causesByDivisions.Columns.Add("Mapping", typeof(string));
            causesByDivisions.Columns.Add("AdditionalInfo", typeof(bool));
            causesByDivisions.Columns.Add("Hours", typeof(bool));
            causesByDivisions.Columns.Add("Days", typeof(bool));
            causesByDivisions.Columns.Add("InterestGroupCode", typeof(string));
            return causesByDivisions;
        }

        /// <summary>
        /// Handles the pre render of the page
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {

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
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {                
                hdfCauseCodeFilter.Value = txtCauseCodeFilter.Text;
                hdfCauseNameFilter.Value = txtCauseNameFilter.Text;
                hdfCauseCategoryFilterValueFilter.Value = GetCauseCategoryFilterSelectedValue();
                hdfCauseCategoryFilterTextFilter.Value = GetCauseCategoryFilterSelectedText();
                
                PageHelper<AbsenteeismCauseEntity> pageHelper = SearchResults(1);                
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
        /// Return the user input for causes by division for given cause code
        /// </summary>
        /// <param name="causeCode">Given cause code</param>
        /// <returns>The user input for causes by division for given cause code</returns>
        private List<AbsenteeismCauseByDivisionEntity> ReadAbsenteeismCauseByDivision(string causeCode)
        {
            List<AbsenteeismCauseByDivisionEntity> causesByDivisions = new List<AbsenteeismCauseByDivisionEntity>();

            foreach (RepeaterItem item in rptCauseByDivision.Items)
            {
                CheckBox chbEnabled = (CheckBox)item.FindControl("chbEnabled");
                if(chbEnabled.Checked)
                {
                    HiddenField hdfGeographicDivisionCode = (HiddenField)item.FindControl("hdfGeographicDivisionCode");
                    HiddenField hdfDivisionCode = (HiddenField)item.FindControl("hdfDivisionCode");
                    HiddenField hdfDivisionName = (HiddenField)item.FindControl("hdfDivisionName");
                    TextBox txtMapping = (TextBox)item.FindControl("txtMapping");
                    DropDownList ddlCauseLocal = (DropDownList)item.FindControl("ddlCausesLocal");
                    
                    CheckBox chbAdditionalInfo = (CheckBox)item.FindControl("chbAdditionalInfo");
                    CheckBox chbHours = (CheckBox)item.FindControl("chbHours");
                    CheckBox chbDays = (CheckBox)item.FindControl("chbDays");
                    HiddenField hdfInterestGroupCodes = (HiddenField)item.FindControl("hdfInterestGroupCodes");
                    causesByDivisions.Add(new AbsenteeismCauseByDivisionEntity(
                        hdfGeographicDivisionCode.Value,
                        causeCode,
                        Convert.ToInt32(hdfDivisionCode.Value),
                        ddlCauseLocal.SelectedValue,
                        chbAdditionalInfo.Checked,
                        hdfInterestGroupCodes.Value
                        )
                        {
                            Hours=chbHours.Checked,
                            Days=chbDays.Checked
                        }
                    );
                }
            }

            return causesByDivisions;

        }


        /// <summary>
        /// Return validation fiels
        /// </summary>
        /// <returns>The user input for causes by division for given cause code</returns>
        private bool ReadAbsenteeismCauseByDivisionValidation()
        {
            

            foreach (RepeaterItem item in rptCauseByDivision.Items)
            {
                CheckBox chbEnabled = (CheckBox)item.FindControl("chbEnabled");
                if (chbEnabled.Checked)
                {
                    
                    CheckBox chbHours = (CheckBox)item.FindControl("chbHours");
                    CheckBox chbDays = (CheckBox)item.FindControl("chbDays");


                    if(!chbHours.Checked && !chbDays.Checked)
                    return false;

                }
            }

            return true;

        }
        /// <summary>
        /// Handles the btnActivateDeletedAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">C
        protected void btnActivateDeletedAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string causeCode =
                    !String.IsNullOrWhiteSpace(hdfActivateDeletedCauseCode.Value) ?
                        hdfActivateDeletedCauseCode.Value :
                        null;

                if (!ReadAbsenteeismCauseByDivisionValidation()) {
                    MensajeriaHelper.MostrarMensaje(Page
                     , TipoMensaje.Validacion
                     , GetLocalResourceObject("msgEntryValidationCausesChecked").ToString());

                    return;
                }

                AbsenteeismCauseEntity cause =
                    new AbsenteeismCauseEntity(
                        causeCode,
                        txtCauseName.Text.Trim(),
                        txtComments.Text.Trim(),
                        new AbsenteeismCauseCategoryEntity(
                            GetCauseCategorySelectedData(),
                            GetCauseCategorySelectedText(),
                            string.Empty),
                        chbSearchEnabled.Checked,
                        true,
                        UserHelper.GetCurrentFullUserName,
                        DateTime.Now,
                        ReadAbsenteeismCauseByDivision(causeCode)
                        );
                
                PageHelper<AbsenteeismCauseEntity> pageHelper = (PageHelper<AbsenteeismCauseEntity>)Session[sessionKeyCausesResults];

                //activate the deleted item
                if (chbActivateDeleted.Checked)
                {
                    objCausesBll.Activate(cause);

                    if (pageHelper != null)
                    {
                        SearchResults(pageHelper.CurrentPage);
                        DisplayResults();
                    }

                    hdfSelectedRowIndex.Value = "0";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                }
                
                //update and activate the deleted item
                else
                {                    
                    cause.Deleted = false;

                    objCausesBll.Edit(cause);
                    
                    if (pageHelper != null)
                    {
                        string position = hdfSelectedRowIndex.Value;

                        SearchResults(pageHelper.CurrentPage);
                        DisplayResults();

                        hdfSelectedRowIndex.Value = position;
                    }

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
                                        
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
        protected void btnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {

                if (!ReadAbsenteeismCauseByDivisionValidation())
                {
                    MensajeriaHelper.MostrarMensaje(Page
                     , TipoMensaje.Validacion
                     , GetLocalResourceObject("msgEntryValidationCausesChecked").ToString());

                    return;
                }

                string causeCode =
                    !String.IsNullOrWhiteSpace(hdfCauseCodeEdit.Value) ?
                        hdfCauseCodeEdit.Value :
                        txtCauseCode.Text.Trim();

                AbsenteeismCauseEntity cause = new AbsenteeismCauseEntity(
                    causeCode,
                    txtCauseName.Text.Trim(),
                    txtComments.Text.Trim(),
                    new AbsenteeismCauseCategoryEntity(
                            GetCauseCategorySelectedData(),
                            GetCauseCategorySelectedText(),
                            string.Empty),
                    chbSearchEnabled.Checked,
                    false,
                    UserHelper.GetCurrentFullUserName,
                    DateTime.Now,
                    ReadAbsenteeismCauseByDivision(causeCode));

                PageHelper<AbsenteeismCauseEntity> pageHelper = (PageHelper<AbsenteeismCauseEntity>)Session[sessionKeyCausesResults];

                if (String.IsNullOrWhiteSpace(hdfCauseCodeEdit.Value))
                {
                    Tuple<bool, AbsenteeismCauseEntity> addResult = objCausesBll.Add(cause);
                    if (addResult.Item1)
                    {
                        if (pageHelper != null)
                        {
                            SearchResults(pageHelper.CurrentPage);
                            DisplayResults();
                            SetCausesByDivisionsAsNew();
                        }

                        hdfSelectedRowIndex.Value = "-1";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBack(); }}, 200);", true);
                    }

                    else if (!addResult.Item1)
                    {
                        AbsenteeismCauseEntity previousCause = addResult.Item2;

                        if (previousCause.Deleted)
                        {
                            txtActivateDeletedCauseCode.Text = previousCause.CauseCode;
                            txtActivateDeletedCauseName.Text = previousCause.CauseName;
                            hdfActivateDeletedCauseCode.Value = previousCause.CauseCode;
                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackDeleted(); }}, 200);", true);
                        }
                        else
                        {
                            //duplicated row
                            divDuplicatedDialogText.InnerHtml = previousCause.CauseCode == cause.CauseCode ? Convert.ToString(GetLocalResourceObject("lblTextCodeDuplicatedDialog")) :
                                    Convert.ToString(GetLocalResourceObject("lblTextNameDuplicatedDialog"));
                            txtDuplicatedCauseCode.Text = previousCause.CauseCode;
                            txtDuplicatedCauseName.Text = previousCause.CauseName;
                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackDuplicated(); }}, 200);", true);
                        }

                    }
                }
                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    objCausesBll.Edit(cause);

                    if (pageHelper != null)
                    {
                        pageHelper.ResultList.Remove(
                            pageHelper.ResultList.Find(x => x.CauseCode == cause.CauseCode)
                        );
                        pageHelper.ResultList.Insert(Convert.ToInt32(hdfSelectedRowIndex.Value), cause);
                        DisplayResults();
                        SetCausesByDivisionsAsNew();
                    }

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBack(); }}, 200);", true);

                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("MsgLimitCausesMap")), ex);
                    MensajeriaHelper.MostrarMensaje(Page
                        , TipoMensaje.Error
                        , Convert.ToString(GetLocalResourceObject("MsgLimitCausesMap")));

                }
                else if (ex is DataAccessException
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
            finally {
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFromBtnAcceptClickPostBackFinally{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFinally(); }}, 200);", true);

            }
        }


        /// <summary>
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnEdit_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCauseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["CauseCode"]);

                    AbsenteeismCauseEntity cause = objCausesBll.ListByKey(null, selectedCauseCode, null);

                    hdfCauseCodeEdit.Value = cause.CauseCode;

                    txtCauseCode.Text = cause.CauseCode;
                    txtCauseCode.Enabled = false;
                    txtCauseName.Text = cause.CauseName;
                    cboCauseCategory.SelectedValue = cause.Category.CauseCategoryCode;
                    txtComments.Text = cause.Comments;
                    chbSearchEnabled.Checked = cause.SearchEnabled;

                    List<KeyValuePair<DivisionEntity, string>> divisions = objDivisionsBll.ListAllWithGeographicDivision();
                    DataTable causesByDivisions = GetCausesByDivisionsStructure();

                    cause.AbsenteeismCausesByDivision.ForEach(acd =>
                    {
                        DivisionEntity division = divisions.First(d => d.Key.DivisionCode == acd.DivisionCode).Key;
                        causesByDivisions.Rows.Add(true, acd.GeographicDivisionCode, acd.DivisionCode, division.DivisionName, acd.CauseCodeAdamMapped,
                            acd.NeedsAdditionalInformation,acd.Hours,acd.Days, acd.InterestGroupCodes);

                        divisions.RemoveAll(d => d.Key.DivisionCode == acd.DivisionCode);
                    });

                    divisions.ForEach(d =>
                    {
                        causesByDivisions.Rows.Add(false, d.Value, d.Key.DivisionCode, d.Key.DivisionName, string.Empty, false,false,false, string.Empty);
                    });


                    rptCauseByDivision.DataSource = causesByDivisions;
                    rptCauseByDivision.DataBind();

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnEditClickPostBack();", true);

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
        /// Handles the btnDelete click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void btnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedCauseCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["CauseCode"]);

                    objCausesBll.Delete(
                        new AbsenteeismCauseEntity(
                            selectedCauseCode,
                            UserHelper.GetCurrentFullUserName));

                    PageHelper<AbsenteeismCauseEntity> pageHelper = (PageHelper<AbsenteeismCauseEntity>)Session[sessionKeyCausesResults];

                    pageHelper.ResultList.Remove(
                        pageHelper.ResultList.Find(x => x.CauseCode == selectedCauseCode)
                        );

                    pageHelper.TotalResults--;

                    if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                    {
                        SearchResults(pageHelper.TotalPages - 1);
                    }

                    pageHelper.UpdateTotalPages();
                    DisplayResults();

                    //aquí realizamos la deselección del borrado
                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);
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
        protected void blstPager_Click(object sender, BulletedListEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(((BulletedList)sender).Items[e.Index].Value))
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
        protected void grvList_PreRender(object sender, EventArgs e)
        {
            if ((grvList.ShowHeader && grvList.Rows.Count > 0)
            || (grvList.ShowHeaderWhenEmpty))
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
        protected void grvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session[sessionKeyCausesResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<AbsenteeismCauseEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
                
                DisplayResults();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<AbsenteeismCauseEntity> SearchResults(int page)
        {
            string causeCode = String.IsNullOrWhiteSpace(hdfCauseCodeFilter.Value) ? null : hdfCauseCodeFilter.Value;
            string causeName = String.IsNullOrWhiteSpace(hdfCauseNameFilter.Value) ? null : hdfCauseNameFilter.Value;
            string causeCategoryCode = String.IsNullOrWhiteSpace(hdfCauseCategoryFilterValueFilter.Value) ? null : hdfCauseCategoryFilterValueFilter.Value; 
            
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);
            
            PageHelper<AbsenteeismCauseEntity> pageHelper = objCausesBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                causeCode , causeName, causeCategoryCode
                , sortExpression, sortDirection, page);

            Session[sessionKeyCausesResults] = pageHelper;
            return pageHelper;
        }


        /// <summary>
        /// Returns the selected cause category id
        /// </summary>
        /// <returns>The selected cause category id</returns>
        private string GetCauseCategoryFilterSelectedValue()
        {
            string selected = null;
            if (!String.IsNullOrWhiteSpace(cboCauseCategoryFilter.SelectedValue)
                && !"-1".Equals(cboCauseCategoryFilter.SelectedValue))
            {
                selected = cboCauseCategoryFilter.SelectedValue;
            }
            return selected;
        }

        /// <summary>
        /// Returns the selected cause category text
        /// </summary>
        /// <returns>The selected cause category text</returns>
        private string GetCauseCategoryFilterSelectedText()
        {
            string selected = null;
            if (!String.IsNullOrWhiteSpace(cboCauseCategoryFilter.SelectedValue)
                && !"-1".Equals(cboCauseCategoryFilter.SelectedValue))
            {
                selected = cboCauseCategoryFilter.SelectedItem.Text;
            }
            return selected;
        }

        /// <summary>
        /// Returns the selected cause category id
        /// </summary>
        /// <returns>The selected cause category id</returns>
        private string GetCauseCategorySelectedData()
        {
            string selected = null;
            if (!String.IsNullOrWhiteSpace(cboCauseCategory.SelectedValue)
                && !"-1".Equals(cboCauseCategory.SelectedValue))
            {
                selected = cboCauseCategory.SelectedValue;
            }
            return selected;
        }

        /// <summary>
        /// Returns the selected cause category id
        /// </summary>
        /// <returns>The selected cause category id</returns>
        private string GetCauseCategorySelectedText()
        {
            string selected = null;
            if (!String.IsNullOrWhiteSpace(cboCauseCategory.SelectedValue)
                && !"-1".Equals(cboCauseCategory.SelectedValue))
            {
                selected = cboCauseCategory.SelectedItem.Text;
            }
            return selected;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyCausesResults] != null)
            {
                PageHelper<AbsenteeismCauseEntity> pageHelper = (PageHelper<AbsenteeismCauseEntity>)Session[sessionKeyCausesResults];
                                
                grvList.DataSource = pageHelper.ResultList;
                grvList.DataBind();
                
                htmlResultsSubtitle.InnerHtml = String.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);

                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            }

            else
            {
                htmlResultsSubtitle.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
            }

            hdfSelectedRowIndex.Value = "-1";
        }

        /// <summary>
        /// Loads the cause categories
        /// </summary>
        /// <returns>The cause categories</returns>
        private List<AbsenteeismCauseCategoryEntity> LoadCauseCategories()
        {
            List<AbsenteeismCauseCategoryEntity> causeCategories = objCausesBll.ListCauseCategories();
            causeCategories.Insert(0, new AbsenteeismCauseCategoryEntity("-1", "", ""));

            return causeCategories;
        }



        #endregion

        public List<AbsenteeismCauseLocalEntity> GetDataCausesLocal(string geographicDivisionCode)
        {
            List<AbsenteeismCauseLocalEntity> listCausesLocal = (List<AbsenteeismCauseLocalEntity>)Session[sessionKeyCausesLocal];
            return listCausesLocal.Where(x => x.GeographicDivisionCode == geographicDivisionCode).ToList();
        }

        public string setCssCheckButton(int number)
        {
            return "enableDivisionMappingSpan chbEnabled_"+ number;
        }

        protected void rptCauseByDivision_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                List<AbsenteeismCauseLocalEntity> listCausesLocal = (List<AbsenteeismCauseLocalEntity>)Session[sessionKeyCausesLocal];
                
                HiddenField geographicDivisionCode = (e.Item.FindControl("hdfGeographicDivisionCode") as HiddenField);
                HiddenField hdfCodeMapSelected = (e.Item.FindControl("hdfCodeMapSelected") as HiddenField);
                
                DropDownList ddlCausesLocal = (e.Item.FindControl("ddlCausesLocal") as DropDownList);
                ddlCausesLocal.DataSource = listCausesLocal.Where(x => x.GeographicDivisionCode == geographicDivisionCode.Value.ToString() && Regex.IsMatch(x.AbsenteeismCausesCode, @"\b([A-Z0-9]+)\b")).ToList();
                ddlCausesLocal.DataValueField = "AbsenteeismCausesCode";
                ddlCausesLocal.DataTextField = "AbsenteeismCausesDescription";
                ddlCausesLocal.DataBind();


                if (!String.IsNullOrEmpty(hdfCodeMapSelected.Value))
                {
                    ddlCausesLocal.SelectedValue = hdfCodeMapSelected.Value;
                }


                List<AbsenteeismInterestGroupEntity> listInterestGroups = (List<AbsenteeismInterestGroupEntity>)Session[sessionKeyInterestGroup];
                DropDownList ddlInterestGroup = (e.Item.FindControl("ddlInterestGroupCode") as DropDownList);

                ddlInterestGroup.DataSource = listInterestGroups.Where(x => x.GeographicDivisionCode == geographicDivisionCode.Value.ToString()).ToList();
                ddlInterestGroup.DataValueField = "Code";
                ddlInterestGroup.DataTextField = "Description";
                ddlInterestGroup.DataBind();
                ddlInterestGroup.SelectedIndex = -1;

                HiddenField hdfInterestGroupCodes = (e.Item.FindControl("hdfInterestGroupCodes") as HiddenField);
                if (!String.IsNullOrEmpty(hdfInterestGroupCodes.Value))
                {
                    //ddlInterestGroup.SelectedValue = hdfInterestGroupCodes.Value;

                    var funcion = "MultiSelectDropdownListRestoreSelectedItemsCustom($('#" + ddlInterestGroup.ClientID + "'),$('#" + hdfInterestGroupCodes.ClientID + "'));";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("MultiSelectDropdownListRestoreSelectedItemsCustom{0}", Guid.NewGuid()), funcion, true);
                }
            }
        }

        /// <summary>
        /// Download Excel File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownloadFile_ServerClick(object sender, EventArgs e)
        {
            try
            {
                AbsenteeismCauseInformationEntity absenteeismCauseInformation = objCausesBll.CauseInformation(Convert.ToInt32(cboUserDivisions.SelectedValue));

                if (absenteeismCauseInformation != null)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheetLocalAdam = workbook.Worksheets.Add(Convert.ToString(GetLocalResourceObject("CausesLocalTitle")));
                        var i = 2;
                        #region Header
                        worksheetLocalAdam.Cell(i, 1).Value = Convert.ToString(GetLocalResourceObject("CauseCodeTitle"));
                        worksheetLocalAdam.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#69899F");
                        worksheetLocalAdam.Cell(i, 1).Style.Font.FontColor = XLColor.White;
                        worksheetLocalAdam.Cell(i, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        worksheetLocalAdam.Cell(i, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheetLocalAdam.Cell(i, 2).Value = Convert.ToString(GetLocalResourceObject("CauseDescriptionTitle"));
                        worksheetLocalAdam.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#69899F");
                        worksheetLocalAdam.Cell(i, 2).Style.Font.FontColor = XLColor.White;
                        worksheetLocalAdam.Cell(i, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        worksheetLocalAdam.Cell(i, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        #endregion
                        foreach (AbsenteeismCauseLocalEntity absenteeismAdam in absenteeismCauseInformation.CausesLocal)
                        {
                            i += 1;
                            worksheetLocalAdam.Cell(i, 1).Value = absenteeismAdam.AbsenteeismCausesCode;
                            worksheetLocalAdam.Cell(i, 2).Value = absenteeismAdam.AbsenteeismCausesDescription;
                        }

                        worksheetLocalAdam.Columns().AdjustToContents();
                        worksheetLocalAdam.Rows().AdjustToContents();

                        var worksheetRegionalCauses = workbook.Worksheets.Add(Convert.ToString(GetLocalResourceObject("RegionalCausesTitle")));
                        i = 2;
                        #region Header
                        worksheetRegionalCauses.Cell(i, 1).Value = Convert.ToString(GetLocalResourceObject("CauseCodeTitle"));
                        worksheetRegionalCauses.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#69899F");
                        worksheetRegionalCauses.Cell(i, 1).Style.Font.FontColor = XLColor.White;
                        worksheetRegionalCauses.Cell(i, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        worksheetRegionalCauses.Cell(i, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheetRegionalCauses.Cell(i, 2).Value = Convert.ToString(GetLocalResourceObject("CauseNameTitle"));
                        worksheetRegionalCauses.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#69899F");
                        worksheetRegionalCauses.Cell(i, 2).Style.Font.FontColor = XLColor.White;
                        worksheetRegionalCauses.Cell(i, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        worksheetRegionalCauses.Cell(i, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheetRegionalCauses.Cell(i, 3).Value = Convert.ToString(GetLocalResourceObject("CauseCommentsTitle")); 
                        worksheetRegionalCauses.Cell(i, 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#69899F");
                        worksheetRegionalCauses.Cell(i, 3).Style.Font.FontColor = XLColor.White;
                        worksheetRegionalCauses.Cell(i, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        worksheetRegionalCauses.Cell(i, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheetRegionalCauses.Cell(i, 4).Value = Convert.ToString(GetLocalResourceObject("CauseCategoryTitle")); 
                        worksheetRegionalCauses.Cell(i, 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#69899F");
                        worksheetRegionalCauses.Cell(i, 4).Style.Font.FontColor = XLColor.White;
                        worksheetRegionalCauses.Cell(i, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        worksheetRegionalCauses.Cell(i, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        #endregion
                        foreach (AbsenteeismCauseEntity absenteeismCause in absenteeismCauseInformation.Causes)
                        {
                            i += 1;
                            worksheetRegionalCauses.Cell(i, 1).Value = absenteeismCause.CauseCode;
                            worksheetRegionalCauses.Cell(i, 2).Value = absenteeismCause.CauseName;
                            worksheetRegionalCauses.Cell(i, 3).Value = absenteeismCause.Comments;
                            worksheetRegionalCauses.Cell(i, 4).Value = absenteeismCause.Category.CauseCategoryName;
                        }
                        worksheetRegionalCauses.Columns().AdjustToContents();
                        worksheetRegionalCauses.Rows().AdjustToContents();

                        var divisionName = "";
                        var worksheetCausesByDivision = workbook.Worksheets.Add(Convert.ToString(GetLocalResourceObject("CausesByDivisionTitle")));
                        i = 2;
                        #region Header
                        worksheetCausesByDivision.Cell(i, 1).Value = Convert.ToString(GetLocalResourceObject("CauseNameTitle"));
                        worksheetCausesByDivision.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#69899F");
                        worksheetCausesByDivision.Cell(i, 1).Style.Font.FontColor = XLColor.White;
                        worksheetCausesByDivision.Cell(i, 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        worksheetCausesByDivision.Cell(i, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheetCausesByDivision.Cell(i, 2).Value = Convert.ToString(GetLocalResourceObject("DivisionNameTitle"));
                        worksheetCausesByDivision.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#69899F");
                        worksheetCausesByDivision.Cell(i, 2).Style.Font.FontColor = XLColor.White;
                        worksheetCausesByDivision.Cell(i, 2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        worksheetCausesByDivision.Cell(i, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheetCausesByDivision.Cell(i, 3).Value = Convert.ToString(GetLocalResourceObject("AdamCodeTitle"));
                        worksheetCausesByDivision.Cell(i, 3).Style.Fill.BackgroundColor = XLColor.FromHtml("#69899F");
                        worksheetCausesByDivision.Cell(i, 3).Style.Font.FontColor = XLColor.White;
                        worksheetCausesByDivision.Cell(i, 3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        worksheetCausesByDivision.Cell(i, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheetCausesByDivision.Cell(i, 4).Value = Convert.ToString(GetLocalResourceObject("NeedAdditionalInformationTitle"));
                        worksheetCausesByDivision.Cell(i, 4).Style.Fill.BackgroundColor = XLColor.FromHtml("#69899F");
                        worksheetCausesByDivision.Cell(i, 4).Style.Font.FontColor = XLColor.White;
                        worksheetCausesByDivision.Cell(i, 4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
                        worksheetCausesByDivision.Cell(i, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        #endregion
                        foreach (AbsenteeismCauseByDivisionEntity absenteeismCauseByDivision in absenteeismCauseInformation.CausesByDivision)
                        {
                            i += 1;
                            worksheetCausesByDivision.Cell(i, 1).Value = absenteeismCauseByDivision.CauseName;
                            worksheetCausesByDivision.Cell(i, 2).Value = absenteeismCauseByDivision.DivisionName;
                            worksheetCausesByDivision.Cell(i, 3).Value = absenteeismCauseByDivision.CauseCodeAdamMapped;
                            worksheetCausesByDivision.Cell(i, 4).Value = absenteeismCauseByDivision.NeedsAdditionalInformation ? Convert.ToString(GetLocalResourceObject("Yes")) : Convert.ToString(GetLocalResourceObject("No"));

                            divisionName = absenteeismCauseByDivision.DivisionName;
                        }

                        worksheetCausesByDivision.Columns().AdjustToContents();
                        worksheetCausesByDivision.Rows().AdjustToContents();

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachement;filename=" + Convert.ToString(GetLocalResourceObject("CauseFileName")) + divisionName.Replace('ó', 'o') + ".xlsx");

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            stream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception)
            {
               //nos comemos la excepción pues siempre se genera al descargar el archivo
            }
        }

        /// <summary>
        /// Load Divisions
        /// </summary>
        private void LoadUserDivisions()
        {
            try
            {
                if (SessionManager.DoesKeyExist(SessionKey.UserDivisions))
                {
                    cboUserDivisions.DataValueField = "DivisionCode";
                    cboUserDivisions.DataTextField = "DivisionName";
                    cboUserDivisions.DataSource = SessionManager.GetSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions).OrderBy(d => d.DivisionName);
                    cboUserDivisions.DataBind();

                    if (SessionManager.DoesKeyExist(SessionKey.WorkingDivision))
                    {
                        ListItem liDivision = cboUserDivisions.Items.FindByValue(Convert.ToString(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode));
                        if (liDivision != null)
                        {
                            liDivision.Selected = true;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppDivisions, TipoMensaje.Error, ex.Message);
                }
                else
                {
                    MensajeriaHelper.MostrarMensaje(uppDivisions, TipoMensaje.Error, (new PresentationException(ErrorMessages.msjPresentationExceptionLoadingUserDivisions, ex)).Message);
                }
            }
        }

        protected void ddlInterestGroupCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var x = "hola";
        }
    }
}