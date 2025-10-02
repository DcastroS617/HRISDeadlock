using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using Unity.Web;
using static System.String;

namespace HRISWeb.Security
{
    public partial class Users : System.Web.UI.Page
    {
        [Dependency]
        public IUsersBll<UserEntity> usersBll { get; set; }
        
        [Dependency]
        public IDivisionsByUsersBll<DivisionByUserEntity> ObjIDivisionsByUsersBll { get; set; }
        
        [Dependency]
        public IActiveDirectoryBll<ActiveDirectorySearchEntity> ObjIActiveDirectoryBll { get; set; }
        
        [Dependency]
        public IDivisionsBll<DivisionEntity> ObjDivisionsBll { get; set; }

        public string KeyUsersADSeraches { get; set; } = "KeyUsersADSeraches";
        public string KeyListUsersAD { get; set; } = "KeyListUsersAD";

        private const string cSessionKeyUserDivisionsResults = "Users-UserDivisionsResults";

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
                    SearchResults(1);
                }

                chkIsActive.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
                chkIsActive.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));
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
        /// Handles the btnAdd click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAdd_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Session.Remove(cSessionKeyUserDivisionsResults);

                hdfUserCodeEdit.Value = Empty;
                txtActiveDirectoryUserAccount.Text = Empty;
                txtUserName.Text = Empty;
                txtEmailAddress.Text = Empty;
                chkIsActive.Checked = true;
                grvDetails.DataSource = null;
                grvDetails.DataBind();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppActions, TipoMensaje.Error, newEx.Message);
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
                    short selectedUserCode = Convert.ToInt16(grvList.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Value);
                    DisplayUserInformation(selectedUserCode);
                    SelectedUserCodeGet.Value = selectedUserCode.ToString();

                    List<DivisionByUserEntity> userDivisions = ObjIDivisionsByUsersBll.ListByUser(selectedUserCode);
                    Session[cSessionKeyUserDivisionsResults] = userDivisions;

                    grvDetails.DataSource = userDivisions.OrderBy(d => d.DivisionName);
                    grvDetails.Sort("DivisionName", SortDirection.Ascending);
                    grvDetails.DataBind();

                    Session.Add(cSessionKeyUserDivisionsResults, userDivisions);

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnEditClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnEditClickPostBack(); }}, 200);", true);
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
        /// Handles the btnAccept click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAccept_ServerClick(object sender, EventArgs e)
        {
            try
            {
                short userCode = !IsNullOrWhiteSpace(hdfUserCodeEdit.Value) ? Convert.ToInt16(hdfUserCodeEdit.Value) : (short)0;
                string activeDirectoryUserAccount = txtActiveDirectoryUserAccount.Text.Trim();
                string userName = txtUserName.Text.Trim();
                string emailAddress = txtEmailAddress.Text.Trim();
                bool isActive = chkIsActive.Checked;
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                if (userCode.Equals(0))
                {
                    usersBll.Add(activeDirectoryUserAccount, userName, emailAddress, isActive, lastModifiedUser);
                    userCode = usersBll.ListByActiveDirectoryAccount(activeDirectoryUserAccount).UserCode;
                }

                else if (Convert.ToInt32(hdfSelectedRowIndex.Value) >= 0)
                {
                    usersBll.Edit(userCode, userName, emailAddress, isActive, lastModifiedUser);
                }

                SaveUserDivisions(userCode);

                SearchResults(PagerUtil.GetActivePage(blstPager));

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "$('.btnAccept').prop('disabled',true); setTimeout(function () {{ ReturnFromBtnAcceptClickPostBack(); }}, 200);", true);
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

                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), Format("ReturnFromBtnAcceptClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnFromBtnAcceptClickPostBackError(); }}, 200);", true);
            }

            finally
            {
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }

        /// <summary>
        /// Handles the btnOpenDialodAddDivision click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnOpenDialodAddDivision_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<DivisionEntity> divisions = ObjDivisionsBll.ListAll();
                if (Session[cSessionKeyUserDivisionsResults] != null)
                {
                    List<DivisionByUserEntity> userDivisions = (List<DivisionByUserEntity>)Session[cSessionKeyUserDivisionsResults];
                    divisions = divisions.Where(d => !userDivisions.Any(u => u.DivisionCode.Equals(d.DivisionCode))).ToList();
                }

                grvAvailableDetails.DataSource = divisions.OrderBy(d => d.DivisionName);
                grvAvailableDetails.DataBind();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(upnAvailableDetails, TipoMensaje.Error, ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(upnAvailableDetails, TipoMensaje.Error, newEx.Message);
                }
            }

            finally
            {

                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }

        /// <summary>
        /// Handles the btnDeleteDivision click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDeleteDivision_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<DivisionByUserEntity> userDivisions = new List<DivisionByUserEntity>();
                if (Session[cSessionKeyUserDivisionsResults] != null)
                {
                    userDivisions = (List<DivisionByUserEntity>)Session[cSessionKeyUserDivisionsResults];
                }

                for (int idx = 0; idx < grvDetails.Rows.Count; idx++)
                {
                    GridViewRow parentRow = grvDetails.Rows[idx];
                    Control ctrlChk = parentRow.FindControl("chkSelectedDetail");

                    if (ctrlChk != null && ctrlChk.GetType() == typeof(CheckBox) && ((CheckBox)ctrlChk).Checked)
                    {
                        int divisionCode = Convert.ToInt16(grvDetails.DataKeys[idx].Values["DivisionCode"]);

                        userDivisions.Remove(userDivisions.Where(d => d.DivisionCode.Equals(divisionCode)).FirstOrDefault());
                    }
                }

                Session[cSessionKeyUserDivisionsResults] = userDivisions;
                grvDetails.DataSource = userDivisions.OrderBy(d => d.DivisionName);
                grvDetails.DataBind();

                ScriptManager.RegisterStartupScript(uppDialogControl, uppDialogControl.GetType(), Format("ReturnFromBtnDeleteClickPostBack{0}", Guid.NewGuid()), "setTimeout(function () { ReturnFromBtnDeleteClickPostBack(); }, 200);", true);
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
        /// Handles the btnAcceptAddNewDivision click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnAcceptAddNewDivision_ServerClick(object sender, EventArgs e)
        {
            try
            {
                List<DivisionByUserEntity> userDivisions = new List<DivisionByUserEntity>();
                if (Session[cSessionKeyUserDivisionsResults] != null)
                {
                    userDivisions = (List<DivisionByUserEntity>)Session[cSessionKeyUserDivisionsResults];
                }

                for (int idx = 0; idx < grvAvailableDetails.Rows.Count; idx++)
                {
                    GridViewRow parentRow = grvAvailableDetails.Rows[idx];
                    Control ctrlChk = parentRow.FindControl("chkSelectedNewDetail");

                    if (ctrlChk != null && ctrlChk.GetType() == typeof(CheckBox) && ((CheckBox)ctrlChk).Checked)
                    {
                        int divisionCode = Convert.ToInt16(grvAvailableDetails.DataKeys[idx].Values["DivisionCode"]);
                        string divisionName = Convert.ToString(grvAvailableDetails.DataKeys[idx].Values["DivisionName"]);
                        string countryId = Convert.ToString(grvAvailableDetails.DataKeys[idx].Values["CountryID"]);

                        userDivisions.Add(new DivisionByUserEntity(divisionCode, divisionName, countryId));
                    }
                }

                Session[cSessionKeyUserDivisionsResults] = userDivisions;
                grvDetails.DataSource = userDivisions.OrderBy(d => d.DivisionName);
                grvDetails.DataBind();

                ScriptManager.RegisterStartupScript(upnAvailableDetails, upnAvailableDetails.GetType(), Format("ReturnFromAcceptAddNewDivisionRequest{0}", Guid.NewGuid()), "setTimeout(function () { ReturnFromAcceptAddNewDivisionRequest(); }, 300);", true);
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
        /// Handles the btnSearchActiveDirectoryUser click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSearchActiveDirectoryUser_ServerClick(object sender, EventArgs e)
        {
            try
            {
                string userName = txtUserNameActiveDirectorySearch.Text.Trim();
                List<ActiveDirectorySearchEntity> result = ObjIActiveDirectoryBll.Search(userName);

                result = result.OrderBy(r => r.UserFullName).ToList();
                List<UserEntity> listaActivos = (List<UserEntity>)Session[KeyListUsersAD];

                listaActivos.AsEnumerable().ToList().ForEach(lt =>
                {
                    result.AsEnumerable().Where(re =>
                        string.Equals(lt.ActiveDirectoryUserAccount.ToUpper(), re.CompleteUserCode.ToUpper())).ToList().ForEach(re => result.Remove(re));
                });

                Session[KeyUsersADSeraches] = result;
                grvActiveDirectoryUsers.DataSource = result;
                grvActiveDirectoryUsers.Sort("CompleteUserCode", SortDirection.Ascending);
                grvActiveDirectoryUsers.DataBind();

                if (result.Count == 0)
                {
                    MensajeriaHelper.MostrarMensaje(uppActiveDirectoryUsersSearch, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msjNoResultsSearchActiveDirectoryUsers")));
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppActiveDirectoryUsersSearch, TipoMensaje.Error, ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msj001.Text")), ex);
                    MensajeriaHelper.MostrarMensaje(uppActiveDirectoryUsersSearch, TipoMensaje.Error, newEx.Message);
                }
            }

            finally
            {
                PagerUtil.SetActivePage(blstPager, PagerUtil.GetActivePage(blstPager));
            }
        }

        /// <summary>
        /// Handles the btnDelete click event        
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnDelete_ServerClick(object sender, EventArgs e)
        {
            try
            {
                var SelectedRow = grvList.DataKeys[int.Parse(hdfSelectedRowIndex.Value)];
                var code = SelectedRow.Value.ToString();

                if (usersBll == null)
                {
                    usersBll = Application.GetContainer().Resolve<IUsersBll<UserEntity>>();
                }

                var result = usersBll.DeleteUser(new UserEntity(short.Parse(code)) { });

                if (result.ErrorNumber == 0)
                {
                    PageHelper<UserEntity> pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));

                    if (PagerUtil.GetActivePage(blstPager) > pageHelper.TotalPages)
                    {
                        PagerUtil.SetActivePage(blstPager, pageHelper.TotalPages);
                        pageHelper = SearchResults(PagerUtil.GetActivePage(blstPager));
                    }

                    DisplayResults(pageHelper);

                    hdfSelectedRowIndex.Value = "-1";

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnDeleteSucess{0}", Guid.NewGuid()), "setTimeout(function () {{ ReturnDeleteSucess(); }}, 200);", true);
                }

                else
                {
                    string msjError = "msjUserDeleteGenericExep";

                    if (result.ErrorNumber.Equals(547) && !result.ErrorMessage.Contains("SecurityAhris.RolUser"))
                    {
                        msjError = "msjUserDeleteExep";
                    }

                    if (result.ErrorNumber.Equals(547) && result.ErrorMessage.Contains("SecurityAhris.RolUser"))
                    {
                        msjError = "msjUserDeleteExepAhris";
                    }

                    ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), String.Format("ReturnDeleteFailed{0}", Guid.NewGuid()), "setTimeout(function () {{ReturnFromBtnDeleteFailClickPostBack('" + Convert.ToString(GetLocalResourceObject(msjError)) + "');}}, 100);", true);
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
        /// Handles the btnSearch click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSearch_ServerClick(object sender, EventArgs e)
        {
            try
            {
                PageHelper<UserEntity> pageHelper = SearchResults(1);
                CommonFunctions.ResetSortDirection(Page.ClientID, grvList.ClientID);
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
        /// Handles the blstPager click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BlstPager_Click(object sender, BulletedListEventArgs e)
        {
            try
            {
                if (!IsNullOrEmpty(((BulletedList)sender).Items[e.Index].Value))
                {
                    PagerUtil.SetActivePage(blstPager, Convert.ToInt32(((BulletedList)sender).Items[e.Index].Value));
                    SearchResults(PagerUtil.GetActivePage(blstPager));
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
        /// Handles the grvAvailableDetails pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvAvailableDetails_PreRender(object sender, EventArgs e)
        {
            if ((grvAvailableDetails.ShowHeader && grvAvailableDetails.Rows.Count > 0) || (grvAvailableDetails.ShowHeaderWhenEmpty))
            {
                grvAvailableDetails.UseAccessibleHeader = true;
                //Force GridView to use <thead> instead of <tbody>
                grvAvailableDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvAvailableDetails.ShowFooter && grvAvailableDetails.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvAvailableDetails.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvDetails pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvDetails_PreRender(object sender, EventArgs e)
        {
            if ((grvDetails.ShowHeader && grvDetails.Rows.Count > 0) || (grvDetails.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvDetails.ShowFooter && grvDetails.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvDetails.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvActiveDirectoryUsers pre render event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvActiveDirectoryUsers_PreRender(object sender, EventArgs e)
        {
            if ((grvDetails.ShowHeader && grvDetails.Rows.Count > 0) || (grvDetails.ShowHeaderWhenEmpty))
            {
                //Force GridView to use <thead> instead of <tbody>
                grvDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (grvDetails.ShowFooter && grvDetails.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody>
                grvDetails.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        /// <summary>
        /// Handles the grvList sorting Division event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvList_SortingDivision(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvDetails.ClientID, e.SortExpression);
            List<DivisionByUserEntity> userDivisions = Session[cSessionKeyUserDivisionsResults] as List<DivisionByUserEntity>;
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvDetails.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvDetails.ClientID);

            grvDetails.DataSource = sortDirection.ToLower() == "asc" ? userDivisions.OrderBy(d => d.DivisionName) : userDivisions.OrderByDescending(d => d.DivisionName);
            grvDetails.DataBind();

            btnSearchUser.Disabled = true;
        }

        /// <summary>
        /// Handles the ActiveDirectoryUsersSort sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvActiveDirectoryUsersSort(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvActiveDirectoryUsers.ClientID, e.SortExpression);
            var SortDirectionGet = CommonFunctions.GetSortDirection(Page.ClientID, grvActiveDirectoryUsers.ClientID);
            var SortExpressionGet = e.SortExpression;

            var DetailList = Session[KeyUsersADSeraches] as List<ActiveDirectorySearchEntity>;

            if (SortDirectionGet.ToLower().Equals("asc"))
            {
                DetailList = DetailList.OrderByDescending(o => o.CompleteUserCode).ToList();

                if (SortExpressionGet.Equals("UserFullName"))
                {
                    DetailList = DetailList.OrderBy(o => o.UserFullName).ToList();
                }

                if (SortExpressionGet.Equals("EmailAddress"))
                {
                    DetailList = DetailList.OrderBy(o => o.EmailAddress).ToList();
                }
            }

            if (!SortDirectionGet.ToLower().Equals("asc"))
            {
                DetailList = DetailList.OrderByDescending(o => o.CompleteUserCode).ToList();

                if (SortExpressionGet.Equals("UserFullName"))
                {
                    DetailList = DetailList.OrderByDescending(o => o.UserFullName).ToList();
                }

                if (SortExpressionGet.Equals("EmailAddress"))
                {
                    DetailList = DetailList.OrderByDescending(o => o.EmailAddress).ToList();
                }
            }

            grvActiveDirectoryUsers.DataSource = DetailList;
            grvActiveDirectoryUsers.DataBind();
        }

        /// <summary>
        /// Handles the grvList sorting event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void GrvList_Sorting(object sender, GridViewSortEventArgs e)
        {
            CommonFunctions.SwitchSetSortDirection(Page.ClientID, grvList.ClientID, e.SortExpression);
            SearchResults(PagerUtil.GetActivePage(blstPager));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Search for the results 
        /// </summary>
        /// <param name="page">Results page</param>
        /// <returns>Results</returns>
        private PageHelper<UserEntity> SearchResults(int page)
        {
            string userName = IsNullOrWhiteSpace(txtUserNameFilter.Text.Trim()) ? null : txtUserNameFilter.Text.Trim();
            string sortExpression = CommonFunctions.GetSortExpression(Page.ClientID, grvList.ClientID);
            string sortDirection = CommonFunctions.GetSortDirection(Page.ClientID, grvList.ClientID);

            if (usersBll == null)
            {
                usersBll = Application.GetContainer().Resolve<IUsersBll<UserEntity>>();
            }

            PageHelper<UserEntity> pageHelper = usersBll.ListByFilters(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                userName,
                sortExpression,
                sortDirection,
                page);

            Session[KeyListUsersAD] = pageHelper.ResultList;
            PagerUtil.SetupPager(blstPager, pageHelper.TotalPages, pageHelper.CurrentPage);
            
            DisplayResults(pageHelper);

            return pageHelper;
        }

        /// <summary>
        /// Set the configuration for displaying the results
        /// </summary>
        /// <param name="pageHelper">The page helper that contains result information</param>
        private void DisplayResults(PageHelper<UserEntity> pageHelper)
        {
            grvList.PageIndex = PagerUtil.GetActivePage(blstPager) - 1;
            grvList.DataSource = pageHelper.ResultList;
            grvList.DataBind();

            htmlResultsSubtitle.InnerHtml = Format(Convert.ToString(GetLocalResourceObject("lblResultsSubtitleWithInformation")), pageHelper.ResultList.Count, pageHelper.TotalResults, pageHelper.TotalPages);
        }

        /// <summary>
        /// Displays on modal the user information
        /// </summary>
        /// <param name="userCode">The user code to search</param>
        private void DisplayUserInformation(short userCode)
        {
            UserEntity user = usersBll.ListByUserCode(userCode);

            hdfUserCodeEdit.Value = Convert.ToString(user.UserCode);
            txtActiveDirectoryUserAccount.Text = user.ActiveDirectoryUserAccount;
            txtUserName.Text = user.UserName;
            txtEmailAddress.Text = user.EmailAddress;
            chkIsActive.Checked = user.IsActive;
        }

        /// <summary>
        /// Save the user assigned divisions
        /// </summary>
        /// <param name="userCode">The user code</param>
        private void SaveUserDivisions(short userCode)
        {
            if (Session[cSessionKeyUserDivisionsResults] != null)
            {
                List<DivisionByUserEntity> userDivisionsTemp = (List<DivisionByUserEntity>)Session[cSessionKeyUserDivisionsResults];
                List<DivisionByUserEntity> userDivisions = ObjIDivisionsByUsersBll.ListByUser(userCode);
                string lastModifiedUser = UserHelper.GetCurrentFullUserName;

                foreach (DivisionByUserEntity item in userDivisionsTemp.Where(t => !userDivisions.Any(d => d.DivisionCode.Equals(t.DivisionCode))))
                {
                    ObjIDivisionsByUsersBll.Add(userCode, item.DivisionCode, lastModifiedUser);
                }

                foreach (DivisionByUserEntity item in userDivisions.Where(t => !userDivisionsTemp.Any(d => d.DivisionCode.Equals(t.DivisionCode))))
                {
                    ObjIDivisionsByUsersBll.Delete(userCode, item.DivisionCode);
                }

                if (SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation).UserCode.Equals(userCode))
                {
                    SessionManager.AddSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions, userDivisionsTemp);
                }
            }
        }

        #endregion

    }
}