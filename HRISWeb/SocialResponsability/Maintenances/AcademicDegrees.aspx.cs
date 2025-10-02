using HRISWeb.Shared;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using System;
using System.Web.UI;
using System.Globalization;
using System.Threading;
using System.Web.UI.WebControls;
using DOLE.HRIS.Application.Business.Interfaces;
using Unity.Attributes;
using DOLE.HRIS.Shared.Entity;
using System.Collections.Generic;
using Unity;
using Unity.Web;
using System.Linq;
using static System.String;

namespace HRISWeb.SocialResponsability.Maintenances
{
    public partial class AcademicDegrees : System.Web.UI.Page
    {
        [Dependency]
        protected IAcademicDegreesBll<AcademicDegreeEntity> objAcademicDegreeBll { get; set; }
        [Dependency]
        protected IDegreeFormationTypeBll<DegreeFormationTypeEntity> objDegreeFormationTypeBll { get; set; }
        [Dependency]
        protected IYearAcademicDegreesBLL<YearAcademicDegreesEntity> objYearAcademicDegreesBll { get; set; }

        //session key for the results
        readonly string sessionKeySocialResponsabilityResults = "SocialResponsability-AcademicDegreeResults";

        public string CultureGlobal { get; set; }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BtnSearch_ServerClick(sender, e);
                    LoadDegreeFormationType();
                }

                if (Session[Constants.cCulture] != null)
                {
                    CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                    CultureGlobal = ci.Name;
                }


                chkSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chkSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));

                //activate the pager
                if (Session[sessionKeySocialResponsabilityResults] != null)
                {
                    PageHelper<AcademicDegreeEntity> pageHelper = (PageHelper<AcademicDegreeEntity>)Session[sessionKeySocialResponsabilityResults];
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


        #region Events

        /// <summary>
        /// Load the DegreeFormationType
        /// </summary>
        private void LoadDegreeFormationType()
        {
            try
            {
                List<DegreeFormationTypeEntity> degreeFormationTypeList;

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDegreeFormationType] == null)
                {
                    objDegreeFormationTypeBll = objDegreeFormationTypeBll ?? Application.GetContainer().Resolve<IDegreeFormationTypeBll<DegreeFormationTypeEntity>>();
                    degreeFormationTypeList = objDegreeFormationTypeBll.ListEnabled();
                    Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDegreeFormationType] = degreeFormationTypeList;
                }

                degreeFormationTypeList = (List<DegreeFormationTypeEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogDegreeFormationType];

                CultureInfo currentCulture = GetCurrentCulture();
                cboDegreeFormationType.Items.Clear();
                cboDegreeFormationType.Items.Add(new ListItem(Empty, "-1"));

                cboDegreeFormationType.Items.AddRange(degreeFormationTypeList.Select(g => new ListItem(currentCulture.Name.Equals(Constants.cCultureEsCR)
                        ? g.DegreeFormationTypeDescriptionSpanish : g.DegreeFormationTypeDescriptionEnglish
                        , g.DegreeFormationTypeCode.ToString())).ToArray());


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
        /// Load the LoadYearDegrees
        /// </summary>
        private void LoadYearDegrees(int AcademicDegree)
        {
            try
            {
                var listaAnios = GetsStudyYears(AcademicDegree);
                grvYearCoursing.DataSource = listaAnios.Where(x => x.Coursing == true);
                grvYearCoursing.DataBind();

                grvNotCoursingYear.DataSource = listaAnios.Where(y => y.Coursing == false);
                grvNotCoursingYear.DataBind();

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
        /// Gets the current culture selected by the user
        /// </summary>
        /// <returns>The current cultture</returns>
        private CultureInfo GetCurrentCulture()
        {
            if (Session[Constants.cCulture] != null)
            {
                return new CultureInfo(Convert.ToString(Session[Constants.cCulture]));
            }
            return new CultureInfo(Constants.cCultureEsCR);
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
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj000.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, newEx.Message);
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
                byte hiddenAcademicDegreeCode = !string.IsNullOrWhiteSpace(hdfAcademicDegreeCodeEdit.Value) ?
                    Convert.ToByte(hdfAcademicDegreeCodeEdit.Value) : (byte)0;

                string AcademicDegreeNameSpanish = txtAcademicDegreeNameSpanish.Text.Trim();
                string AcademicDegreeNameEnglish = txtAcademicDegreeNameEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;
                int orderList = Convert.ToInt32(txtOrderList.Text);
                byte degreeFormationType = Convert.ToByte(cboDegreeFormationType.SelectedValue);

                //activate the deleted item
                if (chkActivateDeleted.Checked)
                {
                    objAcademicDegreeBll.Activate(new AcademicDegreeEntity(hiddenAcademicDegreeCode, lastModifiedUser));
                }

                //update and activate the deleted item
                else
                {
                    objAcademicDegreeBll.Edit(new AcademicDegreeEntity(hiddenAcademicDegreeCode
                        , AcademicDegreeNameSpanish
                        , AcademicDegreeNameEnglish
                        , searchEnable
                        , deleted
                        , lastModifiedUser
                        , orderList
                        , degreeFormationType));
                }

                SearchResults(PagerUtil.GetActivePage(blstPager));
                DisplayResults();

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptActivateDeletedClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptActivateDeletedClickPostBack(); }}, 200);", true);

                hdfSelectedRowIndex.Value = "-1";

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogAcademicDegrees] != null)
                {
                    List<AcademicDegreeEntity> AcademicDegreesBDList = objAcademicDegreeBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogAcademicDegrees, AcademicDegreesBDList);
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
                short hiddenAcademicDegreeCode = !string.IsNullOrWhiteSpace(hdfAcademicDegreeCodeEdit.Value) ?
                    Convert.ToInt16(hdfAcademicDegreeCodeEdit.Value) : (short)-1;

                string AcademicDegreeNameSpanish = txtAcademicDegreeNameSpanish.Text.Trim();
                string AcademicDegreeNameEnglish = txtAcademicDegreeNameEnglish.Text.Trim();
                bool searchEnable = chkSearchEnabled.Checked;
                bool deleted = false;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;
                int orderList = Convert.ToInt32(txtOrderList.Text);
                byte degreeFormationType = Convert.ToByte(cboDegreeFormationType.SelectedValue);

                if (hiddenAcademicDegreeCode.Equals(-1))
                {
                    Tuple<bool, AcademicDegreeEntity> addResult = objAcademicDegreeBll.Add(new AcademicDegreeEntity(AcademicDegreeNameSpanish
                        , AcademicDegreeNameEnglish
                        , searchEnable
                        , deleted
                        , lastModifiedUser
                        , orderList
                        , degreeFormationType));

                    hiddenAcademicDegreeCode = addResult.Item2.AcademicDegreeCode;
                    hdfAcademicDegreeCodeEdit.Value = addResult.Item2.AcademicDegreeCode.ToString();

                    if (addResult.Item1)
                    {
                        SearchResults(PagerUtil.GetActivePage(blstPager));
                        DisplayResults();

                        hdfSelectedRowIndex.Value = "0";
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                    }

                    else if (!addResult.Item1)
                    {
                        AcademicDegreeEntity previousEntity = addResult.Item2;
                        hdfAcademicDegreeCodeEdit.Value = "-1";
                        if (previousEntity.Deleted)
                        {
                            hdfAcademicDegreeCodeEdit.Value = Convert.ToString(hiddenAcademicDegreeCode);
                            txtActivateDeletedAcademicDegreeNameSpanish.Text = previousEntity.AcademicDegreeDescriptionSpanish;
                            txtActivateDeletedAcademicDegreeNameEnglish.Text = previousEntity.AcademicDegreeDescriptionEnglish;

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted();", true);
                        }

                        else
                        {
                            txtDuplicatedAcademicDegreeNameSpanish.Text = previousEntity.AcademicDegreeDescriptionSpanish;
                            txtDuplicatedAcademicDegreeNameEnglish.Text = previousEntity.AcademicDegreeDescriptionEnglish;
                            pnlDuplicatedDialogDataDetail.Visible = true;
                            divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                        }
                    }
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    var academicEntity = new AcademicDegreeEntity(Convert.ToByte(hiddenAcademicDegreeCode)
                          , AcademicDegreeNameSpanish
                          , AcademicDegreeNameEnglish
                          , searchEnable
                          , deleted
                          , lastModifiedUser
                          , orderList
                          , degreeFormationType);

                    var entityFilter = objAcademicDegreeBll.ListByNames(academicEntity.AcademicDegreeDescriptionSpanish, academicEntity.AcademicDegreeDescriptionEnglish);

                    if (entityFilter == null || entityFilter?.AcademicDegreeCode == academicEntity.AcademicDegreeCode)
                    {

                        var result = objAcademicDegreeBll.Edit(academicEntity);

                        if (result.Item1)
                        {
                            SearchResults(PagerUtil.GetActivePage(blstPager));
                            DisplayResults();

                            ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "ReturnFromBtnAcceptClickPostBack(); ", true);
                        }

                        else
                        {
                            AcademicDegreeEntity previousEntity = result.Item2;
                            hdfAcademicDegreeCodeEdit.Value = "-1";

                            if (previousEntity.Deleted)
                            {
                                hdfAcademicDegreeCodeEdit.Value = Convert.ToString(previousEntity.AcademicDegreeCode);
                                txtActivateDeletedAcademicDegreeNameSpanish.Text = previousEntity.AcademicDegreeDescriptionSpanish;
                                txtActivateDeletedAcademicDegreeNameEnglish.Text = previousEntity.AcademicDegreeDescriptionEnglish;

                                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDeleted{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDeleted(); ", true);
                            }

                            else
                            {
                                txtDuplicatedAcademicDegreeNameSpanish.Text = previousEntity.AcademicDegreeDescriptionSpanish;
                                txtDuplicatedAcademicDegreeNameEnglish.Text = previousEntity.AcademicDegreeDescriptionEnglish;
                                pnlDuplicatedDialogDataDetail.Visible = true;
                                divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));

                                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);
                            }
                        }
                    }
                    else
                    {
                        txtDuplicatedAcademicDegreeNameSpanish.Text = entityFilter.AcademicDegreeDescriptionSpanish;
                        txtDuplicatedAcademicDegreeNameEnglish.Text = entityFilter.AcademicDegreeDescriptionEnglish;
                        pnlDuplicatedDialogDataDetail.Visible = true;
                        divDuplicatedDialogText.InnerHtml = Convert.ToString(GetLocalResourceObject("lblTextDuplicatedDialog"));
                        ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnAcceptClickPostBackDuplicated{0}", Guid.NewGuid()), " ReturnFromBtnAcceptClickPostBackDuplicated();", true);

                    }
                }

                if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogAcademicDegrees] != null)
                {
                    List<AcademicDegreeEntity> AcademicDegreesBDList = objAcademicDegreeBll.ListEnabled();
                    Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogAcademicDegrees, AcademicDegreesBDList);
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
        /// Handles the BtnSaveYears_ServerClick click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSaveYears_ServerClick(object sender, EventArgs e)
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
        /// Handles the BtnAddYear_ServerClick click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddCoursingYear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtAcademicCoursingYearDialog.Text))
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjYearEmpty")));
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteYearEmpty_ServerClick{0}", Guid.NewGuid()), "setTimeout(function () {{ FocusYear(); }}, 200);", true);
                }
                else
                {
                    int academicDegreeCode = Convert.ToInt32(hdfAcademicDegreeCodeYearCoursing.Value);
                    int year = Convert.ToInt32(txtAcademicCoursingYearDialog.Text);
                    bool coursing = chkCoursing.Checked;
                    bool readAndWrite = chkReadWrite.Checked;

                    YearAcademicDegreesEntity yearObject = new YearAcademicDegreesEntity(Convert.ToByte(academicDegreeCode), year,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, coursing, readAndWrite, UserHelper.GetCurrentFullUserName);

                    var listaAnios = GetsStudyYears(academicDegreeCode);

                    var existing = listaAnios.Any(l => l.AcademicYear == yearObject.AcademicYear && l.Coursing == yearObject.Coursing);

                    if (existing)
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjYearErrorExist")));
                    }
                    else
                    {
                        objYearAcademicDegreesBll.Add(yearObject);
                        LoadYearDegrees(academicDegreeCode);
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
        /// Handles the BtnSaveYears_ServerClick click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDeleteCoursingYear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtAcademicCoursingYearDialog.Text))
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjYearEmpty")));
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteYearEmpty_ServerClick{0}", Guid.NewGuid()), "setTimeout(function () {{ FocusYear(); }}, 200);", true);
                }
                else
                {
                    int academicDegreeCode = Convert.ToInt32(hdfAcademicDegreeCodeYearCoursing.Value);
                    int year = Convert.ToInt32(txtAcademicCoursingYearDialog.Text);
                    bool coursing = chkCoursing.Checked;

                    YearAcademicDegreesEntity yearObject = new YearAcademicDegreesEntity(Convert.ToByte(academicDegreeCode), year,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, coursing, UserHelper.GetCurrentFullUserName);

                    var listaAnios = GetsStudyYears(academicDegreeCode);
                    var existing = listaAnios.Any(l => l.AcademicYear == yearObject.AcademicYear && l.Coursing == yearObject.Coursing);

                    if (existing)
                    {
                        objYearAcademicDegreesBll.Delete(yearObject);
                        LoadYearDegrees(academicDegreeCode);
                    }
                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjYearErrorDeleteNotExist")));
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
        /// Handles the BtnAddYear_ServerClick click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAddNotCoursingYear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtAcademicNotCoursingYearDialog.Text))
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjYearEmpty")));
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteYearEmpty_ServerClick{0}", Guid.NewGuid()), "setTimeout(function () {{ FocusYear(); }}, 200);", true);
                }
                else
                {
                    int academicDegreeCode = Convert.ToInt32(hdfAcademicDegreeCodeYearNotCoursing.Value);
                    int year = Convert.ToInt32(txtAcademicNotCoursingYearDialog.Text);
                    bool coursing = chkNotCursing.Checked;
                    bool readAndWrite = chkReadWriteNotCoursing.Checked;

                    YearAcademicDegreesEntity yearObject = new YearAcademicDegreesEntity(Convert.ToByte(academicDegreeCode), year,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, coursing, readAndWrite, UserHelper.GetCurrentFullUserName);

                    var listaAnios = GetsStudyYears(academicDegreeCode);

                    var existing = listaAnios.Any(l => l.AcademicYear == yearObject.AcademicYear && l.Coursing == yearObject.Coursing);

                    if (existing)
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjYearErrorExist")));
                    }
                    else
                    {
                        objYearAcademicDegreesBll.Add(yearObject);
                        LoadYearDegrees(academicDegreeCode);
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
        /// Handles the BtnSaveYears_ServerClick click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDeleteNotCoursingYear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtAcademicNotCoursingYearDialog.Text))
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjYearEmpty")));
                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteYearEmpty_ServerClick{0}", Guid.NewGuid()), "setTimeout(function () {{ FocusYear(); }}, 200);", true);
                }
                else
                {
                    int academicDegreeCode = Convert.ToInt32(hdfAcademicDegreeCodeYearNotCoursing.Value);
                    int year = Convert.ToInt32(txtAcademicNotCoursingYearDialog.Text);
                    bool coursing = chkNotCursing.Checked;

                    YearAcademicDegreesEntity yearObject = new YearAcademicDegreesEntity(Convert.ToByte(academicDegreeCode), year,
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode, coursing, UserHelper.GetCurrentFullUserName);

                    var listaAnios = GetsStudyYears(academicDegreeCode);
                    var existing = listaAnios.Any(l => l.AcademicYear == yearObject.AcademicYear && l.Coursing == yearObject.Coursing);

                    if (existing)
                    {
                        objYearAcademicDegreesBll.Delete(yearObject);
                        LoadYearDegrees(academicDegreeCode);
                    }
                    else
                    {
                        MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, Convert.ToString(GetLocalResourceObject("msjYearErrorDeleteNotExist")));
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
                    byte selectedAcademicDegreeCode = Convert.ToByte(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    AcademicDegreeEntity entity = objAcademicDegreeBll.ListByKey(selectedAcademicDegreeCode);

                    hdfAcademicDegreeCodeEdit.Value = selectedAcademicDegreeCode.ToString();
                    txtAcademicDegreeNameSpanish.Text = entity.AcademicDegreeDescriptionSpanish;
                    txtAcademicDegreeNameEnglish.Text = entity.AcademicDegreeDescriptionEnglish;
                    txtOrderList.Text = entity.Orderlist.ToString();
                    cboDegreeFormationType.SelectedIndex = -1;

                    ListItem liDegreeFormationCode = cboDegreeFormationType.Items.FindByValue(entity.DegreeFormationTypeCode.ToString());
                    if (liDegreeFormationCode != null)
                    {
                        liDegreeFormationCode.Selected = true;
                    }
                    chkSearchEnabled.Checked = entity.SearchEnabled;

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
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
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEditCoursingYear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    byte selectedAcademicDegreeCode = Convert.ToByte(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    AcademicDegreeEntity entity = objAcademicDegreeBll.ListByKey(selectedAcademicDegreeCode);

                    hdfAcademicDegreeCodeYearCoursing.Value = selectedAcademicDegreeCode.ToString();
                    txtAcademicDegreeNameEs.Text = entity.AcademicDegreeDescriptionSpanish;
                    txtAcademicDegreeNameEn.Text = entity.AcademicDegreeDescriptionEnglish;
                    chkSearchEnabled.Checked = entity.SearchEnabled;

                    LoadYearDegrees(Convert.ToInt32(selectedAcademicDegreeCode));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditCoursingYearClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditCoursingYearClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
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
        /// Handles the btnEdit click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnEditNotCoursingYear_ServerClick(object sender, EventArgs e)
        {
            try
            {
                if (hdfSelectedRowIndex.Value != "-1")
                {
                    byte selectedAcademicDegreeCode = Convert.ToByte(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);

                    AcademicDegreeEntity entity = objAcademicDegreeBll.ListByKey(selectedAcademicDegreeCode);

                    hdfAcademicDegreeCodeYearNotCoursing.Value = selectedAcademicDegreeCode.ToString();
                    txtAcademicDegreeNameEsNotCoursing.Text = entity.AcademicDegreeDescriptionSpanish;
                    txtAcademicDegreeNameEnNotCoursing.Text = entity.AcademicDegreeDescriptionEnglish;
                    chkSearchEnabled.Checked = entity.SearchEnabled;

                    LoadYearDegrees(Convert.ToInt32(selectedAcademicDegreeCode));

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnEditNotCoursingYearClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditNotCoursingYearClickPostBack(); }}, 200);", true);
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
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
                    byte selectedAcademicDegreeCode = Convert.ToByte(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    objAcademicDegreeBll.Delete(new AcademicDegreeEntity(selectedAcademicDegreeCode, UserHelper.GetCurrentFullUserName));

                    PageHelper<AcademicDegreeEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
                    if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                    {
                        PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                        pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
                    }

                    DisplayResults();

                    //aquí realizamos la deselección del borrado
                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnDeleteClickPostBack(); }}, 200);", true);

                    //
                    if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogAcademicDegrees] != null)
                    {
                        List<AcademicDegreeEntity> AcademicDegreesBDList = objAcademicDegreeBll.ListEnabled();
                        Application.Add(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogAcademicDegrees, AcademicDegreesBDList);
                    }
                }

                else
                {
                    MensajeriaHelper.MostrarMensaje(Page, TipoMensaje.Error, GetLocalResourceObject("msgInvalidSelection").ToString());
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
        /// Handles the GrvYearCoursing pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvYearCoursing_PreRender(object sender, EventArgs e)
        {
            if ((grvYearCoursing.ShowHeader && grvYearCoursing.Rows.Count > 0) || (grvYearCoursing.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvYearCoursing.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvYearCoursing.ShowFooter && grvYearCoursing.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvYearCoursing.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        /// <summary>
        /// Handles the grvYearNotCoursing pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvNotCoursingYear_PreRender(object sender, EventArgs e)
        {
            if ((grvNotCoursingYear.ShowHeader && grvNotCoursingYear.Rows.Count > 0) || (grvNotCoursingYear.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvNotCoursingYear.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvNotCoursingYear.ShowFooter && grvNotCoursingYear.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvNotCoursingYear.FooterRow.TableSection = TableRowSection.TableFooter;
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

            PageHelper<AcademicDegreeEntity> pageHelper = SearchResults(1);
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);

            DisplayResults();
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

        #region Private methods

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<AcademicDegreeEntity> SearchResults(int page)
        {
            string AcademicDegreeNameSpanish = string.IsNullOrWhiteSpace(txtPrincipalAcademicDegreeDescriptionSpanishFilter.Text.Trim()) ? null : txtPrincipalAcademicDegreeDescriptionSpanishFilter.Text.Trim();
            string AcademicDegreeNameEnglish = string.IsNullOrWhiteSpace(txtPrincipalAcademicDegreeDescriptionEnglishFilter.Text.Trim()) ? null : txtPrincipalAcademicDegreeDescriptionEnglishFilter.Text.Trim();
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            PageHelper<AcademicDegreeEntity> pageHelper = objAcademicDegreeBll.ListByFilters(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode
                , AcademicDegreeNameSpanish
                , AcademicDegreeNameEnglish
                , sortExpression
                , sortDirection
                , page);

            Session[sessionKeySocialResponsabilityResults] = pageHelper;
            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        private void DisplayResults()
        {
            if (Session[sessionKeySocialResponsabilityResults] != null)
            {
                PageHelper<AcademicDegreeEntity> pageHelper = (PageHelper<AcademicDegreeEntity>)Session[sessionKeySocialResponsabilityResults];

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
        /// <summary>
        /// Gets the study years
        /// </summary>
        /// <returns>The study years</returns>
        private List<YearAcademicDegreesEntity> GetsStudyYears(int academicDegree)
        {

            List<YearAcademicDegreesEntity> yearAcademicBDlist;

            if (Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogYearAcademicDegrees] != null)
            {
                yearAcademicBDlist = (List<YearAcademicDegreesEntity>)Application[HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogYearAcademicDegrees];
            }
            else
            {
                objYearAcademicDegreesBll = objYearAcademicDegreesBll ?? Application.GetContainer().Resolve<IYearAcademicDegreesBLL<YearAcademicDegreesEntity>>();
                yearAcademicBDlist = objYearAcademicDegreesBll.ListAll();
            }
            return yearAcademicBDlist.Where(x => x.AcademicDegreeCode == academicDegree && x.DivisionCode == SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode).ToList();
        }

        #endregion
    }
}