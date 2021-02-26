using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;


namespace CartRover.Batch.App
{
    
    static class Common
    {
        public static OleDbConnection DBConnection()
        {
            try
            {
                OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Projects\VB.Net\UPWork\UPWork\FBG\DB\FBG.accdb");
                conn.Open();

                return conn;
            }
            catch (Exception ex)
            {
                string errmessage = "Error Opening connection - mCommon.DBConnection - " + ex.Message;
                return null;
            }
        }
    }

}
