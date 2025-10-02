using DOLE.HRIS.Shared.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HRISWeb.GTI
{
    public class TableControl
    {

        /// <summary>
        /// Generates an HTML table and adds it to the provided container.
        /// </summary>
        /// <param name="empleados">List of employees to display</param>
        /// <param name="container">Control container where the table will be added</param>
        public static void GenerarTablaHtml(List<EmployeeGtiReportEntity> empleados, Control container)
        {
            Table table = new Table();
            table.CssClass = "table table-striped table-fixed"; 

            TableRow headerRow = new TableRow();
            
            string[] headers = { "Employee ID", "Last Name", "First Name", "Middle Name", "E-mail Address", "Office Phone", "Birth Date", "Gender", "Hire Date", "Termination Date", "Years Of Service" };

            // Iterates over the headers array to add each header to the table.
            for (int i = 0; i < headers.Length; i++)
            {
                TableCell cell = new TableCell();
                cell.Text = headers[i];

                // Applies a fixed-column class to the first few columns for styling purposes.
                if (i < 4)
                {
                    cell.CssClass = "fixed-column";
                }
                headerRow.Cells.Add(cell);
            }

            // Adds the header row to the table.
            table.Rows.Add(headerRow);

            // Iterates over each employee in the list and creates a row with their data.
            foreach (var empleado in empleados)
            {
                TableRow row = new TableRow();

                // Adds the employee ID (non-editable) and assigns the fixed-column CSS class.
                TableCell cell1 = CreateTextBoxCell(empleado.EmployeeID.ToString(), false);
                cell1.CssClass = "fixed-column";
                row.Cells.Add(cell1);

                // Adds last name (editable) and assigns the fixed-column CSS class.
                TableCell cell2 = CreateTextBoxCell(empleado.LastName, true);
                cell2.CssClass = "fixed-column";
                row.Cells.Add(cell2);

                // Adds first name (editable) and assigns the fixed-column CSS class.
                TableCell cell3 = CreateTextBoxCell(empleado.FirstName, true);
                cell3.CssClass = "fixed-column";
                row.Cells.Add(cell3);

                // Adds middle name (editable) and assigns the fixed-column CSS class.
                TableCell cell4 = CreateTextBoxCell(empleado.MiddleName, true);
                cell4.CssClass = "fixed-column";
                row.Cells.Add(cell4);

                // Adds the rest of the employee details to the row (editable where applicable).
                row.Cells.Add(CreateTextBoxCell(empleado.EmailAddress, true));
                row.Cells.Add(CreateTextBoxCell(empleado.OfficePhone, true));
                row.Cells.Add(CreateTextBoxCell(empleado.BirthDate.ToString("yyyy-MM-dd"), true));
                row.Cells.Add(CreateTextBoxCell(empleado.Gender, true));
                row.Cells.Add(CreateTextBoxCell(empleado.HireDate.ToString("yyyy-MM-dd"), true));
                row.Cells.Add(CreateTextBoxCell(empleado.TerminationDate?.ToString("yyyy-MM-dd") ?? "N/A", true));
                row.Cells.Add(CreateTextBoxCell(empleado.YearsOfService.ToString(), false));

                // Adds the completed row to the table.
                table.Rows.Add(row);
            }

            // Adds the table to the container control.
            container.Controls.Add(table);
        }

        /// <summary>
        /// Creates a table cell containing a TextBox, with the option to make it editable.
        /// </summary>
        /// <param name="text">The text to display in the TextBox</param>
        /// <param name="isEditable">Specifies whether the TextBox is editable</param>
        /// <returns>Returns a TableCell containing the TextBox</returns>
        private static TableCell CreateTextBoxCell(string text, bool isEditable)
        {
            TableCell cell = new TableCell();
            TextBox textBox = new TextBox();
            textBox.Text = text;
            textBox.Enabled = isEditable;// Sets whether the TextBox is editable
            cell.Controls.Add(textBox);// Adds the TextBox to the TableCell
            return cell;
        }
    }
}