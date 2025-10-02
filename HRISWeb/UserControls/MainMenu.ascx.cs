using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web.UI;
using static System.String;

namespace HRISWeb.UserControls
{
    /// <summary>
    /// Control para el menú principal de la aplicación.
    /// Este control requiere el uso de boostrap 3.
    /// </summary>
    public partial class MainMenu : System.Web.UI.UserControl
    {
        #region Events

        /// <summary>
        ///  Init control event
        /// </summary>
        /// <param name="e">Contains the event data</param>
        protected override void OnInit(EventArgs e)
        {
            if (Session[Constants.cCulture] != null)
            {
                CultureInfo ci = new CultureInfo(Session[Constants.cCulture].ToString());
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
        }

        /// <summary>
        ///  Load control event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lbtnChangeWorkingDivision.ToolTip = GetLocalResourceObject("lbtnChangeWorkingDivision.ToolTip").ToString();
        }

        /// <summary>
        /// Handles the click event of the button help.
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void LnkViewHelp_Click(object sender, EventArgs e)
        {
            try
            {
                string pageName = Request.Url.Segments[Request.Url.Segments.Length - 1];

                if (pageName.Equals("Default.aspx"))
                    return;

                if (pageName.Equals("ReportTraining.aspx"))
                {
                    string[] parameters = Request.Url.Query.Split('&');
                    string codeReport = parameters[0].Replace("?code=", "") + ".";

                    pageName = pageName.Replace(".", codeReport);
                }

                if (pageName.Equals("TrainingLogbooks.aspx"))
                {
                    string[] parameters = Request.Url.Query.Split('&');
                    string typeLogbook = parameters[0].Replace("?type=", "");

                    string suffix = typeLogbook.Equals("H") ? "History." : "New.";

                    pageName = pageName.Replace(".", suffix);
                }

                string helpPageName = string.Format("{0}.help.{1}.htm", pageName, Thread.CurrentThread.CurrentCulture.ToString());
                string folder = string.Empty;

                for (int i = 0; i <= Request.Url.Segments.Length - 2; i++)
                {
                    folder = string.Format("{0}{1}", folder, Request.Url.Segments[i]);
                }

                string helpPageUrl = string.Format("{0}Help/{1}", folder, helpPageName);

                //Open Dialog
                ScriptManager.RegisterStartupScript(spnHelp, spnHelp.GetType(), "OpenHelpDialog" + Guid.NewGuid(), String.Format("<script type='text/javascript'>OpenHelpDialog('{0}');</script>", helpPageUrl), false);
            }

            catch (Exception)
            {
                PresentationException exception = new PresentationException(Messages.msjHelpError);
                MensajeriaHelper.MostrarMensaje(udpMenu,
                    TipoMensaje.Error,
                    exception.Message);
            }
        }

