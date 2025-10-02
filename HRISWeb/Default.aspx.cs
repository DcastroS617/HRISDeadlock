using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static HRISWeb.Shared.MensajeriaHelper;
using static System.String;

namespace HRISWeb
{
    public partial class Default : System.Web.UI.Page
    {
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
        /// Handles the Load event of the page.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadUserDivisions();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSelectWorkingDivision control.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        protected void BtnSelectWorkingDivision_Click(object sender, EventArgs e)
        {
            if (cboUserDivisions.SelectedItem != null && !cboUserDivisions.SelectedValue.Equals("0"))
            {
                string divisionCodeStr = cboUserDivisions.SelectedValue;

                int.TryParse(divisionCodeStr, out int divisionCode);
                SaveUserWorkingDivision(divisionCode);
            }

            else
            {
                MostrarMensaje(uppDivisions, TipoMensaje.Validacion, Convert.ToString(GetLocalResourceObject("msjMustSelectADivisionToWork")));
            }
        }

        /// <summary>
        /// Load the dropdown control for the user divisions
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
                    cboUserDivisions.Items.Insert(0, new ListItem(Empty, "0"));

                    if (SessionManager.DoesKeyExist(SessionKey.WorkingDivision))
                    {
                        ListItem liDivision = cboUserDivisions.Items.FindByValue(Convert.ToString(SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode));
                        if (liDivision != null)
                        {
                            liDivision.Selected = true;
                        }
                    }

                    if (SessionManager.GetSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions).Count > 1 && !SessionManager.DoesKeyExist(SessionKey.WorkingDivision))
                    {
                        ScriptManager.RegisterStartupScript(uppDivisions, uppDivisions.GetType(), "executeClientFunction", Format("<script type='text/javascript'>{0}</script>", "OpenWorkingDivisionModal();"), false);
                    }
                }

                else
                {
                    if (SessionManager.DoesUserLoggedIn)
                    {
                        MostrarMensaje(uppDivisions, TipoMensaje.Error, Messages.msjErrorLoadingUserDivisions);
                    }
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException || ex is PresentationException)
                {
                    MostrarMensaje(uppDivisions, TipoMensaje.Error, ex.Message);
                }

                else
                {
                    MostrarMensaje(uppDivisions, TipoMensaje.Error, (new PresentationException(ErrorMessages.msjPresentationExceptionLoadingUserDivisions, ex)).Message);
                }
            }
        }

        /// <summary>
        /// Save in the session state the user working division
        /// </summary>
        /// <param name="divisionCode">The division code</param>
        private void SaveUserWorkingDivision(int divisionCode)
        {
            try
            {
                if (SessionManager.DoesKeyExist(SessionKey.UserDivisions))
                {
                    List<DivisionByUserEntity> userDivisions = SessionManager.GetSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions);
                    if (userDivisions.Exists(d => d.DivisionCode.Equals(divisionCode)))
                    {
                        DivisionByUserEntity workingDivision = userDivisions.Where(d => d.DivisionCode.Equals(divisionCode)).FirstOrDefault();
                        SessionManager.AddSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision, workingDivision);

                        Session.Remove(HrisEnum.ApplicationSessionNames.cSocialResponsabilityCatalogPoliticalDivisions);

                        // Update the working division header label
                        ScriptManager.RegisterStartupScript(uppDivisions, uppDivisions.GetType(), "executeClientFunction", Format("<script type='text/javascript'>CloseWorkingDivisionModal();SetUserWorkingDivision('{0}');</script>", workingDivision.DivisionName), false);
                    }
                }

                else
                {
                    MostrarMensaje(uppDivisions, TipoMensaje.Error, ErrorMessages.msjPresentationExceptionLoadingUserDivisions);
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException || ex is PresentationException)
                {
                    MostrarMensaje(uppDivisions, TipoMensaje.Error, ex.Message);
                }

                else
                {
                    MostrarMensaje(uppDivisions, TipoMensaje.Error, (new PresentationException(ErrorMessages.msjPresentationExceptionSavingUserWorkingDivision, ex)).Message);
                }
            }
        }


        /// <summary>
        /// Check if user is registered, active and has associated divisions to work in the system
        /// </summary>
        /// <returns>True if user is registered and has permissions to access and work in the system, false otherwise</returns>
        [WebMethod(EnableSession = true)]
        public static bool DoesUserHaveWorkingDivision()
        {
            if (SessionManager.DoesUserLoggedIn)
            {
                if (SessionManager.DoesUserHaveWorkingDivision)
                {
                    return true;
                }
            }
            return false;

        }

    }
}