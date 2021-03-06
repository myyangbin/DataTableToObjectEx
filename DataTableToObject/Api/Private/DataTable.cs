﻿namespace DataTableToObjectEx
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;

    public static partial class ObjectExtensions
    {
        private static T ToObject<T>(DataRow row, bool safe)
        {

            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            using (var table = row.Table.Clone())
            {
                table.Locale = CultureInfo.InvariantCulture;
                table.ImportRow(row);
                return ToObject<T>(table, safe).First();
            }
            //if (row == null)
            //{
            //    throw new ArgumentNullException("row");
            //}
            //DataColumn col = null;
            //var table = new DataTable();


            //foreach (DataColumn column in row.Table.Columns)
            //{
            //    col = new DataColumn
            //    {
            //        DataType = column.DataType,
            //        ColumnName = column.ColumnName,
            //        Caption = column.Caption,
            //        ReadOnly = column.ReadOnly,
            //        Unique = column.Unique,
            //        DefaultValue = column.DefaultValue

            //    };
            //    table.Columns.Add(col);
            //    col.Dispose();
            //}

            //table.Locale = CultureInfo.InvariantCulture;
            //table.ImportRow(row);
            //return ToObject<T>(table, safe).First();



        }

        private static IEnumerable<T> ToObject<T>(DataTable table, bool safe)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }

            var names = table.Columns.OfType<DataColumn>().Select(c => c.ColumnName);
            var tree = new NameTree(names);
            var children = names.Count() == 1 ? (IEnumerable<NameNode>)new[] { tree.Root } : tree.Root.Children;

            foreach (DataRow row in table.Rows)
            {
                yield return (T)ToObject(typeof(T), ToDictionary(row), children, safe, false);
            }
        }

        private static IDictionary<string, object> ToDictionary(DataRow row)
        {
            return row.Table.Columns.OfType<DataColumn>()
                .ToDictionary(c => c.ColumnName, c => row[c] == DBNull.Value ? null : row[c]);
        }
    }
}