        /// <summary>
        /// Handles the click event of the ChangeWorkingDivision button.
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void LbtnChangeWorkingDivision_Click(object sender, EventArgs e)
        {
            if (SessionManager.DoesKeyExist(SessionKey.WorkingDivision))
            {
                SessionManager.RemoveKey(SessionKey.WorkingDivision);
            }

            if (!Context.Request.Url.AbsoluteUri.ToLower().Contains("/default.aspx"))
            {
                Context.Response.Redirect(Format("~/default.aspx?ReturnUrl={0}", Context.Request.Url.PathAndQuery), false);
            }
            else
            {
                Context.Response.Redirect("~/default.aspx", false);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set menu data
        /// </summary>
        /// <param name="dsMenuData">Data set menu</param>
        public void SetMenuData(DataTable dsMenuData)
        {
            ulMenuOptions.Controls.Clear();
            string menu = BuildMenuOptions(dsMenuData);
            ulMenuOptions.InnerHtml = menu;
        }

        /// <summary>
        /// Set menu data
        /// </summary>
        /// <param name="dsMenuData">Data set menu</param>
        public void SetMenuDataStatic(string Lang)
        {
            if (Lang.Equals("ES"))
            {
                ulMenuOptions.InnerHtml = @"
                        <li> <a href='#' class=""dropdown-toggle"" data-toggle='dropdown'>Responsabilidad Social<b class='caret'></b></a>
                            <ul class='dropdown-menu multi-level'>
                                <li><a href='/SocialResponsability/StartSurvey.aspx' > Ficha socioeconómica</a></li>
                                <li><a href='/SocialResponsability/SynchronizeSurveys.aspx' > Sincronizar Ficha socioeconómica</a></li>
                            </ul>
                        </li>";
            }

            else
            {
                ulMenuOptions.InnerHtml = @"
                        <li> <a href='#' class=""dropdown-toggle"" data-toggle='dropdown'>Responsabilidad Social<b class='caret'></b></a>
                            <ul class='dropdown-menu multi-level'>
                                <li><a href='/SocialResponsability/StartSurvey.aspx' > Socio-economic card</a></li>
                                <li><a href='/SocialResponsability/SynchronizeSurveys.aspx' > Synchronize Socio-economic card</a></li>
                            </ul>
                        </li>";
            }

        }

        /// <summary>
        /// Build a options menu
        /// </summary>
        /// <returns>String with all allowed options by user</returns>
        public static string BuildMenuOptions(DataTable dsMenuOptions)
        {
            SegMenu root = new SegMenu("/", "-1");

            DataTable dtMenus = dsMenuOptions;
            DataView dvMenus = new DataView(dtMenus, "url = '' OR LEN(GlobalOrder) = 3", "", DataViewRowState.CurrentRows);

            int COLMenuId = 0;
            int COLMenuURL = 1;
            int COLMenuName = 3;

            foreach (DataRowView oRow in dvMenus)
            {
                string menuId = oRow.Row[COLMenuId].ToString();
                string menuName = oRow.Row[COLMenuName].ToString();
                string menuURL = oRow.Row[COLMenuURL].ToString();

                SegMenuItem menuNode = null;

                //Determinar si este menu tiene hijos que sean padres
                menuNode = new SegMenuItem(menuName, menuId, menuURL);

                if (menuNode != null)
                {
                    if (!FindItem(root.ChildItems, menuNode))
                    {
                        root.ChildItems.Add(menuNode);
                        if (FindChilds(dtMenus, menuNode))
                            menuNode.IsRootOption = true;
                    }
                }
            }

            return root.GetMenuInnerHtml();
        }

        /// <summary>
        /// Encuentra un item si existe de la lista
        /// </summary>
        /// <param name="items"></param>
        /// <param name="parentMenu"></param>
        /// <returns></returns>
        private static Boolean FindItem(List<SegMenuItem> items, SegMenuItem parentMenu)
        {
            Boolean find = false;
            if (items == null) return find;

            foreach (var item in items)
            {
                if (item.Id == parentMenu.Id)
                {
                    find = true;
                    if (find) break;
                }
                else
                {
                    find = FindItem(item.ChildItems, parentMenu);
                    if (find) break;
                }
            }
            return find;
        }

        /// <summary>
        /// Find childs
        /// </summary>
        /// <param name="dtTable">Table with the menu structure</param>
        /// <param name="parentMenu">Parent menu item</param>
        private static Boolean FindChilds(DataTable dtTable, SegMenuItem parentMenu)
        {
            Boolean childs = false;
            DataView dvMenus = new DataView(dtTable, "GlobalOrder like '" + parentMenu.Id + "%' AND GlobalOrder <>'" + parentMenu.Id + "'", "", DataViewRowState.CurrentRows);

            int COLMenuId = 0;
            int COLMenuURL = 1;
            int COLMenuName = 3;

            foreach (DataRowView oRow in dvMenus)
            {
                childs = true;
                string menuId = oRow.Row[COLMenuId].ToString();
                string menuName = oRow.Row[COLMenuName].ToString();
                string menuURL = oRow.Row[COLMenuURL].ToString();

                SegMenuItem menuNode = null;
                menuNode = new SegMenuItem(menuName, menuId, menuURL);

                if (menuNode != null && !FindItem(parentMenu.ChildItems, menuNode))
                {
                    parentMenu.ChildItems.Add(menuNode);
                    FindChilds(dtTable, menuNode);
                }
            }

            return childs;
        }

        #endregion

    }

    #region Menu Utils Class

    /// <summary>
    /// Menu class container.
    /// </summary>
    public class SegMenu
    {
        /// <summary>
        /// Get or set name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set menu identifier.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Get or set menu item list.
        /// </summary>
        public List<SegMenuItem> ChildItems { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Menu name.</param>
        /// <param name="id">Menu identifier.</param>
        public SegMenu(string name, string id)
        {
            Name = name;
            ID = id;
            ChildItems = new List<SegMenuItem>();
        }

        /// <summary>
        /// Get the html definition of this menu and submenu items.
        /// </summary>
        /// <returns>Html definition of string.</returns>
        public string GetMenuInnerHtml()
        {
            /// Get html definition for item
            string result = String.Empty;

            foreach (SegMenuItem item in ChildItems)
            {
                result = String.Format("{0}{1}", result, item.InnerHtml());
            }

            return result;
        }
    }

    /// <summary>
    /// Menú item class
    /// </summary>
    public class SegMenuItem
    {
        /// <summary>
        /// Get or set root option
        /// </summary>
        public bool IsRootOption { get; set; }

        /// <summary>
        /// Get or set childs menu items.
        /// </summary>
        public List<SegMenuItem> ChildItems { get; set; }

        /// <summary>
        /// Get or set menu name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set menu identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Get or set menu url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SegMenuItem()
        {
            ChildItems = new List<SegMenuItem>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="menuName">Menu name.</param>
        /// <param name="menuID">Menu identifier.</param>
        /// <param name="menuURL">Menu Url</param>
        public SegMenuItem(string menuName, string menuID, string menuURL)
        {
            ChildItems = new List<SegMenuItem>();
            Id = menuID;
            Url = menuURL;
            Name = menuName;
        }

        /// <summary>
        /// Get the html definition of this menu and sub menu list.
        /// </summary>
        /// <returns>Html definition of string.</returns>
        public string InnerHtml()
        {
            //If the menu has no children, a simple menu is created.
            if (ChildItems == null || ChildItems.Count == 0)
            {
                string result;
                if (Url != null && Url.Trim().Length > 0)
                {
                    result = String.Format("<li><a href = \"{0}\" >{1}</a></li>", Url, Name);
                }

                else
                {
                    result = String.Format("<li><a href = \"#\" >{0}</a></li>", Name);
                }

                return result;
            }

            else
            {
                bool isMulti = false;

                foreach (SegMenuItem item in ChildItems)
                {
                    if (item.IsMultiItem)
                    {
                        isMulti = true;
                        break;
                    }
                }

                StringBuilder builder = new StringBuilder();

                if (!isMulti)
                {
                    if (IsRootOption)
                    {
                        builder.Append(String.Format("<li> <a href = \"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">{0}<b class=\"caret\"></b></a> ", Name));
                        builder.Append("<ul class=\"dropdown-menu\">");
                        builder.Append("{0}");
                        builder.Append("</ul></li>");
                    }

                    else
                    {
                        builder.Append(String.Format("<li class=\"dropdown-submenu\"> <a href = \"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">{0}</a> ", Name));
                        builder.Append("<ul class=\"dropdown-menu\">");
                        builder.Append("{0}");
                        builder.Append("</ul></li>");
                    }
                }

                else
                {
                    if (IsRootOption)
                    {
                        builder.Append(String.Format("<li> <a href = \"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">{0}<b class=\"caret\"></b></a> ", Name));
                        builder.Append("<ul class=\"dropdown-menu multi-level\">");
                        builder.Append("{0}");
                        builder.Append("</ul></li>");
                    }

                    else
                    {
                        builder.Append(String.Format("<li class=\"dropdown-submenu\"> <a href = \"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">{0}</a> ", Name));
                        builder.Append("<ul class=\"dropdown-menu\">");
                        builder.Append("{0}");
                        builder.Append("</ul></li>");
                    }
                }

                string childs = String.Empty;
                foreach (SegMenuItem item in ChildItems)
                {
                    childs = String.Format("{0}{1}", childs, item.InnerHtml());
                }

                return String.Format(builder.ToString(), childs);
            }
        }

        /// <summary>
        /// Determine if the menu has children.
        /// </summary>
        public bool IsMultiItem
        {
            get { return ChildItems != null && ChildItems.Count > 0; }
        }
    }

    #endregion
}