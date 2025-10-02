using DOLE.HRIS.Application.Business;
using DOLE.HRIS.Exceptions;
using DOLE.HRIS.Shared;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace HRISWeb.Reports
{
    public partial class ReportsMenu : System.Web.UI.Page
    {
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
                    TreeView reportMenu = new TreeView();
                    CargarMenuControlReportes(CasbinBll.ObtenerMenuPorRoles(new List<string> { "HRIS-Reportes" }, Thread.CurrentThread.CurrentUICulture.Name), reportMenu);
                    string jsonTreeViewData = SerializeTreeView(reportMenu);
                    hdfJsonTreeView.Value = jsonTreeViewData;
                }

                //this line allow the iframe to resize when onload occurs
                frmReporte.Attributes.Add("onload", "setTimeout(function () { SetFrameHeight(); }, 150);");
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
        /// Handles the btnReport click event
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void BtnReport_Click(object sender, EventArgs e)
        {
            try
            {
                string eventArgument = Request["__EVENTARGUMENT"];
                string[] eventArguments = eventArgument.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                lblReportTitle.InnerHtml = eventArguments[0];
                frmReporte.Attributes["src"] = Uri.EscapeUriString(Utility.Base64Decode(eventArguments[1]));
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

        #region Private methods

        /// <summary>
        /// Serialize the treeview into json
        /// </summary>
        /// <param name="treeView">Treeview to serialize</param>
        /// <returns>Json string</returns>
        private string SerializeTreeView(TreeView treeView)
        {
            return SerializeTreeViewNodes(treeView.Nodes);
        }

        /// <summary>
        /// Serialize the treeNodeCollection into json
        /// </summary>
        /// <param name="treeNodes">TreeNodeCollection to serialize</param>
        /// <returns>Json string</returns>
        private string SerializeTreeViewNodes(TreeNodeCollection treeNodes)
        {
            string nodes = "";

            foreach (TreeNode treeNode in treeNodes)
            {
                if (treeNode.ChildNodes.Count > 0)
                    nodes += "," + SerializeTreeViewItem(treeNode);
                else
                    nodes += "," + SerializeTreeViewLeaf(treeNode);
            }

            return string.Format("[{0}]", nodes.Substring(1));
        }

        /// <summary>
        /// Serialize the node as an item into json
        /// </summary>
        /// <param name="treeNode">TreeNode to serialize</param>
        /// <returns>Json string</returns>
        private string SerializeTreeViewItem(TreeNode treeNode)
        {
            string node =
                @"{{
                    ""text"": ""{0}"",
                    ""href"": """",
                    ""icon"": ""null"",
                    ""tags"": [""{1}""],
                    ""nodes"":{2}
                }}";

            return string.Format(node, treeNode.Text, treeNode. ChildNodes.Count, SerializeTreeViewNodes(treeNode.ChildNodes));
        }

        /// <summary>
        /// Serialize the node as a leaf into json
        /// </summary>
        /// <param name="treeNode">TreeNode to serialize</param>
        /// <returns>Json string</returns>
        private string SerializeTreeViewLeaf(TreeNode treeNode)
        {
            string node =
                @"{{
                    ""text"": ""{0}"",
                    ""href"": ""{1}""
                }}";

            string usuario = HttpContext.Current.User.Identity.Name;
            var paramDivision = "__paramDivision";
            var paramUsuario = "__paramUsuario";

            int division = SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode;

            treeNode.Value = treeNode.Value.Replace(paramDivision, division.ToString());
            treeNode.Value = treeNode.Value.Replace(paramUsuario, usuario);

            return string.Format(node, treeNode.Text, Utility.Base64Encode(treeNode.Value));
        }

        /// <summary>
        /// Carga menu de reportes
        /// </summary>
        /// <param name="dtMenus"></param>
        /// <param name="menuTreeView"></param>
        private void CargarMenuControlReportes(DataTable dtMenus, TreeView menuTreeView)
        {
            foreach (DataRowView item in new DataView(dtMenus, "url = '' AND LEN(GlobalOrder) = 3", "", DataViewRowState.CurrentRows))
            {
                TreeNode treeNode = new TreeNode(item.Row[3].ToString());
                treeNode.Value = item.Row[3].ToString();
                string text = "";
                string arg = Page.User.Identity.Name.ToString().Replace("\\", "&#92");
                text = string.Format(item.Row[1].ToString(), arg);
                treeNode.NavigateUrl = text;
                menuTreeView.Nodes.Add(treeNode);
                CargarSubMenuControlReporte(dtMenus, treeNode, item.Row[0].ToString());
            }
        }

        /// <summary>
        /// Carga submenu de reportes
        /// </summary>
        /// <param name="dtMenus"></param>
        /// <param name="parentMenu"></param>
        /// <param name="parentId"></param>
        private void CargarSubMenuControlReporte(DataTable dtMenus, TreeNode parentMenu, string parentId)
        {
            var dvMenus = new DataView(dtMenus, "GlobalOrder like '" + parentId + "%' AND GlobalOrder <>'" + parentId + "'", "", DataViewRowState.CurrentRows);
            foreach (DataRowView item in dvMenus)
            {
                TreeNode treeNode = new TreeNode(item.Row[3].ToString());
                string text = "";
                string arg = Page.User.Identity.Name.ToString().Replace("\\", "&#92");
                text = string.Format(item.Row[1].ToString(), arg);
                treeNode.Value = text;
                if (!FindItem(parentMenu.ChildNodes, treeNode))
                {
                    parentMenu.ChildNodes.Add(treeNode);
                    CargarSubMenuControlReporte(dtMenus, treeNode, item.Row[0].ToString());
                }
            }
        }

        /// <summary>
        /// Encuentra un item si existe de la lista
        /// </summary>
        /// <param name="items"></param>
        /// <param name="parentMenu"></param>
        /// <returns></returns>
        private static Boolean FindItem(TreeNodeCollection parentMenu, TreeNode treeNode)
        {
            Boolean find = false;
            if (parentMenu == null) return find;

            foreach (TreeNode item in parentMenu)
            {
                if (item.Text == treeNode.Text)
                {
                    find = true;
                    if (find) break;
                }
                else
                {
                    find = FindItem(item.ChildNodes, treeNode);
                    if (find) break;
                }
            }
            return find;
        }

        #endregion

    }  
}