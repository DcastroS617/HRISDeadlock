using DOLE.HRIS.Application.Business;
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
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Unity;
using Unity.Web;
using static HRISWeb.Shared.MensajeriaHelper;
using static HRISWeb.Shared.UserHelper;
using static System.String;

namespace HRISWeb
{
    /// <summary>
    /// Master page class
    /// </summary>
    public partial class Main : System.Web.UI.MasterPage
    {
        private IUsersBll<UserEntity> ObjUsersBll { get; set; }

        private IDivisionsByUsersBll<DivisionByUserEntity> ObjDivisionsByUsersBll { get; set; }
        public IActiveDirectoryBll<ActiveDirectorySearchEntity> ObjIActiveDirectoryBll { get; set; }

        private IEmployeesBll<EmployeeEntity> ObjEmployesBll { get; set; }

        private const string webAppIsLocal = "WebAppIsLocal";
        private const string pruebasDeUsuario = "PruebasDeUsuario";

        //public DOLE.Seguridad.Intranet.ControlAcceso ControlAccesoComponent
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings[webAppIsLocal].ToString().Equals("0") ? this.ControlAcceso : null;
        //    }
        //}

        #region Events

        /// <summary>
        /// Maneja el evento Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings[webAppIsLocal].ToString().Equals("1"))
            {
                pnlControAcceso.Controls.Clear();
            }
        }

        /// <summary>
        /// Maneja el evento OnInit.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                UserHelper.InitComponent();

                if (Session[Constants.cCulture] != null)
                {
                    CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                    Thread.CurrentThread.CurrentCulture = ci;
                    Thread.CurrentThread.CurrentUICulture = ci;

                    ScriptManager.RegisterStartupScript(PanelPorDefecto, PanelPorDefecto.GetType(), "executeLanguajeSelection", Format("<script type='text/javascript'>SetLanguaje('{0}');</script>", ci.Name), false);
                }

                else
                {
                    string languaje = "es-CR";
                    Session[Constants.cCulture] = languaje;
                }

                if (DoesUserLoggedIn())
                {
                    if (ConfigurationManager.AppSettings[webAppIsLocal].ToString().Equals("0"))
                    {
                        var userClaims = HttpContext.Current.User as ClaimsPrincipal;
                        var userRoles = UserHelper.GetUserRoles(userClaims);
                        MainMenu.SetMenuData(CasbinBll.ObtenerMenuPorRoles(userRoles, Thread.CurrentThread.CurrentUICulture.Name));
                    }

                    else
                    {
                        MainMenu.SetMenuDataStatic(GetLocalResourceObject("Lang").ToString());
                    }
                }

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

            Session[Constants.cCulture] = languaje;
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
                if (!SessionManager.DoesUserLoggedIn)
                {
                    ObjUsersBll = Application.GetContainer().Resolve<IUsersBll<UserEntity>>();

                    UserEntity currentUser = ObjUsersBll.ListByActiveDirectoryAccount(GetCurrentUserEmail);
                    if (currentUser != null)
                    {
                        if (currentUser.IsActive)
                        {
                            ObjDivisionsByUsersBll = Application.GetContainer().Resolve<IDivisionsByUsersBll<DivisionByUserEntity>>();
                            List<DivisionByUserEntity> userDivisions = ObjDivisionsByUsersBll.ListByUser(currentUser.UserCode);

                            if (userDivisions.Count > 0)
                            {
                                response = true;
                                SessionManager.DoesUserLoggedIn = true;
                                SessionManager.AddSessionValue<UserEntity>(SessionKey.UserInformation, currentUser);
                                SessionManager.AddSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions, userDivisions);
                            }

                            else
                            {
                                MostrarMensaje(uppProgress, TipoMensaje.Validacion, Format(Messages.msjRegisteredUserWithoutAssociatedDivisions, currentUser.UserName));
                            }
                        }

                        else
                        {
                            MostrarMensaje(uppProgress, TipoMensaje.Validacion, Format(Messages.msjInactiveRegisteredUser, currentUser.UserName));
                        }
                    }

                    else
                    {
                        if (!DoesUserOutLoggedIn())
                        {
                            MostrarMensaje(uppProgress, TipoMensaje.Validacion, Format(Messages.msjUnregisteredUser, HttpUtility.JavaScriptStringEncode(GetCurrentFullUserName)));
                        }
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
        /// Check if user is registered en employees to fill de socioeconomic card
        /// </summary>
        /// <returns>True if user is registered and has permissions to access and work in the system, false otherwise</returns>
        private bool DoesUserOutLoggedIn()
        {
            ObjEmployesBll = Application.GetContainer().Resolve<IEmployeesBll<EmployeeEntity>>();
            if (ObjIActiveDirectoryBll == null)
            {
                ObjIActiveDirectoryBll = Application.GetContainer().Resolve<IActiveDirectoryBll<ActiveDirectorySearchEntity>>();
            }

            //Verify account by ActiveDirectory or EMAIL
            List<ActiveDirectorySearchEntity> result = ObjIActiveDirectoryBll.Search(GetCurrentFullUserName, true);
            EmployeeEntity employeeEntity = ObjEmployesBll.ListEmployeeByActiveDirectoryUserAccount(GetCurrentFullUserName, result.FirstOrDefault().EmailAddress);

            if (employeeEntity != null)
            {
                SessionManager.DoesUserLoggedIn = true;
                SessionManager.DoesUserOutLoggedIn = true;
                SessionManager.AddSessionValue<UserEntity>(SessionKey.UserInformation, new UserEntity()
                {
                    ActiveDirectoryUserAccount = GetCurrentFullUserName,
                    UserName = employeeEntity.EmployeeName,
                    IsActive = employeeEntity.CurrentState == "Activo" ? true : false,
                    EmailAddress = String.IsNullOrEmpty(employeeEntity.Email) ? result.FirstOrDefault().EmailAddress : employeeEntity.Email
                });

                List<DivisionByUserEntity> listDivision = new List<DivisionByUserEntity>() {new DivisionByUserEntity()
                {
                    GeographicDivisionCode = employeeEntity.GeographicDivisionCode,
                    DivisionCode = employeeEntity.DivisionCode,
                    CountryID = employeeEntity.Alpha3Code,
                    DivisionName = employeeEntity.DivisionName
                }};

                SessionManager.AddSessionValue<List<DivisionByUserEntity>>(SessionKey.UserDivisions, listDivision);
            }

            return true;
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
                        lblWorkingDivisionName.Text = Format(Convert.ToString(GetLocalResourceObject("lblWorkingDivisionNameText")), SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionName);
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