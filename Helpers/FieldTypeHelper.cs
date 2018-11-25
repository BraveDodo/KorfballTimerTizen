using KorfballTimerTizen.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace KorfballTimerTizen.Helpers
{
    public class FieldTypeHelper
    {

        public void InsertFieldType(FieldType fieldsize)
        {
            using (var dbConn = SQLiteHelper.GetConnection())
            {
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Insert(fieldsize);
                    });
                }
            }
        }
    }
}