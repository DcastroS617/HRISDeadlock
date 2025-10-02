using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HRISWeb
{
    public static class ExtensionCboSelected
    {

        //autoayuda para poder seleccionar en el combo
        public static void SetSelectedValueDole(this DropDownList ddl, string value)
        {
            try
            {
                ListItem listItem = ddl.Items.FindByValue(value?.Trim());
                ddl.ClearSelection();
                if (listItem != null)
                {

                    ddl.Items.FindByValue(value?.Trim()).Selected = true;
                }

            }
            catch (Exception)
            {

                ddl.ClearSelection();
            }
        }


        public static void SetSelectedValueDole(this DropDownList ddl, int? value)
        {
            try
            {
                ListItem listItem = ddl.Items.FindByValue(value?.ToString().Trim());
                    ddl.ClearSelection();
                if (listItem != null)
                {

                    ddl.Items.FindByValue(value?.ToString().Trim()).Selected=true;
                }

            }
            catch (Exception)
            {

                ddl.ClearSelection();
            }
        }

    }
}