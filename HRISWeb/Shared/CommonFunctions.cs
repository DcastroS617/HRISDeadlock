using DOLE.HRIS.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HRISWeb.Shared
{
    public static class CommonFunctions
    {       
        
        /// <summary>
        /// Validate if the default option is selected in the combo (or empty)
        /// </summary>
        /// <param name="cboDropDown">Dropdown list to validate</param>
        /// <returns>
        /// True if the selection value is selected in the selection list.
        /// False - others
        /// </returns>
        public static bool ValidateSelectedOption(DropDownList cboDropDown)
        {
            return (string.IsNullOrEmpty(cboDropDown.SelectedValue) || cboDropDown.SelectedItem.Text.Trim() == Messages.SelectItemText);
        }

        /// <summary>
        /// Set selected value
        /// </summary>
        /// <param name="cboDropDown">Dropdown list to add option</param>
        public static void SetSelectedOption(DropDownList cboDropDown)
        {
            if (cboDropDown.Items.FindByText(Messages.SelectItemText) == null)
            {
                cboDropDown.Items.Insert(0, new ListItem(Messages.SelectItemText, Messages.SelectItemValue));
            }
        }

        /// <summary>
        /// Find the current year's index on a list with years
        /// </summary>
        /// <param name="cboDropDown">dropdown to find</param> 
        public static void SetIndexCurrentYear(DropDownList cboDropDown)
        {
            if (cboDropDown.Items.FindByValue(Convert.ToString(DateTime.Now.Year)) != null)
            {
                cboDropDown.SelectedValue = Convert.ToString(DateTime.Now.Year);
            }

            else if (cboDropDown.Items.FindByText(Messages.SelectItemText) != null)
            {
                cboDropDown.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Switch and get the sort direction order for the sort expression
        /// </summary>
        /// <param name="pageId">Id of the page</param>
        /// <param name="controlId">Id of the control</param>
        /// <param name="sortExpression">Sort expression</param>
        public static string SwitchGetSortDirection(string pageId, string controlId, string sortExpression)
        {
            string sortConfigurationId = string.Format("{0}-{1}-SortConfiguration", pageId, controlId);
            string sortDirection = "ASC";
            
            if (HttpContext.Current.Session[sortConfigurationId] != null)
            {
                Tuple<string, string> sortConfiguration = (Tuple<string, string>)HttpContext.Current.Session[sortConfigurationId];
                if (sortConfiguration.Item1 == sortExpression)
                {
                    sortDirection = sortConfiguration.Item2 == "ASC" ? "DESC" : "ASC";
                }
            }

            HttpContext.Current.Session[sortConfigurationId] = new Tuple<string, string>(sortExpression, sortDirection);

            return sortDirection;
        }

        /// <summary>
        /// Switch and get the sort direction order for the sort expression
        /// </summary>
        /// <param name="pageId">Id of the page</param>
        /// <param name="controlId">Id of the control</param>
        /// <param name="sortExpression">Sort expression</param>
        public static void SwitchSetSortDirection(string pageId, string controlId, string sortExpression)
        {
            string sortConfigurationId = string.Format("{0}-{1}-SortConfiguration", pageId, controlId);
            string sortDirection = "ASC";
           
            if (HttpContext.Current.Session[sortConfigurationId] != null)
            {
                Tuple<string, string> sortConfiguration = (Tuple<string, string>)HttpContext.Current.Session[sortConfigurationId];
                if (sortConfiguration.Item1 == sortExpression)
                {
                    sortDirection = sortConfiguration.Item2 == "ASC" ? "DESC" : "ASC";
                }
            }

            HttpContext.Current.Session[sortConfigurationId] = new Tuple<string, string>(sortExpression, sortDirection);
            
        }

        /// <summary>
        /// Get the style of the icon to show for the sort expression
        /// </summary>
        /// <param name="pageId">Id of the page</param>
        /// <param name="controlId">Id of the control</param>
        /// <param name="sortExpression">Sort expression</param>
        public static string GetSortDirectionStyle(string pageId, string controlId, string sortExpression)
        {
            string sortConfigurationId = string.Format("{0}-{1}-SortConfiguration", pageId, controlId);
            string sortIcon = "fa-sort";
            
            if (HttpContext.Current.Session[sortConfigurationId] != null)
            {
                Tuple<string, string> sortConfiguration = (Tuple<string, string>)HttpContext.Current.Session[sortConfigurationId];
                if (sortConfiguration.Item1 == sortExpression)
                {
                    sortIcon = sortConfiguration.Item2 == "ASC" ? "fa-sort-asc" : "fa-sort-desc";
                }
            }

            return sortIcon;
        }

        /// <summary>
        /// Get the sort expression
        /// </summary>
        /// <param name="pageId">Id of the page</param>
        /// <param name="controlId">Id of the control</param>
        /// <param name="sortExpression">Sort expression</param>
        public static string GetSortDirection(string pageId, string controlId)
        {
            string sortConfigurationId = string.Format("{0}-{1}-SortConfiguration", pageId, controlId);
            string sortDirection = null;
           
            if (HttpContext.Current.Session[sortConfigurationId] != null)
            {
                Tuple<string, string> sortConfiguration = (Tuple<string, string>)HttpContext.Current.Session[sortConfigurationId];
                sortDirection = sortConfiguration.Item2;               
            }

            return sortDirection;
        }

        /// <summary>
        /// Get the sort expression
        /// </summary>
        /// <param name="pageId">Id of the page</param>
        /// <param name="controlId">Id of the control</param>
        /// <param name="sortExpression">Sort expression</param>
        public static string GetSortExpression(string pageId, string controlId)
        {
            string sortConfigurationId = string.Format("{0}-{1}-SortConfiguration", pageId, controlId);
            string sortExpression = null;
           
            if (HttpContext.Current.Session[sortConfigurationId] != null)
            {
                Tuple<string, string> sortConfiguration = (Tuple<string, string>)HttpContext.Current.Session[sortConfigurationId];
                sortExpression = sortConfiguration.Item1;                
            }

            return sortExpression;
        }

        /// <summary>
        /// Reset the sorter config object
        /// </summary>
        /// <param name="pageId">Id of the page</param>
        /// <param name="controlId">Id of the control</param>
        public static void ResetSortDirection(string pageId, string controlId)
        {
            string sortConfigurationId = string.Format("{0}-{1}-SortConfiguration", pageId, controlId);            
            
            if (HttpContext.Current.Session[sortConfigurationId] != null)
            {
                HttpContext.Current.Session.Remove(sortConfigurationId);
            }
        }
    }
}