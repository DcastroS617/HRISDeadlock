using DOLE.HRIS.Shared;
using System;
using System.Web.UI.WebControls;

namespace HRISWeb.Shared
{
    /// <summary>
    /// Provides utility methods for paging features.
    /// </summary>
    public static class PagerUtil
    {      
        /// <summary>
        /// Gets the current active page for the pager control.
        /// </summary>
        /// <param name="pagerControl">The pager control.</param>
        /// <returns>The active page.</returns>
        public static int GetActivePage(BulletedList pagerControl)
        {
            int page = 1;

            if (pagerControl != null && pagerControl.Items.Count > 0)
            {
                foreach (ListItem item in pagerControl.Items)
                {
                    if ((!string.IsNullOrEmpty(item.Attributes["class"]) && item.Attributes["class"].Contains("active")) || item.Selected)
                    {
                        page = Convert.ToInt32(item.Value);
                        break;
                    }
                }
            }

            return page;
        }

        /// <summary>
        /// Sets the active page (selected page) of the pager control.
        /// </summary>
        /// <param name="pagerControl">The pager control.</param>
        /// <param name="page">The page to set as active.</param>
        public static void SetActivePage(BulletedList pagerControl, int page)
        {
            if (pagerControl != null && pagerControl.Items.Count > 0)
            {
                foreach (ListItem item in pagerControl.Items)
                {
                    int pageNumber = Convert.ToInt32(item.Value);

                    if (pageNumber != page)
                    {
                        if (!string.IsNullOrEmpty(item.Attributes["class"])
                            && item.Attributes["class"].Contains("active"))
                        {
                            item.Attributes["class"] = item.Attributes["class"].Replace("active", string.Empty);
                        }

                        item.Selected = false;
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(item.Attributes["class"])
                            || !item.Attributes["class"].Contains("active"))
                        {
                            item.Attributes["class"] = "active";
                        }

                        item.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Setups the pager control, creating all the neccesary items (pages) inside the control.
        /// </summary>
        /// <param name="pagerControl">The pager control.</param>
        /// <param name="totalRowsCount">The total amount of rows in the collection.</param>
        public static void SetupPager(BulletedList pagerControl, int totalRowsCount)
        {
            SetupPager(pagerControl, totalRowsCount, 1);
        }

        /// <summary>
        /// Setups the pager control, creating all the neccesary items (pages) inside the control.
        /// </summary>
        /// <param name="pagerControl">The pager control.</param>
        /// <param name="totalRowsCount">The total amount of rows in the collection.</param>
        /// <param name="startupPage">The startup page.</param>
        /// <remarks>
        /// ¿Cómo funciona el paginador?
        /// Para listados que tienen pocas páginas (un máximo de 10), el paginador mostrará todas las páginas existentes. Sin embargo, para
        /// listados que tienen muchas páginas (más de 10), el paginador se construirá dinámicamente para no inundar de páginas el listado,
        /// sino más bien mostrar una sección relevante de páginas. Entonces por ejemplo, en un listado con 76 páginas si el usuario se
        /// encuentra en la página 15, el paginador mostrará una configuración similar a la siguiente:
        /// 
        /// « 10 more pages | 11 | 12 | 13 | 14 | 15 | 16 | 17 | 18 | 19 | 20 | 56 more pages »
        /// </remarks>
        public static void SetupPager(BulletedList pagerControl, int totalPages, int startupPage)
        {
            if (startupPage > totalPages)
            {
                startupPage = totalPages;
            }

            pagerControl.Items.Clear();
            if (totalPages > 1)
            {
                int firstPage = Math.Max(1, startupPage - 4 - Math.Max(5 + (startupPage - totalPages), 0));
                int lastPage = Math.Min(totalPages, startupPage + 5 + Math.Min((startupPage - 4 - 1), 0) * -1);

                for (int i = firstPage; i <= lastPage; i++)
                {
                    pagerControl.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                if (pagerControl.Items.Count > 0)
                {
                    pagerControl.Items[startupPage - firstPage].Attributes["class"] = "active";
                    pagerControl.Items[startupPage - firstPage].Selected = true;
                }

                if (firstPage > 1)
                {
                    int countMoreFirstPages = firstPage - 1;
                    ListItem moreFirstPages = new ListItem(string.Format("« {0} {1}", countMoreFirstPages, countMoreFirstPages == 1 ? Messages.strMorePage : Messages.strMorePages), "1");
                    
                    moreFirstPages.Attributes["class"] = "disabled";
                    
                    pagerControl.Items.Insert(0, moreFirstPages);
                }

                if (lastPage < totalPages)
                {
                    int countMoreLastPages = totalPages - lastPage;
                    ListItem moreLastPages = new ListItem(string.Format("{0} {1} »", countMoreLastPages, countMoreLastPages == 1 ? Messages.strMorePage : Messages.strMorePages), totalPages.ToString());
                   
                    moreLastPages.Attributes["class"] = "disabled";
                    
                    pagerControl.Items.Add(moreLastPages);
                }
            }
        }
    }
}