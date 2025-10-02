using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;

namespace HRISWeb.Training.Maintenances
{
    public partial class ClassificationCourse : Page
    {
        [Dependency]
        public IClassificationCourseBll ObjIClassificationCourseBll { get; set; }

        //session key for the results
        readonly string sessionKeyClassificationCourseResults = "ClassificationCourse-ClassificationCourseResults";

        public string CultureGlobal { get; set; }

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

                if (Session[Constants.cCulture] != null)
                {
                    CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                    CultureGlobal = ci.Name;
                }
                
                //activate the pager
                if (Session[sessionKeyClassificationCourseResults] != null)
                {
                    PageHelper<ClassificationCourseEntity> pageHelper = (PageHelper<ClassificationCourseEntity>)Session[sessionKeyClassificationCourseResults];
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
                var seleectid = hdfClassificationCourseIdEdit.Value == "-1" || string.IsNullOrEmpty(hdfClassificationCourseIdEdit.Value)
                    ? (int?)null : int.Parse(hdfClassificationCourseIdEdit.Value);

                var classificationCourseCode = ClassificationCourseCodeEdit.Value.Replace(" ", string.Empty);

                var entity = new ClassificationCourseEntity
                {
                    ClassificationCourseId = seleectid,
                    ClassificationCourseCode = classificationCourseCode.Trim(),
                    ClassificationCourseDesEn = ClassificationCourseDescriptionEnEdit.Value.Trim(),
                    ClassificationCourseDesEs = ClassificationCourseDescriptionEsEdit.Value.Trim(),
                    SearchEnabled = SearchEnabledEdit.Checked,
                };

                DbaEntity result = null;
                if (seleectid.HasValue)
                {
                    result = ObjIClassificationCourseBll.ClassificationCourseEdit(entity);
                }

                else
                {
                    result = ObjIClassificationCourseBll.ClassificationCourseAdd(entity);
                }

                if (result.ErrorNumber == 0)
                {
                    hdfSelectedRowIndex.Value = "-1";
                    RefreshTable();

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnPostBackAcceptClickSave(); },200);", true);
                }

                else if (result.ErrorNumber != -1 && result.ErrorNumber != -2)
                {
                    Exception exception = new Exception(result.ErrorMessage);
                    throw exception;
                }

                else
                {
                    RefreshTable();

                    txtDuplicatedClassificationCourseCode.Text = entity.ClassificationCourseCode;
                    txtDuplicatedClassificationCourseDesEn.Text = entity.ClassificationCourseDesEn;
                    txtDuplicatedClassificationCourseDesEs.Text = entity.ClassificationCourseDesEs;
                    var typetext = result.ErrorNumber == -1 ? GetLocalResourceObject("lblCode").ToString() : GetLocalResourceObject("lblDescription").ToString();

                    divDuplicatedDialogText.InnerHtml =
                        string.Format(GetLocalResourceObject("lblTextDuplicatedDialogNoDetails").ToString(), typetext);

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnAcceptClickPostBackDuplicated(); },200); ", true);
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
                btnAdd.Disabled = true;
                btnDelete.Disabled = true;
                btnEdit.Disabled = true;

                if (hdfSelectedRowIndex.Value != "-1")
                {
                    int selectedid = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["ClassificationCourseId"]);
                    var result = ObjIClassificationCourseBll.ClassificationCourseById(new ClassificationCourseEntity
                    {
                        ClassificationCourseId = selectedid

                    });

                    hdfClassificationCourseIdEdit.Value = result.ClassificationCourseId.ToString();
                    ClassificationCourseCodeEdit.Value = result.ClassificationCourseCode;
                    ClassificationCourseDescriptionEnEdit.Value = result.ClassificationCourseDesEn;
                    ClassificationCourseDescriptionEsEdit.Value = result.ClassificationCourseDesEs;
                    SearchEnabledEdit.Checked = result.SearchEnabled;

                    PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFrombtnEdit_ServerClick{0}", Guid.NewGuid()),
                        " setTimeout(function () {  ReturnRequestBtnEditOpen(); },200);  ", true);
                }

                else
                {
                    hdfClassificationCourseIdEdit.Value = "";
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
        protected void BtnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    int selectedClassificationCourseId = Convert.ToInt32(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)]["ClassificationCourseId"]);

                    var result = ObjIClassificationCourseBll.ClassificationCourseDesactivate(
                        new ClassificationCourseEntity
                        {
                            ClassificationCourseId = selectedClassificationCourseId,
                        });

                    if (result.ErrorNumber == 0)
                    {
                        PageHelper<ClassificationCourseEntity> pageHelper = (PageHelper<ClassificationCourseEntity>)Session[sessionKeyClassificationCourseResults];

                        pageHelper.ResultList.Remove(pageHelper.ResultList.Find(x => x.ClassificationCourseId == selectedClassificationCourseId));
                        pageHelper.TotalResults--;

                        if (pageHelper.ResultList.Count == 0 && pageHelper.TotalPages > 1)
                        {
                            SearchResults(pageHelper.TotalPages - 1);
                            PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages - 1);
                        }

                        pageHelper.UpdateTotalPages();
                        RefreshTable();
                    }

                    else
                    {
                        Exception exception = new Exception(result.ErrorMessage);
                        throw exception;
                    }

                    hdfSelectedRowIndex.Value = "-1";
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {  ReturnFromBtnDeleteClickPostBack(); },200);", true);
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
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page , TipoMensaje.Error, newEx.Message);
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
            if (Session[sessionKeyClassificationCourseResults] != null)
            {
                CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);

                PageHelper<ClassificationCourseEntity> pageHelper = SearchResults(1);
                PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

                DisplayResults();
            }
        }

        /// <summary>
        /// Handles the grvList data bound event
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Session[Constants.cCulture] != null)
            {
                if (e.Row.Cells.Count <= 1)
                {
                    return;
                }

                CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                if (ci.Name.Equals("en-US"))
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = false;
                }
                else if (ci.Name.Equals("es-CR"))
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = true;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<ClassificationCourseEntity> SearchResults(int page)
        {
            var Filter = new ClassificationCourseEntity
            {
                ClassificationCourseCode = string.IsNullOrWhiteSpace(ClassCodeFilter.Value) ? null : ClassCodeFilter.Value,
                ClassificationCourseDesEn = string.IsNullOrWhiteSpace(ClassDesEnFilter.Value) ? null : ClassDesEnFilter.Value,
                ClassificationCourseDesEs = string.IsNullOrWhiteSpace(ClassDesESFilter.Value) ? null : ClassDesESFilter.Value,
            };

            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<ClassificationCourseEntity> pageHelper = ObjIClassificationCourseBll.ClassificationCourseByFilter(Filter,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, 
                sortExpression, sortDirection, page);

            Session[sessionKeyClassificationCourseResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeyClassificationCourseResults] != null)
            {
                PageHelper<ClassificationCourseEntity> pageHelper = (PageHelper<ClassificationCourseEntity>)Session[sessionKeyClassificationCourseResults];

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