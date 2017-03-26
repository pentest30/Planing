using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Planing.Core.Models;
using Planing.Models;

namespace Planing.Core.DbImport
{
    public static class DbAcceess
    {
        #region Properties

        private static readonly OleDbConnection Connexion = new OleDbConnection();
        private static readonly OleDbCommand Cmd = new OleDbCommand();
        private static readonly OleDbDataAdapter Adapter = new OleDbDataAdapter();
        private static DataTable _table = new DataTable();

        #endregion


        #region functions

        static DbAcceess()
        {
            Connexion = new OleDbConnection();
            Cmd = new OleDbCommand();
            _table = new DataTable();
            Adapter = new OleDbDataAdapter();
        }

        public static DataTable DataExcel(string chemin, int index)
        {
            string connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + chemin +
                                ";Extended Properties='Excel 12.0;IMEX=1;'";
            var conn = new OleDbConnection(connection);
            conn.Open();
            var sheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            var sheetName = new List<string>();
            conn.Close();
            if (sheets != null)
            {
                sheetName.AddRange(from DataRow row in sheets.Rows select row["TABLE_NAME"].ToString());
            }
            var strSql = string.Format("SELECT * FROM [{0}]", sheetName[index]);
            var cmd = new OleDbCommand(strSql, conn);
            var dataset = new DataSet();
            var adapter = new OleDbDataAdapter(cmd);
            adapter.Fill(dataset);
            return dataset.Tables[0];
        }

        public static DataTable DataAcceess(string chemin, string password, string command)
        {
            if (Connexion.State == ConnectionState.Open) Connexion.Dispose();
            using (Connexion)
            {
                Connexion.ConnectionString = ConnexionString(chemin, password);
                Cmd.Connection = Connexion;
                Cmd.CommandText = command;
                if (command.Contains("INSERT") || command.Contains("UPDATE"))
                {
                    Connexion.Open();
                    Cmd.ExecuteNonQuery();
                    Cmd.Dispose();
                    return null;
                }
                using (Adapter)
                {
                    Adapter.SelectCommand = Cmd;

                    using (_table = new DataTable())
                    {
                        Adapter.Fill(_table);
                        Cmd.Dispose();
                        return _table;
                    }
                }
            }
        }

        public static IEnumerable<Specialite> GetSpecialites(string chemin, int index)
        {
            return DbToObject.GetListFromTable<Specialite>(DataExcel(chemin, index));
        }

        //static string ConnexionString(string chemin)
        //{
        //    return @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + chemin + ";Extended Properties=" +
        //           "Excel 12.0 Xml;HDR=YES" + ";";
        //}
        private static string ConnexionString(string chemin, string password)
        {
            return @"Provider=Microsoft.ACE.OLEDB.12.0;;Data Source=" + chemin + ";Jet OLEDB:Database Password=" +
                   password;
        }


        public static IEnumerable<ClassRoom> GetClassRooms(string cheminExcel, int p)
        {
            return DbToObject.GetListFromTable<ClassRoom>(DataExcel(cheminExcel, p));
        }

        public static IEnumerable<Teacher> GetTeachers(string cheminExcel, int i)
        {
            return DbToObject.GetListFromTable<Teacher>(DataExcel(cheminExcel, i));
        }

        public static IEnumerable<Course> GetCourses(string cheminExcel, int i)
        {
            return DbToObject.GetListFromTable<Course>(DataExcel(cheminExcel, i));
        }

        public static IEnumerable<Module> GetModule(string cheminExcel, int i)
        {
            return DbToObject.GetListFromTable<Module>(DataExcel(cheminExcel, i));
        }

        public static IEnumerable<TcModel> GetPlaning(string cheminExcel, int i)
        {
            return DbToObject.GetListFromTable<TcModel>(DataExcel(cheminExcel, i));
        }

        public static DataTable ToDictionary(Dictionary<string, List<string>> list, int an, int semestre,
            string specialite, string section, string anneeScolaire, string faculte)
        {
            var result = new DataTable();
            if (list.Count == 0)
                return result;
            result.Columns.Add(new DataColumn {ColumnName = string.Format("Jours")});
            for (int i = 1; i < 7; i++)
            {
                result.Columns.Add(new DataColumn {ColumnName = string.Format("s{0}", i)});
            }

            result.Columns.Add(new DataColumn {ColumnName = string.Format("Annee")});
            result.Columns.Add(new DataColumn {ColumnName = string.Format("Semestre")});
            result.Columns.Add(new DataColumn {ColumnName = string.Format("Section")});
            result.Columns.Add(new DataColumn {ColumnName = string.Format("Specialite")});
            result.Columns.Add(new DataColumn {ColumnName = string.Format("AnneeScolaire")});
            result.Columns.Add(new DataColumn {ColumnName = string.Format("Faculte")});
            //int j = 0;
            foreach (var item in list)
            {

                var row = result.NewRow();
                row[0] = item.Key;

                for (int i = 0; i < 6; i++)
                {
                    row[i + 1] = item.Value[i];

                }
                row[7] = an;
                row[8] = semestre;
                row[9] = section;

                row[10] = specialite;
                row[11] = anneeScolaire;
                row[12] = faculte;
                result.Rows.Add(row);


            }




            return result;
        }

        #endregion
    }
}