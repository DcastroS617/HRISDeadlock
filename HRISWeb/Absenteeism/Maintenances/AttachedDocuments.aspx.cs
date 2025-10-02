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

namespace HRISWeb.Absenteeism
{
    public partial class AttachedDocuments : System.Web.UI.Page
    {
        [Dependency]
        public IAbsenteeismAttachedDocumentsBll<AbsenteeismAttachedDocumentEntity> ObjAttachedDocumentsBll { get; set; }

        [Dependency]
        public IDivisionsBll<DivisionEntity> ObjDivisionsBll { get; set; }

        //session key for the results
        readonly string sessionKeyAttachedDocumentsResults = "AttachedDocuments-AttachedDocumentsResults";
        readonly string sessionKeyAttachedDocumentsLoaded = "AttachedDocuments-AttachedDocumentsLoaded";

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
                    Session[sessionKeyAttachedDocumentsLoaded] = "0"; // 0 = Documents Available, 1 = Documents Deleted

                    List<DivisionEntity> divisions = ObjDivisionsBll.ListAll();

                    cboDocumentState.Items.Insert(cboDocumentState.Items.Count, new ListItem("Activo", "false"));
                    cboDocumentState.Items.Insert(cboDocumentState.Items.Count, new ListItem("Inactivo", "true"));

                    SetDocumentsByDivisionsAsNew();
                    txtAttachedDocumentCode.Enabled = false;

                    //fire the event
                    BtnSearch_ServerClick(sender, e);
                }

                chbSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chbSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));

                //activate the pager
                if (Session[sessionKeyAttachedDocumentsResults] != null)
                {
                    PageHelper<AbsenteeismAttachedDocumentEntity> pageHelper = (PageHelper<AbsenteeismAttachedDocumentEntity>)Session[sessionKeyAttachedDocumentsResults];
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
        /// Handles the pre render of the page
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                cboDocumentState.SelectedIndex = Convert.ToInt32(Session[sessionKeyAttachedDocumentsLoaded]);
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
                string attachedDocumentCode =
                    !string.IsNullOrWhiteSpace(hdfActivateDeletedAttachedDocumentCode.Value) ?
                        hdfActivateDeletedAttachedDocumentCode.Value :
                        null;

                int? attachedDocumentDivisionCode =
                    !string.IsNullOrWhiteSpace(hdfActivateDeletedAttachedDocumentDivisionCode.Value) ?
                        Convert.ToInt32(hdfActivateDeletedAttachedDocumentDivisionCode.Value) :
                        (int?)null;

                AbsenteeismAttachedDocumentEntity attachedDocument =
                    new AbsenteeismAttachedDocumentEntity(
                        attachedDocumentCode,
                        txtAttachedDocumentName.Text.Trim(),
                        attachedDocumentDivisionCode,
                        txtComments.Text.Trim(),
                        chbSearchEnabled.Checked,
                        true,
                        UserHelper.GetCurrentFullUserName,
                        DateTime.Now
                        );

                PageHelper<AbsenteeismAttachedDocumentEntity> pageHelper = (PageHelper<AbsenteeismAttachedDocumentEntity>)Session[sessionKeyAttachedDocumentsResults];

                //activate the deleted item
                if (chbActivateDeleted.Checked)
                {
                    ObjAttachedDocumentsBll.Activate(attachedDocument);

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
                    attachedDocument.Deleted = false;

                    ObjAttachedDocumentsBll.Edit(attachedDocument);

                    if (pageHelper != null)
                    {
                        string position = hdfSelectedRowIndex.Value;

                        SearchResults(pageHelper.CurrentPage);
                        DisplayResults();

                        hdfSelectedRowIndex.Value = position;
                    }

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);
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
                string attachedDocumentCode =
                    !string.IsNullOrWhiteSpace(hdfAttachedDocumentCodeEdit.Value) ?
                        hdfAttachedDocumentCodeEdit.Value :
                        txtAttachedDocumentCode.Text.Trim();

                AbsenteeismAttachedDocumentEntity attachedDocument = new AbsenteeismAttachedDocumentEntity()
                {
                    DocumentCode = attachedDocumentCode,
                    DocumentName = txtAttachedDocumentName.Text.Trim(),
                    DocumentDescription = txtComments.Text.Trim(),
                    SearchEnabled = chbSearchEnabled.Checked,
                    Deleted = Convert.ToBoolean(cboDocumentState.SelectedValue),
                    LastModifiedUser = UserHelper.GetCurrentFullUserName,
                    LastModifiedDate = DateTime.Now,
                    AttachedDocumentsByDivision = ReadAttachedDocumentsByDivision()
                };

                PageHelper<AbsenteeismAttachedDocumentEntity> pageHelper = (PageHelper<AbsenteeismAttachedDocumentEntity>)Session[sessionKeyAttachedDocumentsResults];

                if (string.IsNullOrWhiteSpace(hdfAttachedDocumentCodeEdit.Value))
                {

                    ObjAttachedDocumentsBll.Add(attachedDocument);
                    if (pageHelper != null)
                    {
                        if (Convert.ToInt32(Session[sessionKeyAttachedDocumentsLoaded]) == 1)
                        {
                            SetInterfaceState();
                        }

                        SearchResults(pageHelper.CurrentPage);
                        DisplayResults();
                        SetDocumentsByDivisionsAsNew();
                    }

                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBack(); }}, 200);", true);
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromCboDocumentChangePostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromCboDocumentChangePostBack(); }}, 200);", true);
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    ObjAttachedDocumentsBll.Edit(attachedDocument);

                    if (pageHelper != null)
                    {
                        pageHelper.ResultList.Remove(
                            pageHelper.ResultList.Find(x => x.DocumentCode == attachedDocument.DocumentCode)
                        );

                        pageHelper.ResultList.Insert(Convert.ToInt32(hdfSelectedRowIndex.Value), attachedDocument);

                        DisplayResults();
                        SetDocumentsByDivisionsAsNew();
                    }

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBack(); }}, 200);", true);
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
                    string selectedAttachedDocumentCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["DocumentCode"]);

                    AbsenteeismAttachedDocumentEntity attachedDocument = ObjAttachedDocumentsBll.ListByKey(selectedAttachedDocumentCode);

                    hdfAttachedDocumentCodeEdit.Value = attachedDocument.DocumentCode;
                    hdfAttachedDocumentDivisionCodeEdit.Value = Convert.ToString(attachedDocument.DivisionCode);

                    txtAttachedDocumentCode.Text = attachedDocument.DocumentCode;
                    txtAttachedDocumentCode.Enabled = false;

                    txtAttachedDocumentName.Text = attachedDocument.DocumentName;
                    txtComments.Text = attachedDocument.DocumentDescription;
                    chbSearchEnabled.Checked = attachedDocument.SearchEnabled;

                    List<KeyValuePair<DivisionEntity, string>> divisions = ObjDivisionsBll.ListAllWithGeographicDivision();
                    DataTable divisionEnabled = GetDivisionsEnabledStructure();

                    attachedDocument.AttachedDocumentsByDivision.ForEach(acd =>
                    {
                        DivisionEntity division = divisions.First(d => d.Key.DivisionCode == acd.DivisionCode).Key;
                        divisionEnabled.Rows.Add(true, acd.DivisionCode, division.DivisionName);

                        divisions.RemoveAll(d => d.Key.DivisionCode == acd.DivisionCode);
                    });

                    divisions.ForEach(d =>
                    {
                        divisionEnabled.Rows.Add(false, d.Key.DivisionCode, d.Key.DivisionName);
                    });

                    rptDocumentsByDivision.DataSource = divisionEnabled;
                    rptDocumentsByDivision.DataBind();

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

        /// <summary>
        /// Handles the btnActivate click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnActivate_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedAttachedDocumentCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["DocumentCode"]);

                    ObjAttachedDocumentsBll.Activate(
                        new AbsenteeismAttachedDocumentEntity()
                        {
                            DocumentCode = selectedAttachedDocumentCode,
                            LastModifiedUser = UserHelper.GetCurrentFullUserName
                        });

                    PageHelper<AbsenteeismAttachedDocumentEntity> pageHelper = SearchResults(1, Convert.ToInt32(Session[sessionKeyAttachedDocumentsLoaded]));
                    CommonFunctions.ResetSortDirection(Page.ClientID, grvList.ClientID);

                    DisplayResults();

                    //aquí realizamos la deselección del borrado
                    hdfSelectedRowIndex.Value = "-1";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromCboDocumentChangePostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromCboDocumentChangePostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , GetLocalResourceObject("msgInvalidSelection").ToString());
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromCboDocumentChangePostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromCboDocumentChangePostBack(); }}, 200);", true);

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

        #region Document State

        protected void CboDocumentState_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetInterfaceState();
                PageHelper<AbsenteeismAttachedDocumentEntity> pageHelper = SearchResults(1, Convert.ToInt32(Session[sessionKeyAttachedDocumentsLoaded]));
                CommonFunctions.ResetSortDirection(Page.ClientID, grvList.ClientID);

                DisplayResults();
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromCboDocumentChangePostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromCboDocumentChangePostBack(); }}, 200);", true);
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
        /// Handles the btnDelete click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        ///
        protected void BtnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    string selectedAttachedDocumentCode = Convert.ToString(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["DocumentCode"]);

                    ObjAttachedDocumentsBll.Delete(
                        new AbsenteeismAttachedDocumentEntity()
                        {
                            DocumentCode = selectedAttachedDocumentCode,
                            LastModifiedUser = UserHelper.GetCurrentFullUserName
                        });
      
                    PageHelper<AbsenteeismAttachedDocumentEntity> pageHelper = (PageHelper<AbsenteeismAttachedDocumentEntity>)Session[sessionKeyAttachedDocumentsResults];

                    pageHelper.ResultList.Remove(
                        pageHelper.ResultList.Find(x => x.DocumentCode == selectedAttachedDocumentCode)
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
                    if (ex.Message.Contains("msjDocumentUsed"))
                    {
                        string message = string.Format("{0} {1}",
                            Convert.ToString(GetLocalResourceObject("msjDocumentUsed")),
                            ex.Message.Replace("msjDocumentUsed.", ""));

                        MensajeriaHelper.MostrarMensaje(Page
                      , TipoMensaje.Error
                      , message);
                    }

                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page
                                              , TipoMensaje.Error
                                              , ex.Message);
                    }
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
                hdfAttachedDocumentCodeFilter.Value = txtAttachedDocumentCodeFilter.Text;
                hdfAttachedDocumentNameFilter.Value = txtAttachedDocumentNameFilter.Text;

                int num = Convert.ToInt32(Session[sessionKeyAttachedDocumentsLoaded]);
                PageHelper<AbsenteeismAttachedDocumentEntity> pageHelper = SearchResults(1, num);
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
            if (Session[sessionKeyAttachedDocumentsResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<AbsenteeismAttachedDocumentEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        #endregion

        #region Private methods

        public string SetCssCheckButton(int number)
        {
            return "enableDivisionMappingSpan chbEnabled_" + number;
        }

        /// <summary>
        /// Get the documents by divisions data table structure
        /// </summary>
        /// <returns>The documents by division data table structure</returns>
        private static DataTable GetDivisionsEnabledStructure()
        {
            DataTable divisionsEnabled = new DataTable();
            divisionsEnabled.Columns.Add("Enabled", typeof(bool));
            divisionsEnabled.Columns.Add("Division", typeof(string));
            divisionsEnabled.Columns.Add("DivisionName", typeof(string));

            return divisionsEnabled;
        }

        /// <summary>
        /// Return the user input for in the repeater
        /// </summary>
        private List<AbsenteeismAttachedDocumentByDivisionEntity> ReadAttachedDocumentsByDivision()
        {
            List<AbsenteeismAttachedDocumentByDivisionEntity> attachedDocumentByDivisions = new List<AbsenteeismAttachedDocumentByDivisionEntity>();

            foreach (RepeaterItem item in rptDocumentsByDivision.Items)
            {
                CheckBox chbEnabled = (CheckBox)item.FindControl("chbEnabled");
                if (chbEnabled.Checked)
                {
                    HiddenField hdfDivisionCode = (HiddenField)item.FindControl("hdfDivisionCode");
                    HiddenField hdfDivisionName = (HiddenField)item.FindControl("hdfDivisionName");

                    attachedDocumentByDivisions.Add(new AbsenteeismAttachedDocumentByDivisionEntity()
                    {
                        DivisionCode = Convert.ToInt32(hdfDivisionCode.Value),
                        DivisionName = hdfDivisionName.Value,
                        LastModifiedUser = UserHelper.GetCurrentFullUserName
                    });
                }
            }

            return attachedDocumentByDivisions;
        }

        /// <summary>
        /// Set the documents by division repeater as new item
        /// </summary>
        private void SetDocumentsByDivisionsAsNew()
        {
            txtAttachedDocumentCode.Enabled = false;

            List<KeyValuePair<DivisionEntity, string>> divisions = ObjDivisionsBll.ListAllWithGeographicDivision();
            DataTable divisionEnabled = GetDivisionsEnabledStructure();

            foreach (KeyValuePair<DivisionEntity, string> d in divisions)
            {
                divisionEnabled.Rows.Add(false, d.Key.DivisionCode, d.Key.DivisionName);
            }

            rptDocumentsByDivision.DataSource = divisionEnabled;
            rptDocumentsByDivision.DataBind();
        }

        private void SetInterfaceState()
        {
            if (Convert.ToInt32(Session[sessionKeyAttachedDocumentsLoaded]) == 1)
            {
                hdfDocumentLoaded.Value = "0";
                Session[sessionKeyAttachedDocumentsLoaded] = "0";
            }

            else
            {
                hdfDocumentLoaded.Value = "1";
                Session[sessionKeyAttachedDocumentsLoaded] = "1";
            }
        }

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<AbsenteeismAttachedDocumentEntity> SearchResults(int page, int deleted = 0)
        {
            string attachedDocumentCode = string.IsNullOrWhiteSpace(hdfAttachedDocumentCodeFilter.Value) ? null : hdfAttachedDocumentCodeFilter.Value;
            string attachedDocumentName = string.IsNullOrWhiteSpace(hdfAttachedDocumentNameFilter.Value) ? null : hdfAttachedDocumentNameFilter.Value;
            bool deletedOption = Convert.ToBoolean(cboDocumentState.SelectedValue);

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<AbsenteeismAttachedDocumentEntity> pageHelper = ObjAttachedDocumentsBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                attachedDocumentCode, attachedDocumentName,
                sortExpression, sortDirection, page, deletedOption.ToInt32());

            Session[sessionKeyAttachedDocumentsResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyAttachedDocumentsResults] != null)
            {
                PageHelper<AbsenteeismAttachedDocumentEntity> pageHelper = (PageHelper<AbsenteeismAttachedDocumentEntity>)Session[sessionKeyAttachedDocumentsResults];

                grvList.DataSource = pageHelper.ResultList;
                grvList.DataBind();

                htmlResultsSubtitle.InnerHtml = string.Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);

                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
            }

            else
            {
                htmlResultsSubtitle.InnerHtml = Convert.ToString(GetLocalResourceObject("lblResultsSubtitle"));
            }

            hdfSelectedRowIndex.Value = "-1";
        }


        #endregion
    }
}