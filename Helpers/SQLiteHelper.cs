using KorfballTimerTizen.Model;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tizen.Applications;

namespace KorfballTimerTizen.Helpers
{
    public class SQLiteHelper
    {
        const string DatabaseFilename = "KorfballDb.db3";
        static string DatabasePath;
        
        #region ISQLite implementation
        public static SQLiteConnection GetConnection()
        {
            raw.SetProvider(new SQLite3Provider_sqlite3());
            //raw.FreezeProvider(true);
            string dbPath = global::Tizen.Applications.Application.Current.DirectoryInfo.Data;
            DatabasePath = Path.Combine(dbPath, DatabaseFilename);
            if (!File.Exists(DatabasePath))
            {
                FileStream writeStream = new FileStream(DatabasePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            //SQLiteConnection db;
            //try
            {
                SQLiteConnection db = new SQLiteConnection(DatabasePath);
                {
                    return db;
                }
            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    db = null;
            //    {
            //        //var a = db.TeamTypes.ToList();
            //        return db;
            //    }
            //}
        }
        //public string GetLocalFilePath(string filename)
        //{
        //    string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //    return Path.Combine(path, filename);
        //}
        #endregion
    }
}
