using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using Unity.Attributes;
using Unity.Web;
using static HRISWeb.Shared.MensajeriaHelper;
using static HRISWeb.Shared.UserHelper;
using static System.String;

namespace HRISWeb
{
    /// <summary>
    /// Master page class
    /// </summary>
    public partial class MainCard : System.Web.UI.MasterPage
    {
        [Dependency]
        public IActiveDirectoryBll<ActiveDirectorySearchEntity> ObjIActiveDirectoryBll { get; set; }

        [Dependency]
        protected IEmployeesBll<EmployeeEntity> ObjEmployeesBll { get; set; }

        private const string pruebasDeUsuario = "PruebasDeUsuario";

        #region Events

        /// <summary>
        /// Maneja el evento OnInit.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                if (Session[Constants.cCulture] != null)
                {
                    CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                    Thread.CurrentThread.CurrentCulture = ci;
                    Thread.CurrentThread.CurrentUICulture = ci;

                    ScriptManager.RegisterStartupScript(PanelPorDefecto, PanelPorDefecto.GetType(), "executeLanguajeSelection", Format("<script type='text/javascript'>SetLanguaje('{0}');</script>", ci.Name), false);
                }

                DoesUserLoggedIn();

                btnLang.InnerHtml = Thread.CurrentThread.CurrentUICulture.Name.Equals("es-CR") ? Convert.ToString(GetLocalResourceObject("SpanishLang")) : Convert.ToString(GetLocalResourceObject("EnglishLang"));
                if (Convert.ToInt16(ConfigurationManager.AppSettings[pruebasDeUsuario].ToString()) == 1)
                {
                    CaptionUserTest.Visible = true;
                }

                else
                {
                    CaptionUserTest.Visible = false;
                }

                base.OnInit(e);
            }

