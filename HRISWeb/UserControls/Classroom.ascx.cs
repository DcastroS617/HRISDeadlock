using DOLE.HRIS.Application.Business.Interfaces;
using DOLE.HRIS.Shared.Entity;
using HRISWeb.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using static DOLE.HRIS.Shared.Entity.HrisEnum;

namespace HRISWeb.UserControls
{
    public partial class Classroom : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// Get or set interface.
        /// </summary>
        public ITrainingCentersBll<TrainingCenterEntity> ObjTrainingCentersBll { get; set; }

        #endregion

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
        /// Handles the load of the page
        /// </summary>
        /// <param name="sender">Refers to the object that invoked the event that fired the event handle</param>
        /// <param name="e">Contains the event data</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            chbSearchEnabled.Attributes.Add("data-on", Convert.ToString(GetLocalResourceObject("Yes")));
            chbSearchEnabled.Attributes.Add("data-off", Convert.ToString(GetLocalResourceObject("No")));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Returns the selected classroom code
        /// </summary>
        /// <returns>Returns the selected classroom code</returns>
        public string ClassroomCode()
        {
            string classroomCode = !string.IsNullOrWhiteSpace(hdfClassroomCodeEdit.Value) ?
                hdfClassroomCodeEdit.Value : txtClassroomCode.Text.Trim();

            return classroomCode;
        }

        /// <summary>
        /// Returns the selected classroom geographic Division Code
        /// </summary>
        /// <returns>Returns the selected classroom geographic Division Code</returns>
        public string ClassroomGeographicDivisionCode()
        {
            string classroomGeographicDivisionCode = !string.IsNullOrWhiteSpace(hdfClassroomGeographicDivisionCodeEdit.Value) ?
                hdfClassroomGeographicDivisionCodeEdit.Value :
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode;

            return classroomGeographicDivisionCode;
        }

        /// <summary>
        /// Return is valid form classroom
        /// </summary>
        /// <returns></returns>
        public bool IsValidClassroom(object sender) {
            bool isValidClassroom = false;

            string classroomCode = ClassroomCode();
            string classroomDescription = txtClassroomDescription.Text.Trim();

            if (string.IsNullOrEmpty(classroomCode))
            {
                string script = " setTimeout(function () {  EnabledRequired('" + txtClassroomCode.ClientID + "'); }, 10);  ";
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("EnabledRequired{0}", Guid.NewGuid()), script, true);

                isValidClassroom = true;
            }

            if (string.IsNullOrEmpty(classroomDescription))
            {
                string script = " setTimeout(function () {  EnabledRequired('" + txtClassroomDescription.ClientID + "'); }, 10);  ";
                ScriptManager.RegisterStartupScript((Control)sender, sender.GetType(), string.Format("EnabledRequired{0}", Guid.NewGuid()), script, true);

                isValidClassroom = true;
            }

            return isValidClassroom;
        }

        /// <summary>
        /// Returns the selected training center id
        /// </summary>
        /// <returns>The selected training center id</returns>
        public string GetTrainingCenterSelectedValue()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboTrainingCenter.SelectedValue) && !"--1".Equals(cboTrainingCenter.SelectedValue))
            {
                selected = cboTrainingCenter.SelectedValue;
            }

            return selected;
        }

        /// <summary>
        /// Returns the selected training center id
        /// </summary>
        /// <returns>The selected training center id</returns>
        public string GetTrainingCenterSelectedText()
        {
            string selected = null;
            if (!string.IsNullOrWhiteSpace(cboTrainingCenter.SelectedValue) && !"--1".Equals(cboTrainingCenter.SelectedValue))
            {
                selected = cboTrainingCenter.SelectedItem.Text;
            }

            return selected;
        }

        /// <summary>
        /// Returns the selected classroom object
        /// </summary>
        /// <returns>Returns the selected classroom object</returns>
        public ClassroomEntity GetClassroom()
        {
            ClassroomEntity classroom = new ClassroomEntity(
                ClassroomGeographicDivisionCode(),
                ClassroomCode(),
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                txtClassroomDescription.Text.Trim(),

                new TrainingCenterEntity(
                        SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode,
                        GetTrainingCenterSelectedValue(),
                        GetTrainingCenterSelectedText()),

                Convert.ToInt32(txtCapacity.Text.Trim()),
                txtComments.Text.Trim(),
                chbSearchEnabled.Checked,
                false,
                UserHelper.GetCurrentFullUserName,
                DateTime.Now);

            return classroom;
        }

        /// <summary>
        /// Loads the training centers data
        /// </summary>
        /// <returns>The training centers data</returns>
        public void LoadTrainingCenters()
        {
            List<TrainingCenterEntity> trainingCenters = ObjTrainingCentersBll.ListByDivision(
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).DivisionCode,
                SessionManager.GetSessionValue<DivisionByUserEntity>(SessionKey.WorkingDivision).GeographicDivisionCode);

            IDictionary<string, string> placeLocations = GetAllValuesAndLocalizatedDescriptions<PlaceLocation>();

            trainingCenters = trainingCenters.Select(R =>
            {
                R.TrainingCenterDescription = $"{R.TrainingCenterDescription} - {placeLocations[R.PlaceLocation.ToString()]}";
                return R;
            }).ToList();

            trainingCenters.Insert(0, new TrainingCenterEntity("", "--1", ""));

            cboTrainingCenter.Enabled = true;
            cboTrainingCenter.DataValueField = "TrainingCenterCode";
            cboTrainingCenter.DataTextField = "TrainingCenterDescription";
            cboTrainingCenter.DataSource = trainingCenters;
            cboTrainingCenter.DataBind();
        }

        #endregion

    }
}