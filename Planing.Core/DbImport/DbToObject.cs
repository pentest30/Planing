using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Planing.Core.DbImport
{
    public static class DbToObject
    {
        public static List<T> GetListFromTable<T>(DataTable table) where T : new()
        {
            return (from DataRow row in table.Rows select FromDataRow<T>(row)).AsParallel().ToList();
        }

        public static T FromDataRow<T>(DataRow row) where T : new()
        {
            var item = new T();
            SetItemFormRow(item, row);
            return item;
        }
        static void SetItemFormRow<T>(T item, DataRow row) where T : new()
        {

            var properties = item.GetType().GetProperties();
            Parallel.ForEach(properties, (propertyInfo) =>
            {
                try
                {
                    var attr = ((DbColumnAttribute)propertyInfo.GetCustomAttributes(false).FirstOrDefault());
                    if (attr == null || row[attr.Name] == DBNull.Value) return;
                    try
                    {
                        var varType = propertyInfo.PropertyType;
                        propertyInfo.SetValue(item, Convert.ChangeType(row[attr.Name], varType), null);
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception exception)
                {
                    
                    //MessageBox.Show(exception.Message);
                }
            });

        }

    }

    internal class DbColumnAttribute:Attribute
    {
        public DbColumnAttribute(string name)
        {
            Name = name;
        }
        public String Name { get; set; }

    }
}