            catch (Exception ex)
            {
                if (ex is PresentationException)
                {
                    MostrarMensaje(uppProgress, TipoMensaje.Error, ex.Message);
                }

                else
                {
                    PresentationException newEx = new PresentationException(DOLE.HRIS.Shared.ErrorMessages.msjLoadSite, ex);
                    MostrarMensaje(uppProgress, TipoMensaje.Error, newEx.Message);
                }
            }
        }

        /// <summary>
        /// Handles the click event on language selector.
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void LnkChangeLang_Click(object sender, EventArgs e)
        {
            LinkButton lnkSender = (LinkButton)sender;
            string languaje;

            if (lnkSender == lbtnEspanish)
            {
                languaje = "es-CR";
                imgLangSelect.Attributes.Add("src", String.Format("{0}{1}{2}", ResolveUrl("~"), Constants.cImagesPath, Constants.cCRFlag));
            }

            else
            {
                languaje = "en-US";
                imgLangSelect.Attributes.Add("src", String.Format("{0}{1}{2}", ResolveUrl("~"), Constants.cImagesPath, Constants.cUSFlag));
            }

            Session["culture"] = languaje;
            Response.Redirect(Request.Url.PathAndQuery);

            ScriptManager.RegisterStartupScript(PanelPorDefecto, PanelPorDefecto.GetType(), "executeLanguajeSelection", Format("<script type='text/javascript'>SetLanguaje('{0}');</script>", languaje), false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Proptery show or hide the non authorized message
        /// </summary>
        public bool NonAuthorizedMessageVisible { get; set; }

        /// <summary>
        /// Proptery show or hide the main panel
        /// </summary>
        public bool MainPanelVisible { get; set; }

        #endregion

        /// <summary>
        /// Check if user is registered, active and has associated divisions to work in the system
        /// </summary>
        /// <returns>True if user is registered and has permissions to access and work in the system, false otherwise</returns>
        private bool DoesUserLoggedIn()
        {
            bool response = false;
            try
            {
                var activeDirectoryName = GetCurrentFullUserName;
                if (!SessionManager.DoesUserLoggedIn)
                {
                    if (ObjIActiveDirectoryBll == null)
                    {
                        ObjIActiveDirectoryBll = Application.GetContainer().Resolve<IActiveDirectoryBll<ActiveDirectorySearchEntity>>();
                    }

                    //Verify account by ActiveDirectory
                    List<ActiveDirectorySearchEntity> result = ObjIActiveDirectoryBll.Search(activeDirectoryName, true);

                    if (!result.Any())
                    {
                        var overrideADNameArray = activeDirectoryName.Replace(@"\", ",").Replace("\b", ",")
                             .Replace("\a", ",").Replace("\f", ",").Replace("\n", ",")
                             .Replace("\r", ",").Replace("\t", ",").Replace("\v", ",")
                             .Replace("\\", ",");

                        var overrideADName = overrideADNameArray.Split(',').LastOrDefault();

                        result = ObjIActiveDirectoryBll.Search(overrideADName, true);
                        result = result.Where(R => R.CompleteUserCode.ToLower().Equals(activeDirectoryName.ToLower())).ToList();
                    }

                    if (result.Any())
                    {
                        if (result != null && result[0].EmailAddress != "")
                        {
                            if (ObjEmployeesBll == null)
                            {
                                ObjEmployeesBll = Application.GetContainer().Resolve<IEmployeesBll<EmployeeEntity>>();
                            }

                            //Get Data from Employees Table by Email.
                            EmployeeEntity currentEmployee = ObjEmployeesBll.ListKeyByEmail(result[0].EmailAddress.ToLower());
                            if (currentEmployee != null)
                            {
                                //Set current user data and storage in session
                                UserEntity currentUser = new UserEntity(0, activeDirectoryName, currentEmployee.EmployeeName, result[0].EmailAddress, currentEmployee.SearchEnabled);

                                response = true;
                                SessionManager.DoesUserLoggedIn = true;
                                SessionManager.AddSessionValue<UserEntity>(SessionKey.UserInformation, currentUser);

                                DivisionByUserEntity divisionByUser = new DivisionByUserEntity { DivisionCode = currentEmployee.DivisionCode, GeographicDivisionCode = currentEmployee.GeographicDivisionCode, CountryID = currentEmployee.CountryId, DivisionName = currentEmployee.DivisionName };
                                List<DivisionByUserEntity> userDivisions = new List<DivisionByUserEntity>
                                {
                                    divisionByUser
                                };

                                SessionManager.AddSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions, userDivisions);
                            }

                            else
                            {
                                MostrarMensaje(uppProgress, TipoMensaje.Error, ErrorMessages.msjPresentationExceptionUsersWithOutMailActiveDirectoryAccount);
                            }
                        }

                        else
                        {
                            MostrarMensaje(uppProgress, TipoMensaje.Error, ErrorMessages.msjPresentationExceptionUsersWithOutMailActiveDirectoryAccount);
                        }
                    }

                    else
                    {
                        MostrarMensaje(uppProgress, TipoMensaje.Error, ErrorMessages.msjPresentationExceptionUsersWithOutDirectoryAccount);
                    }
                }

                else
                {
                    response = true;
                }

                LoadUserConfiguration();
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException || ex is PresentationException)
                {
                    MostrarMensaje(uppProgress, TipoMensaje.Error, ex.Message);
                }

                else
                {
                    MostrarMensaje(uppProgress, TipoMensaje.Error, (new PresentationException(ErrorMessages.msjPresentationExceptionUsersListByActiveDirectoryAccount, ex)).Message);
                }
            }

            return response;
        }

        /// <summary>
        /// Load the user settings on screen
        /// </summary>
        private void LoadUserConfiguration()
        {
            try
            {
                if (SessionManager.DoesUserLoggedIn)
                {
                    lblUserName.Text = SessionManager.GetSessionValue<UserEntity>(SessionKey.UserInformation).UserName;

                    if (SessionManager.DoesKeyExist(SessionKey.WorkingDivision))
                    {
                        lblWorkingDivisionName.Text = Format(Convert.ToString(GetLocalResourceObject("lblWorkingDivisionNameText")), 
                            SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionName);
                    }

                    else
                    {
                        List<DivisionByUserEntity> userDivisions = SessionManager.GetSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions);
                        if (userDivisions.Count.Equals(1))
                        {
                            DivisionByUserEntity workinDivision = userDivisions.FirstOrDefault();
                            lblWorkingDivisionName.Text = Format(Convert.ToString(GetLocalResourceObject("lblWorkingDivisionNameText")), workinDivision.DivisionName);
                            SessionManager.AddSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision, workinDivision);
                        }

                        else
                        {
                            SessionManager.RemoveKey(SessionKey.WorkingDivision);
                            lblWorkingDivisionName.Text = Empty;
                        }
                    }
                }

                else
                {
                    lblUserName.Text = Empty;
                    lblWorkingDivisionName.Text = Empty;
                }
            }

            catch (Exception ex)
            {
                if (ex is DataAccessException || ex is BusinessException || ex is PresentationException)
                {
                    MostrarMensaje(uppProgress, TipoMensaje.Error, ex.Message);
                }

                else
                {
                    MostrarMensaje(uppProgress, TipoMensaje.Error, (new PresentationException(ErrorMessages.msjPresentationExceptionLoadingUserConfiguration, ex)).Message);
                }
            }
        }
    }
}