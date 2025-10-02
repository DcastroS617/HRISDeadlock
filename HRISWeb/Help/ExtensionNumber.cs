using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRISWeb
{
    public static class ExtensionNumber
    {

        public static string ToStringNull(this string val)
        {
            return string.IsNullOrEmpty(val) ? null : val;
        }

        public static int? ToInt32Null(this string val)
        {
            return string.IsNullOrEmpty(val) ? (int?)null : Convert.ToInt32(val);
        }

        public static int? ToInt32Null(this object val)
        {
            return val==null ? (int?)null : Convert.ToInt32(val);
        }

        public static int ToInt32(this string val)
        {
            return Convert.ToInt32(val);
        }

        public static int ToInt32(this object val)
        {
            return Convert.ToInt32(val); 
        }

    }
}