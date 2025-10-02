using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRISWeb.Shared
{
    /// <summary>
    /// Class to configure filter search
    /// </summary>
    [Serializable]
    public class FilterParameters
    {


        private string mMethodSource;
        /// <summary>
        /// Method source
        /// </summary>
        public string MethodSource
        {
            get { return mMethodSource; }
            set { mMethodSource = value; }
        }

        private List<ColumnFilterDefinition> mListColumnFilter;
        /// <summary>
        /// 
        /// </summary>
        public List<ColumnFilterDefinition> ListColumnFilter
        {
            get { return mListColumnFilter; }
            set { mListColumnFilter = value; }
        }

        private List<ColumnGridDefinition> mListColumnGrid;
        /// <summary>
        /// 
        /// </summary>
        public List<ColumnGridDefinition> ListColumnGrid
        {
            get { return mListColumnGrid; }
            set { mListColumnGrid = value; }
        }
    }

    /// <summary>
    ///  Class column definition
    /// </summary>
    [Serializable]
    public class ColumnFilterDefinition
    {
        private string mLabelText;
        /// <summary>
        /// Label text
        /// </summary>
        public string LabelText
        {
            get { return mLabelText; }
            set { mLabelText = value; }
        }

        private string mControlID;
        /// <summary>
        /// Label id
        /// </summary>
        public string ControlID
        {
            get { return mControlID; }
            set { mControlID = value; }
        }


        /// <summary>
        /// Enum for types control to configure the controls in the filter
        /// </summary>
        public enum ETypeControl : byte
        {
            eTextBox = 0
            , eComboBox = 1
            , eCheckBox = 2 
        }

        private ETypeControl mTypeControl;
        /// <summary>
        /// Type control
        /// </summary>
        public ETypeControl TypeControl
        {
            get { return mTypeControl; }
            set { mTypeControl = value; }
        }

        private string mValueField;
        /// <summary>
        /// Value field
        /// </summary>
        public string ValueField
        {
            get { return mValueField; }
            set { mValueField = value; }
        }

        private string mTextField;
        /// <summary>
        /// Text field
        /// </summary>
        public string TextField
        {
            get { return mTextField; }
            set { mTextField = value; }
        }

        private bool mRequiredSearch;
        /// <summary>
        /// If field is required to search
        /// </summary>
        public bool RequiredSearch
        {
            get { return mRequiredSearch; }
            set { mRequiredSearch = value; }
        }
        
        private string mBusinessLogicName;
        /// <summary>
        /// Business layer class name to load data
        /// </summary>
        public string BusinessLogicName
        {
            get { return mBusinessLogicName; }
            set { mBusinessLogicName = value; }
        }

        private string mNameMethodList;
        /// <summary>
        /// Name of method to get list
        /// </summary>
        public string NameMethodList
        {
            get { return mNameMethodList; }
            set { mNameMethodList = value; }
        }

        private bool mRequiredHandler;
        /// <summary>
        /// Indicates whether a handler is required
        /// </summary>
        public bool RequiredHandler
        {
            get { return mRequiredHandler; }
            set { mRequiredHandler = value; }
        }


        private List<ParameterQueryDefinition> mParameterDefinition;
        /// <summary>
        /// 
        /// </summary>
        public List<ParameterQueryDefinition> ParameterDefinition
        {
            get { return mParameterDefinition; }
            set { mParameterDefinition = value; }
        }
    }

    /// <summary>
    /// Column grid definition
    /// </summary>
    [Serializable]
    public class ColumnGridDefinition
    {

        private string mHeaderName;
        /// <summary>
        /// Header Name
        /// </summary>
        public string HeaderName
        {
            get { return mHeaderName; }
            set { mHeaderName = value; }
        }

        private string mKey;
        /// <summary>
        /// Key in grid
        /// </summary>
        public string Key
        {
            get { return mKey; }
            set { mKey = value; }
        }

        private bool mReturnSelectedValue;
        /// <summary>
        /// Indicator
        /// </summary>
        public bool ReturnSelectedValue
        {
            get { return mReturnSelectedValue; }
            set { mReturnSelectedValue = value; }
        }

    }

    /// <summary>
    /// Defined a parameter and values
    /// </summary>
    [Serializable]
    public class ParameterQueryDefinition
    {
        /// <summary>
        /// Enum contains a type of values in parameter
        /// </summary>
        public enum EParameterValueType : int
        {
            eConstant = 0
            , eExpresion = 1
            , eControlDepend = 2 
        }

        private EParameterValueType mValueType;
        /// <summary>
        /// Type control
        /// </summary>
        public EParameterValueType ValueType
        {
            get { return mValueType; }
            set { mValueType = value; }
        }

        private string mValueDefault;
        /// <summary>
        /// Value default - Constant
        /// </summary>
        public string ValueDefault
        {
            get { return mValueDefault; }
            set { mValueDefault = value; }
        }

        private string mParameterName;
        /// <summary>
        /// Parameter name
        /// </summary>
        public string ParameterName
        {
            get { return mParameterName; }
            set { mParameterName = value; }
        }

        private string mFieldIDDepend;
        /// <summary>
        /// Field ID
        /// </summary>
        public string FieldIDDepend
        {
            get { return mFieldIDDepend; }
            set { mFieldIDDepend = value; }
        }       
    }
}