using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Entity;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity.Attributes;

namespace HRISWeb.Security
{
    public partial class Privileges : System.Web.UI.Page
    {
        [Dependency]
        public IProfileBll<ProfileEntity> ObjProfileBll { get; set; }

        [Dependency]
        public IDivisionsBll<DivisionEntity> ObjDivisionsBll { get; set; }

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
        /// Handles the pre render of the page
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void Page_PreRender(object sender, EventArgs e)
        { 
            LoadProfiles();
        }
         
        /// <summary>
        /// Handles the click of the Save button on the Add/Edit user dialog
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (IsValidSave())
            {
                byte profileID = Convert.ToByte(txtProfileID.Text);
                int divisionCode = Convert.ToInt32(cboDivisions.SelectedValue);
                string profileDescription = txtProfileDescription.Text.Trim();

                try
                {
                    string userID = UserHelper.GetCurrentUserName();
                    string domain = UserHelper.GetCurrentUserDomain();

                    if (IsAddAction)
                    {
                        ObjProfileBll.Add(userID, 
                            domain, 
                            divisionCode, 
                            profileDescription, 
                            chkAddAllUser.Checked, 
                            UserHelper.GetCurrentFullUserName);
                    }

                    else
                    {
                        ObjProfileBll.Update(profileID, divisionCode, profileDescription, UserHelper.GetCurrentFullUserName);
                    } 

                    LoadProfiles();
                    CloseDialog();
                }

                catch (Exception ex)
                {
                    if (ex is DataAccessException
                        || ex is BusinessException
                        || ex is PresentationException)
                    {
                        MensajeriaHelper.MostrarMensaje(uppControls
                            , TipoMensaje.Error
                            , ex.Message);
                    }

                    else
                    {
                        PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msjPresentationExceptionProfileSave")), ex);
                        MensajeriaHelper.MostrarMensaje(uppControls
                            , TipoMensaje.Error
                            , newEx.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the click of the Delete button on the profiles screen
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            { 
                byte profileId = Convert.ToByte(grvProfile.DataKeys[Convert.ToInt32(hdfSelectedRowIndex.Value)].Values["ProfileID"]);
                string userName = UserHelper.GetCurrentUserName();
                string domain = UserHelper.GetCurrentUserDomain();

                ObjProfileBll.Delete(profileId);

                LoadProfiles();

                ScriptManager.RegisterStartupScript(uppControls, uppControls.GetType(), "informDeleteUser", string.Format("<script type='text/javascript'>MostrarMensaje({0},'{1}',null);</script>", Convert.ToByte(TipoMensaje.Validacion), Convert.ToString(GetLocalResourceObject("msjDeleteProfileSuccessful"))), false);
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppControls
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msjPresentationExceptionProfilesDelete")), ex);
                    MensajeriaHelper.MostrarMensaje(uppControls
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the page change on the groups grid
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BlstPager_Click(object sender, System.Web.UI.WebControls.BulletedListEventArgs e)
        {
            if (!string.IsNullOrEmpty(((BulletedList)sender).Items[e.Index].Value))
            {
                PagerUtil.SetActivePage(blstPager, Convert.ToInt32(((BulletedList)sender).Items[e.Index].Value));
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Load data grid with the registered profiles by user
        /// </summary>
        private void LoadProfiles()
        {
            try
            {
                string userName = UserHelper.GetCurrentUserName();
                string domain = UserHelper.GetCurrentUserDomain();

                PageHelper<ProfileEntity> helper = new PageHelper<ProfileEntity>
                {
                    CurrentPage = PagerUtil.GetActivePage(blstPager),
                    PageSize = 20
                };

                List<ProfileEntity> lstResults = new List<ProfileEntity>();
                 
                lstResults = ObjProfileBll.ListAll();
                
                int pageIndex = lstResults.Count / helper.PageSize;
                if (lstResults.Count % helper.PageSize != 0)
                {
                    pageIndex++;
                }

                helper.TotalPages = pageIndex;
                PagerUtil.SetupPager(blstPager, helper.TotalPages, helper.CurrentPage);

                grvProfile.PageIndex = helper.CurrentPage - 1;
                grvProfile.DataSource = lstResults;
                grvProfile.DataBind();            
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException
                    || ex is BusinessException
                    || ex is PresentationException)
                {
                    MensajeriaHelper.MostrarMensaje(uppControls
                      , TipoMensaje.Error
                      , ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(Convert.ToString(GetLocalResourceObject("msjPresentationExceptionProfilesListAll")), ex);
                    MensajeriaHelper.MostrarMensaje(uppControls
                      , TipoMensaje.Error
                      , newEx.Message);
                }
            }
        }

        /// <summary>
        /// Determines if the save action is valid
        /// </summary>
        /// <returns>true in case sava action is valid, false otherwise</returns>
        private bool IsValidSave()
        {
            if (cboDivisions.SelectedIndex <= 0)
            {              
                MensajeriaHelper.MostrarMensaje(uppControls, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msjProfileDescriptionInvalid")));
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProfileDescription.Text))
            {
                MensajeriaHelper.MostrarMensaje(uppControls, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msjProfileDescriptionInvalid")));
                return false;
            }
           
            return true;
        }

        /// <summary>
        /// Determines whether the action that is being made is to add a new profile or not, true in case is add action, false otherwise
        /// </summary>
        private bool IsAddAction
        {
            get
            {
                if (txtProfileID.Text.Trim().Equals("0", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Close the Add/Edit profile dialog
        /// </summary>
        private void CloseDialog()
        {
            ScriptManager.RegisterStartupScript(uppControls, uppControls.GetType(), "CloseUserDialog", string.Format("<script type='text/javascript'>CloseUserDialog();MostrarMensaje({0},'{1}',null);</script>", Convert.ToByte(TipoMensaje.Validacion), Convert.ToString(GetLocalResourceObject("msjSaveProfileSuccessful"))), false);
        }

        #endregion
    }
}