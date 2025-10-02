using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace FixedAssets.Shared
{
    internal static class DataTransferObject
    {
        internal static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dataTable = new DataTable();

            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                DataColumn dataColumn = new DataColumn(propertyInfo.Name)
                {
                    DataType = propertyInfo.PropertyType
                };

                dataTable.Columns.Add(dataColumn);
            }

            dataTable.AcceptChanges();

            foreach (T item in list)
            {
                DataRow newRow = dataTable.NewRow();

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    newRow[propertyInfo.Name] = propertyInfo.GetValue(item, null);
                }

                dataTable.Rows.Add(newRow);
            }

            dataTable.AcceptChanges();

            return dataTable;
        }
    }
}