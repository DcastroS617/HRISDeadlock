using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HRISWeb.Help
{
    public static class ConvertList
    {
        public static DataTable ToDataTable<T>(List<T> source)
        {
            var props = typeof(T).GetProperties();

            var dt = new DataTable() { TableName = "Table" };
            dt.Columns.AddRange(
                props.Select(p => new DataColumn(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType)).ToArray()
            );

            source.ToList().ForEach(
              i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray())
            );

            return dt;
        }

        public static DataTable ToDataTableGet<T>(this List<T> source)
        {
            var props = typeof(T).GetProperties();

            var dt = new DataTable() { TableName = "Table" };
            dt.Columns.AddRange(
                props.Select(p => new DataColumn(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType)).ToArray()
            );

            source.ToList().ForEach(
              i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray())
            );

            return dt;
        }


    }
}